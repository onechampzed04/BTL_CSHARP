namespace BTL_2.View
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.AccountRoleMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoginMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AccountManagerMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LogOutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DataManagerMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ProductMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SuppliersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CustomerMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OrderMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ThongkeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnView = new System.Windows.Forms.Panel();
            this.BillMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AccountRoleMenuItem,
            this.DataManagerMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(746, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // AccountRoleMenuItem
            // 
            this.AccountRoleMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoginMenuItem,
            this.AccountManagerMenuItem,
            this.LogOutMenuItem});
            this.AccountRoleMenuItem.Name = "AccountRoleMenuItem";
            this.AccountRoleMenuItem.Size = new System.Drawing.Size(129, 20);
            this.AccountRoleMenuItem.Text = "Chức năng tài khoản";
            // 
            // LoginMenuItem
            // 
            this.LoginMenuItem.Name = "LoginMenuItem";
            this.LoginMenuItem.Size = new System.Drawing.Size(170, 22);
            this.LoginMenuItem.Text = "Đăng nhập";
            // 
            // AccountManagerMenuItem
            // 
            this.AccountManagerMenuItem.Name = "AccountManagerMenuItem";
            this.AccountManagerMenuItem.Size = new System.Drawing.Size(170, 22);
            this.AccountManagerMenuItem.Text = "Quản lý tài khoản ";
            this.AccountManagerMenuItem.Visible = false;
            // 
            // LogOutMenuItem
            // 
            this.LogOutMenuItem.Name = "LogOutMenuItem";
            this.LogOutMenuItem.Size = new System.Drawing.Size(170, 22);
            this.LogOutMenuItem.Text = "Đăng xuất";
            this.LogOutMenuItem.Visible = false;
            // 
            // DataManagerMenuItem
            // 
            this.DataManagerMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ProductMenuItem,
            this.SuppliersMenuItem,
            this.CustomerMenuItem,
            this.OrderMenuItem,
            this.ThongkeMenuItem,
            this.BillMenuItem});
            this.DataManagerMenuItem.Name = "DataManagerMenuItem";
            this.DataManagerMenuItem.Size = new System.Drawing.Size(99, 20);
            this.DataManagerMenuItem.Text = "Quản lý dữ liệu";
            // 
            // ProductMenuItem
            // 
            this.ProductMenuItem.Name = "ProductMenuItem";
            this.ProductMenuItem.Size = new System.Drawing.Size(221, 22);
            this.ProductMenuItem.Text = "Quản lý sản phẩm";
            this.ProductMenuItem.Visible = false;
            // 
            // SuppliersMenuItem
            // 
            this.SuppliersMenuItem.Name = "SuppliersMenuItem";
            this.SuppliersMenuItem.Size = new System.Drawing.Size(221, 22);
            this.SuppliersMenuItem.Text = "Quản lý nhà cung cấp";
            this.SuppliersMenuItem.Visible = false;
            // 
            // CustomerMenuItem
            // 
            this.CustomerMenuItem.Name = "CustomerMenuItem";
            this.CustomerMenuItem.Size = new System.Drawing.Size(221, 22);
            this.CustomerMenuItem.Text = "Quản lý khách hàng";
            this.CustomerMenuItem.Visible = false;
            // 
            // OrderMenuItem
            // 
            this.OrderMenuItem.Name = "OrderMenuItem";
            this.OrderMenuItem.Size = new System.Drawing.Size(221, 22);
            this.OrderMenuItem.Text = "Quản lý đơn hàng";
            this.OrderMenuItem.Visible = false;
            // 
            // ThongkeMenuItem
            // 
            this.ThongkeMenuItem.Name = "ThongkeMenuItem";
            this.ThongkeMenuItem.Size = new System.Drawing.Size(221, 22);
            this.ThongkeMenuItem.Text = "Thống kê";
            this.ThongkeMenuItem.Visible = false;
            // 
            // pnView
            // 
            this.pnView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnView.Location = new System.Drawing.Point(0, 25);
            this.pnView.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnView.Name = "pnView";
            this.pnView.Size = new System.Drawing.Size(746, 353);
            this.pnView.TabIndex = 1;
            // 
            // BillMenuItem
            // 
            this.BillMenuItem.Name = "BillMenuItem";
            this.BillMenuItem.Size = new System.Drawing.Size(221, 22);
            this.BillMenuItem.Text = "Manager_Quản lý đơn hàng";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(746, 375);
            this.Controls.Add(this.pnView);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem AccountRoleMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LoginMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AccountManagerMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DataManagerMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LogOutMenuItem;
        private System.Windows.Forms.Panel pnView;
        private System.Windows.Forms.ToolStripMenuItem ProductMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SuppliersMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CustomerMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OrderMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ThongkeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem BillMenuItem;
    }
}