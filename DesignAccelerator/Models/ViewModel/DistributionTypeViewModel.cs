using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DA.BusinessLayer;
using DA.DomainModel;

namespace DesignAccelerator.Models.ViewModel
{
    public class DistributionTypeViewModel
    {
        #region Public Properties
        public int DAID { get; set; }
        public int distributionTypeID { get; set; }

        [Required(ErrorMessage = "Distribution Type is required")]
        [RegularExpression(@"^[a-zA-Z0-9_ ]*$", ErrorMessage = "Special Characters are not allowed in this field")]
        public string distributionDesc { get; set; }

        public string DAName { get; set; }
        public int ApplicationID { get; set; }
        public string ApplicationName { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int ModuleId { get; set; }

        public int roleId { get; set; }
        public string RoleName { get; set; }

        public bool AddPermmission = false;
        public bool EdiPermission = false;
        public bool DeletePermission = false;

        public IList<DistributionTypeViewModel> DistributionTypeList { get; set; }
        #endregion

        public void AddDistributionType(DistributionTypeViewModel distributionTypeViewModel)
        {
            try
            {
                tbl_DistributionType tblDistributionType = new tbl_DistributionType();

                tblDistributionType.DistributionDesc = distributionTypeViewModel.distributionDesc;
                tblDistributionType.daId = distributionTypeViewModel.DAID;//1;

                tblDistributionType.EntityState = DA.DomainModel.EntityState.Added;

                DistributionTypeManager distributionTypeManager = new DistributionTypeManager();
                distributionTypeManager.AddDistributionType(tblDistributionType);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void DeleteDistributionType(DistributionTypeViewModel distributionTypeViewModel)
        {
            try
            {
                tbl_DistributionType tblDistributionType = new tbl_DistributionType();
                tblDistributionType.daId = distributionTypeViewModel.DAID;
                tblDistributionType.DistributionTypeID = distributionTypeViewModel.distributionTypeID;
                tblDistributionType.EntityState = DA.DomainModel.EntityState.Deleted;

                DistributionTypeManager distributionTypeManager = new DistributionTypeManager();
                distributionTypeManager.DeleteDistributionType(tblDistributionType);
            }
            catch (Exception)
            {

                throw;
            }


        }

        public void UpdateDistributionType(DistributionTypeViewModel distributionTypeViewModel)
        {
            try
            {
                tbl_DistributionType tblDistributionType = new tbl_DistributionType();

                tblDistributionType.DistributionTypeID = distributionTypeViewModel.distributionTypeID;
                tblDistributionType.DistributionDesc = distributionTypeViewModel.distributionDesc;
                tblDistributionType.daId = distributionTypeViewModel.DAID;// 1;
                tblDistributionType.EntityState = DA.DomainModel.EntityState.Modified;

                DistributionTypeManager distributionTypeManager = new DistributionTypeManager();
                distributionTypeManager.UpdateDistributionType(tblDistributionType);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void GetDistributionTypeDetails(int daId)
        {
            try
            {
                DistributionTypeManager distributionTypeManager = new DistributionTypeManager();
                var distributionTypeList = distributionTypeManager.GetDistributionTypeDetails(daId);

                DistributionTypeList = new List<DistributionTypeViewModel>();
                foreach (var item in distributionTypeList)
                {
                    DistributionTypeViewModel distributionTypeViewModel = new DistributionTypeViewModel();
                    distributionTypeViewModel.distributionTypeID = item.DistributionTypeID;
                    distributionTypeViewModel.distributionDesc = item.DistributionDesc;
                    distributionTypeViewModel.DAID = item.daId;//1;

                    DistributionTypeList.Add(distributionTypeViewModel);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void GetDAName(int daId, out int appId, out string daName)
        {
            try
            {
                tbl_DesignAccelerator tblDA = new tbl_DesignAccelerator();
                DAManager daManager = new DAManager();
                tblDA = daManager.FindDA(daId);
                appId = (int)tblDA.ModuleId;
                daName = tblDA.daName;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public DistributionTypeViewModel FindDistributionType(int? distributionTypeID)
        {
            try
            {
                DistributionTypeViewModel distributionTypeViewModel = new DistributionTypeViewModel();

                DistributionTypeManager distributionTypeManager = new DistributionTypeManager();
                var distributionType = distributionTypeManager.FindDistributionType(distributionTypeID);

                distributionTypeViewModel.distributionTypeID = distributionType.DistributionTypeID;
                distributionTypeViewModel.distributionDesc = distributionType.DistributionDesc;
                distributionTypeViewModel.DAID = distributionType.daId;//1;

                return distributionTypeViewModel;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool CheckDuplicate(DistributionTypeViewModel distributionTypeVM)
        {
            try
            {
                DistributionTypeManager distTypeManager = new DistributionTypeManager();

                var distribution = distTypeManager.FindDistributionTypeDesc(distributionTypeVM.distributionDesc, distributionTypeVM.DAID);

                if (distribution != null && distribution.DistributionTypeID != distributionTypeVM.distributionTypeID
                    && distribution.DistributionDesc.ToUpper() == distributionTypeVM.distributionDesc.ToUpper())
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
