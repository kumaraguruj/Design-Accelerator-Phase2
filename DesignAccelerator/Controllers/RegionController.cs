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
    public class RegionController : Controller
    {
        ErrorLogViewModel errorlogviewmodel;
        [NoDirectAccess]
        public ActionResult Index()
        {
            try
            {


                string urlPart = @"/Project";
                if (System.Web.HttpContext.Current.Request.UrlReferrer.ToString().IndexOf(urlPart) > 0)
                    Session["PreviousURL"] = System.Web.HttpContext.Current.Request.UrlReferrer;

                RegionViewModel regionViewModel = new RegionViewModel();
                //if (id == null)
                //    id = (int)TempData["ProjectId"];
                regionViewModel.GetRegionDetails();
                regionViewModel.GetScreenAccessRights("Region Details");
                //appVersionViewModel.ProjectID = (int)id;


                //int clientId;
                //string projectName;

                //CommonFunctions comfuns = new CommonFunctions();

                //comfuns.GetProjectName((int)id, out clientId, out projectName);
                //applicationViewModel.ClientId = clientId;
                //applicationViewModel.ProjectName = projectName;

                //applicationViewModel.ClientName = comfuns.GetClientName(clientId);

                return View(regionViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");

            }
        }

        [HttpPost]
        public ActionResult Index(RegionViewModel regionViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isDuplicate = false;
                    isDuplicate = regionViewModel.CheckDuplicate(regionViewModel);

                    if (isDuplicate)
                    {
                        regionViewModel.GetRegionDetails();
                        regionViewModel.GetScreenAccessRights("Region Details");
                        ViewBag.Message = "Region Already Exists";
                        return View("Index", regionViewModel);

                    }
                    else
                    {
                        regionViewModel.Region = regionViewModel.Region == null ? "" : regionViewModel.Region.Trim();
                        regionViewModel.AddRegion(regionViewModel);
                        regionViewModel.GetRegionDetails();
                        regionViewModel.GetScreenAccessRights("Region Details");
                        ViewBag.Message = "New Region Added Successfully";
                        return View("Index", regionViewModel);

                    }

                }
                regionViewModel.GetRegionDetails();
                regionViewModel.GetScreenAccessRights("Region Details");
                return View(regionViewModel);
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

                RegionViewModel regionViewModel = new RegionViewModel();
                var region = regionViewModel.FindRegion(id);
                //TempData["ProjectId"] = appVersion.Id;
                if (region == null)
                {
                    return HttpNotFound();
                }

                return View(region);

            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");

            }

        }

        // POST: Application/Edit/5
        [HttpPost]
        public ActionResult Edit(RegionViewModel regionViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    regionViewModel.Region = regionViewModel.Region.Trim();
                    regionViewModel.UpdateRegion(regionViewModel);
                    return RedirectToAction("Index", "Region");
                }

                return View(regionViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");

            }
        }

        // GET: Application/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            RegionViewModel regionViewModel = new RegionViewModel();
            var region = regionViewModel.FindRegion(id);

            if (region == null)
            {
                return HttpNotFound();
            }
            return View(region);
        }

        // POST: Application/Delete/5
        [HttpPost]
        public ActionResult Delete(RegionViewModel regionViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    regionViewModel.DeleteRegion(regionViewModel);
                    return RedirectToAction("Index", "Region");
                }

                return RedirectToAction("Index", "Region");

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


