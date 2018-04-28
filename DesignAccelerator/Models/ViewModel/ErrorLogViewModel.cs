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
    public class ErrorLogViewModel
    {

        #region Public properties            

        public string ErrorDescription { get; set; }
        public string StackTrace { get; set; }
        public string ClassName { get; set; }

        #endregion

        public void LogError(Exception filterContext)
        {
            try
            {
                tbl_ErrorLog tblErrorLog = new tbl_ErrorLog();

                tblErrorLog.EntityState = DA.DomainModel.EntityState.Added;
                tblErrorLog.ErrorDescription = Convert.ToString(@filterContext.InnerException);
                tblErrorLog.ClassName = Convert.ToString(filterContext.TargetSite.ReflectedType.Name);
                tblErrorLog.StackTrace = Convert.ToString(@filterContext.StackTrace.ToString());
                tblErrorLog.ErrorDate = DateTime.Now;

                ErrorLogManager errorlogManager = new ErrorLogManager();
                errorlogManager.AddErrorLog(tblErrorLog);
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}