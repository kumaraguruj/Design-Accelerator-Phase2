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
    public class Mapping_ForAllManager
    {
        public IList<sp_BusinessRuleMappingData_Result> GetBuzRulesMappingData(int daId)
        {
            try
            {
                IGenericDataRepository<sp_BusinessRuleMappingData_Result> repository = new GenericDataRepository<sp_BusinessRuleMappingData_Result>();
                return repository.ExecuteStoredProcedure(" EXEC sp_BusinessRuleMappingData @DAid", new SqlParameter("daId", SqlDbType.Int) { Value = daId }).ToList();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public IList<sp_InterfaceMappingData_Result> GetInterfaceMappingData(int daId)
        {
            try
            {
                IGenericDataRepository<sp_InterfaceMappingData_Result> repository = new GenericDataRepository<sp_InterfaceMappingData_Result>();
                return repository.ExecuteStoredProcedure(" EXEC sp_InterfaceMappingData @DAid", new SqlParameter("daId", SqlDbType.Int) { Value = daId }).ToList();
            }
            catch (Exception)
            {

                throw;
            }

        }

        //public IList<tbl_Interface> GetInterfaceMappingData(int daId)
        //{
        //    IGenericDataRepository<tbl_Interface> repository = new GenericDataRepository<tbl_Interface>();
        //    IList<tbl_Interface> lstInterfaceDetails = repository.GetList(e => e.daId == daId, e => e.tbl_InterfaceAttrMapping, e => e.tbl_DesignAccelerator, e => e.tbl_Transactions);

        //    return lstInterfaceDetails;
        //}

        public IList<sp_GetReportsMappingData_Result> GetReportsData(int daId)
        {
            try
            {
                IGenericDataRepository<sp_GetReportsMappingData_Result> repository = new GenericDataRepository<sp_GetReportsMappingData_Result>();
                return repository.ExecuteStoredProcedure("EXEC  sp_GetReportsMappingData @daId", new SqlParameter("daId", SqlDbType.Int) { Value = daId }).ToList();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public IList<sp_GetChannelsAndAlertsMappingData_Result> GetChannelAlertData(int daId)
        {
            try
            {
                IGenericDataRepository<sp_GetChannelsAndAlertsMappingData_Result> repository = new GenericDataRepository<sp_GetChannelsAndAlertsMappingData_Result>();
                return repository.ExecuteStoredProcedure("EXEC  sp_GetChannelsAndAlertsMappingData @daId", new SqlParameter("daId", SqlDbType.Int) { Value = daId }).ToList();
            }
            catch (Exception)
            {

                throw;
            }

        }


    }
}
