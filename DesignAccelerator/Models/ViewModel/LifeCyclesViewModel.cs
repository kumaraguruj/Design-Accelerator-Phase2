using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DA.DomainModel;
using DA.BusinessLayer;
using System.ComponentModel.DataAnnotations;

namespace DesignAccelerator.Models.ViewModel
{
    public class LifeCyclesViewModel
    {
        public int LifeCycleID { get; set; }
        [Required(ErrorMessage = "LifeCycle description is required")]
        [RegularExpression(@"^[a-zA-Z0-9_ ]*$", ErrorMessage = "Special Characters are not allowed in this field")]
        public string LifeCycleDesc { get; set; }

        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int ClientID { get; set; }
        public string ClientName { get; set; }
        public int ApplicationID { get; set; }
        public string ApplicationName { get; set; }
        public int daid { get; set; }
        public string daName { get; set; }

        public int roleId { get; set; }
        public string RoleName { get; set; }

        public bool AddPermmission = false;
        public bool EdiPermission = false;
        public bool DeletePermission = false;

        public IList<tbl_LifeCycle> lstLifeCycles { get; set; }

        public LifeCyclesViewModel GetTransactionsLifecycle(int? designAccelaratorID)
        {
            try
            {
                LifeCyclesViewModel lifecyclesviewmodel = new LifeCyclesViewModel();
                LifeCycleManager lifecycleManager = new LifeCycleManager();
                lifecyclesviewmodel.lstLifeCycles = lifecycleManager.GetLifeCycles(designAccelaratorID);

                return lifecyclesviewmodel;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void AddLifeCycle(LifeCyclesViewModel lifecyclesviewmodel)
        {
            try
            {
                tbl_LifeCycle tbllifecycle = new tbl_LifeCycle();

                tbllifecycle.LifeCycleDesc = lifecyclesviewmodel.LifeCycleDesc;
                tbllifecycle.daId = lifecyclesviewmodel.daid;
                tbllifecycle.EntityState = DA.DomainModel.EntityState.Added;

                LifeCycleManager lifecycleManager = new LifeCycleManager();
                lifecycleManager.AddLifeCycle(tbllifecycle);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public LifeCyclesViewModel FindLifeCycles(int? lifeCycleID)
        {
            try
            {
                LifeCyclesViewModel lifecyclevm = new LifeCyclesViewModel();
                LifeCycleManager lifecycleManager = new LifeCycleManager();
                var lifecycle = lifecycleManager.FindLifeCycles(lifeCycleID);
                lifecyclevm.daid = lifecycle.daId;
                lifecyclevm.LifeCycleID = lifecycle.LifeCycleID;
                lifecyclevm.LifeCycleDesc = lifecycle.LifeCycleDesc;

                return lifecyclevm;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool DeleteLifeCycle(LifeCyclesViewModel lifecyclesviewmodel)
        {
            try
            {
                tbl_LifeCycle tbllifecycle = new tbl_LifeCycle();

                tbllifecycle.LifeCycleID = lifecyclesviewmodel.LifeCycleID;
                tbllifecycle.EntityState = DA.DomainModel.EntityState.Deleted;

                LifeCycleManager lifecycleManager = new LifeCycleManager();
                lifecycleManager.DeleteLifeCycle(tbllifecycle);

                return true;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void UpdateLifeCycle(LifeCyclesViewModel lifecyclesviewmodel)
        {
            try
            {
                tbl_LifeCycle tbllifecycle = new tbl_LifeCycle();

                tbllifecycle.LifeCycleID = lifecyclesviewmodel.LifeCycleID;
                tbllifecycle.LifeCycleDesc = lifecyclesviewmodel.LifeCycleDesc;
                tbllifecycle.daId = lifecyclesviewmodel.daid;
                tbllifecycle.EntityState = DA.DomainModel.EntityState.Modified;

                LifeCycleManager lifecycleManager = new LifeCycleManager();
                lifecycleManager.UpdateLifeCycle(tbllifecycle);
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

        //public DAViewModel FindDA(int? daID)
        //{

        //    DAViewModel dAViewModel = new DAViewModel();
        //    DAManager daManager = new DAManager();
        //    var da = daManager.FindDA(daID);
        //    dAViewModel.DAID = da.daid;
        //    dAViewModel.DAName = da.daName;
        //    dAViewModel.ApplicationID = (int)da.ModuleId;
        //    return dAViewModel;
        //}
    }
}