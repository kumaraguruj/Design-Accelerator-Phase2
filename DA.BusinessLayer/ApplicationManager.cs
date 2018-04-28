using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.DataAccessLayer;
using DA.DomainModel;

namespace DA.BusinessLayer
{
    public class ApplicationManager
    {
        public void AddApplication(tbl_Applications tblApplications)
        {
            try
            {
                IGenericDataRepository<tbl_Applications> repository = new GenericDataRepository<tbl_Applications>();
                repository.Add(tblApplications);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public void DeleteApplication(tbl_Applications tblApplications)
        {
            try
            {
                IGenericDataRepository<tbl_Applications> repository = new GenericDataRepository<tbl_Applications>();
                repository.Remove(tblApplications);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateApplication(tbl_Applications tblApplications)
        {
            try
            {
                IGenericDataRepository<tbl_Applications> repository = new GenericDataRepository<tbl_Applications>();
                repository.Update(tblApplications);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<tbl_AppVersion> GetAppVersion(int? appId)
        {
            try
            {
                IGenericDataRepository<tbl_AppVersion> repository = new GenericDataRepository<tbl_AppVersion>();
                IList<tbl_AppVersion> lstAppVersion = repository.GetAll();

                return lstAppVersion;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public IList<tbl_Applications> GetApplicationDetails(int? projectId)
        {
            try
            {
                IGenericDataRepository<tbl_Applications> repository = new GenericDataRepository<tbl_Applications>();
                IList<tbl_Applications> lstApplications = repository.GetList(e => e.ProjectId == projectId);

                return lstApplications;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_Applications FindApplication(int? applicationID)
        {
            try
            {
                IGenericDataRepository<tbl_Applications> repository = new GenericDataRepository<tbl_Applications>();
                tbl_Applications application = repository.GetSingle(c => c.ApplicationID == applicationID);
                return application;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_Applications FindApplicationName(string applicationName, int appVersion, int banktype, int projectId)
        {
            try
            {
                IGenericDataRepository<tbl_Applications> repository = new GenericDataRepository<tbl_Applications>();

                tbl_Applications application = repository.GetSingle(c => c.ApplicationName.ToUpper() == applicationName.ToUpper() && c.AppVersion == appVersion && c.BankType == banktype
                                                                    && c.ProjectId == projectId);
                return application;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
