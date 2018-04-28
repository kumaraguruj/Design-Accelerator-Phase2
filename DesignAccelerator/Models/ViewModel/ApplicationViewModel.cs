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
    public class ApplicationViewModel
    {
        #region Public Properties
        public int ApplicationID { get; set; }

        [Required(ErrorMessage = "Application name is required")]
        [RegularExpression(@"^[a-zA-Z0-9_ ]*$", ErrorMessage = "Special Characters are not allowed in this field")]
        public string ApplicationName { get; set; }

        public int DocVersion { get; set; }
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }

        public int AppVersionId { get; set; }       
        public string AppVersion { get; set; }

        public int BankType { get; set; }
        public string BankTypeName { get; set; }

        public int roleId { get; set; }
        public string RoleName { get; set; }

        public bool AddPermmission = false;
        public bool EdiPermission = false;
        public bool DeletePermission = false;

        public enum BankTypes
        {
            Conventional = 1,
            Islamic = 2            
        }

        public IList<KeyValuePair<string, int>> BankTypeList { get; set; }
        public IList<tbl_AppVersion> lstAppVersion { get; set; }
        public IList<tbl_Applications> lstApplication {get;set;}
        #endregion

        public void GetApplicationDetails(int projectId)
        {
            try
            {
                ApplicationManager applicationManager = new ApplicationManager();
                AppVersionManager appVersionManager = new AppVersionManager();
                lstApplication = new List<tbl_Applications>();
                lstAppVersion = new List<tbl_AppVersion>();
                lstAppVersion = appVersionManager.GetApplVersionDetails();
                lstApplication = applicationManager.GetApplicationDetails(projectId);

                BankTypeList = new List<KeyValuePair<string, int>>();

                BankTypeList = GetEnumList<BankTypes>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddApplication(ApplicationViewModel applicationViewModel)
        {
            try
            {
                tbl_Applications tblApplication = new tbl_Applications();
                tblApplication.ApplicationName = applicationViewModel.ApplicationName;
                tblApplication.ProjectId = applicationViewModel.ProjectID;
                tblApplication.AppVersion = applicationViewModel.AppVersionId;
                tblApplication.BankType = applicationViewModel.BankType;

                tblApplication.EntityState = DA.DomainModel.EntityState.Added;

                ApplicationManager applicationManager = new ApplicationManager();
                applicationManager.AddApplication(tblApplication);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public void UpdateApplication(ApplicationViewModel applicationViewModel)
        {
            try
            {
                tbl_Applications tblApplication = new tbl_Applications();
                tblApplication.ApplicationID = applicationViewModel.ApplicationID;
                tblApplication.ApplicationName = applicationViewModel.ApplicationName;
                tblApplication.ProjectId = applicationViewModel.ProjectID;
                tblApplication.AppVersion = applicationViewModel.AppVersionId;
                tblApplication.BankType = applicationViewModel.BankType;
                tblApplication.EntityState = DA.DomainModel.EntityState.Modified;

                ApplicationManager applicationManager = new ApplicationManager();
                applicationManager.UpdateApplication(tblApplication);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteApplication(ApplicationViewModel applicationViewModel)
        {
            try
            {
                tbl_Applications tblApplication = new tbl_Applications();
                tblApplication.ApplicationID = applicationViewModel.ApplicationID;
                tblApplication.EntityState = DA.DomainModel.EntityState.Deleted;

                ApplicationManager applicationManager = new ApplicationManager();
                applicationManager.DeleteApplication(tblApplication);
            }
            catch(Exception)
            {
                throw;
            }          
        }
        
        public List<KeyValuePair<string, int>> GetEnumList<T>()
        {
            try
            {
                var list = new List<KeyValuePair<string, int>>();
                foreach (var e in Enum.GetValues(typeof(T)))
                {
                    list.Add(new KeyValuePair<string, int>(e.ToString(), (int)e));
                }
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ApplicationViewModel FindApplication(int? applicationID)
        {
            try
            {
                ApplicationViewModel applicationViewModel = new ApplicationViewModel();
                ApplicationManager applicationManager = new ApplicationManager();
                var apps1 = applicationManager.FindApplication(applicationID);

                AppVersionManager appManager = new AppVersionManager();
                applicationViewModel.lstAppVersion = appManager.GetApplVersionDetails();

                applicationViewModel.ApplicationID = apps1.ApplicationID;
                applicationViewModel.ApplicationName = apps1.ApplicationName;
                applicationViewModel.AppVersionId = (int)apps1.AppVersion;
                applicationViewModel.AppVersion = applicationViewModel.lstAppVersion.Where(e => e.Id.Equals((int)apps1.AppVersion)).First().AppVersion;
                applicationViewModel.BankTypeList = GetEnumList<BankTypes>();
                applicationViewModel.BankType = (int)apps1.BankType;
                applicationViewModel.BankTypeName = applicationViewModel.BankTypeList.Where(e => e.Value.Equals((int)apps1.BankType)).First().Key;
                applicationViewModel.ProjectID = apps1.ProjectId;

                return applicationViewModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckDuplicate(ApplicationViewModel appViewModel)
        {
            try
            {
                ApplicationManager appManager = new ApplicationManager();

                var application = appManager.FindApplicationName(appViewModel.ApplicationName, appViewModel.AppVersionId, appViewModel.BankType, appViewModel.ProjectID);

                if (application != null && application.ApplicationID != appViewModel.ApplicationID && application.ApplicationName.ToUpper() == appViewModel.ApplicationName.ToUpper()
                    && application.AppVersion == appViewModel.AppVersionId && application.BankType == appViewModel.BankType)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<tbl_AppVersion> GetVersions(ApplicationViewModel applicationViewModel)
        {
            try
            {
                AppVersionManager appManager = new AppVersionManager();
                applicationViewModel.lstAppVersion = appManager.GetApplVersionDetails();
                return applicationViewModel.lstAppVersion;
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