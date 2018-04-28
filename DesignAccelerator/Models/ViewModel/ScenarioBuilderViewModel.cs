using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using DA.DomainModel;
using DA.BusinessLayer;
using System.Reflection;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;


namespace DesignAccelerator.Models.ViewModel
{
    public class ScenarioBuilderViewModel : MappingViewModel
    {
        #region Public Properties
        public int daId { get; set; }

        //Col2 - High Level Transaction
        //public string TransactionID { get; set; }
        //public string TransactionDesc { get; set; }

        public int TransactionSeq { get; set; }

        //public IList<tbl_AttributeValues> lstAttributeValues { get; set; }

        //public IList<tbl_Attribute> lstCriticalAttributes { get; set; }

        //public IList<tbl_Transactions> lstHighLevelTransactions { get; set; }

        #endregion

        public IList<ScenarioBuilderViewModel> GetTransactions(int? daId)
        {
            try
            {
                ScenarioBuilderViewModel sbVM = new ScenarioBuilderViewModel();
                TransactionsManager transManager = new TransactionsManager();
                MappingManager mappingmanager = new MappingManager();

                lstHighLevelTransactions = transManager.GetAllTransactions(daId);

                List<ScenarioBuilderViewModel> lstScenarioBuilderVM = new List<ScenarioBuilderViewModel>();

                sbVM.lstHighLevelTransactions = lstHighLevelTransactions;

                var transactionAttributeMapping = mappingmanager.GetMappingDetails(daId);

                for (int j = 0; j < transactionAttributeMapping.Count(); j++)
                {
                    ScenarioBuilderViewModel scenarioBuilderViewModel = new ScenarioBuilderViewModel();
                    scenarioBuilderViewModel.TransactionDesc = transactionAttributeMapping[j].tbl_Transactions.HighLevelTxnDesc;
                }

                lstScenarioBuilderVM.Add(sbVM);
                return lstScenarioBuilderVM;
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


        //public static DataTable CreateTestScenarioDataTable(IList<ScenarioBuilderViewModel> lstScenarioBuilder, int transSeq, DataTable dtRuleN)
        //{
        //    //NRow keeps track of where to insert new rows in the destination workbook.
        //    //int NRow = 1;


        //    DataTable dtDb = new DataTable();

        //    var lstPerTrans = lstScenarioBuilder.Where(e => e.TransactionSeq == transSeq);

        //    //Converting the result list into a datatable
        //    foreach (PropertyInfo info in typeof(ScenarioBuilderViewModel).GetProperties())
        //    {
        //        dtDb.Columns.Add(new DataColumn(info.Name, info.PropertyType));
        //    }
        //    foreach (var t in lstPerTrans)
        //    {
        //        DataRow row = dtDb.NewRow();
        //        foreach (PropertyInfo info in typeof(ScenarioBuilderViewModel).GetProperties())
        //        {
        //            row[info.Name] = info.GetValue(t, null);
        //        }
        //        dtDb.Rows.Add(row);
        //    }
        //    List<string> lstAttributes = new List<string>();
        //    List<string> lstMappedAttr = new List<string>();
        //    for (int m = 0; m < dtRuleN.Columns.Count; m++)
        //    {
        //        if (lstAttributes.Contains(dtRuleN.Columns[m].ToString()))
        //        {
        //            lstMappedAttr.Add(dtRuleN.Columns[m].ToString());
        //        }
        //    }

        //    return dtDb;
        //}
    }
}
