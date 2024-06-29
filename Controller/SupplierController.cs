using BTL_2.Model;
using BTL_2.Shareds;
using BTL_2.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace BTL_2.Controller
{
    public class SupplierController
    {
        private DatabaseDataContext dataContext = new DatabaseDataContext();
        private int clickCount = 0; // Biến đếm số lần click vào hàng
        private int lastClickedRowIndex = -1; // Index của hàng được click lần cuối
        public SupplierForm SupplierForm { get; private set; }
        public DataGridView SupllierdataGridView { get; private set; }
        public Label lbSupplierId { get; private set; }
        public Button btnDelete { get; private set; }
        public Button btnUpdate { get; private set; }
        public Button btnInsert { get; private set; }
        public TextBox txtPhone { get; private set; }
        public TextBox txtSupplierName { get; private set; }
        public TextBox txtAddress { get; private set; }
        public TextBox txtEmail { get; private set; }

        public ComboBox cbxTieuChi {  get; private set; } 
        public Button btnSearch { get; private set; }
        public TextBox txtSearchContent {  get; private set; }

        public SupplierController(SupplierForm supplierForm, DataGridView supllierdataGridView, Label lbSupplierId, Button btnDelete, Button btnUpdate, Button btnInsert, TextBox txtPhone, TextBox txtSupplierName, TextBox txtAddress, TextBox txtEmail, ComboBox cbxTieuChi, Button btnSearch,  TextBox txtSearchContent) 
        {

            SupplierForm = supplierForm;
            SupllierdataGridView = supllierdataGridView;
            this.lbSupplierId = lbSupplierId;
            this.btnDelete = btnDelete;
            this.btnUpdate = btnUpdate;
            this.btnInsert = btnInsert;
            this.txtPhone = txtPhone;
            this.txtSupplierName = txtSupplierName;
            this.txtAddress = txtAddress;
            this.txtEmail = txtEmail;

            this.cbxTieuChi = cbxTieuChi;
            this.btnSearch = btnSearch;
            this.txtSearchContent = txtSearchContent;
        }

        /*public SupplierController(SupplierForm supplierForm, DataGridView supllierdataGridView, Label lbSupplierId, Button btnDelete, Button btnUpdate, Button btnInsert, TextBox txtPhone, TextBox txtSupplierName, TextBox txtAddress, TextBox txtEmail)
        {
            SupplierForm = supplierForm;
            SupllierdataGridView = supllierdataGridView;
            this.lbSupplierId = lbSupplierId;
            this.btnDelete = btnDelete;
            this.btnUpdate = btnUpdate;
            this.btnInsert = btnInsert;
            this.txtPhone = txtPhone;
            this.txtSupplierName = txtSupplierName;
            this.txtAddress = txtAddress;
            this.txtEmail = txtEmail;
        }*/

        public void SetEvent()
        {
            SupplierForm.Load += new EventHandler((object sender, EventArgs e) => LoadData());
            btnInsert.Click += InsertSupplier;
            SupllierdataGridView.RowHeaderMouseClick += DataGridView_RowHeaderMouseClick;
            SupllierdataGridView.RowHeaderMouseDoubleClick += DataGridView_RowHeaderMouseDoubleClick;
            btnUpdate.Click += UpdateSupplier;
            btnDelete.Click += DeleteSupplier;
            btnSearch.Click += Search;
           
        }

        private void DataGridView_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < SupllierdataGridView.Rows.Count)
            {
                // Lấy SupplierID từ hàng được double-click
                int supplierID = Convert.ToInt32(SupllierdataGridView.Rows[e.RowIndex].Cells["SupplierID"].Value);

                // Vẽ biểu đồ tỉ lệ tồn kho của các sản phẩm của nhà cung cấp có SupplierID tương ứng
                ChartSupplier chartForm = new ChartSupplier();
                DrawInventoryRatioChart(supplierID, chartForm);
                chartForm.ShowDialog();
            }
        }
        private void DrawInventoryRatioChart(int supplierID, ChartSupplier chartForm)
        {
            // Lấy dữ liệu từ table Products và Inventories dựa trên SupplierID
            FuncResult<List<Product>> productsResult = FuncShares<Product>.GetAllData();
            FuncResult<List<Inventory>> inventoriesResult = FuncShares<Inventory>.GetAllData();

            if (productsResult.ErrorCode == EnumErrorCode.SUCCESS && inventoriesResult.ErrorCode == EnumErrorCode.SUCCESS)
            {
                var products = productsResult.Data;
                var inventories = inventoriesResult.Data;

                // Lọc và tính toán tỉ lệ tồn kho
                var query = from product in products
                            join inventory in inventories on product.ProductID equals inventory.ProductID
                            where product.SupplierID == supplierID
                            select new
                            {
                                ProductName = product.ProductName,
                                InventoryRatio = (double)inventory.Quantity / product.QuantityInStock
                            };
                // Xóa dữ liệu cũ của biểu đồ nếu có
                chartForm.chartSuppplier.Series.Clear();
                chartForm.chartSuppplier.ChartAreas.Clear();

                // Tạo ChartArea mới
                ChartArea chartArea = new ChartArea();
                chartForm.chartSuppplier.ChartAreas.Add(chartArea);

                // Tạo Series cho biểu đồ
                Series series = new Series();
                series.ChartType = SeriesChartType.Column;

                // Thêm các điểm dữ liệu vào Series
                foreach (var item in query)
                {
                    series.Points.AddXY(item.ProductName, item.InventoryRatio * 100); // Chuyển sang phần trăm
                }

                // Thêm Series vào Chart
                chartForm.chartSuppplier.Series.Add(series);

                // Thiết lập các thuộc tính của biểu đồ
                chartArea.AxisX.Interval = 1; // Khoảng cách giữa các cột
                chartArea.AxisX.LabelStyle.Angle = -45; // Góc hiển thị của nhãn trên trục X
                chartArea.AxisX.MajorGrid.Enabled = false; // Tắt lưới chính trên trục X

                chartArea.AxisY.Title = "Inventory Ratio (%)"; // Nhãn trục Y
                chartArea.AxisY.Minimum = 0; // Giá trị tối thiểu trên trục Y
            }
            else
            {
                MessageBox.Show("Failed to load data from database.");
            }
        }

        public void Search(object obj, EventArgs e)
        {
            var content = txtSearchContent.Text;
            var tieuchi = cbxTieuChi.Text;
            //string str = null; Console.WriteLine(str.Length);

            if (string.IsNullOrEmpty(content))
            {
                LoadData();
            }
            else
            {
                List<Supplier> qr = null;
                switch (tieuchi)
                {
                    case "Username":
                        qr = dataContext.Suppliers.Where(u => u.SupplierName.Contains(content)).ToList();
                        break;
                    case "Address":
                        qr = dataContext.Suppliers.Where(u => u.Address.Contains(content)).ToList();
                        break;
                    case "PhoneNumber":
                        qr = dataContext.Suppliers.Where(u => u.PhoneNumber.Contains(content)).ToList();
                        break;
                    case "Email":
                        qr = dataContext.Suppliers.Where(u => u.Email.Contains(content)).ToList();
                        break;
                }
                SupllierdataGridView.DataSource = null;
                SupllierdataGridView.DataSource = qr;
            }
        }
        private void DeleteSupplier(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(lbSupplierId.Text))
            {
                MessageBox.Show("No supplier selected for deletion.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = int.Parse(lbSupplierId.Text);
            var supplierToDelete = dataContext.Suppliers.FirstOrDefault(u => u.SupplierID == id);

            if (supplierToDelete != null && ShowConfirmationMessage())
            {
                BackupUserData(supplierToDelete);

                dataContext.Suppliers.DeleteOnSubmit(supplierToDelete);
                dataContext.SubmitChanges();
                MessageBox.Show("Supplier deleted successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadData();
                ClearInputs();
            }
            if (supplierToDelete == null)
            {
                MessageBox.Show("Supplier not found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BackupUserData(Supplier supplier)
        {
            string backupData = $"ID: {supplier.SupplierID}, name: {supplier.SupplierName}, Address: {supplier.Address}, Email: {supplier.Email}, Phone: {supplier.PhoneNumber}";
            System.IO.File.AppendAllText("backupSupplierData.txt", backupData + Environment.NewLine);
        }

        private void ClearInputs()
        {
            SupllierdataGridView.ClearSelection();
            lbSupplierId.Text = "";
            txtSupplierName.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";
            txtPhone.Text = "";
            btnInsert.Enabled = true;
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
        }

        private void DataGridView_RowHeaderMouseClick(object sender, EventArgs e)
        {
            if (SupllierdataGridView.SelectedRows.Count > 0)
            {
                btnInsert.Enabled = false;
                btnDelete.Enabled = true;
                btnUpdate.Enabled = true;

                DataGridViewRow row = SupllierdataGridView.SelectedRows[0];
                int currentRowIndex = row.Index;

                if (currentRowIndex == lastClickedRowIndex)
                {
                    clickCount++;

                    if (clickCount == 2)
                    {
                        SupllierdataGridView.ClearSelection();
                        ClearInputs();
                        clickCount = 0;
                        lastClickedRowIndex = -1;
                        return;
                    }
                }
                else
                {
                    clickCount = 1;
                }

                lastClickedRowIndex = currentRowIndex;


                lbSupplierId.Text = row.Cells[0].Value.ToString();
                txtSupplierName.Text = row.Cells[1].Value.ToString();
                txtAddress.Text = row.Cells[2].Value.ToString();
                txtPhone.Text = row.Cells[3].Value.ToString();
                txtEmail.Text = row.Cells[4].Value.ToString();
            }
            else
            {
                btnInsert.Enabled = true;
                btnDelete.Enabled = false;
                btnUpdate.Enabled = false;
            }
        }

        private void UpdateSupplier(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            int id = int.Parse(lbSupplierId.Text);
            var name = txtSupplierName.Text;
            var address = txtAddress.Text;
            var phone = txtPhone.Text;
            var email = txtEmail.Text;

            var supplierToUpdate = dataContext.Suppliers.FirstOrDefault(u => u.SupplierID == id);

            if (supplierToUpdate != null)
            {
                supplierToUpdate.SupplierName = name;
                supplierToUpdate.Address = address;
                supplierToUpdate.Email = email;
                supplierToUpdate.PhoneNumber = phone;

                dataContext.SubmitChanges();
                MessageBox.Show("Supplier updated successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadData();
            }
            else
            {
                MessageBox.Show("Supplier not found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void InsertSupplier(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            var name = txtSupplierName.Text;
            var address = txtAddress.Text;
            var phone = txtPhone.Text;
            var email = txtEmail.Text;

            Supplier supplier = new Supplier();
            supplier.SupplierName = name;
            supplier.Address = address;
            supplier.Email = email;
            supplier.PhoneNumber = phone;
            dataContext.Suppliers.InsertOnSubmit(supplier);
            dataContext.SubmitChanges();

            LoadData();
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtSupplierName.Text))
            {
                MessageBox.Show("Supplier Name is required.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Address is required.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("A valid Email is required.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text) || !IsValidPhone(txtPhone.Text))
            {
                MessageBox.Show("A valid Phone Number is required.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhone(string phone)
        {
            return Regex.IsMatch(phone, @"^\d{10}$");
        }

        private void LoadData()
        {
            SupllierdataGridView.DataSource = dataContext.Suppliers.ToList();
        }

        private bool ShowConfirmationMessage()
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            return result == DialogResult.Yes;
        }
    }
}
