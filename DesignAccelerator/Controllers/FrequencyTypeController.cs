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
    public class FrequencyTypeController : Controller
    {
        ErrorLogViewModel errorlogviewmodel;

        [NoDirectAccess]
        public ActionResult Index(int? id)
        {
            try
            {
                FrequencyTypeViewModel frequencytypeviewmodel = new FrequencyTypeViewModel();
                if (id == null)
                    id = (int)TempData["daId"];

                frequencytypeviewmodel = frequencytypeviewmodel.GetFreqType(id);
                frequencytypeviewmodel.GetScreenAccessRights("Frequency Type");
                CommonFunctions comfuns = new CommonFunctions();

                var da = comfuns.FindDA((int)id);
                frequencytypeviewmodel.daid = (int)id;
                frequencytypeviewmodel.ModuleId = da.ModuleId;
                frequencytypeviewmodel.daName = da.DAName;
                TempData["daId"] = frequencytypeviewmodel.daid;

                return View(frequencytypeviewmodel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        [HttpPost]        
        public ActionResult Index(FrequencyTypeViewModel frequencytypeviewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    frequencytypeviewmodel.FreqTypeDesc = frequencytypeviewmodel.FreqTypeDesc.Trim();
                    frequencytypeviewmodel.AddFrequencyType(frequencytypeviewmodel);
                    TempData["daId"] = frequencytypeviewmodel.daid;
                    frequencytypeviewmodel.GetScreenAccessRights("Frequency Type");
                    return RedirectToAction("Index", "FrequencyType");
                }
                frequencytypeviewmodel = frequencytypeviewmodel.GetFreqType(frequencytypeviewmodel.daid);

                return View(frequencytypeviewmodel);
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

                FrequencyTypeViewModel frequencytypeviewmodel = new FrequencyTypeViewModel();
                var frequencytype = frequencytypeviewmodel.FindFrequencyTypes(id);
                TempData["daId"] = frequencytype.daid;

                if (frequencytype.FreqTypeID == 0)
                {
                    return HttpNotFound();
                }
                return View(frequencytype);
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
        public ActionResult Delete(FrequencyTypeViewModel frequencytypeviewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    frequencytypeviewmodel.DeleteFrequencyType(frequencytypeviewmodel);
                    frequencytypeviewmodel.GetFreqType(frequencytypeviewmodel.daid);                                      
                }
                return RedirectToAction("Index", "FrequencyType");
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

        // GET: FrequencyType Edit
        public ActionResult Edit(int? id)
        {
            try
            {


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                FrequencyTypeViewModel frequencytypeviewmodel = new FrequencyTypeViewModel();
                var frequencytype = frequencytypeviewmodel.FindFrequencyTypes(id);
                TempData["daId"] = frequencytype.daid;

                if (frequencytype.FreqTypeID == 0)
                {
                    return HttpNotFound();
                }
                return View(frequencytype);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        //POST: FrequencyType Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FrequencyTypeViewModel frequencytypeviewmodel)
        {
            try
            {


                if (ModelState.IsValid)
                {
                    bool isduplicate = false;
                    frequencytypeviewmodel.FreqTypeDesc = frequencytypeviewmodel.FreqTypeDesc.Trim();
                    isduplicate = frequencytypeviewmodel.CheckDuplicate(frequencytypeviewmodel);
                    if (isduplicate)
                    {
                        ModelState.AddModelError("FreqTypeDesc", "Frequency already exists");
                        return View("Edit", frequencytypeviewmodel);
                    }
                    frequencytypeviewmodel.UpdateFrequencyType(frequencytypeviewmodel);
                    return RedirectToAction("Index", "FrequencyType");
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