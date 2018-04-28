using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DA.DomainModel;
using DA.BusinessLayer;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DesignAccelerator.Models.ViewModel
{
    public class ProjectViewModel
    {
        #region Public Properties
        public int ProjectID { get; set; }

        [Required(ErrorMessage = "Project name required")]
        [RegularExpression(@"^[a-zA-Z0-9_ ]*$", ErrorMessage = "Special Characters are not allowed in this field")]
        public string ProjectName { get; set; }

        public int RegionId { get; set; }
        public string Region { get; set; }

        public IList<ProjectViewModel> ProjectList { get; set; }
        public IList<tbl_Region> lstRegion { get; set; }
        public IList<tbl_Projects> lstProject { get; set; }

        //Flow Implementation
        public int ClientID { get; set; }
        public string ClientName { get; set; }

        public int roleId { get; set; }
        public string RoleName { get; set; }

        public bool AddPermmission = false;
        public bool EdiPermission = false;
        public bool DeletePermission = false;

        #endregion

        public void AddProject(ProjectViewModel projectViewModel)
        {
            try
            {
                tbl_Projects tblProject = new tbl_Projects();

                tblProject.ProjectName = projectViewModel.ProjectName;
                tblProject.RegionId = projectViewModel.RegionId;
                tblProject.ProjectID = projectViewModel.ProjectID;

                tblProject.ClientId = projectViewModel.ClientID;
                tblProject.EntityState = DA.DomainModel.EntityState.Added;
                                
                ProjectManager projectManager = new ProjectManager();
                projectManager.AddProject(tblProject);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public bool DeleteProject(ProjectViewModel projectViewModel)
        {
            try
            {
                tbl_Projects tblProject = new tbl_Projects();

                tblProject.ProjectID = projectViewModel.ProjectID;
                tblProject.EntityState = DA.DomainModel.EntityState.Deleted;

                ProjectManager projectManager = new ProjectManager();
                projectManager.DeleteProject(tblProject);

                return true;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public void UpdateProject(ProjectViewModel projectViewModel)
        {
            try
            {
                tbl_Projects tblProject = new tbl_Projects();

                tblProject.ProjectID = projectViewModel.ProjectID;
                tblProject.ProjectName = projectViewModel.ProjectName;
                tblProject.RegionId = projectViewModel.RegionId;
                tblProject.ClientId = projectViewModel.ClientID;
                tblProject.EntityState = DA.DomainModel.EntityState.Modified;

                ProjectManager projectManager = new ProjectManager();
                projectManager.UpdateProject(tblProject);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void GetProjectDetails(int clientId)
        {
            try
            {
                ProjectManager projectManager = new ProjectManager();
                RegionManager regionManager = new RegionManager();
                var projectList = projectManager.GetProjectDetails(clientId);

                lstProject = new List<tbl_Projects>();
                lstRegion = new List<tbl_Region>();
                lstRegion = regionManager.GetRegionDetails();
                lstProject = projectManager.GetProjectDetails(clientId);
            }
            catch(Exception)
            {
                throw;
            }

        }
        
        public ProjectViewModel FindProject(int? projectID)
        {
            try
            {
                ProjectViewModel projectViewModel = new ProjectViewModel();
                ProjectManager projectManager = new ProjectManager();
                var project = projectManager.FindProject(projectID);

                RegionManager regionManager = new RegionManager();
                projectViewModel.lstRegion = regionManager.GetRegionDetails();

                projectViewModel.ProjectID = project.ProjectID;
                projectViewModel.ProjectName = project.ProjectName;
                projectViewModel.RegionId = (int)project.RegionId;
                //projectViewModel.Region = projectViewModel.lstRegion[0].Region;
                projectViewModel.Region = projectViewModel.lstRegion.Where(e => e.Id.Equals(project.RegionId)).First().Region;
                projectViewModel.ClientID = project.ClientId;
                return projectViewModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckDuplicate(ProjectViewModel projectViewModel)
        {
            try
            {
                ProjectManager projectManager = new ProjectManager();

                var project = projectManager.FindProjectName(projectViewModel.ProjectName, projectViewModel.ClientID, projectViewModel.RegionId);

                if (project != null && project.ProjectID != projectViewModel.ProjectID && project.ProjectName.ToUpper() == projectViewModel.ProjectName.ToUpper()
                    && project.RegionId == projectViewModel.RegionId)
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

        public IList<tbl_Region> GetRegions(ProjectViewModel projectViewModel)
        {
            try
            {
                RegionManager regionManager = new RegionManager();
                projectViewModel.lstRegion = regionManager.GetRegionDetails();
                return projectViewModel.lstRegion;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void GetScreenAccessRights(string screenName)
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
