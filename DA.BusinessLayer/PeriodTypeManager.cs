using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.DomainModel;
using DA.DataAccessLayer;

namespace DA.BusinessLayer
{
    public class PeriodTypeManager
    {
        public void AddPeriodType(tbl_PeriodType tblPeriodType)
        {
            try
            {
                IGenericDataRepository<tbl_PeriodType> repository = new GenericDataRepository<tbl_PeriodType>();
                repository.Add(tblPeriodType);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void DeletePeriodType(tbl_PeriodType tblPeriodType)
        {
            try
            {
                IGenericDataRepository<tbl_PeriodType> repository = new GenericDataRepository<tbl_PeriodType>();
                repository.Remove(tblPeriodType);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void UpdatePeriodType(tbl_PeriodType tblPeriodType)
        {
            try
            {
                IGenericDataRepository<tbl_PeriodType> repository = new GenericDataRepository<tbl_PeriodType>();
                repository.Update(tblPeriodType);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public IList<tbl_PeriodType> GetSPeriodTypeDetails()
        {
            try
            {
                IGenericDataRepository<tbl_PeriodType> repository = new GenericDataRepository<tbl_PeriodType>();
                IList<tbl_PeriodType> lstPeriodType = repository.GetAll();

                return lstPeriodType;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public tbl_PeriodType FindPeriodType(int? periodTypeID)
        {
            try
            {
                IGenericDataRepository<tbl_PeriodType> repository = new GenericDataRepository<tbl_PeriodType>();
                tbl_PeriodType tblPeriodType = repository.GetSingle(s => s.PeriodTypeID == periodTypeID);

                return tblPeriodType;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
