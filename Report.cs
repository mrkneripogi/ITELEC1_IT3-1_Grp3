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
    public partial class Report : Form
    {
        MySqlCommand cm = new MySqlCommand();
        dbConnect dbcon = new dbConnect();
        MySqlDataReader dr;
        string title = "Auto Spa Car Wash Management System";

        public Report()
        {
            InitializeComponent();
            loadTopSelling();
            loadRevenus();
            loadCostofGood();
            loadGrossProfit();
        }
        #region topselling
        private void dtFromTopSelling_ValueChanged_1(object sender, EventArgs e)
        {
            loadTopSelling();

        }

        private void dtToTopSelling_ValueChanged_1(object sender, EventArgs e)
        {
            loadTopSelling();
        }

        // to load top selling 
        public void loadTopSelling()
        {
            try
            {
                int i = 0;
                dgvTopSelling.Rows.Clear();
                cm = new MySqlCommand("SELECT se.name, count(ca.sid) AS qty, IFNULL(SUM(ca.price),0) AS total " +
                      "FROM tbcash AS ca " +
                      "JOIN tbservice AS se ON ca.sid=se.id " +
                      "WHERE ca.date BETWEEN '" + dtFromTopSelling.Value.ToString("yyyy-MM-dd") + "' AND '" + dtToTopSelling.Value.ToString("yyyy-MM-dd") + "' " +
                      "AND status LIKE 'Sold' " +
                      "GROUP BY se.name ORDER BY qty DESC LIMIT 10", dbcon.connect());
                dbcon.Open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dgvTopSelling.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString());
                }
                dr.Close();
                dbcon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }


        #endregion top selling
        #region Revenus

        private void dtFromRevenud_ValueChanged(object sender, EventArgs e)
        {
            loadRevenus();
        }

        private void dtToRevenus_ValueChanged(object sender, EventArgs e)
        {
            loadRevenus();
        }

        public void loadRevenus()
        {
            // select query revenus data from the database
            try
            {
                int i = 0;
                dgvRevenus.Rows.Clear();
                double total = 0;
                cm = new MySqlCommand("SELECT date,IFNULL(sum(price),0) AS total FROM tbcash WHERE date BETWEEN '" + dtFromRevenud.Value.ToString("yyyy-MM-dd") + "' " +
                    "AND '" + dtToRevenus.Value.ToString("yyyy-MM-dd") + "' AND status LIKE 'Sold' GROUP BY date", dbcon.connect());
                dbcon.Open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dgvRevenus.Rows.Add(i, DateTime.Parse(dr[0].ToString()).ToShortDateString(), dr[1].ToString());
                    total += double.Parse(dr[1].ToString());
                }
                lblRevenus.Text = total.ToString("#,##0.00");
                dr.Close();
                dbcon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }

        #endregion Revenus

        #region CostofGood
        private void dtFromCoG_ValueChanged(object sender, EventArgs e)
        {
            loadCostofGood();
        }

        private void dtToCoG_ValueChanged(object sender, EventArgs e)
        {
            loadCostofGood();
        }
        public void loadCostofGood()
        {
            try
            {
                int i = 0; // to show number for cost of good sole
                dgvCostofGood.Rows.Clear();
                double total = 0;
                cm = new MySqlCommand("SELECT costname,cost,date FROM tbcostofgood WHERE date BETWEEN '" + dtFromCoG.Value.ToString("yyyy-MM-dd") + "' AND '" + dtToCoG.Value.ToString("yyyy-MM-dd") + "'", dbcon.connect());
                dbcon.Open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    // to add data to the datagridview from the database
                    dgvCostofGood.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), DateTime.Parse(dr[2].ToString()).ToShortDateString());
                    total += double.Parse(dr[1].ToString());
                }
                lblCoG.Text = total.ToString("#,##0.00");
                dr.Close();
                dbcon.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }
        #endregion CostofGood
        #region GrossProfit

        private void dtFromGP_ValueChanged(object sender, EventArgs e)
        {
            loadGrossProfit();
        }

        private void dtToGP_ValueChanged(object sender, EventArgs e)
        {
            loadGrossProfit();
        }

        public void loadGrossProfit()
        {
            txtRevenus.Text = extractData("SELECT IFNULL(SUM(price),0) AS total FROM tbcash WHERE date BETWEEN '" + dtFromGP.Value.ToString("yyyy-MM-dd") + "' AND '" + dtToGP.Value.ToString("yyyy-MM-dd") + "'").ToString("#,##0.00");
            txtCoG.Text = extractData("SELECT IFNULL(SUM(cost),0) AS Cost FROM tbcostofgood WHERE date BETWEEN '" + dtFromGP.Value.ToString("yyyy-MM-dd") + "' AND '" + dtToGP.Value.ToString("yyyy-MM-dd") + "'").ToString("#,##0.00");
            txtGrossProfit.Text = (double.Parse(txtRevenus.Text) - double.Parse(txtCoG.Text)).ToString("#,##0.00");
            if (double.Parse(txtGrossProfit.Text) < 0)
                txtGrossProfit.ForeColor = Color.Red;
            else
                txtGrossProfit.ForeColor = Color.Green;
        }

        // now to create a function to exract data from database
        public double extractData(string sql)
        {
            dbcon.Open();
            cm = new MySqlCommand(sql, dbcon.connect());
            double data = double.Parse(cm.ExecuteScalar().ToString());
            dbcon.Close();
            return data;
        }

        #endregion GrossProfit

        private void txtRevenus_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvTopSelling_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
