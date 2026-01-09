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
    public partial class receipt : Form
    {

        MySqlCommand cm = new MySqlCommand();
        dbConnect dbcon = new dbConnect();
        MySqlDataReader dr;
        string company, address;
        Cash cash;

        public receipt(Cash cashform, string pcash, string pchange, string ptype)
        {
            InitializeComponent();
            cash = cashform;
            loadCompany();
            loadReceipt(pcash, pchange, ptype); // Call the label-based method
        }

        private void receipt_Load(object sender, EventArgs e)
        {
        }

        public void loadCompany()
        {
            cm = new MySqlCommand("SELECT * FROM tbcompany", dbcon.connect());
            dbcon.Open();
            dr = cm.ExecuteReader();
            if (dr.Read()) // only reads first row
            {
                company = dr["name"].ToString();
                address = dr["address"].ToString();
                MessageBox.Show("Press enter for no refund"); // ✅ for testing
            }
            else
            {
                MessageBox.Show("No rows returned from tbcompany!");
            }
            dr.Close();
            dbcon.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void lblTransNo_Click(object sender, EventArgs e)
        {

        }

        private void lblSukli_Click(object sender, EventArgs e)
        {

        }

        public void loadReceipt(string pcash, string pchange, string ptype)
        {
            lblService.Text = lblService1.Text = lblService2.Text = "";
            lblPrice.Text = lblPrice1.Text = lblPrice2.Text = "";

            // Set company details
            lblCompany.Text = company;
            lblAddress.Text = address;

            // Set transaction details
            lblTransNo.Text = cash.lbltransno.Text;
            lblCarno.Text = cash.carno;
            lblTransNo.Text = cash.lbltransno.Text;
            lblCarmodel.Text = cash.carmodel;

            // Set service and price (you'll need to manually extract the first row or loop if multiple)
            if (cash.dgvCash.Rows.Count > 0)
            {
                lblService.Text = cash.dgvCash.Rows[0].Cells[8].Value.ToString();
                lblPrice.Text = cash.dgvCash.Rows[0].Cells[9].Value.ToString();
            }
            if (cash.dgvCash.Rows.Count > 1)
            {
                lblService1.Text = cash.dgvCash.Rows[1].Cells[8].Value.ToString();
                lblPrice1.Text = cash.dgvCash.Rows[1].Cells[9].Value.ToString();
            }
            if (cash.dgvCash.Rows.Count > 2)
            {
                lblService2.Text = cash.dgvCash.Rows[2].Cells[8].Value.ToString();
                lblPrice2.Text = cash.dgvCash.Rows[2].Cells[9].Value.ToString();
            }

            // Set totals and payment info
            lblTotal.Text = cash.lblTotal.Text;
            lblCash.Text = pcash;
            lblSukli.Text = pchange;
            lblPaymentType.Text = ptype;

            lblDate.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy");
            lblThankYou.Text = "Thank you for your visit!";
        }
    }
}
