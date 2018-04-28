using System;
using System.Web.Mvc;
using DesignAccelerator.Models.ViewModel;
using System.Net;

namespace DesignAccelerator.Controllers
{
    public class DestinationController : Controller
    {
        ErrorLogViewModel errorlogviewmodel;
        // GET: DA
        [NoDirectAccess]
        public ActionResult Index(int? id)
        {
            try
            {

          
            if (id == null)
                id =(int)TempData["daID"];

            DestinationViewModel destViewModel = new DestinationViewModel();
            destViewModel.GetDestinationDetails((int)id);
            destViewModel.GetScreenAccessRights("Destination Details");
                CommonFunctions comfuns = new CommonFunctions();

            var da = comfuns.FindDA((int)id);
            destViewModel.DAID = (int)id;
            destViewModel.DAName = da.DAName;
            destViewModel.ModuleId = da.ModuleId;
            TempData["daId"] = destViewModel.DAID;

            return View(destViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }

        }
        [HttpPost]       
        public ActionResult Index(DestinationViewModel destViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    destViewModel.destDesc = destViewModel.destDesc.Trim();
                    destViewModel.AddDestination(destViewModel);
                    TempData["daID"] = destViewModel.DAID;
                    destViewModel.GetScreenAccessRights("Destination Details");
                    return RedirectToAction("Index", "Destination");
                }
                destViewModel.GetDestinationDetails(destViewModel.DAID);
                destViewModel.GetScreenAccessRights("Destination Details");
                return View(destViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }
        //GET user/Edit/5
        public ActionResult Edit(int? destId)
        {
            try
            {
                if (destId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                DestinationViewModel destViewModel = new DestinationViewModel();
                var dest = destViewModel.FindDestination(destId);
                TempData["daID"] = dest.DAID;

                if (dest.destID == 0)
                {
                    return HttpNotFound();
                }
                return View(dest);
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
        public ActionResult Edit(DestinationViewModel destViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isduplicate = false;
                    destViewModel.destDesc = destViewModel.destDesc.Trim();
                    isduplicate = destViewModel.CheckDuplicate(destViewModel);
                    if(isduplicate)
                    {
                        ModelState.AddModelError("destDesc", "Destination already exists");
                        return View("Edit", destViewModel);
                    }

                    destViewModel.UpdateDestination(destViewModel);
                    return RedirectToAction("Index", "Destination");
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
        public ActionResult Delete(int? destID)
        {
            try
            {

           
            if (destID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }         

            DestinationViewModel destViewModel = new DestinationViewModel();
            var  dest = destViewModel.FindDestination(destID);
            TempData["daID"] = dest.DAID;
            if (dest.destID == 0)
            {
                return HttpNotFound();
            }
            return View(dest);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }
        [HttpPost]       
        public ActionResult Delete(DestinationViewModel destViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    destViewModel.DeleteDestination(destViewModel);                    
                    return RedirectToAction("Index", "Destination");
                }
                return RedirectToAction("Index", "Destination");
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