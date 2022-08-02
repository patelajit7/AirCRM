#region Using Statement
using Business;
using Common;
using Infrastructure.HelpingModels;
using System.Web.Mvc;
using TravelCRM.Authentication;
#endregion
namespace TravelCRM.Controllers
{
    [UserAuthorization]
    [RoleAuthorize("Admin", "Agent", "Supervisor")]
    public class DashboardController : Controller
    {
        [Route("dashboard")]
        public ActionResult Dashboard()
        {
            DashboardData data = null;
            try
            {
                data = BookingBusiness.DashboardDataGet();
                if (data == null)
                {
                    data = new DashboardData() { Completed = 0, Inprogress = 0, TotalBooking = 0, NewBooking = 0 };
                }
            }
            catch (System.Exception ex)
            {
                Utility.Logger.Error("DashboardController.Dashboard|EXCEPTION:" + ex.ToString());
            }

            return View(data);
        }
    }
}