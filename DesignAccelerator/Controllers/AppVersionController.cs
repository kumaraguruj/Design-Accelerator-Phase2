using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DesignAccelerator.Models.ViewModel;
using System.Net;

namespace DesignAccelerator.Controllers
{
    public class AppVersionController : Controller
    {
        ErrorLogViewModel errorlogviewmodel;

        [NoDirectAccess]
        public ActionResult Index()
        {
            try
            {

                string urlPart = @"/Application/";
                if (System.Web.HttpContext.Current.Request.UrlReferrer.ToString().IndexOf(urlPart) > 0)
                    Session["PreviousURL"] = System.Web.HttpContext.Current.Request.UrlReferrer;

                AppVersionViewModel appVersionViewModel = new AppVersionViewModel();

                appVersionViewModel.GetAppVersionDetails();
                appVersionViewModel.GetScreenAccessRights("AppVersion Details");

                return View(appVersionViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }


        [HttpPost]
        public ActionResult Index(AppVersionViewModel appVersionViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    appVersionViewModel.AppVersion = appVersionViewModel.AppVersion.Trim();
                    appVersionViewModel.AddAppVersion(appVersionViewModel);

                    return RedirectToAction("Index", "AppVersion");
                }
                appVersionViewModel.GetAppVersionDetails();
                appVersionViewModel.GetScreenAccessRights("AppVersion Details");
                return View(appVersionViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }


        // GET: Application/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                AppVersionViewModel appVersionViewModel = new AppVersionViewModel();
                var appVersion = appVersionViewModel.FindAppVersion(id);

                if (appVersion == null)
                {
                    return HttpNotFound();
                }

                return View(appVersion);

            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }

        }


        // POST: Application/Edit/5
        [HttpPost]
        public ActionResult Edit(AppVersionViewModel appVersionViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //bool isduplicate = false;
                    //isduplicate = appVersionViewModel.CheckDuplicate(appVersionViewModel);
                    //if(isduplicate)
                    //{ 
                    //return View("Edit", "appVersionViewModel");
                    //}
                    appVersionViewModel.AppVersion = appVersionViewModel.AppVersion.Trim();
                    appVersionViewModel.UpdateAppVersion(appVersionViewModel);
                    return RedirectToAction("Index", "AppVersion");
                }
                return View(appVersionViewModel);
                //return View();
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }


        // GET: Application/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                AppVersionViewModel appVersionViewModel = new AppVersionViewModel();
                var applVersion = appVersionViewModel.FindAppVersion(id);

                if (applVersion == null)
                {
                    return HttpNotFound();
                }
                return View(applVersion);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }


        // POST: Application/Delete/5
        [HttpPost]
        public ActionResult Delete(AppVersionViewModel appVersionViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    appVersionViewModel.DeleteAppVersion(appVersionViewModel);
                    return RedirectToAction("Index", "AppVersion");
                }

                return RedirectToAction("Index", "AppVersion");
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