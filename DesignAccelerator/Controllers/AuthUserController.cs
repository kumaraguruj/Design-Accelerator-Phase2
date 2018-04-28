using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DesignAccelerator.Models.ViewModel;

namespace DesignAccelerator.Controllers
{
    public class AuthUserController : Controller
    {
        ErrorLogViewModel errorlogviewmodel;
        AuthUserViewModel myauthuserViewModel;

        // GET: AuthUser
        [NoDirectAccess]        
        public ActionResult Index()
        {
            try
            {
                myauthuserViewModel = new AuthUserViewModel();
               // List<AuthUserViewModel> lstauthuserViewModel = new List<AuthUserViewModel>();
               
                 myauthuserViewModel.getAuthUsersFrmDB();

                ViewBag.Numberofauthusers = myauthuserViewModel.lstauthusers.Count;

                return View(myauthuserViewModel.lstauthusers);
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
        public ActionResult Index(FormCollection form)
        {
            try
            {
                myauthuserViewModel = new AuthUserViewModel();

                if (ModelState.IsValid)
                {
                    var chckedValues = form.GetValues("chkStatus");
                    
                    foreach (var id in chckedValues)
                    {
                       int id1 = Convert.ToInt32(id);
                        myauthuserViewModel.UpdateUserActive(id1);
                    }

                    myauthuserViewModel.getAuthUsersFrmDB();

                    ViewBag.Numberofauthusers = myauthuserViewModel.lstauthusers.Count;

                }
                return View(myauthuserViewModel.lstauthusers);
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