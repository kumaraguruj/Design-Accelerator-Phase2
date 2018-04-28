using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.DomainModel;
using DA.DataAccessLayer;

namespace DA.BusinessLayer
{
    public class DistributionTypeManager
    {
        public void AddDistributionType(tbl_DistributionType tblDistributionType)
        {
            try
            {
                IGenericDataRepository<tbl_DistributionType> repository = new GenericDataRepository<tbl_DistributionType>();
                repository.Add(tblDistributionType);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void DeleteDistributionType(tbl_DistributionType tblDistributionType)
        {
            try
            {
                IGenericDataRepository<tbl_DistributionType> repository = new GenericDataRepository<tbl_DistributionType>();
                repository.Remove(tblDistributionType);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void UpdateDistributionType(tbl_DistributionType tblDistributionType)
        {
            try
            {
                IGenericDataRepository<tbl_DistributionType> repository = new GenericDataRepository<tbl_DistributionType>();
                repository.Update(tblDistributionType);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public IList<tbl_DistributionType> GetDistributionTypeDetails(int daID)
        {
            try
            {
                IGenericDataRepository<tbl_DistributionType> repository = new GenericDataRepository<tbl_DistributionType>();
                IList<tbl_DistributionType> lstDistributionType = repository.GetList(e => e.daId.Equals(daID));

                return lstDistributionType;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public tbl_DistributionType FindDistributionType(int? distributionTypeID)
        {
            try
            {
                IGenericDataRepository<tbl_DistributionType> repository = new GenericDataRepository<tbl_DistributionType>();
                tbl_DistributionType tblDistributionType = repository.GetSingle(d => d.DistributionTypeID == distributionTypeID);

                return tblDistributionType;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public tbl_DistributionType FindDistributionTypeDesc(string distributionTypeDesc, int daid)
        {
            try
            {
                IGenericDataRepository<tbl_DistributionType> repository = new GenericDataRepository<tbl_DistributionType>();
                tbl_DistributionType tblDistributionType = repository.GetSingle(d => d.DistributionDesc.ToUpper() == distributionTypeDesc.ToUpper() && d.daId == daid);

                return tblDistributionType;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
