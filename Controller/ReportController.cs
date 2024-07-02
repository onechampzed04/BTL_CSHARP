using BTL_2.Model;
using BTL_2.Shareds;
using BTL_2.View;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace BTL_2.Controller
{
    public class ReportController
    {
        private readonly ReportForm reportForm;
        private readonly ComboBox cbxMonth;
        private readonly Chart chart1;
        private readonly Chart chart2;

        public ReportController(ReportForm reportForm, ComboBox cbxMonth, Chart chart1, Chart chart2)
        {
            this.reportForm = reportForm;
            this.cbxMonth = cbxMonth;
            this.chart1 = chart1;
            this.chart2 = chart2;
            SetEvent();
        }

        public void SetEvent()
        {
            reportForm.Load += ReportForm_Load;
            cbxMonth.SelectedValueChanged += CbxMonth_SelectedValueChanged;
        }

        private void CbxMonth_SelectedValueChanged(object sender, EventArgs e)
        {
            string selectedMonth = cbxMonth.SelectedItem.ToString();
            DrawProductSalesChart(selectedMonth);
            //DrawTotalSalesChart(selectedMonth);
        }

        private void ReportForm_Load(object sender, EventArgs e)
        {
            // Add months to ComboBox
            for (int month = 1; month <= 12; month++)
            {
                cbxMonth.Items.Add(month.ToString("D2")); // Display month with 2 digits
            }
            cbxMonth.SelectedIndex = DateTime.Now.Month - 1; // Select current month initially

            // Draw charts for current selected month
            string selectedMonth = cbxMonth.SelectedItem.ToString();
            DrawProductSalesChart(selectedMonth);
            DrawTotalSalesChart();
        }

        private void DrawProductSalesChart(string selectedMonth)
        {
            FuncResult<List<Product>> productsResult = FuncShares<Product>.GetAllData();
            FuncResult<List<OrderDetail>> orderDetailsResult = FuncShares<OrderDetail>.GetAllData();

            if (productsResult.ErrorCode == EnumErrorCode.SUCCESS && orderDetailsResult.ErrorCode == EnumErrorCode.SUCCESS)
            {
                var products = productsResult.Data;
                var orderDetails = orderDetailsResult.Data;

                var query = from p in products
                            join od in orderDetails on p.ProductID equals od.ProductID into productOrderDetails
                            from od in productOrderDetails.DefaultIfEmpty()
                            where od != null && od.Order.OrderDate.Month.ToString("D2") == selectedMonth
                            group od by new { p.ProductID, p.ProductName } into grouped
                            select new
                            {
                                ProductID = grouped.Key.ProductID,
                                ProductName = grouped.Key.ProductName,
                                TotalQuantitySold = grouped.Sum(x => x.Quantity)
                            };

                chart1.Series.Clear();
                chart1.ChartAreas.Clear();

                ChartArea chartArea1 = new ChartArea();
                chart1.ChartAreas.Add(chartArea1);

                Series series1 = new Series("Product Sales");
                series1.ChartType = SeriesChartType.Column;

                // Enable tooltip
                series1.ToolTip = "#VALY";

                foreach (var item in query)
                {
                    DataPoint point = new DataPoint();
                    point.AxisLabel = $"{item.ProductID} - {item.ProductName}";
                    point.YValues = new double[] { item.TotalQuantitySold };
                    point.ToolTip = $"Quantity Sold: {item.TotalQuantitySold}";
                    series1.Points.Add(point);
                }

                chart1.Series.Add(series1);

                // Enable zooming and scrolling
                chartArea1.CursorX.IsUserEnabled = true;
                chartArea1.CursorX.IsUserSelectionEnabled = true;
                chartArea1.AxisX.ScaleView.Zoomable = true;
                chartArea1.AxisX.ScrollBar.IsPositionedInside = true;
            }
            else
            {
                MessageBox.Show(productsResult.ErrorDesc);
            }
        }

        private void DrawTotalSalesChart()
        {
            FuncResult<List<Order>> ordersResult = FuncShares<Order>.GetAllData();

            if (ordersResult.ErrorCode == EnumErrorCode.SUCCESS)
            {
                var orders = ordersResult.Data;

                // Tạo mảng các tháng từ 1 đến 12
                int[] months = Enumerable.Range(1, 12).ToArray();

                // Truy vấn tổng doanh thu theo từng tháng có trong cơ sở dữ liệu
                var query = from o in orders
                            where o.OrderDate.Year == DateTime.Now.Year // Lọc theo năm hiện tại
                            group o by o.OrderDate.Month into grouped
                            select new
                            {
                                Month = grouped.Key,
                                TotalSales = grouped.Sum(x => x.TotalAmount)
                            };

                chart2.Series.Clear();
                chart2.ChartAreas.Clear();

                ChartArea chartArea2 = new ChartArea();
                chart2.ChartAreas.Add(chartArea2);

                Series series2 = new Series("Total Sales");
                series2.ChartType = SeriesChartType.Column;

                // Enable tooltip
                series2.ToolTip = "#VALY";

                // Điền dữ liệu vào biểu đồ
                foreach (int month in months)
                {
                    var salesData = query.FirstOrDefault(x => x.Month == month);
                    double totalSales = (double)((salesData != null) ? salesData.TotalSales : 0);

                    DataPoint point = new DataPoint();
                    point.AxisLabel = $"Month {month}";
                    point.YValues = new double[] { totalSales };
                    point.ToolTip = $"Total Amount: {totalSales}";
                    series2.Points.Add(point);
                }

                chart2.Series.Add(series2);

                // Enable zooming and scrolling
                chartArea2.CursorX.IsUserEnabled = true;
                chartArea2.CursorX.IsUserSelectionEnabled = true;
                chartArea2.AxisX.ScaleView.Zoomable = true;
                chartArea2.AxisX.ScrollBar.IsPositionedInside = true;
            }
            else
            {
                MessageBox.Show(ordersResult.ErrorDesc);
            }
        }
    }
}
