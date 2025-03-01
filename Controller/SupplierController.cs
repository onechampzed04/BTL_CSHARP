﻿using BTL_2.Model;
using BTL_2.Shareds;
using BTL_2.View;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

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

            FuncShares<Object>.SetEnableButton(this.btnInsert, this.btnUpdate, this.btnDelete);

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
            // Lấy dữ liệu từ table Products và OrderDetails dựa trên SupplierID
            FuncResult<List<Product>> productsResult = FuncShares<Product>.GetAllData();
            FuncResult<List<OrderDetail>> orderDetailResult = FuncShares<OrderDetail>.GetAllData();

            if (productsResult.ErrorCode == EnumErrorCode.SUCCESS && orderDetailResult.ErrorCode == EnumErrorCode.SUCCESS)
            {
                var products = productsResult.Data;
                var orderDetails = orderDetailResult.Data;

                // Truy vấn dữ liệu và tính tỷ lệ sản phẩm đã bán
                var query = from p in products
                            join od in orderDetails on p.ProductID equals od.ProductID into productOrderDetails
                            from od in productOrderDetails.DefaultIfEmpty()
                            where p.SupplierID == supplierID
                            group new { p, od } by new { p.ProductID, p.ProductName, p.QuantityInStock } into grouped
                            let totalOrderedQuantity = grouped.Sum(x => x.od != null ? x.od.Quantity : 0)
                            select new
                            {
                                ProductID = grouped.Key.ProductID,
                                ProductName = grouped.Key.ProductName,
                                QuantityInStock = grouped.Key.QuantityInStock,
                                TotalOrderedQuantity = totalOrderedQuantity,
                                OrderRatio = totalOrderedQuantity * 1.0 / (grouped.Key.QuantityInStock + totalOrderedQuantity) * 100 // Đổi sang phần trăm
                            };

                // Xóa dữ liệu cũ của biểu đồ nếu có
                chartForm.chartSuppplier.Series.Clear();
                chartForm.chartSuppplier.ChartAreas.Clear();

                // Tạo ChartArea mới
                ChartArea chartArea = new ChartArea();
                chartForm.chartSuppplier.ChartAreas.Add(chartArea);

                // Tạo Series cho biểu đồ
                Series series = new Series("Inventory Sold Ratio");
                // xác định loại biểu đồ sẽ được sử dụng.
                series.ChartType = SeriesChartType.Column;
                //#VALY sẽ được thay thế bằng giá trị Y của điểm dữ liệu tương ứng. %
                series.ToolTip = "Order Ratio: #VALY%"; // Hiển thị tỷ lệ bán hàng khi hover

                // Thêm các điểm dữ liệu vào Series
                foreach (var item in query)
                {
                    DataPoint point = new DataPoint();
                    point.AxisLabel = $"{item.ProductID} - {item.ProductName}";
                    point.YValues = new double[] { item.OrderRatio };
                    point.ToolTip = $"Quantity Sold: {item.TotalOrderedQuantity}"; // Hiển thị số lượng QuantitySold khi hover
                    series.Points.Add(point);
                }

                // Thêm Series vào Chart
                chartForm.chartSuppplier.Series.Add(series);

                // Thiết lập các thuộc tính của biểu đồ
                chartArea.AxisX.Interval = 1; // Khoảng cách giữa các cột
                chartArea.AxisX.LabelStyle.Angle = -45; // Góc hiển thị của nhãn trên trục X
                chartArea.AxisX.MajorGrid.Enabled = false; // Tắt lưới chính trên trục X

                chartArea.AxisY.Title = "Order Ratio (%)"; // Nhãn trục Y
                chartArea.AxisY.Minimum = 0; // Giá trị tối thiểu trên trục Y
                chartArea.AxisY.Maximum = 100; // Giá trị tối đa trên trục Y (vì tỷ lệ là phần trăm)
            }
            else
            {
                MessageBox.Show(productsResult.ErrorDesc);
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

            if (!int.TryParse(lbSupplierId.Text, out int id))
            {
                MessageBox.Show("Invalid Supplier ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FuncResult<List<Supplier>> searchResult = FuncShares<Supplier>.Search(u => u.SupplierID == id);
            if (searchResult.ErrorCode == EnumErrorCode.ERROR)
            {
                MessageBox.Show(searchResult.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (searchResult.Data.Count == 0)
            {
                MessageBox.Show(Constants.not_found, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            FuncResult<bool> confirmationResult = FuncShares<Supplier>.ShowConfirmationMessage();
            if (!confirmationResult.Data)
            {
                return;
            }

            Supplier obj = searchResult.Data[0];

            var supplierResult = FuncShares<Supplier>.GetAllData();
            if (supplierResult.ErrorCode == EnumErrorCode.ERROR)
            {
                MessageBox.Show(supplierResult.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var productResult = FuncShares<Product>.GetAllData();
            if (productResult.ErrorCode == EnumErrorCode.ERROR)
            {
                MessageBox.Show(productResult.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var orderDetailResult = FuncShares<OrderDetail>.GetAllData();
            if (orderDetailResult.ErrorCode == EnumErrorCode.ERROR)
            {
                MessageBox.Show(orderDetailResult.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var orderResult = FuncShares<Order>.GetAllData();
            if (orderResult.ErrorCode == EnumErrorCode.ERROR)
            {
                MessageBox.Show(orderResult.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<Supplier> list_supplier = supplierResult.Data;
            List<Product> list_product = productResult.Data;
            List<OrderDetail> list_orderdetail = orderDetailResult.Data;
            List<Order> list_order = orderResult.Data;

            var query = from s in list_supplier
                        join p in list_product on s.SupplierID equals p.SupplierID
                        join od in list_orderdetail on p.ProductID equals od.ProductID
                        join o in list_order on od.OrderID equals o.OrderID
                        where s.SupplierID == obj.SupplierID
                        select new
                        {
                            Supplier = s,
                            Product = p,
                            Order = o,
                            OrderDetail = od
                        };

            var itemsToDelete = query.ToList();
            if (itemsToDelete.Count == 0)
            {
                var deleteSupplierResult = FuncShares<Supplier>.Delete(obj);
                if (deleteSupplierResult.ErrorCode == EnumErrorCode.ERROR)
                {
                    MessageBox.Show(deleteSupplierResult.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    LoadDataGridView();
                    ClearInputs();
                    MessageBox.Show(deleteSupplierResult.ErrorDesc, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }
            else if (itemsToDelete.Count > 0)
            {
                BackupData(obj);

                foreach (var item in itemsToDelete)
                {
                    var deleteOrderDetailResult = FuncShares<OrderDetail>.Delete(item.OrderDetail);
                    if (deleteOrderDetailResult.ErrorCode == EnumErrorCode.ERROR)
                    {
                        MessageBox.Show(deleteOrderDetailResult.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var deleteProductResult = FuncShares<Product>.Delete(item.Product);
                    if (deleteProductResult.ErrorCode == EnumErrorCode.ERROR)
                    {
                        MessageBox.Show(deleteProductResult.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var deleteOrderResult = FuncShares<Order>.Delete(item.Order);
                    if (deleteOrderResult.ErrorCode == EnumErrorCode.ERROR)
                    {
                        MessageBox.Show(deleteOrderResult.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var deleteSupplierResult = FuncShares<Supplier>.Delete(item.Supplier);
                    if (deleteSupplierResult.ErrorCode == EnumErrorCode.ERROR)
                    {
                        MessageBox.Show(deleteSupplierResult.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        LoadDataGridView();
                        ClearInputs();
                        MessageBox.Show(deleteSupplierResult.ErrorDesc, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

            }
            else
            {
                string str = string.Format(Constants.not_found, Name);
                MessageBox.Show(str, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                //txtAddress.Text = row.Cells[2].Value.ToString();
                string province = row.Cells[2].Value.ToString();
                string district = row.Cells[3].Value.ToString();
                string ward = row.Cells[4].Value.ToString();
                addressController.SetComboBoxSelection(province,district,ward);
                if (row.Cells[5].Value != null)
                {
                    txtPhone.Text = row.Cells[5].Value.ToString();
                }
                else
                {
                    txtPhone.Text = string.Empty; 
                }

                if (row.Cells[6].Value != null)
                {
                    txtEmail.Text = row.Cells[6].Value.ToString();
                }
                else
                {
                    txtEmail.Text = string.Empty; 
                }
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
                ClearInputs(); 
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
            using (var dataContext = new DatabaseDataContext())
            {
                try
                {
                    // Get the suppliers from the database
                    var suppliers = dataContext.Suppliers.ToList();

                    // Set the DataSource of the DataGridView to the supplier list
                    SupllierdataGridView.DataSource = suppliers;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadComboxAddres()
        {
            addressController = new AddressController(cbxProvince, cbxDistrict, cbxWard);
        }
    }
}
