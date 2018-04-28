using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DesignAccelerator.Models.ViewModel;
using System.Net;
using System.Data.Entity.Infrastructure;

namespace DesignAccelerator.Controllers
{
    public class ModuleController : Controller
    {

        #region PublicProperties
        ErrorLogViewModel errorlogviewmodel;
        #endregion

        // GET: Module
        //[HttpGet]
        [NoDirectAccess]
        public ActionResult Index(int? id)
        {
            try
            { 
                ModuleViewModel modVM = new ModuleViewModel();
                if (id == null)
                    id = (int)TempData["ApplicationId"];

                modVM.GetModuleDetails((int)id);
                modVM.GetScreenAccessRights("Module Input");
                modVM.ApplicationID = (int)id;
                TempData["ApplicationId"] = modVM.ApplicationID;

                int projectId;
                int clientId;
                string projectName;
                string appName;

                CommonFunctions comfuns = new CommonFunctions();

                comfuns.GetApplicationName((int)id, out projectId, out appName);
                modVM.ProjectID = projectId;
                modVM.ApplicationName = appName;

                comfuns.GetProjectName(projectId, out clientId, out projectName);
                modVM.ClientID = clientId;
                modVM.ProjectName = projectName;

                modVM.ClientName = comfuns.GetClientName(clientId);
                
                return View(modVM);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        //POST: Module Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ModuleViewModel modVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    modVM.ModuleName = modVM.ModuleName.Trim();
                    ModuleViewModel modViewModel = new ModuleViewModel();
                    modViewModel.AddMod(modVM);
                    TempData["ApplicationId"] = modViewModel.ApplicationID;

                    return RedirectToAction("Index", "Module");
                }
               

                TempData["ApplicationId"] = modVM.ApplicationID;

                int projectId;
                int clientId;
                string projectName;
                string appName;

                CommonFunctions comfuns = new CommonFunctions();

                comfuns.GetApplicationName(modVM.ApplicationID, out projectId, out appName);
                modVM.ProjectID = projectId;
                modVM.ApplicationName = appName;

                comfuns.GetProjectName(projectId, out clientId, out projectName);
                modVM.ClientID = clientId;
                modVM.ProjectName = projectName;

                modVM.ClientName = comfuns.GetClientName(clientId);

                modVM.GetModuleDetails(modVM.ApplicationID);
                modVM.GetScreenAccessRights("Module Input");

                return View(modVM);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        //GET: Module Edit
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ModuleViewModel modViewModel = new ModuleViewModel();
                modViewModel = modViewModel.FindMod((int)id);
                TempData["ApplicationId"] = modViewModel.ApplicationID;

                if (modViewModel.ModuleId == 0)
                {
                    return HttpNotFound();
                }
                return View(modViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        //POST: Module Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ModuleViewModel modVM)
        {
            try
            { 
                if (ModelState.IsValid)
                {
                    bool isduplicate = false;
                    modVM.ModuleName = modVM.ModuleName.Trim();
                    isduplicate = modVM.CheckDuplicate(modVM);
                    if (isduplicate)
                    {
                        ModelState.AddModelError("ModuleName", "Module already exists");
                        return View("Edit", modVM);
                    }
                    //ModuleViewModel modViewModel = new ModuleViewModel();
                    modVM.UpdateMod(modVM);

                    return RedirectToAction("Index", "Module");
                }
                return View();
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        //GET: Module/Delete/5
        public ActionResult Delete(int? Id)
        {
            try
            {
                if (Id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ModuleViewModel modViewModel = new ModuleViewModel();
                //var mod = modViewModel.FindMod((int)modId);
                modViewModel = modViewModel.FindMod((int)Id);
                TempData["ApplicationId"] = modViewModel.ApplicationID;

                if (modViewModel.ModuleId == 0)
                {
                    return HttpNotFound();
                }
                return View(modViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ModuleViewModel modVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    modVM.DeleteMod(modVM);
                    return RedirectToAction("Index", "Module");
                }
                return RedirectToAction("Index", "Module");
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