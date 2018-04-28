using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.DataAccessLayer;
using DA.DomainModel;

namespace DA.BusinessLayer
{
    public class AppVersionManager
    {
        public void AddAppVersion(tbl_AppVersion tblAppVersion)
        {
            try
            {
                IGenericDataRepository<tbl_AppVersion> repository = new GenericDataRepository<tbl_AppVersion>();
                repository.Add(tblAppVersion);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteAppVersion(tbl_AppVersion tblAppVersion)
        {
            try
            {
                IGenericDataRepository<tbl_AppVersion> repository = new GenericDataRepository<tbl_AppVersion>();
                repository.Remove(tblAppVersion);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateAppVersion(tbl_AppVersion tblAppVersion)
        {
            try
            {
                IGenericDataRepository<tbl_AppVersion> repository = new GenericDataRepository<tbl_AppVersion>();
                repository.Update(tblAppVersion);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public IList<tbl_AppVersion> GetApplVersionDetails()
        {
            try
            {
                IGenericDataRepository<tbl_AppVersion> repository = new GenericDataRepository<tbl_AppVersion>();
                IList<tbl_AppVersion> lstAppVersion = repository.GetAll();

                return lstAppVersion;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_AppVersion FindAppVersion(int? Id)
        {
            try
            {
                IGenericDataRepository<tbl_AppVersion> repository = new GenericDataRepository<tbl_AppVersion>();
                tbl_AppVersion appVersion = repository.GetSingle(c => c.Id == Id);
                return appVersion;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public tbl_AppVersion FindVersionName(string versionName)
        //{
        //    IGenericDataRepository<tbl_AppVersion> repository = new GenericDataRepository<tbl_AppVersion>();

        //    tbl_AppVersion version = repository.GetSingle(c => c.AppVersion.ToUpper() == versionName.ToUpper());
        //    return version;
        //}
    }
}
