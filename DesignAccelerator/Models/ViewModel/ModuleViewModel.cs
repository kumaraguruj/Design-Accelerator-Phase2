using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DA.BusinessLayer;
using DA.DomainModel;

namespace DesignAccelerator.Models.ViewModel
{
    public class ModuleViewModel
    {
        #region Public properties

        public int ModuleId { get; set; }
        [Required(ErrorMessage = "Module name required")]
        [RegularExpression(@"^[a-zA-Z0-9_ ]*$", ErrorMessage = "Special Characters are not allowed in this field")]
        public string ModuleName { get; set; }

        public int daId { get; set; }
        public string daName { get; set; }

        //Flow Implementation
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int ClientID { get; set; }
        public string ClientName { get; set; }
        public int ApplicationID { get; set; }
        public string ApplicationName { get; set; }

        public int roleId { get; set; }
        public string RoleName { get; set; }

        public bool AddPermmission = false;
        public bool EdiPermission = false;
        public bool DeletePermission = false;

        public IList<ModuleViewModel> ModuleList = new List<ModuleViewModel>();

        #endregion

        public void GetModuleDetails(int? appID)
        {
            try
            {
                ModuleManager modManager = new ModuleManager();
                var moduleList = modManager.GetAllModules(appID);
                ModuleList = new List<ModuleViewModel>();
                foreach (var item in moduleList)
                {
                    ModuleViewModel module = new ModuleViewModel();
                    module.ModuleId = item.ModuleID;
                    module.ModuleName = item.ModuleName;

                    ModuleList.Add(module);
                }
            }
            catch(Exception)
            {
                throw;
            }
        }

        public void AddMod(ModuleViewModel modVM)
        {
            try
            {
                tbl_Module tblModule = new tbl_Module();
                tblModule.ApplicationId = modVM.ApplicationID;

                tblModule.ModuleName = modVM.ModuleName;
                tblModule.EntityState = DA.DomainModel.EntityState.Added;

                ModuleManager modManager = new ModuleManager();
                modManager.AddModule(tblModule);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public void UpdateMod(ModuleViewModel modVM)
        {
            try
            {
                tbl_Module tblModule = new tbl_Module();

                tblModule.ModuleID = modVM.ModuleId;
                tblModule.ModuleName = modVM.ModuleName;
                tblModule.ApplicationId = modVM.ApplicationID;
                tblModule.EntityState = DA.DomainModel.EntityState.Modified;

                ModuleManager modManager = new ModuleManager();
                modManager.UpdateModule(tblModule);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public bool DeleteMod(ModuleViewModel modVM)
        {
            try
            {
                tbl_Module tblModule = new tbl_Module();

                tblModule.ModuleID = modVM.ModuleId;
                tblModule.EntityState = DA.DomainModel.EntityState.Deleted;

                ModuleManager modManager = new ModuleManager();
                modManager.DeleteModule(tblModule);

                return true;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public ModuleViewModel FindMod(int modID)
        {
            try
            {
                ModuleViewModel modVM = new ModuleViewModel();
                ModuleManager modManager = new ModuleManager();
                var mod = modManager.FindModule(modID);
                modVM.ModuleId = mod.ModuleID;
                modVM.ModuleName = mod.ModuleName;
                modVM.ApplicationID = (int)mod.ApplicationId;
                return modVM;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public bool CheckDuplicate(ModuleViewModel moduleViewModel)
        {
            try
            {
                ModuleManager moduleManager = new ModuleManager();

                var module = moduleManager.FindModuleName(moduleViewModel.ModuleName, moduleViewModel.ApplicationID);

                if (module != null && module.ModuleID != moduleViewModel.ModuleId && module.ModuleName.ToUpper() == moduleViewModel.ModuleName.ToUpper())
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