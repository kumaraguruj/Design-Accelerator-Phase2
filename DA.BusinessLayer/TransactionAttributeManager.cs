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
    public class TransactionAttributeManager
    {
        public List<sp_GetTransactionAttributesMapping_Result> GetTransactionAttributes(int daId)
        {
            try
            {
                IGenericDataRepository<sp_GetTransactionAttributesMapping_Result> repository = new GenericDataRepository<sp_GetTransactionAttributesMapping_Result>();
                return repository.ExecuteStoredProcedure("EXEC sp_GetTransactionAttributesMapping @daId", new SqlParameter("daId", SqlDbType.Int) { Value = daId }).ToList();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public int SaveData(IList<tbl_TxnAttributeMapping> tblTxnAttributeMapping)
        {
            try
            {
                IGenericDataRepository<tbl_TxnAttributeMapping> repository = new GenericDataRepository<tbl_TxnAttributeMapping>();
                foreach (var item in tblTxnAttributeMapping)
                    repository.Add(item);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }

        }
    }

}