﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;

namespace MiraiConsultMVC.Controllers
{
    public class CustomActionAttribute : FilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.Controller.ViewBag.OnActionExecuted = "IActionFilter.OnActionExecuted filter called";
        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Convert.ToInt32(filterContext.HttpContext.Session["UserType"]) == Convert.ToInt32(UserType.Doctor))
            {
                filterContext.Result = new RedirectResult("/questions");
            }
            else if (Convert.ToInt32(filterContext.HttpContext.Session["UserType"]) == Convert.ToInt32(UserType.Patient))
            {
                filterContext.Result = new RedirectResult("/answers");
            }
            else if (filterContext.HttpContext.Session["UserType"] != null && Convert.ToInt32(filterContext.HttpContext.Session["UserType"]) == Convert.ToInt32(UserType.SuperAdmin))
            {
                filterContext.Result = new RedirectResult("/doctors");
            }
            else
            {
                if (!HttpContext.Current.Request.Url.AbsoluteUri.Contains("login"))
                    filterContext.Result = new RedirectResult("/login");
            }
        }
    }
}