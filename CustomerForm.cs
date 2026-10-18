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
using System.Text.RegularExpressions;

namespace Vehicle_Service_App
{
    public partial class CustomerForm : Form
    {
        public CustomerForm()
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


        private void CustomerForm_Load(object sender, EventArgs e)
        {
            
            string connString = Properties.Settings.Default.ConnString;
            SqlConnection con = new SqlConnection(connString);

            try
            {
                

                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Customer",con);
                cmd.CommandType = CommandType.Text;
                SqlDataReader rd = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(rd);
                dataGridView1.DataSource = dt;


            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
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
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
        }

        private void LoadCustomers()
        {
            string connString = Properties.Settings.Default.ConnString;
            SqlConnection con = new SqlConnection(connString);

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Customer", con);
                cmd.CommandType = CommandType.Text;

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
        private void button1_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                errorProvider1.SetError(textBox1, "Customer ID is required");
                return;
            }
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                errorProvider1.SetError(textBox2, "Full Name is required");
                return;
            }

            if (!string.IsNullOrEmpty(textBox4.Text))
            {
                string emailPattern = @"^[^@\s]+@[^@\s]+\.[a-zA-Z]{2,}$";
                if (!Regex.IsMatch(textBox4.Text, emailPattern))
                {
                    errorProvider1.SetError(textBox4, "Invalid email format (example: name@example.com)");
                    return;
                }
            }

            string connString = Properties.Settings.Default.ConnString;
            SqlConnection con = new SqlConnection(connString);
            try
            {
                int customerId = Convert.ToInt32(textBox1.Text);
                string FullName = textBox2.Text;
                string phone = textBox3.Text;
                string email = textBox4.Text;
                string Address = textBox5.Text;

                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Customer(CustomerId, FullName, Phone, Email, Address) " +
                    "VALUES(@CustomerId, @FullName, @Phone, @Email, @Address)", con);

                cmd.Parameters.AddWithValue("@CustomerId", customerId);
                cmd.Parameters.AddWithValue("@FullName", FullName);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Address", Address);

                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Add Successfull");
                LoadCustomers();
                button4_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Program interrupt" + ex.Message);
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
                MessageBox.Show("Select a Customer From Grid");
                return;
            }

            string connString = Properties.Settings.Default.ConnString;
            SqlConnection con = new SqlConnection(connString);

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Customer SET FullName=@FullName, Phone=@Phone, " +
                    "Email=@Email, Address=@Address WHERE CustomerId=@CustomerId", con);

                cmd.Parameters.AddWithValue("@FullName", textBox2.Text);
                cmd.Parameters.AddWithValue("@Phone", textBox3.Text);
                cmd.Parameters.AddWithValue("@Email", textBox4.Text);
                cmd.Parameters.AddWithValue("@Address", textBox5.Text);
                cmd.Parameters.AddWithValue("@CustomerId", textBox1.Text);

                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                    MessageBox.Show("Customer Updated Successfully");
                else
                    MessageBox.Show("That Customer ID doesn't match");

                LoadCustomers();
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

       
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                textBox1.Text = row.Cells["CustomerId"].Value.ToString();
                textBox2.Text = row.Cells["FullName"].Value.ToString();
                textBox3.Text = row.Cells["Phone"].Value.ToString();
                textBox4.Text = row.Cells["Email"].Value.ToString();
                textBox5.Text = row.Cells["Address"].Value.ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Select a Customer From Grid");
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Are you sure you want to delete this customer?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
                return; 

            string connString = Properties.Settings.Default.ConnString;
            SqlConnection con = new SqlConnection(connString);

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Customer WHERE CustomerId=@CustomerId", con);
                cmd.Parameters.AddWithValue("@CustomerId", textBox1.Text);

                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                    MessageBox.Show("Customer Deleted Successfully");
                else
                    MessageBox.Show("Customer not found");

                LoadCustomers();          
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
                    "SELECT * FROM Customer WHERE FullName LIKE @Search OR Phone LIKE @Search", con);
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
                LoadCustomers();
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
    }
    }

