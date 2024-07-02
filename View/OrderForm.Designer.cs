namespace BTL_2.View
{
    partial class OrderForm
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
            this.btnInsert = new System.Windows.Forms.Button();
            this.cbxCustomer = new System.Windows.Forms.ComboBox();
            this.cbxTieuchi = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSearchContent = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtListOrder = new System.Windows.Forms.TextBox();
            this.OrderName = new System.Windows.Forms.Label();
            this.txtTotalAmount = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.AccountmanagerSplitContainer = new System.Windows.Forms.SplitContainer();
            this.label8 = new System.Windows.Forms.Label();
            this.lbOrderId = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.listProductOfOrddataGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.AccountmanagerSplitContainer)).BeginInit();
            this.AccountmanagerSplitContainer.Panel1.SuspendLayout();
            this.AccountmanagerSplitContainer.Panel2.SuspendLayout();
            this.AccountmanagerSplitContainer.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listProductOfOrddataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // btnInsert
            // 
            this.btnInsert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInsert.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInsert.Location = new System.Drawing.Point(113, 437);
            this.btnInsert.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(99, 46);
            this.btnInsert.TabIndex = 29;
            this.btnInsert.Text = "Insert";
            this.btnInsert.UseVisualStyleBackColor = true;
            // 
            // cbxCustomer
            // 
            this.cbxCustomer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxCustomer.FormattingEnabled = true;
            this.cbxCustomer.Location = new System.Drawing.Point(155, 300);
            this.cbxCustomer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbxCustomer.Name = "cbxCustomer";
            this.cbxCustomer.Size = new System.Drawing.Size(265, 24);
            this.cbxCustomer.TabIndex = 28;
            // 
            // cbxTieuchi
            // 
            this.cbxTieuchi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxTieuchi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxTieuchi.FormattingEnabled = true;
            this.cbxTieuchi.Items.AddRange(new object[] {
            "ProductName",
            "ID",
            "Price",
            "Unit"});
            this.cbxTieuchi.Location = new System.Drawing.Point(289, 14);
            this.cbxTieuchi.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbxTieuchi.Name = "cbxTieuchi";
            this.cbxTieuchi.Size = new System.Drawing.Size(111, 30);
            this.cbxTieuchi.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(188, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(81, 25);
            this.label7.TabIndex = 5;
            this.label7.Text = "Tiêu chí";
            // 
            // txtSearchContent
            // 
            this.txtSearchContent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchContent.Location = new System.Drawing.Point(96, 16);
            this.txtSearchContent.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSearchContent.Name = "txtSearchContent";
            this.txtSearchContent.Size = new System.Drawing.Size(85, 28);
            this.txtSearchContent.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(16, 300);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 25);
            this.label6.TabIndex = 27;
            this.label6.Text = "Customer";
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(413, 12);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(93, 31);
            this.btnSearch.TabIndex = 7;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // txtListOrder
            // 
            this.txtListOrder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtListOrder.Location = new System.Drawing.Point(155, 15);
            this.txtListOrder.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtListOrder.Multiline = true;
            this.txtListOrder.Name = "txtListOrder";
            this.txtListOrder.Size = new System.Drawing.Size(265, 214);
            this.txtListOrder.TabIndex = 24;
            // 
            // OrderName
            // 
            this.OrderName.AutoSize = true;
            this.OrderName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OrderName.Location = new System.Drawing.Point(16, 21);
            this.OrderName.Name = "OrderName";
            this.OrderName.Size = new System.Drawing.Size(62, 25);
            this.OrderName.TabIndex = 23;
            this.OrderName.Text = "Order";
            // 
            // txtTotalAmount
            // 
            this.txtTotalAmount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTotalAmount.Location = new System.Drawing.Point(155, 249);
            this.txtTotalAmount.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtTotalAmount.Multiline = true;
            this.txtTotalAmount.Name = "txtTotalAmount";
            this.txtTotalAmount.Size = new System.Drawing.Size(265, 29);
            this.txtTotalAmount.TabIndex = 22;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(16, 252);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 25);
            this.label3.TabIndex = 21;
            this.label3.Text = "TotalAmount";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 25);
            this.label4.TabIndex = 3;
            this.label4.Text = "Tìm kiếm";
            // 
            // AccountmanagerSplitContainer
            // 
            this.AccountmanagerSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AccountmanagerSplitContainer.Location = new System.Drawing.Point(-1, -2);
            this.AccountmanagerSplitContainer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.AccountmanagerSplitContainer.Name = "AccountmanagerSplitContainer";
            // 
            // AccountmanagerSplitContainer.Panel1
            // 
            this.AccountmanagerSplitContainer.Panel1.Controls.Add(this.label8);
            this.AccountmanagerSplitContainer.Panel1.Controls.Add(this.lbOrderId);
            this.AccountmanagerSplitContainer.Panel1.Controls.Add(this.dtpDate);
            this.AccountmanagerSplitContainer.Panel1.Controls.Add(this.label5);
            this.AccountmanagerSplitContainer.Panel1.Controls.Add(this.btnDelete);
            this.AccountmanagerSplitContainer.Panel1.Controls.Add(this.btnUpdate);
            this.AccountmanagerSplitContainer.Panel1.Controls.Add(this.btnInsert);
            this.AccountmanagerSplitContainer.Panel1.Controls.Add(this.cbxCustomer);
            this.AccountmanagerSplitContainer.Panel1.Controls.Add(this.label6);
            this.AccountmanagerSplitContainer.Panel1.Controls.Add(this.txtListOrder);
            this.AccountmanagerSplitContainer.Panel1.Controls.Add(this.OrderName);
            this.AccountmanagerSplitContainer.Panel1.Controls.Add(this.txtTotalAmount);
            this.AccountmanagerSplitContainer.Panel1.Controls.Add(this.label3);
            // 
            // AccountmanagerSplitContainer.Panel2
            // 
            this.AccountmanagerSplitContainer.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.AccountmanagerSplitContainer.Panel2.Controls.Add(this.btnSearch);
            this.AccountmanagerSplitContainer.Panel2.Controls.Add(this.cbxTieuchi);
            this.AccountmanagerSplitContainer.Panel2.Controls.Add(this.label7);
            this.AccountmanagerSplitContainer.Panel2.Controls.Add(this.txtSearchContent);
            this.AccountmanagerSplitContainer.Panel2.Controls.Add(this.label4);
            this.AccountmanagerSplitContainer.Panel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AccountmanagerSplitContainer.Size = new System.Drawing.Size(944, 494);
            this.AccountmanagerSplitContainer.SplitterDistance = 437;
            this.AccountmanagerSplitContainer.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(15, 395);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 25);
            this.label8.TabIndex = 40;
            this.label8.Text = "ID:";
            // 
            // lbOrderId
            // 
            this.lbOrderId.Location = new System.Drawing.Point(155, 391);
            this.lbOrderId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbOrderId.Name = "lbOrderId";
            this.lbOrderId.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lbOrderId.Size = new System.Drawing.Size(267, 28);
            this.lbOrderId.TabIndex = 38;
            this.lbOrderId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpDate
            // 
            this.dtpDate.Location = new System.Drawing.Point(155, 346);
            this.dtpDate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(265, 22);
            this.dtpDate.TabIndex = 37;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(15, 346);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 25);
            this.label5.TabIndex = 36;
            this.label5.Text = "DateTime";
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Enabled = false;
            this.btnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(323, 437);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(99, 46);
            this.btnDelete.TabIndex = 31;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.Enabled = false;
            this.btnUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.Location = new System.Drawing.Point(217, 437);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(99, 46);
            this.btnUpdate.TabIndex = 30;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 83F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17F));
            this.tableLayoutPanel1.Controls.Add(this.listProductOfOrddataGridView, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 48);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 23F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 77F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(499, 442);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // listProductOfOrddataGridView
            // 
            this.listProductOfOrddataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listProductOfOrddataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.listProductOfOrddataGridView.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.listProductOfOrddataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.listProductOfOrddataGridView.Location = new System.Drawing.Point(4, 4);
            this.listProductOfOrddataGridView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listProductOfOrddataGridView.Name = "listProductOfOrddataGridView";
            this.listProductOfOrddataGridView.RowHeadersWidth = 51;
            this.listProductOfOrddataGridView.Size = new System.Drawing.Size(491, 434);
            this.listProductOfOrddataGridView.TabIndex = 0;
            // 
            // OrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(943, 494);
            this.Controls.Add(this.AccountmanagerSplitContainer);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "OrderForm";
            this.Text = "Form1";
            this.AccountmanagerSplitContainer.Panel1.ResumeLayout(false);
            this.AccountmanagerSplitContainer.Panel1.PerformLayout();
            this.AccountmanagerSplitContainer.Panel2.ResumeLayout(false);
            this.AccountmanagerSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AccountmanagerSplitContainer)).EndInit();
            this.AccountmanagerSplitContainer.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listProductOfOrddataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.ComboBox cbxCustomer;
        private System.Windows.Forms.ComboBox cbxTieuchi;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSearchContent;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtListOrder;
        private System.Windows.Forms.Label OrderName;
        private System.Windows.Forms.TextBox txtTotalAmount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.SplitContainer AccountmanagerSplitContainer;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lbOrderId;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView listProductOfOrddataGridView;
    }
}