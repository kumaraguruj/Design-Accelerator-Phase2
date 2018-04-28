using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DA.DomainModel;
using DA.BusinessLayer;

namespace DesignAccelerator.Models.ViewModel
{
    public class DestinationViewModel
    {
        #region Public Properties
        public int DAID { get; set; }
        public string DAName { get; set; }
        public int destID { get; set; }

        [Required(ErrorMessage = "Destination is required")]
        [RegularExpression(@"^[a-zA-Z0-9_ ]*$", ErrorMessage = "Special Characters are not allowed in this field")]
        public string destDesc { get; set; }

        public int ModuleId { get; set; }
        public int ApplicationID { get; set; }
        public string ApplicationName { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }

        public int roleId { get; set; }
        public string RoleName { get; set; }

        public bool AddPermmission = false;
        public bool EdiPermission = false;
        public bool DeletePermission = false;

        public IList<DestinationViewModel> DestList { get; set; }
        #endregion

        public void GetDestinationDetails(int daId)
        {
            try
            {
                DestinationManager destManager = new DestinationManager();
                var destList = destManager.GetDestinationDetails(daId);

                DestList = new List<DestinationViewModel>();
                foreach (var item in destList)
                {
                    DestinationViewModel destViewModel = new DestinationViewModel();

                    destViewModel.destID = item.DestID;
                    destViewModel.destDesc = item.DestDesc;
                    destViewModel.DAID = item.daId;// 1;

                    DestList.Add(destViewModel);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void AddDestination(DestinationViewModel destViewModel)
        {
            try
            {
                tbl_Destination tblDestination = new tbl_Destination();

                tblDestination.DestDesc = destViewModel.destDesc;
                tblDestination.daId = destViewModel.DAID;//  1; ;

                tblDestination.EntityState = DA.DomainModel.EntityState.Added;

                DestinationManager destManager = new DestinationManager();
                destManager.AddDestination(tblDestination);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void DeleteDestination(DestinationViewModel destViewModel)
        {
            try
            {
                tbl_Destination tblDestination = new tbl_Destination();
                tblDestination.daId = destViewModel.DAID;
                tblDestination.DestID = destViewModel.destID;
                tblDestination.EntityState = DA.DomainModel.EntityState.Deleted;

                DestinationManager destManager = new DestinationManager();
                destManager.DeleteDestination(tblDestination);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void UpdateDestination(DestinationViewModel destViewModel)
        {
            try
            {
                tbl_Destination tblDestination = new tbl_Destination();

                tblDestination.DestID = destViewModel.destID;
                tblDestination.DestDesc = destViewModel.destDesc;
                tblDestination.daId = destViewModel.DAID;//1;
                tblDestination.EntityState = DA.DomainModel.EntityState.Modified;

                DestinationManager destManager = new DestinationManager();
                destManager.UpdateDestination(tblDestination);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public DestinationViewModel FindDestination(int? destID)
        {
            try
            {
                DestinationViewModel destViewModel = new DestinationViewModel();

                DestinationManager destManager = new DestinationManager();
                var dest = destManager.FindDestination(destID);

                destViewModel.destID = dest.DestID;
                destViewModel.destDesc = dest.DestDesc;
                destViewModel.DAID = dest.daId;

                return destViewModel;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool CheckDuplicate(DestinationViewModel destViewModel)
        {
            try
            {
                DestinationManager destmanager = new DestinationManager();

                var destination = destmanager.FindDestinationDesc(destViewModel.destDesc, destViewModel.DAID);

                if (destination != null && destination.DestID != destViewModel.destID && destination.DestDesc.ToUpper() == destViewModel.destDesc.ToUpper())
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