//using Common;
using Common;
using Infrastructure.HelpingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Presentation.Controllers
{
    public class ShareUtility
    {
        public static int getSkip(Controller controller, bool skipflag, string commType, string skipCntName)
        {
            int skip = 0;
            if (skipflag == true)
            {
                if (commType == "Next")
                {

                    controller.TempData["Skip"] = (int)controller.TempData.Peek("Skip") + 10;
                    skip = (int)controller.TempData.Peek("Skip");
                }
                else
                {
                    if ((int)controller.TempData.Peek("Skip") > 0)
                    {
                        controller.TempData["Skip"] = (int)controller.TempData.Peek("Skip") - 10;
                        skip = (int)controller.TempData.Peek("Skip");
                    }
                    else
                    {
                        controller.TempData["Skip"] = 0;
                        skip = 0;
                    }
                }
            }
            else
            {
                controller.TempData["Skip"] = skip;
            }            
            if (HttpContext.Current.Request.Cookies["SkipCount"] != null)
            {
                HttpCookie skipCount = HttpContext.Current.Request.Cookies["SkipCount"];
                skipCount.Values[skipCntName] = Convert.ToString(skip);
            }
            return skip;
        }

        public static void CreateSkipCount(string skipCntName = "", bool isRemove = false)
        {
            if (isRemove && HttpContext.Current.Request.Cookies["SkipCount"] != null)
            {
                HttpContext.Current.Response.Cookies.Remove("SkipCount");
            }
            else
            {
                if (HttpContext.Current.Request.Cookies["SkipCount"] == null)
                {
                    HttpCookie skipCount = new HttpCookie("SkipCount");
                    skipCount.Values[skipCntName] = "0";
                    HttpContext.Current.Response.Cookies.Add(skipCount);
                }
            }
        }

        public static string RenderViewToString(ControllerContext context, string viewName, object model)
        {
            try
            {
                if (string.IsNullOrEmpty(viewName))
                    viewName = context.RouteData.GetRequiredString("action");

                var viewData = new ViewDataDictionary(model);

                using (var sw = new System.IO.StringWriter())
                {
                    var viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                    var viewContext = new ViewContext(context, viewResult.View, viewData, new TempDataDictionary(), sw);
                    viewResult.View.Render(viewContext, sw);

                    return sw.GetStringBuilder().ToString();
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error(string.Format("TravelCRM.Controllers.SharedUtility.RenderViewToString |Exception:{0} ", ex.ToString()));
                var sw = new System.IO.StringWriter();
                return sw.GetStringBuilder().ToString();
            }
        }
        //public static List<SelectListItem> GetWeekDays()
        //{
        //    List<SelectListItem> items = new List<SelectListItem>();
        //    items.Add(new SelectListItem { Text = "Any", Value = null });
        //    items.Add(new SelectListItem { Text = "Sunday", Value = ((int)WeekDays.Sunday).ToString() });
        //    items.Add(new SelectListItem { Text = "Monday", Value = ((int)WeekDays.Monday).ToString() });
        //    items.Add(new SelectListItem { Text = "Tuesday", Value = ((int)WeekDays.Tuesday).ToString() });
        //    items.Add(new SelectListItem { Text = "Wednesday", Value = ((int)WeekDays.Wednesday).ToString() });
        //    items.Add(new SelectListItem { Text = "Thursday", Value = ((int)WeekDays.Thursday).ToString() });
        //    items.Add(new SelectListItem { Text = "Friday", Value = ((int)WeekDays.Friday).ToString() });
        //    items.Add(new SelectListItem { Text = "Saturday", Value = ((int)WeekDays.Saturday).ToString() });
        //    return items;
        //}

        //public static List<SelectListItem> GetMarkupType()
        //{
        //    List<SelectListItem> items = new List<SelectListItem>();
        //    items.Add(new SelectListItem { Text = "Select markup type", Value = ((int)MarkupType.None).ToString() });
        //    items.Add(new SelectListItem { Text = "Amount", Value = ((int)MarkupType.Amount).ToString() });
        //    items.Add(new SelectListItem { Text = "Percentage", Value = ((int)MarkupType.Percentage).ToString() });
        //    return items;
        //}

        //public static List<SelectListItem> GetRoutesMarkup()
        //{
        //    List<SelectListItem> items = new List<SelectListItem>();
        //    items.Add(new SelectListItem { Text = "Select Routes Mark-up", Value = ((int)MarkupRoutes.None).ToString() });
        //    items.Add(new SelectListItem { Text = "Domestic", Value = ((int)MarkupRoutes.Domestic).ToString() });
        //    items.Add(new SelectListItem { Text = "International", Value = ((int)MarkupRoutes.International).ToString() });
        //    return items;
        //}

        public static string GetCurrencySymbol(string code)
        {
            string response = "$";
            try
            {
                if (Utility.Currencies != null && Utility.Currencies.Count > 0)
                {
                    Currency currency = Utility.Currencies.Where(o => o.CurrencyType.Equals(code)).FirstOrDefault();
                    if (currency != null)
                    {
                        response = currency.CurrencySymbol;
                    }
                }

            }
            catch (Exception ex)
            {
                Utility.Logger.Error(string.Format("ShareUtility.GetCurrencySymbol|Exception:", ex.ToString()));

            }
            return response;


        }

    }
}