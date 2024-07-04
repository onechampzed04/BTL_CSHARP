using BTL_2.Model;
using BTL_2.Shareds;
using BTL_2.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BTL_2.Controller
{
    public class OrderControllerForManager
    {
        private  DatabaseDataContext dataContext = new DatabaseDataContext();
        private  OrderFormForManager orderFormForManager;
        private  DataGridView orderDataGridView;
        private  DataGridView orderDetailDataGridView;
        private  Button btnDelete;
        private  Button btnUpdate;
        private  Button btnHoanTra;
        private  TextBox txtQuantity;
        private  ComboBox cbxTieuChi;
        private  Button btnSearch;
        private  TextBox txtSearchContent;

        public OrderControllerForManager(OrderFormForManager orderFormForManager, DataGridView orderDataGridView, DataGridView orderDetailDataGridView,
                                         Button btnUpdate, Button btnDelete, ComboBox cbxTieuChi, Button btnSearch, TextBox txtSearchContent,
                                         TextBox txtQuantity, Button btnHoanTra)
        {
            this.orderFormForManager = orderFormForManager;
            this.orderDataGridView = orderDataGridView;
            this.orderDetailDataGridView = orderDetailDataGridView;
            this.btnDelete = btnDelete;
            this.btnUpdate = btnUpdate;
            this.cbxTieuChi = cbxTieuChi;
            this.btnSearch = btnSearch;
            this.txtSearchContent = txtSearchContent;
            this.txtQuantity = txtQuantity;
            this.btnHoanTra = btnHoanTra;

            SetEventHandlers();
            LoadData();
        }

        private void SetEventHandlers()
        {
            orderFormForManager.Load += OrderForm_Load;
            orderDataGridView.RowHeaderMouseClick += OrderDataGridView_RowHeaderMouseClick;
            orderDetailDataGridView.RowHeaderMouseClick += OrderDetailDataGridView_RowHeaderMouseClick;
            btnSearch.Click += Search;
            btnUpdate.Click += UpdateOrderDetail;
            btnDelete.Click += DeleteOrder;
            btnHoanTra.Click += HoanTra;
        }

        private void OrderForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            orderDataGridView.DataSource = dataContext.Orders.ToList();
        }

        private void OrderDataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < orderDataGridView.Rows.Count)
            {
                int orderId = Convert.ToInt32(orderDataGridView.Rows[e.RowIndex].Cells["OrderID"].Value);
                var orderDetails = dataContext.OrderDetails.Where(od => od.OrderID == orderId).ToList();
                orderDetailDataGridView.DataSource = orderDetails;
            }
        }

        private void OrderDetailDataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < orderDetailDataGridView.Rows.Count)
            {
                DataGridViewRow selectedRow = orderDetailDataGridView.Rows[e.RowIndex];
                txtQuantity.Text = selectedRow.Cells["Quantity"].Value.ToString();
            }
        }

        private void Search(object sender, EventArgs e)
        {
            string content = txtSearchContent.Text.Trim();
            string tieuchi = cbxTieuChi.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(content))
            {
                LoadData();
                return;
            }

            IQueryable<Order> query = null;

            switch (tieuchi)
            {
                case Constants.SearchByID:
                    if (int.TryParse(content, out int orderId))
                    {
                        query = dataContext.Orders.Where(p => p.OrderID == orderId);
                    }
                    break;
                case Constants.SearchByCusID:
                    if (int.TryParse(content, out int customerId))
                    {
                        query = dataContext.Orders.Where(p => p.CustomerID == customerId);
                    }
                    break;
                case Constants.SearchByPrice:
                    if (decimal.TryParse(content, out decimal totalAmount))
                    {
                        query = dataContext.Orders.Where(p => p.TotalAmount >= totalAmount);
                    }
                    break;
                case Constants.SearchByStatus:
                    query = dataContext.Orders.Where(p => p.OrderStatus.Contains(content));
                    break;
            }

            if (query != null)
            {
                orderDataGridView.DataSource = query.ToList();
            }
        }

        private void UpdateOrderDetail(object sender, EventArgs e)
        {
            if (orderDataGridView.SelectedRows.Count == 0 || orderDetailDataGridView.SelectedRows.Count == 0)
                return;

            DataGridViewRow orderRow = orderDataGridView.SelectedRows[0];
            DataGridViewRow orderDetailRow = orderDetailDataGridView.SelectedRows[0];

            int orderId = Convert.ToInt32(orderRow.Cells["OrderID"].Value);
            int orderDetailId = Convert.ToInt32(orderDetailRow.Cells["OrderDetailID"].Value);
            int productId = Convert.ToInt32(orderDetailRow.Cells["ProductID"].Value);

            var orderDetail = dataContext.OrderDetails.FirstOrDefault(od => od.OrderDetailID == orderDetailId);
            if (orderDetail == null)
            {
                MessageBox.Show("Chi tiết đơn hàng không tồn tại.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int newQuantity;
            if (!int.TryParse(txtQuantity.Text, out newQuantity))
            {
                MessageBox.Show("Số lượng không hợp lệ.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var product = dataContext.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product == null)
            {
                MessageBox.Show("Sản phẩm không tồn tại.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int oldQuantity = orderDetail.Quantity;
            int difference = newQuantity - oldQuantity;
            int newQuantityInStock = product.QuantityInStock - difference;

            if (newQuantityInStock < 0)
            {
                MessageBox.Show("Số lượng tồn kho không đủ.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            decimal newPrice = newQuantity * product.Price;

            orderDetail.Quantity = newQuantity;
            orderDetail.Price = newPrice;
            product.QuantityInStock = newQuantityInStock;

            dataContext.SubmitChanges();
            LoadData();
            MessageBox.Show("Cập nhật chi tiết đơn hàng thành công.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DeleteOrder(object sender, EventArgs e)
        {
            if (orderDataGridView.SelectedRows.Count == 0)
                return;

            DataGridViewRow selectedRow = orderDataGridView.SelectedRows[0];
            int orderId = Convert.ToInt32(selectedRow.Cells["OrderID"].Value);

            var orderDetails = dataContext.OrderDetails.Where(od => od.OrderID == orderId).ToList();
            dataContext.OrderDetails.DeleteAllOnSubmit(orderDetails);

            var order = dataContext.Orders.FirstOrDefault(o => o.OrderID == orderId);
            if (order != null)
            {
                dataContext.Orders.DeleteOnSubmit(order);
                dataContext.SubmitChanges();
                MessageBox.Show("Xóa đơn hàng thành công.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Đơn hàng không tồn tại.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            orderDetailDataGridView.DataSource = null;
            LoadData();
        }

        private void HoanTra(object sender, EventArgs e)
        {
            if (orderDataGridView.SelectedRows.Count == 0)
                return;

            DataGridViewRow selectedRow = orderDataGridView.SelectedRows[0];
            int orderId = Convert.ToInt32(selectedRow.Cells["OrderID"].Value);

            var orderDetails = dataContext.OrderDetails.Where(od => od.OrderID == orderId).ToList();

            foreach (var orderDetail in orderDetails)
            {
                int productId = orderDetail.ProductID;
                var product = dataContext.Products.FirstOrDefault(p => p.ProductID == productId);
                if (product != null)
                {
                    product.QuantityInStock += orderDetail.Quantity;
                }
            }

            dataContext.OrderDetails.DeleteAllOnSubmit(orderDetails);

            var order = dataContext.Orders.FirstOrDefault(o => o.OrderID == orderId);
            if (order != null)
            {
                dataContext.Orders.DeleteOnSubmit(order);
                dataContext.SubmitChanges();
                MessageBox.Show("Hoàn trả đơn hàng thành công.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Đơn hàng không tồn tại.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            orderDetailDataGridView.DataSource = null;
            LoadData();
        }
    }
}
