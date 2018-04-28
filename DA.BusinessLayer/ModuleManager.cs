using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.DomainModel;
using DA.DataAccessLayer;

namespace DA.BusinessLayer
{
    public class ModuleManager
    {
        //public IList<tbl_Module> GetAllModules(int? daId)
        public IList<tbl_Module> GetAllModules(int? applicationId)
        {
            try
            {
                IGenericDataRepository<tbl_Module> repository = new GenericDataRepository<tbl_Module>();
                //IList<tbl_Module> modules = repository.GetList(m => m.daId.Equals(daId));
                IList<tbl_Module> modules = repository.GetList(e => e.ApplicationId.Equals(applicationId));
                return modules;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddModule(tbl_Module tblModule)
        {
            try
            {
                IGenericDataRepository<tbl_Module> repository = new GenericDataRepository<tbl_Module>();
                repository.Add(tblModule);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public void UpdateModule(tbl_Module tblModule)
        {
            try
            {
                IGenericDataRepository<tbl_Module> repository = new GenericDataRepository<tbl_Module>();
                repository.Update(tblModule);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public void DeleteModule(tbl_Module tblModule)
        {
            try
            {
                IGenericDataRepository<tbl_Module> repository = new GenericDataRepository<tbl_Module>();
                repository.Remove(tblModule);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public tbl_Module FindModule(int? modID)
        {
            try
            {
                IGenericDataRepository<tbl_Module> repository = new GenericDataRepository<tbl_Module>();
                tbl_Module module = repository.GetSingle(m => m.ModuleID == modID);
                return module;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public tbl_Module FindModuleName(string modName, int applicationId)
        {
            try
            {
                IGenericDataRepository<tbl_Module> repository = new GenericDataRepository<tbl_Module>();
                tbl_Module module = repository.GetSingle(m => m.ModuleName.ToUpper() == modName.ToUpper() && m.ApplicationId == applicationId);
                return module;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public tbl_Module FindModuleNameForRunPlan(int? moduleId)
        {
            try
            {
                IGenericDataRepository<tbl_Module> repository = new GenericDataRepository<tbl_Module>();
                tbl_Module module = repository.GetSingle(m => m.ModuleID == moduleId);
                return module;
            }
            catch(Exception)
            {
                throw;
            }
        }


    }
}
