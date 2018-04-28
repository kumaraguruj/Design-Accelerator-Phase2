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
    public class ApplicationController : Controller
    {

        #region PublicProperties
        ErrorLogViewModel errorlogviewmodel;
        #endregion

        // GET: Application
        [NoDirectAccess]
        public ActionResult Index(int? id)
        {
            try
            {
                ApplicationViewModel applicationViewModel = new ApplicationViewModel();
                if (id == null)
                    id = (int)TempData["ProjectId"];
                applicationViewModel.GetApplicationDetails((int)id);
                applicationViewModel.GetScreenAccessRights("Application Details");
                applicationViewModel.ProjectID = (int)id;
                TempData["ProjectId"] = applicationViewModel.ProjectID;

                ViewData["Applicationviewmodel1"] = (IEnumerable<ApplicationViewModel>)from u in applicationViewModel.lstApplication
                                                                                       join b in applicationViewModel.lstAppVersion on u.AppVersion equals b.Id
                                                                                       join bp in applicationViewModel.BankTypeList on u.BankType equals bp.Value
                                                                                       select new ApplicationViewModel { ApplicationID = u.ApplicationID, ApplicationName = u.ApplicationName, AppVersion = b.AppVersion, BankTypeName = bp.Key };
                
                int clientId;
                string projectName;

                CommonFunctions comfuns = new CommonFunctions();

                comfuns.GetProjectName((int)id, out clientId, out projectName);
                applicationViewModel.ClientId = clientId;
                applicationViewModel.ProjectName = projectName;

                applicationViewModel.ClientName = comfuns.GetClientName(clientId);

                return View(applicationViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }


        [HttpPost]
        public ActionResult Index(ApplicationViewModel applicationViewModel)
        {
            try
            {
                CommonFunctions comfuns = new CommonFunctions();

                if (ModelState.IsValid)
                {
                    applicationViewModel.ApplicationName = applicationViewModel.ApplicationName.Trim();
                    bool isDuplicate = false;
                    isDuplicate = applicationViewModel.CheckDuplicate(applicationViewModel);

                    if(isDuplicate)
                    {
                        applicationViewModel.GetApplicationDetails(applicationViewModel.ProjectID);
                        applicationViewModel.GetScreenAccessRights("Application Details");
                        ViewBag.Message = "Application Already Exists";
                        ViewData["Applicationviewmodel1"] = (IEnumerable<ApplicationViewModel>)from u in applicationViewModel.lstApplication
                                                                                               join b in applicationViewModel.lstAppVersion on u.AppVersion equals b.Id
                                                                                               join bp in applicationViewModel.BankTypeList on u.BankType equals bp.Value
                                                                                               select new ApplicationViewModel { ApplicationID = u.ApplicationID, ApplicationName = u.ApplicationName, AppVersion = b.AppVersion, BankTypeName = bp.Key };
                    

                        comfuns.GetProjectNameForDuplicateCheck(applicationViewModel.ProjectID, applicationViewModel.ClientId, applicationViewModel.ProjectName);
                       

                        applicationViewModel.ClientName = comfuns.GetClientName(applicationViewModel.ClientId);

                        return View(applicationViewModel);

                    }
                    else
                    {
                        applicationViewModel.AddApplication(applicationViewModel);
                        TempData["ProjectId"] = applicationViewModel.ProjectID;
                        applicationViewModel.GetApplicationDetails(applicationViewModel.ProjectID);
                        applicationViewModel.GetScreenAccessRights("Application Details");
                        ViewBag.Message = "New Application Added Successfully";
                        ViewData["Applicationviewmodel1"] = (IEnumerable<ApplicationViewModel>)from u in applicationViewModel.lstApplication
                                                                                               join b in applicationViewModel.lstAppVersion on u.AppVersion equals b.Id
                                                                                               join bp in applicationViewModel.BankTypeList on u.BankType equals bp.Value
                                                                                               select new ApplicationViewModel { ApplicationID = u.ApplicationID, ApplicationName = u.ApplicationName, AppVersion = b.AppVersion, BankTypeName = bp.Key };
                        
                        comfuns.GetProjectNameForDuplicateCheck(applicationViewModel.ProjectID, applicationViewModel.ClientId, applicationViewModel.ProjectName);


                        applicationViewModel.ClientName = comfuns.GetClientName(applicationViewModel.ClientId);



                        return View(applicationViewModel);
                    }
                  
                }
                applicationViewModel.GetApplicationDetails(applicationViewModel.ProjectID);
                applicationViewModel.GetScreenAccessRights("Application Details");

                ViewData["Applicationviewmodel1"] = (IEnumerable<ApplicationViewModel>)from u in applicationViewModel.lstApplication
                                                                                       join b in applicationViewModel.lstAppVersion on u.AppVersion equals b.Id
                                                                                       join bp in applicationViewModel.BankTypeList on u.BankType equals bp.Value
                                                                                       select new ApplicationViewModel { ApplicationID = u.ApplicationID, ApplicationName = u.ApplicationName, AppVersion = b.AppVersion, BankTypeName = bp.Key };
             

                comfuns.GetProjectNameForDuplicateCheck(applicationViewModel.ProjectID, applicationViewModel.ClientId, applicationViewModel.ProjectName);


                applicationViewModel.ClientName = comfuns.GetClientName(applicationViewModel.ClientId);

                int clientId;
                string projectName;


                comfuns.GetProjectName(applicationViewModel.ProjectID, out clientId, out projectName);
                applicationViewModel.ClientId = clientId;
                applicationViewModel.ProjectName = projectName;

                applicationViewModel.ClientName = comfuns.GetClientName(clientId);
                return View(applicationViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        // GET: Application/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ApplicationViewModel applicationViewModel = new ApplicationViewModel();
                applicationViewModel = applicationViewModel.FindApplication(id);
                TempData["ProjectId"] = applicationViewModel.ProjectID;


                if (applicationViewModel.ApplicationID == 0)
                {
                    return HttpNotFound();
                }
                return View(applicationViewModel);
            }
            catch(Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }

        }
            
        // POST: Application/Edit/5
        [HttpPost]
        public ActionResult Edit(ApplicationViewModel applicationViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isduplicate = false;
                    applicationViewModel.ApplicationName = applicationViewModel.ApplicationName.Trim();
                    isduplicate = applicationViewModel.CheckDuplicate(applicationViewModel);
                    if (isduplicate)
                    {
                        ModelState.AddModelError("ApplicationName", "Application already exists");
                        applicationViewModel.GetVersions(applicationViewModel);
                        applicationViewModel.BankTypeList = applicationViewModel.GetEnumList<ApplicationViewModel.BankTypes>();
                        return View("Edit", applicationViewModel);
                    } 

                    applicationViewModel.UpdateApplication(applicationViewModel);
                    return RedirectToAction("Index", "Application");
                }
                applicationViewModel.GetVersions(applicationViewModel);
                applicationViewModel.BankTypeList = applicationViewModel.GetEnumList<ApplicationViewModel.BankTypes>();
                return View(applicationViewModel);
            }
            catch(Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        // GET: Application/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ApplicationViewModel applicationViewModel = new ApplicationViewModel();
                applicationViewModel = applicationViewModel.FindApplication(id);
                TempData["ProjectId"] = applicationViewModel.ProjectID;


                if (applicationViewModel.ApplicationID == 0)
                {
                    return HttpNotFound();
                }
                return View(applicationViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        // POST: Application/Delete/5
        [HttpPost]
        public ActionResult Delete(ApplicationViewModel applicationViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    applicationViewModel.DeleteApplication(applicationViewModel);
                    return RedirectToAction("Index","Application");
                }

                return RedirectToAction("Index", "Application");
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
