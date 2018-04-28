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

namespace DesignAccelerator.Models.ViewModel
{
    public class BCIRMappingViewModel
    {
        #region Public properties
        public int BuzRuleID { get; set; }
        public int ChannelAlert_Id { get; set; }
        public int Interface_Id { get; set; }
        public int report_ID { get; set; }

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
        public int daId { get; set; }

        //BusinessRulesMapping
        public string Rule_Description { get; set; }
        public int transactionSeq { get; set; }
        //Channels&AlertsMapping
        public string message_Desc { get; set; }
        public string isManual { get; set; }
        public string modeType_Desc { get; set; }
        public string freq_Desc { get; set; }
        //InterfaceMapping
        public string Source_Desc { get; set; }
        public string Dest_Desc { get; set; }
        public string Mode_Desc { get; set; }
        //ReportsMapping
        public string reportName { get; set; }
        public string Period_Desc { get; set; }

        public IList<BCIRMappingViewModel> lstBuzRulesData { get; set; }
        public IList<BCIRMappingViewModel> lstChannelAlertsData { get; set; }
        public IList<BCIRMappingViewModel> lstinterfaceData { get; set; }
        public IList<BCIRMappingViewModel> lstReportData { get; set; }
        public IList<tbl_Transactions> lstHighLevelTxns { get; set; }
        #endregion

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

        DataTable dtDb = new DataTable();

