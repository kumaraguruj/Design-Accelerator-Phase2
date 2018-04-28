using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DesignAccelerator.Models.ViewModel;
using System.Net;

namespace DesignAccelerator.Controllers
{
    public class SMDController : Controller
    {
        ErrorLogViewModel errorlogviewmodel;
        // GET: SMD
        [NoDirectAccess]
        public ActionResult Index(int? id)
        {
            try
            {


                if (id == null)
                    id = (int)TempData["daId"];

                BusinessRulesViewModel buzruleVM = new BusinessRulesViewModel();

                CommonFunctions comfuns = new CommonFunctions();

                var da = comfuns.FindDA((int)id);
                buzruleVM.daID = (int)id;
                buzruleVM.ModuleId = da.ModuleId;
                buzruleVM.daName = da.DAName;
                TempData["daId"] = buzruleVM.daID;

                int clientId;
                int projectId;
                int applicationId;

                string projectName;
                string appName;
                string modName;

                comfuns.GetModuleName(da.ModuleId, out applicationId, out modName);
                buzruleVM.ApplicationID = applicationId;
                buzruleVM.ModuleName = modName;

                comfuns.GetApplicationName(applicationId, out projectId, out appName);
                buzruleVM.ProjectID = projectId;
                buzruleVM.ApplicationName = appName;

                comfuns.GetProjectName(projectId, out clientId, out projectName);
                buzruleVM.ClientID = clientId;
                buzruleVM.ProjectName = projectName;

                buzruleVM.ClientName = comfuns.GetClientName(clientId);
                
                return View(buzruleVM);
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