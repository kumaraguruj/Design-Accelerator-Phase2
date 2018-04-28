using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.DomainModel;
using DA.DataAccessLayer;

namespace DA.BusinessLayer
{
    public class AttributeValueManager
    {
        public IList<tbl_AttributeValues> GetAttributeValList(int? attrID)
        {
            try
            {
                IGenericDataRepository<tbl_AttributeValues> repository = new GenericDataRepository<tbl_AttributeValues>();
                IList<tbl_AttributeValues> lstAttribVals = repository.GetList(e => e.AttributeID.Equals(attrID));
                return lstAttribVals;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int AddAttributeVal(IList<tbl_AttributeValues> tblAttributeValues)
        {
            try
            {
                IGenericDataRepository<tbl_AttributeValues> repository = new GenericDataRepository<tbl_AttributeValues>();

                //tbl_AttributeValues DuplicateCheck = FindAttributeValue(tblAttributeValues.FirstOrDefault().AttributeValue);

                //if (DuplicateCheck == null)
                //{ 
                foreach (var item in tblAttributeValues)
                {
                    item.AttributeValue = item.AttributeValue.Trim();
                    repository.Add(item);
                }
                return 1;
            }
            catch (Exception)
            {

                throw;
            }

            //}
            //else
            //{
            //    return 0;

            //}

        }

        //public tbl_AttributeValues FindAttributeValue(string attributeValue)
        //{
        //    IGenericDataRepository<tbl_AttributeValues> repository = new GenericDataRepository<tbl_AttributeValues>();
        //    tbl_AttributeValues attribVals = repository.GetSingle(c => c.AttributeValue == attributeValue);
        //    return attribVals;
        //}

        public IList<tbl_AttributeValues> GetAttributeValListbyDaID(int? daId)
        {
            try
            {
                IGenericDataRepository<tbl_AttributeValues> repository = new GenericDataRepository<tbl_AttributeValues>();
                IList<tbl_AttributeValues> lstAttribVals = repository.GetList(e => e.daId.Equals(daId), e => e.isNegative == "1");
                return lstAttribVals;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
