using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DA.DomainModel;
using DA.BusinessLayer;
using System.ComponentModel.DataAnnotations;

namespace DesignAccelerator.Models.ViewModel
{
    public class RegionViewModel
    {
        #region Public properties
        public int DocVersion { get; set; }

        public int ProjectID { get; set; }

        public IList<RegionViewModel> RegionList { get; set; }

        [Required(ErrorMessage = "Region name required")]
        [RegularExpression(@"^[a-zA-Z0-9_ ]*$", ErrorMessage = "Special Characters are not allowed in this field")]
        public string Region { get; set; }

        public int Id { get; set; }

        public int roleId { get; set; }
        public string RoleName { get; set; }

        public bool AddPermmission = false;
        public bool EdiPermission = false;
        public bool DeletePermission = false;

        #endregion

        public void AddRegion(RegionViewModel regionViewModel)
        {
            try
            {
                tbl_Region tblRegion = new tbl_Region();

                tblRegion.Region = regionViewModel.Region;
                tblRegion.EntityState = DA.DomainModel.EntityState.Added;

                RegionManager regionManager = new RegionManager();
                regionManager.AddRegion(tblRegion);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void DeleteRegion(RegionViewModel regionViewModel)
        {
            try
            {
                tbl_Region tblRegion = new tbl_Region();
                tblRegion.Id = regionViewModel.Id;
                tblRegion.EntityState = DA.DomainModel.EntityState.Deleted;

                RegionManager regionManager = new RegionManager();
                regionManager.DeleteRegion(tblRegion);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void UpdateRegion(RegionViewModel regionViewModel)
        {
            try
            {
                tbl_Region tblRegion = new tbl_Region();
                tblRegion.Id = regionViewModel.Id;
                tblRegion.Region = regionViewModel.Region;

                tblRegion.EntityState = DA.DomainModel.EntityState.Modified;

                RegionManager regionManager = new RegionManager();
                regionManager.UpdateRegion(tblRegion);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void GetRegionDetails()
        {
            try
            {
                RegionManager regionManager = new RegionManager();
                var regionList = regionManager.GetRegionDetails();

                RegionList = new List<RegionViewModel>();
                foreach (var item in regionList)
                {
                    RegionViewModel Region = new RegionViewModel();

                    Region.Id = item.Id;
                    Region.Region = item.Region;

                    RegionList.Add(Region);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public RegionViewModel FindRegion(int? Id)
        {
            try
            {
                RegionViewModel regionViewModel = new RegionViewModel();
                RegionManager regionManager = new RegionManager();
                var project = regionManager.FindRegion(Id);
                regionViewModel.Region = project.Region;
                return regionViewModel;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool CheckDuplicate(RegionViewModel regionViewModel)
        {
            try
            {
                RegionManager regionManager = new RegionManager();

                var region = regionManager.FindRegionName(regionViewModel.Region);

                if (region != null && region.Id != regionViewModel.Id && region.Region.ToUpper() == regionViewModel.Region.ToUpper())
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