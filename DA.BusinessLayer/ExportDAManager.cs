using DA.DataAccessLayer;
using DA.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.BusinessLayer
{
    public class ExportDAManager
    {
        public IList<tbl_Reports> GetLinkedDataForReports(int? daid)
        {
            try
            {
                IGenericDataRepository<tbl_Reports> repository = new GenericDataRepository<tbl_Reports>();
                IList<tbl_Reports> linkedData = repository.GetList(e => e.daId.Equals(daid), e => e.tbl_Transactions);

                return linkedData;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public IList<tbl_BusinessRule> GetLinkedDataForBR(int daid)
        {
            try
            {
                IGenericDataRepository<tbl_BusinessRule> repository = new GenericDataRepository<tbl_BusinessRule>();
                IList<tbl_BusinessRule> linkedBRData = repository.GetList(e => e.daId.Equals(daid), e => e.tbl_BuzRulesAttrMapping, e => e.tbl_Transactions);

                return linkedBRData;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public IList<tbl_TxnAttributeMapping> GetLinkedDataAttrVals(int daid)
        {
            try
            {
                IGenericDataRepository<tbl_TxnAttributeMapping> repository = new GenericDataRepository<tbl_TxnAttributeMapping>();
                IList<tbl_TxnAttributeMapping> linkedAttrVals = repository.GetList(e => e.daId.Equals(daid), e => e.tbl_Attribute, e => e.tbl_Transactions);

                return linkedAttrVals;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public IList<tbl_Interface> GetLinkedDataForInterface(int daid)
        {
            try
            {
                IGenericDataRepository<tbl_Interface> repository = new GenericDataRepository<tbl_Interface>();
                IList<tbl_Interface> linkedInterfaceLst = repository.GetList(e => e.daId.Equals(daid), e => e.tbl_InterfaceAttrMapping, e => e.tbl_Transactions);

                return linkedInterfaceLst;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public IList<tbl_ChannelAlert> GetLinkedDataForChannelAlert(int daid)
        {
            try
            {
                IGenericDataRepository<tbl_ChannelAlert> repository = new GenericDataRepository<tbl_ChannelAlert>();
                IList<tbl_ChannelAlert> linkedCAlst = repository.GetList(e => e.daId.Equals(daid), e => e.tbl_ChannelAlertAttrMapping, e => e.tbl_Transactions);

                return linkedCAlst;
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
