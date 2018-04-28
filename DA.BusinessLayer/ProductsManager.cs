using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.DomainModel;
using DA.DataAccessLayer;

namespace DA.BusinessLayer
{
    public class ProductsManager
    {
        public IList<tbl_LOB> GetLOBs(int? daid)
        {
            try
            {
                IGenericDataRepository<tbl_LOB> repository = new GenericDataRepository<tbl_LOB>();
                IList<tbl_LOB> lstLOBs = repository.GetList(e => e.daId.Equals(daid));

                return lstLOBs;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<tbl_BuzProd> GetBuzProds(int? daid)
        {
            try
            {
                IGenericDataRepository<tbl_BuzProd> repository = new GenericDataRepository<tbl_BuzProd>();
                IList<tbl_BuzProd> lstBuzProds = repository.GetList(e => e.daId.Equals(daid));

                return lstBuzProds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<tbl_Products> GetAllProducts(int? daId)
        {
            try
            {
                IGenericDataRepository<tbl_Products> repository = new GenericDataRepository<tbl_Products>();
                IList<tbl_Products> lstProds = repository.GetList(e => e.daid.Equals(daId));

                return lstProds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddProduct(tbl_Products tblProducts)
        {
            try
            {
                IGenericDataRepository<tbl_Products> repository = new GenericDataRepository<tbl_Products>();
                repository.Add(tblProducts);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //changed as per requirement for flow implementation on - 15/11/2016
        public tbl_Products FindProduct(int? ProductID)
        {
            try
            {
                IGenericDataRepository<tbl_Products> repository = new GenericDataRepository<tbl_Products>();
                tbl_Products tblProducts = repository.GetSingle(b => b.ProductID == ProductID);
                return tblProducts;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_Products FindProductReqRef(string reqRef, int lobdesc, int buzproddesc, int daId)
        {
            try
            {
                IGenericDataRepository<tbl_Products> repository = new GenericDataRepository<tbl_Products>();
                tbl_Products tblProducts = repository.GetSingle(b => b.ReqReference.ToUpper() == reqRef.ToUpper() && b.LobID == lobdesc && b.BuzProdID == buzproddesc
                                                                && b.daid == daId);
                return tblProducts;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteProduct(tbl_Products tblProducts)
        {
            try
            {
                IGenericDataRepository<tbl_Products> repository = new GenericDataRepository<tbl_Products>();
                repository.Remove(tblProducts);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateProduct(tbl_Products tblProduct)
        {
            try
            {
                IGenericDataRepository<tbl_Products> repository = new GenericDataRepository<tbl_Products>();
                repository.Update(tblProduct);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
