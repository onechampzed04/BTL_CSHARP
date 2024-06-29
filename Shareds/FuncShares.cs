using BTL_2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTL_2.Shareds
{
    public class FuncShares<T> where T : class
    {
        private static DatabaseDataContext dataContext = new DatabaseDataContext();

        public static FuncResult<List<T>> GetAllData()
        {
            FuncResult<List<T>> result = new FuncResult<List<T>>();
            try
            {
                var table = dataContext.GetTable<T>();
                result.Data = table.ToList();
                result.ErrorCode = EnumErrorCode.SUCCESS;
                result.ErrorDesc = string.Format(Constants.getAllData_success, typeof(T).Name);
            }
            catch (Exception ex)
            {
                result.ErrorCode = EnumErrorCode.ERROR;
                result.ErrorDesc = string.Format(Constants.getAllData_failed, typeof(T).Name) + ex.Message;
            }
            return result;
        }

        public static FuncResult<bool> Insert(T t)
        {
            FuncResult<bool> result = new FuncResult<bool>();
            try
            {
                var table = dataContext.GetTable<T>();
                table.InsertOnSubmit(t);
                dataContext.SubmitChanges();
                result.Data = true;
                result.ErrorCode = EnumErrorCode.SUCCESS;
                result.ErrorDesc = string.Format(Constants.insert_success, typeof(T).Name);
            }
            catch (Exception ex)
            {
                result.ErrorCode = EnumErrorCode.ERROR;
                result.ErrorDesc = string.Format(Constants.insert_failed, typeof(T).Name) + ex.Message;
            }
            return result;
        }
        public static FuncResult<bool> Update(T t)
        {
            FuncResult<bool> result = new FuncResult<bool>();
            try
            {
                var table = dataContext.GetTable<T>();

                //Lấy trạng thái gốc của thực thể t từ bảng.
                //Nếu thực thể này tồn tại trong cơ sở dữ liệu,
                //nó sẽ trả về trạng thái gốc của thực thể đó.
                var original = table.GetOriginalEntityState(t);

                if (original != null)
                {
                    //Cập nhật thực thể t trong cơ sở dữ liệu bằng cách
                    //giữ nguyên các giá trị hiện tại.
                    dataContext.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, t);
                    dataContext.SubmitChanges();
                    result.Data = true;
                    result.ErrorCode = EnumErrorCode.SUCCESS;
                    result.ErrorDesc = string.Format(Constants.update_success, typeof(T).Name);
                }
                else
                {
                    result.ErrorCode = EnumErrorCode.ERROR;
                    result.ErrorDesc = string.Format(Constants.update_failed, typeof(T).Name);
                }
            }
            catch (System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException ex)
            {
                result.ErrorCode = EnumErrorCode.ERROR;
                result.ErrorDesc = string.Format(Constants.update_failed, typeof(T).Name) + " - ForeignKeyReferenceAlreadyHasValueException: " + ex.Message;
            }
            catch (Exception ex)
            {
                result.ErrorCode = EnumErrorCode.ERROR;
                result.ErrorDesc = string.Format(Constants.insert_failed, typeof(T).Name) + ex.Message;
            }
            return result;
        }

        public static FuncResult<bool> Delete(T t)
        {
            FuncResult<bool> result = new FuncResult<bool>();
            try
            {
                var table = dataContext.GetTable<T>();
                table.DeleteOnSubmit(t);
                dataContext.SubmitChanges();
                result.Data = true;
                result.ErrorCode = EnumErrorCode.SUCCESS;
                result.ErrorDesc = string.Format(Constants.delete_success, typeof(T).Name);
            }
            catch (Exception ex)
            {
                result.ErrorCode = EnumErrorCode.ERROR;
                result.ErrorDesc = string.Format(Constants.delete_failed, typeof(T).Name) + ex.Message;
            }
            return result;
        }

        public static FuncResult<bool> ShowConfirmationMessage()
        {
            FuncResult<bool> result = new FuncResult<bool>();
            try
            {
                var dialogResult = MessageBox.Show(string.Format(Constants.delete_confirmation, typeof(T).Name), typeof(T).Name + " Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                result.Data = dialogResult == DialogResult.Yes;
                result.ErrorCode = EnumErrorCode.SUCCESS;
                result.ErrorDesc = string.Format(Constants.delete_confirm_success, typeof(T).Name);
            }
            catch (Exception ex)
            {
                result.Data = false;
                result.ErrorCode = EnumErrorCode.ERROR;
                result.ErrorDesc = string.Format(Constants.delete_confirm_failed, typeof(T).Name) + ex.Message;
            }
            return result;
        }

        public static FuncResult<List<T>> Search(Expression<Func<T, bool>> predicate)
        {
            FuncResult<List<T>> result = new FuncResult<List<T>>();
            try
            {
                var table = dataContext.GetTable<T>();
                var queryResult = table.Where(predicate).ToList();

                result.Data = queryResult;
                result.ErrorCode = EnumErrorCode.SUCCESS;
                result.ErrorDesc = string.Format(Constants.search_success, typeof(T).Name);
            }
            catch (Exception ex)
            {
                result.ErrorCode = EnumErrorCode.ERROR;
                result.ErrorDesc = string.Format(Constants.search_failed, typeof(T).Name) + ex.Message;
            }
            return result;
        }


    }


}
