using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using DesignAccelerator.Models;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using DesignAccelerator.Models.ViewModel;
using System.Linq;
using DA.BusinessLayer;
using DA.DomainModel;
using System;

namespace DesignAccelerator.Controllers
{
    public class LoginController : Controller
    {
        ErrorLogViewModel errorlogviewmodel;
        private static LoginViewModel loginViewModel;

        [AllowAnonymous]
        public virtual ActionResult Index()
        {
            try
            {


                loginViewModel = new LoginViewModel();
                loginViewModel.UserTypes = new List<UserType>
            {
                new UserType{ID ="1", Type = "Internal"},
                new UserType{ID ="2", Type = "External"}
            };

                ViewData["UserTypeID"] = "1";
                return View(loginViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Index(LoginViewModel model, string returnUrl)
        {
            try
            {
                bool userflag = false;
                tbl_UserData currentUserData;

                model.UserTypes = new List<UserType>
                {
                new UserType{ID ="1", Type = "Internal"},
                new UserType{ID ="2", Type = "External"}
                };

                if (!ModelState.IsValid)
                {
                    ViewData["UserTypeID"] = model.UserTypeId;
                    return View(model);
                }


                //  model.roleID = model.GetRoleID(model.ID);

                //model.UserTypeId = "1";

                if (model.UserTypeId == "1")
                {
                    model.UserTypeId = "1";
                    ViewData["UserTypeID"] = 1;

                    // usually this will be injected via DI. but creating this manually now for brevity
                    IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                    currentUserData = model.CheckUser(model.Username);

                    Session["CurrentLoggedInUserDetails"] = currentUserData;
                    

                    if (currentUserData != null)
                    {
                        userflag = true;
                    }

                    if (userflag)
                    {
                        if (currentUserData.Active == "1")
                        {
                            var authService = new AuthenticationService(authenticationManager);

                            var authenticationResult = authService.SignIn(model.Username, model.Password);



                            if (authenticationResult.IsSuccess)
                            {
                                return RedirectToLocal(returnUrl);
                            }

                            ModelState.AddModelError("", authenticationResult.ErrorMessage);
                        }
                        else
                        {
                            ViewBag.Message = "User is Inactive..please contact your administrator";
                            return View(loginViewModel);
                        }
                    }
                    else
                    {
                        ViewBag.Message = "User does not exist..please contact your administrator";
                        return View(loginViewModel);
                        //return View("Error");
                    }

                }
                else
                {
                    model.UserTypeId = "2";
                    ViewData["UserTypeID"] = 2;


                    //getting the role id of the person who logs in 
                    var roleId = loginViewModel.GetRoleID(model.Username, model.Password);

                    model.lstActionType = loginViewModel.GetMappedScreenRoles(roleId);

                  
                    //Form Authentication
                    IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;

                    currentUserData = model.CheckUser(model.Username, model.Password);

                    Session["CurrentLoggedInUserDetails"] = currentUserData;

                    if (currentUserData != null)
                    {
                        userflag = true;
                    }

                    //if (model.CheckUser().Any(a => a.userName == model.Username && a.password == model.Password))// && a.Status == true))
                    if (userflag)
                    {
                        if (currentUserData.Active == "1")
                        {
                            var authService = new AuthenticationService(authenticationManager);
                            authService.SignInExternalUser(model.Username, model.Password);

                            return RedirectToLocal(returnUrl);
                        }
                        else
                        {
                            ViewBag.Message = "User is Inactive..please contact your administrator";
                            return View(loginViewModel);
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Invalid login attempt.";
                        return View(loginViewModel);
                        //ModelState.AddModelError("", "Invalid login attempt.");
                    }
                }

               
                return View(model);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        [ValidateAntiForgeryToken]
        public virtual ActionResult Logoff()
        {
            try
            {


                IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                authenticationManager.SignOut(MyAuthentication.ApplicationCookie);
                Session.Remove("CurrentLoggedInUserDetails");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }


        private ActionResult RedirectToLocal(string returnUrl)
        {
            try
            {


                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Client");
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }
    }


    public class LoginViewModel
    {
        
        [Required, AllowHtml]
        public string Username { get; set; }

        [Required]
        [AllowHtml]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public int roleID { get; set; }

        public string UserTypeId { get; set; }
        public List<UserType> UserTypes { get; set; }
        public IList<tbl_UserData> lstUserData { get; set; }
        public IList<tbl_RoleScreenMapping> lstActionType { get; set; }

        public int GetRoleID(string userName, string password)
        {
            try
            {


                UserManager userManager = new UserManager();
                int roleId = userManager.FindRoleID(userName, password);

                return roleId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<tbl_RoleScreenMapping> GetMappedScreenRoles(int roleId)
        {
            try
            {


                UserManager userManager = new UserManager();
                lstActionType = userManager.GetMappedIDs(roleId);

                return lstActionType;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<UserModel> getUsers()
        {
            try
            {


                IList<UserModel> users = new List<UserModel>();
                // UserManager userManager = new UserManager();
                //lstUserData = new List<tbl_UserData>();
                
                //lstUserData = userManager.GetUserDetails();

                users.Add(new UserModel() { userId = 1, userName = "admin", Status = true });
                users.Add(new UserModel() { userId = 2, userName = "invite", Status = false });

                return users.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_UserData CheckUser(string UserName)
        {
            try
            {


                //bool bCheck = false;
                UserManager userManager = new UserManager();                

                tbl_UserData userdata = userManager.FindUserName(UserName);
                
                return userdata;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_UserData CheckUser(string UserName, string Password)
        {
            try
            {
               // bool bCheck = false;
                UserManager userManager = new UserManager();

                tbl_UserData userdata = userManager.FindUserName(UserName, Password);

                //if (check != null)
                //{
                //    bCheck = true;
                //}

                return userdata;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }

    }
    public class UserType
    {
        public string ID { get; set; }
        public string Type { get; set; }
    }
}