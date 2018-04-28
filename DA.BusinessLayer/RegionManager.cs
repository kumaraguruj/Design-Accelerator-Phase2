using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.DataAccessLayer;
using DA.DomainModel;

namespace DA.BusinessLayer
{
   public class RegionManager
    {

        public void AddRegion(tbl_Region tblRegion)
        {
            try
            {
                IGenericDataRepository<tbl_Region> repository = new GenericDataRepository<tbl_Region>();
                repository.Add(tblRegion);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteRegion(tbl_Region tblRegion)
        {
            try
            {
                IGenericDataRepository<tbl_Region> repository = new GenericDataRepository<tbl_Region>();
                repository.Remove(tblRegion);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateRegion(tbl_Region tblRegion)
        {
            try
            {
                IGenericDataRepository<tbl_Region> repository = new GenericDataRepository<tbl_Region>();
                repository.Update(tblRegion);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public IList<tbl_Region> GetRegionDetails()
        {
            try
            {
                IGenericDataRepository<tbl_Region> repository = new GenericDataRepository<tbl_Region>();
                IList<tbl_Region> lstRegion = repository.GetAll();

                return lstRegion;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_Region FindRegion(int? Id)
        {
            try
            {
                IGenericDataRepository<tbl_Region> repository = new GenericDataRepository<tbl_Region>();
                tbl_Region tblRegion = repository.GetSingle(c => c.Id == Id);
                return tblRegion;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public tbl_Region FindRegionName(string regionName)
        {
            try
            {
                IGenericDataRepository<tbl_Region> repository = new GenericDataRepository<tbl_Region>();
                tbl_Region tblRegion = repository.GetSingle(c => c.Region.ToUpper() == regionName.ToUpper());
                return tblRegion;
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
