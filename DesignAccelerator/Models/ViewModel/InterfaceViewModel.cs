using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DA.BusinessLayer;
using DA.DomainModel;

namespace DesignAccelerator.Models.ViewModel
{
    public class InterfaceViewModel
    {
        #region Public Properties
        public int interfaceId { get; set; }
        public int transactionSeq { get; set; }
        public int daId { get; set; }
        public int interfaceAttrMapId { get; set; }
        public string reqReference { get; set; }
        public string interfaceDesc { get; set; }
        public int sourceId { get; set; }
        //public string source { get; set; }
        public int destinationId { get; set; }
        public int modeTypeId { get; set; }
        public string highLevelTransaction { get; set; }

        public int attrID1 { get; set; }
        public int attrValueID1 { get; set; }
        public int attrID2 { get; set; }
        public int attrValueID2 { get; set; }
        public int attrID3 { get; set; }
        public int attrValueID3 { get; set; }
        public int attrID4 { get; set; }
        public int attrValueID4 { get; set; }
        public int attrID5 { get; set; }
        public int attrValueID5 { get; set; }
        public int attrID6 { get; set; }
        public int attrValueID6 { get; set; }
        public int attrID7 { get; set; }
        public int attrValueID7 { get; set; }
        public int attrID8 { get; set; }
        public int attrValueID8 { get; set; }
        public int attrID9 { get; set; }
        public int attrValueID9 { get; set; }
        public int attrID10 { get; set; }
        public int attrValueID10 { get; set; }

        public bool IsLinked { get; set; }

        public IList<tbl_Transactions> lstHighLevelTransaction { get; set; }
        public IList<InterfaceViewModel> lstinterfaceData { get; set; }

        public IList<tbl_Source> lstSource { get; set; }
        public IList<tbl_Destination> lstDestination { get; set; }
        public IList<tbl_ModeType> lstModeType { get; set; }
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
        public string daName { get; set; }
        #endregion

        public IList<InterfaceViewModel> GetInterfaceData(int daId)
        {
            try
            {
                InterfaceManager interfaceManager = new InterfaceManager();
                var interfaceData = interfaceManager.GetInterfaceData(daId);

                IList<InterfaceViewModel> interfaceViewModelList = new List<InterfaceViewModel>();
                foreach (var item in interfaceData)
                {
                    InterfaceViewModel interfaceViewModelItem = new InterfaceViewModel();
                    interfaceViewModelItem.interfaceId = Convert.ToInt32(item.InterfaceID);
                    interfaceViewModelItem.interfaceAttrMapId = Convert.ToInt32(item.InterfaceAttrMapID);
                    interfaceViewModelItem.reqReference = (item.ReqReference == null ? "" : item.ReqReference);
                    interfaceViewModelItem.interfaceDesc = item.InterfaceDesc;
                    interfaceViewModelItem.sourceId = item.SourceID;

                    interfaceViewModelItem.destinationId = item.DestinationID;
                    interfaceViewModelItem.modeTypeId = item.ModeTypeID;
                    interfaceViewModelItem.transactionSeq = Convert.ToInt32(item.TransactionSeq);
                    interfaceViewModelItem.highLevelTransaction = item.HIGHLEVELTXN;
                    interfaceViewModelItem.attrID1 = Convert.ToInt32(item.AttrID1);
                    interfaceViewModelItem.attrValueID1 = Convert.ToInt32(item.AttrValueID1);
                    interfaceViewModelItem.attrID2 = Convert.ToInt32(item.AttrID2);
                    interfaceViewModelItem.attrValueID2 = Convert.ToInt32(item.AttrValueID2);
                    interfaceViewModelItem.attrID3 = Convert.ToInt32(item.AttrID3);
                    interfaceViewModelItem.attrValueID3 = Convert.ToInt32(item.AttrValueID3);
                    interfaceViewModelItem.attrID4 = Convert.ToInt32(item.AttrID4);
                    interfaceViewModelItem.attrValueID4 = Convert.ToInt32(item.AttrValueID4);
                    interfaceViewModelItem.attrID5 = Convert.ToInt32(item.AttrID5);
                    interfaceViewModelItem.attrValueID5 = Convert.ToInt32(item.AttrValueID5);
                    interfaceViewModelItem.attrID6 = Convert.ToInt32(item.AttrID6);
                    interfaceViewModelItem.attrValueID6 = Convert.ToInt32(item.AttrValueID6);
                    interfaceViewModelItem.attrID7 = Convert.ToInt32(item.AttrID7);
                    interfaceViewModelItem.attrValueID7 = Convert.ToInt32(item.AttrValueID7);
                    interfaceViewModelItem.attrID8 = Convert.ToInt32(item.AttrID8);
                    interfaceViewModelItem.attrValueID8 = Convert.ToInt32(item.AttrValueID8);
                    interfaceViewModelItem.attrID9 = Convert.ToInt32(item.AttrID9);
                    interfaceViewModelItem.attrValueID9 = Convert.ToInt32(item.AttrValueID9);
                    interfaceViewModelItem.attrID10 = Convert.ToInt32(item.AttrID10);
                    interfaceViewModelItem.attrValueID10 = Convert.ToInt32(item.AttrValueID10);

                    interfaceViewModelList.Add(interfaceViewModelItem);
                }
                //lstinterfaceData = interfaceViewModelList;
                return interfaceViewModelList;
            }
            catch (Exception)
            {

                throw;
            }

        }

