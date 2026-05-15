using System;
using System.Drawing;
using System.Windows.Forms;

namespace DB_project
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e) { }

        // ✅ Dashboard
        private void btnDashboard_Click(object sender, EventArgs e)
        {
            try { new DashboardForm().Show(); this.Hide(); }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ✅ Reservations — matches btnRservation in designer (typo in designer kept)
        private void btnReservations_Click(object sender, EventArgs e)
        {
            try { new ReservationsForm().Show(); this.Hide(); }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ✅ Order System
        private void btnOrderSystem_Click(object sender, EventArgs e)
        {
            try { new OrderForm().Show(); this.Hide(); }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ✅ Menu Screen — already here, just show message
        private void btnMenuScreen_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Aap pehle se Menu Screen par hain!",
                "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ✅ Logout — with confirmation
        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Kya aap logout karna chahte hain?", "Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                new Form1().Show();
                this.Close();
            }
        }

        // ✅ Menu item label clicks
        private void label6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Zinger Burger - Rs. 350\n\nOrder karne ke liye Order System open karein.",
                "Menu Item", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void label7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("BBQ Tikka Pizza - Rs. 850\n\nOrder karne ke liye Order System open karein.",
                "Menu Item", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void label8_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Alfredo Pasta - Rs. 750\n\nOrder karne ke liye Order System open karein.",
                "Menu Item", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Designer protection — do not remove
        private void panel4_Paint(object sender, PaintEventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void btnlogin_Click_1(object sender, EventArgs e) { }
    }
}
