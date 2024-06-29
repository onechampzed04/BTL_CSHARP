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
    public partial class CustomerForm : Form
    {
        public CustomerForm()
        {
            InitializeComponent();
            CustomerController customerController = new CustomerController(this, CustomerDataGridView, lbId, txtName, btnDelete, btnUpdate, btnInsert, txtPhone, txtAddress, txtEmail, txtSearchContent, cbxTieuchi, btnSearch);
            customerController.SetEvent();
        }
    }
}
