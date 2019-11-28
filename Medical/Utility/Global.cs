using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medical.Utility
{
    public class Global
    {
        public const string ROLE_ADMIN = "Admin";
        public const string ROLE_MANAGER = "Manager";
        public const string ROLE_CLERK = "Clerk";
        public const string ROLE_DELIVERY = "Delivery Body";
        public const string ROLE_CUSTOMER = "Customer";

        public static string GetShortString(string strFull)
        {
            return strFull.Length > 300 ? strFull.Substring(0, 300) + "..." : strFull;
        }
    }
}
