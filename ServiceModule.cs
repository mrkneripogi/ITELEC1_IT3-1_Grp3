using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace carWashManagement
{
    public partial class ServiceModule : Form
    {
        MySqlCommand cm = new MySqlCommand();
        dbConnect dbcon = new dbConnect();
        string title = "Auto Spa Car Wash Management System";
        Service service;
        public ServiceModule(Service ser)
        {
            InitializeComponent();
            service = ser;
        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (txtName.Text == "" || txtPrice.Text == "")
                {
                    MessageBox.Show("Required data field!", "Warning");
                    return; // return to the data field and form
                }
                if (MessageBox.Show("Are you sure you want to register this service?", "Service Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new MySqlCommand("INSERT INTO tbservice(name,price)VALUES(@name,@price)", dbcon.connect());
                    cm.Parameters.AddWithValue("@name", txtName.Text);
                    cm.Parameters.AddWithValue("@price", txtPrice.Text);

                    dbcon.Open();// to open connection
                    cm.ExecuteNonQuery();
                    dbcon.Close();// to close connection
                    MessageBox.Show("Service has been successfully registered!", title);
                    Clear();//to clear data field, after data inserted into the database

                    AuditHelper.Log("CREATE", "Service", "Create a new service: " + txtName.Text + ": " + txtPrice.Text);

                    service.loadService();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (txtName.Text == "" || txtPrice.Text == "")
                {
                    MessageBox.Show("Required data field!", "Warning");
                    return; // return to the data field and form
                }
                if (MessageBox.Show("Are you sure you want to edit this service?", "Service Editing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new MySqlCommand("UPDATE tbservice SET name=@name, price=@price WHERE id=@id", dbcon.connect());
                    cm.Parameters.AddWithValue("@id", lblSid.Text);
                    cm.Parameters.AddWithValue("@name", txtName.Text);
                    cm.Parameters.AddWithValue("@price", txtPrice.Text);

                    dbcon.Open();// to open connection
                    cm.ExecuteNonQuery();
                    dbcon.Close();// to close connection
                    MessageBox.Show("Service has been successfully edited!", title);
                    this.Dispose();

                    AuditHelper.Log("UPDATE", "Service", "Update a service: " + txtName.Text + ": " + txtPrice.Text);

                    service.loadService();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            Clear();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            // only allow digit number
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            // only allow one decimal 
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        #region method
        public void Clear()
        {
            txtName.Clear();
            txtPrice.Clear();

            btnSave.Enabled = true;
            btnUpdate.Enabled = false;

        }
        #endregion method
    }
}
