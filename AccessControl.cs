using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace carWashManagement
{
    internal class AccessControl
    {
        public static bool IsAdminOrSupervisor()
        {
            string role = Session.Role?.ToLower();
            return role == "manager" || role == "supervisor";
        }

        public static bool CanViewCustomerSensitive(bool isManagerOverride)
        {
            return IsAdminOrSupervisor() || isManagerOverride;
        }

        public static bool CanViewEmployerSensitive()
        {
            return IsAdminOrSupervisor();
        }
        public static bool CanAlterCustomer()
        {
            return IsAdminOrSupervisor();
        }
        public static bool CanAlterEmployer()
        {
            return IsAdminOrSupervisor();
        }
    }
}
