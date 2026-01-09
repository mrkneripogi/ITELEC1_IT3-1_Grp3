using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
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
    public partial class CustomerModule : Form
    {
        MySqlCommand cm = new MySqlCommand();
        dbConnect dbcon = new dbConnect();
        string title = "Auto Spa Car Wash Management System";
        bool check = false;
        public int vid = 0;
        Customer customer;

        public CustomerModule(Customer cust)
        {
            InitializeComponent();
            customer = cust;
            //cbCarType.SelectedIndex = 0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                checkField();
                if (check)
                {
                    if (MessageBox.Show("Are you sure you want to register this Customer?", "Customer Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new MySqlCommand("INSERT INTO tbcustomer(vid,name,phone,carno,carmodel,address,points)VALUES(@vid,@name,@phone,@carno,@carmodel,@address,@points)", dbcon.connect());
                        cm.Parameters.AddWithValue("@vid", cbCarType.SelectedValue);
                        cm.Parameters.AddWithValue("@name", txtName.Text);
                        cm.Parameters.AddWithValue("@phone", CryptoHelper.Encrypt(txtPhone.Text));
                        cm.Parameters.AddWithValue("@carno", CryptoHelper.Encrypt(txtCarPlateNo.Text));
                        cm.Parameters.AddWithValue("@carmodel", txtCarModel.Text);
                        cm.Parameters.AddWithValue("@address",CryptoHelper.Encrypt(txtAddress.Text));
                        cm.Parameters.AddWithValue("@points", udPoints.Text);


                        dbcon.Open(); // to open connection
                        cm.ExecuteNonQuery();
                        dbcon.Close(); // to close connection
                        MessageBox.Show("Customer has been successfully registered! ", title);
                        check = false;

                        AuditHelper.Log("CREATE", "Customer", "Add new customer: " + txtName.Text);

                        Clear(); // to clear data fields after registration
                                 // refresh the customer list after inserting into the data table
                        customer.loadCustomer();
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
                    if (MessageBox.Show("Are you sure you want to edit this Customer?", "Customer Editing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new MySqlCommand("UPDATE tbcustomer SET vid=@vid, name=@name, phone=@phone, carno=@carno, carmodel=@carmodel, address=@address, points=@points WHERE id=@id", dbcon.connect());
                        cm.Parameters.AddWithValue("@id", IbICid.Text);
                        cm.Parameters.AddWithValue("@vid", cbCarType.SelectedValue);// to save id number of vehicle type
                        cm.Parameters.AddWithValue("@name", txtName.Text);
                        cm.Parameters.AddWithValue("@phone", txtPhone.Text);
                        cm.Parameters.AddWithValue("@carno", txtCarPlateNo.Text);
                        cm.Parameters.AddWithValue("@carmodel", txtCarModel.Text);
                        cm.Parameters.AddWithValue("@address", txtAddress.Text);
                        cm.Parameters.AddWithValue("@points", udPoints.Text);

                        dbcon.Open();// to open connection
                        cm.ExecuteNonQuery();
                        dbcon.Close();// to close connection
                        MessageBox.Show("Customer has been successfully Edited!", title);

                        AuditHelper.Log("UPDATE", "Customer", "Update customer information: " + txtName.Text + " & " + udPoints.Text);

                        this.Dispose();

                        customer.loadCustomer();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void CustomerModule_Load(object sender, EventArgs e)
        {
            // to add vehicle list on the combo box
            cbCarType.DataSource = vehicleType();
            cbCarType.DisplayMember = "name";
            cbCarType.ValueMember = "id";
            if (vid > 0)
                cbCarType.SelectedValue = vid;
        }

        #region method
        // to create a function of vehicle type
        public DataTable vehicleType()
        {
            cm = new MySqlCommand("SELECT * FROM tbvehicletype", dbcon.connect());
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            DataTable dataTable = new DataTable();

            adapter.SelectCommand = cm;
            adapter.Fill(dataTable);

            return dataTable;

        }

        // to create a function for data field
        public void Clear()
        {
            txtAddress.Clear();
            txtCarModel.Clear();
            txtCarPlateNo.Clear();
            txtName.Clear();
            txtPhone.Clear();

            cbCarType.SelectedIndex = 0;
            udPoints.Value = 0;

            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }

        public void checkField()
        {
            if (txtName.Text == "" || txtAddress.Text == "" || txtPhone.Text == "" || txtCarPlateNo.Text == "" || txtCarModel.Text == "")
            {
                MessageBox.Show("Required Data Field!", "Warning!");
                return; // return to the data field
            }
            check = true;
        }


        #endregion method

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control keys (Backspace, Delete, etc.)
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
