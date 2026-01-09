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
    public partial class Main : Form
    {
        public bool isManager = false;
        //private Timer inactivityTimer;
        //private Timer logoutTimer;

        //private DateTime lastActivityTime;
        //private bool isWarningShown = false;

        public Main()
        {
            InitializeComponent();
            ApplyRoleAccess();
            loadGrossProfit();
            openChildForm(new Dashboard());

            //InitializeInactivityTimer();
        }

        private void ApplyRoleAccess()
        {

            if (isManager) return;

            //if (string.IsNullOrEmpty(Session.Role))
            //    return;

            string role = Session.Role.ToLower();

            if (role == "manager" || role == "supervisor")
            {
                EnableAllButtons();
                isManagerOverride.Enabled = false;
                return;
            }
                
            btnEmployer.Enabled = false;
            btnSetting.Enabled = false;
            btnReport.Enabled = false;
            btnService.Enabled = false;

            // Only these allowed
            btnDashboard.Enabled = true;
            btnCustomer.Enabled = true;
            btnCash.Enabled = true;
            
        }
        // Session Timeout
        //private void InitializeInactivityTimer()
        //{
        //    lastActivityTime = DateTime.Now;

        //    inactivityTimer = new Timer();
        //    inactivityTimer.Interval = 1000; // check every second
        //    inactivityTimer.Tick += InactivityTimer_Tick;
        //    inactivityTimer.Start();

        //    // Detect activity
        //    this.MouseMove += ResetActivity;
        //    this.KeyDown += ResetActivity;
        //    this.MouseClick += ResetActivity;
        //}

        //private void ResetActivity(object sender, EventArgs e)
        //{
        //    lastActivityTime = DateTime.Now;
        //    isWarningShown = false;

        //    logoutTimer?.Stop();
        //}

        //private void InactivityTimer_Tick(object sender, EventArgs e)
        //{
        //    TimeSpan inactiveTime = DateTime.Now - lastActivityTime;

        //    // 1 minute inactivity
        //    if (inactiveTime.TotalSeconds >= 60 && !isWarningShown)
        //    {
        //        isWarningShown = true;
        //        ShowInactivityWarning();
        //    }
        //}

        //private void ShowInactivityWarning()
        //{
        //    logoutTimer = new Timer();
        //    logoutTimer.Interval = 30000; // 30 seconds
        //    logoutTimer.Tick += (s, e) =>
        //    {
        //        logoutTimer.Stop();
        //        AutoLogout();
        //    };
        //    logoutTimer.Start();

        //    DialogResult result = MessageBox.Show(
        //        "Are you still there?\n\nYou will be logged out automatically in 30 seconds.",
        //        "Session Timeout Warning",
        //        MessageBoxButtons.YesNo,
        //        MessageBoxIcon.Warning
        //    );

        //    if (result == DialogResult.Yes)
        //    {
        //        lastActivityTime = DateTime.Now;
        //        isWarningShown = false;
        //        logoutTimer.Stop();
        //    }
        //    else
        //    {
        //        AutoLogout();
        //    }
        //}

        //private void AutoLogout()
        //{
        //    inactivityTimer.Stop();
        //    logoutTimer?.Stop();

        //    AuditHelper.Log("AUTO LOGOUT", "Main", "User Inactivity");

        //    isManager = false;
        //    Session.UserName = null;
        //    Session.Role = null;

        //    MessageBox.Show(
        //        "You have been logged out due to inactivity.",
        //        "Session Expired",
        //        MessageBoxButtons.OK,
        //        MessageBoxIcon.Information
        //    );

        //    this.Hide();
        //    new Login().Show();
        //}

        private void panel_logo_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            isManager = false;
            ApplyRoleAccess();

            panelSlide.Height = btnDashboard.Height;
            panelSlide.Top = btnDashboard.Top;
            openChildForm(new Dashboard());
        }

        private void btnEmployer_Click(object sender, EventArgs e)
        {
            panelSlide.Height = btnEmployer.Height;
            panelSlide.Top = btnEmployer.Top;
            openChildForm(new Employer());
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            isManager = false;
            ApplyRoleAccess();

            panelSlide.Height = btnCustomer.Height;
            panelSlide.Top = btnCustomer.Top;
            openChildForm(new Customer());
            
        }

        private void btnService_Click(object sender, EventArgs e)
        {
            panelSlide.Height = btnService.Height;
            panelSlide.Top = btnService.Top;
            openChildForm(new Service());
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            panelSlide.Height = btnLogout.Height;
            panelSlide.Top = btnLogout.Top;
            if (MessageBox.Show("Logout Application?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide();
                Login login = new Login();
                login.ShowDialog();
                AuditHelper.Log("LOGOUT", "Main", "User logged out");
            }
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            panelSlide.Height = btnSetting.Height;
            panelSlide.Top = btnSetting.Top;
            openChildForm(new Setting());
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            panelSlide.Height = btnReport.Height;
            panelSlide.Top = btnReport.Top;
            openChildForm(new Report());
        }

        private void btnCash_Click(object sender, EventArgs e)
        {
            isManager = false;
            ApplyRoleAccess();

            panelSlide.Height = btnCash.Height;
            panelSlide.Top = btnCash.Top;
            openChildForm(new Cash(this));
        }

        private void pictureBox1_Click(object sender, EventArgs e)
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
            panelChild.Controls.Add(childForm);
            panelChild.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }
        public void loadGrossProfit()
        {
            Report module = new Report();

            string dateNowMinus7 = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            string dateNowMinus14 = DateTime.Now.AddDays(-14).ToString("yyyy-MM-dd");
            string dateNow = DateTime.Now.ToString("yyyy-MM-dd");

            lblRevenues.Text = module.extractData("SELECT IFNULL(SUM(price),0) AS total FROM tbcash WHERE date > '" + dateNowMinus7 + "' AND status LIKE 'Sold' ").ToString("#,##0.00");
            lblCostofGood.Text = module.extractData("SELECT IFNULL(SUM(cost),0) AS Cost FROM tbcostofgood WHERE date > '" + dateNowMinus7 + "'").ToString("#,##0.00");
            lblGrossProfit.Text = (double.Parse(lblRevenues.Text) - double.Parse(lblCostofGood.Text)).ToString("#,##0.00");

            double revlast7 = module.extractData("SELECT IFNULL(SUM(price),0) AS total FROM tbcash WHERE date BETWEEN '" + dateNowMinus14 + "' AND '" + dateNowMinus7 + "' AND status LIKE 'Sold' ");
            double coglast7 = module.extractData("SELECT IFNULL(SUM(cost),0) AS Cost FROM tbcostofgood WHERE date BETWEEN  '" + dateNowMinus14 + "' AND '" + dateNowMinus7 + "'");
            double gplast7 = revlast7 - coglast7;

            if (revlast7 > double.Parse(lblRevenues.Text))
                picRevenues.Image = Properties.Resources.down_25px;
            else
                picRevenues.Image = Properties.Resources.up_25px;

            if (gplast7 > double.Parse(lblGrossProfit.Text))
            {
                picGrossProfit.Image = Properties.Resources.down_25px;
                lblGrossProfit.ForeColor = Color.Red;
            }
            else
            {
                picGrossProfit.Image = Properties.Resources.up_25px;
                lblGrossProfit.ForeColor = Color.Green;
            }
        }

        #endregion method

        private async void isManagerOverride_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please call a swipe from a Manager or Supervisor for permission... validating access",
            "Manager Permission", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Disable the button while "swipe" is being simulated
            isManagerOverride.Enabled = false;

            // Simulate the same logic as card payment (5 second wait)
            await Task.Delay(5000);

            // Now grant override
            isManager = true;
            //Session.ManagerSwipeActive = true;
            EnableAllButtons();

            MessageBox.Show("Access Granted.",
                "Access Granted", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (activeForm is Customer customerForm)
            {
                customerForm.UnmaskCustomer();
                customerForm.ApplyCustomerDecryption(isManager);
            }

            foreach (Form form in Application.OpenForms)
            {
                if (form is CashCustomer cashCustomerForm)
                {
                    cashCustomerForm.UnmaskCustomer();
                }
            }

            // Enable button again

            
            isManagerOverride.Enabled = true;

            AuditHelper.Log("OVERRIDE", "Security", "Requested Swipe");
        }

        private void EnableAllButtons()
        {
            foreach (Control c in flowLayoutPanel1.Controls)
            {
                if (c is Button btn)
                    btn.Enabled = true;
            }
        }

        private void lblCostofGood_Click(object sender, EventArgs e)
        {

        }

        private void lblGrossProfit_Click(object sender, EventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
    }
}
