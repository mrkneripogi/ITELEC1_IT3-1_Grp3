using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; // Microsoft
using MySql.Data.MySqlClient;

namespace carWashManagement
{
    public partial class EmployerModule : Form
    {
        MySqlCommand cm = new MySqlCommand();
        dbConnect dbcon = new dbConnect();
        string title = "Car Wash Management System";
        bool check = false;
        Employer employer;


        public EmployerModule(Employer emp)
        {
            InitializeComponent();
            cbRole.SelectedIndex = 2;
            employer = emp;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void EmployerModule_Load(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
            btnUpdate.Enabled = false;
            btnSave.Enabled = true;
        }

        private void txtSalary_TextChanged(object sender, EventArgs e)
        {
            // not an keypressevent args
        }

        private void txtSalary_KeyPress(object sender, KeyPressEventArgs e)
        {
            // only allow digit number
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            // only allow decimal kasi pogi ako 
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > 1))
            {
                e.Handled = true;
            }
        }

        // no password for worker or supervisor
        private void cbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbRole.Text == "Maintenance" || cbRole.Text == "Worker")
            {
                this.Height = 453 - 15; 
                txtPassword.Text = "worker"; //this is clear() before but changed due to required data field
                IbIPass.Visible = false; // to hide them hahaha
                txtPassword.Visible = false;
            }
            else
            {
                IbIPass.Visible = true; // to unhide them hahaha
                txtPassword.Visible = true;
                this.Height = 453;
            }
        }

        // region method for clearing 
        public void Clear()
        {
            txtAddress.Clear();
            txtName.Clear();
            txtPassword.Clear();
            txtPhone.Clear();
            txtSalary.Clear();

            dtDob.Value = DateTime.Now;
            cbRole.SelectedIndex = 2; // default is worker
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        // To Insert an employer to the data 
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                checkField();
                if (check)
                {
                    if (MessageBox.Show("Are you sure you want to register this employee/employer?", "Personell Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new MySqlCommand("INSERT INTO tbemployer(name,phone,address,dob,gender,role,salary,password)VALUES(@name,@phone,@address,@dob,@gender,@role,@salary,@password)", dbcon.connect());
                        cm.Parameters.AddWithValue("@name", txtName.Text);
                        cm.Parameters.AddWithValue("@phone", txtPhone.Text);
                        cm.Parameters.AddWithValue("@address", txtAddress.Text);
                        cm.Parameters.AddWithValue("@dob", dtDob.Text);
                        cm.Parameters.AddWithValue("@gender", rdMale.Checked ? "Male" : "Female");
                        cm.Parameters.AddWithValue("@role", cbRole.Text);
                        cm.Parameters.AddWithValue("@salary", txtSalary.Text);
                        cm.Parameters.AddWithValue("@password", PasswordHelper.Hash(txtPassword.Text));

                        dbcon.Open(); // to open connection
                        cm.ExecuteNonQuery();
                        dbcon.Close(); // to close connection
                        MessageBox.Show("Employer has been successfully registered! ", title);
                        check = false;

                        AuditHelper.Log("CREATE", "Employer", "Add new user: " + txtName.Text);

                        Clear(); // to clear data fields after registration
                        employer.loadEmployer(); // refresh the employer list after inserting into the data table 
                    }
                }   
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                checkField();
                if (check)
                {
                    if (MessageBox.Show("Are you sure you want to edit this record", "Employer Editing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new MySqlCommand("UPDATE tbemployer SET name=@name,phone=@phone,address=@address,dob=@dob,gender=@gender,role=@role,salary=@salary,password=@password WHERE id=@id", dbcon.connect());
                        cm.Parameters.AddWithValue("@id", IbIEid.Text);
                        cm.Parameters.AddWithValue("@name", txtName.Text);
                        cm.Parameters.AddWithValue("@phone", txtPhone.Text);
                        cm.Parameters.AddWithValue("@address", txtAddress.Text);
                        cm.Parameters.AddWithValue("@dob", dtDob.Text);
                        cm.Parameters.AddWithValue("@gender", rdMale.Checked ? "Male" : "Female");
                        cm.Parameters.AddWithValue("@role", cbRole.Text);
                        cm.Parameters.AddWithValue("@salary", txtSalary.Text);
                        cm.Parameters.AddWithValue("@password", PasswordHelper.Hash(txtPassword.Text));

                        dbcon.Open(); // to open connection
                        cm.ExecuteNonQuery();
                        dbcon.Close(); // to close connection
                        MessageBox.Show("Personell modification has been saved to the database. ", title);
                        Clear(); // to clear data fields after registration

                        AuditHelper.Log("UPDATE", "Employer", "Update user information: " + txtName.Text);
                        
                        this.Dispose();

                        employer.loadEmployer();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }

        // check for an empty field with age 
        public void checkField()
        {
            if (txtName.Text == "" || txtAddress.Text == "" || txtPassword.Text == "" || txtPhone.Text == "" || txtSalary.Text == "")
            {
                MessageBox.Show("Required Data Field!", "Warning!");
                return; // return to the data field
            }
            if (checkAge(dtDob.Value) < 18)
            {
                MessageBox.Show("Employer is under 18!", "Warning!");
                return;
            }
            check = true;
        }

        // restriction method for age under 18
        private static int checkAge(DateTime dateofBirth)
        {
            int age = DateTime.Now.Year - dateofBirth.Year;
            if (DateTime.Now.DayOfYear < dateofBirth.DayOfYear)
                age = age - 1;
            return age;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            // All control keys
            if (char.IsControl(e.KeyChar))
                return;

            // Allow digits only
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }

            // Limit to 11 digits
            TextBox tb = sender as TextBox;
            if (tb.Text.Length >= 11)
            {
                e.Handled = true;
            }
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;

            // Allow control keys (Backspace, Delete, etc.)
            if (char.IsControl(e.KeyChar))
                return;

            // Allow letters and space only
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != ' ')
            {
                e.Handled = true;
                return;
            }

            // Limit to 15 characters
            if (tb.Text.Length >= 30)
            {
                e.Handled = true;
            }
        }
    }
}
