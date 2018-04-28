using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.DomainModel;
using DA.DataAccessLayer;
using System.Data;
using System.Data.SqlClient;

namespace DA.BusinessLayer
{
    public class MISReportsManager
    {
        public IList<tbl_UserData> GetUserDetails()
        {
            try
            {
                IGenericDataRepository<tbl_UserData> repository = new GenericDataRepository<tbl_UserData>();
                IList<tbl_UserData> lstUsrData = repository.GetAll();

                return lstUsrData;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public tbl_UserData FindUser(string userName)
        {
            try
            {
                IGenericDataRepository<tbl_UserData> repository = new GenericDataRepository<tbl_UserData>();
                tbl_UserData UsrData = repository.GetSingle(c => c.UserName == userName);
                return UsrData;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<tbl_UserActionArchives> GetUserActions()
        {
            try
            {
                IGenericDataRepository<tbl_UserActionArchives> repository = new GenericDataRepository<tbl_UserActionArchives>();
                IList<tbl_UserActionArchives> lstUsrActions = repository.GetAll();

                return lstUsrActions;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IList<tbl_Actions> GetActions()
        {
            try
            {
                IGenericDataRepository<tbl_Actions> repository = new GenericDataRepository<tbl_Actions>();
                IList<tbl_Actions> lstActions = repository.GetAll();

                return lstActions;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IList<tbl_Region> GetAllRegionDetailsforMIS()
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

        public IList<tbl_Clients> GetAllClientDetailsforMIS()
        {
            try
            {
                IGenericDataRepository<tbl_Clients> repository = new GenericDataRepository<tbl_Clients>();
                IList<tbl_Clients> lstClient = repository.GetAll();

                return lstClient;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<tbl_Projects> GetAllProjectDetailsforMIS()
        {
            try
            {
                IGenericDataRepository<tbl_Projects> repository = new GenericDataRepository<tbl_Projects>();
                IList<tbl_Projects> lstProject = repository.GetAll();

                return lstProject;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<tbl_AppVersion> GetAllAppVersionDetailsforMIS()
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

        public IList<tbl_Applications> GetAllApplicationDetailsforMIS()//int? projectId
        {
            try
            {
                IGenericDataRepository<tbl_Applications> repository = new GenericDataRepository<tbl_Applications>();
                IList<tbl_Applications> lstApplications = repository.GetAll();
                return lstApplications;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
