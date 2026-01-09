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
    public partial class SettlePaymentCard : Form
    {
        MySqlCommand cm = new MySqlCommand();
        dbConnect dbcon = new dbConnect();
        string title = "Auto Spa Car Wash Management System";

        Cash cash;  // reference to Cash module
        Timer timer;  // 5-second simulation timer

        public SettlePaymentCard(Cash cashForm)
        {
            InitializeComponent();
            cash = cashForm;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lblStatus_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void SettlePaymentCard_Load(object sender, EventArgs e)
        {
            lblAmount.Text = cash.lblTotal.Text;   // show total amount
            lblStatus.Text = "Processing payment...";

            // start a 5-second simulation
            timer = new Timer();
            timer.Interval = 5000;  // 5 seconds
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();  // stop the timer
            ProcessCardPayment();
        }
        private void ProcessCardPayment()
        {
            try
            {
                // Fake success message
                lblStatus.Text = "Payment Successful!";
                lblStatus.ForeColor = System.Drawing.Color.Green;

                // UPDATE DATABASE LIKE CASH MODULE
                for (int i = 0; i < cash.dgvCash.Rows.Count; i++)
                {
                    dbcon.executeQuery("UPDATE tbcash SET status='Sold', price='" +
                        cash.dgvCash.Rows[i].Cells[9].Value.ToString() +
                        "' WHERE id='" + cash.dgvCash.Rows[i].Cells[1].Value.ToString() + "'");

                    dbcon.executeQuery("UPDATE tbcustomer SET points = points + 1 WHERE id='" + cash.customerId + "'");
                }

                // ---------------------------
                // SHOW RECEIPT FOR CARD PAYMENT
                // ---------------------------
                string cardAmount = lblAmount.Text;
                string cardChange = "0.00";    // card has no change
                string[] cardTypes = { "MasterCard", "Visa", "American Express" };
                Random rnd = new Random();
                string ptype = cardTypes[rnd.Next(cardTypes.Length)];

                receipt r = new receipt(cash, cardAmount, cardChange, ptype);
                r.ShowDialog();

                // Reload data
                cash.loadCash();
                Main main = new Main();
                main.loadGrossProfit();

                MessageBox.Show("Card payment successful!", "Payment", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Reset UI on cash form
                cash.btnAddCustomer.Enabled = true;
                cash.btnAddService.Enabled = false;
                cash.getTransno();

                this.Dispose();
                AuditHelper.Log("PAYMENT", "Card", "Processed cashless payment");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
