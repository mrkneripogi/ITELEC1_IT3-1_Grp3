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
    public partial class CashService : Form
    {
        MySqlCommand cm = new MySqlCommand();
        dbConnect dbcon = new dbConnect();
        MySqlDataReader dr;
        string title = "Auto Spa Car Wash Management System";
        Cash cash;

        public CashService(Cash cashForm)
        {
            InitializeComponent();
            cash = cashForm;
            loadService();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            loadService();
        }

        #region method
        public void loadService()
        {
            try
            {
                int i = 0; // to show number for service list
                dgvService.Rows.Clear();
                cm = new MySqlCommand("SELECT * FROM tbservice WHERE name LIKE '%" + txtSearch.Text + "%'", dbcon.connect());
                dbcon.Open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    // to add data to the datagridview from the database
                    dgvService.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString());
                }
                dr.Close();
                dbcon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }

        #endregion method

        private void btnSubmit_Click_1(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in dgvService.Rows)
            {
                bool chkbox = Convert.ToBoolean(dr.Cells["Selects"].Value);
                if (chkbox)
                {
                    try
                    {
                        cm = new MySqlCommand("INSERT IGNORE INTO tbcash(transno, cid, sid, vid, price, date) VALUES(@transno, @cid, @sid, @vid, @price, @date)", dbcon.connect());
                        cm.Parameters.AddWithValue("@transno", cash.lbltransno.Text);
                        cm.Parameters.AddWithValue("@cid", cash.customerId);
                        cm.Parameters.AddWithValue("@sid", dr.Cells[1].Value.ToString());
                        cm.Parameters.AddWithValue("@vid", cash.vehicleTypeId);
                        cm.Parameters.AddWithValue("@price", dr.Cells[3].Value.ToString());
                        cm.Parameters.AddWithValue("@date", DateTime.Now);

                        dbcon.Open();
                        cm.ExecuteNonQuery();
                        dbcon.Close();

                        cash.btnCash.Enabled = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, title);
                    }
                }
            }
            this.Dispose();
            cash.panelCash.Height = 1;
            cash.loadCash();
        }

        private void dgvService_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
