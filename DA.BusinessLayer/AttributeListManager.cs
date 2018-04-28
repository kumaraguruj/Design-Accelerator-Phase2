using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.DomainModel;
using DA.DataAccessLayer;

namespace DA.BusinessLayer
{
    public class AttributeListManager
    {
        public IList<tbl_Attribute> GetAttributeList(int? daid)
        {
            try
            {
                IGenericDataRepository<tbl_Attribute> repository = new GenericDataRepository<tbl_Attribute>();
                IList<tbl_Attribute> lstAttrib = repository.GetList(e => e.daId.Equals(daid), e => e.tbl_AttributeType, e => e.tbl_AttributeValues, e => e.tbl_TxnAttributeMapping, e => e.tbl_DesignAccelerator);

                return lstAttrib;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddAttribute(tbl_Attribute tblAttribute)
        {
            try
            {
                IGenericDataRepository<tbl_Attribute> repository = new GenericDataRepository<tbl_Attribute>();
                repository.Add(tblAttribute);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateAttribute(tbl_Attribute tblAttribute)
        {
            try
            {
                IGenericDataRepository<tbl_Attribute> repository = new GenericDataRepository<tbl_Attribute>();
                repository.Update(tblAttribute);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteAttribute(tbl_Attribute tblAttribute)
        {
            try
            {
                IGenericDataRepository<tbl_Attribute> repository = new GenericDataRepository<tbl_Attribute>();
                repository.Remove(tblAttribute);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_Attribute FindAttribs(int? attributeID)
        {
            try
            {
                IGenericDataRepository<tbl_Attribute> repository = new GenericDataRepository<tbl_Attribute>();
                tbl_Attribute tblattribID = repository.GetSingle(b => b.AttributeID == attributeID);
                return tblattribID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_Attribute FindAttribDesc(string attributeDesc, int daid)
        {
            try
            {
                IGenericDataRepository<tbl_Attribute> repository = new GenericDataRepository<tbl_Attribute>();
                tbl_Attribute tblattribDesc = repository.GetSingle(b => b.AttributeDesc.ToUpper() == attributeDesc.ToUpper() && b.daId == daid);
                return tblattribDesc;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_AttributeType FindAttribType(int? attributeTypeID)
        {
            try
            {
                IGenericDataRepository<tbl_AttributeType> repository = new GenericDataRepository<tbl_AttributeType>();
                tbl_AttributeType tblattribTypeID = repository.GetSingle(b => b.AttributeTypeID == attributeTypeID);
                return tblattribTypeID;
            }
            catch (Exception)
            {

                throw;
            }

        }

        //To get the common&critical values
        public IList<tbl_Attribute> GetCommonriticalAttributes(int? daId)
        {
            try
            {
                IGenericDataRepository<tbl_Attribute> repository = new GenericDataRepository<tbl_Attribute>();

                IList<tbl_Attribute> lstAttributes = repository.GetList(a => a.daId == daId && (a.AttributeTypeID == 2 || a.AttributeTypeID == 4));

                return lstAttributes;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
