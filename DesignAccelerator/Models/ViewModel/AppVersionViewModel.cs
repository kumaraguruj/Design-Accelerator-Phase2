using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DA.DomainModel;
using DA.BusinessLayer;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DesignAccelerator.Models.ViewModel
{
    public class AppVersionViewModel
    {
        public int ApplicationID { get; set; }
        public string ApplicationName { get; set; }

        public int DocVersion { get; set; }

        public int ProjectID { get; set; }
        public string ProjectName { get; set; }

        public IList<AppVersionViewModel> AppVersionList { get; set; }
        public IList<tbl_AppVersion> lstAppVersion { get; set; }

        [Required(ErrorMessage = "Application Version is required")]
        [RegularExpression(@"^[a-zA-Z0-9_ ]*$", ErrorMessage = "Special Characters are not allowed in this field")]
        public string AppVersion { get; set; }
        public int Id { get; set; }

        public int ClientId { get; set; }
        public string ClientName { get; set; }

        public int roleId { get; set; }
        public string RoleName { get; set; }

        public bool AddPermmission = false;
        public bool EdiPermission = false;
        public bool DeletePermission = false;

        public void AddAppVersion(AppVersionViewModel appVersionViewModel)
        {
            try
            {
                tbl_AppVersion tblAppVersion = new tbl_AppVersion();

                tblAppVersion.AppVersion = appVersionViewModel.AppVersion;
                tblAppVersion.EntityState = DA.DomainModel.EntityState.Added;

                AppVersionManager appVersionManager = new AppVersionManager();
                appVersionManager.AddAppVersion(tblAppVersion);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void DeleteAppVersion(AppVersionViewModel appVersionViewModel)
        {
            try
            {
                tbl_AppVersion tblAppVersion = new tbl_AppVersion();
                tblAppVersion.Id = appVersionViewModel.Id;
                tblAppVersion.EntityState = DA.DomainModel.EntityState.Deleted;

                AppVersionManager appversionManager = new AppVersionManager();
                appversionManager.DeleteAppVersion(tblAppVersion);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void UpdateAppVersion(AppVersionViewModel appVersionViewModel)
        {
            try
            {
                tbl_AppVersion tblAppVersion = new tbl_AppVersion();
                tblAppVersion.Id = appVersionViewModel.Id;
                tblAppVersion.AppVersion = appVersionViewModel.AppVersion;

                tblAppVersion.EntityState = DA.DomainModel.EntityState.Modified;

                AppVersionManager appVersionManager = new AppVersionManager();
                appVersionManager.UpdateAppVersion(tblAppVersion);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void GetAppVersionDetails()
        {
            try
            {
                AppVersionManager appversionManager = new AppVersionManager();
                var appVersionList = appversionManager.GetApplVersionDetails();

                AppVersionList = new List<AppVersionViewModel>();
                foreach (var item in appVersionList)
                {
                    AppVersionViewModel AppVersion = new AppVersionViewModel();

                    AppVersion.Id = item.Id;
                    AppVersion.AppVersion = item.AppVersion;

                    AppVersionList.Add(AppVersion);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public AppVersionViewModel FindAppVersion(int? Id)
        {
            try
            {
                AppVersionViewModel AppVersionViewModel = new AppVersionViewModel();
                AppVersionManager appversionManager = new AppVersionManager();
                var project = appversionManager.FindAppVersion(Id);
                AppVersionViewModel.AppVersion = project.AppVersion;
                return AppVersionViewModel;
            }
            catch (Exception)
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

        //public bool CheckDuplicate(AppVersionViewModel appVersionmodel)
        //{
        //    AppVersionManager appVerManager = new AppVersionManager();

        //    var appVersion = appVerManager.FindVersionName(appVersionmodel.AppVersion);

        //    if (appVersion != null && appVersion.AppVersion != appVersion.AppVersion)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
    }
}