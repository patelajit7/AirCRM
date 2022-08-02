using Infrastructure.Entities;
using Infrastructure.HelpingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelCRM.Authentication
{
    public class UserSessionsHandler
    {
        public static UserData LoggedInUser
        {
            get { return HttpContext.Current.Session["LoggedInUser"] == null ? null : HttpContext.Current.Session["LoggedInUser"] as UserData; }
            set { HttpContext.Current.Session["LoggedInUser"] = value; }
        }
    }
}