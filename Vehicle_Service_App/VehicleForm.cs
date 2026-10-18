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
    public partial class VehicleForm : Form
    {
        public VehicleForm()
        {
            InitializeComponent();
        }




        
        private void VehicleForm_Load(object sender, EventArgs e)
        {
            LoadVehicles();
            LoadCustomerCombo();
            LoadMakeCombo();
            LoadFuelCombo();
        }

        
        private void LoadVehicles()
        {
            string connString = Properties.Settings.Default.ConnString;
            SqlConnection con = new SqlConnection(connString);

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Vehicle", con);
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

                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "FullName";   
                comboBox1.ValueMember = "CustomerId";   
                comboBox1.SelectedIndex = -1;           
            }
            catch (Exception ex)
            {
                MessageBox.Show("Customer combo load error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        
        private void LoadMakeCombo()
        {
            comboBox2.Items.Clear();
            comboBox2.Items.Add("Toyota");
            comboBox2.Items.Add("BMW");
            comboBox2.Items.Add("Audi");
            comboBox2.Items.Add("Suzuki");
            comboBox2.Items.Add("Honda");
            comboBox2.Items.Add("Nissan");
            comboBox2.Items.Add("Mercedes-Benz");
        }

        
        private void LoadFuelCombo()
        {
            comboBox3.Items.Clear();
            comboBox3.Items.Add("Petrol");
            comboBox3.Items.Add("Diesel");
            comboBox3.Items.Add("Electric");
            comboBox3.Items.Add("Hybrid");
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            if (string.IsNullOrEmpty(textBox1.Text))
            {
                errorProvider1.SetError(textBox1, "Vehicle ID is required");
                return;
            }

            if (string.IsNullOrEmpty(textBox2.Text))
            {
                errorProvider1.SetError(textBox2, "Registration No is required");
                return;
            }

            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Select a Customer");
                return;
            }

            string connString = Properties.Settings.Default.ConnString;
            SqlConnection con = new SqlConnection(connString);

            try
            {
                int vehicleId = Convert.ToInt32(textBox1.Text);
                string regNo = textBox2.Text;
                int custId = Convert.ToInt32(comboBox1.SelectedValue); 
                string make = comboBox2.Text;
                string model = textBox5.Text;
                int year = Convert.ToInt32(textBox3.Text);
                string fuel = comboBox3.Text;

                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Vehicle(VehicleId, RegistrationNo, CustomerId, Make, Model, Year, FuelType) " +
                    "VALUES(@VehicleId, @RegNo, @CustomerId, @Make, @Model, @Year, @Fuel)", con);

                cmd.Parameters.AddWithValue("@VehicleId", vehicleId);
                cmd.Parameters.AddWithValue("@RegNo", regNo);
                cmd.Parameters.AddWithValue("@CustomerId", custId);
                cmd.Parameters.AddWithValue("@Make", make);
                cmd.Parameters.AddWithValue("@Model", model);
                cmd.Parameters.AddWithValue("@Year", year);
                cmd.Parameters.AddWithValue("@Fuel", fuel);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Add Successful");

                LoadVehicles(); 
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

        
        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            textBox5.Clear();
            comboBox3.SelectedIndex = -1;
        }
       

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Select a Vehicle From Grid");
                return;
            }

            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Select a Customer");
                return;
            }

            string connString = Properties.Settings.Default.ConnString;
            SqlConnection con = new SqlConnection(connString);

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Vehicle SET RegistrationNo=@RegNo, CustomerId=@CustomerId, " +
                    "Make=@Make, Model=@Model, Year=@Year, FuelType=@Fuel WHERE VehicleId=@VehicleId", con);

                cmd.Parameters.AddWithValue("@RegNo", textBox2.Text);
                cmd.Parameters.AddWithValue("@CustomerId", Convert.ToInt32(comboBox1.SelectedValue));
                cmd.Parameters.AddWithValue("@Make", comboBox2.Text);
                cmd.Parameters.AddWithValue("@Model", textBox5.Text);
                cmd.Parameters.AddWithValue("@Year", Convert.ToInt32(textBox3.Text));
                cmd.Parameters.AddWithValue("@Fuel", comboBox3.Text);
                cmd.Parameters.AddWithValue("@VehicleId", textBox1.Text);

                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                    MessageBox.Show("Vehicle Updated Successfully");
                else
                    MessageBox.Show("Vehicle not found");

                LoadVehicles();
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
                MessageBox.Show("Select a Vehicle From Grid");
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Are you sure you want to delete this vehicle?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
                return;

            string connString = Properties.Settings.Default.ConnString;
            SqlConnection con = new SqlConnection(connString);

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Vehicle WHERE VehicleId=@VehicleId", con);
                cmd.Parameters.AddWithValue("@VehicleId", textBox1.Text);

                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                    MessageBox.Show("Vehicle Deleted Successfully");
                else
                    MessageBox.Show("Vehicle not found");

                LoadVehicles();
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


        private void button5_Click(object sender, EventArgs e)
        {
            string connString = Properties.Settings.Default.ConnString;
            SqlConnection con = new SqlConnection(connString);

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Vehicle WHERE RegistrationNo LIKE @Search OR Make LIKE @Search", con);
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
                LoadVehicles();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                textBox1.Text = row.Cells["VehicleId"].Value.ToString();
                textBox2.Text = row.Cells["RegistrationNo"].Value.ToString();
                comboBox1.SelectedValue = Convert.ToInt32(row.Cells["CustomerId"].Value);
                comboBox2.Text = row.Cells["Make"].Value.ToString();
                textBox5.Text = row.Cells["Model"].Value.ToString();
                textBox3.Text = row.Cells["Year"].Value.ToString();
                comboBox3.Text = row.Cells["FuelType"].Value.ToString();
            }
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

    }
    }

