using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DA.DomainModel;
using DA.BusinessLayer;
using System.ComponentModel.DataAnnotations;

namespace DesignAccelerator.Models.ViewModel
{
    public class ModeTypeViewModel
    {
        #region Public Properties
        public int ModeTypeID { get; set; }
        [Required(ErrorMessage = "ModeType is required")]

        [RegularExpression(@"^[a-zA-Z0-9_ ]*$", ErrorMessage = "Special Characters are not allowed in this field")]
        public string ModeTypeDesc { get; set; }

        public int ModuleId { get; set; }
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int ClientID { get; set; }
        public string ClientName { get; set; }
        public int ApplicationID { get; set; }
        public string ApplicationName { get; set; }
        public int daid { get; set; }
        public string daName { get; set; }
        public IList<tbl_ModeType> lstModeTypes { get; set; }

        public int roleId { get; set; }
        public string RoleName { get; set; }

        public bool AddPermmission = false;
        public bool EdiPermission = false;
        public bool DeletePermission = false;
        #endregion

        public ModeTypeViewModel GetlModeTypes(int? designAccelaratorID)
        {
            try
            {
                ModeTypeViewModel modetypeviewmodel = new ModeTypeViewModel();
                ModeTypeManager modetypeManager = new ModeTypeManager();
                modetypeviewmodel.lstModeTypes = modetypeManager.GetModeTypes(designAccelaratorID);

                return modetypeviewmodel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void AddModeType(ModeTypeViewModel modetypeviewmodel)
        {
            try
            {
                tbl_ModeType tblmodetype = new tbl_ModeType();

                tblmodetype.ModeTypeDesc = modetypeviewmodel.ModeTypeDesc;
                tblmodetype.daId = modetypeviewmodel.daid;
                tblmodetype.EntityState = DA.DomainModel.EntityState.Added;

                ModeTypeManager modetypeManager = new ModeTypeManager();
                modetypeManager.AddModeType(tblmodetype);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UpdateModeType(ModeTypeViewModel modetypeviewmodel)
        {
            try
            {
                tbl_ModeType tblmodetype = new tbl_ModeType();

                tblmodetype.ModeTypeID = modetypeviewmodel.ModeTypeID;
                tblmodetype.ModeTypeDesc = modetypeviewmodel.ModeTypeDesc;
                tblmodetype.daId = modetypeviewmodel.daid;
                tblmodetype.EntityState = DA.DomainModel.EntityState.Modified;

                ModeTypeManager modetypeManager = new ModeTypeManager();
                modetypeManager.UpdateModeType(tblmodetype);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool DeleteModeType(ModeTypeViewModel modetypeviewmodel)
        {
            try
            {
                tbl_ModeType tblmodetype = new tbl_ModeType();

                tblmodetype.ModeTypeID = modetypeviewmodel.ModeTypeID;
                tblmodetype.EntityState = DA.DomainModel.EntityState.Deleted;

                ModeTypeManager modetypeManager = new ModeTypeManager();
                modetypeManager.DeleteModeType(tblmodetype);

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ModeTypeViewModel FindModeTypes(int? ModeTypeID)
        {
            try
            {
                ModeTypeViewModel modetypevm = new ModeTypeViewModel();
                ModeTypeManager modetypeManager = new ModeTypeManager();
                var modetypes = modetypeManager.FindModeTypes(ModeTypeID);
                modetypevm.daid = modetypes.daId;
                modetypevm.ModeTypeDesc = modetypes.ModeTypeDesc;
                modetypevm.ModeTypeID = modetypes.ModeTypeID;

                return modetypevm;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool CheckDuplicate(ModeTypeViewModel modeTypeVM)
        {
            try
            {
                ModeTypeManager modetypemanager = new ModeTypeManager();

                var mode = modetypemanager.FindModeDesc(modeTypeVM.ModeTypeDesc, modeTypeVM.daid);

                if (mode != null && mode.ModeTypeID != modeTypeVM.ModeTypeID && mode.ModeTypeDesc.ToUpper() == modeTypeVM.ModeTypeDesc.ToUpper())
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