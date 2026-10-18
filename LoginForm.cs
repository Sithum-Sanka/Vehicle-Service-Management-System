using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Vehicle_Service_App
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();
            bool hasError = true;

            if (username == "")
            {
                errorProvider1.SetError(textBox1, "cannot be Empty");
                hasError = false;
            }
           
            else if (!Regex.IsMatch(username, @"^[a-zA-Z0-9]{4,}$"))
            {
                errorProvider1.SetError(textBox1, "Username must be at least 4 characters (letters/numbers only)");
                hasError = false;
            }

            if (password == "")
            {
                errorProvider1.SetError(textBox2, "cannot be Empty");
                hasError = false;
            }
            
            else if (!Regex.IsMatch(password, @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{6,}$"))
            {
                errorProvider1.SetError(textBox2, "Password needs 6+ chars, a letter, a number and a special character");
                hasError = false;
            }

            if (hasError)
            {
                if (username == "admin" && password == "Abc@123")
                {
                    DashboardForm dash = new DashboardForm();
                    dash.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Username Or Password Incorrect");
                }
            }




        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
