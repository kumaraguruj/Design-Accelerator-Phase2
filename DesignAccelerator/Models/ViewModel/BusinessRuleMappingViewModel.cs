using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DA.DomainModel;
using DA.BusinessLayer;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using System.Data;
using System.Reflection;
using System.Text;

namespace DesignAccelerator.Models.ViewModel
{
    public class BusinessRuleMappingViewModel
    {
        #region Public properties
        public int BuzRuleID { get; set; }
        public string attr1 { get; set; }
        public string attrValue1 { get; set; }
        public string attr2 { get; set; }
        public string attrValue2 { get; set; }
        public string attr3 { get; set; }
        public string attrValue3 { get; set; }
        public string attr4 { get; set; }
        public string attrValue4 { get; set; }
        public string attr5 { get; set; }
        public string attrValue5 { get; set; }
        public string attr6 { get; set; }
        public string attrValue6 { get; set; }
        public string attr7 { get; set; }
        public string attrValue7 { get; set; }
        public string attr8 { get; set; }
        public string attrValue8 { get; set; }
        public string attr9 { get; set; }
        public string attrValue9 { get; set; }
        public string attr10 { get; set; }
        public string attrValue10 { get; set; }
        public string Test_Condition_ID { get; set; }
        public string Rule_Description { get; set; }
        public int TransactionSeq { get; set; }
        public int daId { get; set; }

        public IList<BusinessRuleMappingViewModel> lstBuzRulesData { get; set; }
        #endregion

