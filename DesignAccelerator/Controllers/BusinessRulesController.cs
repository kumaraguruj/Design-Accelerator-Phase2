using DesignAccelerator.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Data.Entity.Infrastructure;

namespace DesignAccelerator.Controllers
{
    public class BusinessRulesController : Controller
    {
        ErrorLogViewModel errorlogviewmodel;
        // GET: BusinessRules
        [HttpGet]
        [NoDirectAccess]
        public ActionResult Index(int? id)
        {
            try
            {


                if (id == null)
                    id = (int)TempData["daId"];

                BusinessRulesViewModel businessrulesviewmodel = new BusinessRulesViewModel();

                businessrulesviewmodel.lstbusinessrules = businessrulesviewmodel.GetBusinessRules((int)id);
                businessrulesviewmodel.GetAllTransactions(id);
                businessrulesviewmodel.GetAllAttributes(id);

                CommonFunctions comfuns = new CommonFunctions();

                var da = comfuns.FindDA((int)id);
                businessrulesviewmodel.daID = (int)id;
                businessrulesviewmodel.ModuleId = da.ModuleId;
                businessrulesviewmodel.daName = da.DAName;
                TempData["daId"] = businessrulesviewmodel.daID;

                int clientId;
                int projectId;
                int applicationId;

                string projectName;
                string appName;
                string modName;

                comfuns.GetModuleName(da.ModuleId, out applicationId, out modName);
                businessrulesviewmodel.ApplicationID = applicationId;
                businessrulesviewmodel.ModuleName = modName;

                comfuns.GetApplicationName(applicationId, out projectId, out appName);
                businessrulesviewmodel.ProjectID = projectId;
                businessrulesviewmodel.ApplicationName = appName;

                comfuns.GetProjectName(projectId, out clientId, out projectName);
                businessrulesviewmodel.ClientID = clientId;
                businessrulesviewmodel.ProjectName = projectName;

                businessrulesviewmodel.ClientName = comfuns.GetClientName(clientId);

                return View(businessrulesviewmodel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetValues(int input)
        {
            try
            {


                BusinessRulesViewModel businessrulesviewmodel = new BusinessRulesViewModel();
                businessrulesviewmodel.GetAllAttributeValues(input);
                var result = from r in businessrulesviewmodel.lstAttributeValues
                             select new { r.AttrValueID, r.AttributeValue };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");

            }
        }

        [HttpPost]
        public ActionResult SaveData(IList<BusinessRulesViewModel> transactionAttributes)
        {
            try
            {


                int index = 0;
                String result = String.Empty;
                int daId = transactionAttributes.First().daID;

                TempData["daId"] = daId;
                BusinessRulesViewModel transactionAttribute = new BusinessRulesViewModel();
                index = transactionAttribute.SaveBusinessRulesData(transactionAttributes, daId);
                if (index > 0)
                {
                    result = "1";
                }
                else
                    result = "0";

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        public ActionResult DeleteRow(int postData)
        {
            try
            {
 
                BusinessRulesViewModel businessRulesVM = new BusinessRulesViewModel();
                businessRulesVM.DeleteBuzrulattrmap(postData);

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