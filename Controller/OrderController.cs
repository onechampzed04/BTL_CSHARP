/*using BTL_2.Model;
using BTL_2.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTL_2.Controller
{
    public class OrderController
    {
        private DatabaseDataContext dataContext = new DatabaseDataContext();
        public OrderForm Form { get; private set; }
        public DataGridView SupllierdataGridView { get; private set; }
        public Label lbSupplierId { get; private set; }
        public Button btnDelete { get; private set; }
        public Button btnUpdate { get; private set; }
        public Button btnInsert { get; private set; }
        public TextBox txtPhone { get; private set; }
        public TextBox txtSupplierName { get; private set; }
        public TextBox txtAddress { get; private set; }
        public TextBox txtEmail { get; private set; }

        public ComboBox cbxTieuChi { get; private set; }
        public Button btnSearch { get; private set; }
        public TextBox txtSearchContent { get; private set; }

        public SupplierController(SupplierForm supplierForm, DataGridView supllierdataGridView, Label lbSupplierId, Button btnDelete, Button btnUpdate, Button btnInsert, TextBox txtPhone, TextBox txtSupplierName, TextBox txtAddress, TextBox txtEmail, ComboBox cbxTieuChi, Button btnSearch, TextBox txtSearchContent)
        {

            SupplierForm = supplierForm;
            SupllierdataGridView = supllierdataGridView;
            this.lbSupplierId = lbSupplierId;
            this.btnDelete = btnDelete;
            this.btnUpdate = btnUpdate;
            this.btnInsert = btnInsert;
            this.txtPhone = txtPhone;
            this.txtSupplierName = txtSupplierName;
            this.txtAddress = txtAddress;
            this.txtEmail = txtEmail;

            this.cbxTieuChi = cbxTieuChi;
            this.btnSearch = btnSearch;
            this.txtSearchContent = txtSearchContent;
        }

        *//*public SupplierController(SupplierForm supplierForm, DataGridView supllierdataGridView, Label lbSupplierId, Button btnDelete, Button btnUpdate, Button btnInsert, TextBox txtPhone, TextBox txtSupplierName, TextBox txtAddress, TextBox txtEmail)
        {
            SupplierForm = supplierForm;
            SupllierdataGridView = supllierdataGridView;
            this.lbSupplierId = lbSupplierId;
            this.btnDelete = btnDelete;
            this.btnUpdate = btnUpdate;
            this.btnInsert = btnInsert;
            this.txtPhone = txtPhone;
            this.txtSupplierName = txtSupplierName;
            this.txtAddress = txtAddress;
            this.txtEmail = txtEmail;
        }*//*

        public void SetEvent()
        {
            SupplierForm.Load += new EventHandler((object sender, EventArgs e) => LoadData());
            btnInsert.Click += InsertSupplier;
            SupllierdataGridView.SelectionChanged += DataGridView_SelectionChanged;
            btnUpdate.Click += UpdateSupplier;
            btnDelete.Click += DeleteSupplier;
            btnSearch.Click += Search;

        }


        public void Search(object obj, EventArgs e)
        {
            var content = txtSearchContent.Text;
            var tieuchi = cbxTieuChi.Text;
            //string str = null; Console.WriteLine(str.Length);

            if (string.IsNullOrEmpty(content))
            {
                LoadData();
            }
            else
            {
                List<Order> qr = null;
                switch (tieuchi)
                {
                    case "Username":
                        qr = dataContext.Suppliers.Where(u => u.SupplierName.Contains(content)).ToList();
                        break;
                    case "Address":
                        qr = dataContext.Suppliers.Where(u => u.Address.Contains(content)).ToList();
                        break;
                    case "PhoneNumber":
                        qr = dataContext.Suppliers.Where(u => u.PhoneNumber.Contains(content)).ToList();
                        break;
                    case "Email":
                        qr = dataContext.Suppliers.Where(u => u.Email.Contains(content)).ToList();
                        break;
                }
                SupllierdataGridView.DataSource = null;
                SupllierdataGridView.DataSource = qr;
            }
        }
        private void DeleteSupplier(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(lbSupplierId.Text))
            {
                MessageBox.Show("No Order selected for deletion.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = int.Parse(lbSupplierId.Text);
            var supplierToDelete = dataContext.Suppliers.FirstOrDefault(u => u.SupplierID == id);

            if (supplierToDelete != null && ShowConfirmationMessage())
            {
                BackupUserData(supplierToDelete);

                dataContext.Suppliers.DeleteOnSubmit(supplierToDelete);
                dataContext.SubmitChanges();
                MessageBox.Show("Order deleted successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadData();
                ClearInputs();
            }
            if (supplierToDelete == null)
            {
                MessageBox.Show("Order not found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BackupUserData(Order Order)
        {
            string backupData = $"ID: {Order.SupplierID}, name: {Order.SupplierName}, Address: {Order.Address}, Email: {Order.Email}, Phone: {Order.PhoneNumber}";
            System.IO.File.AppendAllText("backupSupplierData.txt", backupData + Environment.NewLine);
        }

        private void ClearInputs()
        {
            lbSupplierId.Text = "";
            txtSupplierName.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            txtPhone.Text = "";
            btnInsert.Enabled = true;
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
        }

        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (SupllierdataGridView.SelectedRows.Count > 0)
            {
                btnInsert.Enabled = false;
                btnDelete.Enabled = true;
                btnUpdate.Enabled = true;

                DataGridViewRow row = SupllierdataGridView.SelectedRows[0];

                lbSupplierId.Text = row.Cells[0].Value.ToString();
                txtSupplierName.Text = row.Cells[1].Value.ToString();
                txtAddress.Text = row.Cells[2].Value.ToString();
                txtPhone.Text = row.Cells[3].Value.ToString();
                txtEmail.Text = row.Cells[4].Value.ToString();
            }
        }

        private void UpdateSupplier(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            int id = int.Parse(lbSupplierId.Text);
            var name = txtSupplierName.Text;
            var address = txtAddress.Text;
            var phone = txtPhone.Text;
            var email = txtEmail.Text;

            var supplierToUpdate = dataContext.Suppliers.FirstOrDefault(u => u.SupplierID == id);

            if (supplierToUpdate != null)
            {
                supplierToUpdate.SupplierName = name;
                supplierToUpdate.Address = address;
                supplierToUpdate.Email = email;
                supplierToUpdate.PhoneNumber = phone;

                dataContext.SubmitChanges();
                MessageBox.Show("Order updated successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadData();
            }
            else
            {
                MessageBox.Show("Order not found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void InsertSupplier(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            var name = txtSupplierName.Text;
            var address = txtAddress.Text;
            var phone = txtPhone.Text;
            var email = txtEmail.Text;

            Order Order = new Order();
            Order.SupplierName = name;
            Order.Address = address;
            Order.Email = email;
            Order.PhoneNumber = phone;
            dataContext.Suppliers.InsertOnSubmit(Order);
            dataContext.SubmitChanges();

            LoadData();
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtSupplierName.Text))
            {
                MessageBox.Show("Order Name is required.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Address is required.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("A valid Email is required.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text) || !IsValidPhone(txtPhone.Text))
            {
                MessageBox.Show("A valid Phone Number is required.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
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

        private bool IsValidPhone(string phone)
        {
            return Regex.IsMatch(phone, @"^\d{10}$");
        }

        private void LoadData()
        {
            SupllierdataGridView.DataSource = dataContext.Suppliers.ToList();
        }

        private bool ShowConfirmationMessage()
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            return result == DialogResult.Yes;
        }
    }
}
*/