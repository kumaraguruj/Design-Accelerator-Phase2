using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.DomainModel;
using DA.DataAccessLayer;
using System.Data.SqlClient;
using System.Data;

namespace DA.BusinessLayer
{
    public class MappingViewModelManager
    {
        public List<sp_GetMappingViewModelData_Result> GetMappingViewModelData(int daId)
        {
            try
            {
                IGenericDataRepository<sp_GetMappingViewModelData_Result> repository = new GenericDataRepository<sp_GetMappingViewModelData_Result>();
                return repository.ExecuteStoredProcedure("EXEC sp_GetMappingViewModelData @DAID", new SqlParameter("DAID", SqlDbType.Int) { Value = daId }).ToList();
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
