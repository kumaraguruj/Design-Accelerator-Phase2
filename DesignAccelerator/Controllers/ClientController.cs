using System;
using System.Web.Mvc;
using DesignAccelerator.Models.ViewModel;
using System.Net;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace DesignAccelerator.Controllers
{
    public class ClientController : Controller
    {

        #region PublicProperties
        ErrorLogViewModel errorlogviewmodel;        
        #endregion

        // GET: Client
        [Authorize]
        public ActionResult Index()
        {
            try
            {
                ClientViewModel clientViewModel = new ClientViewModel();
                //if (id == null)
                //    id = (int)TempData["ClientId"];
                clientViewModel.GetClientDetails();
                clientViewModel.GetScreenAccessRights("Client Details");
                //clientViewModel.ClientID = (int)id;
                
                //int ClientId;

                //CommonFunctions comfuns = new CommonFunctions();
                //var da = comfuns.FindDA((int)id);
                //clientViewModel.daid = (int)id;

                return View(clientViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        [HttpPost]        
        public ActionResult Index(ClientViewModel clientViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isDuplicate = false;
                    clientViewModel.ClientName = clientViewModel.ClientName.Trim();
                    isDuplicate = clientViewModel.CheckDuplicate(clientViewModel);

                    if (isDuplicate)
                    {
                        clientViewModel.GetClientDetails();
                        ViewBag.Message = "Client Already Exists";
                        clientViewModel.GetScreenAccessRights("Client Details");
                        return View("Index",clientViewModel);
                    }
                    else
                    {
                        clientViewModel.AddClient(clientViewModel);
                        clientViewModel.GetClientDetails();
                        ViewBag.Message = "New Client Added Successfully";
                        clientViewModel.GetScreenAccessRights("Client Details");
                        return View("Index", clientViewModel);
                    }
                }
                clientViewModel.GetClientDetails();
                clientViewModel.GetScreenAccessRights("Client Details");
                return View(clientViewModel);
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

                ClientViewModel clientViewModel = new ClientViewModel();
                var client = clientViewModel.FindClient(id);

                if (client == null)
                {
                    return HttpNotFound();
                }
                return View(client);
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
        public ActionResult Delete(ClientViewModel clientViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    clientViewModel.DeleteClient(clientViewModel);
                    return RedirectToAction("Index", "Client");
                }
                return RedirectToAction("Index", "Client");
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

        //// GET: /User/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ClientViewModel clientViewModel = new ClientViewModel();
                var client = clientViewModel.FindClient(id);

                if (client == null)
                {
                    return HttpNotFound();
                }
                return View(client);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");                
            }
        }

        //POST: /User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientViewModel clientViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    clientViewModel.ClientName = clientViewModel.ClientName.Trim();
                    bool isDuplicate = false;
                    isDuplicate = clientViewModel.CheckDuplicate(clientViewModel);
                    if (isDuplicate)
                    {
                        ModelState.AddModelError("ClientName", "Client already exists");
                        return View("Edit", clientViewModel);
                    }

                    clientViewModel.UpdateClient(clientViewModel);
                    return RedirectToAction("Index");
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
    }
}