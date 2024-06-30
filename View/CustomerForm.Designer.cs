namespace BTL_2.View
{
    partial class CustomerForm
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
            this.lbId = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnInsert = new System.Windows.Forms.Button();
            this.cbxTieuchi = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSearchContent = new System.Windows.Forms.TextBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.CustomerDataGridView = new System.Windows.Forms.DataGridView();
            this.CustomerName = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.CustomerSplitContainer = new System.Windows.Forms.SplitContainer();
            this.cbxDistrict = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbxWard = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbxProvince = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.CustomerDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CustomerSplitContainer)).BeginInit();
            this.CustomerSplitContainer.Panel1.SuspendLayout();
            this.CustomerSplitContainer.Panel2.SuspendLayout();
            this.CustomerSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbId
            // 
            this.lbId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbId.Location = new System.Drawing.Point(187, 31);
            this.lbId.Name = "lbId";
            this.lbId.Size = new System.Drawing.Size(123, 30);
            this.lbId.TabIndex = 32;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Enabled = false;
            this.btnDelete.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(243, 402);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(97, 59);
            this.btnDelete.TabIndex = 31;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.Enabled = false;
            this.btnUpdate.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.Location = new System.Drawing.Point(137, 402);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(97, 59);
            this.btnUpdate.TabIndex = 30;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            // 
            // btnInsert
            // 
            this.btnInsert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInsert.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInsert.Location = new System.Drawing.Point(33, 402);
            this.btnInsert.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(97, 59);
            this.btnInsert.TabIndex = 29;
            this.btnInsert.Text = "Insert";
            this.btnInsert.UseVisualStyleBackColor = true;
            // 
            // cbxTieuchi
            // 
            this.cbxTieuchi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxTieuchi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxTieuchi.FormattingEnabled = true;
            this.cbxTieuchi.Items.AddRange(new object[] {
            "Name",
            "Address",
            "Email",
            "PhoneNumber"});
            this.cbxTieuchi.Location = new System.Drawing.Point(313, 14);
            this.cbxTieuchi.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbxTieuchi.Name = "cbxTieuchi";
            this.cbxTieuchi.Size = new System.Drawing.Size(153, 29);
            this.cbxTieuchi.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(228, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 23);
            this.label7.TabIndex = 5;
            this.label7.Text = "Tiêu chí";
            // 
            // txtSearchContent
            // 
            this.txtSearchContent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchContent.Location = new System.Drawing.Point(99, 14);
            this.txtSearchContent.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSearchContent.Name = "txtSearchContent";
            this.txtSearchContent.Size = new System.Drawing.Size(124, 28);
            this.txtSearchContent.TabIndex = 4;
            // 
            // txtPhone
            // 
            this.txtPhone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPhone.Location = new System.Drawing.Point(189, 316);
            this.txtPhone.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtPhone.Multiline = true;
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(121, 29);
            this.txtPhone.TabIndex = 26;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(47, 315);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(133, 23);
            this.label5.TabIndex = 25;
            this.label5.Text = "PhoneNumber";
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(476, 14);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(92, 31);
            this.btnSearch.TabIndex = 7;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(189, 78);
            this.txtName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtName.Multiline = true;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(121, 29);
            this.txtName.TabIndex = 24;
            // 
            // CustomerDataGridView
            // 
            this.CustomerDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CustomerDataGridView.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.CustomerDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CustomerDataGridView.GridColor = System.Drawing.SystemColors.ButtonHighlight;
            this.CustomerDataGridView.Location = new System.Drawing.Point(3, 53);
            this.CustomerDataGridView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CustomerDataGridView.Name = "CustomerDataGridView";
            this.CustomerDataGridView.ReadOnly = true;
            this.CustomerDataGridView.RowHeadersWidth = 51;
            this.CustomerDataGridView.RowTemplate.Height = 24;
            this.CustomerDataGridView.Size = new System.Drawing.Size(592, 437);
            this.CustomerDataGridView.TabIndex = 2;
            // 
            // CustomerName
            // 
            this.CustomerName.AutoSize = true;
            this.CustomerName.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CustomerName.Location = new System.Drawing.Point(47, 78);
            this.CustomerName.Name = "CustomerName";
            this.CustomerName.Size = new System.Drawing.Size(61, 23);
            this.CustomerName.TabIndex = 23;
            this.CustomerName.Text = "Name";
            // 
            // txtEmail
            // 
            this.txtEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEmail.Location = new System.Drawing.Point(189, 268);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtEmail.Multiline = true;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(121, 29);
            this.txtEmail.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(47, 268);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 23);
            this.label2.TabIndex = 19;
            this.label2.Text = "Email";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 23);
            this.label4.TabIndex = 3;
            this.label4.Text = "Tìm kiếm";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(47, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 23);
            this.label1.TabIndex = 18;
            this.label1.Text = "ID";
            // 
            // CustomerSplitContainer
            // 
            this.CustomerSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CustomerSplitContainer.Location = new System.Drawing.Point(11, 9);
            this.CustomerSplitContainer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CustomerSplitContainer.Name = "CustomerSplitContainer";
            // 
            // CustomerSplitContainer.Panel1
            // 
            this.CustomerSplitContainer.Panel1.Controls.Add(this.cbxDistrict);
            this.CustomerSplitContainer.Panel1.Controls.Add(this.label8);
            this.CustomerSplitContainer.Panel1.Controls.Add(this.cbxWard);
            this.CustomerSplitContainer.Panel1.Controls.Add(this.label6);
            this.CustomerSplitContainer.Panel1.Controls.Add(this.cbxProvince);
            this.CustomerSplitContainer.Panel1.Controls.Add(this.label3);
            this.CustomerSplitContainer.Panel1.Controls.Add(this.lbId);
            this.CustomerSplitContainer.Panel1.Controls.Add(this.btnDelete);
            this.CustomerSplitContainer.Panel1.Controls.Add(this.btnUpdate);
            this.CustomerSplitContainer.Panel1.Controls.Add(this.btnInsert);
            this.CustomerSplitContainer.Panel1.Controls.Add(this.txtPhone);
            this.CustomerSplitContainer.Panel1.Controls.Add(this.label5);
            this.CustomerSplitContainer.Panel1.Controls.Add(this.txtName);
            this.CustomerSplitContainer.Panel1.Controls.Add(this.CustomerName);
            this.CustomerSplitContainer.Panel1.Controls.Add(this.txtEmail);
            this.CustomerSplitContainer.Panel1.Controls.Add(this.label2);
            this.CustomerSplitContainer.Panel1.Controls.Add(this.label1);
            // 
            // CustomerSplitContainer.Panel2
            // 
            this.CustomerSplitContainer.Panel2.Controls.Add(this.btnSearch);
            this.CustomerSplitContainer.Panel2.Controls.Add(this.cbxTieuchi);
            this.CustomerSplitContainer.Panel2.Controls.Add(this.label7);
            this.CustomerSplitContainer.Panel2.Controls.Add(this.txtSearchContent);
            this.CustomerSplitContainer.Panel2.Controls.Add(this.label4);
            this.CustomerSplitContainer.Panel2.Controls.Add(this.CustomerDataGridView);
            this.CustomerSplitContainer.Panel2.Font = new System.Drawing.Font("Arial", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CustomerSplitContainer.Size = new System.Drawing.Size(952, 494);
            this.CustomerSplitContainer.SplitterDistance = 368;
            this.CustomerSplitContainer.TabIndex = 1;
            // 
            // cbxDistrict
            // 
            this.cbxDistrict.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxDistrict.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxDistrict.FormattingEnabled = true;
            this.cbxDistrict.Location = new System.Drawing.Point(190, 179);
            this.cbxDistrict.Name = "cbxDistrict";
            this.cbxDistrict.Size = new System.Drawing.Size(120, 24);
            this.cbxDistrict.TabIndex = 60;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(42, 175);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(70, 25);
            this.label8.TabIndex = 59;
            this.label8.Text = "District";
            // 
            // cbxWard
            // 
            this.cbxWard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxWard.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxWard.FormattingEnabled = true;
            this.cbxWard.Location = new System.Drawing.Point(190, 225);
            this.cbxWard.Name = "cbxWard";
            this.cbxWard.Size = new System.Drawing.Size(120, 24);
            this.cbxWard.TabIndex = 58;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(42, 221);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 25);
            this.label6.TabIndex = 57;
            this.label6.Text = "Ward";
            // 
            // cbxProvince
            // 
            this.cbxProvince.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxProvince.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxProvince.FormattingEnabled = true;
            this.cbxProvince.Location = new System.Drawing.Point(190, 132);
            this.cbxProvince.Name = "cbxProvince";
            this.cbxProvince.Size = new System.Drawing.Size(120, 24);
            this.cbxProvince.TabIndex = 56;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(42, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 25);
            this.label3.TabIndex = 55;
            this.label3.Text = "Province";
            // 
            // CustomerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(952, 494);
            this.Controls.Add(this.CustomerSplitContainer);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "CustomerForm";
            this.Text = "CustomerForm";
            ((System.ComponentModel.ISupportInitialize)(this.CustomerDataGridView)).EndInit();
            this.CustomerSplitContainer.Panel1.ResumeLayout(false);
            this.CustomerSplitContainer.Panel1.PerformLayout();
            this.CustomerSplitContainer.Panel2.ResumeLayout(false);
            this.CustomerSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CustomerSplitContainer)).EndInit();
            this.CustomerSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lbId;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.ComboBox cbxTieuchi;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSearchContent;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.DataGridView CustomerDataGridView;
        private System.Windows.Forms.Label CustomerName;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer CustomerSplitContainer;
        private System.Windows.Forms.ComboBox cbxDistrict;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbxWard;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbxProvince;
        private System.Windows.Forms.Label label3;
    }
}