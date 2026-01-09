using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace carWashManagement
{
    internal class dbConnect
    {
        MySqlCommand cm = new MySqlCommand();
        MySqlConnection cn = new MySqlConnection("server=localhost;uid=root;pwd=J.neri2307;database=carwashmanagement;");
        
        public MySqlConnection connect()
        {
            return cn;
        }
        
        public void Open()
        {
            if (cn.State == System.Data.ConnectionState.Closed)
                cn.Open();
        }

        public void Close() 
        {
            if (cn.State == System.Data.ConnectionState.Open)
                cn.Close();
        }

        public void executeQuery(string sql)
        {
            try
            {
                Open();
                cm = new MySqlCommand(sql, connect());
                cm.ExecuteNonQuery();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Car Wash Management System");
            }
        }
    } 
}
