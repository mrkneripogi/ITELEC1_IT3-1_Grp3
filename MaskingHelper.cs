using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace carWashManagement
{
    internal class MaskingHelper
    {
        // Generic mask: preserves length
        public static string Mask(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return "*********";
            // OR: return new string('*', value.Length);
            // OR: return "AHWQAZ@!";
        }

        // Phone masking: show last 2 digits
        //public static string MaskPhone(string phone)
        //{
        //    if (string.IsNullOrEmpty(phone) || phone.Length < 4)
        //        return "****";

        //    return new string('*', phone.Length - 2) + phone[^2..];
        //}

        //// Name masking: first letter only
        //public static string MaskName(string name)
        //{
        //    if (string.IsNullOrEmpty(name))
        //        return name;

        //    return name[0] + new string('*', name.Length - 1);
        //}

        //// Plate masking: show first + last
        //public static string MaskPlate(string plate)
        //{
        //    if (string.IsNullOrEmpty(plate) || plate.Length < 3)
        //        return "***";

        //    return plate[0] +
        //           new string('*', plate.Length - 2) +
        //           plate[^1];
        //}

        //// Address masking: fixed obfuscation
        //public static string MaskAddress(string address)
        //{
        //    return "██████████";
        //}
    }
}
