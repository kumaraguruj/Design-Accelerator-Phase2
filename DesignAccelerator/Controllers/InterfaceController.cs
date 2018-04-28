using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DA.BusinessLayer;
using DesignAccelerator.Models.ViewModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Data.Entity.Infrastructure;

namespace DesignAccelerator.Controllers
{
    public class InterfaceController : Controller
    {
        ErrorLogViewModel errorlogviewmodel;
        // GET: Interface
        //[HttpGet]
        [NoDirectAccess]
        public ActionResult Index(int? id)
        {
            try
            {


                InterfaceViewModel interfaceViewModel = new InterfaceViewModel();
                if (id == null)
                    id = (int)TempData["daId"];

                //IList<InterfaceViewModel> interfaceForHeader = 
                interfaceViewModel.lstinterfaceData = interfaceViewModel.GetInterfaceData((int)id);
                interfaceViewModel.GetAllHighLevelTransactions((int)id);
                interfaceViewModel.GetSource(id);
                interfaceViewModel.GetDestination(id);
                interfaceViewModel.GetModeType(id);
                interfaceViewModel.GetAllAttributes(id);

                CommonFunctions comfuns = new CommonFunctions();

                var da = comfuns.FindDA((int)id);
                interfaceViewModel.daId = (int)id;
                interfaceViewModel.ModuleId = da.ModuleId;
                interfaceViewModel.daName = da.DAName;
                TempData["daId"] = interfaceViewModel.daId;

                int clientId;
                int projectId;
                int applicationId;

                string projectName;
                string appName;
                string modName;

                comfuns.GetModuleName(da.ModuleId, out applicationId, out modName);
                interfaceViewModel.ApplicationID = applicationId;
                interfaceViewModel.ModuleName = modName;

                comfuns.GetApplicationName(applicationId, out projectId, out appName);
                interfaceViewModel.ProjectID = projectId;
                interfaceViewModel.ApplicationName = appName;

                comfuns.GetProjectName(projectId, out clientId, out projectName);
                interfaceViewModel.ClientID = clientId;
                interfaceViewModel.ProjectName = projectName;

                interfaceViewModel.ClientName = comfuns.GetClientName(clientId);

                return View(interfaceViewModel);
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


                InterfaceViewModel interfaceviewmodel = new InterfaceViewModel();
                interfaceviewmodel.GetAllAttributeValues(input);
                var result = from r in interfaceviewmodel.lstAttributeValues
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
        public ActionResult SaveData(IList<InterfaceViewModel> interfaceViewModelList)
        {
            try
            {


                int index = 0;
                String result = String.Empty;
                int daId = interfaceViewModelList.First().daId;

                TempData["daId"] = daId;
                InterfaceViewModel interfacedata = new InterfaceViewModel();
                index = interfacedata.SaveData(interfaceViewModelList, daId);
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



        //Delete logic for interface---Mohseen
        public ActionResult DeleteRowInterface(int postData)
        {
            try
            {
                InterfaceViewModel interfaceVM = new InterfaceViewModel();
                interfaceVM.DeleteInterfaceAttr(postData);

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