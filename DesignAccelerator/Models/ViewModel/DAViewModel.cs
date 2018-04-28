using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DA.BusinessLayer;
using DA.DomainModel;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DesignAccelerator.Models.ViewModel
{
    public class DAViewModel
    {
        #region Public Properties

        public int DAID { get; set; }

        [Required(ErrorMessage = "DA name required")]
        [RegularExpression(@"^[a-zA-Z0-9_. ]*$", ErrorMessage = "Special Characters are not allowed in this field")]
        public string DAName { get; set; }

        public int ApplicationID { get; set; }
        public string ApplicationName { get; set; }

        public int ModuleId { get; set; }
        public string ModuleName { get; set; }

        public string DAComplete { get; set;}

        public IList<DAViewModel> DAList { get; set; }

        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }

        public int roleId { get; set; }
        public string RoleName { get; set; }

        public bool AddPermmission = false;
        public bool EdiPermission = false;
        public bool DeletePermission = false;

        #endregion

        public void AddDA(DAViewModel daViewModel)
        {
            try
            {
                tbl_DesignAccelerator tblDesignAccelerator = new tbl_DesignAccelerator();
                tblDesignAccelerator.daName = daViewModel.DAName;
                tblDesignAccelerator.ModuleId = daViewModel.ModuleId;
                tblDesignAccelerator.dacomplete = "0";
                tblDesignAccelerator.EntityState = DA.DomainModel.EntityState.Added;

                DAManager daManager = new DAManager();
                daManager.AddDA(tblDesignAccelerator);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public void DeleteDA(DAViewModel daViewModel)
        {
            try
            {
                tbl_DesignAccelerator tblDesignAccelerator = new tbl_DesignAccelerator();
                tblDesignAccelerator.daid = daViewModel.DAID;
                tblDesignAccelerator.daName = daViewModel.DAName;
                tblDesignAccelerator.dacomplete = daViewModel.DAComplete;
                tblDesignAccelerator.ModuleId = daViewModel.ModuleId;

                tblDesignAccelerator.EntityState = DA.DomainModel.EntityState.Deleted;

                DAManager daManager = new DAManager();
                daManager.DeleteDA(tblDesignAccelerator);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public void UpdateDA(DAViewModel daViewModel)
        {
            try
            {
                tbl_DesignAccelerator tblDesignAccelerator = new tbl_DesignAccelerator();
                tblDesignAccelerator.daid = daViewModel.DAID;
                tblDesignAccelerator.daName = daViewModel.DAName;
                tblDesignAccelerator.ModuleId = daViewModel.ModuleId;
                tblDesignAccelerator.dacomplete = "0";
                tblDesignAccelerator.EntityState = DA.DomainModel.EntityState.Modified;

                DAManager daManager = new DAManager();
                daManager.UpdateDA(tblDesignAccelerator);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public void GetDADetails(int appId)
        {
            try
            {
                DAManager daManager = new DAManager();

                var dAList = daManager.GetDADetails(appId);

                DAList = new List<DAViewModel>();
                foreach (var item in dAList)
                {
                    DAViewModel dAViewModel = new DAViewModel();

                    dAViewModel.DAID = item.daid;
                    dAViewModel.DAName = item.daName;

                    DAList.Add(dAViewModel);
                }
            }
            catch(Exception)
            {
                throw;
            }
        }

        public DAViewModel FindDA(int? daID)
        {
            try
            {
                DAViewModel dAViewModel = new DAViewModel();
                DAManager daManager = new DAManager();
                var da = daManager.FindDA(daID);
                dAViewModel.DAID = da.daid;
                dAViewModel.DAName = da.daName;
                dAViewModel.ModuleId = (int)da.ModuleId;
                return dAViewModel;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public bool CheckDuplicate(DAViewModel daViewModel)
        {
            try
            {
                DAManager daManager = new DAManager();

                var da = daManager.FindDAName(daViewModel.DAName, daViewModel.ModuleId);

                if (da != null && da.daid != daViewModel.DAID && da.daName.ToUpper() == daViewModel.DAName.ToUpper())
                {
                    return true;
                }
                return false;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public void GetScreenAccessRights(string screenName)
        {
            try
            {
                tbl_UserData currentloggedinuserdata = (tbl_UserData)HttpContext.Current.Session["CurrentLoggedInUserDetails"];
                roleId = currentloggedinuserdata.RoleID;

                RoleManager roleManager = new RoleManager();
                var userrolepermissions = roleManager.GetUserViewAccessPermissions(screenName, roleId);

                foreach (var item in userrolepermissions)
                {
                    if (item.ActionType == "Add")
                        AddPermmission = true;
                    else if (item.ActionType == "Edit")
                        EdiPermission = true;
                    else if (item.ActionType == "Delete")
                        DeletePermission = true;

                    RoleName = item.RoleName;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}