using DA.BusinessLayer;
using DA.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DesignAccelerator.Models.ViewModel
{
    public class RolesActionsViewModel
    {
        #region Public properties
        public int screenID { get; set; }
        public string screenName { get; set; }
        public string purpose { get; set; }
        public string userType { get; set; }
        public string actionType { get; set; }

        public int roleID { get; set; }
        public string roleName { get; set; }
        public string columnAdd { get; set; }


        public IList<RolesActionsViewModel> RolesList { get; set; }
        public IList<RolesActionsViewModel> ScreenList { get; set; }
        public IList<RolesActionsViewModel> MappedScreensRoles { get; set; }
        #endregion

        public IList<string> LstActionType()
        {
            try
            {
                IList<string> actionTypes = new List<string>();

                actionTypes.Add("Add");
                actionTypes.Add("Edit");
                actionTypes.Add("Delete");
                actionTypes.Add("View");

                return actionTypes;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public IList<RolesActionsViewModel> GetMappedScreensRoles()
        {
            try
            {
                RolesActionsManager rolesActionManager = new RolesActionsManager();
                var lstMappedScreensRoles = rolesActionManager.GetMappedScreenRoles();
                MappedScreensRoles = new List<RolesActionsViewModel>();
                foreach (var item in lstMappedScreensRoles)
                {
                    RolesActionsViewModel rolesActionsVM = new RolesActionsViewModel();
                    rolesActionsVM.actionType = item.ActionType;
                    rolesActionsVM.screenID = Convert.ToInt32(item.ScreenID);
                    rolesActionsVM.roleID = Convert.ToInt32(item.RoleID);

                    MappedScreensRoles.Add(rolesActionsVM);
                }

                return MappedScreensRoles;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void GetAllRoles()
        {
            try
            {
                RolesActionsManager rolesActionManager = new RolesActionsManager();
                IList<tbl_Roles> lstRoles = rolesActionManager.GetAllRoles();
                RolesList = new List<RolesActionsViewModel>();
                foreach (var item in lstRoles)
                {
                    RolesActionsViewModel rolesPermissionsViewModel = new RolesActionsViewModel();
                    rolesPermissionsViewModel.roleID = item.RoleID;
                    rolesPermissionsViewModel.roleName = item.RoleName;

                    RolesList.Add(rolesPermissionsViewModel);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public IList<RolesActionsViewModel> GetAllScreens()
        {
            try
            {
                RolesActionsManager rolesActionManager = new RolesActionsManager();
                var screenList = rolesActionManager.GetAllScreens();
                ScreenList = new List<RolesActionsViewModel>();
                foreach (var item in screenList)
                {
                    RolesActionsViewModel rolesPermissionsViewModel = new RolesActionsViewModel();

                    rolesPermissionsViewModel.screenID = item.Screen_ID;
                    rolesPermissionsViewModel.screenName = item.ScreenName;

                    ScreenList.Add(rolesPermissionsViewModel);
                }
                return ScreenList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int SaveData(IList<RolesActionsViewModel> rolesPermissions)
        {
            try
            {
                int result = 0;
                RolesActionsManager rolesActionManager = new RolesActionsManager();
                var screenList = rolesActionManager.GetMappedScreenRoles();

                foreach (var item in rolesPermissions)
                {
                    tbl_RoleScreenMapping tblRolesScreenMapping = new tbl_RoleScreenMapping();

                    var existingMappedScreen = screenList.Where(e => e.RoleID.Equals(item.roleID) && e.ScreenID.Equals(item.screenID) && e.ActionType.Equals(item.actionType));

                    //update
                    if (existingMappedScreen.Count() != 0)
                    {
                        foreach (var scrnRoles in existingMappedScreen.ToList())
                        {
                            if (scrnRoles.RoleScreenMappingID > 0)
                            {
                                tblRolesScreenMapping.RoleScreenMappingID = scrnRoles.RoleScreenMappingID;
                                tblRolesScreenMapping.ScreenID = scrnRoles.ScreenID;
                                tblRolesScreenMapping.RoleID = scrnRoles.RoleID;
                                tblRolesScreenMapping.ActionType = scrnRoles.ActionType;

                                tblRolesScreenMapping.EntityState = DA.DomainModel.EntityState.Modified;
                            }
                            screenList.Add(tblRolesScreenMapping);

                        }
                    }
                    //Add new data
                    else
                    {
                        if (item.actionType != null)
                        {
                            tblRolesScreenMapping.RoleID = item.roleID;
                            tblRolesScreenMapping.ScreenID = item.screenID;
                            tblRolesScreenMapping.ActionType = item.actionType;
                            tblRolesScreenMapping.EntityState = DA.DomainModel.EntityState.Added;

                            screenList.Add(tblRolesScreenMapping);
                        }
                        else
                        {
                            switch (item.columnAdd)
                            {
                                case "ColumnAdd":
                                    {
                                        tblRolesScreenMapping.ActionType = "Add";
                                        var getAddactionType = screenList.Where(e => e.RoleID.Equals(item.roleID) && e.ScreenID.Equals(item.screenID) && e.ActionType.Equals("Add"));

                                        foreach (var item1 in getAddactionType)
                                        {
                                            tblRolesScreenMapping.RoleScreenMappingID = item1.RoleScreenMappingID;
                                            tblRolesScreenMapping.RoleID = item1.RoleID;
                                            tblRolesScreenMapping.ScreenID = item1.ScreenID;
                                            tblRolesScreenMapping.EntityState = DA.DomainModel.EntityState.Deleted;
                                            rolesActionManager.DeleteMappedScreenRole(tblRolesScreenMapping);
                                        }

                                    }
                                    break;
                                case "ColumnEdit":
                                    {
                                        tblRolesScreenMapping.ActionType = "Edit";
                                        var getEditactionType = screenList.Where(e => e.RoleID.Equals(item.roleID) && e.ScreenID.Equals(item.screenID) && e.ActionType.Equals("Edit"));
                                        foreach (var item1 in getEditactionType)
                                        {
                                            tblRolesScreenMapping.RoleScreenMappingID = item1.RoleScreenMappingID;
                                            tblRolesScreenMapping.RoleID = item1.RoleID;
                                            tblRolesScreenMapping.ScreenID = item1.ScreenID;
                                            tblRolesScreenMapping.EntityState = DA.DomainModel.EntityState.Deleted;
                                            rolesActionManager.DeleteMappedScreenRole(tblRolesScreenMapping);
                                        }
                                    }
                                    break;
                                case "ColumnDelete":
                                    {
                                        tblRolesScreenMapping.ActionType = "Delete";
                                        var getEditactionType = screenList.Where(e => e.RoleID.Equals(item.roleID) && e.ScreenID.Equals(item.screenID) && e.ActionType.Equals("Delete"));
                                        foreach (var item1 in getEditactionType)
                                        {
                                            tblRolesScreenMapping.RoleScreenMappingID = item1.RoleScreenMappingID;
                                            tblRolesScreenMapping.RoleID = item1.RoleID;
                                            tblRolesScreenMapping.ScreenID = item1.ScreenID;
                                            tblRolesScreenMapping.EntityState = DA.DomainModel.EntityState.Deleted;
                                            rolesActionManager.DeleteMappedScreenRole(tblRolesScreenMapping);
                                        }
                                    }
                                    break;
                                case "ColumnView":
                                    {
                                        tblRolesScreenMapping.ActionType = "View";
                                        var getEditactionType = screenList.Where(e => e.RoleID.Equals(item.roleID) && e.ScreenID.Equals(item.screenID) && e.ActionType.Equals("View"));
                                        foreach (var item1 in getEditactionType)
                                        {
                                            tblRolesScreenMapping.RoleScreenMappingID = item1.RoleScreenMappingID;
                                            tblRolesScreenMapping.RoleID = item1.RoleID;
                                            tblRolesScreenMapping.ScreenID = item1.ScreenID;
                                            tblRolesScreenMapping.EntityState = DA.DomainModel.EntityState.Deleted;
                                            rolesActionManager.DeleteMappedScreenRole(tblRolesScreenMapping);
                                        }
                                    }
                                    break;
                            }

                        }
                    }
                }
                result = rolesActionManager.SaveData(screenList);
                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}