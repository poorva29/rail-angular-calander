﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using Model;

namespace MiraiConsultMVC
{
    public class BasePage:System.Web.UI.Page
    {    /// <summary>
        /// Check for session expired if session is expired redirect to login page
        /// </summary>
        public void checkSessionExpired()
        {
            Utilities.checkSessionExpired();
        }        
        /// <summary>
        /// If any error then redirect to Custom error page. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void Page_Error(Object sender, EventArgs args)
        {
            Exception error = Server.GetLastError();
            Session["unHandledErrors"] = error;
            Response.Redirect("../admin/CustomError.aspx");
        }
        public void isAuthorisedandSessionExpired(int privilege)
        {
            Utilities.checkSessionExpired();
            if (!Utilities.isAuthorisedandSessionExpired(privilege))
            {
                Response.Redirect("/admin/NoPrivilegeError");
            }
        }
    }
}