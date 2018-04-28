using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DesignAccelerator.Models.ViewModel;
using System.Net;

namespace DesignAccelerator.Controllers
{
    public class AttributeListController : Controller
    {
        ErrorLogViewModel errorlogviewmodel;
        //// GET: AttributeList
        //[HttpGet]
        [NoDirectAccess]
        public ActionResult Index(int? id)
        {
            try
            {
                AttributeListViewModel attributeListViewModel = new AttributeListViewModel();
                if (id == null)
                    id = (int)TempData["daId"];
                attributeListViewModel.GetAttribute(id);
                attributeListViewModel.GetScreenAccessRights("Attribute List Input");
                CommonFunctions comfuns = new CommonFunctions();

                var da = comfuns.FindDA((int)id);
                attributeListViewModel.daId = (int)id;
                attributeListViewModel.ModuleId = da.ModuleId;
                attributeListViewModel.daName = da.DAName;
                TempData["daId"] = attributeListViewModel.daId;

                int projectId;
                int clientId;
                int applicationId;
                string projectName;
                string appName;
                string modName;

                comfuns.GetModuleName(da.ModuleId, out applicationId, out modName);
                attributeListViewModel.ApplicationID = applicationId;
                attributeListViewModel.ModuleName = modName;

                comfuns.GetApplicationName(applicationId, out projectId, out appName);
                attributeListViewModel.ProjectID = projectId;
                attributeListViewModel.ApplicationName = appName;

                ApplicationViewModel applicationViewModel = new ApplicationViewModel();
                comfuns.GetProjectName(projectId, out clientId, out projectName);
                attributeListViewModel.ClientID = clientId;
                attributeListViewModel.ProjectName = projectName;

                ProjectViewModel projectViewModel = new ProjectViewModel();
                attributeListViewModel.ClientName = comfuns.GetClientName(clientId);

                return View(attributeListViewModel);
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
        public ActionResult Index(AttributeListViewModel attriblistviewmodel)
        {
            try
            {
                AttributeListViewModel attributeListViewModel = new AttributeListViewModel();

                if (ModelState.IsValid)
                {
                    attriblistviewmodel.AttributeDesc = attriblistviewmodel.AttributeDesc.Trim();

                    attriblistviewmodel.AddAttrib(attriblistviewmodel);
                    TempData["daId"] = attriblistviewmodel.daId;
                    attributeListViewModel.GetScreenAccessRights("Attribute List Input");
                    return RedirectToAction("Index", "AttributeList");
                }
                attributeListViewModel.GetAttribute(attriblistviewmodel.daId);
                attributeListViewModel.GetScreenAccessRights("Attribute List Input");
                CommonFunctions comfuns = new CommonFunctions();

                var da = comfuns.FindDA(attriblistviewmodel.daId);
                attributeListViewModel.ModuleId = da.ModuleId;
                attributeListViewModel.daName = da.DAName;
                TempData["daId"] = attributeListViewModel.daId;

                int projectId;
                int clientId;
                int applicationId;
                string projectName;
                string appName;
                string modName;

                comfuns.GetModuleName(da.ModuleId, out applicationId, out modName);
                attributeListViewModel.ApplicationID = applicationId;
                attributeListViewModel.ModuleName = modName;

                comfuns.GetApplicationName(applicationId, out projectId, out appName);
                attributeListViewModel.ProjectID = projectId;
                attributeListViewModel.ApplicationName = appName;

                ApplicationViewModel applicationViewModel = new ApplicationViewModel();
                comfuns.GetProjectName(projectId, out clientId, out projectName);
                attributeListViewModel.ClientID = clientId;
                attributeListViewModel.ProjectName = projectName;

                ProjectViewModel projectViewModel = new ProjectViewModel();
                attributeListViewModel.ClientName = comfuns.GetClientName(clientId);

                return View(attributeListViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }


        //GET: Attribute Edit
        public ActionResult Edit(int? id)
        {
            try
            {


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                AttributeListViewModel attribListVM = new AttributeListViewModel();
                var attrib = attribListVM.FindAttrib(id);
                TempData["daId"] = attrib.daId;

                if (attrib.AttributeID == 0)
                {
                    return HttpNotFound();
                }


                return View(attrib);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        //POST: Attribute Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AttributeListViewModel attriblistviewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    attriblistviewmodel.AttributeDesc = attriblistviewmodel.AttributeDesc.Trim();
                    bool isDuplicate = false;
                    isDuplicate = attriblistviewmodel.CheckDuplicate(attriblistviewmodel);
                    if (isDuplicate)
                    {
                        ModelState.AddModelError("AttributeDesc", "Attribute already exists");
                        return View("Edit");
                    }
                    attriblistviewmodel.GetAttribute(attriblistviewmodel.daId);
                    attriblistviewmodel.UpdateAttrib(attriblistviewmodel);

                    return RedirectToAction("Index", "AttributeList");
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



        //GET: Attribute/Delete/5
        //[HttpGet]
        public ActionResult Delete(int? id)
        {
            try
            {


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                AttributeListViewModel attribListVM = new AttributeListViewModel();
                var attrib = attribListVM.FindAttrib(id);
                TempData["daId"] = attrib.daId;

                if (attrib.AttributeID == 0)
                {
                    return HttpNotFound();
                }
                return View(attrib);
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
        public ActionResult Delete(AttributeListViewModel attribVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //AttributeListViewModel attribListVM = new AttributeListViewModel();
                    attribVM.DeleteAttrib(attribVM);
                    return RedirectToAction("Index", "AttributeList");
                }
                return RedirectToAction("Index", "AttributeList");
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }
    }






























    //[HttpPost]
    //public ActionResult InsertAttribute(AttributeListViewModel objattributeListViewModel)
    //{
    //    objattributeListViewModel. = Convert.ToDateTime(attributeListViewModel.Birthdate);
    //    if (ModelState.IsValid) //checking model is valid or not
    //    {
    //        DataAccessLayer objDB = new DataAccessLayer();
    //        string result = objDB.InsertData(objattributeListViewModel);
    //        ViewData["result"] = result;
    //        ModelState.Clear(); //clearing model
    //        return View();
    //    }
    //    else
    //    {
    //        ModelState.AddModelError("", "Error in saving data");
    //        return View();
    //    }
    //}


    //[HttpPost]
    //public ActionResult Index(AttributeListViewModel attributeListViewModel)
    //{
    //    //attributeListViewModel.attribTypeDesc;

    //    //UserModel obj = new UserModel();
    //    //obj.attribTypeDesc = GetUserList();
    //    //if (ch != null)
    //    //{
    //    //    ViewBag.mes = "You have selected following Attribute Description(s):" + string.Join(", ", CChkbox);
    //    //}
    //    //else
    //    //{
    //    //    ViewBag.mes = "No record selected";
    //    //}
    //    return View();
    //}

}
