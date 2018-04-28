using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DA.BusinessLayer;
using DA.DomainModel;

namespace DesignAccelerator.Models.ViewModel
{
    public class ChannelsAndAlertsViewModel
    {
        #region Public Variables

        [System.ComponentModel.DataAnnotations.Key]
        public int ChannelAlertID { get; set; }
        public string ReqReference { get; set; }
        public string MessageDesc { get; set; }
        public int TransactionSeq { get; set; }
        public int SourceID { get; set; }
        public int ModeTypeID { get; set; }
        public int DestID { get; set; }
        public int DistributionTypeID { get; set; }
        public int FreqID { get; set; }
        public string IsManual { get; set; }
        public string HighLevelTransaction { get; set; }

        public int ChannelAlertAttrMapID { get; set; }
        public int daID { get; set; }
        public string daName { get; set; }
        public bool IsLinked { get; set; }

        public int AttrID1 { get; set; }
        public int AttrID2 { get; set; }
        public int AttrID3 { get; set; }
        public int AttrID4 { get; set; }
        public int AttrID5 { get; set; }
        public int AttrID6 { get; set; }
        public int AttrID7 { get; set; }
        public int AttrID8 { get; set; }
        public int AttrID9 { get; set; }
        public int AttrID10 { get; set; }

        public int AttrValueID1 { get; set; }
        public int AttrValueID2 { get; set; }
        public int AttrValueID3 { get; set; }
        public int AttrValueID4 { get; set; }
        public int AttrValueID5 { get; set; }
        public int AttrValueID6 { get; set; }
        public int AttrValueID7 { get; set; }
        public int AttrValueID8 { get; set; }
        public int AttrValueID9 { get; set; }
        public int AttrValueID10 { get; set; }

        public IList<tbl_Source> lstSource { get; set; }
        public IList<tbl_Destination> lstDestination { get; set; }
        public IList<tbl_ModeType> lstModeType { get; set; }
        public IList<tbl_DistributionType> lstDistribution { get; set; }
        public IList<tbl_FrequencyType> lstFrequency { get; set; }

        public IList<tbl_Transactions> lstTransactions { get; set; }
        public IList<ChannelsAndAlertsViewModel> lstchannelsalerts { get; set; }
        public IList<tbl_Attribute> lstAttributes { get; set; }
        public IList<tbl_AttributeValues> lstAttributeValues { get; set; }

