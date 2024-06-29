using BTL_2.Model;
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
    public class MainFormController
    {
        private DatabaseDataContext dataContext = new DatabaseDataContext();
        
        public MainForm mainForm { get; private set; }
        public ToolStripMenuItem LoginMenuItem { get; private set; }
        public ToolStripMenuItem AccountManagerMenuItem { get; private set; }
        public ToolStripMenuItem LogOutMenuItem { get; private set; }
        public Panel pnView { get; private set; }
        public ToolStripMenuItem ProductMenuItem { get; private set; }
        public ToolStripMenuItem SuppliersMenuItem { get; private set; }
        public ToolStripMenuItem CustomerMenuItem { get; private set; }
        public ToolStripMenuItem OrderMenuItem { get; private set; }
        public ToolStripMenuItem InventoryMenuItem { get; private set; }

        public MainFormController(MainForm mainForm, ToolStripMenuItem loginMenuItem, ToolStripMenuItem accountManagerMenuItem, ToolStripMenuItem logOutMenuItem, Panel pnView, ToolStripMenuItem productMenuItem, ToolStripMenuItem suppliersMenuItem, ToolStripMenuItem customerMenuItem, ToolStripMenuItem orderMenuItem, ToolStripMenuItem inventoryMenuItem)
        {
            this.mainForm = mainForm;
            LoginMenuItem = loginMenuItem;
            AccountManagerMenuItem = accountManagerMenuItem;
            LogOutMenuItem = logOutMenuItem;
            this.pnView = pnView;
            ProductMenuItem = productMenuItem;
            SuppliersMenuItem = suppliersMenuItem;
            CustomerMenuItem = customerMenuItem;
            OrderMenuItem = orderMenuItem;
            InventoryMenuItem = inventoryMenuItem;
        }

        public void SetMenuItem()
        {
            if (Constant.User.RoleID == 1)
            {
                AccountManagerMenuItem.Visible = true;
            }
            if(Constant.User.RoleID == 3)
            {
                CustomerMenuItem.Visible = false;
                OrderMenuItem.Visible = false;
                InventoryMenuItem.Visible = false;
            }
            if (Constant.User.RoleID == 2)
            {
//                (2, (SELECT PermissionID FROM Permissions WHERE PermissionName = 'ViewProducts')),
//(2, (SELECT PermissionID FROM Permissions WHERE PermissionName = 'AddOrder')),
//(2, (SELECT PermissionID FROM Permissions WHERE PermissionName = 'ViewOrders')),
//(2, (SELECT PermissionID FROM Permissions WHERE PermissionName = 'AddCustomer')),
//(2, (SELECT PermissionID FROM Permissions WHERE PermissionName = 'ViewCustomers'));

                SuppliersMenuItem.Visible = false;
                InventoryMenuItem.Visible = false;
            }
            if (Constant.User.RoleID == 4)
            {
//                (4, (SELECT PermissionID FROM Permissions WHERE PermissionName = 'ViewOrders')),
//(4, (SELECT PermissionID FROM Permissions WHERE PermissionName = 'ViewReports'));
//```
                SuppliersMenuItem.Visible = false;
                ProductMenuItem.Visible = false;
                CustomerMenuItem.Visible = false;
                InventoryMenuItem.Visible = false;
            }
        }
        public void SetEvent()
        {
            mainForm.Load += ShowLoginForm;

            LoginMenuItem.Click += ShowLoginForm;

            AccountManagerMenuItem.Click += new EventHandler((object sender, EventArgs e) =>
            {
                AccountManagerViewLoad();
            });

            LogOutMenuItem.Click += new EventHandler((object sender, EventArgs e) =>
            {
                Constant.User = null;
                UpdateMenuItems();
            }); 
            SuppliersMenuItem.Click += new EventHandler((object sender, EventArgs e) =>
            {
                SupplierManagerViewLoad();
            });

            ProductMenuItem.Click += new EventHandler((object sender, EventArgs e) =>
            {
                ProductViewLoad();
            });

            CustomerMenuItem.Click += new EventHandler((object sender, EventArgs e) =>
            {
                CustomerManagerViewLoad();
            });
        }

        private void ShowLoginForm(object sender, EventArgs e)
        {
            if (Constant.User == null)
            {
                using (LoginForm login = new LoginForm())
                {
                    login.ShowDialog();
                }

                if (Constant.User != null)
                {
                    LoginMenuItem.Visible = false;
                    //AccountManagerMenuItem.Visible = true;
                    LogOutMenuItem.Visible = true;
                    SetMenuItem();
                    ViewLoad();
                    //LoadData();
                }
            }
        }

        private void CustomerManagerViewLoad()
        {
            pnView.Controls.Clear();
            CustomerForm childForm = new CustomerForm();
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            pnView.Controls.Add(childForm);
            pnView.Tag = childForm;
            childForm.Show();
        }

        private void UpdateMenuItems()
        {
            pnView.Controls.Clear();
            LoginMenuItem.Visible = true;
            AccountManagerMenuItem.Visible = false;
            LogOutMenuItem.Visible = false;
            
        }
        private void ViewLoad()
        {
            if (Constant.User.RoleID == 1)
            {
                AccountManagerViewLoad();
                return;
            }

            if(Constant.User.RoleID == 3)
            {
                SupplierManagerViewLoad();
                return;
            }

            /*if (Constant.User.RoleID == 4)
            {
                ProductViewLoad();
                return;
            }*/
        }
        private void ManagerViewLoad<T>() where T : Form, new()
        {
            pnView.Controls.Clear();
            T childForm = new T();

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            pnView.Controls.Add(childForm);
            pnView.Tag = childForm;

            childForm.Show();
        }

        // Usage for AccountManagerForm
        private void AccountManagerViewLoad()
        {
            ManagerViewLoad<AccountManagerForm>();
        }

        // Usage for SupplierForm
        private void SupplierManagerViewLoad()
        {
            ManagerViewLoad<SupplierForm>();
        }

        private void OrderManagerViewLoad()
        {
            ManagerViewLoad<OrderForm>();
        }
        private void ProductViewLoad()
        {
            ManagerViewLoad<ProductForm>();
        }
        private void LoadData()
        {
            //AccountDataGridView.DataSource = dataContext.Users.ToList();

            //cbxRole.DataSource = dataContext.Roles.ToList();
            //cbxRole.DisplayMember = "RoleName";
            //cbxRole.ValueMember = "RoleId";

        }
    }
}
