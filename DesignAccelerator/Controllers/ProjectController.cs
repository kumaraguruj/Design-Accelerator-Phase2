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
    public class ProjectController : Controller
    {

        ErrorLogViewModel errorlogviewmodel;

        // GET: Project
        [NoDirectAccess]
        public ActionResult Index(int? id)
        {
            try
            {
                ProjectViewModel projectViewModel = new ProjectViewModel();
                if (id == null)
                    id = (int)TempData["ClientId"];
                projectViewModel.GetProjectDetails((int)id);
                projectViewModel.GetScreenAccessRights("Project Details");
                projectViewModel.ClientID = (int)id;

                ViewData["ProjectViewModel1"] = (IEnumerable<ProjectViewModel>)from u in projectViewModel.lstProject
                                                                               join b in projectViewModel.lstRegion on u.RegionId equals b.Id
                                                                               // join bp in applicationViewModel.lstRegion on u.RegionId equals bp.Id
                                                                               select new ProjectViewModel { ProjectID = u.ProjectID, ProjectName = u.ProjectName, Region = b.Region };



                CommonFunctions comfuns = new CommonFunctions();
                projectViewModel.ClientName = comfuns.GetClientName((int)id);

                return View(projectViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }

        }


        [HttpPost]
        public ActionResult Index(ProjectViewModel projectViewModel)
        {
            try
            {
                CommonFunctions comfuns = new CommonFunctions();
                if (ModelState.IsValid)
                {
                    bool isDuplicate = false;
                    projectViewModel.ProjectName = projectViewModel.ProjectName.Trim();
                    isDuplicate = projectViewModel.CheckDuplicate(projectViewModel);
                    if (isDuplicate)
                    {
                        projectViewModel.GetProjectDetails(projectViewModel.ClientID);
                        ViewBag.Message = "Project Already Exists";
                        ViewData["ProjectViewModel1"] = (IEnumerable<ProjectViewModel>)from u in projectViewModel.lstProject
                                                                                       join b in projectViewModel.lstRegion on u.RegionId equals b.Id
                                                                                       // join bp in applicationViewModel.lstRegion on u.RegionId equals bp.Id
                                                                                       select new ProjectViewModel { ProjectID = u.ProjectID, ProjectName = u.ProjectName, Region = b.Region };



                       
                        projectViewModel.ClientName = comfuns.GetClientName((int)projectViewModel.ClientID);
                        projectViewModel.GetScreenAccessRights("Project Details");
                        return View(projectViewModel);
                    }
                    else
                    {
                        projectViewModel.AddProject(projectViewModel);
                        TempData["ClientId"] = projectViewModel.ClientID;
                        projectViewModel.GetProjectDetails(projectViewModel.ClientID);
                        ViewBag.Message = "New Project Added Successfully";
                        ViewData["ProjectViewModel1"] = (IEnumerable<ProjectViewModel>)from u in projectViewModel.lstProject
                                                                                       join b in projectViewModel.lstRegion on u.RegionId equals b.Id
                                                                                       // join bp in applicationViewModel.lstRegion on u.RegionId equals bp.Id
                                                                                       select new ProjectViewModel { ProjectID = u.ProjectID, ProjectName = u.ProjectName, Region = b.Region };



                         
                        projectViewModel.ClientName = comfuns.GetClientName((int)projectViewModel.ClientID);
                        projectViewModel.GetScreenAccessRights("Project Details");
                        return View(projectViewModel);

                    }
                }
                projectViewModel.GetProjectDetails(projectViewModel.ClientID);
                ViewData["ProjectViewModel1"] = (IEnumerable<ProjectViewModel>)from u in projectViewModel.lstProject
                                                                               join b in projectViewModel.lstRegion on u.RegionId equals b.Id
                                                                               // join bp in applicationViewModel.lstRegion on u.RegionId equals bp.Id
                                                                               select new ProjectViewModel { ProjectID = u.ProjectID, ProjectName = u.ProjectName, Region = b.Region };



                
                projectViewModel.ClientName = comfuns.GetClientName((int)projectViewModel.ClientID);
                projectViewModel.GetScreenAccessRights("Project Details");
                return View("Index", projectViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }

        }



        // GET: Project/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ProjectViewModel projectViewModel = new ProjectViewModel();
                projectViewModel = projectViewModel.FindProject(id);
                TempData["ClientId"] = projectViewModel.ClientID;

                if (projectViewModel.ProjectID == 0)
                {
                    return HttpNotFound();
                }
                return View(projectViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        // POST: Project/Edit/5
        [HttpPost]
        public ActionResult Edit(ProjectViewModel projectViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    projectViewModel.ProjectName = projectViewModel.ProjectName.Trim();
                    bool isDuplicate = false;
                    isDuplicate = projectViewModel.CheckDuplicate(projectViewModel);
                    if (isDuplicate)
                    {
                        ModelState.AddModelError("ProjectName", "Project already exists");
                        projectViewModel.GetRegions(projectViewModel);
                        return View("Edit", projectViewModel);
                    }
                    projectViewModel.UpdateProject(projectViewModel);
                    return RedirectToAction("Index", "Project");
                }
                projectViewModel.GetRegions(projectViewModel);
                return View(projectViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        // GET: Project/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ProjectViewModel projectViewModel = new ProjectViewModel();
                projectViewModel = projectViewModel.FindProject(id);
                TempData["ClientId"] = projectViewModel.ClientID;
                if (projectViewModel.ProjectID == 0)
                {
                    return HttpNotFound();
                }
                return View(projectViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        // POST: Project/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ProjectViewModel projectViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    projectViewModel.DeleteProject(projectViewModel);

                    return RedirectToAction("Index", "Project");
                }
                return RedirectToAction("Index", "Project");
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
