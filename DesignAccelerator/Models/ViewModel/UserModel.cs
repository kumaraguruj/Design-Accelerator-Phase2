using System;
using System.Collections.Generic;
using DA.BusinessLayer;
using DA.DomainModel;
using System.ComponentModel.DataAnnotations;
using DesignAccelerator.Controllers;
using System.Linq;
using System.Web;

namespace DesignAccelerator.Models.ViewModel
{
    public class UserModel
    {
        #region Public Properties

        //tbl_UserData
        public int userId { set; get; }
        [Required(ErrorMessage = "UserName is required")]
        [RegularExpression(@"^[a-zA-Z0-9_. ]*$", ErrorMessage = "Special Characters are not allowed in this field")]
        public string userName { set; get; }

        public string password { set; get; }

        public string ConfirmPassword { set; get; }
        public Boolean Status { set; get; }
        
        public int AuthID { get; set; }
        public string AuthName { set; get; }

        //tbl_Roles
        public int RoleId { get; set; }
        public string Rolename { get; set; }

        public int LoggedInUserRoleID { get; set; }
        public string LoggedInUserName { get; set; }

        public bool AddPermmission = false;
        public bool EdiPermission = false;
        public bool DeletePermission = false;

        public int UserTypeId { get; set; }
        public int CreatorID { get; set; }
        
        public int UserType { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<UserType> UserTypes { get; set; }
        public IList<tbl_UserData> lstUserData { get; set; }
        public IList<UserModel> lstAdminUsers { get; set; }
        public IList<tbl_Roles> lstRoles { get; set; }

        #endregion

        public void GetUserDetails()
        {
            try
            {
                tbl_UserData currentloggedinuserdata = (tbl_UserData)HttpContext.Current.Session["CurrentLoggedInUserDetails"];
                int currentloggedinuserID = currentloggedinuserdata.UserID;

                UserManager userManager = new UserManager();
                RoleManager roleManager = new RoleManager();

                lstUserData = new List<tbl_UserData>();
                lstRoles = new List<tbl_Roles>();

                lstRoles = roleManager.GetRoleDetails();
                lstUserData = userManager.GetUserDetails();
                var AdminUsers = userManager.GetAdminUsersToAuthorize("Admin", currentloggedinuserID);                
                List<UserModel> adminuserViewModelList = new List<UserModel>();

                foreach (var item in AdminUsers)
                {
                    UserModel adminusersviewmodelItem = new UserModel();
                    adminusersviewmodelItem.userId = item.UserID;
                    adminusersviewmodelItem.userName = item.UserName;

                    adminuserViewModelList.Add(adminusersviewmodelItem);
                }
                lstAdminUsers = adminuserViewModelList;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddUser(UserModel userModel)
        {
            try
            {
                tbl_UserData currentloggedinuserdata = (tbl_UserData)HttpContext.Current.Session["CurrentLoggedInUserDetails"];               

                tbl_UserData tblUserData = new tbl_UserData();
                tblUserData.UserName = userModel.userName;
                tblUserData.Password = userModel.password ?? string.Empty;
                tblUserData.RoleID = userModel.RoleId;
                tblUserData.CreatorID = currentloggedinuserdata.UserID;
                tblUserData.AuthID = userModel.AuthID;
                tblUserData.UserType = userModel.UserTypeId;
                tblUserData.CreatedDate = DateTime.Now;
                if (!userModel.Status)
                {
                    tblUserData.Active = "0";
                }
                else
                { tblUserData.Active = "1"; }

                tblUserData.EntityState = DA.DomainModel.EntityState.Added;

                UserManager userManager = new UserManager();
                userManager.AddUserData(tblUserData);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void UpdateUser(UserModel userModel)
        {
            try
            {
                tbl_UserData tblUserData = new tbl_UserData();
               
                tblUserData.UserID = userModel.userId;
                tblUserData.UserName = userModel.userName;
                tblUserData.Password = userModel.password;  
                tblUserData.RoleID = userModel.RoleId;
                tblUserData.CreatorID = userModel.CreatorID;
                tblUserData.AuthID = userModel.AuthID;
                tblUserData.UserType = userModel.UserType;
                tblUserData.CreatedDate = userModel.CreatedDate;

                int oldroleid = Convert.ToInt32(HttpContext.Current.Session["UserOldRoleID"]);

                if (userModel.RoleId == oldroleid)
                {
                    if (!userModel.Status)
                    {
                        tblUserData.Active = "0";
                    }
                    else
                    { tblUserData.Active = "1"; }
                }
                else
                {
                    tblUserData.Active = "0";
                }

                tblUserData.EntityState = DA.DomainModel.EntityState.Modified;

                UserManager userManager = new UserManager();
                userManager.UpdateUserData(tblUserData);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool DeleteUser(UserModel userModel)
        {
            try
            {
                tbl_UserData tblUserData = new tbl_UserData();
                tblUserData.UserID = userModel.userId;

                tblUserData.EntityState = DA.DomainModel.EntityState.Deleted;

                UserManager userManager = new UserManager();
                userManager.DeleteUserData(tblUserData);

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public UserModel FindUser(int? userId)
        {
            try
            {
                UserModel userModel = new UserModel();
                UserManager userManager = new UserManager();

                var usr = userManager.FindUserData(userId);

                RoleManager roleManager = new RoleManager();
                userModel.lstRoles = roleManager.GetRoleDetails();

                userModel.userId = usr.UserID;
                userModel.userName = usr.UserName;
                userModel.password = usr.Password;
                userModel.RoleId = usr.RoleID;
                userModel.AuthID = usr.AuthID;
                userModel.CreatorID = usr.CreatorID;
                userModel.UserType = usr.UserType;
                userModel.CreatedDate = usr.CreatedDate;
                //to get name instead of id
                userModel.Rolename = userModel.lstRoles.Where(a => a.RoleID == userModel.RoleId).First().RoleName;
                userModel.Status = Convert.ToBoolean(Convert.ToInt32(usr.Active));

                return userModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IList<tbl_Roles> GetUserRoles(UserModel userModel)
        {
            try
            {
                RoleManager roleManager = new RoleManager();
                userModel.lstRoles = roleManager.GetRoleDetails();
                return userModel.lstRoles;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool CheckDuplicate(UserModel userModel)
        {
            try
            {
                UserManager userManager = new UserManager();

                var user = userManager.FindUserName(userModel.userName); //, userModel.password

                if (user != null && user.UserID != userModel.userId && user.UserName.ToUpper() == userModel.userName.ToUpper()) // && user.Password == userModel.password
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
                LoggedInUserRoleID = currentloggedinuserdata.RoleID;

                RoleManager roleManager = new RoleManager();
                var userrolepermissions = roleManager.GetUserViewAccessPermissions(screenName, LoggedInUserRoleID);

                foreach (var item in userrolepermissions)
                {
                    if (item.ActionType == "Add")
                        AddPermmission = true;
                    else if (item.ActionType == "Edit")
                        EdiPermission = true;
                    else if (item.ActionType == "Delete")
                        DeletePermission = true;

                    LoggedInUserName = item.RoleName;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}