using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DB_project
{
    public partial class OrderForm : Form
    {
        string conStr = @"Data Source=SAAD-570\SQLEXPRESS;Initial Catalog=RestaurantDB;Integrated Security=True;Encrypt=False";

        public OrderForm()
        {
            InitializeComponent();
        }

        private void OrderForm_Load(object sender, EventArgs e)
        {
            // ✅ FIX: Turn off AutoGenerateColumns to prevent duplicate columns
            dgvOrder.AutoGenerateColumns = false;

            // ✅ FIX: Map designer columns to DB columns exactly
            dgvOrder.Columns["Column1"].DataPropertyName = "ItemName";
            dgvOrder.Columns["Column2"].DataPropertyName = "Price";
            dgvOrder.Columns["Column3"].DataPropertyName = "Quantity";
            dgvOrder.Columns["Column4"].DataPropertyName = "SubTotal";

            LoadOrders();
        }

        void LoadOrders()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(
                        "SELECT ItemName, Price, Quantity, SubTotal FROM Orders ORDER BY OrderID DESC", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvOrder.DataSource = dt;
                }
                CalculateTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load Error: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddToOrder_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtItem.Text) ||
                string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Item aur Price likhein!",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!double.TryParse(txtPrice.Text, out double price) || price <= 0)
            {
                MessageBox.Show("Price mein sirf valid number likhein!",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int qty = (int)numQty.Value;
            if (qty <= 0)
            {
                MessageBox.Show("Quantity kam se kam 1 honi chahiye!",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_AddOrder", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ItemName", txtItem.Text.Trim());
                    cmd.Parameters.AddWithValue("@Price",    price);
                    cmd.Parameters.AddWithValue("@Quantity", qty);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Order Added Successfully!",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtItem.Clear();
                txtPrice.Clear();
                numQty.Value = 1;
                txtItem.Focus();
                LoadOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show("DB Error: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void CalculateTotal()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_GetTotalBill", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    object result = cmd.ExecuteScalar();
                    double total = (result == DBNull.Value || result == null)
                        ? 0 : Convert.ToDouble(result);
                    lblTotal.Text = "Total: RS." + total.ToString("F2");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Total Error: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Navigation
        private void btnDashboard_Click(object sender, EventArgs e)
        {
            new DashboardForm().Show(); this.Hide();
        }

        private void btnReservations_Click(object sender, EventArgs e)
        {
            new ReservationsForm().Show(); this.Hide();
        }

        private void btnOrderSystem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Aap pehle se Order System par hain.",
                "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnMenuScreen_Click(object sender, EventArgs e)
        {
            new Form2().Show(); this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
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
    }
}
