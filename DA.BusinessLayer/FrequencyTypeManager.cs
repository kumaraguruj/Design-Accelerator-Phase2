using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.DomainModel;
using DA.DataAccessLayer;

namespace DA.BusinessLayer
{
    public class FrequencyTypeManager
    {
        public IList<tbl_FrequencyType> GetFrequencyTypes(int? daId)
        {
            try
            {
                IGenericDataRepository<tbl_FrequencyType> repository = new GenericDataRepository<tbl_FrequencyType>();
                IList<tbl_FrequencyType> lstFrequencyType = repository.GetList(e => e.daId.Equals(daId));

                return lstFrequencyType;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void AddFrequencyType(tbl_FrequencyType tblFrequencyType)
        {
            try
            {
                IGenericDataRepository<tbl_FrequencyType> repository = new GenericDataRepository<tbl_FrequencyType>();
                repository.Add(tblFrequencyType);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void DeleteFrequencyType(tbl_FrequencyType tblFrequencyTypes)
        {
            try
            {
                IGenericDataRepository<tbl_FrequencyType> repository = new GenericDataRepository<tbl_FrequencyType>();
                repository.Remove(tblFrequencyTypes);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void UpdateFrequencyType(tbl_FrequencyType tblFrequencyTypes)
        {
            try
            {
                IGenericDataRepository<tbl_FrequencyType> repository = new GenericDataRepository<tbl_FrequencyType>();
                repository.Update(tblFrequencyTypes);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public tbl_FrequencyType FindFrequencyTypes(int? FreqTypeID)
        {
            try
            {
                IGenericDataRepository<tbl_FrequencyType> repository = new GenericDataRepository<tbl_FrequencyType>();
                tbl_FrequencyType tblFrequencyTypes = repository.GetSingle(b => b.FreqTypeID == FreqTypeID);
                return tblFrequencyTypes;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public tbl_FrequencyType FindFrequencyTypeDesc(string freqTypeDesc, int daid)
        {
            try
            {
                IGenericDataRepository<tbl_FrequencyType> repository = new GenericDataRepository<tbl_FrequencyType>();
                tbl_FrequencyType tblFrequencyTypes = repository.GetSingle(b => b.FreqTypeDesc.ToUpper() == freqTypeDesc.ToUpper() && b.daId == daid);
                return tblFrequencyTypes;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
