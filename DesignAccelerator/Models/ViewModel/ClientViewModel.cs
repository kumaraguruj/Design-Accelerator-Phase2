using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using DA.BusinessLayer;
using DA.DomainModel;

namespace DesignAccelerator.Models.ViewModel
{
    public class ClientViewModel  
    {
        #region Public properties

        public int ClientID { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9_ ]*$", ErrorMessage = "Special Characters are not allowed in this field")]
        [Required(ErrorMessage = "Client Name required")]
        public string ClientName { get; set; }

        public int roleId { get; set; }
        public string RoleName { get; set; }

        public bool AddPermmission = false;
        public bool EdiPermission = false;
        public bool DeletePermission = false;

        #endregion

        public void AddClient(ClientViewModel clientViewModel)
        {
            try
            {
                tbl_Clients tblClient = new tbl_Clients();

                tblClient.ClientName = clientViewModel.ClientName;
                tblClient.EntityState = DA.DomainModel.EntityState.Added;

                ClientManager clientManager = new ClientManager();

                clientManager.AddClient(tblClient);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public bool DeleteClient(ClientViewModel clientViewModel)
        {            
            try
            {
                tbl_Clients tblClient = new tbl_Clients();

                tblClient.ClientID = clientViewModel.ClientID;
                tblClient.EntityState = DA.DomainModel.EntityState.Deleted;

                ClientManager clientManager = new ClientManager();
                clientManager.DeleteClient(tblClient);
            }
            catch (Exception)
            {
                throw;
            }         

            return true;
        }

        public void UpdateClient(ClientViewModel clientViewModel)
        {
            try
            {
                tbl_Clients tblClient = new tbl_Clients();

                tblClient.ClientID = clientViewModel.ClientID;
                tblClient.ClientName = clientViewModel.ClientName;
                tblClient.EntityState = DA.DomainModel.EntityState.Modified;

                ClientManager clientManager = new ClientManager();
                clientManager.UpdateClient(tblClient);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public IList<ClientViewModel> ClientList { get; set; }

        public void GetClientDetails()
        {
            try
            {
                ClientManager clientManager = new ClientManager();
                var clientList = clientManager.GetClientDetails();
                ClientList = new List<ClientViewModel>();
                foreach (var item in clientList)
                {
                    ClientViewModel Client = new ClientViewModel();

                    Client.ClientID = item.ClientID;
                    Client.ClientName = item.ClientName;

                    ClientList.Add(Client);
                }
            }
            catch(Exception)
            {
                throw;
            }
          }

        public ClientViewModel FindClient(int? clientID)
        {
            try
            {
                ClientViewModel clientViewModel = new ClientViewModel();

                ClientManager clientManager = new ClientManager();
                var client = clientManager.FindClient(clientID);

                clientViewModel.ClientID = client.ClientID;
                clientViewModel.ClientName = client.ClientName;

                return clientViewModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckDuplicate(ClientViewModel clientViewModel)
        {
            try
            {
                ClientManager clientManager = new ClientManager();

                var client = clientManager.FindClientName(clientViewModel.ClientName);

                if (client != null && client.ClientID != clientViewModel.ClientID && client.ClientName.ToUpper() == clientViewModel.ClientName.ToUpper())
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void GetScreenAccessRights(string  screenName)
        {
            try
            {
                tbl_UserData currentloggedinuserdata = (tbl_UserData)HttpContext.Current.Session["CurrentLoggedInUserDetails"];
                roleId = currentloggedinuserdata.RoleID;

                RoleManager roleManager = new RoleManager();
                var userrolepermissions = roleManager.GetUserViewAccessPermissions(screenName, roleId);
               
                foreach (var item in userrolepermissions)
                {
                    if (item.ActionType == "Add")
                        AddPermmission = true;
                    else if (item.ActionType == "Edit")
                        EdiPermission = true;
                    else if (item.ActionType == "Delete")
                        DeletePermission = true;

                    RoleName = item.RoleName;                     
                }                    
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}