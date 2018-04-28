using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.DataAccessLayer;
using DA.DomainModel;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace DA.BusinessLayer
{
    public class UserManager
    {
        public void AddUserData(tbl_UserData tblUserData)
        {
            try
            {
                IGenericDataRepository<tbl_UserData> repository = new GenericDataRepository<tbl_UserData>();
                repository.Add(tblUserData);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void UpdateUserData(tbl_UserData tblUserData)
        {
            try
            {
                IGenericDataRepository<tbl_UserData> repository = new GenericDataRepository<tbl_UserData>();
                repository.Update(tblUserData);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void DeleteUserData(tbl_UserData tblUserData)
        {
            try
            {
                IGenericDataRepository<tbl_UserData> repository = new GenericDataRepository<tbl_UserData>();
                repository.Remove(tblUserData);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public IList<tbl_Roles> GetRoles(int? userId)
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

        public IList<tbl_UserData> GetUserDetails()//int userId
        {
            try
            {
                IGenericDataRepository<tbl_UserData> repository = new GenericDataRepository<tbl_UserData>();
                IList<tbl_UserData> lstUsrData = repository.GetAll();
                //IList<tbl_UserData> lstUsrData = repository.GetList(e => e.UserID.Equals(userId));

                return lstUsrData;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public IList<sp_GetAuthorizers_Result> GetAdminUsersToAuthorize(string userrole, int userid)
        {
            try
            {
                IGenericDataRepository<sp_GetAuthorizers_Result> repository = new GenericDataRepository<sp_GetAuthorizers_Result>();
                return repository.ExecuteStoredProcedure("EXEC sp_GetAuthorizers @RoleName, @UserID ", new SqlParameter("RoleName", SqlDbType.VarChar) { Value = userrole }, new SqlParameter("UserID", SqlDbType.Int) { Value = userid }).ToList();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public tbl_UserData FindUserData(int? userId)
        {
            try
            {
                IGenericDataRepository<tbl_UserData> repository = new GenericDataRepository<tbl_UserData>();
                tbl_UserData userData = repository.GetSingle(c => c.UserID == userId);
                return userData;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public tbl_UserData FindUserName(string userName) //, string password
        {
            try
            {
                IGenericDataRepository<tbl_UserData> repository = new GenericDataRepository<tbl_UserData>();

                tbl_UserData userData = repository.GetSingle(c => c.UserName.ToUpper() == userName.ToUpper());// && c.Password == password
                return userData;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public tbl_UserData FindUserName(string userName, string password)
        {
            try
            {
                IGenericDataRepository<tbl_UserData> repository = new GenericDataRepository<tbl_UserData>();

                tbl_UserData userData = repository.GetSingle(c => c.UserName.ToUpper() == userName.ToUpper() && c.Password == password);
                return userData;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public int FindRoleID(string userName, string password)
        {
            try
            {
                IGenericDataRepository<tbl_UserData> repository = new GenericDataRepository<tbl_UserData>();

                int userData = repository.GetSingle(q => q.UserName.ToUpper() == userName.ToUpper() && q.Password == password).RoleID;
                return userData;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public IList<tbl_RoleScreenMapping> GetMappedIDs(int roleId)
        {
            try
            {
                IGenericDataRepository<tbl_RoleScreenMapping> rep = new GenericDataRepository<tbl_RoleScreenMapping>();
                IList<tbl_RoleScreenMapping> mappedIds = rep.GetList(q => q.RoleID.Equals(roleId));
                return mappedIds;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public IList<sp_GetUsersToAuthorize_Result> GetAllUsersToAuthorize(int AuthID)
        {
            try
            {
                IGenericDataRepository<sp_GetUsersToAuthorize_Result> repository = new GenericDataRepository<sp_GetUsersToAuthorize_Result>();
                return repository.ExecuteStoredProcedure("EXEC sp_GetUsersToAuthorize @AuthId", new SqlParameter("AuthID", SqlDbType.Int) { Value = AuthID }).ToList();
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
