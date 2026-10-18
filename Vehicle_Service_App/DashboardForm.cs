using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Vehicle_Service_App
{
    public partial class DashboardForm : Form
    {
        public DashboardForm()
        {
            InitializeComponent();
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
            LoadDashboardStats();
            LoadRecentServices();
        }

        private void LoadDashboardStats()
        {
            string connString = Properties.Settings.Default.ConnString;
            SqlConnection con = new SqlConnection(connString);

            try
            {
                con.Open();

                // Total Customers
                SqlCommand cmd1 = new SqlCommand("SELECT COUNT(*) FROM Customer", con);
                label5.Text = cmd1.ExecuteScalar().ToString();

                // Total Vehicles
                SqlCommand cmd2 = new SqlCommand("SELECT COUNT(*) FROM Vehicle", con);
                label6.Text = cmd2.ExecuteScalar().ToString();

                // Today's Services
                SqlCommand cmd3 = new SqlCommand(
                    "SELECT COUNT(*) FROM ServiceRecords WHERE CAST(ServiceDate AS DATE) = CAST(GETDATE() AS DATE)", con);
                label7.Text = cmd3.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Dashboard stats error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        
        private void LoadRecentServices()
        {
            string connString = Properties.Settings.Default.ConnString;
            SqlConnection con = new SqlConnection(connString);

            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(
                    "SELECT ServiceRecords.JobNo, " +
                    "Customer.FullName, " +
                    "Vehicle.RegistrationNo, " +
                    "Services.ServiceName, " +
                    "ServiceRecords.ServiceDate, " +
                    "ServiceRecords.Amount " +
                    "FROM Customer " +
                    "INNER JOIN Vehicle ON Customer.CustomerId = Vehicle.CustomerId " +
                    "INNER JOIN ServiceRecords ON Vehicle.VehicleId = ServiceRecords.VehicleId " +
                    "INNER JOIN Services ON ServiceRecords.ServiceId = Services.ServiceId", con);

                SqlDataReader rd = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(rd);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Recent services error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CustomerForm customer = new CustomerForm();
            customer.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            VehicleForm vehicle = new VehicleForm();
            vehicle.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ServiceForm service = new ServiceForm();
            service.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ServiceRecordsForm ser_rec = new ServiceRecordsForm();
            ser_rec.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ReportsForm report = new ReportsForm();
            report.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            login.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DashboardForm dash = new DashboardForm();
            dash.Show();
            this.Hide();
        }
    }
}
