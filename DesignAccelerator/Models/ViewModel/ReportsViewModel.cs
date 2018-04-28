using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DA.BusinessLayer;
using DA.DomainModel;
using System.Net;
using System.ComponentModel.DataAnnotations;

namespace DesignAccelerator.Models.ViewModel
{
    public class ReportsViewModel
    {
        #region Public Properties
        public int reportID { get; set; }

        public string reqReference { get; set; }

        public string reportName { get; set; }

        public string reportDesc { get; set; }
        public int transactionSeq { get; set; }
        public int sourceId { get; set; }
        public int periodId { get; set; }

        public int daId { get; set; }
        public string daName { get; set; }

        public string highLevelTxn { get; set; }
        public bool isLinked { get; set; }

        public IList<tbl_Transactions> lstHighLevelTxns { get; set; }
        public IList<ReportsViewModel> lstReports { get; set; }
        public IList<SourceViewModel> lstSource { get; set; }
        public IList<PeriodTypeViewModel> lstPeriodType { get; set; }
        //public IList<tbl_PeriodType> lstPeriodType { get; set; }

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

        public IList<ReportsViewModel> getReportsFrmDB(int daId)
        {
            try
            {
                ReportManager reportManager = new ReportManager();

                var reportData = reportManager.GetReportFrmDB(daId);

                IList<ReportsViewModel> reportViewModelList = new List<ReportsViewModel>();

                foreach (var item in reportData)
                {
                    ReportsViewModel reportViewModelItem = new ReportsViewModel();
                    reportViewModelItem.reportID = Convert.ToInt32(item.ReportID);
                    reportViewModelItem.reqReference = (item.ReqReference == null ? "" : item.ReqReference);
                    reportViewModelItem.reportName = item.ReportName;
                    reportViewModelItem.reportDesc = item.ReportDesc;
                    reportViewModelItem.sourceId = item.SourceID;
                    reportViewModelItem.transactionSeq = Convert.ToInt32(item.TransactionSeq);
                    reportViewModelItem.highLevelTxn = item.HIGHLEVELTXN;
                    reportViewModelItem.periodId = item.PeriodID;
                    reportViewModelItem.isLinked = Convert.ToBoolean(Convert.ToInt32(item.IsLinked));

                    reportViewModelList.Add(reportViewModelItem);
                }
                lstReports = reportViewModelList;
                return reportViewModelList;
            }
            catch (Exception)
            {

                throw;
            }

        }

        #region GetSourceList
        public IList<SourceViewModel> GetSourcesList(int daId)
        {
            try
            {
                SourceViewModel sourceViewModel = new SourceViewModel();
                return sourceViewModel.GetSourceDetails(daId);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region GetTransactionList
        public TransactionsViewModel GetTransactionsList(int? daId)
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
        #endregion

        #region GetPeriodTypeList
        public IList<PeriodTypeViewModel> GetPeriodTypeList(int? daId)
        {
            try
            {
                PeriodTypeViewModel periodTypeViewModel = new PeriodTypeViewModel();
                return periodTypeViewModel.GetPeriodTypeDetails(daId);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        public int SaveReports(IList<ReportsViewModel> reportsViewModel, int daId)
        {
            try
            {
                int result = 0;
                ReportManager reportsManager = new ReportManager();
                var lstReportsfrmDB = reportsManager.GetReportFrmDB(daId);
                List<tbl_Reports> lstReports = new List<tbl_Reports>();

                foreach (var item in reportsViewModel)
                {
                    tbl_Reports tblReports = new tbl_Reports();

                    #region Modify Reports

                    //var existingReport = lstReportsfrmDB.Where(e => e.ReportName.Equals(item.reportName) && e.TransactionSeq.Equals(item.transactionSeq));
                    var existingReport = lstReportsfrmDB.Where(e => e.ReportID.Equals(item.reportID) && e.TransactionSeq.Equals(item.transactionSeq));
                    if (existingReport.Count() != 0)
                    {
                        foreach (var rpts in existingReport)
                        {
                            if (rpts.ReportID > 0)
                            {
                                tblReports.ReportID = Convert.ToInt32(rpts.ReportID);
                                tblReports.ReqReference = (item.reqReference == null ? "" : item.reqReference);
                                tblReports.ReportDesc = item.reportDesc;
                                tblReports.ReportName = item.reportName;
                                tblReports.SourceID = Convert.ToInt32(item.sourceId);
                                tblReports.PeriodID = Convert.ToInt32(item.periodId);
                                tblReports.TransactionSeq = Convert.ToInt32(item.transactionSeq);
                                tblReports.daId = daId;

                                ////To keep record as it is
                                //if (Convert.ToBoolean(Convert.ToInt32(rpts.IsLinked)) == item.isLinked)
                                //    tblReports.EntityState = DA.DomainModel.EntityState.Unchanged;
                                //to remove unchecked transaction
                                //else
                                tblReports.EntityState = DA.DomainModel.EntityState.Modified;

                                tblReports.IsLinked = (item.isLinked == true ? "1" : "0");//item.isLinked;

                            }
                            lstReports.Add(tblReports);
                            #endregion
                        }
                    }
                    else
                    #region Add new Report
                    {
                        if (item.isLinked == true)
                        {
                            tblReports.ReqReference = (item.reqReference == null ? "" : item.reqReference);
                            tblReports.ReportDesc = item.reportDesc;
                            tblReports.ReportName = item.reportName;
                            tblReports.SourceID = Convert.ToInt32(item.sourceId);
                            tblReports.PeriodID = Convert.ToInt32(item.periodId);
                            tblReports.daId = daId;
                            tblReports.TransactionSeq = item.transactionSeq;
                            tblReports.IsLinked = (item.isLinked == true ? "1" : "0");
                            tblReports.EntityState = DA.DomainModel.EntityState.Added;

                            lstReports.Add(tblReports);
                            #endregion
                        }
                    }
                }
                result = reportsManager.SaveReports(lstReports);
                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}