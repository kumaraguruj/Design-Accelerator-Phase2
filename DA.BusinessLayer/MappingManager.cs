using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.DataAccessLayer;
using DA.DomainModel;

namespace DA.BusinessLayer
{
    public class MappingManager
    {
        public IList<tbl_TxnAttributeMapping> GetMappingDetails(int? daId)
        {
            try
            {
                IGenericDataRepository<tbl_TxnAttributeMapping> repository = new GenericDataRepository<tbl_TxnAttributeMapping>();
                IList<tbl_TxnAttributeMapping> lstMappingDetails = repository.GetList(e => e.daId == daId && e.isLinked == "1", e => e.tbl_Attribute, e => e.tbl_DesignAccelerator, e => e.tbl_Transactions, e => e.tbl_Attribute.tbl_AttributeType);

                return lstMappingDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<tbl_AttributeValues> GetNegativeAttributeValue(int? daId)
        {
            try
            {
                IGenericDataRepository<tbl_AttributeValues> repository = new GenericDataRepository<tbl_AttributeValues>();
                IList<tbl_AttributeValues> lstNegAttrValues = repository.GetList(e => e.daId == daId && e.isNegative == "1", e => e.tbl_Attribute, e => e.tbl_DesignAccelerator, e => e.tbl_DesignAccelerator.tbl_Transactions);
                return lstNegAttrValues;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
