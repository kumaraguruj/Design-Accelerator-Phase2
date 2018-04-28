using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DA.DomainModel;
using DA.BusinessLayer;
using System.Web.Mvc;
using System.Net;

namespace DesignAccelerator.Models.ViewModel
{
    public class BusinessRulesViewModel
    {
        #region Public Variables
        [System.ComponentModel.DataAnnotations.Key]
        public int BuzRuleID { get; set; }
        public string ReqReference { get; set; }
        public string BuzRuleDesc { get; set; }
        public int TransactionSeq { get; set; }
        public string HighLevelTransaction { get; set; }

        public int BuzRuleAttrMapID { get; set; }
        public string Remarks { get; set; }
        public int daID { get; set; }
        public string daName { get; set; }
        public bool IsLinked { get; set; }

        public int AttrID1 { get; set; }
        public int AttrID2 { get; set; }
        public int AttrID3 { get; set; }
        public int AttrID4 { get; set; }
        public int AttrID5 { get; set; }
        public int AttrID6 { get; set; }
        public int AttrID7 { get; set; }
        public int AttrID8 { get; set; }
        public int AttrID9 { get; set; }
        public int AttrID10 { get; set; }

        public int AttrValueID1 { get; set; }
        public int AttrValueID2 { get; set; }
        public int AttrValueID3 { get; set; }
        public int AttrValueID4 { get; set; }
        public int AttrValueID5 { get; set; }
        public int AttrValueID6 { get; set; }
        public int AttrValueID7 { get; set; }
        public int AttrValueID8 { get; set; }
        public int AttrValueID9 { get; set; }
        public int AttrValueID10 { get; set; }

        public IList<tbl_Transactions> lstTransactions { get; set; }
        public IList<BusinessRulesViewModel> lstbusinessrules { get; set; }
        public IList<tbl_Attribute> lstAttributes { get; set; }
        public IList<tbl_AttributeValues> lstAttributeValues { get; set; }

