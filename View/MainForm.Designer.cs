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
            this.pnView = new System.Windows.Forms.Panel();
            this.ProductMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SuppliersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CustomerMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OrderMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.InventoryMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.menuStrip1.Size = new System.Drawing.Size(995, 28);
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
            this.AccountRoleMenuItem.Size = new System.Drawing.Size(158, 24);
            this.AccountRoleMenuItem.Text = "Chức năng tài khoản";
            // 
            // LoginMenuItem
            // 
            this.LoginMenuItem.Name = "LoginMenuItem";
            this.LoginMenuItem.Size = new System.Drawing.Size(211, 26);
            this.LoginMenuItem.Text = "Đăng nhập";
            // 
            // AccountManagerMenuItem
            // 
            this.AccountManagerMenuItem.Name = "AccountManagerMenuItem";
            this.AccountManagerMenuItem.Size = new System.Drawing.Size(211, 26);
            this.AccountManagerMenuItem.Text = "Quản lý tài khoản ";
            this.AccountManagerMenuItem.Visible = false;
            // 
            // LogOutMenuItem
            // 
            this.LogOutMenuItem.Name = "LogOutMenuItem";
            this.LogOutMenuItem.Size = new System.Drawing.Size(211, 26);
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
            this.InventoryMenuItem});
            this.DataManagerMenuItem.Name = "DataManagerMenuItem";
            this.DataManagerMenuItem.Size = new System.Drawing.Size(123, 24);
            this.DataManagerMenuItem.Text = "Quản lý dữ liệu";
            // 
            // pnView
            // 
            this.pnView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnView.Location = new System.Drawing.Point(0, 31);
            this.pnView.Name = "pnView";
            this.pnView.Size = new System.Drawing.Size(995, 434);
            this.pnView.TabIndex = 1;
            // 
            // ProductMenuItem
            // 
            this.ProductMenuItem.Name = "ProductMenuItem";
            this.ProductMenuItem.Size = new System.Drawing.Size(234, 26);
            this.ProductMenuItem.Text = "Quản lý sản phẩm";
            // 
            // SuppliersMenuItem
            // 
            this.SuppliersMenuItem.Name = "SuppliersMenuItem";
            this.SuppliersMenuItem.Size = new System.Drawing.Size(234, 26);
            this.SuppliersMenuItem.Text = "Quản lý nhà cung cấp";
            // 
            // CustomerMenuItem
            // 
            this.CustomerMenuItem.Name = "CustomerMenuItem";
            this.CustomerMenuItem.Size = new System.Drawing.Size(234, 26);
            this.CustomerMenuItem.Text = "Quản lý khách hàng";
            // 
            // OrderMenuItem
            // 
            this.OrderMenuItem.Name = "OrderMenuItem";
            this.OrderMenuItem.Size = new System.Drawing.Size(234, 26);
            this.OrderMenuItem.Text = "Quản lý đơn hàng";
            // 
            // InventoryMenuItem
            // 
            this.InventoryMenuItem.Name = "InventoryMenuItem";
            this.InventoryMenuItem.Size = new System.Drawing.Size(234, 26);
            this.InventoryMenuItem.Text = "Quản lý kho hàng";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(995, 462);
            this.Controls.Add(this.pnView);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
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
        private System.Windows.Forms.ToolStripMenuItem InventoryMenuItem;
    }
}