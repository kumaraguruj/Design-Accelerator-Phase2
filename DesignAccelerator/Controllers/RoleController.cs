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
    public class RoleController : Controller
    {
        ErrorLogViewModel errorlogviewmodel;
        // GET: Role
        [NoDirectAccess]
        public ActionResult Index()
        {
            try
            {


                RoleViewModel roleViewModel = new RoleViewModel();
                roleViewModel.GetRoleDetails();

                ViewData["Roleviewmodel1"] = (from u in roleViewModel.lstRoles
                                              select new RoleViewModel { roleId = u.RoleID, rolename = u.RoleName, status = Convert.ToBoolean(u.Active) });

                //List<RoleViewModel> roleList = ViewData["Roleviewmodel1"] as List<RoleViewModel>;

                return View(roleViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult Index(RoleViewModel roleViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isDuplicate = false;
                    roleViewModel.rolename = roleViewModel.rolename.Trim();
                    isDuplicate = roleViewModel.CheckDuplicate(roleViewModel);

                    if (isDuplicate)
                    {
                        roleViewModel.GetRoleDetails();
                        ViewBag.Message = "Role Already Exists";

                        return View("Index", roleViewModel);
                    }
                    else
                    {
                        roleViewModel.AddRole(roleViewModel);
                        roleViewModel.GetRoleDetails();
                        ViewBag.Message = "New Role Added Successfully ";

                        ViewData["Roleviewmodel1"] = (from u in roleViewModel.lstRoles
                                                      select new RoleViewModel { rolename = u.RoleName, status = Convert.ToBoolean(u.Active) });

                        return View("Index", roleViewModel);
                    }
                }
                roleViewModel.GetRoleDetails();
                ViewData["Roleviewmodel1"] = (from u in roleViewModel.lstRoles
                                              select new RoleViewModel { roleId = u.RoleID, rolename = u.RoleName, status = Convert.ToBoolean(u.Active) });
                return View("Index", roleViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        //GET: Role/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                RoleViewModel roleViewModel = new RoleViewModel();
                roleViewModel = roleViewModel.FindRole(id);
                TempData["id"] = roleViewModel.roleId;

                if (roleViewModel.roleId == 0)
                {
                    return HttpNotFound();
                }
                return View(roleViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        //Post: Role/Edit/5
        [HttpPost]
        public ActionResult Edit(RoleViewModel roleViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isDuplicate = false;
                    roleViewModel.rolename = roleViewModel.rolename.Trim();
                    isDuplicate = roleViewModel.CheckDuplicate(roleViewModel);
                    if (isDuplicate)
                    {
                        ModelState.AddModelError("RoleName", "RoleName already exists");
                        return View("Edit", roleViewModel);
                    }
                    roleViewModel.UpdateRole(roleViewModel);
                    return RedirectToAction("Index", "Role");
                }
                return View(roleViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        // GET: Role/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                RoleViewModel roleViewModel = new RoleViewModel();
                roleViewModel = roleViewModel.FindRole(id);
                TempData["id"] = roleViewModel.roleId;

                if (roleViewModel.roleId == 0)
                {
                    return HttpNotFound();
                }
                return View(roleViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        // Post: Role/Delete/5
        [HttpPost]
        public ActionResult Delete(RoleViewModel roleViewModel)
        {
            try
            {
                //string cd = "";
                roleViewModel.DeleteRole(roleViewModel);//, out cd
                                                        // ViewBag.Message1 = cd;
                return RedirectToAction("Index", "Role");
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