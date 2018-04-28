using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.DataAccessLayer;
using DA.DomainModel;

namespace DA.BusinessLayer
{
    public class ChangePasswordManager
    {
        public void AddChangePassword(tbl_ChangePassword tblChangePassword)
        {
            IGenericDataRepository<tbl_ChangePassword> repository = new GenericDataRepository<tbl_ChangePassword>();
            repository.Add(tblChangePassword);
        }

        public void DeleteChangePassword(tbl_ChangePassword tblChangePassword)
        {
            IGenericDataRepository<tbl_ChangePassword> repository = new GenericDataRepository<tbl_ChangePassword>();
            repository.Remove(tblChangePassword);
        }

        public void UpdateChangePassword(tbl_ChangePassword tblChangePassword)
        {
            IGenericDataRepository<tbl_ChangePassword> repository = new GenericDataRepository<tbl_ChangePassword>();
            repository.Update(tblChangePassword);
        }

        public IList<tbl_ChangePassword> GetChangePwdDetails()
        {
            IGenericDataRepository<tbl_ChangePassword> repository = new GenericDataRepository<tbl_ChangePassword>();
            IList<tbl_ChangePassword> lstChangePwd = repository.GetAll();

            return lstChangePwd;
        }

        public tbl_ChangePassword FindChangePwdDetails(int? changePwdId)
        {
            try
            {
                IGenericDataRepository<tbl_ChangePassword> repository = new GenericDataRepository<tbl_ChangePassword>();
                tbl_ChangePassword changePwd = repository.GetSingle(c => c.UserID == changePwdId);
                return changePwd;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public tbl_ChangePassword FindChangePwdUserName(string userName)
        {
            IGenericDataRepository<tbl_ChangePassword> repository = new GenericDataRepository<tbl_ChangePassword>();
            tbl_ChangePassword tblChangePassword = repository.GetSingle(c => c.UserName.ToUpper() == userName.ToUpper());
            return tblChangePassword;
        }
    }
}
