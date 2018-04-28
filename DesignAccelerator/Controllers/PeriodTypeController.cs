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
    public class PeriodTypeController : Controller
    {
        ErrorLogViewModel errorlogviewmodel;
        // GET: PeriodType
        //[NoDirectAccess]
        public ActionResult Index(int? id)
        {
            try
            {


                PeriodTypeViewModel periodTypeViewModel = new PeriodTypeViewModel();
                periodTypeViewModel.PeriodTypeList = periodTypeViewModel.GetPeriodTypeDetails((int)id);// (int)id);

                return View(periodTypeViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult Index(PeriodTypeViewModel periodTypeViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    periodTypeViewModel.AddPeriodType(periodTypeViewModel);
                    return RedirectToAction("Index", "PeriodType");
                }

                return RedirectToAction("Index", "PeriodType");
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");

            }
        }
        //GET user/Edit/5
        public ActionResult Edit(int? periodTypeId)
        {
            try
            {
                if (periodTypeId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                PeriodTypeViewModel periodTypeViewModel = new PeriodTypeViewModel();
                var periodType = periodTypeViewModel.FindPeriodType(periodTypeId);
                
                if (periodType == null)
                {
                    return HttpNotFound();
                }
                return View(periodType);
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
        public ActionResult Edit(PeriodTypeViewModel periodTypeViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    periodTypeViewModel.UpdatePeriodType(periodTypeViewModel);
                    return RedirectToAction("Index", "PeriodType");
                }
                return RedirectToAction("Index", "PeriodType");
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");

            }
        }
        //Get USER/Delete
        public ActionResult Delete(int? periodTypeID)
        {
            if (periodTypeID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PeriodTypeViewModel periodTypeViewModel = new PeriodTypeViewModel();
            var periodType = periodTypeViewModel.FindPeriodType(periodTypeID);
            
            if (periodType == null)
            {
                return HttpNotFound();
            }
            return View(periodType);
        }
        [HttpPost]
        public ActionResult Delete(PeriodTypeViewModel periodTypeViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    periodTypeViewModel.DeletePeriodType(periodTypeViewModel);
                    return RedirectToAction("Index", "PeriodType");
                }
                return RedirectToAction("Index", "PeriodType");
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