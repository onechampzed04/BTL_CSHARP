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

namespace BTL_2.Controller
{
    public class OrderController
    {
        private DatabaseDataContext dataContext = new DatabaseDataContext();
        public OrderForm OrderForm { get; private set; }
        public DataGridView listProductOfOrddataGridView { get; private set; }
        public Label lbOrderId { get; private set; }
        public Button btnInsert { get; private set; }
        public Button btnUpdate { get; private set; }
        public Button btnDelete { get; private set; }
        public TextBox txtListOrder { get; private set; }
        public TextBox txtTotalAmount { get; private set; }
        public ComboBox cbxCustomerID { get; private set; }
        public DateTimePicker dtpDate { get; private set; }
        public ComboBox cbxTieuChi { get; private set; }
        public Button btnSearch { get; private set; }
        public TextBox txtSearchContent { get; private set; }

        public OrderController(OrderForm orderForm, DataGridView listProductOfOrddataGridView, Label lbOrderId, Button btnInsert, Button btnUpdate, Button btnDelete,TextBox txtListOrder, TextBox txtTotalAmount, ComboBox cbxCustomerID, DateTimePicker dtpDate, ComboBox cbxTieuChi, Button btnSearch, TextBox txtSearchContent)
        {
            OrderForm = orderForm;
            this.listProductOfOrddataGridView = listProductOfOrddataGridView;
            this.lbOrderId = lbOrderId;
            this.btnInsert = btnInsert;
            this.btnUpdate = btnUpdate;
            this.btnDelete = btnDelete;
            this.txtListOrder = txtListOrder;
            this.txtTotalAmount = txtTotalAmount;
            this.cbxCustomerID = cbxCustomerID;
            this.dtpDate = dtpDate;
            this.cbxTieuChi = cbxTieuChi;
            this.btnSearch = btnSearch;
            this.txtSearchContent = txtSearchContent;
            FuncShares<Object>.SetEnableButton(this.btnInsert, this.btnUpdate, this.btnDelete);
            SetEvent();
        }

        public void SetEvent()
        {
            OrderForm.Load += new EventHandler((object sender, EventArgs e) => LoadData());
            btnInsert.Click += InsertOrder;
            btnSearch.Click += Search;
            listProductOfOrddataGridView.CellContentClick += listProductOfOrddataGridView_CellContentClick;
            listProductOfOrddataGridView.CellValueChanged += listProductOfOrddataGridView_CellValueChanged;
            listProductOfOrddataGridView.CurrentCellDirtyStateChanged += listProductOfOrddataGridView_CurrentCellDirtyStateChanged;
            listProductOfOrddataGridView.EditingControlShowing += listProductOfOrddataGridView_EditingControlShowing;
            listProductOfOrddataGridView.CellBeginEdit += listProductOfOrddataGridView_CellBeginEdit;
        }

        public void Search(object obj, EventArgs e)
        {
            var content = txtSearchContent.Text;
            var tieuchi = cbxTieuChi.Text;

            if (string.IsNullOrEmpty(content))
            {
                LoadData();
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
                listProductOfOrddataGridView.DataSource = null;
                listProductOfOrddataGridView.DataSource = qr.Data.ToList();
            }

        }

        private void BackupUserData(Order order)
        {
            string backupData = $"ID: {order.OrderID}, Date: {order.OrderDate}, Details: {order.OrderDetails}, Status: {order.OrderStatus}";
            System.IO.File.AppendAllText("backupOrderData.txt", backupData + Environment.NewLine);
        }

        private void ClearInputs()
        {
            lbOrderId.Text = "";
            txtListOrder.Text = "";
            txtTotalAmount.Text = "";
            cbxCustomerID.SelectedIndex = -1;
            btnInsert.Enabled = true;
        }

        // Biến để kiểm tra xem đã insert order chưa
        private bool isOrderInserted = false;

