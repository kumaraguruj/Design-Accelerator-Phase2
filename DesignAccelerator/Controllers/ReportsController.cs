using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DesignAccelerator.Models.ViewModel;
using System.Data.Entity.Infrastructure;

namespace DesignAccelerator.Controllers
{
    public class ReportsController : Controller
    {
        ErrorLogViewModel errorlogviewmodel;
        // GET: Reports
        //[HttpGet]
        [NoDirectAccess]
        public ActionResult Index(int? id)
        {
            try
            {


                if (id == null)
                    id = (int)TempData["daId"];

                ReportsViewModel reportsViewModel = new ReportsViewModel();

                reportsViewModel.lstSource = reportsViewModel.GetSourcesList((int)id);
                reportsViewModel.lstHighLevelTxns = reportsViewModel.GetTransactionsList((int)id).lstTransactions;
                reportsViewModel.lstPeriodType = reportsViewModel.GetPeriodTypeList((int)id);
                //reportsViewModel.GetPeriodTypeList((int)id);
                reportsViewModel.lstReports = reportsViewModel.getReportsFrmDB((int)id); //(1);

                //reportsViewModel.GetSourcesList((int)id);

                CommonFunctions comfuns = new CommonFunctions();

                var da = comfuns.FindDA((int)id);
                reportsViewModel.daId = (int)id;
                reportsViewModel.ModuleId = da.ModuleId;
                reportsViewModel.daName = da.DAName;
                TempData["daId"] = reportsViewModel.daId;

                int clientId;
                int projectId;
                int applicationId;

                string projectName;
                string appName;
                string modName;

                comfuns.GetModuleName(da.ModuleId, out applicationId, out modName);
                reportsViewModel.ApplicationID = applicationId;
                reportsViewModel.ModuleName = modName;

                comfuns.GetApplicationName(applicationId, out projectId, out appName);
                reportsViewModel.ProjectID = projectId;
                reportsViewModel.ApplicationName = appName;

                comfuns.GetProjectName(projectId, out clientId, out projectName);
                reportsViewModel.ClientID = clientId;
                reportsViewModel.ProjectName = projectName;

                reportsViewModel.ClientName = comfuns.GetClientName(clientId);

                return View(reportsViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult SaveData(IList<ReportsViewModel> reportsViewModelList)
        {
            try
            {


                int index = 0;
                String result = String.Empty;
                int daId = reportsViewModelList.First().daId; //1;

                TempData["daId"] = daId;
                ReportsViewModel reportsdata = new ReportsViewModel();
                index = reportsdata.SaveReports(reportsViewModelList, daId);
                if (index > 0)
                {
                    result = "1";
                }
                else
                    result = "0";

                // ModelState.Clear();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        public ActionResult DeleteReport(string postData, int DaId)
        {
            try
            {


                ReportsMappingViewModel transactionAttribute = new ReportsMappingViewModel();
                transactionAttribute.DeleteReports(postData, DaId);
                return Json(postData, JsonRequestBehavior.AllowGet);
            }
            catch (DbUpdateException exception)
            {
                //Log Exception
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(exception);

                //Check for Referential Integrity
                if (((System.Data.SqlClient.SqlException)exception.InnerException.InnerException).Number == 547)
                {
                    return View("Error_ReferentialIntegrity");
                }

                return View("Error");
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }
    }
}