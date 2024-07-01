using BTL_2.Model;
using BTL_2.View;
using BTL_2.Shareds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Reflection;

namespace BTL_2.Controller
{
    public class CustomerController
    {
        private AddressController addressController = null;
        private static string Name = "Customer";
        private DatabaseDataContext dataContext = new DatabaseDataContext();
        private int clickCount = 0; // Biến đếm số lần click vào hàng
        private int lastClickedRowIndex = -1; // Index của hàng được click lần cuối
        public CustomerForm CustomerForm { get; private set; }

        public DataGridView CustomerdataGridView { get; private set; }

        public Label lbId { get; private set; }
        public TextBox txtName { get; private set; }
        public Button btnDelete { get; private set; }
        public Button btnUpdate { get; private set; }
        public Button btnInsert { get; private set; }
        public TextBox txtPhone{ get; private set; }
        public ComboBox cbxProvince { get; private set; }
        public ComboBox cbxDistrict { get; private set; }
        public ComboBox cbxWard { get; private set; }
        public TextBox txtEmail { get; private set; }


        public TextBox txtSearchContent { get; private set; }
        public ComboBox cbxTieuChi { get; private set; }
        public Button btnSearch { get; private set; }
       

        public CustomerController(CustomerForm customerForm, DataGridView customerdataGridView, Label lbId, TextBox txtName, Button btnDelete, Button btnUpdate, Button btnInsert, TextBox txtPhone, ComboBox cbxProvince, ComboBox cbxDistrict, ComboBox cbxWard, TextBox txtEmail, TextBox txtSearchContent, ComboBox cbxTieuChi, Button btnSearch)
        {
            CustomerForm = customerForm;
            CustomerdataGridView = customerdataGridView;
            this.lbId = lbId;
            this.txtName = txtName;
            this.btnDelete = btnDelete;
            this.btnUpdate = btnUpdate;
            this.btnInsert = btnInsert;
            this.txtPhone = txtPhone;
            this.cbxProvince = cbxProvince;
            this.cbxDistrict = cbxDistrict;
            this.cbxWard = cbxWard;
            this.txtEmail = txtEmail;

            this.txtSearchContent = txtSearchContent;
            this.cbxTieuChi = cbxTieuChi;
            this.btnSearch = btnSearch;
            
        }

        public void SetEvent()
        {
            CustomerForm.Load += new EventHandler((object sender, EventArgs e) => {
                LoadDataGridView();
                LoadComboxAddres();
            }); 
            btnInsert.Click += Insert;
            CustomerdataGridView.RowHeaderMouseClick += DataGridView_RowHeaderMouseClick;
            btnUpdate.Click += Update;
            btnDelete.Click += Delete;
            btnSearch.Click += Search;
        }

