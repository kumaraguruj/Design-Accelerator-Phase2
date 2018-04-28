using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.DomainModel;
using DA.DataAccessLayer;

namespace DA.BusinessLayer
{
    public class BuzProdManager
    {
        public IList<tbl_BuzProd> GetBusinessProducts(int? daId)
        {
            try
            {
                IGenericDataRepository<tbl_BuzProd> repository = new GenericDataRepository<tbl_BuzProd>();
                IList<tbl_BuzProd> lstBuzProds = repository.GetList(e => e.daId.Equals(daId));

                return lstBuzProds;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public void AddBuzProd(tbl_BuzProd tblbuzprod)
        {
            try
            {
                IGenericDataRepository<tbl_BuzProd> repository = new GenericDataRepository<tbl_BuzProd>();
                repository.Add(tblbuzprod);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public tbl_BuzProd FindBuzProd(int? buzProdID)
        {
            try
            {
                IGenericDataRepository<tbl_BuzProd> repository = new GenericDataRepository<tbl_BuzProd>();
                tbl_BuzProd tblbuzprod = repository.GetSingle(b => b.BuzProdID == buzProdID);
                return tblbuzprod;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public void DeleteBuzProd(tbl_BuzProd tblbuzprod)
        {
            try
            {
                IGenericDataRepository<tbl_BuzProd> repository = new GenericDataRepository<tbl_BuzProd>();
                repository.Remove(tblbuzprod);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public void UpdateBuzProd(tbl_BuzProd tblbuzprod)
        {
            try
            {
                IGenericDataRepository<tbl_BuzProd> repository = new GenericDataRepository<tbl_BuzProd>();
                repository.Update(tblbuzprod);
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
