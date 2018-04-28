using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.DataAccessLayer;
using DA.DomainModel;

namespace DA.BusinessLayer
{
    public class LOBManager
    {
        public void AddLOB(tbl_LOB tblLOB)
        {
            try
            {
                IGenericDataRepository<tbl_LOB> repository = new GenericDataRepository<tbl_LOB>();
                repository.Add(tblLOB);
            }
            catch(Exception)
            {
                throw;
            }
        }
        public void DeleteLOB(tbl_LOB tblLOB)
        {
            try
            {
                IGenericDataRepository<tbl_LOB> repository = new GenericDataRepository<tbl_LOB>();
                repository.Remove(tblLOB);
            }
            catch(Exception)
            {
                throw;
            }
        }
        public void UpdateLOB(tbl_LOB tblLOB)
        {
            try
            {
                IGenericDataRepository<tbl_LOB> repository = new GenericDataRepository<tbl_LOB>();
                repository.Update(tblLOB);
            }
            catch(Exception)
            {
                throw;
            }
        }
        public IList<tbl_LOB> GetLOBDetails(int daID)
        {
            try
            {
                IGenericDataRepository<tbl_LOB> repository = new GenericDataRepository<tbl_LOB>();
                IList<tbl_LOB> lstDest = repository.GetList(e => e.daId.Equals(daID));

                return lstDest;
            }
            catch(Exception)
            {
                throw;
            }
        }
        public tbl_LOB FindLOB(int? lobID)
        {
            try
            {
                IGenericDataRepository<tbl_LOB> repository = new GenericDataRepository<tbl_LOB>();
                tbl_LOB tblLOB = repository.GetSingle(d => d.LobID == lobID);

                return tblLOB;
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