        public void Search(object obj, EventArgs e)
        {
            var content = txtSearchContent.Text;
            var tieuchi = cbxTieuChi.Text;

            if (string.IsNullOrEmpty(content))
            {
                LoadDataGridView();
            }
            else
            {
                FuncResult<List<Customer>> qr = null;
                switch (tieuchi)
                {
                    case "Name":
                        qr = FuncShares<Customer>.Search(u => u.CustomerName.Contains(content));
                        break;
                    case "Address":
                        qr = FuncShares<Customer>.Search(u => u.Province.Contains(content));
                        break;
                    case "PhoneNumber":
                        qr = FuncShares<Customer>.Search(u => u.PhoneNumber.Contains(content));
                        break;
                    case "Email":
                        qr = FuncShares<Customer>.Search(u => u.Email.Contains(content));
                        break;
                }

                if (qr != null && qr.ErrorCode == EnumErrorCode.SUCCESS)
                {
                    CustomerdataGridView.DataSource = null;
                    CustomerdataGridView.DataSource = qr.Data;
                }
                else if (qr != null)
                {
                    MessageBox.Show(qr.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Insert(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {

            return; 
            }
            var name = txtName.Text;
            var phone = txtPhone.Text;
            var email = txtEmail.Text;
            string province = cbxProvince.Text;
            string district = cbxDistrict.Text;
            string ward = cbxWard.Text;

            Customer customer = new Customer();
            customer.CustomerName = name;
            customer.Province = province;
            customer.District = district;
            customer.Ward = ward; ;
            customer.PhoneNumber = phone;
            customer.Email = email;
            FuncResult<bool> funcResult = FuncShares<Customer>.Insert(customer);
            if (funcResult.Data)
            {
                LoadDataGridView();
            }
            else
            {
                MessageBox.Show(funcResult.ErrorDesc, "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Update(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }
            int id = int.Parse(lbId.Text);
            var name = txtName.Text;
            var phone = txtPhone.Text;
            var email = txtEmail.Text;
            string province = cbxProvince.Text;
            string district = cbxDistrict.Text;
            string ward = cbxWard.Text;
            var customerToUpdate = dataContext.Customers.FirstOrDefault(o=> o.CustomerID == id);
            if (customerToUpdate != null)
            {
                customerToUpdate.CustomerName = name;
                customerToUpdate.Province = province;
                customerToUpdate.District = district;
                customerToUpdate.Ward = ward;
                customerToUpdate.Email = email;
                customerToUpdate.PhoneNumber = phone;

                dataContext.SubmitChanges();
                string str = string.Format(Constants.update_success, Name);
                MessageBox.Show(str, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadDataGridView();
                ClearInputs();
            }
            else
            {
                MessageBox.Show(Constants.not_found, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Delete(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(lbId.Text))
            {
                MessageBox.Show(Constants.no_selected, "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(lbId.Text, out int id))
            {
                MessageBox.Show("Invalid Customer ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FuncResult<List<Customer>> searchResult = FuncShares<Customer>.Search(u => u.CustomerID == id);
            if (searchResult.ErrorCode == EnumErrorCode.ERROR)
            {
                MessageBox.Show(searchResult.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (searchResult.Data.Count == 0)
            {
                MessageBox.Show(Constants.not_found, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            FuncResult<bool> confirmationResult = FuncShares<Customer>.ShowConfirmationMessage();
            if (!confirmationResult.Data)
            {
                return;
            }

            Customer obj = searchResult.Data[0];

            var orderDetailResult = FuncShares<OrderDetail>.GetAllData();
            if (orderDetailResult.ErrorCode == EnumErrorCode.ERROR)
            {
                MessageBox.Show(orderDetailResult.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var orderResult = FuncShares<Order>.GetAllData();
            if (orderResult.ErrorCode == EnumErrorCode.ERROR)
            {
                MessageBox.Show(orderResult.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<OrderDetail> list_orderdetail = orderDetailResult.Data;
            List<Order> list_order = orderResult.Data;

            var query = from c in searchResult.Data
                        join o in list_order on c.CustomerID equals o.CustomerID
                        join od in list_orderdetail on o.OrderID equals od.OrderID
                        where c.CustomerID == obj.CustomerID
                        select new
                        {
                            Customer = c,
                            Order = o,
                            OrderDetail = od
                        };

            var itemsToDelete = query.ToList();

            if (itemsToDelete.Count > 0)
            {
                //BackupData(obj);

                foreach (var item in itemsToDelete)
                {
                    var deleteOrderDetailResult = FuncShares<OrderDetail>.Delete(item.OrderDetail);
                    if (deleteOrderDetailResult.ErrorCode == EnumErrorCode.ERROR)
                    {
                        MessageBox.Show(deleteOrderDetailResult.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var deleteOrderResult = FuncShares<Order>.Delete(item.Order);
                    if (deleteOrderResult.ErrorCode == EnumErrorCode.ERROR)
                    {
                        MessageBox.Show(deleteOrderResult.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var deleteCustomerResult = FuncShares<Customer>.Delete(item.Customer);
                    if (deleteCustomerResult.ErrorCode == EnumErrorCode.ERROR)
                    {
                        MessageBox.Show(deleteCustomerResult.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        LoadDataGridView();
                        ClearInputs();
                        MessageBox.Show(deleteCustomerResult.ErrorDesc, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

            }
            else
            {
                MessageBox.Show(Constants.not_found, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show(Constants.requiredName, "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(cbxProvince.Text))
            {
                MessageBox.Show(Constants.requiredAddress, "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if(string.IsNullOrWhiteSpace(txtEmail.Text) || !isValidEmail(txtEmail.Text))
            {
                MessageBox.Show(Constants.requiredEmail, "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if(string.IsNullOrWhiteSpace(txtPhone.Text) || !isValidPhone(txtPhone.Text))
            {
                MessageBox.Show(Constants.requiredPhone, "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void BackupCustomerData(Customer customer)
        {
            string backupdata = $"ID: {customer.CustomerID}, name: {customer.CustomerName}, Address: {customer.Province}, Email: {customer.Email}, Phone: {customer.PhoneNumber}";
            System.IO.File.AppendAllText("backupSupplierData.txt", backupdata + Environment.NewLine);
        }

        private void ClearInputs()
        {
            CustomerdataGridView.ClearSelection();
            lbId.Text = string.Empty;
            txtName.Text = string.Empty;
            cbxProvince.SelectedIndex = 1;
            //cbxDistrict.SelectedIndex = 1;
            //cbxWard.SelectedIndex = 1;
            txtEmail.Text = string.Empty;
            txtPhone.Text = string.Empty;
            btnInsert.Enabled = true;
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
        }

        private void DataGridView_RowHeaderMouseClick(object sender, EventArgs e)
        {
            if (CustomerdataGridView.SelectedRows.Count > 0)
            {
                btnInsert.Enabled = false;
                btnDelete.Enabled = true;
                btnUpdate.Enabled = true;

                DataGridViewRow row = CustomerdataGridView.SelectedRows[0];
                int currentRowIndex = row.Index;

                if (currentRowIndex == lastClickedRowIndex)
                {
                    clickCount++;

                    if (clickCount == 2)
                    {
                        //AccountDataGridView.ClearSelection();
                        ClearInputs();
                        clickCount = 0;
                        lastClickedRowIndex = -1;
                        return;
                    }
                }
                else
                {
                    clickCount = 1;
                }

                lastClickedRowIndex = currentRowIndex;

                lbId.Text = row.Cells[0].Value.ToString();
                txtName.Text = row.Cells[1].Value.ToString();
                string province = row.Cells[2].Value.ToString();
                string district = row.Cells[3].Value.ToString();
                string ward = row.Cells[4].Value.ToString();
                addressController.SetComboBoxSelection(province, district, ward);
                txtPhone.Text = row.Cells[5].Value.ToString();
                txtEmail.Text = row.Cells[6].Value.ToString();
            }
        }
        private bool isValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool isValidPhone(string phone)
        {
            return Regex.IsMatch(phone, @"^\d{10}$");
        }
        private void LoadDataGridView()
        {
            FuncResult<List<Customer>> rs = FuncShares<Customer>.GetAllData();
            switch (rs.ErrorCode)
            {
                case EnumErrorCode.ERROR:
                    MessageBox.Show(rs.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case EnumErrorCode.SUCCESS:
                    CustomerdataGridView.DataSource = rs.Data;
                    break;
                case EnumErrorCode.FAILED:
                    break;
            }
        }
        private void LoadComboxAddres()
        {
            addressController = new AddressController(cbxProvince, cbxDistrict, cbxWard);
        }

    }
}
