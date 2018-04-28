using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DA.BusinessLayer;
using DesignAccelerator.Models.ViewModel;
using System.Net;
using DA.DomainModel;
using System.Data.Entity.Infrastructure;

namespace DesignAccelerator.Controllers
{
    public class DesignAcceleratorController : Controller
    {

        #region PublicProperties
        ErrorLogViewModel errorlogviewmodel;
        #endregion

        CommonFunctions comfuns = new CommonFunctions();
        // GET: DA
        [NoDirectAccess]
        public ActionResult Index(int? id)
        {
            try
            {
                DAViewModel daViewModel = new DAViewModel();
                if (id == null)
                    id = (int)TempData["ModuleId"];
                daViewModel.GetDADetails((int)id);
                daViewModel.GetScreenAccessRights("Design Accelerator");
                daViewModel.ModuleId = (int)id;
                TempData["ModuleId"] = daViewModel.ModuleId;

                int projectId;
                int clientId;
                string projectName;
                string appName;
                int applicationId;
                string modName;



                comfuns.GetModuleName((int)id, out applicationId, out modName);
                daViewModel.ApplicationID = applicationId;
                daViewModel.ModuleName = modName;

                comfuns.GetApplicationName(applicationId, out projectId, out appName);
                daViewModel.ProjectId = projectId;
                daViewModel.ApplicationName = appName;

                comfuns.GetProjectName(projectId, out clientId, out projectName);
                daViewModel.ClientId = clientId;
                daViewModel.ProjectName = projectName;

                daViewModel.ClientName = comfuns.GetClientName(clientId);

                return View(daViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }


        [HttpPost]
        public ActionResult Index(DAViewModel DAViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DAViewModel.DAName = DAViewModel.DAName.Trim();
                    DAViewModel.AddDA(DAViewModel);
                    TempData["ModuleId"] = DAViewModel.ModuleId;
                    return RedirectToAction("Index", "DesignAccelerator");
                }
                int projectId;
                int clientId;
                string projectName;
                string appName;
                int applicationId;
                string modName;


                comfuns.GetModuleName((int)DAViewModel.ModuleId, out applicationId, out modName);
                DAViewModel.ApplicationID = applicationId;
                DAViewModel.ModuleName = modName;

                comfuns.GetApplicationName(applicationId, out projectId, out appName);
                DAViewModel.ProjectId = projectId;
                DAViewModel.ApplicationName = appName;

                comfuns.GetProjectName(projectId, out clientId, out projectName);
                DAViewModel.ClientId = clientId;
                DAViewModel.ProjectName = projectName;

                DAViewModel.ClientName = comfuns.GetClientName(clientId);

                DAViewModel.GetDADetails(DAViewModel.ApplicationID);
                DAViewModel.GetScreenAccessRights("Design Accelerator");
                return View(DAViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        // GET: DA/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                DAViewModel DAViewModel = new DAViewModel();
                var DA = DAViewModel.FindDA(id);
                TempData["ModuleId"] = DA.ModuleId;
                if (DA.DAID == 0)
                {
                    return HttpNotFound();
                }

                return View(DA);

            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }

        }

        // POST: DA/Edit/5
        [HttpPost]
        public ActionResult Edit(DAViewModel daViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    daViewModel.DAName = daViewModel.DAName.Trim();
                    bool isduplicate = false;
                    isduplicate = daViewModel.CheckDuplicate(daViewModel);
                    if (isduplicate)
                    {
                        ModelState.AddModelError("DAName", "DA already exists");
                        return View("Edit");
                    }

                    daViewModel.UpdateDA(daViewModel);
                    return RedirectToAction("Index", "DesignAccelerator");
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

        // GET: DA/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                DAViewModel DAViewModel = new DAViewModel();
                var DA = DAViewModel.FindDA(id);
                TempData["ModuleId"] = DA.ModuleId;
                if (DA.DAID == 0)
                {
                    return HttpNotFound();
                }
                return View(DA);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        // POST: DA/Delete/5
        [HttpPost]
        public ActionResult Delete(DAViewModel DAViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DAViewModel.DeleteDA(DAViewModel);
                    return RedirectToAction("Index", "DesignAccelerator");
                }

                return RedirectToAction("Index", "DesignAccelerator");
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
