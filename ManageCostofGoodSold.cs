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
using System.Xml.Linq;

namespace carWashManagement
{
    public partial class ManageCostofGoodSold : Form
    {
        MySqlCommand cm = new MySqlCommand();
        dbConnect dbcon = new dbConnect();
        string title = "Auto Spa Car Wash Management System";
        Setting setting;
        bool check = false;

        public ManageCostofGoodSold(Setting sett)
        {
            InitializeComponent();
            setting = sett;
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Setting setting = new Setting();

            try
            {
                checkField();
                if (check)
                {
                    if (MessageBox.Show("Are you sure you want to register this cost of good sold?", "Cost of Good Sold Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new MySqlCommand("INSERT INTO tbcostofgood(costname,cost,date)VALUES(@costname,@cost,@date)", dbcon.connect());
                        cm.Parameters.AddWithValue("@costname", txtCostName.Text);
                        cm.Parameters.AddWithValue("@cost", txtCost.Text);
                        cm.Parameters.AddWithValue("@date", dtCoG.Value);

                        dbcon.Open(); // to open connection
                        cm.ExecuteNonQuery();
                        dbcon.Close(); // to close connection
                        MessageBox.Show("Cost of Good has been successfully registered! ", title);
                        Clear(); // to clear data fields after registration

                        AuditHelper.Log("CREATE", "COG", "Added new business expense");

                        setting.loadCostofGood();
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
            Setting setting = new Setting();

            try
            {
                checkField();
                if (check)
                {
                    if (MessageBox.Show("Are you sure you want to edit this cost of good sold?", "Cost of Good Sold Editing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new MySqlCommand("UPDATE tbcostofgood SET costname=@costname,cost=@cost,date=@date WHERE id=@id", dbcon.connect());
                        cm.Parameters.AddWithValue("@id", IbiCid.Text);
                        cm.Parameters.AddWithValue("@costname", txtCostName.Text);
                        cm.Parameters.AddWithValue("@cost", txtCost.Text);
                        cm.Parameters.AddWithValue("@date", dtCoG.Value);

                        dbcon.Open(); // to open connection
                        cm.ExecuteNonQuery();
                        dbcon.Close(); // to close connection
                        MessageBox.Show("Cost of Good has been successfully edited! ", title);
                        Clear(); // to clear data fields after registration
                        this.Dispose();

                        AuditHelper.Log("UPDATE", "COG", "Update a business expense");
                        
                        setting.loadCostofGood();
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

        private void txtCost_KeyPress(object sender, KeyPressEventArgs e)
        {
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

        #region method
        public void checkField()
        {
            if (txtCostName.Text == "" || txtCost.Text == "")
            {
                MessageBox.Show("Required data field!", "Warning!");
                return; // return to the data field
            }
            check = true;
        }

        public void Clear()
        {
            txtCost.Clear();
            txtCostName.Clear();
            dtCoG.Value = DateTime.Now;

            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }

        #endregion method

        private void ManageCostofGoodSold_Load(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtCost_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCostName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
