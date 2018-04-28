using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DesignAccelerator.Models.ViewModel;
using System.Net;
using DA.DomainModel;
using DA.BusinessLayer;

namespace DesignAccelerator.Controllers
{
    public class AttributeValueController : Controller
    {
        ErrorLogViewModel errorlogviewmodel;
        //// GET: AttributeValueList
        //[HttpGet]
        [NoDirectAccess]
        public ActionResult Index(int? id)
        {
            try
            {
                string urlPart = @"/AttributeList/";
                if (System.Web.HttpContext.Current.Request.UrlReferrer.ToString().IndexOf(urlPart) > 0)
                    Session["PreviousURL"] = System.Web.HttpContext.Current.Request.UrlReferrer;

                AttributeValueViewModel attribValVM = new AttributeValueViewModel();

                if (id == null)
                    id = (int)TempData["daId"];
                attribValVM.GetAttributeList(id);
                //attribValVM.GetAttributeValues(id);

                CommonFunctions comfuns = new CommonFunctions();

                var da = comfuns.FindDA((int)id);
                attribValVM.DaId = (int)id;
                attribValVM.daName = da.DAName;
                attribValVM.ModuleId = da.ModuleId;
                TempData["daId"] = attribValVM.DaId;

                return View(attribValVM);
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
        public ActionResult Index(AttributeValueViewModel attribValviewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    attribValviewmodel.AttributeDesc = attribValviewmodel.AttributeDesc.Trim();

                    AttributeValueViewModel attribValVM = new AttributeValueViewModel();
                    TempData["daId"] = attribValVM.DaId;

                    return RedirectToAction("Index", "AttributeValue");
                }

                return View(attribValviewmodel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetValues(int attributeId)
        {
            try
            {
                AttributeValueViewModel attribValVM = new AttributeValueViewModel();
                attribValVM.GetAttributeValues(attributeId);
                return Json(attribValVM.LstAttribVal, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }




        [HttpPost]
        public ActionResult SaveData(IList<AttributeValueViewModel> attributeValueViewModel)
        {
            try
            {

                int index = 0;
                String result = String.Empty;
                AttributeValueViewModel attribValVM = new AttributeValueViewModel();

                index = attribValVM.SaveData(attributeValueViewModel);
                if (index > 0)
                {
                    result = "1";
                }
                else
                    result = "0";

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






















//public ActionResult GetAttributeValues1(int? id)
//{

//    AttributeValueManager attribValManager = new AttributeValueManager();
//    var attrib = attribValManager.GetAttributeValList(id);
//    if (attrib.Count > 0)
//    {
//        var user = attrib.FirstOrDefault();
//        attribValVM.AttributeValue1 = user.AttributeValue;
//        attribValVM.AttributeValue2 = user.AttributeValue;
//        attribValVM.AttributeValue3 = user.AttributeValue;
//        attribValVM.AttributeValue4 = user.AttributeValue;
//        attribValVM.AttributeValue5 = user.AttributeValue;
//        attribValVM.AttributeValue6 = user.AttributeValue;
//        attribValVM.AttributeValue7 = user.AttributeValue;
//        attribValVM.AttributeValue8 = user.AttributeValue;
//        attribValVM.AttributeValue9 = user.AttributeValue;
//        attribValVM.AttributeValue10 = user.AttributeValue;
//        attribValVM.AttributeValue11 = user.AttributeValue;
//        attribValVM.AttributeValue12 = user.AttributeValue;
//        attribValVM.AttributeValue13 = user.AttributeValue;
//        attribValVM.AttributeValue14 = user.AttributeValue;
//        attribValVM.AttributeValue15 = user.AttributeValue;
//    }
//    else
//    //No Records Found
//    {
//        attribValVM.AttributeValue1 = string.Empty;
//        attribValVM.AttributeValue2 = string.Empty;
//        attribValVM.AttributeValue3 = string.Empty;
//        attribValVM.AttributeValue4 = string.Empty;
//        attribValVM.AttributeValue5 = string.Empty;
//        attribValVM.AttributeValue6 = string.Empty;
//        attribValVM.AttributeValue7 = string.Empty;
//        attribValVM.AttributeValue8 = string.Empty;
//        attribValVM.AttributeValue9 = string.Empty;
//        attribValVM.AttributeValue10 = string.Empty;
//        attribValVM.AttributeValue11 = string.Empty;
//        attribValVM.AttributeValue12 = string.Empty;
//        attribValVM.AttributeValue13 = string.Empty;
//        attribValVM.AttributeValue14 = string.Empty;
//        attribValVM.AttributeValue15 = string.Empty;
//    }

//    //attribValVM.GetAttributeValues(id);
//    //return RedirectToAction("Index", "AttributeValue");
//    return View(attribValVM);
//}





//public IEnumerable<AttributeValueViewModel> GetValVM()
//{
//    var user = from u in attribValVM.lstTransactions
//    return ();
//}

//CommonFunctions comfuns = new CommonFunctions();

//var da = comfuns.FindDA((int)id);
//attribValVM.daId = (int)id;
//attribValVM.ApplicationID = da.ApplicationID;
//attribValVM.daName = da.DAName;
//TempData["daId"] = attribValVM.daId;

//int projectId;
//int clientId;
//int applicationId;
//string projectName;
//string appName;
//string modName;

//comfuns.GetModuleName(da.ModuleId, out applicationId, out modName);
//attribValVM.ApplicationID = applicationId;
//attribValVM.ModuleName = modName;

//comfuns.GetApplicationName(applicationId, out projectId, out appName);
//attribValVM.ProjectID = projectId;
//attribValVM.ApplicationName = appName;

//ApplicationViewModel applicationViewModel = new ApplicationViewModel();
//comfuns.GetProjectName(projectId, out clientId, out projectName);
//attribValVM.ClientID = clientId;
//attribValVM.ProjectName = projectName;

//ProjectViewModel projectViewModel = new ProjectViewModel();
//attribValVM.ClientName = comfuns.GetClientName(clientId);


//List<AttributeListViewModel> lstAVM = new List<AttributeListViewModel>()
//{

//};