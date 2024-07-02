using BTL_2.Model;
using BTL_2.Shareds;
using BTL_2.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BTL_2.Controller
{
    public class ProductController
    {
        DatabaseDataContext dataContext = new DatabaseDataContext();
        private static string Name = "Product";
        private int clickCount = 0; // Biến đếm số lần click vào hàng
        private int lastClickedRowIndex = -1; // Index của hàng được click lần cuối
        public ProductForm ProductForm { get; private set; }
        public DataGridView ProductDataGridView { get; private set; }
        public Label lbProductId { get; private set; }
        public Button btnDelete { get; private set; }
        public Button btnUpdate { get; private set; }
        public Button btnInsert { get; private set; }
        public TextBox txtDescription { get; private set; }
        public TextBox txtProductName { get; private set; }
        public TextBox txtPrice { get; private set; }
        public TextBox txtUnit { get; private set; }
        public ComboBox cbxSupplierID { get; private set; }
        public TextBox txtQuantity { get; private set; }
        public ComboBox cbxTieuChi { get; private set; }
        public Button btnSearch { get; private set; }
        public TextBox txtSearchContent { get; private set; }

        public ProductController(ProductForm productForm, DataGridView productDataGridView, Label lbProductId, Button btnDelete, Button btnUpdate, Button btnInsert, TextBox txtDescription, TextBox txtProductName, TextBox txtPrice, TextBox txtUnit, ComboBox cbxSupplierID, TextBox txtQuantity, ComboBox cbxTieuChi, Button btnSearch, TextBox txtSearchContent)
        {
            ProductForm = productForm;
            ProductDataGridView = productDataGridView;
            this.lbProductId = lbProductId;
            this.btnDelete = btnDelete;
            this.btnUpdate = btnUpdate;
            this.btnInsert = btnInsert;
            this.txtDescription = txtDescription;
            this.txtProductName = txtProductName;
            this.txtPrice = txtPrice;
            this.txtUnit = txtUnit;
            this.cbxSupplierID = cbxSupplierID;
            this.txtQuantity = txtQuantity;
            this.cbxTieuChi = cbxTieuChi;
            this.btnSearch = btnSearch;
            this.txtSearchContent = txtSearchContent;

            FuncShares<Object>.SetEnableButton(this.btnInsert, this.btnUpdate, this.btnDelete);

        }

        public void SetEvent()
        {
            ProductForm.Load += new EventHandler((object sender, EventArgs e) => LoadDataGridView());
            btnInsert.Click += Insert;
            ProductDataGridView.RowHeaderMouseClick += DataGridView_RowHeaderMouseClick;
            btnUpdate.Click += Update;
            btnDelete.Click += Delete;
            btnSearch.Click += Search;
        }

        public void Search(object obj, EventArgs e)
        {
            var content = txtSearchContent.Text;
            var tieuchi = cbxTieuChi.Text;

            if (string.IsNullOrEmpty(content))
            {
                LoadDataGridView();
            }
            else
            {
                FuncResult<List<Product>> qr = null;

                switch (tieuchi)
                {
                    case Constants.SearchByProductname:
                        qr = FuncShares<Product>.Search(u => u.ProductName.Contains(content));
                        break;
                    case Constants.SearchByID:
                        if (int.TryParse(content, out int productId))
                        {
                            qr = FuncShares<Product>.Search(p => p.ProductID == productId);
                        }
                        break;
                    case Constants.SearchByPrice:
                        if (int.TryParse(content, out int productprice))
                        {
                            qr = FuncShares<Product>.Search(p => p.Price > productprice);
                        }
                        break;
                    case Constants.SearchByUnit:
                        qr = FuncShares<Product>.Search(u => u.Unit.Contains(content));
                        break;

                        // Adjust other search criteria as per your Product model
                }
                ProductDataGridView.DataSource = null;
                ProductDataGridView.DataSource = qr.Data.ToList();

            }
        }


        private void Delete(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(lbProductId.Text))
            {
                MessageBox.Show(Constants.no_selected, "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(lbProductId.Text, out int id))
            {
                MessageBox.Show("Invalid Product ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FuncResult<List<Product>> searchResult = FuncShares<Product>.Search(u => u.ProductID == id);
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

            FuncResult<bool> confirmationResult = FuncShares<Product>.ShowConfirmationMessage();
            if (!confirmationResult.Data)
            {
                return;
            }

            Product obj = searchResult.Data[0];

            var orderDetailResult = FuncShares<OrderDetail>.GetAllData();
            if (orderDetailResult.ErrorCode == EnumErrorCode.ERROR)
            {
                MessageBox.Show(orderDetailResult.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            List<OrderDetail> list_orderdetail = orderDetailResult.Data;

            var query = from p in searchResult.Data
                        join od in list_orderdetail on p.ProductID equals od.ProductID
                        where p.ProductID == obj.ProductID
                        select new
                        {
                            Product = p,
                            OrderDetail = od
                        };

            var itemsToDelete = query.ToList();

            if (itemsToDelete.Count > 0)
            {
                //BackupData(obj);

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
                    else
                    {
                        LoadDataGridView();
                        ClearInputs();
                        MessageBox.Show(deleteProductResult.ErrorDesc, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show(Constants.not_found, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void BackupProductData(Product product)
        {
            string backupData = $"ID: {product.ProductID}, Name: {product.ProductName}, Price: {product.Price}, Unit: {product.Unit}, Quantity: {product.QuantityInStock}, SupplierID: {product.SupplierID}";
            System.IO.File.AppendAllText("backupProductData.txt", backupData + Environment.NewLine);
        }

        private void ClearInputs()
        {
            ProductDataGridView.ClearSelection();
            lbProductId.Text = "";
            txtProductName.Text = "";
            txtUnit.Text = "";
            txtPrice.Text = "";
            txtDescription.Text = "";
            txtQuantity.Text = "";
            btnInsert.Enabled = true;
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
        }

        private void DataGridView_RowHeaderMouseClick(object sender, EventArgs e)
        {
            if (ProductDataGridView.SelectedRows.Count > 0)
            {
                btnInsert.Enabled = false;
                btnDelete.Enabled = true;
                btnUpdate.Enabled = true;

                DataGridViewRow row = ProductDataGridView.SelectedRows[0];
                int currentRowIndex = row.Index;

                if (currentRowIndex == lastClickedRowIndex)
                {
                    clickCount++;

                    if (clickCount == 2)
                    {
                        ProductDataGridView.ClearSelection();
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

                lbProductId.Text = row.Cells[0].Value.ToString();
                txtProductName.Text = row.Cells[1].Value.ToString();
                txtPrice.Text = row.Cells[2].Value.ToString();
                txtDescription.Text = row.Cells[3].Value.ToString();
                txtUnit.Text = row.Cells[4].Value.ToString();
                txtQuantity.Text = row.Cells[5].Value.ToString();
                cbxSupplierID.SelectedValue = row.Cells[6].Value;
            }
        }

        private void Update(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            int id = int.Parse(lbProductId.Text);
            var name = txtProductName.Text;
            var price = decimal.Parse(txtPrice.Text); // Assuming price is decimal in your database
            var description = txtDescription.Text;
            var unit = txtUnit.Text;
            var quantity = int.Parse(txtQuantity.Text);
            var supplierid = (int)cbxSupplierID.SelectedValue;

            FuncResult<List<Product>> rs = FuncShares<Product>.GetAllData();
            switch (rs.ErrorCode)
            {
                case EnumErrorCode.ERROR:
                    MessageBox.Show(rs.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case EnumErrorCode.SUCCESS:
                    var objToUpdate = rs.Data
                        .Where(u => u.ProductID == id)
                        .FirstOrDefault();

                    if (objToUpdate != null)
                    {
                        var deleteResult = FuncShares<Product>.Delete(objToUpdate);
                        Product newProduct = new Product
                        {
                            ProductID = id,
                            ProductName = name,
                            Price = price,
                            Description = description,
                            Unit = unit,
                            QuantityInStock = quantity,
                            SupplierID = supplierid,
                            Image = null
                        };

                        var result = FuncShares<Product>.Insert(newProduct);
                        if (result.Data)
                        {

                            LoadDataGridView();
                            ClearInputs();
                            MessageBox.Show("Update successful", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show(result.ErrorDesc, "Error1", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }


                    }
                    else
                    {
                        string str = string.Format(Constants.not_found, name);
                        MessageBox.Show(str, "Information3", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;
                case EnumErrorCode.FAILED:
                    break;
            }

            /*var productToUpdate = dataContext.Products.FirstOrDefault(u => u.ProductID == id);

            if (productToUpdate != null)
            {
                // Cập nhật các thuộc tính mới, bao gồm SupplierID
                productToUpdate.ProductName = name;
                productToUpdate.Price = price;
                productToUpdate.Description = description;
                productToUpdate.Unit = unit;
                productToUpdate.QuantityInStock = quantity;

                // Kiểm tra xem SupplierID có thay đổi không
                if (productToUpdate.SupplierID != supplierid)
                {
                    productToUpdate.SupplierID = supplierid;
                }

                dataContext.SubmitChanges();
                MessageBox.Show("Product updated successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadDataGridView();
                ClearInputs();
            }
            else
            {
                MessageBox.Show("Product not found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/
        }


        private void Insert(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            var name = txtProductName.Text;
            var price = decimal.Parse(txtPrice.Text); // Assuming price is decimal in your database
            var description = txtDescription.Text;
            var unit = txtUnit.Text;
            var quantity = int.Parse(txtQuantity.Text);
            var supplierid = (int)cbxSupplierID.SelectedValue;

            Product product = new Product();
            product.ProductName = name;
            product.Price = price;
            product.Description = description;
            product.Unit = unit;
            product.QuantityInStock = quantity;
            product.SupplierID = supplierid;

            FuncResult<bool> funcResult = FuncShares<Product>.Insert(product);
            if (funcResult.Data)
            {
                LoadDataGridView();
            }
            else
            {
                MessageBox.Show(funcResult.ErrorDesc, "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                MessageBox.Show("Product Name is required.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Price is required.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Price must be a valid number greater than zero.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            // Optionally, you may include additional validations for other fields here

            return true;
        }

        private void LoadDataGridView()
        {
            ProductDataGridView.DataSource = null;
            FuncResult<List<Product>> rs = FuncShares<Product>.GetAllData();
            switch (rs.ErrorCode)
            {
                case EnumErrorCode.ERROR:
                    MessageBox.Show(rs.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case EnumErrorCode.SUCCESS:
                    ProductDataGridView.DataSource = rs.Data;
                    FuncResult<List<Supplier>> supplierResult = FuncShares<Supplier>.GetAllData();
                    switch (supplierResult.ErrorCode)
                    {
                        case EnumErrorCode.ERROR:
                            MessageBox.Show(supplierResult.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        case EnumErrorCode.SUCCESS:
                            var uniqueSuppliers = supplierResult.Data
                                .GroupBy(s => s.SupplierID)
                                .Select(g => g.First())
                                .ToList();

                            cbxSupplierID.DataSource = uniqueSuppliers;
                            cbxSupplierID.DisplayMember = "SupplierID";
                            cbxSupplierID.ValueMember = "SupplierID";
                            break;
                        case EnumErrorCode.FAILED:
                            break;
                    }

                    cbxTieuChi.DataSource = new List<string>() {
                Constants.SearchByProductname,
                Constants.SearchByID,
                Constants.SearchByPrice,
                Constants.SearchByUnit,
            };
                    break;
                case EnumErrorCode.FAILED:
                    break;
            }
        }
    }
}
