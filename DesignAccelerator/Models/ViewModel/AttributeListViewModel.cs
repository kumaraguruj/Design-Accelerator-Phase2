using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DA.BusinessLayer;
using DA.DomainModel;

namespace DesignAccelerator.Models.ViewModel
{
    public class AttributeListViewModel
    {
        #region Public Properties

        //Table Attribute
        public int AttributeID { get; set; }

        [Required(ErrorMessage = "Attribute Name is required")]
        [RegularExpression(@"^[a-zA-Z0-9_ ]*$", ErrorMessage = "Special Characters are not allowed in this field")]
        public string AttributeDesc { get; set; }

        //Table AttributeType
        public int AttributeTypeID { get; set; }
        public string AttributeTypeDesc { get; set; }
        public bool CommonAttributeTypeID { get; set; }
        public bool CriticalAttributeTypeID { get; set; }

        //Table AttributeValues
        public int AttrValueID { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9_ ]*$", ErrorMessage = "Special Characters are not allowed in this field")]
        public string AttributeValue { get; set; }

        //Flow Implementation
        public int ClientID { get; set; }
        public string ClientName { get; set; }
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int ApplicationID { get; set; }
        public string ApplicationName { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public int daId { get; set; }
        public string daName { get; set; }
        public int ProductId { get; set; }
        public int TransactionSeq { get; set; }

        public int roleId { get; set; }
        public string RoleName { get; set; }

        public bool AddPermmission = false;
        public bool EdiPermission = false;
        public bool DeletePermission = false;

        public IList<AttributeListViewModel> LstAttrib { get; set; }
        //public IList<AttributeListViewModel> LstAttribVal { get; set; }

        #endregion

        public void GetAttribute(int? designAccelaratorID)
        {
            try
            {
                //AttributeListViewModel attriblistviewmodel = new AttributeListViewModel();
                AttributeListManager attribManager = new AttributeListManager();
                var lstAttrib = attribManager.GetAttributeList(designAccelaratorID);
                LstAttrib = new List<AttributeListViewModel>();
                foreach (var item in lstAttrib)
                {
                    AttributeListViewModel attrib = new AttributeListViewModel();

                    attrib.AttributeID = item.AttributeID;
                    attrib.AttributeDesc = item.AttributeDesc;
                    attrib.AttributeTypeID = item.tbl_AttributeType.AttributeTypeID;
                    attrib.AttributeTypeDesc = item.tbl_AttributeType.AttributeTypeDesc;

                    LstAttrib.Add(attrib);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public IList<int> GetAttributeList(int? designAccelaratorID)
        {
            try
            {
                AttributeListManager attribManager = new AttributeListManager();
                var lstAttrib = attribManager.GetAttributeList(designAccelaratorID);
                List<int> LstAttrib = new List<int>();
                foreach (var item in lstAttrib)
                {
                    LstAttrib.Add(item.AttributeID);
                }

                return LstAttrib;
            }
            catch (Exception)
            {

                throw;
            }


        }
        public void AddAttrib(AttributeListViewModel attriblistviewmodel)
        {
            try
            {
                tbl_Attribute tblattrib = new tbl_Attribute();
                AttributeListManager attribManager = new AttributeListManager();

                tblattrib.AttributeDesc = attriblistviewmodel.AttributeDesc.Trim();
                tblattrib.AttributeDesc = attriblistviewmodel.AttributeDesc;
                tblattrib.daId = attriblistviewmodel.daId;
                tblattrib.EntityState = DA.DomainModel.EntityState.Added;
                // 1 - None; 2 - Common; 3 - Critical; 4 - Common&Critical

                // 1 - None;
                if (!attriblistviewmodel.CommonAttributeTypeID && !attriblistviewmodel.CriticalAttributeTypeID)
                {
                    //attriblistviewmodel.AttributeTypeID = 1;
                    tblattrib.AttributeTypeID = 1;
                }
                //4 - Common Critical;
                else if (attriblistviewmodel.CommonAttributeTypeID && attriblistviewmodel.CriticalAttributeTypeID)
                {

                    tblattrib.AttributeTypeID = 4;
                }
                //2 - Critical;
                else if (attriblistviewmodel.CriticalAttributeTypeID)
                {
                    tblattrib.AttributeTypeID = 3;
                }//1 - Common
                else if (attriblistviewmodel.CommonAttributeTypeID)
                    tblattrib.AttributeTypeID = 2;

                attribManager.AddAttribute(tblattrib);
            }
            catch (Exception)
            {

                throw;
            }


        }

        public void UpdateAttrib(AttributeListViewModel attriblistviewmodel)
        {
            try
            {
                tbl_Attribute tblattrib = new tbl_Attribute();

                tblattrib.AttributeID = attriblistviewmodel.AttributeID;
                if (attriblistviewmodel.AttributeDesc != null)
                {
                    tblattrib.AttributeDesc = attriblistviewmodel.AttributeDesc.Trim();
                }
                tblattrib.AttributeDesc = attriblistviewmodel.AttributeDesc;
                tblattrib.daId = attriblistviewmodel.daId;
                // 0 - NonSelected;
                if (!attriblistviewmodel.CommonAttributeTypeID && !attriblistviewmodel.CriticalAttributeTypeID)
                {
                    //attriblistviewmodel.AttributeTypeID = 1;
                    tblattrib.AttributeTypeID = 1;
                }
                //1 - Common Critical;
                else if (attriblistviewmodel.CommonAttributeTypeID && attriblistviewmodel.CriticalAttributeTypeID)
                {

                    tblattrib.AttributeTypeID = 4;
                }
                //2 - Critical;
                else if (attriblistviewmodel.CriticalAttributeTypeID)
                {
                    tblattrib.AttributeTypeID = 3;
                }//Common
                else if (attriblistviewmodel.CommonAttributeTypeID)
                    tblattrib.AttributeTypeID = 2;

                tblattrib.EntityState = DA.DomainModel.EntityState.Modified;

                AttributeListManager attribManager = new AttributeListManager();
                attribManager.UpdateAttribute(tblattrib);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool DeleteAttrib(AttributeListViewModel attriblistviewmodel)
        {
            try
            {
                tbl_Attribute tblattrib = new tbl_Attribute();

                tblattrib.AttributeID = attriblistviewmodel.AttributeID;

                tblattrib.EntityState = DA.DomainModel.EntityState.Deleted;

                AttributeListManager attribManager = new AttributeListManager();
                attribManager.DeleteAttribute(tblattrib);

                return true;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public AttributeListViewModel FindAttrib(int? attribID)
        {
            try
            {
                AttributeListViewModel attribVM = new AttributeListViewModel();
                AttributeListManager attribManager = new AttributeListManager();

                var attrib = attribManager.FindAttribs(attribID);

                attribVM.AttributeID = attrib.AttributeID;
                attribVM.AttributeDesc = attrib.AttributeDesc;
                attribVM.AttributeTypeID = attrib.AttributeTypeID;
                // 1 - NonSelected;
                if (attribVM.AttributeTypeID == 1)
                {
                    attribVM.CommonAttributeTypeID = false;
                    attribVM.CriticalAttributeTypeID = false;
                }
                //2 - Common;
                else if (attrib.AttributeTypeID == 2)
                {
                    //4 - Common&Critical;
                    attribVM.CommonAttributeTypeID = true;
                    attribVM.CriticalAttributeTypeID = false;
                }
                //3 - Critical;
                else if (attrib.AttributeTypeID == 3)
                {
                    attribVM.CommonAttributeTypeID = false;
                    attribVM.CriticalAttributeTypeID = true;
                }
                else
                {
                    attribVM.CommonAttributeTypeID = true;
                    attribVM.CriticalAttributeTypeID = true;
                }

                //attribVM.AttributeTypeID = attrib.AttributeTypeID;
                attribVM.daId = attrib.daId; // 1
                return attribVM;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool CheckDuplicate(AttributeListViewModel attributeListViewModel)
        {
            try
            {
                AttributeListManager attribManager = new AttributeListManager();

                var attrib = attribManager.FindAttribDesc(attributeListViewModel.AttributeDesc, attributeListViewModel.daId);

                if (attrib != null && attrib.AttributeID != attributeListViewModel.AttributeID && attrib.AttributeDesc.ToUpper() == attributeListViewModel.AttributeDesc.ToUpper())
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