using System;
using System.Web.Mvc;
using DesignAccelerator.Models.ViewModel;
using System.Net;
using System.Data.Entity.Infrastructure;

namespace DesignAccelerator.Controllers
{
    public class SourceController : Controller
    {
        ErrorLogViewModel errorlogviewmodel;
        // GET: DA
        [NoDirectAccess]
        public ActionResult Index(int? id)
        {
            try
            {



                if (id == null)
                    id = (int)TempData["daID"];

                SourceViewModel sourceViewModel = new SourceViewModel();
                //sourceViewModel.SourceList= sourceViewModel.GetSourceDetails((int)id);
                sourceViewModel.GetSourceDetails((int)id);
                sourceViewModel.GetScreenAccessRights("Source Details");
                CommonFunctions comfuns = new CommonFunctions();

                var da = comfuns.FindDA((int)id);
                sourceViewModel.DAID = (int)id;
                sourceViewModel.DAName = da.DAName;
                sourceViewModel.ModuleId = da.ModuleId;
                TempData["daId"] = sourceViewModel.DAID;

                return View(sourceViewModel);

            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        [HttpPost]        
        public ActionResult Index(SourceViewModel sourceViewModel)
        {
            try
            {
               

                if (ModelState.IsValid)
                {
                    sourceViewModel.sourceDesc = sourceViewModel.sourceDesc.Trim();
                    sourceViewModel.AddSource(sourceViewModel);
                    TempData["daID"] = sourceViewModel.DAID;
                    sourceViewModel.GetScreenAccessRights("Source Details");
                    return RedirectToAction("Index", "Source");
                }
                sourceViewModel.GetScreenAccessRights("Source Details");
                sourceViewModel.GetSourceDetails(sourceViewModel.DAID);

                return View(sourceViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }
        //GET user/Edit/5
        public ActionResult Edit(int? sourceId)
        {
            try
            {
                if (sourceId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                SourceViewModel sourceViewModel = new SourceViewModel();
                var source = sourceViewModel.FindSource(sourceId);
                TempData["daID"] = source.DAID;

                if (source.sourceID == 0)
                {
                    return HttpNotFound();
                }
                return View(source);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }
        //POST
        [HttpPost]
        public ActionResult Edit(SourceViewModel sourceViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isduplicate = false;
                    sourceViewModel.sourceDesc = sourceViewModel.sourceDesc.Trim();
                    isduplicate = sourceViewModel.CheckDuplicate(sourceViewModel);
                    if(isduplicate)
                    {
                        ModelState.AddModelError("sourceDesc", "Source already exists");
                        return View("Edit", sourceViewModel);
                    }
                    sourceViewModel.UpdateSource(sourceViewModel);
                    return RedirectToAction("Index", "Source");
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
        //Get USER/Delete
        public ActionResult Delete(int? sourceID)
        {
            try
            {


                if (sourceID == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                SourceViewModel sourceViewModel = new SourceViewModel();
                var source = sourceViewModel.FindSource(sourceID);
                TempData["daID"] = source.DAID;

                if (source.sourceID == 0)
                {
                    return HttpNotFound();
                }
                return View(source);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        [HttpPost]        
        public ActionResult Delete(SourceViewModel sourceViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    sourceViewModel.DeleteSource(sourceViewModel);
                    return RedirectToAction("Index", "Source");
                }
                return RedirectToAction("Index", "Source");
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