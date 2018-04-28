using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using DA.DomainModel;
using DA.DataAccessLayer;

namespace DA.BusinessLayer
{
    class BusinessRuleMappingManager
    {
        public IList<tbl_Attribute> GetAllAttributes(int? daId)
        {
            try
            {
                IGenericDataRepository<tbl_Attribute> repository = new GenericDataRepository<tbl_Attribute>();
                IList<tbl_Attribute> lstAttributes = repository.GetList(e => e.daId.Equals(daId));

                return lstAttributes;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
