using Microsoft.Reporting.Map.WebForms.BingMaps;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace carWashManagement
{
    public partial class Customer : Form
    {
        MySqlCommand cm = new MySqlCommand();
        dbConnect dbcon = new dbConnect();
        MySqlDataReader dr;
        //private bool isManager;

        string title = "Auto Spa Car Wash Management System";
        public Customer()
        {
            InitializeComponent();
            loadCustomer();
            StoreOriginalCustomerValues();
            ApplyCustomerMasking();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CustomerModule module = new CustomerModule(this);
            module.btnUpdate.Enabled = false;
            module.ShowDialog();
        }

        private void dgvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvCustomer.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                if (!AccessControl.CanAlterCustomer())
                {
                    MessageBox.Show(
                        "You are not authorized to edit customer information.",
                        "Access Denied",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }
                // to sent customer data to the customer module
                CustomerModule module = new CustomerModule(this);
                module.IbICid.Text = dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString();
                module.txtName.Text = dgvCustomer.Rows[e.RowIndex].Cells[2].Value.ToString();
                module.txtPhone.Text = dgvCustomer.Rows[e.RowIndex].Cells[3].Value.ToString();
                module.txtCarPlateNo.Text = dgvCustomer.Rows[e.RowIndex].Cells[4].Value.ToString();
                module.txtCarModel.Text = dgvCustomer.Rows[e.RowIndex].Cells[5].Value.ToString();
                module.vid = vehicleIdbyName(dgvCustomer.Rows[e.RowIndex].Cells[6].Value.ToString());
                module.txtAddress.Text = dgvCustomer.Rows[e.RowIndex].Cells[7].Value.ToString();
                module.udPoints.Text = dgvCustomer.Rows[e.RowIndex].Cells[8].Value.ToString();


                module.btnSave.Enabled = false;
                module.udPoints.Enabled = true;
                module.ShowDialog();
            }
            else if (colName == "Delete")
            {
                if (!AccessControl.CanAlterCustomer())
                {
                    MessageBox.Show(
                        "You are not authorized to delete customer information.",
                        "Access Denied",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }
                try
                {
                    if (MessageBox.Show("Are you sure you want to delete this record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new MySqlCommand("DELETE FROM tbcustomer WHERE id LIKE'" + dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", dbcon.connect());
                        dbcon.Open();
                        cm.ExecuteNonQuery();
                        dbcon.Close();
                        MessageBox.Show("Customer data has been successfully removed!", title, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        AuditHelper.Log("DELETE", "Customer", "Delete customer record");

                        loadCustomer();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, title);
                }
            }   
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            loadCustomer();
        }

        #region method
        public void loadCustomer()
        {
            Main main = new Main();

            try
            {
                int i = 0; // to show number for customer list 
                dgvCustomer.Rows.Clear();
                cm = new MySqlCommand("SELECT C.id, C.name, phone, carno, carmodel, V.name, address, points FROM tbcustomer AS C INNER JOIN tbvehicletype AS V ON C.vid=V.id WHERE CONCAT (C.name,carno,carmodel,address) LIKE '%" + txtSearch.Text + "%'", dbcon.connect());
                dbcon.Open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;

                    string phone = dr[2].ToString();
                    string carno = dr[3].ToString();
                    string address = dr[6].ToString();

                    //string phone = CryptoHelper.Decrypt(dr[4].ToString());
                    //string carno = CryptoHelper.Decrypt(dr[5].ToString());
                    //string address = CryptoHelper.Decrypt(dr[7].ToString());

                    if (AccessControl.IsAdminOrSupervisor())
                    {
                        phone = CryptoHelper.Decrypt(phone);
                        carno = CryptoHelper.Decrypt(carno);
                        address = CryptoHelper.Decrypt(address);
                    }

                    // to add data to the datagridview from the database
                    dgvCustomer.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), phone, carno, dr[4].ToString(), dr[5].ToString(), address, dr[7].ToString());

                    //DataGridViewRow row = dgvCustomer.Rows[i];
                    //row.Cells[4].Tag = phone;
                    //row.Cells[5].Tag = carno;
                    //row.Cells[7].Tag = address;
                }
                dbcon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }
        public int vehicleIdbyName(string str)
        {
            int i = 0;
            cm = new MySqlCommand("SELECT id FROM tbvehicletype WHERE name LIKE '" + str + "' ", dbcon.connect());
            dbcon.Open();
            dr = cm.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                i = int.Parse(dr["id"].ToString());
            }
            dbcon.Close();
            return i;
        }
        #endregion method

        private void StoreOriginalCustomerValues()
        {
            foreach (DataGridViewRow row in dgvCustomer.Rows)
            {
                if (row.IsNewRow) continue;

                // Store originals
                row.Cells[3].Tag = row.Cells[3].Value; // Phone
                row.Cells[4].Tag = row.Cells[4].Value; // Plate
                row.Cells[7].Tag = row.Cells[7].Value; // Address
            }
        }

        public void ApplyCustomerMasking()
        {
            Main mainform = new Main();

            bool canView = AccessControl.CanViewCustomerSensitive(mainform.isManager);

            if (canView) return;

            foreach (DataGridViewRow row in dgvCustomer.Rows)
            {
                if (row.IsNewRow) continue;

                row.Cells[3].Value = MaskingHelper.Mask(row.Cells[3].Tag?.ToString());
                row.Cells[4].Value = MaskingHelper.Mask(row.Cells[4].Tag?.ToString());
                row.Cells[7].Value = MaskingHelper.Mask(row.Cells[7].Tag?.ToString());
            }
        }

        public void ApplyCustomerDecryption(bool isManager)
        {
            if (!isManager) return;

            foreach (DataGridViewRow row in dgvCustomer.Rows)
            {
                if (row.IsNewRow) continue;

                row.Cells[3].Value =
                    CryptoHelper.Decrypt(row.Cells[3].Value?.ToString()); // phone

                row.Cells[4].Value =
                    CryptoHelper.Decrypt(row.Cells[4].Value?.ToString()); // plate

                row.Cells[7].Value =
                    CryptoHelper.Decrypt(row.Cells[7].Value?.ToString()); // address
            }
        }

        public void UnmaskCustomer()
        {
            foreach (DataGridViewRow row in dgvCustomer.Rows)
            {
                if (row.IsNewRow) continue;

                row.Cells[3].Value = row.Cells[3].Tag;
                row.Cells[4].Value = row.Cells[4].Tag;
                row.Cells[7].Value = row.Cells[7].Tag;
            }
        }
    }
}
