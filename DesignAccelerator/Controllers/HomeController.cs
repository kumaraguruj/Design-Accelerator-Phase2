using DA.DomainModel;
using DesignAccelerator.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DesignAccelerator.Controllers
{
    public class HomeController : Controller
    {
        ErrorLogViewModel errorlogviewmodel;
        [NoDirectAccess]
        public ActionResult Index(int? id)
        {
            try
            {


                DAViewModel daViewModel = new DAViewModel();
                CommonFunctions comfuns = new CommonFunctions();

                var da = comfuns.FindDA((int)id);

                daViewModel.DAID = (int)id;
                daViewModel.ModuleId = da.ModuleId;
                daViewModel.DAName = da.DAName;


                return View(daViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        [NoDirectAccess]
        public ActionResult Masters(int? id)
        {
            try
            {


                DAViewModel daViewModel = new DAViewModel();
                CommonFunctions comfuns = new CommonFunctions();

                var da = comfuns.FindDA((int)id);
                daViewModel.DAID = (int)id;
                daViewModel.ApplicationID = da.ApplicationID;
                daViewModel.DAName = da.DAName;

                return View(daViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }
        [NoDirectAccess]
        public ActionResult MapAttributes(int? id)
        {
            try
            {


                DAViewModel daViewModel = new DAViewModel();
                CommonFunctions comfuns = new CommonFunctions();

                var da = comfuns.FindDA((int)id);
                daViewModel.DAID = (int)id;
                daViewModel.ApplicationID = da.ApplicationID;
                daViewModel.DAName = da.DAName;

                return View(daViewModel);
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