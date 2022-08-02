using ClosedXML.Excel;
using Common;
using Database;
using Infrastructure.Entities;
using Infrastructure.HelpingModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class EmailBusiness
    {
        public static void SendDailyUsageViaEmail()
        {
            try
            {
                PrepareAndSend();
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("EmailBusiness.SendDailyUsageViaEmail | Exception: " + ex.Message);
            }
        }
        private static void PrepareAndSend()
        {
            try
            {
                Utility.Logger.Info("Scheduler Run started...");
                List<Companies> companies = Utility.DatabaseService.Where<Companies>(o => o.IsActive).OrderBy(c => c.Id).ToList();
                if (companies != null && companies.Count > 0)
                {
                    foreach (var com in companies)
                    {
                        List<EmployeeUsage> empList = Procedures.GetEmployeeUsage(com.Id, null, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), Utility.ConnString);
                        if (empList != null && empList.Count > 0)
                        {
                            List<string> lstHead = empList.Select(x => x.HeadEmail).Distinct().ToList();
                            if (lstHead != null && lstHead.Count > 0)
                            {
                                foreach (string email in lstHead)
                                {
                                    if (!string.IsNullOrEmpty(email))
                                    {
                                        List<EmployeeUsage> lstToSendEmail = empList.Where(o => o.HeadEmail.Equals(email)).ToList();
                                        if (lstToSendEmail != null && lstToSendEmail.Count > 0)
                                        {
                                            string wb = string.Empty;
                                            if (com.SendMailWithAttachment)
                                            {
                                                wb = PrepareExcelSheet(lstToSendEmail);
                                            }
                                            string htmlTable = Utility.ExportDatatableToHtml(ConvertToDataTable(lstToSendEmail));
                                            string mailBody = GetEmailBody(lstToSendEmail[0].ReportingHead, htmlTable);
                                            SendEmail(email, "Daily statistics of employee", mailBody, wb);

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("EmailBusiness.PrepareAndSend | Exception: " + ex.Message);
            }
            Utility.Logger.Info("Scheduler Run ended...");
        }
        private static string PrepareExcelSheet(List<EmployeeUsage> lstToSendEmail)
        {
            string path = string.Empty;
            XLWorkbook wb = null;
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("EmployeeCode", typeof(string));
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("StartTime", typeof(string));
                dt.Columns.Add("EndTime", typeof(string));
                dt.Columns.Add("Worked", typeof(string));
                dt.Columns.Add("Idle", typeof(string));
                dt.Columns.Add("Locked", typeof(string));
                foreach (var item in lstToSendEmail)
                {
                    DataRow dr = dt.NewRow();
                    dr["EmployeeCode"] = item.EmployeeCode;
                    dr["Name"] = item.Name;
                    dr["StartTime"] = string.Format("{0}", item.Start != null ? Convert.ToDateTime(item.Start).ToString("HH:mm") : "--");
                    dr["EndTime"] = string.Format("{0}", item.End != null ? Convert.ToDateTime(item.End).ToString("HH:mm") : "--");
                    dr["Worked"] = Utility.Convert(item.Login);
                    dr["Idle"] = Utility.Convert(item.Idle);
                    dr["Locked"] = Utility.Convert(item.Locked);
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                }
                wb = new XLWorkbook();
                wb.Worksheets.Add(dt, "Statistics");
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                path = string.Format("{0}\\{1}", Utility.Settings.ExcelPath, string.Format("EmployeeStatistics{0}.xlsx", DateTime.Now.ToString("yyyyddMMHHmmss")));
                wb.SaveAs(path);

            }
            catch (Exception ex)
            {
                Utility.Logger.Error("EmailBusiness.PrepareExcelSheet | Exception: " + ex.Message);
            }
            return path;
        }
        private static DataTable ConvertToDataTable(List<EmployeeUsage> lstToSendEmail)
        {
            DataTable dt = new DataTable();
            try
            {
                dt.Columns.Add("EmployeeCode", typeof(string));
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("StartTime", typeof(string));
                dt.Columns.Add("EndTime", typeof(string));
                dt.Columns.Add("Worked", typeof(string));
                dt.Columns.Add("Idle", typeof(string));
                dt.Columns.Add("Locked", typeof(string));
                foreach (var item in lstToSendEmail)
                {
                    DataRow dr = dt.NewRow();
                    dr["EmployeeCode"] = item.EmployeeCode;
                    dr["Name"] = item.Name;
                    dr["StartTime"] = string.Format("{0}", item.Start != null ? Convert.ToDateTime(item.Start).ToString("HH:mm") : "--");
                    dr["EndTime"] = string.Format("{0}", item.End != null ? Convert.ToDateTime(item.End).ToString("HH:mm") : "--");
                    dr["Worked"] = Utility.Convert(item.Login);
                    dr["Idle"] = Utility.Convert(item.Idle);
                    dr["Locked"] = Utility.Convert(item.Locked);
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                }

            }
            catch (Exception ex)
            {
                Utility.Logger.Error("EmailBusiness.ConvertToDataTable | Exception: " + ex.Message);
            }
            return dt;
        }
        private static void SendEmail(string _mailTo, string _mailSubject, string _mailBody, string _attachment)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(Utility.Settings.EmailSettings.SMTPClient);
                mail.From = new MailAddress(Utility.Settings.EmailSettings.Email);
                mail.To.Add(_mailTo);
                mail.Subject = _mailSubject;
                mail.Body = _mailBody;
                mail.IsBodyHtml = true;

                if (!string.IsNullOrEmpty(_attachment))
                {
                    Attachment attachment;
                    attachment = new Attachment(_attachment, "application/vnd.ms-excel");
                    mail.Attachments.Add(attachment);
                }

                SmtpServer.Port = Utility.Settings.EmailSettings.Port;
                SmtpServer.EnableSsl = Utility.Settings.EmailSettings.EnableSSL;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new NetworkCredential(Utility.Settings.EmailSettings.Email, Utility.Settings.EmailSettings.Password);
                SmtpServer.Send(mail);

            }
            catch (Exception ex)
            {
                Utility.Logger.Error("EmailBusiness.SendEmail | Exception: " + ex.Message);
            }
        }

        private static Stream ConvertWorkbookToStream(XLWorkbook workbook)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream, new SaveOptions { EvaluateFormulasBeforeSaving = false, GenerateCalculationChain = false, ValidatePackage = false });

                    return stream;
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("EmailBusiness.ConvertWorkbookToStream | Exception: " + ex.Message);
            }
            return null;
        }
        private static string GetEmailBody(string name, string htmlTable)
        {
            string response = null;
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("<!DOCTYPE html PUBLIC ' -//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
                stringBuilder.Append("<html xmlns='http://www.w3.org/1999/xhtml' lang='en-GB'>");
                stringBuilder.Append("<head>");
                stringBuilder.Append("<meta http-equiv='Content -Type' content='text/html; charset=UTF-8' />");
                stringBuilder.Append("<title>Employee Daily Statistics</title>");
                stringBuilder.Append("<meta name='viewport' content='width=device-width, initial-scale=1.0' />");
                stringBuilder.Append("<style type='text /css'>");
                stringBuilder.Append(" a[x-apple-data-detectors] {");
                stringBuilder.Append("color: inherit !important;");
                stringBuilder.Append("}");
                stringBuilder.Append("</style>");
                stringBuilder.Append("</head>");
                stringBuilder.Append("<body style='margin: 20px; padding: 0; '>");
                stringBuilder.Append("<table role='presentation' border='0' cellpadding='0' cellspacing='0' width='100% '>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td style='color: #153643; font - family: Arial,sans - serif'>");
                stringBuilder.Append("<h1 style='font -size: 18px; margin: 0; '>Hi " + name + "</h1>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td style='color: #153643; font - family: Arial,sans - serif; font-size: 16px; line-height: 30px; padding: 20px 0 30px 0;'>");
                stringBuilder.Append("<h3 style='margin: 0; '>Today's statistics of employee.</h3>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("</table>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("</table>");
                stringBuilder.Append("<br>");
                stringBuilder.Append(htmlTable);

                stringBuilder.Append("</body>");
                stringBuilder.Append("</html>");
                response = stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("EmailBusiness.GetEmailBody | Exception: " + ex.Message);
            }
            return response;
        }
    }
}
