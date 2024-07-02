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
    public partial class OrderFormForManager : Form
    {
        public OrderFormForManager()
        {
            InitializeComponent();
            OrderControllerForManager ordercontroller = new OrderControllerForManager(this, OrderdataGridView, OrderDetaildataGridView, btnUpdate, btnDelete, cbxTieuchi, btnSearch, txtSearchContent);
            ordercontroller.SetEvent();
        }
    }
}
