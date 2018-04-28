using DesignAccelerator.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DesignAccelerator.Controllers
{
    public class TransactionAttributeController : Controller
    {
        ErrorLogViewModel errorlogviewmodel;
        // GET: TransactionAttribute
        [HttpGet]
        [NoDirectAccess]
        public ActionResult Index(int? id)
        {
            try
            {


                TransactionAttributes transactionAttributes = new TransactionAttributes();


                if (id == null)
                    id = (int)TempData["daId"];
                IList<TransactionAttributes> transactionAttributesList = transactionAttributes.GetTransactionAttributes((int)id);

                CommonFunctions comfuns = new CommonFunctions();

                var da = comfuns.FindDA((int)id);
                transactionAttributes.daId = (int)id;
                transactionAttributes.ModuleId = da.ModuleId;
                transactionAttributes.daName = da.DAName;
                TempData["daId"] = transactionAttributes.daId;

                int clientId;
                int projectId;
                int applicationId;

                string projectName;
                string appName;
                string modName;

                comfuns.GetModuleName(transactionAttributes.ModuleId, out applicationId, out modName);
                transactionAttributes.ApplicationID = applicationId;
                transactionAttributes.ModuleName = modName;

                comfuns.GetApplicationName(applicationId, out projectId, out appName);
                transactionAttributes.ProjectID = projectId;
                transactionAttributes.ApplicationName = appName;

                comfuns.GetProjectName(projectId, out clientId, out projectName);
                transactionAttributes.ClientID = clientId;
                transactionAttributes.ProjectName = projectName;

                transactionAttributes.ClientName = comfuns.GetClientName(clientId);
                ViewData["ClientId"] = clientId;
                ViewData["ClientName"] = transactionAttributes.ClientName;
                ViewData["ProjectName"] = transactionAttributes.ProjectName;
                ViewData["ProjectID"] = transactionAttributes.ProjectID;
                ViewData["ApplicationName"] = transactionAttributes.ApplicationName;
                ViewData["ApplicationID"] = transactionAttributes.ApplicationID;
                ViewData["ModuleName"] = transactionAttributes.ModuleName;
                ViewData["ModuleId"] = transactionAttributes.ModuleId;
                ViewData["daName"] = transactionAttributes.daName;
                ViewData["daId"] = transactionAttributes.daId;

                AttributeListViewModel attributeListViewModel = new AttributeListViewModel();
                ViewData["AttributeList"] = attributeListViewModel.GetAttributeList(transactionAttributes.daId);
                return View(transactionAttributesList);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult SaveData(IList<TransactionAttributes> transactionAttributes)
        {
            try
            {


                int index = 0;
                String result = String.Empty;
                int daId = transactionAttributes.First().daId;

                TempData["daId"] = daId;
                TransactionAttributes transactionAttribute = new TransactionAttributes();
                index = transactionAttribute.SaveData(transactionAttributes, daId);
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
    }
}