using BTL_2.Model;
using BTL_2.Shareds;
using BTL_2.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTL_2.Controller
{
    public class OrderControllerForManager
    {
        private DatabaseDataContext dataContext = new DatabaseDataContext();
        public OrderFormForManager OrderFormForManager { get; private set; }
        public DataGridView OrderdataGridView { get; private set; }
        public DataGridView OrderDetaildataGridView { get; private set; }
        private static string Name = "Order";

        public Button btnDelete { get; private set; }
        public Button btnUpdate { get; private set; }
        public Button btnHoanTra { get; private set; }
        public TextBox txtQuantity { get; private set; }
        public ComboBox cbxTieuChi { get; private set; }
        public Button btnSearch { get; private set; }
        public TextBox txtSearchContent { get; private set; }

        public OrderControllerForManager(OrderFormForManager orderFormForManager, DataGridView OrderdataGridView, DataGridView OrderDetaildataGridView, Button btnUpdate, Button btnDelete, ComboBox cbxTieuChi, Button btnSearch, TextBox txtSearchContent, TextBox txtQuantity, Button btnHoanTra)
        {
            OrderFormForManager = orderFormForManager;
            this.OrderdataGridView = OrderdataGridView;
            this.OrderDetaildataGridView = OrderDetaildataGridView;
            this.btnDelete = btnDelete;
            this.btnUpdate = btnUpdate;

            this.cbxTieuChi = cbxTieuChi;
            this.btnSearch = btnSearch;
            this.txtSearchContent = txtSearchContent;
            this.txtQuantity = txtQuantity;
            this.btnHoanTra = btnHoanTra;
            SetEvent();

        }

        public void SetEvent()
        {
            OrderFormForManager.Load += new EventHandler((object sender, EventArgs e) => LoadData());
            OrderdataGridView.RowHeaderMouseClick += DataGridView_RowHeaderMouseClick;
            OrderDetaildataGridView.RowHeaderMouseClick += DataGridView_RowHeaderMouseClick1;
            btnSearch.Click += Search;
            btnUpdate.Click += Update;
            btnDelete.Click += Delete;
            btnHoanTra.Click += HoanTra;

        }


        private void HoanTra(object sender, EventArgs e)
        {
            if (OrderdataGridView.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = OrderdataGridView.SelectedRows[0];
                int orderId = int.Parse(selectedRow.Cells["OrderID"].Value.ToString());

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

                OrderDetaildataGridView.DataSource = null;
                LoadData();
            }
        }

        private void Update(object sender, EventArgs e)
        {
            if (OrderdataGridView.SelectedRows.Count > 0 && OrderDetaildataGridView.SelectedRows.Count > 0)
            {
                DataGridViewRow OrderselectedRow = OrderdataGridView.SelectedRows[0];
                DataGridViewRow OrderDetailselectedRow = OrderDetaildataGridView.SelectedRows[0];
                int orderId = int.Parse(OrderselectedRow.Cells["OrderID"].Value.ToString());

                int newQuantity = int.Parse(txtQuantity.Text);
                int orderDetailID = int.Parse(OrderDetailselectedRow.Cells["OrderDetailID"].Value.ToString());
                int productID = int.Parse(OrderDetailselectedRow.Cells["ProductID"].Value.ToString());

                var product = dataContext.Products.FirstOrDefault(p => p.ProductID == productID);
                if (product != null)
                {
                    int currentQuantityInStock = product.QuantityInStock;
                    int oldQuantity = dataContext.OrderDetails.FirstOrDefault(od => od.OrderDetailID == orderDetailID)?.Quantity ?? 0;
                    int difference = newQuantity - oldQuantity;
                    int newQuantityInStock = currentQuantityInStock - difference;

                    if (newQuantityInStock < 0)
                    {
                        MessageBox.Show("Số lượng tồn kho không đủ.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var orderDetail = dataContext.OrderDetails.FirstOrDefault(od => od.OrderDetailID == orderDetailID);
                    if (orderDetail != null)
                    {
                        orderDetail.Quantity = newQuantity;
                        decimal productPrice = product.Price;
                        decimal newPrice = newQuantity * productPrice;
                        orderDetail.Price = newPrice;

                        product.QuantityInStock = newQuantityInStock;

                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Chi tiết đơn hàng không tồn tại.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Sản phẩm không tồn tại.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                decimal totalAmount = 0;
                foreach (DataGridViewRow row in OrderDetaildataGridView.Rows)
                {
                    if (row.Cells["Price"].Value != null)
                    {
                        totalAmount += Convert.ToDecimal(row.Cells["Price"].Value);
                    }
                }

                var order = dataContext.Orders.FirstOrDefault(o => o.OrderID == orderId);
                if (order != null)
                {
                    order.TotalAmount = totalAmount;
                    dataContext.SubmitChanges();

                    MessageBox.Show("Tổng số tiền của đơn hàng đã được cập nhật thành công.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadData();
                }
                else
                {
                    MessageBox.Show("Đơn hàng không tồn tại.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }





        private void DataGridView_RowHeaderMouseClick1(object sender, EventArgs e)
        {
            if (OrderDetaildataGridView.SelectedRows.Count > 0)
            {
                txtQuantity.ReadOnly = false;

                DataGridViewRow row = OrderDetaildataGridView.SelectedRows[0];
                int currentRowIndex = row.Index;
                txtQuantity.Text = row.Cells["Quantity"].Value.ToString();
            }
        }

        private void Delete(object sender, EventArgs e)
        {
            if (OrderdataGridView.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = OrderdataGridView.SelectedRows[0];
                int orderId = int.Parse(selectedRow.Cells[0].Value.ToString());

                var orderDetails = dataContext.OrderDetails.Where(od => od.OrderID == orderId).ToList();
                dataContext.OrderDetails.DeleteAllOnSubmit(orderDetails);

                var order = dataContext.Orders.FirstOrDefault(o => o.OrderID == orderId);
                if (order != null)
                {
                    dataContext.Orders.DeleteOnSubmit(order);
                    dataContext.SubmitChanges();
                    MessageBox.Show("Xóa dữ liệu thành công.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show(Constants.not_found, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                OrderDetaildataGridView.DataSource = null;
                LoadData();
            }
        }




        private void DataGridView_RowHeaderMouseClick(object sender, EventArgs e)
        {
            if (OrderdataGridView.SelectedRows.Count > 0)
            {

                btnDelete.Enabled = true;
                btnUpdate.Enabled = true;
                btnHoanTra.Enabled = true;
            }
            DataGridViewRow row = OrderdataGridView.SelectedRows[0];
            int currentRowIndex = row.Index;
            int id = int.Parse(row.Cells[0].Value.ToString());
            var qr = dataContext.OrderDetails.Where(o => o.OrderID == id);
            OrderDetaildataGridView.DataSource = null;
            OrderDetaildataGridView.DataSource = qr;



        }

        private void LoadData()
        {
            OrderdataGridView.DataSource = dataContext.Orders.ToList();

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
                FuncResult<List<Order>> qr = null;

                switch (tieuchi)
                {
                    case Constants.SearchByID:
                        if (int.TryParse(content, out int ordertId))
                        {
                            qr = FuncShares<Order>.Search(p => p.OrderID == ordertId);
                        }
                        break;
                    case Constants.SearchByCusID:
                        if (int.TryParse(content, out int customertId))
                        {
                            qr = FuncShares<Order>.Search(p => p.CustomerID == customertId);
                        }
                        break;
                    case Constants.SearchByPrice:
                        if (int.TryParse(content, out int TotalAmount))
                        {
                            qr = FuncShares<Order>.Search(p => p.TotalAmount >= TotalAmount);
                        }
                        break;
                    case Constants.SearchByStatus:
                        qr = FuncShares<Order>.Search(p => p.OrderStatus.Contains(content));
                        break;
                }
                if (qr != null && qr.ErrorCode == EnumErrorCode.SUCCESS)
                {
                    OrderdataGridView.DataSource = null;
                    OrderdataGridView.DataSource = qr.Data.ToList();
                }
                else if (qr != null)
                {
                    MessageBox.Show(qr.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }
    }
}
