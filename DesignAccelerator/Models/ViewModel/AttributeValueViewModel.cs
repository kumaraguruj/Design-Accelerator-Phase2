using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DA.DomainModel;
using DA.BusinessLayer;
using System.Data;
using System.Text.RegularExpressions;

namespace DesignAccelerator.Models.ViewModel
{
    public class AttributeValueViewModel
    {
        #region Public Properties
        public int AttributeID { get; set; }
        public string AttributeDesc { get; set; }

        public int AttrValueID { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9_ ]*$", ErrorMessage = "Special Characters are not allowed in this field")]
        public string AttributeValue { get; set; }

        public string IsNegative { get; set; }

        public int DaId { get; set; }
        public string daName { get; set; }
        public int ModuleId { get; set; }

        //store list of value
        public IList<AttributeValueViewModel> LstAttribVal { get; set; }
        public IList<AttributeListViewModel> LstAttrib { get; set; }
        #endregion

        public void GetAttributeValues(int? attributeId)
        {
            try
            {
                AttributeValueManager attribValManager = new AttributeValueManager();
                var lstAttribVal = attribValManager.GetAttributeValList(attributeId);
                LstAttribVal = new List<AttributeValueViewModel>();
                foreach (var item in lstAttribVal)
                {
                    AttributeValueViewModel attribVal = new AttributeValueViewModel();
                    attribVal.AttrValueID = item.AttrValueID;
                    attribVal.AttributeValue = item.AttributeValue;
                    attribVal.IsNegative = item.isNegative;
                    LstAttribVal.Add(attribVal);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void GetAttributeList(int? designAccelaratorID)
        {
            try
            {
                LstAttrib = new List<AttributeListViewModel>();
                AttributeListManager attributeListManager = new AttributeListManager();
                var attributeList = attributeListManager.GetAttributeList(designAccelaratorID);

                foreach (var item in attributeList)
                {
                    AttributeListViewModel attributeListViewModel = new AttributeListViewModel();
                    attributeListViewModel.AttributeID = item.AttributeID;
                    attributeListViewModel.AttributeDesc = item.AttributeDesc;
                    LstAttrib.Add(attributeListViewModel);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public int SaveData(IList<AttributeValueViewModel> attribValVM)
        {
            try
            {
                int result = 0;

                List<tbl_AttributeValues> attribVal = new List<tbl_AttributeValues>();
                AttributeValueManager attribValManager = new AttributeValueManager();
                Regex avoidSpecialChars = new Regex(@"^[a-zA-Z0-9_<>= ]*$");

                foreach (var item in attribValVM)
                {
                    if (item.AttrValueID > 0)
                    {
                        //Updated
                        item.AttributeValue = item.AttributeValue == null ? "" : item.AttributeValue.Trim();

                        if (avoidSpecialChars.IsMatch(item.AttributeValue))
                        {
                            tbl_AttributeValues updateItem = new tbl_AttributeValues();
                            updateItem.AttributeID = item.AttributeID;
                            updateItem.AttributeValue = item.AttributeValue == null ? "" : item.AttributeValue;
                            updateItem.AttrValueID = item.AttrValueID;
                            updateItem.daId = item.DaId;
                            updateItem.isNegative = item.IsNegative;

                            updateItem.EntityState = updateItem.AttributeValue == "" ? DA.DomainModel.EntityState.Deleted : DA.DomainModel.EntityState.Modified;
                            attribVal.Add(updateItem);
                        }

                    }
                    else
                    {
                        // Added
                        if (item.AttributeValue != null)
                        {
                            item.AttributeValue = item.AttributeValue.Trim();

                            if (avoidSpecialChars.IsMatch(item.AttributeValue))
                            {
                                tbl_AttributeValues newItem = new tbl_AttributeValues();
                                newItem.AttributeID = item.AttributeID;
                                newItem.AttributeValue = item.AttributeValue;
                                newItem.AttrValueID = item.AttrValueID;
                                newItem.daId = item.DaId;
                                newItem.isNegative = item.IsNegative;

                                newItem.EntityState = DA.DomainModel.EntityState.Added;
                                attribVal.Add(newItem);
                            }
                        }
                    }
                }
                result = attribValManager.AddAttributeVal(attribVal);
                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}