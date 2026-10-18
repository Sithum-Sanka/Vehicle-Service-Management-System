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
    public partial class ServiceRecordsForm : Form
    {
        public ServiceRecordsForm()
        {
            InitializeComponent();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            DashboardForm dash = new DashboardForm();
            dash.Show();
            this.Hide();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            CustomerForm customer = new CustomerForm();
            customer.Show();
            this.Hide();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            VehicleForm vehicle = new VehicleForm();
            vehicle.Show();
            this.Hide();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ServiceForm service = new ServiceForm();
            service.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
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

        
        private void ServiceRecordsForm_Load(object sender, EventArgs e)
        {
            LoadRecords();
            LoadCustomerCombo();
            LoadServiceCombo();
        }

        private void LoadRecords()
        {
            string connString = Properties.Settings.Default.ConnString;
            SqlConnection con = new SqlConnection(connString);

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM ServiceRecords", con);
                SqlDataReader rd = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(rd);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load error: " + ex.Message);
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

                comboBox1.DisplayMember = "FullName";
                comboBox1.ValueMember = "CustomerId";
                comboBox1.DataSource = dt;
                comboBox1.SelectedIndex = -1;
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

        
        private void LoadServiceCombo()
        {
            string connString = Properties.Settings.Default.ConnString;
            SqlConnection con = new SqlConnection(connString);

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT ServiceId, ServiceName FROM Services", con);
                SqlDataReader rd = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(rd);

                comboBox3.DisplayMember = "ServiceName";
                comboBox3.ValueMember = "ServiceId";
                comboBox3.DataSource = dt;
                comboBox3.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Service combo error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1 || comboBox1.SelectedValue == null || comboBox1.SelectedValue == DBNull.Value)
            {
                comboBox2.DataSource = null;
                return;
            }

            int custId = Convert.ToInt32(comboBox1.SelectedValue);

            string connString = Properties.Settings.Default.ConnString;
            SqlConnection con = new SqlConnection(connString);

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT VehicleId, RegistrationNo FROM Vehicle WHERE CustomerId=@CustomerId", con);
                cmd.Parameters.AddWithValue("@CustomerId", custId);

                SqlDataReader rd = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(rd);

                comboBox2.DisplayMember = "RegistrationNo";
                comboBox2.ValueMember = "VehicleId";
                comboBox2.DataSource = dt;
                comboBox2.SelectedIndex = -1;
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

        
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Select a Customer");
                return;
            }

            if (comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Select a Vehicle");
                return;
            }

            if (comboBox3.SelectedIndex == -1)
            {
                MessageBox.Show("Select a Service");
                return;
            }

            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Job No is required");
                return;
            }

            string connString = Properties.Settings.Default.ConnString;
            SqlConnection con = new SqlConnection(connString);

            try
            {
                int jobNo = Convert.ToInt32(textBox1.Text);
                int custId = Convert.ToInt32(comboBox1.SelectedValue);
                int vehicleId = Convert.ToInt32(comboBox2.SelectedValue);
                int serviceId = Convert.ToInt32(comboBox3.SelectedValue);

                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO ServiceRecords(JobNo, CustomerId, VehicleId, ServiceId, ServiceDate, KmReading, Amount) " +
                    "VALUES(@JobNo, @CustomerId, @VehicleId, @ServiceId, @ServiceDate, @KmReading, @Amount)", con);

                cmd.Parameters.AddWithValue("@JobNo", jobNo);
                cmd.Parameters.AddWithValue("@CustomerId", custId);
                cmd.Parameters.AddWithValue("@VehicleId", vehicleId);
                cmd.Parameters.AddWithValue("@ServiceId", serviceId);
                cmd.Parameters.AddWithValue("@ServiceDate", dateTimePicker1.Value.Date);
                cmd.Parameters.AddWithValue("@KmReading", textBox3.Text);
                cmd.Parameters.AddWithValue("@Amount", textBox4.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Add Successful");

                LoadRecords();
                button4_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Program interrupt: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

       
        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Select a Job From Grid");
                return;
            }

            string connString = Properties.Settings.Default.ConnString;
            SqlConnection con = new SqlConnection(connString);

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE ServiceRecords SET CustomerId=@CustomerId, VehicleId=@VehicleId, " +
                    "ServiceId=@ServiceId, ServiceDate=@ServiceDate, KmReading=@KmReading, Amount=@Amount " +
                    "WHERE JobNo=@JobNo", con);

                cmd.Parameters.AddWithValue("@CustomerId", Convert.ToInt32(comboBox1.SelectedValue));
                cmd.Parameters.AddWithValue("@VehicleId", Convert.ToInt32(comboBox2.SelectedValue));
                cmd.Parameters.AddWithValue("@ServiceId", Convert.ToInt32(comboBox3.SelectedValue));
                cmd.Parameters.AddWithValue("@ServiceDate", dateTimePicker1.Value.Date);
                cmd.Parameters.AddWithValue("@KmReading", textBox3.Text);
                cmd.Parameters.AddWithValue("@Amount", textBox4.Text);
                cmd.Parameters.AddWithValue("@JobNo", textBox1.Text);

                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                    MessageBox.Show("Record Updated Successfully");
                else
                    MessageBox.Show("Job not found");

                LoadRecords();
                button4_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Select a Job From Grid");
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Are you sure you want to delete this record?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
                return;

            string connString = Properties.Settings.Default.ConnString;
            SqlConnection con = new SqlConnection(connString);

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM ServiceRecords WHERE JobNo=@JobNo", con);
                cmd.Parameters.AddWithValue("@JobNo", textBox1.Text);

                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                    MessageBox.Show("Record Deleted Successfully");
                else
                    MessageBox.Show("Job not found");

                LoadRecords();
                button4_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Delete error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        
        private void button4_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = -1;
            comboBox2.DataSource = null;
            comboBox3.SelectedIndex = -1;
            textBox1.Clear();
            textBox3.Clear();
            textBox4.Clear();
            dateTimePicker1.Value = DateTime.Now;
        }

        
        private void button5_Click(object sender, EventArgs e)
        {
            string connString = Properties.Settings.Default.ConnString;
            SqlConnection con = new SqlConnection(connString);

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM ServiceRecords WHERE CAST(JobNo AS VARCHAR) LIKE @Search", con);
                cmd.Parameters.AddWithValue("@Search", "%" + textBox6.Text + "%");

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

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox6.Text))
            {
                LoadRecords();
            }
        }

  
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                textBox1.Text = row.Cells["JobNo"].Value.ToString();
                comboBox1.SelectedValue = Convert.ToInt32(row.Cells["CustomerId"].Value);
                
                comboBox2.SelectedValue = Convert.ToInt32(row.Cells["VehicleId"].Value);
                comboBox3.SelectedValue = Convert.ToInt32(row.Cells["ServiceId"].Value);
                dateTimePicker1.Value = Convert.ToDateTime(row.Cells["ServiceDate"].Value);
                textBox3.Text = row.Cells["KmReading"].Value.ToString();
                textBox4.Text = row.Cells["Amount"].Value.ToString();
            }
        }
    }
}