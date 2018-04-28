using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.DataAccessLayer;
using DA.DomainModel;
using System.Data.SqlClient;
using System.Data;


namespace DA.BusinessLayer
{
    public class ReportManager
    {
        public IList<sp_GetReports_Result> GetReportFrmDB(int daId)
        {
            try
            {
                IGenericDataRepository<sp_GetReports_Result> repository = new GenericDataRepository<sp_GetReports_Result>();
                return repository.ExecuteStoredProcedure("EXEC sp_GetReports @daId", new SqlParameter("daId", SqlDbType.Int) { Value = daId }).ToList();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public IList<tbl_PeriodType> GetAllPeriodTypes()
        {
            try
            {
                IGenericDataRepository<tbl_PeriodType> repository = new GenericDataRepository<tbl_PeriodType>();
                IList<tbl_PeriodType> lstPeriodTypes = repository.GetAll();

                return lstPeriodTypes;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public int SaveReports(IList<tbl_Reports> tblReports)
        {
            try
            {
                IGenericDataRepository<tbl_Reports> repository = new GenericDataRepository<tbl_Reports>();
                foreach (var item in tblReports)
                    repository.Add(item);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }

        }

        public void DeleteReport(string postData, int id)
        {
            try
            {


                tbl_Reports tblReport = new tbl_Reports();

                IList<tbl_Reports> lstReports = GetReports(id);
                var isExisting = lstReports.Where(q => q.ReportName == postData && q.daId == id);

                tblReport.EntityState = EntityState.Deleted;

                if (isExisting != null)
                {
                    foreach (var item in isExisting)
                    {
                        IGenericDataRepository<tbl_Reports> repository = new GenericDataRepository<tbl_Reports>();
                        tblReport.daId = item.daId;
                        tblReport.ReportDesc = item.ReportDesc;
                        tblReport.ReportID = item.ReportID;
                        tblReport.ReportName = item.ReportName;
                        tblReport.ReqReference = item.ReqReference;
                        tblReport.SourceID = item.SourceID;
                        tblReport.IsLinked = item.IsLinked;
                        tblReport.PeriodID = item.PeriodID;
                        tblReport.TransactionSeq = item.TransactionSeq;
                        tblReport.EntityState = EntityState.Deleted;
                        repository.Remove(tblReport);
                    }


                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public IList<tbl_Reports> GetReports(int id)
        {
            try
            {


                IGenericDataRepository<tbl_Reports> repository = new GenericDataRepository<tbl_Reports>();
                IList<tbl_Reports> lstscreenRoles = repository.GetAll();

                return lstscreenRoles;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
