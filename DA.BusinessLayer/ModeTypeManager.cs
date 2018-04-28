using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.DomainModel;
using DA.DataAccessLayer;

namespace DA.BusinessLayer
{
    public class ModeTypeManager
    {
        public IList<tbl_ModeType> GetModeTypes(int? daId)
        {
            try
            {
                IGenericDataRepository<tbl_ModeType> repository = new GenericDataRepository<tbl_ModeType>();
                IList<tbl_ModeType> lstModeType = repository.GetList(e => e.daId.Equals(daId));

                return lstModeType;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void AddModeType(tbl_ModeType tblModeType)
        {
            try
            {
                IGenericDataRepository<tbl_ModeType> repository = new GenericDataRepository<tbl_ModeType>();
                repository.Add(tblModeType);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public tbl_ModeType FindModeTypes(int? ModeTypeID)
        {
            try
            {
                IGenericDataRepository<tbl_ModeType> repository = new GenericDataRepository<tbl_ModeType>();
                tbl_ModeType tblModeTypes = repository.GetSingle(b => b.ModeTypeID == ModeTypeID);
                return tblModeTypes;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public tbl_ModeType FindModeDesc(string modeTypedesc, int daid)
        {
            try
            {
                IGenericDataRepository<tbl_ModeType> repository = new GenericDataRepository<tbl_ModeType>();
                tbl_ModeType tblModeTypes = repository.GetSingle(b => b.ModeTypeDesc.ToUpper() == modeTypedesc.ToUpper() && b.daId == daid);
                return tblModeTypes;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void DeleteModeType(tbl_ModeType tblModeTypes)
        {
            try
            {
                IGenericDataRepository<tbl_ModeType> repository = new GenericDataRepository<tbl_ModeType>();
                repository.Remove(tblModeTypes);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void UpdateModeType(tbl_ModeType tblModeTypes)
        {
            try
            {
                IGenericDataRepository<tbl_ModeType> repository = new GenericDataRepository<tbl_ModeType>();
                repository.Update(tblModeTypes);
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
