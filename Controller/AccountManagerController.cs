using BTL_2.Model;
using BTL_2.Shareds;
using BTL_2.View;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace BTL_2.Controller
{
    public class AccountManagerController
    {
        private static string Name = "User";
        private int clickCount = 0; // Biến đếm số lần click vào hàng
        private int lastClickedRowIndex = -1; // Index của hàng được click lần cuối
        public AccountManagerForm AccountManagerForm { get; private set; }
        public TextBox txtFullName { get; private set; }
        public Label lbID { get; private set; }
        public Button btnDelete { get; private set; }
        public Button btnUpdate { get; private set; }
        public Button btnInsert { get; private set; }
        public ComboBox cbxRole { get; private set; }
        public TextBox txtPhoneNumber { get; private set; }
        public TextBox txtUserName { get; private set; }
        public TextBox txtPassword { get; private set; }
        public TextBox txtEmail { get; private set; }
        public DataGridView AccountDataGridView { get; private set; }

        public TextBox txtSearchContent { get; private set; }
        public ComboBox cbxTieuChi { get; private set; }
        public Button btnSearch { get; private set; }
        public ComboBox cbxSearchContent { get; private set; }

        public AccountManagerController(AccountManagerForm accountManagerForm, TextBox txtFullName, Label lbID, Button btnDelete, Button btnUpdate, Button btnInsert, ComboBox cbxRole, TextBox txtPhoneNumber, TextBox txtUserName, TextBox txtPassword, TextBox txtEmail, DataGridView accountDataGridView, TextBox txtSearchContent, ComboBox cbxTieuChi, Button btnSearch, ComboBox cbxSearchContent)
        {
            AccountManagerForm = accountManagerForm;
            this.txtFullName = txtFullName;
            this.lbID = lbID;
            this.btnDelete = btnDelete;
            this.btnUpdate = btnUpdate;
            this.btnInsert = btnInsert;
            this.cbxRole = cbxRole;
            this.txtPhoneNumber = txtPhoneNumber;
            this.txtUserName = txtUserName;
            this.txtPassword = txtPassword;
            this.txtEmail = txtEmail;
            AccountDataGridView = accountDataGridView;
            this.txtSearchContent = txtSearchContent;
            this.cbxTieuChi = cbxTieuChi;
            this.btnSearch = btnSearch;
            this.cbxSearchContent = cbxSearchContent;

            //AccountDataGridView.Leave += new EventHandler((object sender, EventArgs e) => ClearInputs());

        }

        public void SetEvent()
        {
            AccountManagerForm.Load += new EventHandler((object sender, EventArgs e) => LoadData());
            btnInsert.Click += Insert;
            AccountDataGridView.RowHeaderMouseClick += DataGridView_RowHeaderMouseClick;
            btnUpdate.Click += Update;
            btnDelete.Click += Delete;
            btnSearch.Click += Search;
            cbxTieuChi.SelectedIndexChanged += CbxTieuChi_SelectedIndexChanged;
            //AccountManagerForm.Click += Form_Click;
        }

        private void CbxTieuChi_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tieuchi = cbxTieuChi.SelectedItem.ToString();
            if (tieuchi.Equals(Constants.SearchByRole))
            {
                cbxSearchContent.Visible = true;
                txtSearchContent.Visible = false;
                txtSearchContent.Text = "";
                FuncResult<List<Role>> rs = FuncShares<Role>.GetAllData();
                switch (rs.ErrorCode)
                {
                    case EnumErrorCode.ERROR:
                        MessageBox.Show(rs.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case EnumErrorCode.SUCCESS:
                        cbxSearchContent.DataSource = rs.Data;
                        cbxSearchContent.DisplayMember = "RoleName";
                        cbxSearchContent.ValueMember = "RoleId";
                        break;
                    case EnumErrorCode.FAILED:
                        break;
                }
            }
            else
            {
                cbxSearchContent.Visible = false;
                txtSearchContent.Visible = true;
            }
        }

        public void Search(object obj, EventArgs e)
        {
            var content = txtSearchContent.Text;
            var tieuchi = cbxTieuChi.Text;

            if (string.IsNullOrEmpty(content) && tieuchi != Constants.SearchByRole)
            {
                LoadData();
            }
            else
            {
                FuncResult<List<User>> qr = null;
                switch (tieuchi)
                {
                    case Constants.SearchByUsername:
                        qr = FuncShares<User>.Search(u => u.Username.Contains(content));
                        break;
                    case Constants.SearchByRole:
                        qr = FuncShares<User>.Search(u => u.RoleID == (int)cbxSearchContent.SelectedValue);
                        break;
                    case Constants.SearchByPhoneNumber:
                        qr = FuncShares<User>.Search(u => u.PhoneNumber.Contains(content));
                        break;
                    case Constants.SearchByFullName:
                        qr = FuncShares<User>.Search(u => u.FullName.Contains(content));
                        break;
                }

                if (qr != null && qr.ErrorCode == EnumErrorCode.SUCCESS)
                {
                    AccountDataGridView.DataSource = null;
                    AccountDataGridView.DataSource = qr.Data;
                }
                else if (qr != null)
                {
                    MessageBox.Show(qr.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Delete(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(lbID.Text))
            {
                MessageBox.Show(Constants.no_selected, "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = int.Parse(lbID.Text);
            FuncResult<List<User>> rs = FuncShares<User>.GetAllData();
            switch (rs.ErrorCode)
            {
                case EnumErrorCode.ERROR:
                    MessageBox.Show(rs.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case EnumErrorCode.SUCCESS:
                    var objToDelete = rs.Data
                        .Where(u => u.UserID == id)
                    .FirstOrDefault();

                    FuncResult<bool> funcResult = FuncShares<User>.ShowConfirmationMessage();
                    if (objToDelete != null && funcResult.Data)
                    {
                        BackupData(objToDelete);

                        var result = FuncShares<User>.Delete(objToDelete);
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

        private void BackupData(User user)
        {
            string backupData = $"ID: {user.UserID}, Username: {user.Username}, FullName: {user.FullName}, Email: {user.Email}, Phone: {user.PhoneNumber}, RoleID: {user.RoleID}";
            System.IO.File.AppendAllText("backupUserData.txt", backupData + Environment.NewLine);
        }

        private void ClearInputs()
        {
            AccountDataGridView.ClearSelection();
            lbID.Text = "";
            txtUserName.Text = "";
            txtPassword.Text = "";
            txtEmail.Text = "";
            txtPhoneNumber.Text = "";
            cbxRole.SelectedIndex = -1;
            txtFullName.Text = "";
            btnInsert.Enabled = true;
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
        }

        private void DataGridView_RowHeaderMouseClick(object sender, EventArgs e)
        {
            if (AccountDataGridView.SelectedRows.Count > 0)
            {
                btnInsert.Enabled = false;
                btnDelete.Enabled = true;
                btnUpdate.Enabled = true;

                DataGridViewRow row = AccountDataGridView.SelectedRows[0];
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

                lbID.Text = row.Cells[0].Value.ToString();
                txtUserName.Text = row.Cells[1].Value.ToString();
                txtPassword.Text = row.Cells[2].Value.ToString();
                txtEmail.Text = row.Cells[4].Value.ToString();
                var temp = row.Cells[5];
                if (temp.Value != null)
                {
                    txtPhoneNumber.Text = row.Cells[5].Value.ToString();

                }
                else txtPhoneNumber.Text = "";

                cbxRole.SelectedValue = row.Cells[6].Value;
                txtFullName.Text = row.Cells[3].Value.ToString();
            }
            else
            {
                ClearInputs();
            }
        }

        private void Update(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            int id;
            if (!int.TryParse(lbID.Text, out id))
            {
                MessageBox.Show(Constants.validateId, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var name = txtUserName.Text;
            var pass = txtPassword.Text;
            var phone = txtPhoneNumber.Text;
            var email = txtEmail.Text;
            var fullName = txtFullName.Text;
            int roleid;

            if (cbxRole.SelectedValue == null || !int.TryParse(cbxRole.SelectedValue.ToString(), out roleid))
            {
                MessageBox.Show(Constants.validateRole, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FuncResult<List<User>> rs = FuncShares<User>.GetAllData();
            switch (rs.ErrorCode)
            {
                case EnumErrorCode.ERROR:
                    MessageBox.Show(rs.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case EnumErrorCode.SUCCESS:
                    var objToUpdate = rs.Data
                        .Where(u => u.UserID == id)
                        .FirstOrDefault();

                    if (objToUpdate != null)
                    {
                        var deleteResult = FuncShares<User>.Delete(objToUpdate);
                        if (deleteResult.Data)
                        {
                            User newUser = new User
                            {
                                UserID = id,
                                Username = name,
                                PasswordHash = pass,
                                Email = email,
                                PhoneNumber = phone,
                                FullName = fullName,
                                RoleID = roleid
                            };

                            var insertResult = FuncShares<User>.Insert(newUser);
                            if (insertResult.Data)
                            {
                                LoadData();
                                ClearInputs();
                                MessageBox.Show("Update successful", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show(insertResult.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show(deleteResult.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        string str = string.Format(Constants.not_found, Name);
                        MessageBox.Show(str, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;
                case EnumErrorCode.FAILED:
                    break;
            }
        }

        /*private void Update(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            int id = int.Parse(lbID.Text);
            var name = txtUserName.Text;
            var pass = txtPassword.Text;
            var phone = txtPhoneNumber.Text;
            var email = txtEmail.Text;
            var roleid = (int)cbxRole.SelectedValue;

            FuncResult<List<User>> rs = FuncShares<User>.GetAllData();
            switch (rs.ErrorCode)
            {
                case EnumErrorCode.ERROR:
                    MessageBox.Show(rs.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case EnumErrorCode.SUCCESS:
                    var objToUpdate = rs.Data
                        .Where(u => u.UserID == id)
                        .FirstOrDefault();

                    if (objToUpdate != null)
                    {
                        // Kiểm tra xem có cần phải gán lại RoleID hay không
                        if (objToUpdate.RoleID != roleid)
                        {
                            objToUpdate.RoleID = roleid;
                        }

                        objToUpdate.Username = name;
                        objToUpdate.PasswordHash = pass;
                        objToUpdate.Email = email;
                        objToUpdate.PhoneNumber = phone;
                        objToUpdate.FullName = txtFullName.Text;

                        var result = FuncShares<User>.Update(objToUpdate);
                        if (result.Data)
                        {
                            LoadData();
                            ClearInputs();
                            MessageBox.Show(result.ErrorDesc, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show(result.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        string str = string.Format(Constants.not_found, Name);
                        MessageBox.Show(str, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;
                case EnumErrorCode.FAILED:
                    break;
            }
        }*/

        private void Insert(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            var name = txtUserName.Text;
            var pass = txtPassword.Text;
            var phone = txtPhoneNumber.Text;
            var email = txtEmail.Text;
            var roleid = (int)cbxRole.SelectedValue;

            User user = new User();
            user.Username = name;
            user.PasswordHash = pass;
            user.Email = email;
            user.PhoneNumber = phone;
            user.RoleID = roleid;
            user.FullName = txtFullName.Text;
            
            FuncResult<bool> funcResult = FuncShares<User>.Insert(user);
            if(funcResult.Data)
            {
                LoadData();
            }
            else
            {
                MessageBox.Show( funcResult.ErrorDesc, "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                MessageBox.Show( Constants.requiredUserName, "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show(Constants.requiredPassword, "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show(Constants.requiredEmail, "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPhoneNumber.Text) || !IsValidPhone(txtPhoneNumber.Text))
            {
                MessageBox.Show(Constants.requiredPhone, "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cbxRole.SelectedIndex == -1)
            {
                MessageBox.Show(Constants.requiredRole, "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            AccountDataGridView.DataSource = null;
            FuncResult<List<User>> rs = FuncShares<User>.GetAllData();
            switch (rs.ErrorCode)
            {
                case EnumErrorCode.ERROR:
                    MessageBox.Show(rs.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case EnumErrorCode.SUCCESS:
                    AccountDataGridView.DataSource = rs.Data;
                    FuncResult<List<Role>> role = FuncShares<Role>.GetAllData();
                    switch (rs.ErrorCode)
                    {
                        case EnumErrorCode.ERROR:
                            MessageBox.Show(rs.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        case EnumErrorCode.SUCCESS:
                            cbxRole.DataSource = role.Data;
                            cbxRole.DisplayMember = "RoleName";
                            cbxRole.ValueMember = "RoleId";
                            break;
                        case EnumErrorCode.FAILED:
                            break;
                    }
                    cbxTieuChi.DataSource = new List<string>() {
                        Constants.SearchByUsername,
                        Constants.SearchByRole,
                        Constants.SearchByPhoneNumber,
                        Constants.SearchByFullName,
                    };
                    break;
                case EnumErrorCode.FAILED:
                    break;
            }
        }
    }
}
