using DA.BusinessLayer;
using DA.DomainModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace DesignAccelerator.Models.ViewModel
{
    public class ReportsMappingViewModel
    {
        #region Public properties
        public int report_ID { get; set; }
        public string reportName { get; set; }
        public string Period_Desc { get; set; }
        public string Test_Condition_ID { get; set; }
        public int transactionSeq { get; set; }
        public int daId { get; set; }

        public IList<ReportsMappingViewModel> lstReportData { get; set; }
        #endregion

        public IList<ReportsMappingViewModel> GetReportsList(int daId)
        {
            try
            {
                Mapping_ForAllManager mappingManager = new Mapping_ForAllManager();
                var reportsData = mappingManager.GetReportsData(daId);

                IList<ReportsMappingViewModel> reportsMappingViewModelList = new List<ReportsMappingViewModel>();
                foreach (var item in reportsData)
                {
                    ReportsMappingViewModel reportMappingViewModelItem = new ReportsMappingViewModel();
                    reportMappingViewModelItem.report_ID = Convert.ToInt32(item.ReportID);
                    reportMappingViewModelItem.Period_Desc = item.PeriodTypeDesc;
                    reportMappingViewModelItem.transactionSeq = Convert.ToInt32(item.TransactionSeq);
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

        public static DataTable CreateReportsDataTable(IList<ReportsMappingViewModel> lstReports, int transSeq, DataTable dtRuleN)
        {
            try
            {
                DataTable dtDb = new DataTable();

                var lstPerTrans = lstReports.Where(e => e.transactionSeq == transSeq);

                //Converting the result list into a datatable
                foreach (PropertyInfo info in typeof(ReportsMappingViewModel).GetProperties())
                {
                    dtDb.Columns.Add(new DataColumn(info.Name, info.PropertyType));
                }
                foreach (var t in lstPerTrans)
                {
                    DataRow row = dtDb.NewRow();
                    foreach (PropertyInfo info in typeof(ReportsMappingViewModel).GetProperties())
                    {
                        row[info.Name] = info.GetValue(t, null);
                    }
                    dtDb.Rows.Add(row);
                }

                List<string> lstHeaders = new List<string>();
                lstHeaders.Add("Report_ID");
                lstHeaders.Add("Test_Cond_ID");
                lstHeaders.Add("Report Name");
                lstHeaders.Add("Period");

                //to Create the ID's in mapping tables
                var ID = dtRuleN.Rows[0][0].ToString();
                var reportID = new StringBuilder(ID);
                reportID = reportID.Remove(reportID.Length - 2, 2);
                reportID.Remove(0, 2).Insert(0, "RE");

                //Creating datatable for generating reports mapping table
                DataTable dt = new DataTable();

                for (int i = 0; i < lstHeaders.Count; i++)
                {
                    dt.Columns.Add(lstHeaders[i]);
                }

                //Iterating through reports data from DB to create rows of the reports mapping table
                for (int i = 0; i < dtDb.Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();

                    dr[dt.Columns[0].ToString()] = reportID + ((i + 1).ToString("D3"));
                    //To add Test conditionID from Rule of N table
                    dr[dt.Columns[1].ToString()] = dtRuleN.Rows[0][1].ToString();
                    //remaining columns
                    dr[dt.Columns[2].ToString()] = dtDb.Rows[i][1].ToString();
                    dr[dt.Columns[3].ToString()] = dtDb.Rows[i][2].ToString();

                    dt.Rows.Add(dr);
                }

                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void DeleteReports(string postData, int id)
        {
            try
            {

                ReportManager rm = new ReportManager();
                rm.DeleteReport(postData, id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}