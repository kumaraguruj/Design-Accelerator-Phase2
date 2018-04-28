using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DA.DomainModel;
using DA.BusinessLayer;
using System.ComponentModel.DataAnnotations;

namespace DesignAccelerator.Models.ViewModel
{
    public class FrequencyTypeViewModel
    {
        #region Public Properties
        public int FreqTypeID { get; set; }

        [Required(ErrorMessage = "Frequency Type is required")]
        [RegularExpression(@"^[a-zA-Z0-9_ ]*$", ErrorMessage = "Special Characters are not allowed in this field")]
        public string FreqTypeDesc { get; set; }

        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int ClientID { get; set; }
        public string ClientName { get; set; }
        public int ApplicationID { get; set; }
        public string ApplicationName { get; set; }
        public int daid { get; set; }
        public string daName { get; set; }
        public int ModuleId { get; set; }

        public int roleId { get; set; }
        public string RoleName { get; set; }

        public bool AddPermmission = false;
        public bool EdiPermission = false;
        public bool DeletePermission = false;
        public IList<tbl_FrequencyType> lstFreqTypes { get; set; }
        #endregion

        public FrequencyTypeViewModel GetFreqType(int? designAccelaratorID)
        {
            try
            {
                FrequencyTypeViewModel frequencytypeviewmodel = new FrequencyTypeViewModel();
                FrequencyTypeManager lifecycleManager = new FrequencyTypeManager();
                frequencytypeviewmodel.lstFreqTypes = lifecycleManager.GetFrequencyTypes(designAccelaratorID);

                return frequencytypeviewmodel;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void AddFrequencyType(FrequencyTypeViewModel frequencytypeviewmodel)
        {
            try
            {
                tbl_FrequencyType tblfrequencytype = new tbl_FrequencyType();

                tblfrequencytype.FreqTypeDesc = frequencytypeviewmodel.FreqTypeDesc;
                tblfrequencytype.daId = frequencytypeviewmodel.daid;
                tblfrequencytype.EntityState = DA.DomainModel.EntityState.Added;

                FrequencyTypeManager frequencytypeManager = new FrequencyTypeManager();
                frequencytypeManager.AddFrequencyType(tblfrequencytype);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool DeleteFrequencyType(FrequencyTypeViewModel frequencytypeviewmodel)
        {
            try
            {
                tbl_FrequencyType tblfrequencytype = new tbl_FrequencyType();

                tblfrequencytype.FreqTypeID = frequencytypeviewmodel.FreqTypeID;
                tblfrequencytype.EntityState = DA.DomainModel.EntityState.Deleted;

                FrequencyTypeManager frequencytypeManager = new FrequencyTypeManager();
                frequencytypeManager.DeleteFrequencyType(tblfrequencytype);

                return true;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void UpdateFrequencyType(FrequencyTypeViewModel frequencytypeviewmodel)
        {
            try
            {
                tbl_FrequencyType tblfrequencytype = new tbl_FrequencyType();

                tblfrequencytype.FreqTypeID = frequencytypeviewmodel.FreqTypeID;
                tblfrequencytype.FreqTypeDesc = frequencytypeviewmodel.FreqTypeDesc;
                tblfrequencytype.daId = frequencytypeviewmodel.daid;
                tblfrequencytype.EntityState = DA.DomainModel.EntityState.Modified;

                FrequencyTypeManager frequencytypeManager = new FrequencyTypeManager();
                frequencytypeManager.UpdateFrequencyType(tblfrequencytype);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void GetApplicationName(int appId, out int projectId, out string appName)
        {
            try
            {
                tbl_Applications tblApp = new tbl_Applications();
                ApplicationManager appManager = new ApplicationManager();
                tblApp = appManager.FindApplication(appId);
                projectId = tblApp.ProjectId;
                appName = tblApp.ApplicationName;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public FrequencyTypeViewModel FindFrequencyTypes(int? FrequencyTypeID)
        {
            try
            {
                FrequencyTypeViewModel frequencytypevm = new FrequencyTypeViewModel();

                FrequencyTypeManager freqtypeManager = new FrequencyTypeManager();
                var frequencytype = freqtypeManager.FindFrequencyTypes(FrequencyTypeID);
                frequencytypevm.daid = frequencytype.daId;
                frequencytypevm.FreqTypeID = frequencytype.FreqTypeID;
                frequencytypevm.FreqTypeDesc = frequencytype.FreqTypeDesc;

                return frequencytypevm;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool CheckDuplicate(FrequencyTypeViewModel frequencytypevm)
        {
            try
            {
                FrequencyTypeManager freqtypeManager = new FrequencyTypeManager();

                var Frequency = freqtypeManager.FindFrequencyTypeDesc(frequencytypevm.FreqTypeDesc, frequencytypevm.daid);

                if (Frequency != null && Frequency.FreqTypeID != frequencytypevm.FreqTypeID && Frequency.FreqTypeDesc.ToUpper() == frequencytypevm.FreqTypeDesc.ToUpper())
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