        //public void GetAllHighLevelTransactions(int daId)
        //{
        //    InterfaceManager interfaceManager = new InterfaceManager();
        //    var lstHighLevelTrans = interfaceManager.GetTransactionHeader(daId);
        //    IList<string> lsthighlvltrns = new List<string>();
        //    foreach (var item in lstHighLevelTrans)
        //    {
        //        lsthighlvltrns.Add(item.ToString());
        //        //lsthighlvltrns.Add(e => item.HIGHLEVELTXN.Equals(item.TRANSACTIONSEQ));
        //    }

        //    lstHighLevelTransaction = lsthighlvltrns;
        //}

        public void GetAllHighLevelTransactions(int daId)
        {
            try
            {
                IList<tbl_Transactions> lstHighLevelTrans = new List<tbl_Transactions>();
                InterfaceManager interfaceManager = new InterfaceManager();

                lstHighLevelTrans = interfaceManager.GetTransactionHeader(daId);
                lstHighLevelTransaction = lstHighLevelTrans;
            }
            catch (Exception)
            {

                throw;
            }
        }

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

        public void GetAllAttributes(int? daId)
        {
            try
            {
                IList<tbl_Attribute> lstAttr = new List<tbl_Attribute>();
                InterfaceManager interfaceManager = new InterfaceManager();

                lstAttr = interfaceManager.GetAllAttributes(daId);
                lstAttributes = lstAttr;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void GetAllAttributeValues(int? attributeId)
        {
            try
            {
                IList<tbl_AttributeValues> lstAttrVal = new List<tbl_AttributeValues>();
                InterfaceManager interfaceManager = new InterfaceManager();

                lstAttrVal = interfaceManager.GetAttributeValues(attributeId);
                lstAttributeValues = lstAttrVal;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int SaveData(IList<InterfaceViewModel> interfaceViewModel, int daId)
        {
            try
            {
                int result = 0;
                InterfaceManager interfaceManager = new InterfaceManager();
                List<tbl_InterfaceAttrMapping> lstInterfaceAttrMapping = new List<tbl_InterfaceAttrMapping>();

                var allInterfaces = interfaceViewModel.GroupBy(i => i.interfaceAttrMapId);

                foreach (var intfs in allInterfaces)
                {
                    tbl_InterfaceAttrMapping tblInterfaceAttrMapping = new tbl_InterfaceAttrMapping();

                    if (intfs.Key.ToString().Length > 5 && intfs.Key.ToString().Substring(intfs.Key.ToString().Length - 2, 2).Equals("00"))
                    {
                        #region Add new Interfaces

                        var counter = 1;
                        var newInterface = interfaceViewModel.Where(e => e.interfaceAttrMapId == intfs.Key && e.IsLinked.Equals(true));

                        foreach (var item in newInterface)
                        {
                            if (counter == 1)
                            {
                                tblInterfaceAttrMapping.ReqReference = (item.reqReference == null ? "" : item.reqReference);
                                tblInterfaceAttrMapping.InterfaceDesc = item.interfaceDesc;
                                tblInterfaceAttrMapping.SourceId = Convert.ToInt32(item.sourceId);
                                tblInterfaceAttrMapping.DestinationId = Convert.ToInt32(item.destinationId);
                                tblInterfaceAttrMapping.ModeTypeId = Convert.ToInt32(item.modeTypeId);
                                tblInterfaceAttrMapping.AttrID1 = Convert.ToInt32(item.attrID1);
                                tblInterfaceAttrMapping.AttrValueID1 = Convert.ToInt32(item.attrValueID1);
                                tblInterfaceAttrMapping.AttrID2 = item.attrID2;
                                tblInterfaceAttrMapping.AttrValueID2 = item.attrValueID2;
                                tblInterfaceAttrMapping.AttrID3 = item.attrID3;
                                tblInterfaceAttrMapping.AttrValueID3 = item.attrValueID3;
                                tblInterfaceAttrMapping.AttrID4 = item.attrID4;
                                tblInterfaceAttrMapping.AttrValueID4 = item.attrValueID4;
                                tblInterfaceAttrMapping.AttrID5 = item.attrID5;
                                tblInterfaceAttrMapping.AttrValueID5 = item.attrValueID5;
                                tblInterfaceAttrMapping.AttrID6 = item.attrID6;
                                tblInterfaceAttrMapping.AttrValueID6 = item.attrValueID6;
                                tblInterfaceAttrMapping.AttrID7 = item.attrID7;
                                tblInterfaceAttrMapping.AttrValueID7 = item.attrValueID7;
                                tblInterfaceAttrMapping.AttrID8 = item.attrID8;
                                tblInterfaceAttrMapping.AttrValueID8 = item.attrValueID8;
                                tblInterfaceAttrMapping.AttrID9 = item.attrID9;
                                tblInterfaceAttrMapping.AttrValueID9 = item.attrValueID9;
                                tblInterfaceAttrMapping.AttrID10 = item.attrID10;
                                tblInterfaceAttrMapping.AttrValueID10 = item.attrValueID10;

                                tblInterfaceAttrMapping.EntityState = DA.DomainModel.EntityState.Added;
                            }
                            counter++;

                            tbl_Interface tblInterface = new tbl_Interface();
                            tblInterface.daId = daId;
                            tblInterface.TransactionSeq = item.transactionSeq;

                            //to enter the FK interfaceMappingID
                            tblInterface.tbl_InterfaceAttrMapping = tblInterfaceAttrMapping;
                            tblInterface.EntityState = DA.DomainModel.EntityState.Added;

                            tblInterfaceAttrMapping.tbl_Interface.Add(tblInterface);
                        }
                        lstInterfaceAttrMapping.Add(tblInterfaceAttrMapping);
                        #endregion
                    }
                    else
                    {
                        #region Add,Modify,Delete Interfaces

                        var counter = 1;
                        var existingInterface = interfaceViewModel.Where(e => e.interfaceAttrMapId == intfs.Key && (e.IsLinked.Equals(true) || e.interfaceId != 0));

                        foreach (var item in existingInterface)
                        {
                            if (counter == 1)
                            {
                                tblInterfaceAttrMapping.InterfaceAttrMapID = Convert.ToInt32(item.interfaceAttrMapId);
                                tblInterfaceAttrMapping.ReqReference = (item.reqReference == null ? "" : item.reqReference);
                                tblInterfaceAttrMapping.InterfaceDesc = item.interfaceDesc;
                                tblInterfaceAttrMapping.SourceId = Convert.ToInt32(item.sourceId);
                                tblInterfaceAttrMapping.DestinationId = Convert.ToInt32(item.destinationId);
                                tblInterfaceAttrMapping.ModeTypeId = Convert.ToInt32(item.modeTypeId);
                                tblInterfaceAttrMapping.AttrID1 = Convert.ToInt32(item.attrID1);
                                tblInterfaceAttrMapping.AttrValueID1 = Convert.ToInt32(item.attrValueID1);
                                tblInterfaceAttrMapping.AttrID2 = item.attrID2;
                                tblInterfaceAttrMapping.AttrValueID2 = item.attrValueID2;
                                tblInterfaceAttrMapping.AttrID3 = item.attrID3;
                                tblInterfaceAttrMapping.AttrValueID3 = item.attrValueID3;
                                tblInterfaceAttrMapping.AttrID4 = item.attrID4;
                                tblInterfaceAttrMapping.AttrValueID4 = item.attrValueID4;
                                tblInterfaceAttrMapping.AttrID5 = item.attrID5;
                                tblInterfaceAttrMapping.AttrValueID5 = item.attrValueID5;
                                tblInterfaceAttrMapping.AttrID6 = item.attrID6;
                                tblInterfaceAttrMapping.AttrValueID6 = item.attrValueID6;
                                tblInterfaceAttrMapping.AttrID7 = item.attrID7;
                                tblInterfaceAttrMapping.AttrValueID7 = item.attrValueID7;
                                tblInterfaceAttrMapping.AttrID8 = item.attrID8;
                                tblInterfaceAttrMapping.AttrValueID8 = item.attrValueID8;
                                tblInterfaceAttrMapping.AttrID9 = item.attrID9;
                                tblInterfaceAttrMapping.AttrValueID9 = item.attrValueID9;
                                tblInterfaceAttrMapping.AttrID10 = item.attrID10;
                                tblInterfaceAttrMapping.AttrValueID10 = item.attrValueID10;

                                tblInterfaceAttrMapping.EntityState = DA.DomainModel.EntityState.Modified;
                            }
                            counter++;
                            tbl_Interface tblInterface = new tbl_Interface();

                            tblInterface.daId = daId;
                            tblInterface.InterfaceID = item.interfaceId;
                            tblInterface.InterfaceAttrMapID = Convert.ToInt32(item.interfaceAttrMapId);
                            tblInterface.TransactionSeq = Convert.ToInt32(item.transactionSeq);

                            //To keep record as it is
                            if ((item.interfaceId != 0) && (item.IsLinked == true))
                            {
                                tblInterface.EntityState = DA.DomainModel.EntityState.Unchanged;
                            }
                            //to remove unchecked transaction                       
                            else if ((item.interfaceId != 0) && (item.IsLinked == false))
                            {
                                tblInterface.EntityState = DA.DomainModel.EntityState.Deleted;
                            }
                            //to add newly checked transaction
                            else if ((item.interfaceId == 0) && (item.IsLinked == true))
                            {
                                tblInterface.EntityState = DA.DomainModel.EntityState.Added;
                            }
                            tblInterfaceAttrMapping.tbl_Interface.Add(tblInterface);
                        }
                        lstInterfaceAttrMapping.Add(tblInterfaceAttrMapping);
                        #endregion
                    }
                }
                result = interfaceManager.SaveDataMapping(lstInterfaceAttrMapping);
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void DeleteInterfaceAttr(int postData)
        {
            try
            {
                InterfaceManager objbusinessrulemanager = new InterfaceManager();
                objbusinessrulemanager.DeleteInterfaceAttr(postData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        
    }
}