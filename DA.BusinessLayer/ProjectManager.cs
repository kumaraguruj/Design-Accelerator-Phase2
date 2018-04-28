using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.DataAccessLayer;
using DA.DomainModel;

namespace DA.BusinessLayer
{
    public class ProjectManager
    {
        public void AddProject(tbl_Projects tblProjects)
        {
            try
            {
                IGenericDataRepository<tbl_Projects> repository = new GenericDataRepository<tbl_Projects>();
                repository.Add(tblProjects);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public void DeleteProject(tbl_Projects tblProjects)
        {
            try
            {
                IGenericDataRepository<tbl_Projects> repository = new GenericDataRepository<tbl_Projects>();
                repository.Remove(tblProjects);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public void UpdateProject(tbl_Projects tblProjects)
        {
            try
            {
                IGenericDataRepository<tbl_Projects> repository = new GenericDataRepository<tbl_Projects>();
                repository.Update(tblProjects);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public IList<tbl_Projects> GetProjectDetails(int clientId)
        {
            try
            {
                IGenericDataRepository<tbl_Projects> repository = new GenericDataRepository<tbl_Projects>();
                IList<tbl_Projects> lstProjects = repository.GetList(e => e.ClientId.Equals(clientId));

                return lstProjects;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public tbl_Projects FindProject(int? projectID)
        {
            try
            {
                IGenericDataRepository<tbl_Projects> repository = new GenericDataRepository<tbl_Projects>();
                tbl_Projects Project = repository.GetSingle(c => c.ProjectID == projectID);
                return Project;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_Projects FindProjectName(string projectName, int clientId, int regionId)
        {
            try
            {
                IGenericDataRepository<tbl_Projects> repository = new GenericDataRepository<tbl_Projects>();
                tbl_Projects Project = repository.GetSingle(c => c.ProjectName.ToUpper() == projectName.ToUpper() && c.ClientId == clientId && c.RegionId == regionId);
                return Project;
            }
            catch(Exception)
            {
                throw;
            }
        }

    }
}
