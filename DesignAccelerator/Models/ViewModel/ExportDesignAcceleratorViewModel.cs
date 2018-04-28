using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DA.DomainModel;
using DA.BusinessLayer;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeOpenXml;


namespace DesignAccelerator.Models.ViewModel
{
    public class ExportDesignAcceleratorViewModel
    {
        #region Public properties
        public int BuzRuleID { get; set; }
        public int TransactionSeq { get; set; }
        public int daId { get; set; }
        public int BuzRuleAttrMapID { get; set; }

        public IList<tbl_Products> lstprods { get; set; }
        public IList<tbl_Transactions> lstTransactions { get; set; }
        public IList<tbl_Attribute> lstattribs { get; set; }
        #endregion

        public IList<tbl_Products> GetProducts(int? daId)
        {
            try
            {
                ProductsManager ProdManager = new ProductsManager();
                IList<tbl_Products> lstprods = ProdManager.GetAllProducts(daId);

                return lstprods;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public IList<tbl_Transactions> GetTransactions(int? daId)
        {
            try
            {
                TransactionsManager transManager = new TransactionsManager();
                IList<tbl_Transactions> lstTransactions = transManager.GetAllTransactions(daId);

                return lstTransactions;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public IList<tbl_Attribute> GetAttributes(int? daId)
        {
            try
            {
                AttributeListManager attribManager = new AttributeListManager();
                IList<tbl_Attribute> lstattribs = attribManager.GetAttributeList(daId);

                return lstattribs;
            }
            catch (Exception)
            {

                throw;
            }

        }

        //public IList<tbl_AttributeValues> GetAttributeValues(int? daId)
        //{
        //    AttributeValueManager attribValManager = new AttributeValueManager();
        //    IList<tbl_AttributeValues> lstattribVal = attribValManager.GetAttributeValList(daId);

        //    return lstattribVal;
        //}
    }
}