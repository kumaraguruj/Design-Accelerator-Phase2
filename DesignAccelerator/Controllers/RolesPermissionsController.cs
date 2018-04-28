using DesignAccelerator.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DesignAccelerator.Controllers
{
    public class RolesPermissionsController : Controller
    {
        ErrorLogViewModel errorlogviewmodel;
        // GET: RolesPermissions
        public ActionResult Index()
        {
            try
            {


                RolesActionsViewModel rolesActionsVM = new RolesActionsViewModel();
                ViewData["ScreenList"] = rolesActionsVM.GetAllScreens();

                rolesActionsVM.GetAllRoles();

                ViewData["ActionType"] = rolesActionsVM.LstActionType();

                ViewData["MappedScreensRoles"] = rolesActionsVM.GetMappedScreensRoles();

                rolesActionsVM.GetMappedScreensRoles();

                return View(rolesActionsVM);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }

        }

        [HttpPost]
        public ActionResult SaveData(IList<RolesActionsViewModel> roleActionsViewModel)
        {
            try
            {


                int index = 0;
                String result = String.Empty;
                RolesActionsViewModel rolesActionsVM = new RolesActionsViewModel();
                index = rolesActionsVM.SaveData(roleActionsViewModel);
                if (index > 0)
                {
                    result = "1";
                }
                else
                    result = "0";

                // ModelState.Clear();
                return Json(result, JsonRequestBehavior.AllowGet);
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