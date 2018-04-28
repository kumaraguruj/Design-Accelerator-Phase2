using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DA.DomainModel;
using DesignAccelerator.Controllers;
using DA.BusinessLayer;

namespace DesignAccelerator.Models.ViewModel
{
    public class AuthUserViewModel
    {
        public int userId { set; get; }

        public string userName { set; get; }

        public Boolean Status { set; get; }

        public int RoleId { get; set; }

        public string Rolename { get; set; }

        public int LoggedinUserID { get; set; }

        public int AuthID { get; set; }

        public DateTime CreatedDate { get; set; }

        public List<AuthUserViewModel> lstauthusers { get; set; }


        public void getAuthUsersFrmDB()
        {
            try
            {
                UserManager userManager = new UserManager();

                tbl_UserData currentloggedinuserdata = (tbl_UserData)HttpContext.Current.Session["CurrentLoggedInUserDetails"];

                LoggedinUserID = currentloggedinuserdata.UserID;

                var authUserData = userManager.GetAllUsersToAuthorize(LoggedinUserID);

                List<AuthUserViewModel> authuserViewModelList = new List<AuthUserViewModel>();

                foreach (var item in authUserData)
                {
                    AuthUserViewModel authUserviewmodelItem = new AuthUserViewModel();
                    authUserviewmodelItem.userId = item.UserID;
                    authUserviewmodelItem.userName = item.UserName;
                    authUserviewmodelItem.CreatedDate = item.CreatedDate;
                    authUserviewmodelItem.RoleId = item.RoleID;
                    authUserviewmodelItem.Rolename = item.RoleName;
                    authUserviewmodelItem.Status = Convert.ToBoolean(Convert.ToInt32(item.Active));
                    authuserViewModelList.Add(authUserviewmodelItem);
                }
                lstauthusers = authuserViewModelList;
              //  return authuserViewModelList;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public void UpdateUserActive(int userID)
        {
            try
            {
                UserManager userManager = new UserManager();
                
                tbl_UserData tblUserData = new tbl_UserData();
                tblUserData = userManager.FindUserData(userID);
                 tblUserData.Active = "1"; 

                tblUserData.EntityState = DA.DomainModel.EntityState.Modified;

                
                userManager.UpdateUserData(tblUserData);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}