        //Flow Implementation
        public int ClientID { get; set; }
        public string ClientName { get; set; }
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int ApplicationID { get; set; }
        public string ApplicationName { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public int ProductId { get; set; }

        #endregion

        #region GetChannelsAlerts

        public IList<ChannelsAndAlertsViewModel> GetChannelsAlerts(int daId)
        {
            try
            {
                ChannelsAndAlertsManager channelalertsManager = new ChannelsAndAlertsManager();
                var channelalerts = channelalertsManager.GetAllChannelAndAlerts(daId);

                IList<ChannelsAndAlertsViewModel> channelsalertsList = new List<ChannelsAndAlertsViewModel>();
                foreach (var item in channelalerts)
                {
                    ChannelsAndAlertsViewModel channelsalertsItem = new ChannelsAndAlertsViewModel();
                    channelsalertsItem.ChannelAlertID = Convert.ToInt32(item.ChannelAlertID);
                    channelsalertsItem.ReqReference = (item.ReqReference == null ? "" : item.ReqReference);
                    channelsalertsItem.MessageDesc = (item.MessageDesc == null ? "" : item.MessageDesc);
                    channelsalertsItem.TransactionSeq = Convert.ToInt32(item.TransactionSeq);
                    channelsalertsItem.SourceID = Convert.ToInt32(item.SourceID);
                    channelsalertsItem.ModeTypeID = Convert.ToInt32(item.ModeTypeID);
                    channelsalertsItem.DestID = Convert.ToInt32(item.DestnID);
                    channelsalertsItem.DistributionTypeID = Convert.ToInt32(item.DistributionTypeID);
                    channelsalertsItem.FreqID = Convert.ToInt32(item.FreqID);
                    channelsalertsItem.IsManual = item.IsManual;
                    channelsalertsItem.ChannelAlertAttrMapID = Convert.ToInt32(item.ChannelAlertAttrMapID);

                    channelsalertsItem.HighLevelTransaction = item.HIGHLEVELTXN;

                    channelsalertsItem.AttrID1 = Convert.ToInt32(item.AttrID1);
                    channelsalertsItem.AttrID2 = Convert.ToInt32(item.AttrID2);
                    channelsalertsItem.AttrID3 = Convert.ToInt32(item.AttrID3);
                    channelsalertsItem.AttrID4 = Convert.ToInt32(item.AttrID4);
                    channelsalertsItem.AttrID5 = Convert.ToInt32(item.AttrID5);
                    channelsalertsItem.AttrID6 = Convert.ToInt32(item.AttrID6);
                    channelsalertsItem.AttrID7 = Convert.ToInt32(item.AttrID7);
                    channelsalertsItem.AttrID8 = Convert.ToInt32(item.AttrID8);
                    channelsalertsItem.AttrID9 = Convert.ToInt32(item.AttrID9);
                    channelsalertsItem.AttrID10 = Convert.ToInt32(item.AttrID10);

                    channelsalertsItem.AttrValueID1 = Convert.ToInt32(item.AttrValueID1);
                    channelsalertsItem.AttrValueID2 = Convert.ToInt32(item.AttrValueID2);
                    channelsalertsItem.AttrValueID3 = Convert.ToInt32(item.AttrValueID3);
                    channelsalertsItem.AttrValueID4 = Convert.ToInt32(item.AttrValueID4);
                    channelsalertsItem.AttrValueID5 = Convert.ToInt32(item.AttrValueID5);
                    channelsalertsItem.AttrValueID6 = Convert.ToInt32(item.AttrValueID6);
                    channelsalertsItem.AttrValueID7 = Convert.ToInt32(item.AttrValueID7);
                    channelsalertsItem.AttrValueID8 = Convert.ToInt32(item.AttrValueID8);
                    channelsalertsItem.AttrValueID9 = Convert.ToInt32(item.AttrValueID9);
                    channelsalertsItem.AttrValueID10 = Convert.ToInt32(item.AttrValueID10);

                    channelsalertsList.Add(channelsalertsItem);
                }

                return channelsalertsList;
            }
            catch (Exception)
            {

                throw;
            }

        }

        #endregion

        public void GetSource(int? daId)
        {
            try
            {
                IList<tbl_Source> lstSources = new List<tbl_Source>();
                InterfaceManager interfaceManager = new InterfaceManager();

                lstSources = interfaceManager.GetSources(daId);
                lstSource = lstSources;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void GetDestination(int? daId)
        {
            try
            {
                IList<tbl_Destination> lstDest = new List<tbl_Destination>();
                InterfaceManager interfaceManager = new InterfaceManager();

                lstDest = interfaceManager.GetDestination(daId);
                lstDestination = lstDest;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void GetModeType(int? daId)
        {
            try
            {
                IList<tbl_ModeType> lstMode = new List<tbl_ModeType>();
                InterfaceManager interfaceManager = new InterfaceManager();

                lstMode = interfaceManager.GetMode(daId);
                lstModeType = lstMode;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void GetDistribution(int? daId)
        {
            try
            {
                IList<tbl_DistributionType> lstDistributions = new List<tbl_DistributionType>();
                ChannelsAndAlertsManager channelandalertsManager = new ChannelsAndAlertsManager();

                lstDistributions = channelandalertsManager.GetAllDistributionTypes(daId);
                lstDistribution = lstDistributions;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void GetFrequency(int? daId)
        {
            try
            {
                IList<tbl_FrequencyType> lstFrequencyTypes = new List<tbl_FrequencyType>();
                ChannelsAndAlertsManager channelandalertsManager = new ChannelsAndAlertsManager();

                lstFrequencyTypes = channelandalertsManager.GetAllFrequencyTypes(daId);
                lstFrequency = lstFrequencyTypes;
            }
            catch (Exception)
            {

                throw;
            }

        }

        #region GetAllAttributes

        public void GetAllAttributes(int? designAccelaratorID)
        {
            try
            {
                IList<tbl_Attribute> lstAttr = new List<tbl_Attribute>();
                BusinessRulesManager buzRuleManager = new BusinessRulesManager();

                lstAttr = buzRuleManager.GetAllAttributes(designAccelaratorID);
                lstAttributes = lstAttr;
            }
            catch (Exception)
            {

                throw;
            }

        }

        #endregion

        #region GetAllTransactions

        public void GetAllTransactions(int? designAccelaratorID)
        {
            try
            {
                IList<tbl_Transactions> lstTrans = new List<tbl_Transactions>();
                BusinessRulesManager buzRuleManager = new BusinessRulesManager();

                lstTrans = buzRuleManager.GetAllTransactions(designAccelaratorID);
                lstTransactions = lstTrans;
            }
            catch (Exception)
            {

                throw;
            }

        }

        #endregion

        #region GetAllAttributeValues

        public void GetAllAttributeValues(int? attributeID)
        {
            try
            {
                IList<tbl_AttributeValues> lstAttrVal = new List<tbl_AttributeValues>();
                BusinessRulesManager buzRuleManager = new BusinessRulesManager();

                lstAttrVal = buzRuleManager.GetAllAttributeValues(attributeID);
                lstAttributeValues = lstAttrVal;
            }
            catch (Exception)
            {

                throw;
            }

        }

        #endregion

        #region SaveChannelAlertsData
        public int SaveChannelAlertsData(IList<ChannelsAndAlertsViewModel> channelalerts, int daId)
        {
            try
            {
                int result = 0;
                ChannelsAndAlertsManager channelalertsManager = new ChannelsAndAlertsManager();
                List<tbl_ChannelAlert> lstChannelAlerts = new List<tbl_ChannelAlert>();
                List<tbl_ChannelAlertAttrMapping> lstChannelAlertsAttrMapping = new List<tbl_ChannelAlertAttrMapping>();

                var AllChannelAlerts = channelalerts.GroupBy(b => b.ChannelAlertAttrMapID);

                foreach (var chn in AllChannelAlerts)
                {
                    tbl_ChannelAlertAttrMapping tblchannelalertsattrmapping = new tbl_ChannelAlertAttrMapping();

                    if ((chn.Key.ToString().Length > 5) && (chn.Key.ToString().Substring(chn.Key.ToString().Length - 2, 2) == "00"))
                    {
                        #region Add New Channel and Alert

                        int cnt = 1;
                        var cur = channelalerts.Where(e => e.ChannelAlertAttrMapID == chn.Key && e.IsLinked.Equals(true)); //Querying checked highlevel transactions and BuzruleAttrMapID marked new
                        foreach (var HighTrans in cur)
                        {
                            if (cnt == 1)
                            {
                                AttributesMapping(ref tblchannelalertsattrmapping, HighTrans);
                                tblchannelalertsattrmapping.EntityState = DA.DomainModel.EntityState.Added;
                            }
                            cnt++;

                            tbl_ChannelAlert channelalert = new tbl_ChannelAlert();
                            channelalert.daId = daId;
                            channelalert.TransactionSeq = HighTrans.TransactionSeq;

                            channelalert.tbl_ChannelAlertAttrMapping = tblchannelalertsattrmapping;
                            channelalert.EntityState = DA.DomainModel.EntityState.Added;
                            tblchannelalertsattrmapping.tbl_ChannelAlert.Add(channelalert);
                        }
                        lstChannelAlertsAttrMapping.Add(tblchannelalertsattrmapping);

                        #endregion
                    }
                    else
                    {
                        #region AddModifyDelete Business Rules

                        int cnt1 = 1;
                        var cur = channelalerts.Where(e => e.ChannelAlertAttrMapID == chn.Key && (e.IsLinked.Equals(true) || e.ChannelAlertID != 0)); //Querying checked highlevel transactions and BuzruleID existed
                        foreach (var HighTrans in cur)
                        {
                            if (cnt1 == 1)
                            {
                                tblchannelalertsattrmapping.ChannelAlertAttrMapID = HighTrans.ChannelAlertAttrMapID;
                                AttributesMapping(ref tblchannelalertsattrmapping, HighTrans);
                                tblchannelalertsattrmapping.EntityState = DA.DomainModel.EntityState.Modified;
                            }
                            cnt1++;
                            tbl_ChannelAlert channelalert = new tbl_ChannelAlert();
                            channelalert.daId = daId;
                            channelalert.TransactionSeq = HighTrans.TransactionSeq;
                            channelalert.ChannelAlertAttrMapID = HighTrans.ChannelAlertAttrMapID;
                            channelalert.ChannelAlertID = HighTrans.ChannelAlertID;

                            if ((HighTrans.ChannelAlertID != 0) && (HighTrans.IsLinked == true))
                            {
                                channelalert.EntityState = DA.DomainModel.EntityState.Unchanged;
                            }
                            else if ((HighTrans.ChannelAlertID == 0) && (HighTrans.IsLinked == true))
                            {
                                channelalert.EntityState = DA.DomainModel.EntityState.Added;
                            }
                            else if ((HighTrans.ChannelAlertID != 0) && (HighTrans.IsLinked == false))
                            {
                                channelalert.EntityState = DA.DomainModel.EntityState.Deleted;
                            }

                            tblchannelalertsattrmapping.tbl_ChannelAlert.Add(channelalert);
                        }
                        lstChannelAlertsAttrMapping.Add(tblchannelalertsattrmapping);

                        #endregion
                    }
                }

                result = channelalertsManager.SaveChannelAlertDataMapping(lstChannelAlertsAttrMapping);
                return result;
            }
            catch (Exception)
            {
                throw;
            }

        }

        #endregion

        #region AttributesMapping

        private void AttributesMapping(ref tbl_ChannelAlertAttrMapping tblchannelalertattrmapping, ChannelsAndAlertsViewModel HighTrans)
        {
            try
            {
                tblchannelalertattrmapping.ReqReference = HighTrans.ReqReference;
                tblchannelalertattrmapping.MessageDesc = HighTrans.MessageDesc;
                tblchannelalertattrmapping.SourceID = HighTrans.SourceID;
                tblchannelalertattrmapping.AttrID1 = HighTrans.AttrID1;
                tblchannelalertattrmapping.AttrValueID1 = HighTrans.AttrValueID1;
                tblchannelalertattrmapping.AttrID2 = HighTrans.AttrID2;
                tblchannelalertattrmapping.AttrValueID2 = HighTrans.AttrValueID2;
                tblchannelalertattrmapping.AttrID3 = HighTrans.AttrID3;
                tblchannelalertattrmapping.AttrValueID3 = HighTrans.AttrValueID3;
                tblchannelalertattrmapping.AttrID4 = HighTrans.AttrID4;
                tblchannelalertattrmapping.AttrValueID4 = HighTrans.AttrValueID4;
                tblchannelalertattrmapping.AttrID5 = HighTrans.AttrID5;
                tblchannelalertattrmapping.AttrValueID5 = HighTrans.AttrValueID5;
                tblchannelalertattrmapping.AttrID6 = HighTrans.AttrID6;
                tblchannelalertattrmapping.AttrValueID6 = HighTrans.AttrValueID6;
                tblchannelalertattrmapping.AttrID7 = HighTrans.AttrID7;
                tblchannelalertattrmapping.AttrValueID7 = HighTrans.AttrValueID7;
                tblchannelalertattrmapping.AttrID8 = HighTrans.AttrID8;
                tblchannelalertattrmapping.AttrValueID8 = HighTrans.AttrValueID8;
                tblchannelalertattrmapping.AttrID9 = HighTrans.AttrID9;
                tblchannelalertattrmapping.AttrValueID9 = HighTrans.AttrValueID9;
                tblchannelalertattrmapping.AttrID10 = HighTrans.AttrID10;
                tblchannelalertattrmapping.AttrValueID10 = HighTrans.AttrValueID10;
                tblchannelalertattrmapping.IsManual = HighTrans.IsManual;
                tblchannelalertattrmapping.ModeTypeID = HighTrans.ModeTypeID;
                tblchannelalertattrmapping.DestnID = HighTrans.DestID;
                tblchannelalertattrmapping.DistributionTypeID = HighTrans.DistributionTypeID;
                tblchannelalertattrmapping.FreqID = HighTrans.FreqID;
            }
            catch (Exception)
            {

                throw;
            }

        }

        #endregion

        public void DeleteChannelAttrMapId(int postData)
        {
            try
            {
                ChannelsAndAlertsManager channelAlertManager = new ChannelsAndAlertsManager();
                channelAlertManager.DeleteChannelAttrMapId(postData);
            }
            catch (Exception)
            {

                throw;
            }


        }
    }
}