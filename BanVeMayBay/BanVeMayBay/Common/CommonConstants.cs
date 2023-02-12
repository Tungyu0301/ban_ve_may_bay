using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanVeMayBay.Common
{
    public class CommonConstants
    {
        public static string ADMIN_GROUP = "ADMIN";
        public static string USER_SESSION = "USER_SESSION";
        public static string CUSTOMER_SESSION = "CUSTOMER_SESSION";
        public static string SESSION_CREDENTIALS = "SESSION_CREDENTIALS";
        public static string CartSession = "CartSession";
        public static string CurrentCulture { set; get; }
    }
}