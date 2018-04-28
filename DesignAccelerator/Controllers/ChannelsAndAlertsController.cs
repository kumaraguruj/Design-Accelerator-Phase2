using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DesignAccelerator.Models.ViewModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Data.Entity.Infrastructure;

namespace DesignAccelerator.Controllers
{
    public class ChannelsAndAlertsController : Controller
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
                    id = (int)TempData["daId"]; // id = 1;

                ChannelsAndAlertsViewModel channelandalertsviewmodel = new ChannelsAndAlertsViewModel();

                channelandalertsviewmodel.lstchannelsalerts = channelandalertsviewmodel.GetChannelsAlerts((int)id);
                channelandalertsviewmodel.GetAllTransactions(id);
                channelandalertsviewmodel.GetAllAttributes(id);

                channelandalertsviewmodel.GetSource(id);
                channelandalertsviewmodel.GetDestination(id);
                channelandalertsviewmodel.GetModeType(id);
                channelandalertsviewmodel.GetDistribution(id);
                channelandalertsviewmodel.GetFrequency(id);

                channelandalertsviewmodel.GetAllAttributes(id);

                CommonFunctions comfuns = new CommonFunctions();

                var da = comfuns.FindDA((int)id);
                channelandalertsviewmodel.daID = (int)id;
                channelandalertsviewmodel.ModuleId = da.ModuleId;
                channelandalertsviewmodel.daName = da.DAName;
                TempData["daId"] = channelandalertsviewmodel.daID;

                int clientId;
                int projectId;
                int applicationId;

                string projectName;
                string appName;
                string modName;

                comfuns.GetModuleName(da.ModuleId, out applicationId, out modName);
                channelandalertsviewmodel.ApplicationID = applicationId;
                channelandalertsviewmodel.ModuleName = modName;

                comfuns.GetApplicationName(applicationId, out projectId, out appName);
                channelandalertsviewmodel.ProjectID = projectId;
                channelandalertsviewmodel.ApplicationName = appName;

                comfuns.GetProjectName(projectId, out clientId, out projectName);
                channelandalertsviewmodel.ClientID = clientId;
                channelandalertsviewmodel.ProjectName = projectName;

                channelandalertsviewmodel.ClientName = comfuns.GetClientName(clientId);

                return View(channelandalertsviewmodel);
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


                ChannelsAndAlertsViewModel channelandalertsviewmodel = new ChannelsAndAlertsViewModel();
                channelandalertsviewmodel.GetAllAttributeValues(input);
                var result = from r in channelandalertsviewmodel.lstAttributeValues
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
        public ActionResult SaveData(IList<ChannelsAndAlertsViewModel> transactionAttributes)
        {
            try
            {


                int index = 0;
                String result = String.Empty;
                int daId = transactionAttributes.First().daID;
                TempData["daId"] = daId;
                ChannelsAndAlertsViewModel transactionAttribute = new ChannelsAndAlertsViewModel();
                index = transactionAttribute.SaveChannelAlertsData(transactionAttributes, daId);
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


        //Delete logic---Mohseen
        public ActionResult DeleteRowChannelAlert(int postData)
        {
            try
            {
                ChannelsAndAlertsViewModel channelAlertVM = new ChannelsAndAlertsViewModel();
                channelAlertVM.DeleteChannelAttrMapId(postData);

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