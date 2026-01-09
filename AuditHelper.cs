using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace carWashManagement
{
    internal class AuditHelper
    {
        public static void Log(string action, string module, string description)
        {
            try
            {
                if (Session.UserName == null || Session.Role == null)
                    return;

                dbConnect db = new dbConnect();

                string sql = $@"
                    INSERT INTO tbaudit
                    (username, role, action, module, description)
                    VALUES
                    ('{Session.UserName}',
                     '{Session.Role}',
                     '{action}',
                     '{module}',
                     '{description.Replace("'", "''")}')
                ";

                db.executeQuery(sql);
            }
            catch
            {
                // ❌ DO NOTHING – audit must never crash the system
            }
        }
    }
}
