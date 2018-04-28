using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using DA.DomainModel;
using DA.DataAccessLayer;

namespace DA.BusinessLayer
{
    public class BusinessRulesManager
    {
        public List<sp_GetAllBusinessRules_Result> GetBusinessRules(int daId)
        {
            try
            {
                IGenericDataRepository<sp_GetAllBusinessRules_Result> repository = new GenericDataRepository<sp_GetAllBusinessRules_Result>();
                return repository.ExecuteStoredProcedure("EXEC sp_GetAllBusinessRules @daId", new SqlParameter("daId", SqlDbType.Int) { Value = daId }).ToList();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public IList<tbl_Attribute> GetAllAttributes(int? daId)
        {
           
            try
            {
                IGenericDataRepository<tbl_Attribute> repository = new GenericDataRepository<tbl_Attribute>();
                IList<tbl_Attribute> lstAttributes = repository.GetList(e => e.daId.Equals(daId)).OrderBy(e => e.AttributeDesc).ToList();

                return lstAttributes;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public IList<tbl_Transactions> GetAllTransactions(int? daId)
        {
            try
            {
                IGenericDataRepository<tbl_Transactions> repository = new GenericDataRepository<tbl_Transactions>();
                IList<tbl_Transactions> lstTransactions = repository.GetList(e => e.daId.Equals(daId));

                return lstTransactions;
            }
            catch (Exception)
            {

                throw;
            }

        }


        public IList<tbl_AttributeValues> GetAllAttributeValues(int? attrID)
        {
            try
            {
                IGenericDataRepository<tbl_AttributeValues> repository = new GenericDataRepository<tbl_AttributeValues>();
                IList<tbl_AttributeValues> lstAttrValues = repository.GetList(e => e.AttributeID.Equals(attrID));

                return lstAttrValues;
            }
            catch (Exception)
            {

                throw;
            }
        }

        

        public void AddBuzRulesAttrMapping(tbl_BuzRulesAttrMapping tblbuzruleattrmapping)
        {
            try
            {
                IGenericDataRepository<tbl_BuzRulesAttrMapping> repository = new GenericDataRepository<tbl_BuzRulesAttrMapping>();
                repository.Add(tblbuzruleattrmapping);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddBuzRules(tbl_BusinessRule tblbuzrules)
        {
            try
            {
                IGenericDataRepository<tbl_BusinessRule> repository = new GenericDataRepository<tbl_BusinessRule>();
                repository.Add(tblbuzrules);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public int SaveBuzRulesDataMapping(IList<tbl_BuzRulesAttrMapping> tblBusinessRulesAttrMapping)
        {
            try
            {
                IGenericDataRepository<tbl_BuzRulesAttrMapping> repository = new GenericDataRepository<tbl_BuzRulesAttrMapping>();
                foreach (var item in tblBusinessRulesAttrMapping)
                    repository.Add(item);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public void DeleteBuzrulattrmap(int id)
        {
            try
            {

                tbl_BusinessRule objBusinessRule = new tbl_BusinessRule();
                tbl_BuzRulesAttrMapping objtblBuzRuleAttrMap = new tbl_BuzRulesAttrMapping();
                IGenericDataRepository<tbl_BusinessRule> repository = new GenericDataRepository<tbl_BusinessRule>();
                IGenericDataRepository<tbl_BuzRulesAttrMapping> repository1 = new GenericDataRepository<tbl_BuzRulesAttrMapping>();
                IList<tbl_BusinessRule> lstBusinessRule = repository.GetList(q => q.BuzRuleID == id);

                if (lstBusinessRule != null)
                {
                    foreach (var item in lstBusinessRule)
                    {

                        objBusinessRule.BuzRuleID = item.BuzRuleID;
                        objBusinessRule.BuzRuleAttrMapID = item.BuzRuleAttrMapID;
                        objBusinessRule.daId = item.daId;
                        objBusinessRule.TransactionSeq = item.TransactionSeq;
                        objBusinessRule.EntityState = EntityState.Deleted;
                        var buzruleId = item.BuzRuleAttrMapID;

                        IList<tbl_BuzRulesAttrMapping> lstBuzRuleMapId = repository1.GetList(q => q.BuzRulesAttrMapID.Equals(item.BuzRuleAttrMapID));

                        if (lstBuzRuleMapId != null)
                        {
                            foreach (var item1 in lstBuzRuleMapId)
                            {
                                objtblBuzRuleAttrMap.AttrID1 = item1.AttrID1;
                                objtblBuzRuleAttrMap.AttrID2 = item1.AttrID2;
                                objtblBuzRuleAttrMap.AttrID3 = item1.AttrID3;
                                objtblBuzRuleAttrMap.AttrID4 = item1.AttrID4;
                                objtblBuzRuleAttrMap.AttrID5 = item1.AttrID5;
                                objtblBuzRuleAttrMap.AttrID6 = item1.AttrID6;
                                objtblBuzRuleAttrMap.AttrID7 = item1.AttrID7;
                                objtblBuzRuleAttrMap.AttrID8 = item1.AttrID8;
                                objtblBuzRuleAttrMap.AttrID9 = item1.AttrID9;
                                objtblBuzRuleAttrMap.AttrID10 = item1.AttrID10;
                                objtblBuzRuleAttrMap.AttrValueID1 = item1.AttrValueID1;
                                objtblBuzRuleAttrMap.AttrValueID2 = item1.AttrValueID2;
                                objtblBuzRuleAttrMap.AttrValueID3 = item1.AttrValueID3;
                                objtblBuzRuleAttrMap.AttrValueID4 = item1.AttrValueID4;
                                objtblBuzRuleAttrMap.AttrValueID5 = item1.AttrValueID5;
                                objtblBuzRuleAttrMap.AttrValueID6 = item1.AttrValueID6;
                                objtblBuzRuleAttrMap.AttrValueID7 = item1.AttrValueID7;
                                objtblBuzRuleAttrMap.AttrValueID8 = item1.AttrValueID8;
                                objtblBuzRuleAttrMap.AttrValueID9 = item1.AttrValueID9;
                                objtblBuzRuleAttrMap.AttrValueID10 = item1.AttrValueID10;
                                objtblBuzRuleAttrMap.BuzRuleDesc = item1.BuzRuleDesc;
                                objtblBuzRuleAttrMap.BuzRulesAttrMapID = item1.BuzRulesAttrMapID;
                                objtblBuzRuleAttrMap.Remarks = item1.Remarks;
                                objtblBuzRuleAttrMap.ReqReference = item1.ReqReference;
                                objtblBuzRuleAttrMap.EntityState = EntityState.Deleted;

                                repository1.Remove(objtblBuzRuleAttrMap);


                            }
                        }

                        repository.Remove(objBusinessRule);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
