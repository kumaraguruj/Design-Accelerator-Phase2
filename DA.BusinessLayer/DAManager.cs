using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.DataAccessLayer;
using DA.DomainModel;

namespace DA.BusinessLayer
{
    public class DAManager
    {
        public void AddDA(tbl_DesignAccelerator tblDesignAccelerator)
        {
            try
            {
                IGenericDataRepository<tbl_DesignAccelerator> repository = new GenericDataRepository<tbl_DesignAccelerator>();
                repository.Add(tblDesignAccelerator);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteDA(tbl_DesignAccelerator tblDesignAccelerator)
        {
            try
            {
                IGenericDataRepository<tbl_DesignAccelerator> repository = new GenericDataRepository<tbl_DesignAccelerator>();
                repository.Remove(tblDesignAccelerator);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public void UpdateDA(tbl_DesignAccelerator tblDesignAccelerator)
        {
            try
            {
                IGenericDataRepository<tbl_DesignAccelerator> repository = new GenericDataRepository<tbl_DesignAccelerator>();
                repository.Update(tblDesignAccelerator);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public IList<tbl_DesignAccelerator> GetDADetails(int appId)
        {
            try
            {
                IGenericDataRepository<tbl_DesignAccelerator> repository = new GenericDataRepository<tbl_DesignAccelerator>();
                IList<tbl_DesignAccelerator> lstDAs = repository.GetList(e => e.ModuleId.Equals(appId));

                return lstDAs;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public tbl_DesignAccelerator FindDA(int? DAID)
        {
            try
            {
                IGenericDataRepository<tbl_DesignAccelerator> repository = new GenericDataRepository<tbl_DesignAccelerator>();
                tbl_DesignAccelerator DA = repository.GetSingle(c => c.daid == DAID);
                return DA;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public tbl_DesignAccelerator FindDAName(string DAName, int moduleId)
        {
            try
            {
                IGenericDataRepository<tbl_DesignAccelerator> repository = new GenericDataRepository<tbl_DesignAccelerator>();
                tbl_DesignAccelerator DA = repository.GetSingle(c => c.daName.ToUpper() == DAName.ToUpper() && c.ModuleId == moduleId);
                return DA;
                //ClientID
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
