using BTL_2.Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTL_2.View
{
    public partial class AccountManagerForm : Form
    {
        public AccountManagerForm()
        {
            InitializeComponent();
            AccountManagerController accountManagerController = new AccountManagerController(this, txtFullName, lbId, btnDelete, btnUpdate, btnInsert, cbxRole, txtPhone, txtName,txtPass, txtEmail, AccountDataGridView, txtSearchContent, cbxTieuchi, btnSearch,cbxSearchContent);
            accountManagerController.SetEvent();
        }
    }
}
