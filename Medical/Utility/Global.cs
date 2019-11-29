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

        public const string ORDER_READY = "Ready to Pick Up";
        public const string ORDER_PICKED_UP = "Picked Up";
        public const string ORDER_DELIVERY_IN_PROGRESS = "Delivery In Progress";
        public const string ORDER_DELIVERED = "Delivered";
        public const string ORDER_RETURNED = "Returned";

        public static string GetShortString(string strFull)
        {
            return strFull.Length > 300 ? strFull.Substring(0, 300) + "..." : strFull;
        }
    }
}
