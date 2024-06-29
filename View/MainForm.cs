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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            MainFormController mainFormController = new MainFormController(this , LoginMenuItem, AccountManagerMenuItem, LogOutMenuItem, pnView, ProductMenuItem, SuppliersMenuItem, CustomerMenuItem, OrderMenuItem, InventoryMenuItem);
            mainFormController.SetEvent();
        }
    }
}
