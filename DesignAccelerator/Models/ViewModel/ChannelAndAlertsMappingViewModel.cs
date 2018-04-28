using DA.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace DesignAccelerator.Models.ViewModel
{
    public class ChannelAndAlertsMappingViewModel
    {
        #region Public properties
        public int ChannelAlert_Id { get; set; }
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
        public string message_Desc { get; set; }
        public string isManual { get; set; }
        public string modeType_Desc { get; set; }
        public string freq_Desc { get; set; }
        public int transactionSeq { get; set; }
        public int daId { get; set; }

        public IList<ChannelAndAlertsMappingViewModel> lstChannelAlertsData { get; set; }
        #endregion

        public IList<ChannelAndAlertsMappingViewModel> GetChannelAlertsList(int daId)
        {
            try
            {
                Mapping_ForAllManager mappingManager = new Mapping_ForAllManager();
                var channelAlertData = mappingManager.GetChannelAlertData(daId);

                IList<ChannelAndAlertsMappingViewModel> channelAlertMappingViewModelList = new List<ChannelAndAlertsMappingViewModel>();
                foreach (var item in channelAlertData)
                {
                    ChannelAndAlertsMappingViewModel channelAlertMappingViewModelItem = new ChannelAndAlertsMappingViewModel();
                    channelAlertMappingViewModelItem.ChannelAlert_Id = Convert.ToInt32(item.ChannelAlertID);
                    channelAlertMappingViewModelItem.message_Desc = item.MessageDesc;
                    channelAlertMappingViewModelItem.isManual = item.IsManual;
                    channelAlertMappingViewModelItem.modeType_Desc = item.ModeTypeDesc;
                    channelAlertMappingViewModelItem.freq_Desc = item.FreqTypeDesc;
                    channelAlertMappingViewModelItem.transactionSeq = Convert.ToInt32(item.TransactionSeq);
                    channelAlertMappingViewModelItem.Test_Condition_ID = item.HighLevelTxnDesc;
                    channelAlertMappingViewModelItem.attr1 = item.AttributeDesc1;
                    channelAlertMappingViewModelItem.attrValue1 = item.AttributeValue1;
                    channelAlertMappingViewModelItem.attr2 = item.AttributeDesc2;
                    channelAlertMappingViewModelItem.attrValue2 = item.AttributeValue2;
                    channelAlertMappingViewModelItem.attr3 = item.AttributeDesc3;
                    channelAlertMappingViewModelItem.attrValue3 = item.AttributeValue3;
                    channelAlertMappingViewModelItem.attr4 = item.AttributeDesc4;
                    channelAlertMappingViewModelItem.attrValue4 = item.AttributeValue4;
                    channelAlertMappingViewModelItem.attr5 = item.AttributeDesc5;
                    channelAlertMappingViewModelItem.attrValue5 = item.AttributeValue5;
                    channelAlertMappingViewModelItem.attr6 = item.AttributeDesc6;
                    channelAlertMappingViewModelItem.attrValue6 = item.AttributeValue6;
                    channelAlertMappingViewModelItem.attr7 = item.AttributeDesc7;
                    channelAlertMappingViewModelItem.attrValue7 = item.AttributeValue7;
                    channelAlertMappingViewModelItem.attr8 = item.AttributeDesc8;
                    channelAlertMappingViewModelItem.attrValue8 = item.AttributeValue8;
                    channelAlertMappingViewModelItem.attr9 = item.AttributeDesc9;
                    channelAlertMappingViewModelItem.attrValue9 = item.AttributeValue9;
                    channelAlertMappingViewModelItem.attr10 = item.AttributeDesc10;
                    channelAlertMappingViewModelItem.attrValue10 = item.AttributeValue10;

                    channelAlertMappingViewModelList.Add(channelAlertMappingViewModelItem);
                }
                return channelAlertMappingViewModelList;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public static DataTable CreateChannelAlertDataTable(IList<ChannelAndAlertsMappingViewModel> lstChannelAlert, int transSeq, DataTable dtRuleN)
        {
            try
            {
                DataTable dtDb = new DataTable();

                var lstPerTrans = lstChannelAlert.Where(e => e.transactionSeq == transSeq);

                //Converting the result list into a datatable
                foreach (PropertyInfo info in typeof(ChannelAndAlertsMappingViewModel).GetProperties())
                {
                    dtDb.Columns.Add(new DataColumn(info.Name, info.PropertyType));
                }
                foreach (var t in lstPerTrans)
                {
                    DataRow row = dtDb.NewRow();
                    foreach (PropertyInfo info in typeof(ChannelAndAlertsMappingViewModel).GetProperties())
                    {
                        row[info.Name] = info.GetValue(t, null);
                    }
                    dtDb.Rows.Add(row);
                }
                //To get the attributes selected for channels & Alerts
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
                lstHeaders.Add("ChannelAlert_ID");
                lstHeaders.Add("Test_Cond_ID");
                foreach (var att in lstAttributes)
                {
                    lstHeaders.Add(att);
                }
                lstHeaders.Add("Message Description");
                lstHeaders.Add("Channel");
                lstHeaders.Add("Mode");
                lstHeaders.Add("Frequency");

                //Mapping attributes from Rule of N table and channelAlerts
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
                var alertID = new StringBuilder(ID);
                alertID = alertID.Remove(alertID.Length - 2, 2);
                alertID.Remove(0, 2).Insert(0, "CA");


                //Creating datatable for generating channelAlert mapping table
                DataTable dt = new DataTable();

                //Add columns names of the datatable
                for (int i = 0; i < lstHeaders.Count; i++)
                {
                    dt.Columns.Add(lstHeaders[i]);
                }

                //Iterating through channelAlert data from DB to create rows of the channelAlerts mapping table
                for (int i = 0; i < dtDb.Rows.Count; i++)
                {
                    int j;
                    string testCondition = "";

                    DataRow dr = dt.NewRow();

                    dr[dt.Columns[0].ToString()] = alertID + ((i + 1).ToString("D3"));

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
                                        //To check for Alert Channel value
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
                                        //commented as it was restricting Alert Channel values - 14/06/2017
                                        //if (testCondition == tempTCID1)
                                        //   testCondition = tempTCID1;
                                        //else
                                        //    testCondition = "";
                                    }
                                }
                                k++;
                            }
                            else if (dt.Columns[k].ToString() != "Message Description")
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
                            if (dtDb.Rows[i][j].ToString() == "N" || dtDb.Rows[i][j].ToString() == "Y")
                            {
                                if (dtDb.Rows[i][j].ToString() == "N")
                                    dr[dt.Columns[k].ToString()] = "Autogenerated";
                                else
                                    dr[dt.Columns[k].ToString()] = "Manual";
                            }
                            else
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