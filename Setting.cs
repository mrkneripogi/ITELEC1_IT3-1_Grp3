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
    public partial class Setting : Form
    {
        MySqlCommand cm = new MySqlCommand();
        dbConnect dbcon = new dbConnect();
        MySqlDataReader dr;

        string title = "Auto Spa Car Wash Management System";

        bool hasdetail = false;
        public Setting()
        {
            InitializeComponent();
            loadVehicleType();
            loadCostofGood();
            loadCompany();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        #region vehicletype

        private void txtSearchVT_TextChanged(object sender, EventArgs e)
        {
            loadVehicleType();
        }

        private void btnAddVT_Click(object sender, EventArgs e)
        {
            ManageVehicleType module = new ManageVehicleType(this);
            module.btnUpdate.Enabled = false;
            module.ShowDialog();
        }

        private void dgvVehicleType_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvVehicleType.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                // to sent ehicle data to the vehicle module
                // cbrole wasnt in the index
                ManageVehicleType module = new ManageVehicleType(this);
                module.Vid.Text = dgvVehicleType.Rows[e.RowIndex].Cells[1].Value.ToString();
                module.txtName.Text = dgvVehicleType.Rows[e.RowIndex].Cells[2].Value.ToString();
                module.cbClass.Text = dgvVehicleType.Rows[e.RowIndex].Cells[3].Value.ToString();
               
                module.btnSave.Enabled = false;
                module.ShowDialog();
            }
            else if (colName == "Delete")
            {
                try
                {
                    if (MessageBox.Show("Are you sure you want to delete this record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new MySqlCommand("DELETE FROM tbvehicletype WHERE id LIKE'" + dgvVehicleType.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", dbcon.connect());
                        dbcon.Open();
                        cm.ExecuteNonQuery();
                        dbcon.Close();
                        MessageBox.Show("Vehicle type data has been successfully removed!", title, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        AuditHelper.Log("DELETE", "V.Type", "Delete a vehicle type");

                        loadVehicleType(); //to reload after editing updateing
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, title);
                }
            }
            
        }

        public void loadVehicleType()
        {
            try
            {
                int i = 0; // to show number for vehicle list 
                dgvVehicleType.Rows.Clear();
                cm = new MySqlCommand("SELECT * FROM tbvehicletype WHERE CONCAT (name,class) LIKE '%" + txtSearchVT.Text + "%'", dbcon.connect());
                dbcon.Open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    // to add data to the datagridview from the database
                    dgvVehicleType.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString());
                }
                dbcon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }

        #endregion vehicletype

        #region CostofGoodSold
        private void btnAddCoG_Click(object sender, EventArgs e)
        {
            ManageCostofGoodSold module = new ManageCostofGoodSold(this);
            module.btnUpdate.Enabled = false;
            module.ShowDialog();
        }
        private void txtSearchCoG_TextChanged(object sender, EventArgs e)
        {
            loadCostofGood();
        }

        private void dgvCostofGood_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvCostofGood.Columns[e.ColumnIndex].Name;
            if (colName == "EditCoG")
            {
                // to sent cost of good sold to the management of cost of good sold module
                // cbrole wasnt in the index
                ManageCostofGoodSold module = new ManageCostofGoodSold(this);
                module.IbiCid.Text = dgvCostofGood.Rows[e.RowIndex].Cells[1].Value.ToString();
                module.txtCostName.Text = dgvCostofGood.Rows[e.RowIndex].Cells[2].Value.ToString();
                module.txtCost.Text = dgvCostofGood.Rows[e.RowIndex].Cells[3].Value.ToString();
                module.dtCoG.Text = dgvCostofGood.Rows[e.RowIndex].Cells[4].Value.ToString();

                module.btnSave.Enabled = false;
                module.ShowDialog();
            }
            else if (colName == "DeleteCoG")
            {
                try
                {
                    if (MessageBox.Show("Are you sure you want to delete this record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new MySqlCommand("DELETE FROM tbcostofgood WHERE id LIKE'" + dgvCostofGood.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", dbcon.connect());
                        dbcon.Open();
                        cm.ExecuteNonQuery();
                        dbcon.Close();
                        MessageBox.Show("Cost of good sold data has been successfully removed!", title, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        AuditHelper.Log("DELETE", "COG", "Delete a COG");

                        loadCostofGood(); //to reload after editing updating
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, title);
                }
            }
            
        }
        #endregion CostofGoodSold

        public void loadCostofGood()
        {
            try
            {
                int i = 0; // to show number for cost of good sold list
                dgvCostofGood.Rows.Clear();
                cm = new MySqlCommand("SELECT * FROM tbcostofgood WHERE CONCAT (costname,date) LIKE '%" + txtSearchCoG.Text + "%'", dbcon.connect());
                dbcon.Open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    // to add data to the datagridview from the database
                    dgvCostofGood.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(),DateTime.Parse(dr[3].ToString()).ToShortDateString());
                }
                dbcon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }

        private void txtSearchCoG_TextChanged_1(object sender, EventArgs e)
        {

        }

        #region Company Detail
        // 1st: Load the data from the database 
        public void loadCompany()
        {
            try
            {
                dbcon.Open();
                cm = new MySqlCommand("SELECT * FROM tbcompany", dbcon.connect());
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    hasdetail = true;
                    txtComName.Text = dr["name"].ToString();
                    txtComAddress.Text = dr["address"].ToString();

                }
                else
                {
                    txtComName.Clear();
                    txtComAddress.Clear();
                }
                dr.Close();
                dbcon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(MessageBox.Show("Save company detail?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {// now we create a function for execute query only one line
                    if (hasdetail)
                    {
                        dbcon.executeQuery("UPDATE tbcompany SET name='" + txtComName.Text + "', address='" + txtComAddress.Text + "'");
                    }
                    else
                    {
                        dbcon.executeQuery("INSERT INTO tbcompany (name, address) VALUES ('" + txtComName.Text + "','" + txtComAddress.Text + "')");
                    }
                    MessageBox.Show("Company detail has been successfully saved!", "Save Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtComName.Clear();
            txtComAddress.Clear();
        }
        #endregion Company Detail
    }
}