        private void InsertOrder(object sender, EventArgs e)
        {
            // Validate data before proceeding to insert
            IsAnyProductSelected();
            if (isOrderInserted == false)
            {
                if (!ValidateInputs())
                {
                    MessageBox.Show("Danh sách đơn hàng là bắt buộc.", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var customerid = int.Parse(cbxCustomerID.SelectedValue.ToString());
                var listorder = txtListOrder.Text;
                decimal totalamount = 0;
                var date = dtpDate.Value;

                List<Product> selectedProducts = new List<Product>();
                int collumnselect = 0;

                foreach (DataGridViewRow row in listProductOfOrddataGridView.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        DataGridViewCheckBoxCell checkBoxCell = row.Cells["checkBoxColumn"] as DataGridViewCheckBoxCell;

                        if (checkBoxCell != null && (bool)(checkBoxCell.Value ?? false))
                        {
                            DataGridViewTextBoxCell quantityCell = row.Cells["quantityColumn"] as DataGridViewTextBoxCell;

                            if (quantityCell != null && quantityCell.Value != null && !string.IsNullOrWhiteSpace(quantityCell.Value.ToString()))
                            {
                                int quantity;

                                if (int.TryParse(quantityCell.Value.ToString(), out quantity) && quantity > 0)
                                {
                                    Product product = (Product)row.DataBoundItem;
                                    selectedProducts.Add(product);
                                    totalamount += product.Price * quantity;
                                    collumnselect++;
                                }
                            }
                        }
                    }
                }

                // Check if any product is selected
                if (collumnselect == 0)
                {
                    MessageBox.Show("Bạn cần chọn ít nhất một sản phẩm để thêm vào đơn hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Create an order object
                Order order = new Order
                {
                    CustomerID = customerid,
                    TotalAmount = totalamount,
                    OrderDate = date,
                    OrderStatus = "OK"
                };

                try
                {
                    // Add order to the database
                    dataContext.Orders.InsertOnSubmit(order);
                    dataContext.SubmitChanges();
                    isOrderInserted = true;

                    // Backup data
                    //BackupUserData(order);

                    // Get the newly created OrderID
                    int orderId = order.OrderID;

                    foreach (var product in selectedProducts)
                    {
                        DataGridViewRow correspondingRow = listProductOfOrddataGridView.Rows
                            .Cast<DataGridViewRow>()
                            .Where(r => r.DataBoundItem == product)
                            .FirstOrDefault();

                        if (correspondingRow != null)
                        {
                            DataGridViewTextBoxCell quantityCell = correspondingRow.Cells["quantityColumn"] as DataGridViewTextBoxCell;
                            int quantityToSell;

                            if (quantityCell != null && int.TryParse(quantityCell.Value?.ToString(), out quantityToSell) && quantityToSell > 0 && quantityToSell <= product.QuantityInStock)
                            {
                                OrderDetail orderDetail = new OrderDetail
                                {
                                    OrderID = orderId,
                                    ProductID = product.ProductID,
                                    Price = product.Price * quantityToSell,
                                    Quantity = quantityToSell
                                };

                                totalamount += orderDetail.Price;

                                // Add OrderDetail to the database
                                dataContext.OrderDetails.InsertOnSubmit(orderDetail);
                                product.QuantityInStock -= quantityToSell;
                            }
                            else
                            {
                                MessageBox.Show("Số lượng sản phẩm trong kho không đủ hoặc không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                // Delete the order added
                                try
                                {
                                    dataContext.Orders.DeleteOnSubmit(order);
                                    //var customer = dataContext.Customers.FirstOrDefault(c => c.CustomerID == customerid);
                                    //if (customer != null)
                                    //{
                                    //    dataContext.Customers.DeleteOnSubmit(customer);
                                    //    dataContext.SubmitChanges();
                                    //}
                                }
                                catch (Exception deleteEx)
                                {
                                    MessageBox.Show($"Đã xảy ra lỗi trong quá trình xóa khách hàng: {deleteEx.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                isOrderInserted = false;
                                return;
                            }
                        }
                    }

                    // Submit all changes to the database
                    dataContext.SubmitChanges();
                    LoadData();
                    ClearInputs(); // Call ClearInputs only if the insert is successful
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Đã xảy ra lỗi trong quá trình thêm order: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    isOrderInserted = false;
                }
                finally
                {
                    if (isOrderInserted)
                    {
                        // If order is successfully inserted, notify the user
                        MessageBox.Show("Đơn hàng đã được bán thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        // Method to add a new customer if not exists
        //private bool AddNewCustomer()
        //{
        //    try
        //    {
        //        // Create a new Customer object
        //        Customer newCustomer = new Customer
        //        {
        //            CustomerID = int.Parse(cbxCustomerID.Text), // Assuming cbxCustomerID.Text contains the new customer ID
        //            CustomerName = "", // Set appropriate customer name based on your requirements
        //                               // Add other properties as necessary
        //            District = "Thành ph? Long Xuyên",
        //            Province = "T?nh An Giang",
        //            Ward = "Phu?ng M? Long",
        //            PhoneNumber = "0",
        //            Email = "nulls@gmail.com",
        //        };

        //        // Add new customer to the database
        //        dataContext.Customers.InsertOnSubmit(newCustomer);
        //        dataContext.SubmitChanges();

        //        return true; // Return true if customer added successfully
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Đã xảy ra lỗi trong quá trình thêm khách hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return false; // Return false on error
        //    }
        //}



        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtListOrder.Text))
            {
                return false;
            }

            // Check if a customer is selected or a new customer needs to be added
            if (cbxCustomerID.SelectedIndex == -1 && string.IsNullOrWhiteSpace(cbxCustomerID.Text))
            {
                MessageBox.Show("Vui lòng chọn hoặc nhập thông tin khách hàng.", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            //else if (cbxCustomerID.SelectedIndex == -1 && !string.IsNullOrWhiteSpace(cbxCustomerID.Text))
            //{
            //    // Validate and add new customer
            //    if (!AddNewCustomer())
            //    {
            //        MessageBox.Show("Thêm khách hàng mới không thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        return false;
            //    }
            //}

            if (!IsAnyProductSelected())
            {
                MessageBox.Show("Bạn cần chọn ít nhất một sản phẩm để thêm vào đơn hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }


        private bool IsAnyProductSelected()
        {
            foreach (DataGridViewRow row in listProductOfOrddataGridView.Rows)
            {
                if (!row.IsNewRow)
                {
                    DataGridViewCheckBoxCell checkBoxCell = row.Cells["checkBoxColumn"] as DataGridViewCheckBoxCell;

                    if (checkBoxCell != null && (bool)(checkBoxCell.Value ?? false))
                    {
                        DataGridViewTextBoxCell quantityCell = row.Cells["quantityColumn"] as DataGridViewTextBoxCell;

                        if (quantityCell != null && quantityCell.Value != null && !string.IsNullOrWhiteSpace(quantityCell.Value.ToString()))
                        {
                            int quantity;
                            if (int.TryParse(quantityCell.Value.ToString(), out quantity) && quantity > 0)
                            {
                                isOrderInserted = false;
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private void UpdateTotalAmountAndListOrder()
        {
            decimal totalAmount = 0;
            StringBuilder listOrderDetails = new StringBuilder();

            foreach (DataGridViewRow row in listProductOfOrddataGridView.Rows)
            {
                if (!row.IsNewRow)
                {
                    DataGridViewCheckBoxCell checkBoxCell = row.Cells["checkBoxColumn"] as DataGridViewCheckBoxCell;

                    if (checkBoxCell != null && (bool)(checkBoxCell.Value ?? false))
                    {
                        DataGridViewTextBoxCell quantityCell = row.Cells["quantityColumn"] as DataGridViewTextBoxCell;

                        if (quantityCell != null && quantityCell.Value != null && !string.IsNullOrWhiteSpace(quantityCell.Value.ToString()))
                        {
                            int quantity;

                            if (int.TryParse(quantityCell.Value.ToString(), out quantity) && quantity > 0)
                            {
                                Product product = (Product)row.DataBoundItem;
                                totalAmount += product.Price * quantity;

                                // Append product details to listOrderDetails
                                listOrderDetails.AppendLine($"{product.ProductID}, {product.ProductName}, {quantity}, {product.Price:C}");
                            }
                        }
                    }
                }
            }

            txtTotalAmount.Text = totalAmount.ToString("C"); // Display the total amount in the textbox
            txtListOrder.Text = listOrderDetails.ToString(); // Display the selected product details in the textbox
        }

        private void LoadData()
        {
            listProductOfOrddataGridView.DataSource = dataContext.Products.ToList();

            FuncResult<List<Customer>> customerResult = FuncShares<Customer>.GetAllData();
            switch (customerResult.ErrorCode)
            {
                case EnumErrorCode.ERROR:
                    MessageBox.Show(customerResult.ErrorDesc, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case EnumErrorCode.SUCCESS:
                    var uniqueCustomers = customerResult.Data
                        .GroupBy(c => c.CustomerID)
                        .Select(g => g.First())
                        .ToList();

                    cbxCustomerID.DataSource = uniqueCustomers;
                    cbxCustomerID.DisplayMember = "CustomerName"; // Điều chỉnh DisplayMember nếu cần
                    cbxCustomerID.ValueMember = "CustomerID";
                    break;
                case EnumErrorCode.FAILED:
                    break;
            }

            // Thêm cột CheckBox vào cuối cùng nếu chưa có
            if (listProductOfOrddataGridView.Columns["checkBoxColumn"] == null)
            {
                DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn
                {
                    HeaderText = "Chọn",
                    Name = "checkBoxColumn",
                    Width = 50
                };
                listProductOfOrddataGridView.Columns.Add(checkBoxColumn);
            }

            // Thêm cột Quantity vào nếu chưa tồn tại
            if (listProductOfOrddataGridView.Columns["quantityColumn"] == null)
            {
                DataGridViewTextBoxColumn quantityColumn = new DataGridViewTextBoxColumn
                {
                    HeaderText = "Số lượng",
                    Name = "quantityColumn",
                    Width = 100
                };
                listProductOfOrddataGridView.Columns.Add(quantityColumn);
            }
            cbxTieuChi.DataSource = new List<string>() {
                Constants.SearchByProductname,
                Constants.SearchByID,
                Constants.SearchByPrice,
                Constants.SearchByUnit,
            };
        }

        private void listProductOfOrddataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == listProductOfOrddataGridView.Columns["checkBoxColumn"].Index && e.RowIndex >= 0)
            {
                DataGridViewCheckBoxCell checkBoxCell = (DataGridViewCheckBoxCell)listProductOfOrddataGridView.Rows[e.RowIndex].Cells["checkBoxColumn"];
                checkBoxCell.Value = !(bool)(checkBoxCell.Value ?? false);
                UpdateTotalAmountAndListOrder();
            }
        }

        private void listProductOfOrddataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (listProductOfOrddataGridView.CurrentCell is DataGridViewCheckBoxCell)
            {
                listProductOfOrddataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
                UpdateTotalAmountAndListOrder();
            }
        }

        private void listProductOfOrddataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == listProductOfOrddataGridView.Columns["checkBoxColumn"].Index && e.RowIndex >= 0)
            {
                DataGridViewCheckBoxCell checkBoxCell = (DataGridViewCheckBoxCell)listProductOfOrddataGridView.Rows[e.RowIndex].Cells["checkBoxColumn"];
                bool isChecked = (bool)checkBoxCell.Value;
                UpdateTotalAmountAndListOrder();
            }

            // Kiểm tra nếu thay đổi trên cột "quantityColumn" và hàng không phải hàng mới
            if (e.ColumnIndex == listProductOfOrddataGridView.Columns["quantityColumn"].Index && e.RowIndex >= 0)
            {
                DataGridViewCheckBoxCell checkBoxCell = listProductOfOrddataGridView.Rows[e.RowIndex].Cells["checkBoxColumn"] as DataGridViewCheckBoxCell;

                // Kiểm tra nếu checkbox được chọn
                if (checkBoxCell != null && (bool)(checkBoxCell.Value ?? false))
                {
                    // Gọi hàm UpdateTotalAmountAndListOrder để cập nhật tổng tiền và danh sách đơn hàng
                    UpdateTotalAmountAndListOrder();
                }
            }
        }

        private void listProductOfOrddataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (listProductOfOrddataGridView.CurrentCell.ColumnIndex == listProductOfOrddataGridView.Columns["quantityColumn"].Index)
            {
                TextBox textBox = e.Control as TextBox;
                if (textBox != null)
                {
                    textBox.KeyPress -= new KeyPressEventHandler(textBox_KeyPress);
                    textBox.KeyPress += new KeyPressEventHandler(textBox_KeyPress);
                }
            }
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho phép nhập số
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void listProductOfOrddataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;
            if (dataGridView != null && e.ColumnIndex != dataGridView.Columns["quantityColumn"].Index && e.ColumnIndex != dataGridView.Columns["checkBoxColumn"].Index)
            {
                e.Cancel = true; // Ngăn chặn chỉnh sửa các cột khác
            }
        }
    }
}
