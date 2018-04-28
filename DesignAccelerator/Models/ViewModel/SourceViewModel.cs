using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DA.BusinessLayer;
using DA.DomainModel;
using System.ComponentModel.DataAnnotations;

namespace DesignAccelerator.Models.ViewModel
{
    public class SourceViewModel
    {
        #region Public Properties
        public int DAID { get; set; }
        public string DAName { get; set; }
        public int sourceID { get; set; }

        [Required(ErrorMessage = "Source is required")]
        [RegularExpression(@"^[a-zA-Z0-9_ ]*$", ErrorMessage = "Special Characters are not allowed in this field")]
        public string sourceDesc { get; set; }

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

        public IList<SourceViewModel> SourceList { get; set; }

        #endregion

        public void AddSource(SourceViewModel sourceViewModel)
        {
            try
            {
                tbl_Source tblSource = new tbl_Source();

                tblSource.SourceDesc = sourceViewModel.sourceDesc;
                tblSource.daId = sourceViewModel.DAID;//1

                tblSource.EntityState = DA.DomainModel.EntityState.Added;

                SourceManager sourceManager = new SourceManager();
                sourceManager.AddSource(tblSource);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void DeleteSource(SourceViewModel sourceViewModel)
        {
            try
            {
                tbl_Source tblSource = new tbl_Source();
                tblSource.SourceID = sourceViewModel.sourceID;
                tblSource.daId = sourceViewModel.DAID;
                tblSource.EntityState = DA.DomainModel.EntityState.Deleted;

                SourceManager sourceManager = new SourceManager();
                sourceManager.DeleteSource(tblSource);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void UpdateSource(SourceViewModel sourceViewModel)
        {
            try
            {
                tbl_Source tblSource = new tbl_Source();

                tblSource.SourceID = sourceViewModel.sourceID;
                tblSource.SourceDesc = sourceViewModel.sourceDesc;
                tblSource.daId = sourceViewModel.DAID;//1;
                tblSource.EntityState = DA.DomainModel.EntityState.Modified;

                SourceManager sourceManager = new SourceManager();
                sourceManager.UpdateSource(tblSource);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public IList<SourceViewModel> GetSourceDetails(int daId)
        {
            try
            {
                SourceManager sourceManager = new SourceManager();
                var sourceList = sourceManager.GetSourceDetails(daId);

                SourceList = new List<SourceViewModel>();
                foreach (var item in sourceList)
                {
                    SourceViewModel sourceViewModel = new SourceViewModel();
                    sourceViewModel.sourceID = item.SourceID;
                    sourceViewModel.sourceDesc = item.SourceDesc;
                    sourceViewModel.DAID = item.daId;//1;

                    SourceList.Add(sourceViewModel);
                }
                return SourceList;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public SourceViewModel FindSource(int? sourceID)
        {
            try
            {
                SourceViewModel sourceViewModel = new SourceViewModel();

                SourceManager sourceManager = new SourceManager();
                var source = sourceManager.FindSource(sourceID);

                sourceViewModel.sourceID = source.SourceID;
                sourceViewModel.sourceDesc = source.SourceDesc;
                sourceViewModel.DAID = source.daId;

                return sourceViewModel;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool CheckDuplicate(SourceViewModel sourceviewmodel)
        {
            try
            {
                SourceManager sourceManager = new SourceManager();

                var source = sourceManager.FindSourceName(sourceviewmodel.sourceDesc, sourceviewmodel.DAID);

                if (source != null && source.SourceID != sourceviewmodel.sourceID && source.SourceDesc.ToUpper() == sourceviewmodel.sourceDesc.ToUpper())
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