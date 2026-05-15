using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DB_project
{
    public partial class Form1 : Form
    {
        string conStr = @"Data Source=SAAD-570\SQLEXPRESS;Initial Catalog=RestaurantDB;Integrated Security=True;Encrypt=False";

        public Form1()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUser.Text) ||
                string.IsNullOrWhiteSpace(txtPass.Text))
            {
                MessageBox.Show("Username aur Password enter karein!",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_Login", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", txtUser.Text.Trim());
                    cmd.Parameters.AddWithValue("@Password", txtPass.Text.Trim());

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("Login Successful! Dashboard khul raha hai...",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        new DashboardForm().Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Ghalat Username ya Password!",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtUser.Clear();
                        txtPass.Clear();
                        txtUser.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnlogin_Click_1(object sender, EventArgs e) { btnLogin_Click(sender, e); }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void Form1_Load(object sender, EventArgs e) { }
    }
}
