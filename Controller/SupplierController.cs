using BTL_2.Model;
using BTL_2.Shareds;
using BTL_2.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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

        private string Name = "Supplier";
        private AddressController addressController = null;

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
        //public TextBox txtAddress { get; private set; }
        public ComboBox cbxProvince {  get; private set; }
        public ComboBox cbxDistrict {  get; private set; }
        public ComboBox cbxWard {  get; private set; }
        public TextBox txtEmail { get; private set; }

        public ComboBox cbxTieuChi {  get; private set; } 
        public Button btnSearch { get; private set; }
        public TextBox txtSearchContent {  get; private set; }

        public SupplierController(SupplierForm supplierForm, DataGridView supllierdataGridView, Label lbSupplierId, Button btnDelete, Button btnUpdate, Button btnInsert, TextBox txtPhone, TextBox txtSupplierName, ComboBox cbxProvince, ComboBox cbxDistrict, ComboBox cbxWard, TextBox txtEmail, ComboBox cbxTieuChi, Button btnSearch,  TextBox txtSearchContent) 
        {

            SupplierForm = supplierForm;
            SupllierdataGridView = supllierdataGridView;
            this.lbSupplierId = lbSupplierId;
            this.btnDelete = btnDelete;
            this.btnUpdate = btnUpdate;
            this.btnInsert = btnInsert;
            this.txtPhone = txtPhone;
            this.txtSupplierName = txtSupplierName;
            //this.txtAddress = txtAddress;
            this.cbxProvince = cbxProvince;
            this.cbxDistrict = cbxDistrict;
            this.cbxWard = cbxWard;
            this.txtEmail = txtEmail;

            this.cbxTieuChi = cbxTieuChi;
            this.btnSearch = btnSearch;
            this.txtSearchContent = txtSearchContent;
        }

        public void SetEvent()
        {
            SupplierForm.Load += new EventHandler((object sender, EventArgs e) => {
                LoadDataGridView();
                LoadComboxAddres();
                });
            btnInsert.Click += Insert;
            SupllierdataGridView.RowHeaderMouseClick += DataGridView_RowHeaderMouseClick;
            SupllierdataGridView.RowHeaderMouseDoubleClick += DataGridView_RowHeaderMouseDoubleClick;
            btnUpdate.Click += Update;
            btnDelete.Click += Delete;
            btnSearch.Click += Search;
            //txtAddress.Click += new EventHandler((object sender, EventArgs e) => Load_Data());
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
            FuncResult<List<OrderDetail>> orderDetailResult = FuncShares<OrderDetail>.GetAllData();

            if (productsResult.ErrorCode == EnumErrorCode.SUCCESS && orderDetailResult.ErrorCode == EnumErrorCode.SUCCESS)
            {
                var products = productsResult.Data;
                var orderDetails = orderDetailResult.Data;

                // Truy vấn dữ liệu và tính tỷ lệ sản phẩm đã bán
                var query = from p in products
                            join od in orderDetails
                            on p.ProductID equals od.ProductID into productOrderDetails
                            from od in productOrderDetails.DefaultIfEmpty()
                            where p.SupplierID == supplierID
                            select new
                            {
                                p.ProductID,
                                p.ProductName,
                                QuantitySold = od != null ? od.Quantity : 0, // Kiểm tra null cho OrderDetail
                                SoldRatio = p.QuantityInStock > 0 ? ((od != null ? od.Quantity : 0) / (double)p.QuantityInStock) * 100 : 0 // Kiểm tra QuantityInStock > 0
                            };

                // Xóa dữ liệu cũ của biểu đồ nếu có
                chartForm.chartSuppplier.Series.Clear();
                chartForm.chartSuppplier.ChartAreas.Clear();

                // Tạo ChartArea mới
                ChartArea chartArea = new ChartArea();
                chartForm.chartSuppplier.ChartAreas.Add(chartArea);

                // Tạo Series cho biểu đồ
                Series series = new Series("Inventory Sold Ratio"); // Thêm tên cho Series
                series.ChartType = SeriesChartType.Column;
                series.ToolTip = "Quantity Sold: #VALY"; // Hiển thị số lượng QuantitySold khi hover

                // Thêm các điểm dữ liệu vào Series
                foreach (var item in query)
                {
                    DataPoint point = new DataPoint();
                    point.AxisLabel = item.ProductID+" - "+item.ProductName;
                    point.YValues = new double[] { item.SoldRatio };
                    point.ToolTip = $"Quantity Sold: {item.QuantitySold}"; // Hiển thị số lượng QuantitySold khi hover
                    series.Points.Add(point);
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
                LoadDataGridView();
            }
            else
            {
                FuncResult<List<Supplier>> qr = null;
                switch (tieuchi)
                {
                    case "Username":
                        qr = FuncShares<Supplier>.Search(u => u.SupplierName.Contains(content));
                        break;
                    case "Address":
                        qr = FuncShares<Supplier>.Search(u => u.Province.Contains(content));
                        break;
                    case "PhoneNumber":
                        qr = FuncShares<Supplier>.Search(u => u.PhoneNumber.Contains(content));
                        break;
                    case "Email":
                        qr = FuncShares<Supplier>.Search(u => u.Email.Contains(content));
                        break;
                }
                if (qr != null && qr.ErrorCode == EnumErrorCode.SUCCESS)
                {
                    SupllierdataGridView.DataSource = null;
                    SupllierdataGridView.DataSource = qr.Data;
                }
                else if (qr != null)
                {
                    MessageBox.Show(qr.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void Delete(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(lbSupplierId.Text))
            {
                MessageBox.Show(Constants.no_selected, "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = int.Parse(lbSupplierId.Text);
            FuncResult<List<Supplier>> rs = FuncShares<Supplier>.GetAllData();
            switch (rs.ErrorCode)
            {
                case EnumErrorCode.ERROR:
                    MessageBox.Show(rs.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case EnumErrorCode.SUCCESS:
                    var objToDelete = rs.Data
                        .Where(u => u.SupplierID == id)
                        .FirstOrDefault();
                    
                    FuncResult<bool> funcResult = FuncShares<User>.ShowConfirmationMessage();
                    if (objToDelete != null && funcResult.Data)
                    {
                        BackupData(objToDelete);

                        var result = FuncShares<Supplier>.Delete(objToDelete);
                        if (result.Data)
                        {
                            //MessageBox.Show(Constants.delete_success, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataGridView();
                            ClearInputs();
                        }
                        else
                        {
                            MessageBox.Show(result.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (objToDelete == null)
                    {
                        MessageBox.Show(Constants.not_found, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                case EnumErrorCode.FAILED:
                    break;
            }

        }

        private void BackupData(Supplier supplier)
        {
            string backupData = $"ID: {supplier.SupplierID}, name: {supplier.SupplierName}, Address: {supplier.Province}, Email: {supplier.Email}, Phone: {supplier.PhoneNumber}";
            System.IO.File.AppendAllText("backupSupplierData.txt", backupData + Environment.NewLine);
        }

        private void ClearInputs()
        {
            SupllierdataGridView.ClearSelection();
            lbSupplierId.Text = "";
            txtSupplierName.Text = "";
            txtEmail.Text = "";
            //txtAddress.Text = "";
            cbxProvince.SelectedIndex = 1;
            //cbxDistrict.SelectedIndex = 1;
            //cbxWard.SelectedIndex = 1;
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
                        //SupllierdataGridView.ClearSelection();
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
                //txtAddress.Text = row.Cells[2].Value.ToString();
                string province = row.Cells[2].Value.ToString();
                string district = row.Cells[3].Value.ToString();
                string ward = row.Cells[4].Value.ToString();
                addressController.SetComboBoxSelection(province,district,ward);
                txtPhone.Text = row.Cells[5].Value.ToString();
                txtEmail.Text = row.Cells[6].Value.ToString();
            }
            else
            {
                btnInsert.Enabled = true;
                btnDelete.Enabled = false;
                btnUpdate.Enabled = false;
            }
        }

        private void Update(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            int id = int.Parse(lbSupplierId.Text);
            var name = txtSupplierName.Text;
            //var address = txtAddress.Text;
            string province = cbxProvince.Text;
            string district = cbxDistrict.Text;
            string ward = cbxWard.Text;
            var phone = txtPhone.Text;
            var email = txtEmail.Text;

            var supplierToUpdate = dataContext.Suppliers.FirstOrDefault(u => u.SupplierID == id);

            if (supplierToUpdate != null)
            {
                supplierToUpdate.SupplierName = name;
                supplierToUpdate.Province = province;
                supplierToUpdate.District = district;
                supplierToUpdate.Ward = ward;
                supplierToUpdate.Email = email;
                supplierToUpdate.PhoneNumber = phone;

                dataContext.SubmitChanges();
                string str = string.Format(Constants.update_success, Name);
                MessageBox.Show(str, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadDataGridView();
            }
            else
            {
                MessageBox.Show(Constants.not_found, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void Insert(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            var name = txtSupplierName.Text;
            //var address = txtAddress.Text;
            string province = cbxProvince.Text;
            string district = cbxDistrict.Text;
            string ward = cbxWard.Text;
            var phone = txtPhone.Text;
            var email = txtEmail.Text;

            Supplier supplier = new Supplier();
            supplier.SupplierName = name;
            supplier.Province = province;
            supplier.District = district;
            supplier.Ward = ward;
            supplier.Email = email;
            supplier.PhoneNumber = phone;
            dataContext.Suppliers.InsertOnSubmit(supplier);
            dataContext.SubmitChanges();

            LoadDataGridView();
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtSupplierName.Text))
            {
                MessageBox.Show("Supplier Name is required.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(cbxProvince.Text))
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

        private void LoadDataGridView()
        {
            SupllierdataGridView.DataSource = dataContext.Suppliers.ToList();
        }
        private void LoadComboxAddres()
        {
            addressController = new AddressController(cbxProvince, cbxDistrict, cbxWard);
        }
    }
}
