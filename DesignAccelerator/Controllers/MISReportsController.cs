using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using DesignAccelerator.Models.ViewModel;
using System.Data.Entity;
using System.Data;
using System.Reflection;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DesignAccelerator.Controllers
{
    public class MISReportsController : Controller
    {
        private static DataTable dt, dt1;
        private static string operation = "";
        static List<usermngdata> res = new List<usermngdata>();
        static List<clientInfo> result = new List<clientInfo>();
        List<MISReports> misreportsList;
        List<MISReports> misreportsList1;
        static string selectdValueCopy = "";
        static string byRegionCopy = "";
        static string byApplicationCopy = "";
        static MISReports misReportsViewModel = new MISReports();

        // GET: MISReports
        [NoDirectAccess]
        public ActionResult Index()//string actionName
        {
            try
            {
                string urlPart = @"/Client";
                if (System.Web.HttpContext.Current.Request.UrlReferrer.ToString().IndexOf(urlPart) > 0)
                    Session["PreviousURL"] = System.Web.HttpContext.Current.Request.UrlReferrer;

                operation = "";
                selectdValueCopy = "";
                byRegionCopy = "";
                byApplicationCopy = "";
                //actionName1 = actionName;

                misReportsViewModel.GetUserDetails();
                misReportsViewModel.GetUserActions();
                misReportsViewModel.GetActions();

                misReportsViewModel.GetApplicationRegionDetails();
                misReportsViewModel.GetAllDetails();

                //var Actionlist = new SelectList(new[]
                //{
                //    new { ID = "1", Name = "Active" },
                //    new { ID = "0", Name = "Inactive" },
                //},
                //    "ID", "Name", 1);

                //ViewData["list"] = Actionlist;

                //User Management

                misreportsList = ((IEnumerable<MISReports>)from u in misReportsViewModel.lstUserData
                                                           join e in misReportsViewModel.lstUserData on u.CreatorID equals e.UserID
                                                           join g in misReportsViewModel.lstUserData on u.AuthID equals g.UserID
                                                           where u.Active == "1"
                                                           select new MISReports
                                                           {
                                                               CreatorName = e.UserName,
                                                               AuthName = g.UserName,
                                                               UserName = u.UserName,
                                                               CreateDate = u.CreatedDate,
                                                               Status = Convert.ToBoolean(Convert.ToInt32(u.Active))
                                                           }).ToList();

                dt = usermngdata.ToDataTable(misreportsList.Select(x => new usermngdata
                {
                    UserName = x.UserName,
                    AuthName = x.AuthName,
                    CreatorName = x.CreatorName,
                    CreateDate = string.Format("{0:dd/MM/yyyy}", x.CreateDate),
                    Status = x.Status.ToString() == "True" ? "Active" : "InActive",
                    ActionName = "Add"
                }).ToList());

                ViewData["misReportsViewModel2"] = misreportsList;

                //Client Information
                misreportsList1 = ((IEnumerable<MISReports>)from g in misReportsViewModel.lstClients
                                                            join h in misReportsViewModel.lstProjects on g.ClientID equals h.ClientId
                                                            join i in misReportsViewModel.lstApplication on h.ProjectID equals i.ProjectId
                                                            join j in misReportsViewModel.lstAppVersion on i.AppVersion equals j.Id
                                                            join bp in misReportsViewModel.BankTypeList on i.BankType equals bp.Value
                                                            join k in misReportsViewModel.lstRegion on h.RegionId equals k.Id
                                                            select new MISReports
                                                            {
                                                                ClientName = g.ClientName,
                                                                projectName = h.ProjectName,
                                                                ApplicationName = i.ApplicationName,
                                                                appVersion = j.AppVersion,
                                                                BankTypeName = bp.Key,
                                                                RegionName = k.Region
                                                            }).ToList();

                dt1 = usermngdata.ToDataTable(misreportsList1.Select(x => new clientInfo
                {
                    ClientName = x.ClientName,
                    ProjectName = x.projectName,
                    ApplicationName = x.ApplicationName,
                    AppVersion = x.appVersion,
                    BankTypeName = x.BankTypeName,
                    RegionName = x.RegionName
                }).ToList());

                ViewData["misReportsViewModel1"] = misreportsList1;

                misReportsViewModel.RegionList = misReportsViewModel.lstRegion.Select(m => m.Region).Distinct().Select(i => new SelectListItem()
                {
                    Text = i.ToString(),
                    Value = i
                }).ToList();

                ViewBag.applicationids = misReportsViewModel.lstApplication.Where(x => x.ApplicationID == 0).Select(m => m.ApplicationName).Distinct().Select(i => new SelectListItem()
                {
                    Text = i.ToString(),
                    Value = i
                });


                return View(misReportsViewModel);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public JsonResult GetApplicationList(string regionName)
        {
            var applicationids = (from app in misReportsViewModel.lstApplication
                                  join proj in misReportsViewModel.lstProjects on app.ProjectId equals proj.ProjectID
                                  join reg in misReportsViewModel.lstRegion on proj.RegionId equals reg.Id
                                  where app.ApplicationName != null
                                  select new
                                  {
                                      ApplicationName = app.ApplicationName,
                                      ApplicationId = app.ApplicationID,
                                      RegionName = reg.Region,
                                      RegionId = reg.Id
                                  }).Where(x => x.RegionName == regionName).ToList().Select(m => m.ApplicationName).Distinct().Select(i => new SelectListItem()
                                  {
                                      Text = i.ToString(),
                                      Value = misReportsViewModel.lstApplication.Where(x => x.ApplicationName == i.ToString()).First().ApplicationID.ToString()
                                  });

            return Json(applicationids, JsonRequestBehavior.AllowGet);

        }

        public void ExportToExcelUserManagement1(string selectdValue)
        {

            if (operation == "Modify" || operation == "Delete" || operation == "Add")
            {
                dt = usermngdata.ToDataTable(res);
                ExcelFormatPreparation.FormExceldata(dt);

            }
            else
            {
                ExcelFormatPreparation.FormExceldata(dt);
            }

        }

        public void ExportToExcelUserManagement2(string byRegion, string byApplication)
        {

            if ((byRegion == "--By Region--" && byApplication == "--By Application--") || (byRegion == "" && byApplication == "") || (byRegion != "--By Region--" && byApplication == "--By Application--"))
            {

                ExcelFormatPreparation.FormExceldata(dt1);
            }
            else
            {
                dt1 = usermngdata.ToDataTable(result);
                ExcelFormatPreparation.FormExceldata(dt1);

            }
        }

        public class ExcelFormatPreparation
        {
            public static void FormExceldata(DataTable dt)
            {

                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.ClearContent();
                System.Web.HttpContext.Current.Response.ClearHeaders();
                System.Web.HttpContext.Current.Response.Buffer = true;
                // System.Web.HttpContext.Current.Response.ContentType = "application/ms-excel";
                System.Web.HttpContext.Current.Response.ContentType = "application / vnd.openxmlformats - officedocument.spreadsheetml.sheet";

                System.Web.HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
                string FileName = "MISReports.xls";
                System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);

                System.Web.HttpContext.Current.Response.Charset = "utf-8";
                System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
                //sets font
                System.Web.HttpContext.Current.Response.Write("<font style='font-size:15pt; font-family:Calibri;'>");
                System.Web.HttpContext.Current.Response.Write("<BR><BR><BR>");
                //if (selectedItem.ToLower().Trim() == "User Management".ToLower().Trim())
                //{
                //    System.Web.HttpContext.Current.Response.Write("User Management");
                //}
                //else
                //{
                //    System.Web.HttpContext.Current.Response.Write("Client Information");
                //}
                //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
                System.Web.HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
                  "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
                  "style='font-size:15pt; font-family:Calibri; background:white; text-align='center''> <TR>");

                //if (selectedItem.ToLower().Trim() == "User Management".ToLower().Trim())
                //{
                //    System.Web.HttpContext.Current.Response.Write("<TR> User Management </TR>");
                //}
                //else
                //{
                //    System.Web.HttpContext.Current.Response.Write("<TR> Client Information </TR>");
                //}
                //System.Web.HttpContext.Current.Response.Write("<TR>");
                //am getting my grid's column headers
                // int columnscount = dt.Columns.Count;

                for (int j = 0; j < dt.Columns.Count; j++)
                {   //write in new column
                    if (operation == "Add")
                    {
                        System.Web.HttpContext.Current.Response.Write("<Td>");
                        //Get column headers and make it as bold in excel columns
                        System.Web.HttpContext.Current.Response.Write("<B>");
                        System.Web.HttpContext.Current.Response.Write(dt.Columns[j].ColumnName.ToString());
                        System.Web.HttpContext.Current.Response.Write("</B>");
                        System.Web.HttpContext.Current.Response.Write("</Td>");
                    }
                    else if (operation == "Modify" || operation == "Delete")
                    {
                        if (dt.Columns[j].ColumnName != "Status")
                        {
                            System.Web.HttpContext.Current.Response.Write("<Td>");
                            //Get column headers  and make it as bold in excel columns
                            System.Web.HttpContext.Current.Response.Write("<B>");
                            System.Web.HttpContext.Current.Response.Write(dt.Columns[j].ColumnName.ToString());
                            System.Web.HttpContext.Current.Response.Write("</B>");
                            System.Web.HttpContext.Current.Response.Write("</Td>");
                        }

                        else
                        {

                            dt.Columns.Remove("Status");

                        }
                    }
                    else
                    {
                        System.Web.HttpContext.Current.Response.Write("<Td>");
                        //Get column headers and make it as bold in excel columns
                        System.Web.HttpContext.Current.Response.Write("<B>");
                        System.Web.HttpContext.Current.Response.Write(dt.Columns[j].ColumnName.ToString());
                        System.Web.HttpContext.Current.Response.Write("</B>");
                        System.Web.HttpContext.Current.Response.Write("</Td>");
                    }
                }
                System.Web.HttpContext.Current.Response.Write("</TR>");
                foreach (DataRow row in dt.Rows)
                {//write in new row
                    System.Web.HttpContext.Current.Response.Write("<TR>");
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        System.Web.HttpContext.Current.Response.Write("<Td>");
                        System.Web.HttpContext.Current.Response.Write(row[i].ToString());
                        System.Web.HttpContext.Current.Response.Write("</Td>");
                    }

                    System.Web.HttpContext.Current.Response.Write("</TR>");
                }
                System.Web.HttpContext.Current.Response.Write("</Table>");
                System.Web.HttpContext.Current.Response.Write("</font>");
                System.Web.HttpContext.Current.Response.Flush();
                System.Web.HttpContext.Current.Response.End();
            }
        }

        public class clientInfo
        {
            public string ClientName { get; set; }
            public string ProjectName { get; set; }
            public string ApplicationName { get; set; }
            public string AppVersion { get; set; }
            public string BankTypeName { get; set; }
            public string RegionName { get; set; }
        }

        public class usermngdata
        {
            //public int CreatorId { set; get; }
            public string UserName { set; get; }

            public string CreatorName { set; get; }
            //public int AuthId { set; get; }

            public string AuthName { set; get; }
            //public int UserId { set; get; }

            public string ActionName { get; set; }

            public string CreateDate { set; get; }
            //public string ArchiveCreatedDate { get; set; }

            public string Status { set; get; }

            public static DataTable ToDataTable<T>(List<T> items)
            {
                DataTable dataTable = new DataTable(typeof(T).Name);
                //Get all the properties
                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in Props)
                {
                    //Setting column names as Property names
                    string name = string.Empty;
                    name = CreateHeaderCaption(prop.Name);
                    dataTable.Columns.Add(name);
                }

                foreach (T item in items)
                {
                    var values = new object[Props.Length];
                    for (int i = 0; i < Props.Length; i++)
                    {
                        //inserting property values to datatable rows
                        values[i] = Props[i].GetValue(item, null);
                    }

                    dataTable.Rows.Add(values);

                }
                //put a breakpoint here and check datatable
                return dataTable;
            }
        }

        public static string CreateHeaderCaption(string propName)
        {
            string a = "Name";
            string b = "Date";
            string newValue = "";
            if (propName.Contains(a))
            {
                int length = a.Length + 1;
                for (int i = 0; i <= propName.Length - length; i++)
                {
                    newValue += propName[i].ToString();
                }
                newValue = newValue + " " + a;
            }
            else if (propName.Contains(b))
            {
                int length = b.Length + 1;
                for (int i = 0; i <= propName.Length - length; i++)
                {
                    newValue += propName[i].ToString();
                }
                newValue = newValue + " " + b;
            }
            else
            {
                return propName;
            }

            return newValue; ;
        }

        public enum Actions
        {
            Add = 1,
            Modify = 2,
            Delete = 3
        }

        public JsonResult ShowData(string fromDate, string toDate, int actions, int status = 1)
        {
            try
            {

                MISReports misReportsVM = new MISReports();
                misReportsVM.GetUserDetails();
                misReportsVM.GetUserActions();
                misReportsVM.GetActions();
                Actions action = new Actions();
                if (Enum.IsDefined(typeof(Actions), actions))
                {
                    action = (Actions)actions;
                }
                operation = action.ToString();
                if (actions == Convert.ToInt32(Actions.Modify) || actions == Convert.ToInt32(Actions.Delete))
                {
                    res = (from uaa in misReportsVM.lstUserActions
                           join ua in misReportsVM.lstUserData on uaa.AuthID equals ua.UserID
                           join uc in misReportsVM.lstUserData on uaa.CreatorID equals uc.UserID
                           where uaa.CreatedDate >= Convert.ToDateTime(fromDate) && uaa.CreatedDate <= Convert.ToDateTime(toDate) && uaa.ActionID == actions
                           select new usermngdata
                           {
                               UserName = uaa.UserName,
                               CreatorName = uc.UserName,
                               AuthName = ua.UserName,
                               CreateDate = string.Format("{0:dd/MM/yyyy}", uaa.CreatedDate),
                               ActionName = uaa.ActionID == 2 ? "Modify" : "Delete"
                           }).ToList();

                    return Json(res, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    string statusflag = status == 1 ? "1" : "0";
                    res = (from a in misReportsVM.lstUserData
                           join ua in misReportsVM.lstUserData on a.AuthID equals ua.UserID
                           join u in misReportsVM.lstUserData on a.CreatorID equals u.UserID
                           where u.CreatedDate >= Convert.ToDateTime(fromDate) && u.CreatedDate <= Convert.ToDateTime(toDate) && a.Active == statusflag
                           select new usermngdata
                           {
                               CreatorName = u.UserName,
                               AuthName = ua.UserName,
                               UserName = u.UserName,
                               CreateDate = string.Format("{0:dd/MM/yyyy}", u.CreatedDate),
                               Status = a.Active == "1" ? "Active" : "InActive",
                               ActionName = "Add"
                           }).ToList();
                    return Json(res, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public JsonResult ChangeData(string regionId, string applicationId)
        {
            try
            {

                MISReports misReportsViewModel = new MISReports();
                misReportsViewModel.GetApplicationRegionDetails();
                misReportsViewModel.GetAllDetails();

                result = (from g in misReportsViewModel.lstClients
                          join h in misReportsViewModel.lstProjects on g.ClientID equals h.ClientId
                          join i in misReportsViewModel.lstApplication on h.ProjectID equals i.ProjectId
                          join j in misReportsViewModel.lstAppVersion on i.AppVersion equals j.Id
                          join bp in misReportsViewModel.BankTypeList on i.BankType equals bp.Value
                          join k in misReportsViewModel.lstRegion on h.RegionId equals k.Id
                          where i.ApplicationName == applicationId && k.Region == regionId
                          select new clientInfo
                          {
                              ClientName = g.ClientName,
                              ProjectName = h.ProjectName,
                              ApplicationName = i.ApplicationName,
                              AppVersion = j.AppVersion,
                              BankTypeName = bp.Key,
                              RegionName = k.Region
                          }).ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}