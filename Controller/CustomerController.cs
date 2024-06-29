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

namespace BTL_2.Controller
{
    public class CustomerController
    {
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
        public TextBox txtAddress { get; private set; }
        public TextBox txtEmail { get; private set; }


        public TextBox txtSearchContent { get; private set; }
        public ComboBox cbxTieuChi { get; private set; }
        public Button btnSearch { get; private set; }
       

        public CustomerController(CustomerForm customerForm, DataGridView customerdataGridView, Label lbId, TextBox txtName, Button btnDelete, Button btnUpdate, Button btnInsert, TextBox txtPhone, TextBox txtAddress, TextBox txtEmail, TextBox txtSearchContent, ComboBox cbxTieuChi, Button btnSearch)
        {
            CustomerForm = customerForm;
            CustomerdataGridView = customerdataGridView;
            this.lbId = lbId;
            this.txtName = txtName;
            this.btnDelete = btnDelete;
            this.btnUpdate = btnUpdate;
            this.btnInsert = btnInsert;
            this.txtPhone = txtPhone;
            this.txtAddress = txtAddress;
            this.txtEmail = txtEmail;

            this.txtSearchContent = txtSearchContent;
            this.cbxTieuChi = cbxTieuChi;
            this.btnSearch = btnSearch;
            
        }

        public void SetEvent()
        {
            CustomerForm.Load += new EventHandler((object sender, EventArgs e) => LoadData());
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
                LoadData();
            }
            else
            {
                List<Customer> qr = null;
                switch (tieuchi)
                {
                    case "Name":
                        qr = dataContext.Customers.Where(u => u.CustomerName.Contains(content)).ToList();
                        break;
                    case "Address":
                        qr = dataContext.Customers.Where(u => u.Address.Contains(content)).ToList();
                        break;
                    case "PhoneNumber":
                        qr = dataContext.Customers.Where(u => u.PhoneNumber.Contains(content)).ToList();
                        break;
                    case "Email":
                        qr = dataContext.Customers.Where(u => u.Email.Contains(content)).ToList();
                        break;
                }
                
                    CustomerdataGridView.DataSource = null;
                    CustomerdataGridView.DataSource = qr;
                

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
            var address = txtAddress.Text;

            Customer customer = new Customer();
            customer.CustomerName = name;
            customer.Address = address;
            customer.PhoneNumber = phone;
            customer.Email = email;
            FuncResult<bool> funcResult = FuncShares<Customer>.Insert(customer);
            if (funcResult.Data)
            {
                LoadData();
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
            var address = txtAddress.Text;
            var customerToUpdate = dataContext.Customers.FirstOrDefault(o=> o.CustomerID == id);
            if (customerToUpdate != null)
            {
                customerToUpdate.CustomerName = name;
                customerToUpdate.Address = address;
                customerToUpdate.Email = email;
                customerToUpdate.PhoneNumber = phone;

                dataContext.SubmitChanges();
                string str = string.Format(Constants.update_success, Name);
                MessageBox.Show(str, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadData();
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
            int id = int.Parse(lbId.Text);
            FuncResult<List<Customer>> rs = FuncShares<Customer>.GetAllData();
            switch (rs.ErrorCode)
            {
                case EnumErrorCode.ERROR:
                    MessageBox.Show(rs.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case EnumErrorCode.SUCCESS:
                    var objToDelete = rs.Data.Where(u => u.CustomerID == id).FirstOrDefault();

                    FuncResult<bool> funcResult = FuncShares<Customer>.ShowConfirmationMessage();
                    if (objToDelete != null && funcResult.Data)
                    {
                        BackupCustomerData(objToDelete);

                        var result = FuncShares<Customer>.Delete(objToDelete);
                        if (result.Data)
                        {
                            //MessageBox.Show(Constants.delete_success, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();
                            ClearInputs();
                        }
                        else
                        {
                            MessageBox.Show(result.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (objToDelete == null)
                    {
                        MessageBox.Show(Constants.not_found, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                case EnumErrorCode.FAILED:
                    break;
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show(Constants.requiredName, "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
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
            string backupdata = $"ID: {customer.CustomerID}, name: {customer.CustomerName}, Address: {customer.Address}, Email: {customer.Email}, Phone: {customer.PhoneNumber}";
            System.IO.File.AppendAllText("backupSupplierData.txt", backupdata + Environment.NewLine);
        }

        private void ClearInputs()
        {
            CustomerdataGridView.ClearSelection();
            lbId.Text = string.Empty;
            txtName.Text = string.Empty;
            txtAddress.Text = string.Empty;
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
                txtAddress.Text = row.Cells[2].Value.ToString();
                txtPhone.Text = row.Cells[3].Value.ToString();
                txtEmail.Text = row.Cells[4].Value.ToString();
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


        private void LoadData()
        {
            CustomerdataGridView.DataSource = dataContext.Customers.ToList();
        }

        
    }
}
