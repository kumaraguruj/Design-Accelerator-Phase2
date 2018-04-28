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
    public class ModeTypeController : Controller
    {
        ErrorLogViewModel errorlogviewmodel;
        [NoDirectAccess]
        public ActionResult Index(int? id)
        {
            try
            {
                if (id == null)
                    id = (int)TempData["daId"];

                ModeTypeViewModel modetypeviewmodel = new ModeTypeViewModel();
                modetypeviewmodel = modetypeviewmodel.GetlModeTypes(id);
                modetypeviewmodel.GetScreenAccessRights("ModeType Input");
                CommonFunctions comfuns = new CommonFunctions();

                var da = comfuns.FindDA((int)id);
                modetypeviewmodel.daid = (int)id;
                modetypeviewmodel.daName = da.DAName;
                modetypeviewmodel.ModuleId = da.ModuleId;
                TempData["daId"] = modetypeviewmodel.daid;

                return View(modetypeviewmodel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult Index(ModeTypeViewModel modetypeviewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    modetypeviewmodel.ModeTypeDesc = modetypeviewmodel.ModeTypeDesc.Trim();
                    modetypeviewmodel.AddModeType(modetypeviewmodel);
                    TempData["daId"] = modetypeviewmodel.daid;
                    modetypeviewmodel.GetScreenAccessRights("ModeType Input");
                    return RedirectToAction("Index", "ModeType");
                }
                modetypeviewmodel = modetypeviewmodel.GetlModeTypes(modetypeviewmodel.daid);
                modetypeviewmodel.GetScreenAccessRights("ModeType Input");
                return View(modetypeviewmodel);
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

                ModeTypeViewModel modetypeviewmodel = new ModeTypeViewModel();
                var modetype = modetypeviewmodel.FindModeTypes(id);
                TempData["daId"] = modetype.daid;

                if (modetype.ModeTypeID == 0)
                {
                    return HttpNotFound();
                }
                return View(modetype);
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
        public ActionResult Delete(ModeTypeViewModel modetypeviewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    modetypeviewmodel.DeleteModeType(modetypeviewmodel);
                    modetypeviewmodel.GetlModeTypes(modetypeviewmodel.daid);
                }
                return RedirectToAction("Index", "ModeType");
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

        // GET: ModeType Edit
        public ActionResult Edit(int? id)
        {
            try
            {


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ModeTypeViewModel modetypeviewmodel = new ModeTypeViewModel();
                var modetype = modetypeviewmodel.FindModeTypes(id);
                TempData["daId"] = modetype.daid;

                if (modetype.ModeTypeID == 0)
                {
                    return HttpNotFound();
                }
                return View(modetype);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");

            }
        }

        //POST: ModeType Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ModeTypeViewModel modetypeviewmodel)
        {
            try
            {


                if (ModelState.IsValid)
                {
                    bool isduplicate = false;
                    modetypeviewmodel.ModeTypeDesc = modetypeviewmodel.ModeTypeDesc.Trim();
                    isduplicate = modetypeviewmodel.CheckDuplicate(modetypeviewmodel);
                    if (isduplicate)
                    {
                        ModelState.AddModelError("ModeTypeDesc", "Mode already exists");
                        return View("Edit", modetypeviewmodel);
                    }
                    modetypeviewmodel.UpdateModeType(modetypeviewmodel);
                    return RedirectToAction("Index", "ModeType");
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