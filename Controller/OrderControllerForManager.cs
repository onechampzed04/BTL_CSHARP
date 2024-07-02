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
using System.Xml.Linq;

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

        public ComboBox cbxTieuChi { get; private set; }
        public Button btnSearch { get; private set; }
        public TextBox txtSearchContent { get; private set; }

        public OrderControllerForManager(OrderFormForManager orderFormForManager, DataGridView OrderdataGridView, DataGridView OrderDetaildataGridView, Button btnUpdate, Button btnDelete, ComboBox cbxTieuChi, Button btnSearch, TextBox txtSearchContent)
        {
            OrderFormForManager = orderFormForManager;
            this.OrderdataGridView = OrderdataGridView;
            this.OrderDetaildataGridView = OrderDetaildataGridView;
            this.btnDelete = btnDelete;
            this.btnUpdate = btnUpdate;

            this.cbxTieuChi = cbxTieuChi;
            this.btnSearch = btnSearch;
            this.txtSearchContent = txtSearchContent;

            SetEvent();
        }

        public void SetEvent()
        {
            OrderFormForManager.Load += new EventHandler((object sender, EventArgs e) => LoadData());
            OrderdataGridView.RowHeaderMouseClick += DataGridView_RowHeaderMouseClick;
            btnSearch.Click += Search;
            OrderDetaildataGridView.CellValueChanged += OrderDetaildataGridView_CellValueChanged;
            btnUpdate.Click += Update;
            btnDelete.Click += Delete;

        }

        private void Delete(object sender, EventArgs e)
        {
            if (OrderdataGridView.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = OrderdataGridView.SelectedRows[0];
                int orderId = int.Parse(selectedRow.Cells[0].Value.ToString());

                // Xóa các chi tiết đơn hàng liên quan
                var orderDetails = dataContext.OrderDetails.Where(od => od.OrderID == orderId).ToList();
                dataContext.OrderDetails.DeleteAllOnSubmit(orderDetails);

                // Xóa đơn hàng
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
                // Load lại
                OrderDetaildataGridView.DataSource = null;
                LoadData();
            }
        }

        private void Update(object sender, EventArgs e)
        {
            if (OrderdataGridView.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = OrderdataGridView.SelectedRows[0];
                int orderId = int.Parse(selectedRow.Cells[0].Value.ToString());

                // Tính tổng tiền
                decimal totalAmount = 0;
                foreach (DataGridViewRow row in OrderDetaildataGridView.Rows)
                {
                    if (row.Cells["Price"].Value != null)
                    {
                        totalAmount += Convert.ToDecimal(row.Cells["Price"].Value);
                    }
                }

                // TotalAmount 
                selectedRow.Cells["TotalAmount"].Value = totalAmount;

                // Update
                var order = dataContext.Orders.FirstOrDefault(o => o.OrderID == orderId);
                if (order != null)
                {
                    order.TotalAmount = totalAmount;
                    dataContext.SubmitChanges();
                    string str = string.Format(Constants.update_success, Name);
                    MessageBox.Show(str, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show(Constants.not_found, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        }

        private void DataGridView_RowHeaderMouseClick(object sender, EventArgs e)
        {
            if (OrderdataGridView.SelectedRows.Count > 0)
            {

                btnDelete.Enabled = true;
                btnUpdate.Enabled = true;
            }
            DataGridViewRow row = OrderdataGridView.SelectedRows[0];
            int currentRowIndex = row.Index;
            int id = int.Parse(row.Cells[0].Value.ToString());
            var qr = dataContext.OrderDetails.Where(o => o.OrderID == id);
            OrderDetaildataGridView.DataSource = null;
            OrderDetaildataGridView.DataSource = qr;
            foreach (DataGridViewColumn column in OrderDetaildataGridView.Columns)
            {
                if (column.Name != "Quantity")
                {
                    column.ReadOnly = true;
                }
                else
                {
                    column.ReadOnly = false;
                }
            }


        }

        private void LoadData()
        {

            FuncResult<List<Order>> rs = FuncShares<Order>.GetAllData();
            switch (rs.ErrorCode)
            {
                case EnumErrorCode.ERROR:
                    MessageBox.Show(rs.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case EnumErrorCode.SUCCESS:
                    OrderdataGridView.DataSource = rs.Data;
                    break;
                case EnumErrorCode.FAILED:
                    break;
            }
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
                //OrderdataGridView.DataSource = null;
                //OrderdataGridView.DataSource = qr.Data.ToList();
            }
        }

        private void OrderDetaildataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == OrderDetaildataGridView.Columns["Quantity"].Index && e.RowIndex >= 0)
            {
                DataGridViewRow row = OrderDetaildataGridView.Rows[e.RowIndex];
                int quantity = int.Parse(row.Cells["Quantity"].Value.ToString());
                int productId = int.Parse(row.Cells["ProductID"].Value.ToString());

                var product = dataContext.Products.FirstOrDefault(p => p.ProductID == productId);
                if (product != null)
                {
                    decimal productPrice = product.Price;
                    decimal newPrice = quantity * productPrice;

                    row.Cells["Price"].Value = newPrice;


                    int orderDetailId = int.Parse(row.Cells["OrderDetailID"].Value.ToString());
                    var orderDetail = dataContext.OrderDetails.FirstOrDefault(od => od.OrderDetailID == orderDetailId);
                    if (orderDetail != null)
                    {
                        orderDetail.Quantity = quantity;
                        orderDetail.Price = newPrice;
                        dataContext.SubmitChanges();
                    }
                }
            }
        }
        private void BackupUserData(Order order)
        {
            string backupData = $"ID: {order.OrderID}, Date: {order.OrderDate}, Details: {order.OrderDetails}, Status: {order.OrderStatus}";
            System.IO.File.AppendAllText("backupOrderData.txt", backupData + Environment.NewLine);
        }


    }
}
