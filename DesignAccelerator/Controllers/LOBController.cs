using System;
using System.Web.Mvc;
using DesignAccelerator.Models.ViewModel;
using System.Net;
using System.Data.Entity.Infrastructure;

namespace DesignAccelerator.Controllers
{
    public class LOBController : Controller
    {

        #region PublicProperties
        ErrorLogViewModel errorlogviewmodel;
        #endregion

        // GET: DA
        [NoDirectAccess]
        public ActionResult Index(int? id)
        {
            try
            {
                if (System.Web.HttpContext.Current.Request.UrlReferrer.ToString().IndexOf("Products") > 0)
                    Session["PreviousURL"] = System.Web.HttpContext.Current.Request.UrlReferrer;
                LOBViewModel lobViewModel = new LOBViewModel();
                if (id == null)
                    id = (int)TempData["daID"];
                lobViewModel.GetLobDetails((int)id);
                lobViewModel.GetScreenAccessRights("LOB Details");
                lobViewModel.DAID = (int)id;

                return View(lobViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }
        [HttpPost]        
        public ActionResult Index(LOBViewModel lobViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    lobViewModel.lobDesc = lobViewModel.lobDesc.Trim();
                   
                    lobViewModel.AddLOB(lobViewModel);
                    TempData["daID"] = lobViewModel.DAID;
                    lobViewModel.GetScreenAccessRights("LOB Details");
                    return RedirectToAction("Index", "LOB");
                }

                lobViewModel.GetLobDetails(lobViewModel.DAID);
                lobViewModel.GetScreenAccessRights("LOB Details");
                return View(lobViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }
        //GET user/Edit/5
        public ActionResult Edit(int? lobId)
        {
            try
            {
                if (lobId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                LOBViewModel lobViewModel = new LOBViewModel();
                var lob = lobViewModel.FindLob(lobId);
                TempData["daID"] = lob.DAID;

                if (lob.lobID == 0)
                {
                    return HttpNotFound();
                }
                return View(lob);
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
        public ActionResult Edit(LOBViewModel lobViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    lobViewModel.lobDesc = lobViewModel.lobDesc.Trim();
                    lobViewModel.UpdateLob(lobViewModel);
                    return RedirectToAction("Index", "LOB");
                }
                return View(lobViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }
        //Get USER/Delete
        public ActionResult Delete(int? lobID)
        {
            try
            {
                if (lobID == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                LOBViewModel lobViewModel = new LOBViewModel();
                var lob = lobViewModel.FindLob(lobID);
                TempData["daID"] = lob.DAID;

                if (lob.lobID == 0)
                {
                    return HttpNotFound();
                }
                return View(lob);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }
        [HttpPost]        
        public ActionResult Delete(LOBViewModel lobViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    lobViewModel.DeleteLob(lobViewModel);                   
                    return RedirectToAction("Index", "LOB");
                }
                return RedirectToAction("Index", "LOB");
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