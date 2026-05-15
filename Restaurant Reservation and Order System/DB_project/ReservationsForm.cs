using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DB_project
{
    public partial class ReservationsForm : Form
    {
        string conStr = @"Data Source=SAAD-570\SQLEXPRESS;Initial Catalog=RestaurantDB;Integrated Security=True;Encrypt=False";

        public ReservationsForm()
        {
            InitializeComponent();
        }

        private void ReservationsForm_Load(object sender, EventArgs e)
        {
            // ✅ FIX: Turn off AutoGenerateColumns so DB columns don't duplicate designer columns
            dataGridView1.AutoGenerateColumns = false;

            // ✅ FIX: Map each designer column to exact DB column name
            dataGridView1.Columns["Column1"].DataPropertyName = "ReservationID";
            dataGridView1.Columns["Column2"].DataPropertyName = "GuestName";
            dataGridView1.Columns["Column3"].DataPropertyName = "ReservationDate";
            dataGridView1.Columns["Column4"].DataPropertyName = "Time";
            dataGridView1.Columns["Column5"].DataPropertyName = "TableNumber";
            dataGridView1.Columns["Column6"].DataPropertyName = "Status";

            LoadReservations();
        }

        void LoadReservations()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    // ✅ Simple SELECT — no alias so DataPropertyName matches exactly
                    SqlDataAdapter da = new SqlDataAdapter(
                        "SELECT ReservationID, GuestName, ReservationDate, Time, TableNumber, Status FROM Reservations ORDER BY ReservationID DESC", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load Error: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddRes_Click(object sender, EventArgs e)
        {
            // ✅ Validate all fields
            if (string.IsNullOrWhiteSpace(txtGuestName.Text) ||
                string.IsNullOrWhiteSpace(txtContact.Text)   ||
                string.IsNullOrWhiteSpace(txtTime.Text)      ||
                string.IsNullOrWhiteSpace(txtTable.Text))
            {
                MessageBox.Show("Tamam fields bharein — Name, Contact, Time aur Table!",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_AddReservation", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@GuestName",       txtGuestName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Contact",         txtContact.Text.Trim());
                    cmd.Parameters.AddWithValue("@ReservationDate", dptDate.Value.Date);
                    cmd.Parameters.AddWithValue("@Time",            txtTime.Text.Trim());
                    cmd.Parameters.AddWithValue("@TableNumber",     txtTable.Text.Trim());
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Reservation successfully add ho gayi!",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtGuestName.Clear();
                txtContact.Clear();
                txtTime.Clear();
                txtTable.Clear();

                LoadReservations(); // 🔄 Refresh grid
            }
            catch (Exception ex)
            {
                MessageBox.Show("DB Error: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Navigation
        private void btnDashboard_Click(object sender, EventArgs e)
        {
            new DashboardForm().Show();
            this.Hide();
        }

        private void btnOrderSystem_Click(object sender, EventArgs e)
        {
            new OrderForm().Show();
            this.Hide();
        }

        private void btnMenuScreen_Click(object sender, EventArgs e)
        {
            new Form2().Show();
            this.Hide();
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
