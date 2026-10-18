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
using System.IO;

namespace Vehicle_Service_App
{
    public partial class ReportsForm : Form
    {
        public ReportsForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DashboardForm dash = new DashboardForm();
            dash.Show();
            this.Hide();
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

        private void ReportsForm_Load(object sender, EventArgs e)
        {
            LoadSummaryStats();
            LoadVehicleCombo();
            LoadCustomerCombo();
            LoadFullReport();
        }

        
        private void LoadSummaryStats()
        {
            string connString = Properties.Settings.Default.ConnString;
            SqlConnection con = new SqlConnection(connString);

            try
            {
                con.Open();

                SqlCommand cmd1 = new SqlCommand("SELECT COUNT(*) FROM Customer", con);
                label5.Text = cmd1.ExecuteScalar().ToString();

                SqlCommand cmd2 = new SqlCommand("SELECT COUNT(*) FROM Vehicle", con);
                label6.Text = cmd2.ExecuteScalar().ToString();

                SqlCommand cmd3 = new SqlCommand(
                    "SELECT COUNT(*) FROM ServiceRecords WHERE CAST(ServiceDate AS DATE) = CAST(GETDATE() AS DATE)", con);
                label7.Text = cmd3.ExecuteScalar().ToString();

                SqlCommand cmd4 = new SqlCommand("SELECT ISNULL(SUM(Amount), 0) FROM ServiceRecords", con);
                label12.Text = cmd4.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Summary stats error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        
        private void LoadVehicleCombo()
        {
            string connString = Properties.Settings.Default.ConnString;
            SqlConnection con = new SqlConnection(connString);

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT VehicleId, RegistrationNo FROM Vehicle", con);
                SqlDataReader rd = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(rd);

                comboBox1.DisplayMember = "RegistrationNo";
                comboBox1.ValueMember = "VehicleId";
                comboBox1.DataSource = dt;
                comboBox1.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Vehicle combo error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        
        private void LoadCustomerCombo()
        {
            string connString = Properties.Settings.Default.ConnString;
            SqlConnection con = new SqlConnection(connString);

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT CustomerId, FullName FROM Customer", con);
                SqlDataReader rd = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(rd);

                comboBox2.DisplayMember = "FullName";
                comboBox2.ValueMember = "CustomerId";
                comboBox2.DataSource = dt;
                comboBox2.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Customer combo error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        
        private void LoadFullReport()
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
                    "ServiceRecords.KmReading, " +
                    "ServiceRecords.Amount " +
                    "FROM ServiceRecords " +
                    "INNER JOIN Customer ON ServiceRecords.CustomerId = Customer.CustomerId " +
                    "INNER JOIN Vehicle ON ServiceRecords.VehicleId = Vehicle.VehicleId " +
                    "INNER JOIN Services ON ServiceRecords.ServiceId = Services.ServiceId", con);

                SqlDataReader rd = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(rd);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Report load error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string connString = Properties.Settings.Default.ConnString;
            SqlConnection con = new SqlConnection(connString);

            try
            {
                con.Open();

                string query =
                    "SELECT ServiceRecords.JobNo, " +
                    "Customer.FullName, " +
                    "Vehicle.RegistrationNo, " +
                    "Services.ServiceName, " +
                    "ServiceRecords.ServiceDate, " +
                    "ServiceRecords.KmReading, " +
                    "ServiceRecords.Amount " +
                    "FROM ServiceRecords " +
                    "INNER JOIN Customer ON ServiceRecords.CustomerId = Customer.CustomerId " +
                    "INNER JOIN Vehicle ON ServiceRecords.VehicleId = Vehicle.VehicleId " +
                    "INNER JOIN Services ON ServiceRecords.ServiceId = Services.ServiceId " +
                    "WHERE ServiceRecords.ServiceDate >= @FromDate " +
                    "AND ServiceRecords.ServiceDate <= @ToDate";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@FromDate", dateTimePicker1.Value.Date);
                cmd.Parameters.AddWithValue("@ToDate", dateTimePicker2.Value.Date);

                if (comboBox1.SelectedIndex == -1)
                    cmd.Parameters.AddWithValue("@VehicleId", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@VehicleId", Convert.ToInt32(comboBox1.SelectedValue));

                if (comboBox2.SelectedIndex == -1)
                    cmd.Parameters.AddWithValue("@CustomerId", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@CustomerId", Convert.ToInt32(comboBox2.SelectedValue));

                SqlDataReader rd = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(rd);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No data to export");
                return;
            }

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "CSV files (*.csv)|*.csv";
            saveDialog.FileName = "ServiceHistoryReport_" + DateTime.Now.ToString("yyyyMMdd") + ".csv";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamWriter sw = new StreamWriter(saveDialog.FileName);

                    // Column headers
                    for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {
                        sw.Write(dataGridView1.Columns[i].HeaderText);
                        if (i < dataGridView1.Columns.Count - 1)
                            sw.Write(",");
                    }
                    sw.WriteLine();

                    // Data rows
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            for (int i = 0; i < dataGridView1.Columns.Count; i++)
                            {
                                sw.Write(row.Cells[i].Value?.ToString() ?? "");
                                if (i < dataGridView1.Columns.Count - 1)
                                    sw.Write(",");
                            }
                            sw.WriteLine();
                        }
                    }

                    sw.Close();
                    MessageBox.Show("Report exported successfully to:\n" + saveDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Export error: " + ex.Message);
                }
            }
        }
    }
}
