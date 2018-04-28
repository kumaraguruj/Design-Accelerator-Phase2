using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using DesignAccelerator.Models.ViewModel;
using DA.BusinessLayer;
using DA.DomainModel;

namespace DesignAccelerator.Controllers
{
    public class CommonFunctions
    {
        // GET: Common

        public string GetClientName(int clientId)
        {
            try
            {
                tbl_Clients tblClient = new tbl_Clients();
                ClientManager clientManager = new ClientManager();
                tblClient = clientManager.FindClient(clientId);
                return tblClient.ClientName;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void GetProjectName(int prjId, out int clientId, out string prjName)
        {
            try
            {
                tbl_Projects tblProj = new tbl_Projects();
                ProjectManager prjManager = new ProjectManager();
                tblProj = prjManager.FindProject(prjId);
                clientId = tblProj.ClientId;
                prjName = tblProj.ProjectName;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public void GetApplicationName(int appId, out int projectId, out string appName)
        {
            try
            {
                tbl_Applications tblApp = new tbl_Applications();
                ApplicationManager appManager = new ApplicationManager();
                tblApp = appManager.FindApplication(appId);
                projectId = tblApp.ProjectId;
                appName = tblApp.ApplicationName;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public void GetModuleName(int modId, out int applicationId, out string modName)
        {
            try
            {
                tbl_Module tblModule = new tbl_Module();
                ModuleManager modManager = new ModuleManager();
                tblModule = modManager.FindModule(modId);
                applicationId = (int)tblModule.ApplicationId;
                modName = tblModule.ModuleName;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public DAViewModel FindDA(int? daID)
        {
            try
            {
                DAViewModel dAViewModel = new DAViewModel();
                DAManager daManager = new DAManager();
                var da = daManager.FindDA(daID);
                dAViewModel.DAID = da.daid;
                dAViewModel.DAName = da.daName;
                dAViewModel.ModuleId = (int)da.ModuleId;
                return dAViewModel;
            }
            catch(Exception)
            {
                throw;
            }
        }
        public void GetProjectNameForDuplicateCheck(int prjId, int clientId, string prjName)
        {
            try
            {
                tbl_Projects tblProj = new tbl_Projects();
                ProjectManager prjManager = new ProjectManager();
                tblProj = prjManager.FindProject(prjId);
                clientId = tblProj.ClientId;
                prjName = tblProj.ProjectName;
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}