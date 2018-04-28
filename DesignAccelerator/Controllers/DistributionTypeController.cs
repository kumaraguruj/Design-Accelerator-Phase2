using System;
using System.Web.Mvc;
using DesignAccelerator.Models.ViewModel;
using System.Net;
using System.Data.Entity.Infrastructure;

namespace DesignAccelerator.Controllers
{
    public class DistributionTypeController : Controller
    {
        ErrorLogViewModel errorlogviewmodel;
        // GET: DA
        [NoDirectAccess]
        public ActionResult Index(int? id)
        {
            try
            {

           
            DistributionTypeViewModel distributionTypeViewModel = new DistributionTypeViewModel();

            if (id == null)
                id = (int)TempData["daID"];

            distributionTypeViewModel.GetDistributionTypeDetails((int)id);
            distributionTypeViewModel.GetScreenAccessRights("Distribution Type");
            CommonFunctions comfuns = new CommonFunctions();

            var da = comfuns.FindDA((int)id);
            distributionTypeViewModel.DAID = (int)id;
            distributionTypeViewModel.DAName = da.DAName;
            distributionTypeViewModel.ModuleId = da.ModuleId;
            TempData["daId"] = distributionTypeViewModel.DAID;
            
            return View(distributionTypeViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }

        }

        [HttpPost]        
        public ActionResult Index(DistributionTypeViewModel distributionTypeViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    distributionTypeViewModel.distributionDesc = distributionTypeViewModel.distributionDesc.Trim();
                    distributionTypeViewModel.AddDistributionType(distributionTypeViewModel);
                    TempData["daID"] = distributionTypeViewModel.DAID;
                    distributionTypeViewModel.GetScreenAccessRights("Distribution Type");
                    return RedirectToAction("Index", "DistributionType");
                }
                distributionTypeViewModel.GetScreenAccessRights("Distribution Type");
                distributionTypeViewModel.GetDistributionTypeDetails(distributionTypeViewModel.DAID);

                return View(distributionTypeViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        //GET user/Edit/5
        public ActionResult Edit(int? distributionTypeId)
        {
            try
            {
                if (distributionTypeId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                DistributionTypeViewModel distributionTypeViewModel = new DistributionTypeViewModel();
                var distributionType = distributionTypeViewModel.FindDistributionType(distributionTypeId);
                TempData["daID"] = distributionType.DAID;

                if (distributionType.distributionTypeID == 0)
                {
                    return HttpNotFound();
                }
                return View(distributionType);
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
        public ActionResult Edit(DistributionTypeViewModel distributionTypeViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isduplicate = false;
                    distributionTypeViewModel.distributionDesc = distributionTypeViewModel.distributionDesc.Trim();
                    isduplicate = distributionTypeViewModel.CheckDuplicate(distributionTypeViewModel);
                    if(isduplicate)
                    {
                        ModelState.AddModelError("distributionDesc", "Distribution already exists");
                        return View("Edit", distributionTypeViewModel);
                    }

                    distributionTypeViewModel.UpdateDistributionType(distributionTypeViewModel);                    
                    return RedirectToAction("Index", "DistributionType");
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
        public ActionResult Delete(int? distributionTypeID)
        {
            try
            {

           
            if (distributionTypeID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }         

            DistributionTypeViewModel distributionTypeViewModel = new DistributionTypeViewModel();
            var distributionType = distributionTypeViewModel.FindDistributionType(distributionTypeID);
            TempData["daID"] = distributionType.DAID;

            if (distributionType.distributionTypeID == 0)
            {
                return HttpNotFound();
            }
            return View(distributionType);
            }
           
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        [HttpPost]       
        public ActionResult Delete(DistributionTypeViewModel distributionTypeViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    distributionTypeViewModel.DeleteDistributionType(distributionTypeViewModel);                   
                    return RedirectToAction("Index", "DistributionType");
                }
                return RedirectToAction("Index", "DistributionType");
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