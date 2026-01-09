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
    public partial class ManageVehicleType : Form
    {
        MySqlCommand cm = new MySqlCommand();
        dbConnect dbcon = new dbConnect();
        string title = "Auto Spa Car Wash Management System";
        Setting setting;

        public ManageVehicleType(Setting sett)
        {
            InitializeComponent();
            setting = sett;
            cbClass.SelectedIndex = 0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtName.Text == "")
                {
                    MessageBox.Show("Required vehicle name!", "Warning!");
                    return; // return to the data field
                }
                
                if (MessageBox.Show("Are you sure you want to register this vehicle?", "Vehicle Type Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new MySqlCommand("INSERT INTO tbvehicletype(name,class)VALUES(@name,@class)", dbcon.connect());
                    cm.Parameters.AddWithValue("@name", txtName.Text);
                    cm.Parameters.AddWithValue("@class", cbClass.Text);

                    dbcon.Open(); // to open connection
                    cm.ExecuteNonQuery();
                    dbcon.Close(); // to close connection
                    MessageBox.Show("Vehicle Type has been successfully registered! ", title);
                    Clear(); // to clear data fields after registration

                    AuditHelper.Log("CREATE", "V.Type", "Added new vehicle type: " + txtName.Text);
                    setting.loadVehicleType();
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
                if (txtName.Text == "")
                {
                    MessageBox.Show("Required vehicle name!", "Warning!");
                    return; // return to the data field
                }

                if (MessageBox.Show("Are you sure you want to update this vehicle?", "Vehicle Type Editing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new MySqlCommand("UPDATE tbvehicletype SET name=@name,class=@class WHERE id=@id", dbcon.connect());
                    cm.Parameters.AddWithValue("@id", Vid.Text);
                    cm.Parameters.AddWithValue("@name", txtName.Text);
                    cm.Parameters.AddWithValue("@class", cbClass.Text);

                    dbcon.Open(); // to open connection
                    cm.ExecuteNonQuery();
                    dbcon.Close(); // to close connection
                    MessageBox.Show("Vehicle Type has been successfully edited! ", title);
                    Clear(); // to clear data fields after registration
                    
                    this.Dispose();
                    AuditHelper.Log("UPDATE", "V.Type", "Updated a vehicle type: " + txtName.Text);
                    setting.loadVehicleType();


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
            this.Dispose();
        }

        private void ManageVehicleType_Load(object sender, EventArgs e)
        {

        }

        #region method
        public void Clear()
        {
            txtName.Clear();
            cbClass.SelectedIndex = 0;

            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }
        #endregion method
    }
}
