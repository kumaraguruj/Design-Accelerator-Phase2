using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using DA.DomainModel;
using DA.DataAccessLayer;

namespace DA.BusinessLayer
{
    public class ChannelsAndAlertsManager
    {
        public List<sp_GetAllChannelsAndAlerts_Result> GetAllChannelAndAlerts(int daId)
        {
            try
            {
                IGenericDataRepository<sp_GetAllChannelsAndAlerts_Result> repository = new GenericDataRepository<sp_GetAllChannelsAndAlerts_Result>();
                return repository.ExecuteStoredProcedure("EXEC sp_GetAllChannelsAndAlerts @DAId", new SqlParameter("DAId", SqlDbType.Int) { Value = daId }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IList<tbl_Attribute> GetAllAttributes(int? daId)
        {
            try
            {
                IGenericDataRepository<tbl_Attribute> repository = new GenericDataRepository<tbl_Attribute>();
                IList<tbl_Attribute> lstAttributes = repository.GetList(e => e.daId.Equals(daId));

                return lstAttributes;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IList<tbl_Transactions> GetAllTransactions(int? daId)
        {
            try
            {
                IGenericDataRepository<tbl_Transactions> repository = new GenericDataRepository<tbl_Transactions>();
                IList<tbl_Transactions> lstTransactions = repository.GetList(e => e.daId.Equals(daId));

                return lstTransactions;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IList<tbl_AttributeValues> GetAllAttributeValues(int? attrID)
        {
            try
            {
                IGenericDataRepository<tbl_AttributeValues> repository = new GenericDataRepository<tbl_AttributeValues>();
                IList<tbl_AttributeValues> lstAttrValues = repository.GetList(e => e.AttributeID.Equals(attrID));

                return lstAttrValues;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IList<tbl_DistributionType> GetAllDistributionTypes(int? daID)
        {
            try
            {
                IGenericDataRepository<tbl_DistributionType> repository = new GenericDataRepository<tbl_DistributionType>();
                IList<tbl_DistributionType> lstDistributionTypes = repository.GetList(e => e.daId.Equals(daID));

                return lstDistributionTypes;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IList<tbl_FrequencyType> GetAllFrequencyTypes(int? daID)
        {
            try
            {
                IGenericDataRepository<tbl_FrequencyType> repository = new GenericDataRepository<tbl_FrequencyType>();
                IList<tbl_FrequencyType> lstFrequencyTypes = repository.GetList(e => e.daId.Equals(daID));

                return lstFrequencyTypes;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void AddChannelAlertsAttrMapping(tbl_ChannelAlertAttrMapping tblchannelalertattrmapping)
        {
            try
            {
                IGenericDataRepository<tbl_ChannelAlertAttrMapping> repository = new GenericDataRepository<tbl_ChannelAlertAttrMapping>();
                repository.Add(tblchannelalertattrmapping);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddChannelAlert(tbl_ChannelAlert tblchannelalerts)
        {
            try
            {
                IGenericDataRepository<tbl_ChannelAlert> repository = new GenericDataRepository<tbl_ChannelAlert>();
                repository.Add(tblchannelalerts);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public int SaveChannelAlertDataMapping(IList<tbl_ChannelAlertAttrMapping> tblChannelAlertAttrMapping)
        {
            try
            {
                IGenericDataRepository<tbl_ChannelAlertAttrMapping> repository = new GenericDataRepository<tbl_ChannelAlertAttrMapping>();
                //if(tblChannelAlertAttrMapping.Count > 0)
                //{ 
                foreach (var item in tblChannelAlertAttrMapping)
                    repository.Add(item);
                return 1;
                //}
                //else
                //{
                //    return 0;
                //}
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public void DeleteChannelAttrMapId(int id)
        {
            try
            {
                tbl_ChannelAlert tblchannelAlert = new tbl_ChannelAlert();
                IGenericDataRepository<tbl_ChannelAlert> repository = new GenericDataRepository<tbl_ChannelAlert>();
                tbl_ChannelAlertAttrMapping objtblChannelAlertMapping = new tbl_ChannelAlertAttrMapping();
                IGenericDataRepository<tbl_ChannelAlertAttrMapping> repository1 = new GenericDataRepository<tbl_ChannelAlertAttrMapping>();

                IList<tbl_ChannelAlert> lstChannelalert = repository.GetList(q => q.ChannelAlertID.Equals(id));

                if(lstChannelalert!=null)
                {
                    foreach (var item in lstChannelalert)
                    {
                        tblchannelAlert.ChannelAlertID = item.ChannelAlertID;
                        tblchannelAlert.ChannelAlertAttrMapID = item.ChannelAlertAttrMapID;
                        tblchannelAlert.daId = item.daId;
                        tblchannelAlert.TransactionSeq = item.TransactionSeq;

                        tblchannelAlert.EntityState = EntityState.Deleted;

                        IList<tbl_ChannelAlertAttrMapping> lstChannelAlertMappingid = repository1.GetList(q => q.ChannelAlertAttrMapID.Equals(item.ChannelAlertAttrMapID));

                        if(lstChannelAlertMappingid!=null)
                        {
                            foreach (var item1 in lstChannelAlertMappingid)
                            {
                                objtblChannelAlertMapping.AttrID1 = item1.AttrID1;
                                objtblChannelAlertMapping.AttrID2 = item1.AttrID2;
                                objtblChannelAlertMapping.AttrID3 = item1.AttrID3;
                                objtblChannelAlertMapping.AttrID4 = item1.AttrID4;
                                objtblChannelAlertMapping.AttrID5 = item1.AttrID5;
                                objtblChannelAlertMapping.AttrID6 = item1.AttrID6;
                                objtblChannelAlertMapping.AttrID7 = item1.AttrID7;
                                objtblChannelAlertMapping.AttrID8 = item1.AttrID8;
                                objtblChannelAlertMapping.AttrID9 = item1.AttrID9;
                                objtblChannelAlertMapping.AttrID10 = item1.AttrID10;
                                objtblChannelAlertMapping.AttrValueID1 = item1.AttrValueID1;
                                objtblChannelAlertMapping.AttrValueID2 = item1.AttrValueID2;
                                objtblChannelAlertMapping.AttrValueID3 = item1.AttrValueID3;
                                objtblChannelAlertMapping.AttrValueID4 = item1.AttrValueID4;
                                objtblChannelAlertMapping.AttrValueID5 = item1.AttrValueID5;
                                objtblChannelAlertMapping.AttrValueID6 = item1.AttrValueID6;
                                objtblChannelAlertMapping.AttrValueID7 = item1.AttrValueID7;
                                objtblChannelAlertMapping.AttrValueID8 = item1.AttrValueID8;
                                objtblChannelAlertMapping.AttrValueID9 = item1.AttrValueID9;
                                objtblChannelAlertMapping.AttrValueID10 = item1.AttrValueID10;
                                objtblChannelAlertMapping.ChannelAlertAttrMapID = item1.ChannelAlertAttrMapID;
                                objtblChannelAlertMapping.DestnID = item1.DestnID;
                                objtblChannelAlertMapping.DistributionTypeID = item1.DistributionTypeID;
                                objtblChannelAlertMapping.FreqID = item1.FreqID;
                                objtblChannelAlertMapping.IsManual = item1.IsManual;
                                objtblChannelAlertMapping.MessageDesc = item1.MessageDesc;
                                objtblChannelAlertMapping.ModeTypeID = item1.ModeTypeID;
                                objtblChannelAlertMapping.SourceID = item1.SourceID;
                                objtblChannelAlertMapping.ReqReference = item1.ReqReference;
                                objtblChannelAlertMapping.EntityState = EntityState.Deleted;

                                repository1.Remove(objtblChannelAlertMapping);
                            }
                        }
                        repository.Remove(tblchannelAlert);
                    }
                }
               
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
