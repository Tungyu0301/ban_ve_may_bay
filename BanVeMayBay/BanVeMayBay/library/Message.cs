using BanVeMayBay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanVeMayBay
{
    public static class Message
    {
        public static bool has_flash()
        {
            if (System.Web.HttpContext.Current.Session["Message"].Equals("")) {
                return false;
            }
            return true;
        }
        public static void set_flash(String msg, String msg_css) {
            MessageModel ms = new MessageModel();
            ms.msg = msg;
            ms.msg_css = msg_css;
            System.Web.HttpContext.Current.Session["Message"]=ms;
        }
         public static MessageModel get_flash() {
            MessageModel ms = (MessageModel)System.Web.HttpContext.Current.Session["Message"];
            System.Web.HttpContext.Current.Session["Message"] = "";
            return ms;
        }
    }
}