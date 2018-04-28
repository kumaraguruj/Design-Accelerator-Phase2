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
    public class BusinessProductsController : Controller
    {

        #region PublicProperties
        ErrorLogViewModel errorlogviewmodel;
        #endregion

        [NoDirectAccess]
        public ActionResult Index(int? id)
        {
            try
            {
                string urlPart = @"/Products/";
                if (System.Web.HttpContext.Current.Request.UrlReferrer.ToString().IndexOf(urlPart) > 0)
                    Session["PreviousURL"] = System.Web.HttpContext.Current.Request.UrlReferrer;

                BuzProdViewModel buzprodviewmodel = new BuzProdViewModel();
                if (id == null)
                    id = (int)TempData["daId"];
                buzprodviewmodel = buzprodviewmodel.GetBuzProducts(id);
                buzprodviewmodel.GetScreenAccessRights("Business Products");

                CommonFunctions comfuns = new CommonFunctions();

                var da = comfuns.FindDA((int)id);
                buzprodviewmodel.daId = (int) id;
                buzprodviewmodel.ApplicationID = da.ApplicationID;
                buzprodviewmodel.daName = da.DAName;
                TempData["daId"] = buzprodviewmodel.daId;

                return View(buzprodviewmodel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        [HttpPost]        
        public ActionResult Index(BuzProdViewModel buzprodviewmodel)
        {
            try
            {                               
                if (ModelState.IsValid)
                { 
                    buzprodviewmodel.AddBuzProd(buzprodviewmodel);
                    TempData["daId"] = buzprodviewmodel.daId;
                    buzprodviewmodel.GetScreenAccessRights("Business Products");
                    return RedirectToAction("Index", "BusinessProducts");
                }
                buzprodviewmodel = buzprodviewmodel.GetBuzProducts(buzprodviewmodel.daId);
                buzprodviewmodel.GetScreenAccessRights("Business Products");
                return View(buzprodviewmodel);
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

                BuzProdViewModel buzprodviewmodel = new BuzProdViewModel();
                var buzprod = buzprodviewmodel.FindBuzProd(id);
                TempData["daId"] = buzprod.daId;

                if (buzprod.BuzProdID == 0)
                {
                    return HttpNotFound();
                }
                return View(buzprod);
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
        public ActionResult Delete(BuzProdViewModel buzprodviewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    buzprodviewmodel.DeleteBuzProd(buzprodviewmodel);
                    buzprodviewmodel.GetBuzProducts(buzprodviewmodel.daId);
                }
                return RedirectToAction("Index", "BusinessProducts");
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

        // GET: BuzProd Edit
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                BuzProdViewModel buzprodviewmodel = new BuzProdViewModel();
                var buzprod = buzprodviewmodel.FindBuzProd(id);
                TempData["daId"] = buzprod.daId;

                if (buzprod.BuzProdID == 0)
                {
                    return HttpNotFound();
                }
                return View(buzprod);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        //POST: BuzProd Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BuzProdViewModel buzprodviewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    buzprodviewmodel.UpdateBuzProd(buzprodviewmodel);
                    return RedirectToAction("Index", "BusinessProducts");
                }
                return View(buzprodviewmodel);
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