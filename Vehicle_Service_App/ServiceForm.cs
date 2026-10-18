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
    public partial class ServiceForm : Form
    {
        public ServiceForm()
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
            LoginForm login =new LoginForm();
            login.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            if (string.IsNullOrEmpty(textBox1.Text))
            {
                errorProvider1.SetError(textBox1, "Service ID is required");
                return;
            }

            if (string.IsNullOrEmpty(textBox2.Text))
            {
                errorProvider1.SetError(textBox2, "Service Name is required");
                return;
            }

            string connString = Properties.Settings.Default.ConnString;
            SqlConnection con = new SqlConnection(connString);

            try
            {
                int serviceId = Convert.ToInt32(textBox1.Text);

                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Services(ServiceId, ServiceName, Category, Cost, DurationMin, Description) " +
                    "VALUES(@ServiceId, @ServiceName, @Category, @Cost, @DurationMin, @Description)", con);

                cmd.Parameters.AddWithValue("@ServiceId", serviceId);
                cmd.Parameters.AddWithValue("@ServiceName", textBox2.Text);
                cmd.Parameters.AddWithValue("@Category", comboBox1.Text);
                cmd.Parameters.AddWithValue("@Cost", numericUpDown1.Value);
                cmd.Parameters.AddWithValue("@DurationMin", numericUpDown2.Value);
                cmd.Parameters.AddWithValue("@Description", textBox5.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Add Successful");

                LoadServices();
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
                MessageBox.Show("Select a Service From Grid");
                return;
            }

            string connString = Properties.Settings.Default.ConnString;
            SqlConnection con = new SqlConnection(connString);

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Services SET ServiceName=@ServiceName, Category=@Category, " +
                    "Cost=@Cost, DurationMin=@DurationMin, Description=@Description " +
                    "WHERE ServiceId=@ServiceId", con);

                cmd.Parameters.AddWithValue("@ServiceName", textBox2.Text);
                cmd.Parameters.AddWithValue("@Category", comboBox1.Text);
                cmd.Parameters.AddWithValue("@Cost", numericUpDown1.Value);
                cmd.Parameters.AddWithValue("@DurationMin", numericUpDown2.Value);
                cmd.Parameters.AddWithValue("@Description", textBox5.Text);
                cmd.Parameters.AddWithValue("@ServiceId", textBox1.Text);

                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                    MessageBox.Show("Service Updated Successfully");
                else
                    MessageBox.Show("Service not found");

                LoadServices();
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
                MessageBox.Show("Select a Service From Grid");
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Are you sure you want to delete this service?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
                return;

            string connString = Properties.Settings.Default.ConnString;
            SqlConnection con = new SqlConnection(connString);

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Services WHERE ServiceId=@ServiceId", con);
                cmd.Parameters.AddWithValue("@ServiceId", textBox1.Text);

                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                    MessageBox.Show("Service Deleted Successfully");
                else
                    MessageBox.Show("Service not found");

                LoadServices();
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
            textBox1.Clear();
            textBox2.Clear();
            comboBox1.SelectedIndex = -1;
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
            textBox5.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string connString = Properties.Settings.Default.ConnString;
            SqlConnection con = new SqlConnection(connString);

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Services WHERE ServiceName LIKE @Search OR Category LIKE @Search", con);
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
                LoadServices();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                textBox1.Text = row.Cells["ServiceId"].Value.ToString();
                textBox2.Text = row.Cells["ServiceName"].Value.ToString();
                comboBox1.Text = row.Cells["Category"].Value.ToString();
                numericUpDown1.Value = Convert.ToDecimal(row.Cells["Cost"].Value);
                numericUpDown2.Value = Convert.ToDecimal(row.Cells["DurationMin"].Value);
                textBox5.Text = row.Cells["Description"].Value.ToString();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.SetError(textBox1, "");
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.SetError(textBox2, "");
        }

        private void ServiceForm_Load_1(object sender, EventArgs e)
        {
            LoadServices();
            LoadCategoryCombo();
        }

        private void LoadServices()
        {
            string connString = Properties.Settings.Default.ConnString;
            SqlConnection con = new SqlConnection(connString);

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Services", con);
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
        
        private void LoadCategoryCombo()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add("General Service");
            comboBox1.Items.Add("Oil Change");
            comboBox1.Items.Add("Brake Service");
            comboBox1.Items.Add("Engine Repair");
            comboBox1.Items.Add("Tire Service");
            comboBox1.Items.Add("AC Service");
            comboBox1.Items.Add("Battery Service");
            comboBox1.Items.Add("Inspection");
        }
    }
}
