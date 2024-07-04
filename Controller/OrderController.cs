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
        private bool isOrderInserted = false;

        // Controls
        private OrderForm OrderForm;
        private DataGridView listProductOfOrddataGridView;
        private Label lbOrderId;
        private Button btnInsert;
        private Button btnUpdate;
        private Button btnDelete;
        private TextBox txtListOrder;
        private TextBox txtTotalAmount;
        private ComboBox cbxCustomerID;
        private DateTimePicker dtpDate;
        private ComboBox cbxTieuChi;
        private Button btnSearch;
        private TextBox txtSearchContent;

        public OrderController(OrderForm orderForm, DataGridView listProductOfOrddataGridView, Label lbOrderId, Button btnInsert, Button btnUpdate, Button btnDelete, TextBox txtListOrder, TextBox txtTotalAmount, ComboBox cbxCustomerID, DateTimePicker dtpDate, ComboBox cbxTieuChi, Button btnSearch, TextBox txtSearchContent)
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
            SetEventHandlers();
        }

        private void SetEventHandlers()
        {
            OrderForm.Load += (sender, e) => LoadData();
            btnInsert.Click += InsertOrder;
            btnSearch.Click += Search;
            listProductOfOrddataGridView.CellContentClick += listProductOfOrddataGridView_CellContentClick;
            listProductOfOrddataGridView.CellValueChanged += listProductOfOrddataGridView_CellValueChanged;
            listProductOfOrddataGridView.CurrentCellDirtyStateChanged += listProductOfOrddataGridView_CurrentCellDirtyStateChanged;
            listProductOfOrddataGridView.EditingControlShowing += listProductOfOrddataGridView_EditingControlShowing;
            listProductOfOrddataGridView.CellBeginEdit += listProductOfOrddataGridView_CellBeginEdit;
        }

        private void Search(object sender, EventArgs e)
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
                listProductOfOrddataGridView.DataSource = qr?.Data?.ToList() ?? new List<Product>();
            }
        }

        private void InsertOrder(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin để thêm đơn hàng.", "Thông tin không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var customerid = int.Parse(cbxCustomerID.SelectedValue.ToString());
            var listorder = txtListOrder.Text;
            decimal totalamount = 0;
            var date = dtpDate.Value;

            List<Product> selectedProducts = GetSelectedProducts(ref totalamount);

            if (selectedProducts.Count == 0)
            {
                MessageBox.Show("Bạn cần chọn ít nhất một sản phẩm để thêm vào đơn hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Order order = new Order
            {
                CustomerID = customerid,
                TotalAmount = totalamount,
                OrderDate = date,
                OrderStatus = "OK"
            };

            try
            {
                dataContext.Orders.InsertOnSubmit(order);
                dataContext.SubmitChanges();
                isOrderInserted = true;

                int orderId = order.OrderID;

                foreach (var product in selectedProducts)
                {
                    DataGridViewRow correspondingRow = listProductOfOrddataGridView.Rows
                        .Cast<DataGridViewRow>()
                        .FirstOrDefault(r => r.DataBoundItem == product);

                    if (correspondingRow != null && correspondingRow.Cells["quantityColumn"] is DataGridViewTextBoxCell quantityCell)
                    {
                        if (int.TryParse(quantityCell.Value?.ToString(), out int quantityToSell) && quantityToSell > 0 && quantityToSell <= product.QuantityInStock)
                        {
                            OrderDetail orderDetail = new OrderDetail
                            {
                                OrderID = orderId,
                                ProductID = product.ProductID,
                                Price = product.Price * quantityToSell,
                                Quantity = quantityToSell
                            };

                            totalamount += orderDetail.Price;
                            dataContext.OrderDetails.InsertOnSubmit(orderDetail);
                            product.QuantityInStock -= quantityToSell;
                        }
                        else
                        {
                            MessageBox.Show("Số lượng sản phẩm trong kho không đủ hoặc không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            RollbackOrder(order);
                            return;
                        }
                    }
                }

                dataContext.SubmitChanges();
                LoadData();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi trong quá trình thêm order: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                RollbackOrder(order);
            }
            finally
            {
                if (isOrderInserted)
                {
                    MessageBox.Show("Đơn hàng đã được bán thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void RollbackOrder(Order order)
        {
            try
            {
                dataContext.Orders.DeleteOnSubmit(order);
                dataContext.SubmitChanges();
                isOrderInserted = false;
            }
            catch (Exception deleteEx)
            {
                MessageBox.Show($"Đã xảy ra lỗi trong quá trình xóa đơn hàng: {deleteEx.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<Product> GetSelectedProducts(ref decimal totalamount)
        {
            List<Product> selectedProducts = new List<Product>();

            foreach (DataGridViewRow row in listProductOfOrddataGridView.Rows)
            {
                if (!row.IsNewRow && row.Cells["checkBoxColumn"] is DataGridViewCheckBoxCell checkBoxCell && (bool)(checkBoxCell.Value ?? false))
                {
                    if (row.DataBoundItem is Product product && row.Cells["quantityColumn"] is DataGridViewTextBoxCell quantityCell)
                    {
                        if (int.TryParse(quantityCell.Value?.ToString(), out int quantity) && quantity > 0)
                        {
                            selectedProducts.Add(product);
                            totalamount += product.Price * quantity;
                        }
                    }
                }
            }

            return selectedProducts;
        }

        private void ClearInputs()
        {
            lbOrderId.Text = "";
            txtListOrder.Text = "";
            txtTotalAmount.Text = "";
            cbxCustomerID.SelectedIndex = -1;
            btnInsert.Enabled = true;
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtListOrder.Text))
            {
                return false;
            }

            if (cbxCustomerID.SelectedIndex == -1 && string.IsNullOrWhiteSpace(cbxCustomerID.Text))
            {
                MessageBox.Show("Vui lòng chọn hoặc nhập thông tin khách hàng.", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void UpdateTotalAmountAndListOrder()
        {
            decimal totalAmount = 0;
            StringBuilder listOrderDetails = new StringBuilder();

            foreach (DataGridViewRow row in listProductOfOrddataGridView.Rows)
            {
                if (!row.IsNewRow && row.Cells["checkBoxColumn"] is DataGridViewCheckBoxCell checkBoxCell && (bool)(checkBoxCell.Value ?? false))
                {
                    if (row.DataBoundItem is Product product && row.Cells["quantityColumn"] is DataGridViewTextBoxCell quantityCell)
                    {
                        if (int.TryParse(quantityCell.Value?.ToString(), out int quantity) && quantity > 0)
                        {
                            totalAmount += product.Price * quantity;
                            listOrderDetails.AppendLine($"- {product.ProductName}: {quantity} {product.Unit} x {product.Price.ToString("N0")}đ = {(product.Price * quantity).ToString("N0")}đ");
                        }
                    }
                }
            }

            txtTotalAmount.Text = totalAmount.ToString("N0");
            txtListOrder.Text = listOrderDetails.ToString();
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

            FuncShares<Object>.SetEnableButton(this.btnInsert, this.btnUpdate, this.btnDelete);
            this.btnUpdate.Enabled = false;
            this.btnDelete.Enabled = false;
        }

        private void listProductOfOrddataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == listProductOfOrddataGridView.Columns["checkBoxColumn"].Index)
            {
                UpdateTotalAmountAndListOrder();
            }
        }

        private void listProductOfOrddataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == listProductOfOrddataGridView.Columns["quantityColumn"].Index)
            {
                UpdateTotalAmountAndListOrder();
            }
        }

        private void listProductOfOrddataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (listProductOfOrddataGridView.IsCurrentCellDirty)
            {
                listProductOfOrddataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void listProductOfOrddataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (listProductOfOrddataGridView.CurrentCell.ColumnIndex == listProductOfOrddataGridView.Columns["quantityColumn"].Index && e.Control is TextBox tb)
            {
                tb.KeyPress -= DataGridViewCell_KeyPress;
                tb.KeyPress += DataGridViewCell_KeyPress;
            }
        }

        private void DataGridViewCell_KeyPress(object sender, KeyPressEventArgs e)
        {
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
