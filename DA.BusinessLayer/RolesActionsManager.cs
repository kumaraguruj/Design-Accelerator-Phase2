using DA.DataAccessLayer;
using DA.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.BusinessLayer
{
    public class RolesActionsManager
    {
        public IList<tbl_Screens> GetAllScreens()
        {
            try
            {
                IGenericDataRepository<tbl_Screens> repository = new GenericDataRepository<tbl_Screens>();
                IList<tbl_Screens> lstScreens = repository.GetAll();

                return lstScreens;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public IList<tbl_Roles> GetAllRoles()
        {
            try
            {
                IGenericDataRepository<tbl_Roles> repository = new GenericDataRepository<tbl_Roles>();
                IList<tbl_Roles> lstRoles = repository.GetAll();

                return lstRoles;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public IList<tbl_RoleScreenMapping> GetMappedScreenRoles()
        {
            try
            {
                IGenericDataRepository<tbl_RoleScreenMapping> repository = new GenericDataRepository<tbl_RoleScreenMapping>();
                IList<tbl_RoleScreenMapping> lstscreenRoles = repository.GetAll();

                return lstscreenRoles;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public int SaveData(IList<tbl_RoleScreenMapping> screenRoleMapping)
        {
            try
            {
                IGenericDataRepository<tbl_RoleScreenMapping> repository = new GenericDataRepository<tbl_RoleScreenMapping>();
                foreach (var item in screenRoleMapping)
                    repository.Add(item);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }

        }

        public void DeleteMappedScreenRole(tbl_RoleScreenMapping roleScreenMapping)
        {
            try
            {
                IGenericDataRepository<tbl_RoleScreenMapping> repository = new GenericDataRepository<tbl_RoleScreenMapping>();
                var a = repository.GetSingle(q => q.RoleScreenMappingID == roleScreenMapping.RoleScreenMappingID);
                repository.Remove(roleScreenMapping);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
