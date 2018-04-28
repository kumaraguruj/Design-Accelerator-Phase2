using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.DomainModel;
using DA.DataAccessLayer;

namespace DA.BusinessLayer
{
    public class SourceManager
    {
        public void AddSource(tbl_Source tblSource)
        {
            try
            {
                IGenericDataRepository<tbl_Source> repository = new GenericDataRepository<tbl_Source>();
                repository.Add(tblSource);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void DeleteSource(tbl_Source tblSource)
        {
            try
            {
                IGenericDataRepository<tbl_Source> repository = new GenericDataRepository<tbl_Source>();
                repository.Remove(tblSource);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UpdateSource(tbl_Source tblSource)
        {
            try
            {
                IGenericDataRepository<tbl_Source> repository = new GenericDataRepository<tbl_Source>();
                repository.Update(tblSource);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IList<tbl_Source> GetSourceDetails(int daID)
        {
            try
            {
                IGenericDataRepository<tbl_Source> repository = new GenericDataRepository<tbl_Source>();
                IList<tbl_Source> lstSource = repository.GetList(e => e.daId.Equals(daID));

                return lstSource;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public tbl_Source FindSource(int? sourceID)
        {
            try
            {
                IGenericDataRepository<tbl_Source> repository = new GenericDataRepository<tbl_Source>();
                tbl_Source tblSource = repository.GetSingle(s => s.SourceID == sourceID);

                return tblSource;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public tbl_Source FindSourceName(String sourceName, int daid)
        {
            try
            {
                IGenericDataRepository<tbl_Source> repository = new GenericDataRepository<tbl_Source>();
                tbl_Source tblSource = repository.GetSingle(s => s.SourceDesc.ToUpper() == sourceName.ToUpper() && s.daId == daid);

                return tblSource;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
