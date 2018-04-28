using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.DomainModel;
using DA.DataAccessLayer;

namespace DA.BusinessLayer
{
    public class LifeCycleManager
    {
        public IList<tbl_LifeCycle> GetLifeCycles(int? daId)
        {
            try
            {
                IGenericDataRepository<tbl_LifeCycle> repository = new GenericDataRepository<tbl_LifeCycle>();
                IList<tbl_LifeCycle> lstLifeCycles = repository.GetList(e => e.daId.Equals(daId));

                return lstLifeCycles;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void AddLifeCycle(tbl_LifeCycle tblLifeCycles)
        {
            try
            {
                IGenericDataRepository<tbl_LifeCycle> repository = new GenericDataRepository<tbl_LifeCycle>();
                repository.Add(tblLifeCycles);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public tbl_LifeCycle FindLifeCycles(int? LifeCycleID)
        {
            try
            {
                IGenericDataRepository<tbl_LifeCycle> repository = new GenericDataRepository<tbl_LifeCycle>();
                tbl_LifeCycle tblLifeCycles = repository.GetSingle(b => b.LifeCycleID == LifeCycleID);
                return tblLifeCycles;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void DeleteLifeCycle(tbl_LifeCycle tblLifeCycles)
        {
            try
            {
                IGenericDataRepository<tbl_LifeCycle> repository = new GenericDataRepository<tbl_LifeCycle>();
                repository.Remove(tblLifeCycles);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void UpdateLifeCycle(tbl_LifeCycle tblLifeCycles)
        {
            try
            {
                IGenericDataRepository<tbl_LifeCycle> repository = new GenericDataRepository<tbl_LifeCycle>();
                repository.Update(tblLifeCycles);
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
