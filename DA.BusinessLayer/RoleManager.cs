using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.DataAccessLayer;
using DA.DomainModel;
using System.Data;
using System.Data.SqlClient;

namespace DA.BusinessLayer
{
    public class RoleManager
    {
        public IList<tbl_Roles> GetRoleDetails()
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

        public void AddRoles(tbl_Roles tblRoles)
        {
            try
            {
                IGenericDataRepository<tbl_Roles> repository = new GenericDataRepository<tbl_Roles>();
                repository.Add(tblRoles);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void UpdateRoles(tbl_Roles tblRoles)
        {
            try
            {
                IGenericDataRepository<tbl_Roles> repository = new GenericDataRepository<tbl_Roles>();
                repository.Update(tblRoles);
            }
            catch (Exception)
            {

                throw;
            }

        }

       public void DeleteRoleMapping(int roleId)
        {
            IGenericDataRepository<tbl_Roles> roleRepository = new GenericDataRepository<tbl_Roles>();
            IGenericDataRepository<tbl_RoleScreenMapping> roleScreenMappingRepository = new GenericDataRepository<tbl_RoleScreenMapping>();
            try
            {
                var rolesScreenMapping = roleRepository.GetAll(a => a.tbl_RoleScreenMapping).SingleOrDefault(a => a.RoleID == roleId);

                if (rolesScreenMapping != null)
                {
                    foreach (var roleScreen in rolesScreenMapping.tbl_RoleScreenMapping
                        .Where(at => at.RoleID == roleId).ToList())
                    {
                        roleScreen.EntityState = DA.DomainModel.EntityState.Deleted;
                        roleScreenMappingRepository.Remove(roleScreen);
                    }

                    rolesScreenMapping.EntityState = DA.DomainModel.EntityState.Deleted;
                    roleRepository.Remove(rolesScreenMapping);
                    //repository.SaveChanges();
                }
                //var roles = repository1.GetAll(b => b.tbl_Roles).SingleOrDefault(b => b.RoleID == roleId);
                //roles.EntityState = DA.DomainModel.EntityState.Deleted;
                //repository1.Remove(roles);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_Roles FindRoles(int? roleId)
        {
            try
            {
                IGenericDataRepository<tbl_Roles> repository = new GenericDataRepository<tbl_Roles>();
                tbl_Roles roles = repository.GetSingle(c => c.RoleID == roleId);
                return roles;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_Roles FindRoleName(string roleName)
        {
            try
            {
                IGenericDataRepository<tbl_Roles> repository = new GenericDataRepository<tbl_Roles>();
                tbl_Roles tblRole = repository.GetSingle(c => c.RoleName.ToUpper() == roleName.ToUpper());
                return tblRole;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public IList<sp_GetUserAccessInView_Result> GetUserViewAccessPermissions(string screenname, int roleid)
        {
            try
            {
                IGenericDataRepository<sp_GetUserAccessInView_Result> repository = new GenericDataRepository<sp_GetUserAccessInView_Result>();
                return repository.ExecuteStoredProcedure("EXEC sp_GetUserAccessInView @ScreenName, @RoleID ", new SqlParameter("ScreenName", SqlDbType.VarChar) { Value = screenname }, new SqlParameter("RoleID", SqlDbType.Int) { Value = roleid }).ToList();
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
