using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using DesignAccelerator.Models.ViewModel;

namespace DesignAccelerator.Controllers
{
    public class DFController : Controller
    {
        ErrorLogViewModel errorlogviewmodel;
        // GET: DF
        [NoDirectAccess]
        public ActionResult Index(int? id)
        {
            try
            {


                if (id == null)
                    id = (int)TempData["daId"];

                InterfaceViewModel interfaceVM = new InterfaceViewModel();

                CommonFunctions comfuns = new CommonFunctions();

                var da = comfuns.FindDA((int)id);
                interfaceVM.daId = (int)id;
                interfaceVM.ModuleId = da.ModuleId;
                interfaceVM.daName = da.DAName;
                TempData["daId"] = interfaceVM.daId;

                int clientId;
                int projectId;
                int applicationId;

                string projectName;
                string appName;
                string modName;

                comfuns.GetModuleName(da.ModuleId, out applicationId, out modName);
                interfaceVM.ApplicationID = applicationId;
                interfaceVM.ModuleName = modName;

                comfuns.GetApplicationName(applicationId, out projectId, out appName);
                interfaceVM.ProjectID = projectId;
                interfaceVM.ApplicationName = appName;

                comfuns.GetProjectName(projectId, out clientId, out projectName);
                interfaceVM.ClientID = clientId;
                interfaceVM.ProjectName = projectName;

                interfaceVM.ClientName = comfuns.GetClientName(clientId);
                

                return View(interfaceVM);
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