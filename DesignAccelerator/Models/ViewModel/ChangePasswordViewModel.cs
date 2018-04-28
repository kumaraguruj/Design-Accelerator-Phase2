using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DA.BusinessLayer;
using DA.DomainModel;

namespace DesignAccelerator.Models.ViewModel
{
    public class ChangePasswordViewModel
    {
        #region Properties
        //tbl_ChangePassword
        public int userId { get; set; }
        [Required(ErrorMessage = "User Name is required")]
        [RegularExpression(@"^[a-zA-Z0-9_ ]*$", ErrorMessage = "Special Characters are not allowed in this field")]
        public string userName { get; set; }
        [Required(ErrorMessage = "Old Password is required")]
        public string oldPassword { get; set; }
        [Required(ErrorMessage = "New Password is required")]
        public string newPassword { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        public string confirmPassword { get; set; }
        #endregion
        public IList<tbl_UserData> lstUserData { get; set; }

        public void GetUserDetails()
        {
            try
            {
                UserManager userManager = new UserManager();
                lstUserData = new List<tbl_UserData>();

                lstUserData = userManager.GetUserDetails();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void UpdateChangePwdUser(tbl_UserData tblUserData)
        {
            try
            {
                tblUserData.EntityState = DA.DomainModel.EntityState.Modified;

                UserManager userManager = new UserManager();
                userManager.UpdateUserData(tblUserData);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public UserModel FindChangePwdUser(int? userId)
        {
            try
            {
                UserModel userModel = new UserModel();
                UserManager userManager = new UserManager();

                var usr = userManager.FindUserData(userId);

                userModel.userId = usr.UserID;
                userModel.userName = usr.UserName;
                userModel.password = usr.Password;

                return userModel;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}