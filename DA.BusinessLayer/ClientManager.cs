using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.DomainModel;
using DA.DataAccessLayer;

namespace DA.BusinessLayer
{
    public class ClientManager
    {
        public void AddClient(tbl_Clients tblClient)
        {
            try
            {
                IGenericDataRepository<tbl_Clients> repository = new GenericDataRepository<tbl_Clients>();

                repository.Add(tblClient);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public void DeleteClient(tbl_Clients tblClient)
        {
            try
            {
                IGenericDataRepository<tbl_Clients> repository = new GenericDataRepository<tbl_Clients>();
                repository.Remove(tblClient);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateClient(tbl_Clients tblClient)
        {
            try
            {
                IGenericDataRepository<tbl_Clients> repository = new GenericDataRepository<tbl_Clients>();

                repository.Update(tblClient);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public IList<tbl_Clients> GetClientDetails()
        {
            try
            {
                IGenericDataRepository<tbl_Clients> repository = new GenericDataRepository<tbl_Clients>();
                IList<tbl_Clients> lstClients = repository.GetAll();

                return lstClients;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public tbl_Clients FindClient(int? clientID)
        {
            try
            {
                IGenericDataRepository<tbl_Clients> repository = new GenericDataRepository<tbl_Clients>();
                tbl_Clients client = repository.GetSingle(c => c.ClientID == clientID);
                return client;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public tbl_Clients FindClientName(string clientName)
        {
            try
            {
                IGenericDataRepository<tbl_Clients> repository = new GenericDataRepository<tbl_Clients>();
                tbl_Clients tblclientName = repository.GetSingle(c => c.ClientName.ToUpper() == clientName.ToUpper());
                return tblclientName;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
