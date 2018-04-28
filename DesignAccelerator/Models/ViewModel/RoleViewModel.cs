using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DA.BusinessLayer;
using DA.DomainModel;
using System.Web.Mvc;
using System.Data.Entity;

namespace DesignAccelerator.Models.ViewModel
{
    public class RoleViewModel
    {
        #region Properties
        //tbl_Roles
        public int roleId { get; set; }
        [Required(ErrorMessage = "RoleName is required")]
        [RegularExpression(@"^[a-zA-Z0-9_ ]*$", ErrorMessage = "Special Characters are not allowed in this field")]
        public string rolename { get; set; }
        public Boolean status { get; set; }

        public IList<tbl_Roles> lstRoles { get; set; }
        public IList<tbl_RoleScreenMapping> lstRoleScreenMapping { get; set; }

        #endregion

        public void GetRoleDetails()
        {
            try
            {
                RoleManager roleManager = new RoleManager();
                lstRoles = new List<tbl_Roles>();
                lstRoles = roleManager.GetRoleDetails();
                //lstRoleScreenMapping = new List<tbl_RoleScreenMapping>();
                //lstRoleScreenMapping = roleManager.GetRoleMappingDetails();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void AddRole(RoleViewModel roleViewModel)
        {
            try
            {
                tbl_Roles tblRoles = new tbl_Roles();

                tblRoles.RoleID = roleViewModel.roleId;
                tblRoles.RoleName = roleViewModel.rolename;
                if (!roleViewModel.status)
                {
                    tblRoles.Active = false;
                }
                else
                { tblRoles.Active = true; }

                tblRoles.EntityState = DA.DomainModel.EntityState.Added;

                RoleManager roleManager = new RoleManager();
                roleManager.AddRoles(tblRoles);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void UpdateRole(RoleViewModel roleViewModel)
        {
            try
            {
                tbl_Roles tblRoles = new tbl_Roles();

                tblRoles.RoleID = roleViewModel.roleId;
                tblRoles.RoleName = roleViewModel.rolename;

                if (!roleViewModel.status)
                {
                    tblRoles.Active = false;
                }
                else
                { tblRoles.Active = true; }

                tblRoles.EntityState = DA.DomainModel.EntityState.Modified;

                RoleManager roleManager = new RoleManager();
                roleManager.UpdateRoles(tblRoles);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteRole(RoleViewModel roleViewModel)
        {
            try
            {
                //tbl_Roles tblRoles = new tbl_Roles();
                //tbl_RoleScreenMapping tblRoleScreenMapping = new tbl_RoleScreenMapping();

                RoleManager roleManager = new RoleManager();
                roleManager.DeleteRoleMapping(roleViewModel.roleId);
                return true;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public RoleViewModel FindRole(int? roleId)
        {
            try
            {
                RoleViewModel roleModel = new RoleViewModel();
                RoleManager roleManager = new RoleManager();

                var role = roleManager.FindRoles(roleId);

                roleModel.lstRoles = roleManager.GetRoleDetails();

                roleModel.roleId = role.RoleID;
                roleModel.rolename = role.RoleName;
                if (role.Active == true)
                {
                    roleModel.status = true;
                }
                else
                {
                    roleModel.status = false;
                }

                return roleModel;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool CheckDuplicate(RoleViewModel roleViewModel)
        {
            try
            {
                RoleManager roleManager = new RoleManager();

                var role = roleManager.FindRoleName(roleViewModel.rolename);

                if (role != null && role.RoleID != roleViewModel.roleId && role.RoleName.ToUpper() == roleViewModel.rolename.ToUpper())
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
    }
}