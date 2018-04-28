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
    public class ProductsController : Controller
    {

        #region PublicProperties
        ErrorLogViewModel errorlogviewmodel;
        #endregion

        [NoDirectAccess]
        public ActionResult Index(int? id)
        {
            try
            {
                ProductsViewModel prodviewmodel = new ProductsViewModel();
                if (id == null)
                    id = (int)TempData["daId"];
                prodviewmodel = prodviewmodel.GetProducts(id);
                prodviewmodel.GetScreenAccessRights("Products");

                ViewData["prodviewmodel1"] = (IEnumerable<ProductsViewModel>)from u in prodviewmodel.lstProd
                                                                             join b in prodviewmodel.lstLOB on u.LobID equals b.LobID
                                                                             join bp in prodviewmodel.lstBuzProd on u.BuzProdID equals bp.BuzProdID
                                                                             select new ProductsViewModel { ProductID = u.ProductID, ReqReference = u.ReqReference, BuzProdID = u.BuzProdID, BuzProdDesc = bp.BuzProdDesc, LobID = b.LobID, LobDesc = b.LobDesc };

                CommonFunctions comfuns = new CommonFunctions();

                var da = comfuns.FindDA((int)id);
                prodviewmodel.daid = (int)id;
                prodviewmodel.ModuleId = da.ModuleId;
                prodviewmodel.daName = da.DAName;
                TempData["daId"] = prodviewmodel.daid;

                int projectId;
                int clientId;
                string projectName;
                string appName;

                int applicationId;
                string modName;

                //CommonFunctions comfuns = new CommonFunctions();

                comfuns.GetModuleName(da.ModuleId, out applicationId, out modName);
                prodviewmodel.ApplicationID = applicationId;
                prodviewmodel.ModuleName = modName;

                comfuns.GetApplicationName(applicationId, out projectId, out appName);
                prodviewmodel.ProjectID = projectId;
                prodviewmodel.ApplicationName = appName;

                comfuns.GetProjectName(projectId, out clientId, out projectName);
                prodviewmodel.ClientID = clientId;
                prodviewmodel.ProjectName = projectName;

                prodviewmodel.ClientName = comfuns.GetClientName(clientId);

                if (Convert.ToString(TempData["ErrorMsg"]) != "")
                {
                    ModelState.AddModelError("ReqReference", "Requirement Reference Required");
                    TempData["ErrorMsg"] = "";
                }

                return View(prodviewmodel);
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
        public ActionResult Index(ProductsViewModel prodviewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    prodviewmodel.ReqReference = prodviewmodel.ReqReference == null ? "" : prodviewmodel.ReqReference.Trim();
                    prodviewmodel.AddProduct(prodviewmodel);
                    TempData["daId"] = prodviewmodel.daid;
                    prodviewmodel.GetScreenAccessRights("Products");
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    TempData["ErrorMsg"] = "Requirement Reference Required";
                    //ModelState.AddModelError("ReqReference", "Requirement Reference required");
                    prodviewmodel.GetScreenAccessRights("Products");
                    return RedirectToAction("Index", prodviewmodel);
                }

                //return View("Index");
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        // GET: Prod Delete
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ProductsViewModel prodviewmodel = new ProductsViewModel();
                prodviewmodel = prodviewmodel.FindProd(id);
                TempData["daId"] = prodviewmodel.daid;


                if (prodviewmodel.ProductID == 0)
                {
                    return HttpNotFound();
                }
                return View(prodviewmodel);
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
        public ActionResult Delete(ProductsViewModel prodviewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    prodviewmodel.DeleteProd(prodviewmodel);
                }
                return RedirectToAction("Index", "Products");
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

        // GET: Prod Edit
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ProductsViewModel prodviewmodel = new ProductsViewModel();
                prodviewmodel = prodviewmodel.FindProd(id);
                TempData["daId"] = prodviewmodel.daid;


                if (prodviewmodel.ProductID == 0)
                {
                    return HttpNotFound();
                }
                return View(prodviewmodel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }

        }

        //POST: Prod Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductsViewModel prodviewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    prodviewmodel.ReqReference = prodviewmodel.ReqReference == null ? "" : prodviewmodel.ReqReference.Trim();
                    bool isduplicate = false;
                    isduplicate = prodviewmodel.CheckDuplicate(prodviewmodel);
                    if (isduplicate)
                    {
                        ModelState.AddModelError("ReqReference", "ReqReference already exists");
                        prodviewmodel.GetLOBS(prodviewmodel);
                        prodviewmodel.GetBusinessProds(prodviewmodel);
                        return View("Edit", prodviewmodel);
                    }
                    prodviewmodel.UpdateProd(prodviewmodel);
                    return RedirectToAction("Index", "Products");
                }


                prodviewmodel.GetLOBS(prodviewmodel);
                prodviewmodel.GetBusinessProds(prodviewmodel);
                return View(prodviewmodel);
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