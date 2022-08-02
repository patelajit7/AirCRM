using Common;
using Database;
using Infrastructure.HelpingModels;
using Infrastructure.HelpingModels.Masters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class WinTrackingBusiness
    {
        public static List<EmployeeUsage> GetEmployeeUsage(int _companyId, string _employeeId, DateTime _date)
        {
            List<EmployeeUsage> response = null;
            try
            {
                response = Procedures.GetEmployeeUsage(_companyId, _employeeId, _date, Utility.ConnString);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Business.WinTrackingBusiness.GetEmployeeUsage | Exception: " + ex.Message);
            }
            return response;
        }
        public static List<EmployeeApplicationUsage> GetApplicationUsage(int _companyId, int _employeeId, DateTime _date)
        {
            List<EmployeeApplicationUsage> response = null;
            try
            {
                response = Procedures.GetApplicationUsage(_companyId, _employeeId, _date, Utility.ConnString);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Business.WinTrackingBusiness.GetApplicationUsage | Exception: " + ex.Message);
            }
            return response;
        }
        public static List<WindowUsage> GetWindowUsage(int _companyId, int _employeeId, int _applicationId, DateTime _date)
        {
            List<WindowUsage> response = null;
            try
            {
                response = Procedures.GetWindowUsage(_companyId, _employeeId, _applicationId, _date, Utility.ConnString);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Business.WinTrackingBusiness.GetWindowUsage | Exception: " + ex.Message);
            }
            return response;
        }
        public static WindowKeystrokes GetKeystrokes(int _companyId, int _employeeId, int _windowId, DateTime _date)
        {
            WindowKeystrokes response = null;
            try
            {
                response = Procedures.GetKeystrokes(_companyId, _employeeId, _windowId, _date, Utility.ConnString);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Business.WinTrackingBusiness.GetKeystrokes | Exception: " + ex.Message);
            }
            return response;
        }
        public static DashboardStatistics GetDashboardStatistics(int _companyId)
        {
            DashboardStatistics response = null;
            try
            {
                response = Procedures.GetDashboardStatistics(_companyId, Utility.ConnString);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Business.WinTrackingBusiness.GetDashboardStatistics | Exception: " + ex.Message);
            }
            return response;
        }

        public static List<DateRangeData> GetDateRangeData(int _companyId, string _employeeId, DateTime _fromDate, DateTime _toDate)
        {
            List<DateRangeData> response = null;
            try
            {
                response = Procedures.GetDateRangeData(_companyId, _employeeId, _fromDate, _toDate, Utility.ConnString);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Business.WinTrackingBusiness.GetDateRangeData | Exception: " + ex.Message);
            }
            return response;
        }
        public static ProcedureResponse<List<User>> UserAddOrUpdate(User _user)
        {
            ProcedureResponse<List<User>> response = null;
            try
            {
                DataTable dt = GetUserParmissionTable(_user);
                response = Procedures.UserAddOrUpdate(_user, dt, Utility.ConnString);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Business.WinTrackingBusiness.UserAddOrUpdate | Exception: " + ex.Message);
            }
            return response;
        }
        public static ProcedureResponse<List<User>> UsersGet(int? _userId, int _companyId)
        {
            ProcedureResponse<List<User>> response = null;
            try
            {

                response = Procedures.UsersGet(_userId, _companyId, Utility.ConnString);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Business.WinTrackingBusiness.UsersGet | Exception: " + ex.Message);
            }
            return response;
        }
        public static ProcedureResponse<User> UserLogin(string _userName, string _userPassword)
        {
            ProcedureResponse<User> response = null;
            try
            {

                response = Procedures.UsersLogin(_userName, _userPassword, Utility.ConnString);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Business.WinTrackingBusiness.UserLogin | Exception: " + ex.Message);
            }
            return response;
        }

        public static ProcedureResponse<bool> UsersDelete(int _userId, int _companyId)
        {
            ProcedureResponse<bool> response = null;
            try
            {

                response = Procedures.UsersDelete(_userId, _companyId, Utility.ConnString);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Business.WinTrackingBusiness.UsersDelete | Exception: " + ex.Message);
            }
            return response;
        }

        public static ProcedureResponse<bool> UsersExist(string _userName)
        {
            ProcedureResponse<bool> response = null;
            try
            {
                response = Procedures.UsersExist(_userName, Utility.ConnString);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Business.WinTrackingBusiness.UsersExist | Exception: " + ex.Message);
            }
            return response;
        }
        private static DataTable GetUserParmissionTable(User _user)
        {
            DataTable response = null;
            try
            {
                if (_user != null && _user.UserPermissions != null && _user.UserPermissions.Count > 0)
                {
                    response = new DataTable();
                    response.Columns.Add("Id");
                    response.Columns.Add("UserId");
                    response.Columns.Add("PermissionId");
                    response.Columns.Add("CompanyId");
                    DataRow dr = null;
                    foreach (var item in _user.UserPermissions.Where(o => o.IsSelected))
                    {
                        dr = response.NewRow();
                        dr["Id"] = item.Id;
                        dr["UserId"] = item.UserId;
                        dr["PermissionId"] = item.PermissionId;
                        dr["CompanyId"] = item.CompanyId;
                        response.Rows.Add(dr);
                    }

                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Business.WinTrackingBusiness.GetUserParmissionTable | Exception: " + ex.Message);
            }
            return response;
        }
    }
}
