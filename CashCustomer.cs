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
    public partial class CashCustomer : Form
    {
        MySqlCommand cm = new MySqlCommand();
        dbConnect dbcon = new dbConnect();
        MySqlDataReader dr;
        string title = "Auto Spa Car Wash Managment";
        Cash cash;

        public CashCustomer(Cash cashform)
        {
            InitializeComponent();
            cash = cashform;
            loadCustomer();

            Main mainForm = new Main();

            if (AccessControl.CanViewCustomerSensitive(mainForm.isManager))
                UnmaskCustomer();
            else
                ApplyCustomerMasking();
        }

        private void textSearch_TextChanged(object sender, EventArgs e)
        {
            loadCustomer();
        }

        private void dgvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvCustomer.Columns[e.ColumnIndex].Name;
            if (colName == "Selecta")
            {
                cash.customerId = int.Parse(dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString());
                cash.vehicleTypeId = int.Parse(dgvCustomer.Rows[e.RowIndex].Cells[2].Value.ToString());
            }
            else return;
            this.Dispose();
            cash.panelCash.Height = 1;
        }

        #region method
        // create function for load customer
        public void loadCustomer()
        {
            try
            {
                int i = 0; // to show number for customer list
                dgvCustomer.Rows.Clear();
                cm = new MySqlCommand("SELECT * FROM tbcustomer WHERE CONCAT (name,phone,address) LIKE '%" + textSearch.Text + "%'", dbcon.connect());
                dbcon.Open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;

                    //string phone = dr[4].ToString();
                    //string carPlate = dr[5].ToString();
                    //string address = dr[7].ToString();

                    // //🔐 decrypt ONLY if allowed
                    //if (AccessControl.IsAdminOrSupervisor())
                    //    {
                    //        phone = CryptoHelper.Decrypt(phone);
                    //        carPlate = CryptoHelper.Decrypt(carPlate);
                    //        address = CryptoHelper.Decrypt(address);
                    //    }
                    // to add data to the datagridview from the database
                    dgvCustomer.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString());

                    DataGridViewRow row = dgvCustomer.Rows[dgvCustomer.Rows.Count - 1];

                    row.Cells[4].Tag = dr[4].ToString(); // Phone
                    row.Cells[5].Tag = dr[5].ToString(); // Car Plate No
                    row.Cells[7].Tag = dr[7].ToString(); // Address

                }
                dbcon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }
        #endregion method

        private void ApplyCustomerMasking()
        {
            Main mainform = new Main();

            bool canView = AccessControl.CanViewCustomerSensitive(mainform.isManager);

            if (canView) return;

            foreach (DataGridViewRow row in dgvCustomer.Rows)
            {
                if (row.IsNewRow) continue;

                row.Cells[4].Value = MaskingHelper.Mask(row.Cells[4].Tag?.ToString());
                row.Cells[5].Value = MaskingHelper.Mask(row.Cells[5].Tag?.ToString());
                row.Cells[7].Value = MaskingHelper.Mask(row.Cells[7].Tag?.ToString());
            }
        }

        public void UnmaskCustomer()
        {
            foreach (DataGridViewRow row in dgvCustomer.Rows)
            {
                if (row.IsNewRow) continue;

                row.Cells[4].Value = row.Cells[4].Tag;
                row.Cells[5].Value = row.Cells[5].Tag;
                row.Cells[7].Value = row.Cells[7].Tag;
            }
        }
        private void btnCash_Click(object sender, EventArgs e)
        {
        }

       
    }
}