        #region BusinessRulesMapping
        public IList<BCIRMappingViewModel> GetBuzRulesList(int daId)
        {
            try
            {
                Mapping_ForAllManager mappingManager = new Mapping_ForAllManager();
                var buzRulesData = mappingManager.GetBuzRulesMappingData(daId);

                IList<BCIRMappingViewModel> buzRulesMappingViewModelList = new List<BCIRMappingViewModel>();
                foreach (var item in buzRulesData)
                {
                    BCIRMappingViewModel buzRulesMappingViewModelItem = new BCIRMappingViewModel();
                    buzRulesMappingViewModelItem.BuzRuleID = Convert.ToInt32(item.BuzRuleID);
                    buzRulesMappingViewModelItem.Rule_Description = item.BuzRuleDesc;
                    buzRulesMappingViewModelItem.transactionSeq = Convert.ToInt32(item.TransactionSeq);
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

        public static DataTable CreateBuzRulesDataTable(IList<BCIRMappingViewModel> lstBuzRules, int transSeq)
        {
            try
            {
                DataTable dtDb = new DataTable();

                var lstPerTrans = lstBuzRules.Where(e => e.transactionSeq == transSeq);

                //Converting the result list into a datatable
                foreach (PropertyInfo info in typeof(BCIRMappingViewModel).GetProperties())
                {
                    dtDb.Columns.Add(new DataColumn(info.Name, info.PropertyType));
                }
                foreach (var t in lstPerTrans)
                {
                    DataRow row = dtDb.NewRow();
                    foreach (PropertyInfo info in typeof(BCIRMappingViewModel).GetProperties())
                    {
                        row[info.Name] = info.GetValue(t, null);
                    }
                    dtDb.Rows.Add(row);
                }

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

                foreach (var att in lstAttributes)
                {
                    lstHeaders.Add(att);
                }
                lstHeaders.Add("Test_Cond_ID");
                lstHeaders.Add("Rule Description");

                DataTable dt = new DataTable();

                for (int i = 0; i < lstHeaders.Count; i++)
                {
                    dt.Columns.Add(lstHeaders[i]);
                }

                //check number of records in table
                for (int i = 0; i < dtDb.Rows.Count; i++)
                {
                    int j;
                    DataRow dr = dt.NewRow();
                    dr[dt.Columns[0].ToString()] = dtDb.Rows[i][0].ToString();
                    for (int k = 1; k < dt.Columns.Count; k++)
                    {
                        for (j = 2; j < 21; j = j + 2)
                        {
                            if (dt.Columns[k].ToString() == dtDb.Rows[i][j - 1].ToString())
                            {
                                dr[dt.Columns[k].ToString()] = dtDb.Rows[i][j].ToString();
                                k++;
                            }
                            else if (dt.Columns[k].ToString() != "Test_Cond_ID")
                            {
                                dr[dt.Columns[k].ToString()] = "";
                                k++;
                            }
                        }
                        for (j = 21; j < dtDb.Columns.Count - 3; j++)
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
        #endregion

        #region ChannelsAlertsMapping
        public IList<BCIRMappingViewModel> GetChannelAlertsList(int daId)
        {
            try
            {
                Mapping_ForAllManager mappingManager = new Mapping_ForAllManager();
                var channelAlertData = mappingManager.GetChannelAlertData(daId);

                IList<BCIRMappingViewModel> channelAlertMappingViewModelList = new List<BCIRMappingViewModel>();
                foreach (var item in channelAlertData)
                {
                    BCIRMappingViewModel channelAlertMappingViewModelItem = new BCIRMappingViewModel();
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

        public static DataTable CreateChannelAlertDataTable(IList<BCIRMappingViewModel> lstChannelAlert, int transSeq)
        {
            try
            {
                DataTable dtDb = new DataTable();

                var lstPerTrans = lstChannelAlert.Where(e => e.transactionSeq == transSeq);

                //Converting the result list into a datatable
                foreach (PropertyInfo info in typeof(BCIRMappingViewModel).GetProperties())
                {
                    dtDb.Columns.Add(new DataColumn(info.Name, info.PropertyType));
                }
                foreach (var t in lstPerTrans)
                {
                    DataRow row = dtDb.NewRow();
                    foreach (PropertyInfo info in typeof(BCIRMappingViewModel).GetProperties())
                    {
                        row[info.Name] = info.GetValue(t, null);
                    }
                    dtDb.Rows.Add(row);
                }

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
                foreach (var att in lstAttributes)
                {
                    lstHeaders.Add(att);
                }
                lstHeaders.Add("Test_Cond_ID");
                lstHeaders.Add("Message Description");
                lstHeaders.Add("Channel");
                lstHeaders.Add("Mode");
                lstHeaders.Add("Frequency");

                DataTable dt = new DataTable();

                for (int i = 0; i < lstHeaders.Count; i++)
                {
                    dt.Columns.Add(lstHeaders[i]);
                }

                for (int i = 0; i < dtDb.Rows.Count; i++)
                {
                    int j;
                    DataRow dr = dt.NewRow();

                    dr[dt.Columns[0].ToString()] = dtDb.Rows[i][0].ToString();
                    for (int k = 1; k < dt.Columns.Count; k++)
                    {
                        for (j = 2; j < 21; j = j + 2)
                        {
                            if (dt.Columns[k].ToString() == dtDb.Rows[i][j - 1].ToString())
                            {
                                dr[dt.Columns[k].ToString()] = dtDb.Rows[i][j].ToString();
                                k++;
                            }
                            else if (dt.Columns[k].ToString() != "Test_Cond_ID")
                            {
                                dr[dt.Columns[k].ToString()] = "";
                                k++;
                            }

                        }
                        for (j = 21; j < dtDb.Columns.Count - 3; j++)
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
        #endregion

        #region InterfaceMapping
        public IList<BCIRMappingViewModel> GetInterfaceList(int daId)
        {
            try
            {
                Mapping_ForAllManager mappingManager = new Mapping_ForAllManager();
                var interfaceData = mappingManager.GetInterfaceMappingData(daId);

                IList<BCIRMappingViewModel> interfaceMappingViewModelList = new List<BCIRMappingViewModel>();
                foreach (var item in interfaceData)
                {
                    BCIRMappingViewModel interfaceMappingViewModelItem = new BCIRMappingViewModel();
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

        public static DataTable CreateInterfaceDataTable(IList<BCIRMappingViewModel> lstInterface, int transSeq)
        {
            try
            {
                DataTable dtDb = new DataTable();

                var lstPerTrans = lstInterface.Where(e => e.transactionSeq == transSeq);

                //Converting the result list into a datatable
                foreach (PropertyInfo info in typeof(BCIRMappingViewModel).GetProperties())
                {
                    dtDb.Columns.Add(new DataColumn(info.Name, info.PropertyType));
                }
                foreach (var t in lstPerTrans)
                {
                    DataRow row = dtDb.NewRow();
                    foreach (PropertyInfo info in typeof(BCIRMappingViewModel).GetProperties())
                    {
                        row[info.Name] = info.GetValue(t, null);
                    }
                    dtDb.Rows.Add(row);
                }
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

                foreach (var att in lstAttributes)
                {
                    lstHeaders.Add(att);
                }
                lstHeaders.Add("Test_Cond_ID");
                lstHeaders.Add("Rule Description");
                lstHeaders.Add("Source");
                lstHeaders.Add("Destination");
                lstHeaders.Add("Mode");


                DataTable dt = new DataTable();

                for (int i = 0; i < lstHeaders.Count; i++)
                {
                    dt.Columns.Add(lstHeaders[i]);
                }

                for (int i = 0; i < dtDb.Rows.Count; i++)
                {
                    int j;
                    DataRow dr = dt.NewRow();

                    dr[dt.Columns[0].ToString()] = dtDb.Rows[i][0].ToString();

                    for (int k = 1; k < dt.Columns.Count; k++)
                    {
                        for (j = 2; j < 21; j = j + 2)
                        {
                            if (dt.Columns[k].ToString() == dtDb.Rows[i][j - 1].ToString())
                            {
                                dr[dt.Columns[k].ToString()] = dtDb.Rows[i][j].ToString();
                                k++;

                            }
                            else if (dt.Columns[k].ToString() != "Test_Cond_ID")
                            {
                                dr[dt.Columns[k].ToString()] = "";
                                k++;
                            }

                        }
                        for (j = 21; j < dtDb.Columns.Count - 3; j++)
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
        #endregion

        #region ReportsMapping
        public IList<BCIRMappingViewModel> GetReportsList(int daId)
        {
            try
            {
                Mapping_ForAllManager mappingManager = new Mapping_ForAllManager();
                var reportsData = mappingManager.GetReportsData(daId);

                IList<BCIRMappingViewModel> reportsMappingViewModelList = new List<BCIRMappingViewModel>();
                foreach (var item in reportsData)
                {
                    BCIRMappingViewModel reportMappingViewModelItem = new BCIRMappingViewModel();
                    reportMappingViewModelItem.report_ID = Convert.ToInt32(item.ReportID);
                    reportMappingViewModelItem.Period_Desc = item.PeriodTypeDesc;
                    reportMappingViewModelItem.transactionSeq = Convert.ToInt32(item.TransactionSeq); ;
                    reportMappingViewModelItem.Test_Condition_ID = item.HighLevelTxnDesc;
                    reportMappingViewModelItem.reportName = item.ReportName;

                    reportsMappingViewModelList.Add(reportMappingViewModelItem);
                }
                return reportsMappingViewModelList;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public static DataTable CreateReportsDataTable(IList<BCIRMappingViewModel> lstReports, int transSeq)
        {
            try
            {
                DataTable dtDb = new DataTable();

                var lstPerTrans = lstReports.Where(e => e.transactionSeq == transSeq);

                //Converting the result list into a datatable
                foreach (PropertyInfo info in typeof(BCIRMappingViewModel).GetProperties())
                {
                    dtDb.Columns.Add(new DataColumn(info.Name, info.PropertyType));
                }
                foreach (var t in lstPerTrans)
                {
                    DataRow row = dtDb.NewRow();
                    foreach (PropertyInfo info in typeof(BCIRMappingViewModel).GetProperties())
                    {
                        row[info.Name] = info.GetValue(t, null);
                    }
                    dtDb.Rows.Add(row);
                }

                List<string> lstHeaders = new List<string>();
                lstHeaders.Add("Report_ID");
                lstHeaders.Add("Report Name");
                lstHeaders.Add("Period");
                lstHeaders.Add("Test_Cond_ID");

                DataTable dt = new DataTable();

                for (int i = 0; i < lstHeaders.Count; i++)
                {
                    dt.Columns.Add(lstHeaders[i]);
                }

                for (int i = 0; i < dtDb.Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();

                    for (int k = 0; k < dt.Columns.Count; k++)
                    {
                        dr[dt.Columns[k].ToString()] = dtDb.Rows[i][k].ToString();
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
        #endregion
    }
}

