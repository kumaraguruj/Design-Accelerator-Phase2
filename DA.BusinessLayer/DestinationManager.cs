using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.DataAccessLayer;
using DA.DomainModel;

namespace DA.BusinessLayer
{
    public class DestinationManager
    {
        public void AddDestination(tbl_Destination tblDestination)
        {
            try
            {
                IGenericDataRepository<tbl_Destination> repository = new GenericDataRepository<tbl_Destination>();
                repository.Add(tblDestination);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void DeleteDestination(tbl_Destination tblDestination)
        {
            try
            {
                IGenericDataRepository<tbl_Destination> repository = new GenericDataRepository<tbl_Destination>();
                repository.Remove(tblDestination);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void UpdateDestination(tbl_Destination tblDestination)
        {
            try
            {
                IGenericDataRepository<tbl_Destination> repository = new GenericDataRepository<tbl_Destination>();
                repository.Update(tblDestination);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public IList<tbl_Destination> GetDestinationDetails(int daID)
        {
            try
            {
                IGenericDataRepository<tbl_Destination> repository = new GenericDataRepository<tbl_Destination>();
                IList<tbl_Destination> lstDest = repository.GetList(e => e.daId.Equals(daID));

                return lstDest;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public tbl_Destination FindDestination(int? destID)
        {
            try
            {
                IGenericDataRepository<tbl_Destination> repository = new GenericDataRepository<tbl_Destination>();
                tbl_Destination tblDestination = repository.GetSingle(d => d.DestID == destID);

                return tblDestination;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public tbl_Destination FindDestinationDesc(string destDesc, int daid)
        {
            try
            {
                IGenericDataRepository<tbl_Destination> repository = new GenericDataRepository<tbl_Destination>();
                tbl_Destination tblDestination = repository.GetSingle(d => d.DestDesc.ToUpper() == destDesc.ToUpper() && d.daId == daid);

                return tblDestination;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
