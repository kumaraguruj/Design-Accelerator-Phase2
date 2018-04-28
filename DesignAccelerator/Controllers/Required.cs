using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace DesignAccelerator.Controllers
{
    public class Required
    {
      
        private DataTable _dtMappingTable;
        private DataTable _dtTM;
        private Dictionary<string, IList<DA.DomainModel.tbl_AttributeValues>> _dicAttributeValue;
        private Dictionary<string, int> _dicCriticalAttributes;
        public Required()
        {

            this._dtTM = new DataTable();
            this._dtMappingTable = new DataTable();
            this._dicAttributeValue = new Dictionary<string, IList<DA.DomainModel.tbl_AttributeValues>>();
            this._dicCriticalAttributes = new Dictionary<string, int>();
        }


    
        public DataTable dtMappingTable { get { return this._dtMappingTable; } set { } }
        public DataTable dtTM
        {
            get { return this._dtTM; }
            set {}
        }
        public Dictionary<string, IList<DA.DomainModel.tbl_AttributeValues>> dicAttributeValue
        {
            get { return this._dicAttributeValue; }
            set { }
        }
        public Dictionary<string, int> dicCriticalAttributes
        {
            get { return this._dicCriticalAttributes; }
            set {}
        }

    }
}