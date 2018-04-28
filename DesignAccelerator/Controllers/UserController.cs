using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using DesignAccelerator.Models.ViewModel;
using System.Data.Entity.Infrastructure;

namespace DesignAccelerator.Controllers
{
    public class UserController : Controller
    {
        ErrorLogViewModel errorlogviewmodel;

        // GET: User
        [NoDirectAccess]
        public ActionResult Index()
        {
            try
            {


                UserModel userViewModel = new UserModel();
                userViewModel.GetUserDetails();
                userViewModel.GetScreenAccessRights("Create Users");

                ViewData["Userviewmodel1"] = (IEnumerable<UserModel>)from u in userViewModel.lstUserData
                                                                    join m in userViewModel.lstUserData on u.AuthID equals m.UserID
                                                                     join b in userViewModel.lstRoles on u.RoleID equals b.RoleID
                                                                     select new UserModel { userId = u.UserID, userName = u.UserName, AuthName=m.UserName, password = u.Password, CreatedDate = u.CreatedDate, Rolename = b.RoleName, Status = Convert.ToBoolean(Convert.ToInt32(u.Active)) };

                //List<UserModel> userList = ViewData["Userviewmodel1"] as List<UserModel>;

                userViewModel.UserTypes = new List<UserType>
            {
                new UserType{ID ="1", Type = "Internal"},
                new UserType{ID ="2", Type = "External"}
            };


                return View(userViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");

            }
        }

        [HttpPost]
        public ActionResult Index(UserModel userModel)
        {
            try
            {
                userModel.UserTypes = new List<UserType>
                {
                    new UserType{ID ="1", Type = "Internal"},
                    new UserType{ID ="2", Type = "External"}
                };

                if (ModelState.IsValid)
                {
                    bool isDuplicate = false;
                    userModel.userName = userModel.userName.Trim();
                    isDuplicate = userModel.CheckDuplicate(userModel);

                    if (isDuplicate)
                    {
                        userModel.GetUserDetails();
                        userModel.GetScreenAccessRights("Create Users");
                        ViewBag.Message = "User Already Exists";
                        //return View("Index", userModel);
                    }
                    else
                    {
                        userModel.AddUser(userModel);
                        userModel.GetUserDetails();
                        userModel.GetScreenAccessRights("Create Users");
                        ViewBag.Message = "New User Added Successfully";
                        //ViewData["Userviewmodel1"] = "";
                        ViewData["Userviewmodel1"] = (IEnumerable<UserModel>)from u in userModel.lstUserData
                                                                             join m in userModel.lstUserData on u.AuthID equals m.UserID
                                                                             join b in userModel.lstRoles
                                                                             on u.RoleID equals b.RoleID
                                                                             select new UserModel { userId = u.UserID, userName = u.UserName, AuthName = m.UserName, password = u.Password, CreatedDate = u.CreatedDate, Rolename = b.RoleName, Status = Convert.ToBoolean(Convert.ToInt32(u.Active)) };


                        return View("Index", userModel);
                    }
                }
                userModel.GetUserDetails();
                userModel.GetScreenAccessRights("Create Users");
                ViewData["Userviewmodel1"] = (IEnumerable<UserModel>)from u in userModel.lstUserData
                                                                     join m in userModel.lstUserData on u.AuthID equals m.UserID
                                                                     join b in userModel.lstRoles
                                                                     on u.RoleID equals b.RoleID
                                                                     select new UserModel { userId = u.UserID, userName = u.UserName, AuthName = m.UserName, password = u.Password, CreatedDate = u.CreatedDate, Rolename = b.RoleName, Status = Convert.ToBoolean(Convert.ToInt32(u.Active)) };
                return View(userModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");

            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                UserModel userVMModel = new UserModel();
                userVMModel = userVMModel.FindUser(id);
                TempData["id"] = userVMModel.userId;
                Session["UserOldRoleID"] = userVMModel.RoleId;
                if (userVMModel.userId == 0)
                {
                    return HttpNotFound();
                }
                return View(userVMModel);

            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");

            }
        }

        //Post: User/Edit/5
        [HttpPost]
        public ActionResult Edit(UserModel userVWModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isduplicate = false;
                    userVWModel.userName = userVWModel.userName.Trim();
                    isduplicate = userVWModel.CheckDuplicate(userVWModel);
                    if (isduplicate)
                    {
                        ModelState.AddModelError("UserName", "UserName already exists");
                        userVWModel.GetUserRoles(userVWModel);
                        return View("Edit", userVWModel);
                    }
                    userVWModel.UpdateUser(userVWModel);
                    return RedirectToAction("Index", "User");
                }
                userVWModel.GetUserRoles(userVWModel);
                return View(userVWModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");

            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                UserModel userVMModel = new UserModel();
                userVMModel = userVMModel.FindUser(id);
                TempData["id"] = userVMModel.userId;

                if (userVMModel.userId == 0)
                {
                    return HttpNotFound();
                }
                return View(userVMModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");

            }
        }

        // Post: User/Delete/5
        [HttpPost]
        public ActionResult Delete(UserModel userVWModel)
        {
            try
            {
                    userVWModel.DeleteUser(userVWModel);
                    return RedirectToAction("Index", "User");
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

    }
}