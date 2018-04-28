using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DesignAccelerator.Models.ViewModel;
using DA.DomainModel;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using DA.BusinessLayer;
using System.Reflection;
using System.Text;

namespace DesignAccelerator.Models.ViewModel
{
    public class InterfaceMappingViewModel
    {
        #region Public properties
        public int Interface_Id { get; set; }
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
        public string Source_Desc { get; set; }
        public string Dest_Desc { get; set; }
        public string Mode_Desc { get; set; }
        public int transactionSeq { get; set; }
        public int daId { get; set; }

        public IList<InterfaceMappingViewModel> lstinterfaceData { get; set; }
        public IList<tbl_Transactions> lstHighLevelTxns { get; set; }
        #endregion

        public IList<InterfaceMappingViewModel> GetInterfaceList(int daId)
        {
            try
            {
                Mapping_ForAllManager mappingManager = new Mapping_ForAllManager();
                var interfaceData = mappingManager.GetInterfaceMappingData(daId);

                IList<InterfaceMappingViewModel> interfaceMappingViewModelList = new List<InterfaceMappingViewModel>();
                foreach (var item in interfaceData)
                {
                    InterfaceMappingViewModel interfaceMappingViewModelItem = new InterfaceMappingViewModel();
                    interfaceMappingViewModelItem.Interface_Id = Convert.ToInt32(item.InterfaceID);
                    interfaceMappingViewModelItem.Rule_Description = item.InterfaceDesc;
                    interfaceMappingViewModelItem.Source_Desc = item.SourceDesc;
                    interfaceMappingViewModelItem.Dest_Desc = item.DestDesc;
                    interfaceMappingViewModelItem.Mode_Desc = item.ModeTypeDesc;
                    interfaceMappingViewModelItem.transactionSeq = Convert.ToInt32(item.TransactionSeq);
                    interfaceMappingViewModelItem.Test_Condition_ID = item.HighLevelTxnDesc;
                    interfaceMappingViewModelItem.attr1 = item.AttributeDesc1;
                    interfaceMappingViewModelItem.attrValue1 = item.AttributeValue1;
                    interfaceMappingViewModelItem.attr2 = item.AttributeDesc2;
                    interfaceMappingViewModelItem.attrValue2 = item.AttributeValue2;
                    interfaceMappingViewModelItem.attr3 = item.AttributeDesc3;
                    interfaceMappingViewModelItem.attrValue3 = item.AttributeValue3;
                    interfaceMappingViewModelItem.attr4 = item.AttributeDesc4;
                    interfaceMappingViewModelItem.attrValue4 = item.AttributeValue4;
                    interfaceMappingViewModelItem.attr5 = item.AttributeDesc5;
                    interfaceMappingViewModelItem.attrValue5 = item.AttributeValue5;
                    interfaceMappingViewModelItem.attr6 = item.AttributeDesc6;
                    interfaceMappingViewModelItem.attrValue6 = item.AttributeValue6;
                    interfaceMappingViewModelItem.attr7 = item.AttributeDesc7;
                    interfaceMappingViewModelItem.attrValue7 = item.AttributeValue7;
                    interfaceMappingViewModelItem.attr8 = item.AttributeDesc8;
                    interfaceMappingViewModelItem.attrValue8 = item.AttributeValue8;
                    interfaceMappingViewModelItem.attr9 = item.AttributeDesc9;
                    interfaceMappingViewModelItem.attrValue9 = item.AttributeValue9;
                    interfaceMappingViewModelItem.attr10 = item.AttributeDesc10;
                    interfaceMappingViewModelItem.attrValue10 = item.AttributeValue10;

                    interfaceMappingViewModelList.Add(interfaceMappingViewModelItem);
                }
                return interfaceMappingViewModelList;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public TransactionsViewModel GetTransactionsList(int daId)
        {
            try
            {
                TransactionsViewModel transactionViewModel = new TransactionsViewModel();
                return transactionViewModel.GetTransactions(daId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static DataTable CreateInterfaceDataTable(IList<InterfaceMappingViewModel> lstInterface, int transSeq, DataTable dtRuleN)
        {
            try
            {
                DataTable dtDb = new DataTable();

                var lstPerTrans = lstInterface.Where(e => e.transactionSeq == transSeq);

                //Converting the result list into a datatable
                foreach (PropertyInfo info in typeof(InterfaceMappingViewModel).GetProperties())
                {
                    dtDb.Columns.Add(new DataColumn(info.Name, info.PropertyType));
                }
                foreach (var t in lstPerTrans)
                {
                    DataRow row = dtDb.NewRow();
                    foreach (PropertyInfo info in typeof(InterfaceMappingViewModel).GetProperties())
                    {
                        row[info.Name] = info.GetValue(t, null);
                    }
                    dtDb.Rows.Add(row);
                }

                //To get the attributes selected for interfaces
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
                lstHeaders.Add("Interface_ID");
                lstHeaders.Add("Test_Cond_ID");
                foreach (var att in lstAttributes)
                {
                    lstHeaders.Add(att);
                }
                lstHeaders.Add("Interface Description");
                lstHeaders.Add("Source");
                lstHeaders.Add("Destination");
                lstHeaders.Add("Mode");

                //Mapping attributes from Rule of N table and interfaces
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
                var interfaceID = new StringBuilder(ID);
                interfaceID = interfaceID.Remove(interfaceID.Length - 2, 2);
                interfaceID.Remove(0, 2).Insert(0, "IN");

                //Creating datatable for generating interface mapping table
                DataTable dt = new DataTable();

                //Add columns names of the datatable
                for (int i = 0; i < lstHeaders.Count; i++)
                {
                    dt.Columns.Add(lstHeaders[i]);
                }

                //Iterating through Interface data from DB to create rows of the interface mapping table
                for (int i = 0; i < dtDb.Rows.Count; i++)
                {
                    int j;
                    string testCondition = "";

                    DataRow dr = dt.NewRow();

                    dr[dt.Columns[0].ToString()] = interfaceID + ((i + 1).ToString("D3"));
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
                            else if (dt.Columns[k].ToString() != "Interface Description")
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
                        for (j = 22; j < dtDb.Columns.Count - 4; j++)
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