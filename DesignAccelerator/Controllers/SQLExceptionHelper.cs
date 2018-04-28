using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DesignAccelerator.Controllers
{
    public class SQLExceptionHelper
    {
        public void SqlExceptionHelper(SqlException sqlException)
        {
            // Do Nothing.
        }

        public static string GetDBUpdateSqlDescription(DbUpdateException exception)
        {
            string sException = exception.InnerException.InnerException.Message;
        
            if (sException.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
            {
                return "Referential Integrity";
            }
            else
            {
                return "";
            }
            //switch ()
            //{
            //    case 21:
            //        return "Fatal Error Occurred: Error Code 21.";
            //    case 53:
            //        return "Error in Establishing a Database Connection: 53.";
            //    default:
            //        return ("Unexpected Error: " + sqlException.Message.ToString());
            //}
            
        }
    }
}