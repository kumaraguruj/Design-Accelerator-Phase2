using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using DesignAccelerator.Models.ViewModel;
using DA.BusinessLayer;
using DA.DomainModel;

namespace DesignAccelerator.Controllers
{
    public class ChangePasswordController : Controller
    {
        ErrorLogViewModel errorlogviewmodel;
        // GET: ChangePassword
        [NoDirectAccess]
        public ActionResult Index()
        {
            try
            {

                return View();
            }

            catch(Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
            
        }

        [HttpPost]
        public ActionResult Index(ChangePasswordViewModel changePasswordViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserManager userManager = new UserManager();
                    tbl_UserData tblUserData = null;
                    tblUserData = userManager.FindUserName(changePasswordViewModel.userName.Trim());
                    
                    if (tblUserData != null)
                    {
                        if (tblUserData.Password.Trim().ToLower() != changePasswordViewModel.oldPassword.Trim().ToLower())
                        {
                            ViewBag.Message = "Old password does not match.";

                            return View("Index", changePasswordViewModel);

                        }
                        else
                        {
                            tblUserData.Password = changePasswordViewModel.newPassword;
                            changePasswordViewModel.UpdateChangePwdUser(tblUserData);
                            ViewBag.Message = "New password changed Successfully..";

                            return View("Index", changePasswordViewModel);
                        }
                    }
                    else
                    {
                        ViewBag.Message = "User does not exists";
                      
                    }
                }
                return View("Index", changePasswordViewModel);
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