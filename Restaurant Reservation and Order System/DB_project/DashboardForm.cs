using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DB_project
{
    public partial class DashboardForm : Form
    {
        string conStr = @"Data Source=SAAD-570\SQLEXPRESS;Initial Catalog=RestaurantDB;Integrated Security=True;Encrypt=False";

        public DashboardForm()
        {
            InitializeComponent();
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
            LoadDashboardStats();
            LoadChartData();
        }

        void LoadDashboardStats()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();

                    // Total Reservations
                    SqlCommand cmd1 = new SqlCommand("SELECT COUNT(*) FROM Reservations", con);
                    int totalRes = Convert.ToInt32(cmd1.ExecuteScalar());

                    // New Orders
                    SqlCommand cmd2 = new SqlCommand("SELECT COUNT(*) FROM Orders", con);
                    int totalOrders = Convert.ToInt32(cmd2.ExecuteScalar());

                    // Free Tables (assume 10 total)
                    SqlCommand cmd3 = new SqlCommand(
                        "SELECT COUNT(DISTINCT TableNumber) FROM Reservations", con);
                    int usedTables = Convert.ToInt32(cmd3.ExecuteScalar());
                    int freeTables = 10 - usedTables;

                    // label2 = Total Reservations, label3 = New Orders, label4 = Tables Free
                    label2.Text = "Total Reservation : " + totalRes;
                    label3.Text = "New Orders : " + totalOrders;
                    label4.Text = "Current Tables Free : " + freeTables;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Stats Error: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void LoadChartData()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"
                        SELECT DayName, SUM(Amount) AS Total
                        FROM Sales
                        GROUP BY DayName
                        ORDER BY CASE DayName
                            WHEN 'Mon' THEN 1
                            WHEN 'Tue' THEN 2
                            WHEN 'Wed' THEN 3
                            WHEN 'Thu' THEN 4
                            WHEN 'Fri' THEN 5
                            WHEN 'Sat' THEN 6
                            WHEN 'Sun' THEN 7
                            ELSE 8 END", con);

                    SqlDataReader dr = cmd.ExecuteReader();

                    chart1.Series["Series1"].Points.Clear();
                    chart1.Series["Series1"].ChartType = SeriesChartType.Column;

                    while (dr.Read())
                    {
                        chart1.Series["Series1"].Points.AddXY(
                            dr["DayName"].ToString(),
                            Convert.ToDouble(dr["Total"])
                        );
                    }
                    dr.Close();

                    // Chart display settings
                    chart1.Series["Series1"].IsValueShownAsLabel = true;
                    chart1.Series["Series1"]["PointWidth"] = "0.6";
                    chart1.Series["Series1"].Color = System.Drawing.Color.SteelBlue;
                    chart1.ChartAreas[0].AxisX.Interval = 1;
                    chart1.ChartAreas[0].AxisX.LabelStyle.Angle = 0;
                    chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                    chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chart Error: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Navigation buttons
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Aap pehle se Dashboard par hain.",
                "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new ReservationsForm().Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new OrderForm().Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new Form2().Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Logout karna chahte hain?", "Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                new Form1().Show();
                this.Close();
            }
        }

        private void label4_Click(object sender, EventArgs e) { }
    }
}