        //Flow Implementation
        public int ClientID { get; set; }
        public string ClientName { get; set; }
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int ApplicationID { get; set; }
        public string ApplicationName { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public int ProductId { get; set; }

        #endregion

        #region GetBusinessRules

        public IList<BusinessRulesViewModel> GetBusinessRules(int daId)
        {
            try
            {
                BusinessRulesManager businessrulesManager = new BusinessRulesManager();
                var businessrules = businessrulesManager.GetBusinessRules(daId);

                IList<BusinessRulesViewModel> businessrulesList = new List<BusinessRulesViewModel>();
                foreach (var item in businessrules)
                {
                    BusinessRulesViewModel businessrulesItem = new BusinessRulesViewModel();
                    businessrulesItem.BuzRuleID = Convert.ToInt32(item.BuzRuleID);
                    businessrulesItem.ReqReference = (item.ReqReference == null ? "" : item.ReqReference);
                    businessrulesItem.TransactionSeq = Convert.ToInt32(item.TransactionSeq);
                    businessrulesItem.BuzRuleAttrMapID = Convert.ToInt32(item.BuzRuleAttrMapID);
                    businessrulesItem.BuzRuleDesc = Convert.ToString(item.BuzRuleDesc);
                    businessrulesItem.Remarks = (item.Remarks == null ? "" : item.Remarks);
                    businessrulesItem.HighLevelTransaction = item.HIGHLEVELTXN;

                    businessrulesItem.AttrID1 = Convert.ToInt32(item.AttrID1);
                    businessrulesItem.AttrID2 = Convert.ToInt32(item.AttrID2);
                    businessrulesItem.AttrID3 = Convert.ToInt32(item.AttrID3);
                    businessrulesItem.AttrID4 = Convert.ToInt32(item.AttrID4);
                    businessrulesItem.AttrID5 = Convert.ToInt32(item.AttrID5);
                    businessrulesItem.AttrID6 = Convert.ToInt32(item.AttrID6);
                    businessrulesItem.AttrID7 = Convert.ToInt32(item.AttrID7);
                    businessrulesItem.AttrID8 = Convert.ToInt32(item.AttrID8);
                    businessrulesItem.AttrID9 = Convert.ToInt32(item.AttrID9);
                    businessrulesItem.AttrID10 = Convert.ToInt32(item.AttrID10);

                    businessrulesItem.AttrValueID1 = Convert.ToInt32(item.AttrValueID1);
                    businessrulesItem.AttrValueID2 = Convert.ToInt32(item.AttrValueID2);
                    businessrulesItem.AttrValueID3 = Convert.ToInt32(item.AttrValueID3);
                    businessrulesItem.AttrValueID4 = Convert.ToInt32(item.AttrValueID4);
                    businessrulesItem.AttrValueID5 = Convert.ToInt32(item.AttrValueID5);
                    businessrulesItem.AttrValueID6 = Convert.ToInt32(item.AttrValueID6);
                    businessrulesItem.AttrValueID7 = Convert.ToInt32(item.AttrValueID7);
                    businessrulesItem.AttrValueID8 = Convert.ToInt32(item.AttrValueID8);
                    businessrulesItem.AttrValueID9 = Convert.ToInt32(item.AttrValueID9);
                    businessrulesItem.AttrValueID10 = Convert.ToInt32(item.AttrValueID10);

                    businessrulesList.Add(businessrulesItem);
                }

                return businessrulesList;
            }
            catch (Exception)
            {

                throw;
            }

        }

        #endregion

        #region GetAllAttributes

        public void GetAllAttributes(int? designAccelaratorID)
        {
            try
            {
                IList<tbl_Attribute> lstAttr = new List<tbl_Attribute>();
                BusinessRulesManager buzRuleManager = new BusinessRulesManager();

                lstAttr = buzRuleManager.GetAllAttributes(designAccelaratorID);
                lstAttributes = lstAttr;
            }
            catch (Exception)
            {

                throw;
            }

        }

        #endregion

        #region GetAllTransactions

        public void GetAllTransactions(int? designAccelaratorID)
        {
            try
            {
                IList<tbl_Transactions> lstTrans = new List<tbl_Transactions>();
                BusinessRulesManager buzRuleManager = new BusinessRulesManager();

                lstTrans = buzRuleManager.GetAllTransactions(designAccelaratorID);
                lstTransactions = lstTrans;
            }
            catch (Exception)
            {

                throw;
            }

        }

        #endregion

        #region GetAllAttributeValues

        public void GetAllAttributeValues(int? attributeID)
        {
            try
            {
                IList<tbl_AttributeValues> lstAttrVal = new List<tbl_AttributeValues>();
                BusinessRulesManager buzRuleManager = new BusinessRulesManager();

                lstAttrVal = buzRuleManager.GetAllAttributeValues(attributeID);
                lstAttributeValues = lstAttrVal;
            }
            catch (Exception)
            {

                throw;
            }

        }

        #endregion

        #region SaveBusinessRulesData
        public int SaveBusinessRulesData(IList<BusinessRulesViewModel> buzrules, int daId)
        {
            try
            {
                int result = 0;
                BusinessRulesManager buzrulesManager = new BusinessRulesManager();
                List<tbl_BusinessRule> lstBusinessRules = new List<tbl_BusinessRule>();
                List<tbl_BuzRulesAttrMapping> lstBuzRulesAttrMapping = new List<tbl_BuzRulesAttrMapping>();


                var AllBuzRules = buzrules.GroupBy(b => b.BuzRuleAttrMapID);

                foreach (var buz in AllBuzRules)
                {
                    tbl_BuzRulesAttrMapping tblbuzruleattrmapping = new tbl_BuzRulesAttrMapping();

                    if ((buz.Key.ToString().Length > 5) && (buz.Key.ToString().Substring(buz.Key.ToString().Length - 2, 2) == "00"))
                    {
                        #region Add New Business Rules

                        int cnt = 1;
                        var cur = buzrules.Where(e => e.BuzRuleAttrMapID == buz.Key && e.IsLinked.Equals(true)); //Querying checked highlevel transactions and BuzruleAttrMapID marked new
                        foreach (var HighTrans in cur)
                        {
                            if (cnt == 1)
                            {
                                AttributesMapping(ref tblbuzruleattrmapping, HighTrans);
                                tblbuzruleattrmapping.EntityState = DA.DomainModel.EntityState.Added;
                            }
                            cnt++;

                            tbl_BusinessRule businessrule = new tbl_BusinessRule();
                            businessrule.daId = daId;
                            businessrule.TransactionSeq = HighTrans.TransactionSeq;

                            businessrule.tbl_BuzRulesAttrMapping = tblbuzruleattrmapping;
                            businessrule.EntityState = DA.DomainModel.EntityState.Added;
                            tblbuzruleattrmapping.tbl_BusinessRule.Add(businessrule);
                        }
                        lstBuzRulesAttrMapping.Add(tblbuzruleattrmapping);

                        #endregion
                    }
                    else
                    {
                        #region AddModifyDelete Business Rules

                        int cnt1 = 1;
                        var cur = buzrules.Where(e => e.BuzRuleAttrMapID == buz.Key && (e.IsLinked.Equals(true) || e.BuzRuleID != 0)); //Querying checked highlevel transactions and BuzruleID existed
                        foreach (var HighTrans in cur)
                        {
                            if (cnt1 == 1)
                            {
                                tblbuzruleattrmapping.BuzRulesAttrMapID = HighTrans.BuzRuleAttrMapID;
                                AttributesMapping(ref tblbuzruleattrmapping, HighTrans);
                                tblbuzruleattrmapping.EntityState = DA.DomainModel.EntityState.Modified;
                            }
                            cnt1++;
                            tbl_BusinessRule businessrule = new tbl_BusinessRule();
                            businessrule.daId = daId;
                            businessrule.TransactionSeq = HighTrans.TransactionSeq;
                            businessrule.BuzRuleAttrMapID = HighTrans.BuzRuleAttrMapID;
                            businessrule.BuzRuleID = HighTrans.BuzRuleID;

                            if ((HighTrans.BuzRuleID != 0) && (HighTrans.IsLinked == true))
                            {
                                businessrule.EntityState = DA.DomainModel.EntityState.Unchanged;
                            }
                            else if ((HighTrans.BuzRuleID == 0) && (HighTrans.IsLinked == true))
                            {
                                businessrule.EntityState = DA.DomainModel.EntityState.Added;
                            }
                            else if ((HighTrans.BuzRuleID != 0) && (HighTrans.IsLinked == false))
                            {
                                businessrule.EntityState = DA.DomainModel.EntityState.Deleted;
                            }

                            tblbuzruleattrmapping.tbl_BusinessRule.Add(businessrule);
                        }
                        lstBuzRulesAttrMapping.Add(tblbuzruleattrmapping);

                        #endregion
                    }
                }

                result = buzrulesManager.SaveBuzRulesDataMapping(lstBuzRulesAttrMapping);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region AttributesMapping

        private void AttributesMapping(ref tbl_BuzRulesAttrMapping tblbuzruleattrmapping, BusinessRulesViewModel HighTrans)
        {
            try
            {
                tblbuzruleattrmapping.ReqReference = HighTrans.ReqReference;
                tblbuzruleattrmapping.BuzRuleDesc = HighTrans.BuzRuleDesc;
                tblbuzruleattrmapping.Remarks = HighTrans.Remarks;
                tblbuzruleattrmapping.AttrID1 = HighTrans.AttrID1;
                tblbuzruleattrmapping.AttrValueID1 = HighTrans.AttrValueID1;
                tblbuzruleattrmapping.AttrID2 = HighTrans.AttrID2;
                tblbuzruleattrmapping.AttrValueID2 = HighTrans.AttrValueID2;
                tblbuzruleattrmapping.AttrID3 = HighTrans.AttrID3;
                tblbuzruleattrmapping.AttrValueID3 = HighTrans.AttrValueID3;
                tblbuzruleattrmapping.AttrID4 = HighTrans.AttrID4;
                tblbuzruleattrmapping.AttrValueID4 = HighTrans.AttrValueID4;
                tblbuzruleattrmapping.AttrID5 = HighTrans.AttrID5;
                tblbuzruleattrmapping.AttrValueID5 = HighTrans.AttrValueID5;
                tblbuzruleattrmapping.AttrID6 = HighTrans.AttrID6;
                tblbuzruleattrmapping.AttrValueID6 = HighTrans.AttrValueID6;
                tblbuzruleattrmapping.AttrID7 = HighTrans.AttrID7;
                tblbuzruleattrmapping.AttrValueID7 = HighTrans.AttrValueID7;
                tblbuzruleattrmapping.AttrID8 = HighTrans.AttrID8;
                tblbuzruleattrmapping.AttrValueID8 = HighTrans.AttrValueID8;
                tblbuzruleattrmapping.AttrID9 = HighTrans.AttrID9;
                tblbuzruleattrmapping.AttrValueID9 = HighTrans.AttrValueID9;
                tblbuzruleattrmapping.AttrID10 = HighTrans.AttrID10;
                tblbuzruleattrmapping.AttrValueID10 = HighTrans.AttrValueID10;
            }
            catch (Exception)
            {

                throw;
            }

        }

        #endregion

        public void DeleteBuzrulattrmap(int postData)
        {
            tbl_BuzRulesAttrMapping objbuzrulattrmap = new tbl_BuzRulesAttrMapping();
            BusinessRulesManager objbusinessrulemanager = new BusinessRulesManager();



            objbusinessrulemanager.DeleteBuzrulattrmap(postData);
 
        }
    }
}