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
    public class LifeCyclesController : Controller
    {
        ErrorLogViewModel errorlogviewmodel;
        [NoDirectAccess]
        public ActionResult Index(int? id)
        {
            try
            {
                if (System.Web.HttpContext.Current.Request.UrlReferrer.ToString().IndexOf("Transactions") > 0)
                    Session["PreviousURL"] = System.Web.HttpContext.Current.Request.UrlReferrer;

                LifeCyclesViewModel lifecyclesviewmodel = new LifeCyclesViewModel();
                if (id == null)
                    id = (int)TempData["daId"];
                lifecyclesviewmodel = lifecyclesviewmodel.GetTransactionsLifecycle(id);
                lifecyclesviewmodel.GetScreenAccessRights("Transaction Life Cycle");

                //TempData["daId"] = lifecyclesviewmodel.daid;
                lifecyclesviewmodel.daid = (int)id;

                return View(lifecyclesviewmodel);
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
        public ActionResult Index(LifeCyclesViewModel lifecyclesviewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    lifecyclesviewmodel.LifeCycleDesc = lifecyclesviewmodel.LifeCycleDesc.Trim();
                    lifecyclesviewmodel.AddLifeCycle(lifecyclesviewmodel);
                    TempData["daId"] = lifecyclesviewmodel.daid;
                    lifecyclesviewmodel.GetScreenAccessRights("Transaction Life Cycle");
                    return RedirectToAction("Index", "LifeCycles");
                }
                lifecyclesviewmodel = lifecyclesviewmodel.GetTransactionsLifecycle(lifecyclesviewmodel.daid);
                lifecyclesviewmodel.GetScreenAccessRights("Transaction Life Cycle");
                return View(lifecyclesviewmodel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        public ActionResult Delete(int? id)
        {
            try
            {


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                LifeCyclesViewModel lifecyclesviewmodel = new LifeCyclesViewModel();
                var lifecycle = lifecyclesviewmodel.FindLifeCycles(id);
                TempData["daId"] = lifecycle.daid;

                if (lifecycle.LifeCycleID == 0)
                {
                    return HttpNotFound();
                }
                return View(lifecycle);
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
        public ActionResult Delete(LifeCyclesViewModel lifecyclesviewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    lifecyclesviewmodel.DeleteLifeCycle(lifecyclesviewmodel);
                    lifecyclesviewmodel.GetTransactionsLifecycle(lifecyclesviewmodel.daid);                   
                }
                return RedirectToAction("Index", "LifeCycles");
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

        // GET: LifeCycle Edit
        public ActionResult Edit(int? id)
        {
            try
            {


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                LifeCyclesViewModel lifecyclesviewmodel = new LifeCyclesViewModel();
                var lifecycle = lifecyclesviewmodel.FindLifeCycles(id);
                TempData["daId"] = lifecycle.daid;

                if (lifecycle.LifeCycleID == 0)
                {
                    return HttpNotFound();
                }
                return View(lifecycle);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        //POST: LifeCycle Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LifeCyclesViewModel lifecyclesviewmodel)
        {
            try
            {


                if (ModelState.IsValid)
                {
                    lifecyclesviewmodel.LifeCycleDesc = lifecyclesviewmodel.LifeCycleDesc.Trim();
                    lifecyclesviewmodel.UpdateLifeCycle(lifecyclesviewmodel);
                    return RedirectToAction("Index", "LifeCycles");
                }
                return View();
            }
            catch(Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }
    }
}