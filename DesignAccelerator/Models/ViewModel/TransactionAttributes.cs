using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DA.BusinessLayer;
using DA.DomainModel;
using System.Text.RegularExpressions;

namespace DesignAccelerator.Models.ViewModel
{
    public class TransactionAttributes
    {
        #region Properties
        [System.ComponentModel.DataAnnotations.Key]
        public int MapId { get; set; }
        public string RequirementReference { get; set; }
        public int TransactionId { get; set; }
        public string HighLevelTransaction { get; set; }
        public bool IsLinked { get; set; }
        public int AttributeId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeTypeDesc { get; set; }
        public int daId { get; set; }

        //Flow Implementation
        public int ClientID { get; set; }
        public string ClientName { get; set; }
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int ApplicationID { get; set; }
        public string ApplicationName { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string daName { get; set; }
        public int ProductId { get; set; }
        //public string ProductName { get; set; }

        #endregion

        public IList<TransactionAttributes> GetTransactionAttributes(int daId)
        {
            try
            {
                TransactionAttributeManager transactionAttributeManager = new TransactionAttributeManager();
                var transactionAttributes = transactionAttributeManager.GetTransactionAttributes(daId);

                List<TransactionAttributes> transactionAttrabutesList = new List<TransactionAttributes>();
                foreach (var item in transactionAttributes)
                {
                    TransactionAttributes transactionAttributesItem = new TransactionAttributes();
                    transactionAttributesItem.MapId = Convert.ToInt32(item.MAPID);
                    transactionAttributesItem.RequirementReference = (item.REQREFERENCE == null ? "" : item.REQREFERENCE.Trim());
                    transactionAttributesItem.TransactionId = Convert.ToInt32(item.TRANSACTIONSEQ);
                    transactionAttributesItem.AttributeId = item.ATTRIBUTEID;
                    transactionAttributesItem.AttributeName = item.ATTRIBUTEDESC;
                    transactionAttributesItem.AttributeTypeDesc = item.ATTRIBUTETYPEDESC;
                    transactionAttributesItem.HighLevelTransaction = item.HIGHLEVELTXN;
                    transactionAttributesItem.IsLinked = Convert.ToBoolean(Convert.ToInt32(item.ISLINKED));
                    transactionAttrabutesList.Add(transactionAttributesItem);
                }

                return transactionAttrabutesList;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public int SaveData(IList<TransactionAttributes> transactionAttributes, int daId)
        {
            try
            {
                int result = 0;
                TransactionAttributeManager transactionAttributeManager = new TransactionAttributeManager();
                var transactionAttributesList = transactionAttributeManager.GetTransactionAttributes(daId);
                List<tbl_TxnAttributeMapping> tblTxnAttributeMapping = new List<tbl_TxnAttributeMapping>();
                foreach (var item in transactionAttributes)
                {
                    var queryResult = transactionAttributesList.Where(e => e.ATTRIBUTEID.Equals(item.AttributeId) && e.HIGHLEVELTXN.Equals(item.HighLevelTransaction));
                    //OR
                    //var queryResult = from transactionAttribute in transactionAttributesList
                    //                  where transactionAttribute.ATTRIBUTEID.Equals(item.AttributeId)
                    //                  && transactionAttribute.HIGHLEVELTXN.Equals(item.HighLevelTransaction)
                    //                  select transactionAttribute;

                    Regex avoidSpecialChars = new Regex(@"^[a-zA-Z0-9_ ]*$");

                    foreach (var e in queryResult)
                    {
                        if (e.MAPID != null && e.MAPID > 0)
                        {
                            //Updated
                            tbl_TxnAttributeMapping updateItem = new tbl_TxnAttributeMapping();

                            if (item.RequirementReference != null)
                            {
                                if (avoidSpecialChars.IsMatch(item.RequirementReference))
                                {
                                    updateItem.AttributeID = item.AttributeId;
                                    updateItem.daId = daId;
                                    if (Convert.ToBoolean(Convert.ToInt32(e.ISLINKED)) == item.IsLinked && e.REQREFERENCE == item.RequirementReference)
                                        updateItem.EntityState = DA.DomainModel.EntityState.Unchanged;
                                    else
                                        updateItem.EntityState = DA.DomainModel.EntityState.Modified;
                                    updateItem.isLinked = (item.IsLinked == true ? "1" : "0");
                                    updateItem.MapID = Convert.ToInt32(e.MAPID);
                                    updateItem.ReqReference = (item.RequirementReference == null ? "" : item.RequirementReference.Trim());
                                    updateItem.TransactionSeq = Convert.ToInt32(e.TRANSACTIONSEQ);
                                    tblTxnAttributeMapping.Add(updateItem);
                                }
                            }


                        }
                        else
                        {
                            //Added
                            if (item.IsLinked == true)
                            {
                                if (item.RequirementReference != null)
                                {
                                    if (avoidSpecialChars.IsMatch(item.RequirementReference))
                                    {
                                        tbl_TxnAttributeMapping newItem = new tbl_TxnAttributeMapping();
                                        newItem.AttributeID = item.AttributeId;
                                        newItem.daId = daId;
                                        newItem.EntityState = DA.DomainModel.EntityState.Added;
                                        newItem.isLinked = (item.IsLinked == true ? "1" : "0");
                                        newItem.ReqReference = (item.RequirementReference == null ? "" : item.RequirementReference.Trim());
                                        newItem.TransactionSeq = Convert.ToInt32(e.TRANSACTIONSEQ);
                                        tblTxnAttributeMapping.Add(newItem);
                                    }
                                }


                            }
                        }
                    }

                }
                result = transactionAttributeManager.SaveData(tblTxnAttributeMapping);
                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}