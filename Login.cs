using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace carWashManagement
{
    public partial class Login : Form
    {
        MySqlCommand cm = new MySqlCommand();
        dbConnect dbcon = new dbConnect();
        MySqlDataReader dr;
        string title = "Auto Spa Car Wash Management System";

        public Login()
        {
            InitializeComponent();
            loadCompany();
        }


        public void loadCompany()
        {
            cm = new MySqlCommand("SELECT * FROM tbcompany", dbcon.connect());
            dbcon.Open();
            dr = cm.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                lblCompany.Text = dr["name"].ToString();
                lblAddress.Text = dr["address"].ToString();
            }
            dr.Close();
            dbcon.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtName.Clear();
            txtPassword.Clear();
            txtName.Focus();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtName.Text.Trim();
            string password = txtPassword.Text.Trim();
            

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter username and password.");
                return;
            }

            string hashedInput = PasswordHelper.Hash(password);

            try
            {
                using (MySqlConnection con = new dbConnect().connect())
                {
                    string query = @"
                SELECT name, password, role
                FROM tbemployer
                WHERE name = @name
                LIMIT 1";

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@name", username);

                    con.Open();
                    MySqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        string dbPassword = dr["password"].ToString();
                        string dbRole = dr["role"].ToString();

                        if (hashedInput == dbPassword) // (hash later if you want)
                        {
                            // ✅ SESSION SET HERE
                            Session.UserName = username;
                            Session.Role = dbRole.ToLower();

                            MessageBox.Show($"Welcome {username} ({dbRole})",
                                "Access Granted", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            this.Hide();
                            new Main().Show();

                            AuditHelper.Log( "LOGIN", "Login", "User logged in successfully");
                        }
                        else
                        {
                            MessageBox.Show("Invalid username or password.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Login Error");
            }
            //string username = txtName.Text.Trim();
            //string password = txtPassword.Text.Trim();

            //// Load Employer.cs in memory
            //Employer employerForm = new Employer();
            //employerForm.loadEmployer();  

            //bool found = false;

            //foreach (DataGridViewRow row in employerForm.dgvEmployer.Rows)
            //{
            //    if (row.IsNewRow) continue;

            //    string dbUser = row.Cells[2].Value.ToString();  // Username column
            //    string dbPass = row.Cells[8].Value.ToString();  // Password column
            //    string dbRole = row.Cells[8].Value.ToString();  // Role column

            //    if (username == dbUser && password == dbPass)
            //    {
            //        found = true;

            //        // Save session
            //        Session.UserName = dbUser;
            //        Session.Role = dbRole.ToLower(); // admin/supervisor/worker/maintenance

            //        MessageBox.Show("Welcome " + dbUser, "Access Granted",
            //            MessageBoxButtons.OK, MessageBoxIcon.Information);

            //        this.Hide();
            //        Main m = new Main();
            //        m.ShowDialog();
            //        break;
            //    }
            //}

            //if (!found)
            //{
            //    MessageBox.Show("Invalid username or password!", "Access Denied",
            //        MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}

            //Second comment

            //try
            //{
            //    cm = new MySqlCommand("SELECT name FROM tbemployer WHERE name ='" + txtName.Text + "' AND password ='" + txtPassword.Text + "'", dbcon.connect());
            //    dbcon.Open();
            //    dr = cm.ExecuteReader();
            //    dr.Read();
            //    if (dr.HasRows)
            //    {
            //        Session.UserName = dr["Uname"].ToString();
            //        Session.Role = txtPassword.Text.Trim().ToLower();  // using password as role

            //        MessageBox.Show("WELCOME " + Session.UserName, "ACCESS GRANTED");
            //        //MessageBox.Show("WELCOME " + dr[0].ToString() + " | ", "ACCESS GRANTED", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        this.Hide();
            //        Main main = new Main();
            //        main.ShowDialog();
            //    }
            //    else
            //    {
            //        MessageBox.Show("Invalid username or password", "ACCESS DENIED", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    }
            //    dbcon.Close();
            //    dr.Close();
            //}
            //catch (Exception ex)
            //{

            //    MessageBox.Show(ex.Message, title);
            //}
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                txtPassword.UseSystemPasswordChar = false;
            else
                txtPassword.UseSystemPasswordChar = true;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
