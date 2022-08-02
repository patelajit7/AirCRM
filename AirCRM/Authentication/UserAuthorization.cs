using Common;
using Infrastructure.HelpingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace TravelCRM.Authentication
{
    public class UserAuthorization : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var user = UserSessionsHandler.LoggedInUser;

            if (user == null || user.UserId==0)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new JsonResult
                    {
                        Data = new { isSuccess = false, message = "Session out client action (Ajax)" },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                    filterContext.HttpContext.Response.StatusCode = 401;
                    filterContext.HttpContext.Response.StatusDescription = "User should be authonticate";
                    filterContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
                }
                else
                {
                    filterContext.Result = new HttpUnauthorizedResult();
                }
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            var user = UserSessionsHandler.LoggedInUser;

            if (user == null || user.UserId == 0)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new JsonResult
                    {
                        Data = new { isSuccess = false, message = "SessionTimeOutThroughJQueryAjax" },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                    filterContext.HttpContext.Response.StatusCode = 401;
                    filterContext.HttpContext.Response.StatusDescription = "Humans and robots must authenticate";
                    filterContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
                }
                else
                {
                    filterContext.Result = new RedirectResult(string.Format("{0}", Utility.Settings.DomainUrl));
                }
            }
        }
    }

    public class RoleAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] allowedroles;
        public RoleAuthorizeAttribute(params string[] roles)
        {
            this.allowedroles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            UserData user = UserSessionsHandler.LoggedInUser;
            if (user != null)
            {
                foreach (var role in allowedroles)
                {
                    if (user.Roles.Any(o => o.Equals(role)))
                    {
                        authorize = true;
                        break;
                    }

                }
            }
            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
               new RouteValueDictionary
               {
                    { "controller", "Account" },
                    { "action", "Login" }
               });
        }
    }

}