        public IList<BusinessRuleMappingViewModel> GetBuzRulesList(int daId)
        {
            try
            {
                Mapping_ForAllManager mappingManager = new Mapping_ForAllManager();
                var buzRulesData = mappingManager.GetBuzRulesMappingData(daId);

                IList<BusinessRuleMappingViewModel> buzRulesMappingViewModelList = new List<BusinessRuleMappingViewModel>();
                foreach (var item in buzRulesData)
                {
                    BusinessRuleMappingViewModel buzRulesMappingViewModelItem = new BusinessRuleMappingViewModel();
                    buzRulesMappingViewModelItem.BuzRuleID = Convert.ToInt32(item.BuzRuleID);
                    buzRulesMappingViewModelItem.Rule_Description = item.BuzRuleDesc;
                    buzRulesMappingViewModelItem.TransactionSeq = Convert.ToInt32(item.TransactionSeq);
                    buzRulesMappingViewModelItem.Test_Condition_ID = item.HighLevelTxnDesc;
                    buzRulesMappingViewModelItem.attr1 = item.AttributeDesc1;
                    buzRulesMappingViewModelItem.attrValue1 = item.AttributeValue1;
                    buzRulesMappingViewModelItem.attr2 = item.AttributeDesc2;
                    buzRulesMappingViewModelItem.attrValue2 = item.AttributeValue2;
                    buzRulesMappingViewModelItem.attr3 = item.AttributeDesc3;
                    buzRulesMappingViewModelItem.attrValue3 = item.AttributeValue3;
                    buzRulesMappingViewModelItem.attr4 = item.AttributeDesc4;
                    buzRulesMappingViewModelItem.attrValue4 = item.AttributeValue4;
                    buzRulesMappingViewModelItem.attr5 = item.AttributeDesc5;
                    buzRulesMappingViewModelItem.attrValue5 = item.AttributeValue5;
                    buzRulesMappingViewModelItem.attr6 = item.AttributeDesc6;
                    buzRulesMappingViewModelItem.attrValue6 = item.AttributeValue6;
                    buzRulesMappingViewModelItem.attr7 = item.AttributeDesc7;
                    buzRulesMappingViewModelItem.attrValue7 = item.AttributeValue7;
                    buzRulesMappingViewModelItem.attr8 = item.AttributeDesc8;
                    buzRulesMappingViewModelItem.attrValue8 = item.AttributeValue8;
                    buzRulesMappingViewModelItem.attr9 = item.AttributeDesc9;
                    buzRulesMappingViewModelItem.attrValue9 = item.AttributeValue9;
                    buzRulesMappingViewModelItem.attr10 = item.AttributeDesc10;
                    buzRulesMappingViewModelItem.attrValue10 = item.AttributeValue10;

                    buzRulesMappingViewModelList.Add(buzRulesMappingViewModelItem);
                }
                return buzRulesMappingViewModelList;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public static DataTable CreateBuzRulesDataTable(IList<BusinessRuleMappingViewModel> lstBuzRules, int transSeq, DataTable dtRuleN)
        {
            try
            {
                DataTable dtDb = new DataTable();

                var lstPerTrans = lstBuzRules.Where(e => e.TransactionSeq == transSeq);

                //Converting the result list into a datatable
                foreach (PropertyInfo info in typeof(BusinessRuleMappingViewModel).GetProperties())
                {
                    dtDb.Columns.Add(new DataColumn(info.Name, info.PropertyType));
                }
                foreach (var t in lstPerTrans)
                {
                    DataRow row = dtDb.NewRow();
                    foreach (PropertyInfo info in typeof(BusinessRuleMappingViewModel).GetProperties())
                    {
                        row[info.Name] = info.GetValue(t, null);
                    }
                    dtDb.Rows.Add(row);
                }
                //To get the attributes selected for businessRules
                List<string> lstAttributes = new List<string>();
                foreach (DataRow r in dtDb.Rows)
                {
                    int j = 1;
                    while (j <= 19)
                    {
                        if (r[j].ToString() != "" && !lstAttributes.Contains(r[j].ToString()))
                            lstAttributes.Add(r[j].ToString());
                        j = j + 2;
                    }
                }

                List<string> lstHeaders = new List<string>();
                lstHeaders.Add("Business_Rule_ID");
                lstHeaders.Add("Test_Cond_ID");
                foreach (var att in lstAttributes)
                {
                    lstHeaders.Add(att);
                }
                lstHeaders.Add("Rule Description");

                //Mapping attributes from Rule of N table and businessRule
                List<string> lstMappedAttr = new List<string>();

                for (int m = 0; m < dtRuleN.Columns.Count; m++)
                {
                    if (lstAttributes.Contains(dtRuleN.Columns[m].ToString()))
                    {
                        lstMappedAttr.Add(dtRuleN.Columns[m].ToString());
                    }
                }

                //to Create the ID's in mapping tables
                var ID = dtRuleN.Rows[0][0].ToString();
                var buzRuleID = new StringBuilder(ID);
                buzRuleID = buzRuleID.Remove(buzRuleID.Length - 2, 2);
                buzRuleID.Remove(0, 2).Insert(0, "BR");

                //Creating datatable for generating businessRule mapping table
                DataTable dt = new DataTable();

                //Add columns names of the datatable
                for (int i = 0; i < lstHeaders.Count; i++)
                {
                    dt.Columns.Add(lstHeaders[i]);
                }

                //Iterating through businessRule data from DB to create rows of the businessRule mapping table
                for (int i = 0; i < dtDb.Rows.Count; i++)
                {
                    int j;
                    string testCondition = "";

                    DataRow dr = dt.NewRow();
                    dr[dt.Columns[0].ToString()] = buzRuleID + ((i + 1).ToString("D3"));

                    //to set testcondition
                    dr[dt.Columns[1].ToString()] = testCondition;

                    //To fill data into each column
                    for (int k = 2; k < dt.Columns.Count; k++)
                    {
                        //to store TCID temporarily for comparing
                        string tempTCID1 = "";

                        //To fill data of each matching attribute
                        for (j = 2; j < 21; j = j + 2)
                        {
                            if (dt.Columns[k].ToString() == dtDb.Rows[i][j - 1].ToString())
                            {
                                dr[dt.Columns[k].ToString()] = dtDb.Rows[i][j].ToString();

                                //To check if attribute is mapped in RuleOfN table
                                if (lstMappedAttr.Contains(dt.Columns[k].ToString()))
                                {
                                    //Adding rule of n table loop                                                            
                                    for (int rm = 0; rm < dtRuleN.Rows.Count; rm++)
                                    {
                                        string test = dtRuleN.Rows[rm][dt.Columns[k].ToString()].ToString();
                                        if (dtRuleN.Rows[rm][dt.Columns[k].ToString()].ToString() == dtDb.Rows[i][j].ToString())
                                        {
                                            tempTCID1 = dtRuleN.Rows[rm][1].ToString();
                                            break;//break out of rm loop
                                        }
                                    }//end of rm loop

                                    //To set the testcondition ID 
                                    if (testCondition == "")
                                        testCondition = tempTCID1;
                                    else
                                    {
                                        if (testCondition == tempTCID1)
                                            testCondition = tempTCID1;
                                        else
                                            testCondition = "";
                                    }
                                }
                                k++;
                            }
                            else if (dt.Columns[k].ToString() != "Rule Description")
                            {
                                dr[dt.Columns[k].ToString()] = "";
                                k++;
                                j = j - 2;//to handle first column null situation
                            }
                        }//end of j loop

                        //to fill value of test condition
                        if (testCondition != "")
                        {
                            //to set testcondition
                            dr[dt.Columns[1].ToString()] = testCondition;
                        }

                        //For remaining columns
                        //skipping test condition which is j=21
                        for (j = 22; j < dtDb.Columns.Count - 3; j++)
                        {
                            dr[dt.Columns[k].ToString()] = dtDb.Rows[i][j].ToString();
                            k++;
                        }
                    }
                    dt.Rows.Add(dr);
                }
                return dt;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}