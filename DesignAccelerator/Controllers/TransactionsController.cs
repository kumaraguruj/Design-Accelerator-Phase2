using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DesignAccelerator.Models.ViewModel;
using System.Net;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;

namespace DesignAccelerator.Controllers
{
    public class TransactionsController : Controller
    {
        ErrorLogViewModel errorlogviewmodel;
        [NoDirectAccess]
        public ActionResult Index(int? id)
        {
            try
            {

                TransactionsViewModel transviewmodel = new TransactionsViewModel();
                if (id == null)
                    id = (int)TempData["daId"];
                transviewmodel = transviewmodel.GetTransactions(id);
                transviewmodel.GetScreenAccessRights("Transactions");

                ViewData["transviewmodel1"] = (IEnumerable<TransactionsViewModel>)from u in transviewmodel.lstTransactions
                                                                                  join b in transviewmodel.lstLifeCycle on u.LifeCycleID equals b.LifeCycleID
                                                                                  select new TransactionsViewModel { TransactionSeq = u.TransactionSeq, ReqReference = u.ReqReference, HighLevelTxnID = u.HighLevelTxnID, HighLevelTxnDesc = u.HighLevelTxnDesc, LifeCycleID = b.LifeCycleID, LifeCycleDesc = b.LifeCycleDesc };
                //TempData["daId"] = transviewmodel.daId;

                CommonFunctions comfuns = new CommonFunctions();

                var da = comfuns.FindDA((int)id);
                transviewmodel.daId = (int)id;
                transviewmodel.ModuleId = da.ModuleId;
                transviewmodel.daName = da.DAName;
                TempData["daId"] = transviewmodel.daId;

                int clientId;
                int projectId;
                int applicationId;
                //int moduleId;

                string projectName;
                string appName;
                string modName;
                //string prodName;


                //transviewmodel.ProductName = prodName;

                comfuns.GetModuleName(transviewmodel.ModuleId, out applicationId, out modName);
                transviewmodel.ApplicationID = applicationId;
                transviewmodel.ModuleName = modName;

                comfuns.GetApplicationName(applicationId, out projectId, out appName);
                transviewmodel.ProjectID = projectId;
                transviewmodel.ApplicationName = appName;

                comfuns.GetProjectName(projectId, out clientId, out projectName);
                transviewmodel.ClientID = clientId;
                transviewmodel.ProjectName = projectName;

                transviewmodel.ClientName = comfuns.GetClientName(clientId);

                return View(transviewmodel);
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
        public ActionResult Index(TransactionsViewModel transactionsViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    transactionsViewModel.HighLevelTxnDesc = transactionsViewModel.HighLevelTxnDesc.Trim();
                    if (transactionsViewModel.ReqReference != null)
                    {
                        transactionsViewModel.ReqReference = transactionsViewModel.ReqReference.Trim();
                    }
                    TransactionsViewModel transVM = new TransactionsViewModel();
                    TempData["daId"] = transactionsViewModel.daId;
                    transactionsViewModel.AddTrans(transactionsViewModel);
                    transactionsViewModel.GetScreenAccessRights("Transactions");

                    return RedirectToAction("Index", "Transactions");
                }
                TransactionsViewModel transviewmodel = new TransactionsViewModel();
                transviewmodel = transviewmodel.GetTransactions(transactionsViewModel.daId);
                transactionsViewModel.GetScreenAccessRights("Transactions");

                ViewData["transviewmodel1"] = (IEnumerable<TransactionsViewModel>)from u in transviewmodel.lstTransactions
                                                                                  join b in transviewmodel.lstLifeCycle on u.LifeCycleID equals b.LifeCycleID
                                                                                  select new TransactionsViewModel { TransactionSeq = u.TransactionSeq, ReqReference = u.ReqReference, HighLevelTxnID = u.HighLevelTxnID, HighLevelTxnDesc = u.HighLevelTxnDesc, LifeCycleID = b.LifeCycleID, LifeCycleDesc = b.LifeCycleDesc };
                //TempData["daId"] = transviewmodel.daId;

                CommonFunctions comfuns = new CommonFunctions();

                var da = comfuns.FindDA(transactionsViewModel.daId);
                transviewmodel.ModuleId = da.ModuleId;
                transviewmodel.daName = da.DAName;
                TempData["daId"] = transviewmodel.daId;

                int clientId;
                int projectId;
                int applicationId;
                //int moduleId;

                string projectName;
                string appName;
                string modName;
                //string prodName;


                //transviewmodel.ProductName = prodName;

                comfuns.GetModuleName(transviewmodel.ModuleId, out applicationId, out modName);
                transviewmodel.ApplicationID = applicationId;
                transviewmodel.ModuleName = modName;

                comfuns.GetApplicationName(applicationId, out projectId, out appName);
                transviewmodel.ProjectID = projectId;
                transviewmodel.ApplicationName = appName;

                comfuns.GetProjectName(projectId, out clientId, out projectName);
                transviewmodel.ClientID = clientId;
                transviewmodel.ProjectName = projectName;

                transviewmodel.ClientName = comfuns.GetClientName(clientId);

                return View(transviewmodel);

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

                TransactionsViewModel transviewmodel = new TransactionsViewModel();
                transviewmodel = transviewmodel.FindTrans(id);
                TempData["daId"] = transviewmodel.daId;

                if (transviewmodel.TransactionSeq == 0)
                {
                    return HttpNotFound();
                }
                return View(transviewmodel);
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
        public ActionResult Delete(TransactionsViewModel transviewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    transviewmodel.DeleteTrans(transviewmodel);
                    return RedirectToAction("Index", "Transactions");
                }
                return RedirectToAction("Index", "Transactions");
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

        public ActionResult Edit(int? id)
        {
            try
            {


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                TransactionsViewModel transviewmodel = new TransactionsViewModel();
                transviewmodel = transviewmodel.FindTrans(id);
                TempData["daId"] = transviewmodel.daId;

                if (transviewmodel.TransactionSeq == 0)
                {
                    return HttpNotFound();
                }
                return View(transviewmodel);
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
        public ActionResult Edit(TransactionsViewModel transviewmodel)
        {
            try
            {


                if (ModelState.IsValid)
                {
                    transviewmodel.HighLevelTxnDesc = transviewmodel.HighLevelTxnDesc.Trim();
                    if (transviewmodel.ReqReference != null)
                    {
                        transviewmodel.ReqReference = transviewmodel.ReqReference.Trim();
                    }
                    bool isduplicate = false;
                    isduplicate = transviewmodel.CheckDuplicate(transviewmodel);
                    if (isduplicate)
                    {
                        ModelState.AddModelError("HighLevelTxnID", "HighLevelTransaction already exists");
                        transviewmodel.GetLifeCycle(transviewmodel);
                        return View("Edit", transviewmodel);
                    }
                    transviewmodel.GetLifeCycle(transviewmodel);
                    transviewmodel.UpdateTrans(transviewmodel);
                    return RedirectToAction("Index", "Transactions");
                }
                transviewmodel.GetLifeCycle(transviewmodel);
                return View(transviewmodel);
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