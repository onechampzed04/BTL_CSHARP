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
    public partial class OrderForm : Form
    {
        public OrderForm()
        {
            InitializeComponent();
            OrderController ordercontroller = new OrderController(this, listProductOfOrddataGridView, lbOrderId, btnInsert, btnUpdate, btnDelete,txtListOrder, txtTotalAmount, cbxCustomer, dtpDate, cbxTieuchi, btnSearch, txtSearchContent);
;           //ordercontroller.SetEvent();
        }
    }
}
