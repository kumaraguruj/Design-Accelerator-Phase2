using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.DomainModel;
using DA.DataAccessLayer;

namespace DA.BusinessLayer
{
    public class ErrorLogManager
    {
        public void AddErrorLog(tbl_ErrorLog tblErrorLog)
        {
            try
            {
                IGenericDataRepository<tbl_ErrorLog> repository = new GenericDataRepository<tbl_ErrorLog>();
                repository.Add(tblErrorLog);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
