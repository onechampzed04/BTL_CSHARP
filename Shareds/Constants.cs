using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTL_2.Shareds
{
    public class Constants
    {
        public static string invalidAccount = "Invalid username or password.";
        public static string emptyAccount = "Empty username or password.";

        public static string requiredAddress = "Address is required.";
        public static string requiredName = "Customer Name is required.";
        public static string requiredsName = "Customer Name is reqsred.";

        public static string requiredUserName = "User Name is required.";
        public static string requiredPassword = "Password is required.";
        public static string requiredEmail = "A valid Email is required.";
        public static string requiredPhone = "A valid Phone Number is required.";
        public static string requiredRole = "Role must be selected.";

        public static string validateId = "ID is invalid";
        public static string validateRole = "Role is invalid";

        public static string getAllData_success = "Lấy dữ liệu {0} thành công";
        public static string getAllData_failed = "Lấy dữ liệu {0} thất bại";

        public static string insert_success = "Thêm {0} thành công";
        public static string insert_failed = "Thêm {0} thất bại";

        public static string update_success = "Cập nhật {0} thành công";
        public static string update_failed = "Cập nhật {0} thất bại";

        public static string delete_confirmation = "Bạn có chắc chắn muốn xóa {0} không?";
        public static string no_selected = "No {0} selected for deletion.";
        public static string not_found = "{0} not found.";
        public static string delete_success = "Xóa {0} thành công";
        public static string delete_failed = "Xóa {0} thất bại";
        public static string delete_confirm_success = "Confirmation delete {0} completed.";
        public static string delete_confirm_failed = "Error during delete {0} confirmation: ";

        public const string SearchByUsername = "Username";
        public const string SearchByRole = "Role";
        public const string SearchByPhoneNumber = "PhoneNumber";
        public const string SearchByFullName = "FullName";

        public const string SearchByProductname = "Productname";
        public const string SearchByID = "ID";
        public const string SearchByPrice = "Price";
        public const string SearchByUnit = "Unit";


        public const string search_success = "{0} search completed successfully.";
        public const string search_failed = "{0} search failed: ";
    }
}
