using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DA.DomainModel;
using DA.BusinessLayer;


namespace DesignAccelerator.Models.ViewModel
{
    public class ExportDAVM
    {
        public IList<tbl_Reports> isLinkedlst { get; set; }

        public IList<tbl_BusinessRule> linkedBRlst { get; set; }

        public IList<tbl_TxnAttributeMapping> linkedAttrVals { get; set; }

        public IList<tbl_Interface> linkedInterfacelst { get; set; }

        public IList<tbl_ChannelAlert> linkedCAlst { get; set; }

        public IList<tbl_Reports> GetLinkedDataReports(int? id)
        {
            try
            {
                ExportDAManager exportDaManager = new ExportDAManager();

                isLinkedlst = exportDaManager.GetLinkedDataForReports(id);

                return isLinkedlst;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IList<tbl_BusinessRule> GetLinkedDataBR(int id)
        {
            try
            {
                ExportDAManager exportDAManager = new ExportDAManager();

                linkedBRlst = exportDAManager.GetLinkedDataForBR(id);

                return linkedBRlst;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public IList<tbl_TxnAttributeMapping> GetLinkedTransactionAttributes(int id)
        {
            try
            {
                ExportDAManager exportDAManager = new ExportDAManager();

                linkedAttrVals = exportDAManager.GetLinkedDataAttrVals(id);

                return linkedAttrVals;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public IList<tbl_Interface> GetLinkedDataInterface(int id)
        {
            try
            {
                ExportDAManager exportDAManager = new ExportDAManager();

                linkedInterfacelst = exportDAManager.GetLinkedDataForInterface(id);

                return linkedInterfacelst;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public IList<tbl_ChannelAlert> GetLinkedDataForCA(int id)
        {
            try
            {
                ExportDAManager exportDAManager = new ExportDAManager();

                linkedCAlst = exportDAManager.GetLinkedDataForChannelAlert(id);

                return linkedCAlst;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}