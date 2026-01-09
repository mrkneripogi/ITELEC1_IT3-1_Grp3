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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace carWashManagement
{
    public partial class Cash : Form
    {
        MySqlCommand cm = new MySqlCommand();
        dbConnect dbcon = new dbConnect();
        MySqlDataReader dr;
        string title = "Auto Spa Car Wash Management System";
        public int customerId = 0, vehicleTypeId = 0;
        public string carno, carmodel;
        Main main;


        public Cash(Main mainForm)
        {
            InitializeComponent();
            getTransno();
            loadCash();
            main = mainForm;
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            openChildForm(new CashCustomer(this));
            btnAddService.Enabled = true;
        }

        private void btnAddService_Click(object sender, EventArgs e)
        {
            openChildForm(new CashService(this));
            btnAddCustomer.Enabled = false;
        }

        private void btnCash_Click(object sender, EventArgs e)
        {
            SettlePayment module = new SettlePayment(this);
            module.txtSale.Text = lblTotal.Text;
            module.ShowDialog();
            main.loadGrossProfit();
        }

        private void btnCard_Click(object sender, EventArgs e)
        {
            SettlePaymentCard module = new SettlePaymentCard(this);
            module.lblAmount.Text = lblTotal.Text;
            module.ShowDialog();
            main.loadGrossProfit();
        }

        private void paneCash_Paint(object sender, PaintEventArgs e)
        {

        }



        #region method
        private Form activeForm = null;
        public void openChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelCash.Height = 200;
            panelCash.Controls.Add(childForm);
            panelCash.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        // Create a function for transatoin generator depend on date

        public void getTransno()
        {
            try
            {
                string sdate = DateTime.Now.ToString("yyyyMMdd");
                int count;
                string transno;


                dbcon.Open();
                cm = new MySqlCommand("SELECT transno FROM tbcash WHERE transno LIKE '" + sdate + "%' ORDER BY id DESC LIMIT 1", dbcon.connect());
                dr = cm.ExecuteReader();
                dr.Read();

                if (dr.HasRows)
                {
                    transno = dr[0].ToString();
                    count = int.Parse(transno.Substring(8, 4));
                    lbltransno.Text = sdate + (count + 1);
                }
                else
                {
                    transno = sdate + "1001";
                    lbltransno.Text = transno;
                }

                dbcon.Close();
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }

        private void dgvCash_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dgvCash_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvCash.Columns[e.ColumnIndex].Name;
            if (colName == "Delete") // if you want to delete the record to click the delete icon on the datagridview
            {
                try
                {
                    if (MessageBox.Show("Are you sure you want to cancel this service?", "Cancel Services", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new MySqlCommand("DELETE FROM tbcash WHERE id LIKE'" + dgvCash.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", dbcon.connect());
                        dbcon.Open();
                        cm.ExecuteNonQuery();
                        dbcon.Close();
                        MessageBox.Show("Service has been successfully Canceled!", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, title);
                }
            }
            loadCash();
        }

        private void lblTotal_Click(object sender, EventArgs e)
        {

        }

        

        private void Cash_Load(object sender, EventArgs e)
        {
        }

        public void loadCash()
        {
            int i = 0;
            double total = 0;
            double price = 0;
            dgvCash.Rows.Clear();
            cm = new MySqlCommand("SELECT ca.id,ca.transno,cu.name,cu.carno,cu.carmodel,v.name,v.class,s.name,ca.price,ca.date FROM tbcash AS Ca " +
                "LEFT JOIN tbcustomer AS Cu ON CA.cid = Cu.id LEFT JOIN tbservice AS s ON CA.sid = s.id LEFT JOIN tbvehicletype AS v ON Ca.vid = v.id WHERE status LIKE 'Pending' AND Ca.transno='" + lbltransno.Text + "'", dbcon.connect());
            dbcon.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                price = double.Parse(dr[6].ToString()) * double.Parse(dr[8].ToString());
                dgvCash.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString(), price, dr[9].ToString());
                total += price;
                carno = dr[3].ToString();
                carmodel = dr[4].ToString();
            }

            dr.Close();
            dbcon.Close();
            lblTotal.Text = total.ToString("#,##0.00");
        }
        #endregion method
    }
}
