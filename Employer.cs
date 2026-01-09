using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using MySql.Data.MySqlClient;

namespace carWashManagement
{
    public partial class Employer : Form
    {
        MySqlCommand cm = new MySqlCommand();
        dbConnect dbcon = new dbConnect();
        MySqlDataReader dr;

        //private bool isManager;
        string title = "Auto Spa Car Wash Management System";


        public Employer()
        {
            InitializeComponent();
            loadEmployer(); // to call this function , this form starting
            ApplyEmployerMasking();

        }

        private void ApplyEmployerMasking()
        {
            bool canView = AccessControl.CanViewEmployerSensitive();

            foreach (DataGridViewRow row in dgvEmployer.Rows)
            {
                if (row.IsNewRow) continue;

                if (!canView)
                {
                    row.Cells[3].Value = MaskingHelper.Mask(row.Cells[3].Value?.ToString()); // Phone
                    row.Cells[4].Value = MaskingHelper.Mask(row.Cells[4].Value?.ToString()); // Address
                    row.Cells[7].Value = MaskingHelper.Mask(row.Cells[7].Value?.ToString()); // Salary
                    row.Cells[8].Value = MaskingHelper.Mask(row.Cells[8].Value?.ToString()); // Password
                }
            }
        }

        private void Employer_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            EmployerModule module = new EmployerModule(this);
            module.btnUpdate.Enabled = false; // not a save for an edit update
            module.ShowDialog();
        }


        // another region method to query employer list data from the database to the datagridview
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            loadEmployer();
        }
        private void dgvEmployer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            EmployerModule module = new EmployerModule(this);

            string colName = dgvEmployer.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {

                if (!AccessControl.CanAlterEmployer())
                {
                    MessageBox.Show("You are not authorized to edit sensitive employer information.",
                        "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // to sent employer data to the employer module
                // cbrole wasnt in the index
                
                module.IbIEid.Text = dgvEmployer.Rows[e.RowIndex].Cells[1].Value.ToString();
                module.txtName.Text = dgvEmployer.Rows[e.RowIndex].Cells[2].Value.ToString();
                module.txtPhone.Text = dgvEmployer.Rows[e.RowIndex].Cells[3].Value.ToString();
                module.txtAddress.Text = dgvEmployer.Rows[e.RowIndex].Cells[4].Value.ToString();
                module.dtDob.Text = dgvEmployer.Rows[e.RowIndex].Cells[5].Value.ToString();
                module.rdMale.Checked = dgvEmployer.Rows[e.RowIndex].Cells[6].Value.ToString()=="Male"?true:false;// like if condition
                module.cbRole.Text = dgvEmployer.Rows[e.RowIndex].Cells[6].Value.ToString();
                module.txtSalary.Text = dgvEmployer.Rows[e.RowIndex].Cells[7].Value.ToString();
                module.txtPassword.Text = dgvEmployer.Rows[e.RowIndex].Cells[8].Value.ToString();

                module.btnSave.Enabled = false;
                module.ShowDialog();
            }
            else if(colName == "Delete")
            {
                if (!AccessControl.CanAlterEmployer())
                {
                    MessageBox.Show("You are not authorized to delete sensitive employer information.",
                        "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    if(MessageBox.Show("Are you sure you want to delete this record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new MySqlCommand("DELETE FROM tbemployer WHERE id LIKE'" + dgvEmployer.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", dbcon.connect());
                        dbcon.Open();
                        cm.ExecuteNonQuery();
                        dbcon.Close();
                        MessageBox.Show("Employer data has been successfully removed!", title, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        AuditHelper.Log("DELETE", "Employer", "Delete user record");

                        loadEmployer();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, title);
                }
            }
        }

        public void loadEmployer()
        {
            try
            {
                int i = 0; // to show number for employer list 
                dgvEmployer.Rows.Clear();
                cm = new MySqlCommand("SELECT * FROM tbemployer WHERE CONCAT (name,address,role) LIKE '%"+txtSearch.Text+"%'", dbcon.connect());
                dbcon.Open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    // to add data to the datagridview from the database
                    dgvEmployer.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), DateTime.Parse(dr[4].ToString()).ToShortDateString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString(), dr[8].ToString());
                }
                dbcon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        //MaskEmployeeData();
        }
    }
}
