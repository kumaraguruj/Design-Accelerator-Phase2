using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.DataAccessLayer;
using DA.DomainModel;
using System.Data.SqlClient;
using System.Data;

namespace DA.BusinessLayer
{
    public class InterfaceManager
    {
        public IList<tbl_Source> GetSources(int? daid)
        {
            try
            {
                IGenericDataRepository<tbl_Source> repository = new GenericDataRepository<tbl_Source>();
                IList<tbl_Source> lstSources = repository.GetList(e => e.daId.Equals(daid));

                return lstSources;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public IList<tbl_Destination> GetDestination(int? daid)
        {
            try
            {
                IGenericDataRepository<tbl_Destination> repository = new GenericDataRepository<tbl_Destination>();
                IList<tbl_Destination> lstDestination = repository.GetList(e => e.daId.Equals(daid));

                return lstDestination;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public IList<tbl_ModeType> GetMode(int? daid)
        {
            try
            {
                IGenericDataRepository<tbl_ModeType> repository = new GenericDataRepository<tbl_ModeType>();
                IList<tbl_ModeType> lstMode = repository.GetList(e => e.daId.Equals(daid));

                return lstMode;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public IList<tbl_Attribute> GetAllAttributes(int? daid)
        {
            try
            {
                IGenericDataRepository<tbl_Attribute> repository = new GenericDataRepository<tbl_Attribute>();
                IList<tbl_Attribute> lstAttributes = repository.GetList(e => e.daId.Equals(daid));

                return lstAttributes;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public IList<tbl_AttributeValues> GetAttributeValues(int? attrId)
        {
            try
            {
                IGenericDataRepository<tbl_AttributeValues> repository = new GenericDataRepository<tbl_AttributeValues>();
                IList<tbl_AttributeValues> lstAttributeValues = repository.GetList(e => e.AttributeID.Equals(attrId));

                return lstAttributeValues;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public IList<sp_GetInterfaceMapping_Result> GetInterfaceData(int daId)
        {
            try
            {
                IGenericDataRepository<sp_GetInterfaceMapping_Result> repository = new GenericDataRepository<sp_GetInterfaceMapping_Result>();
                return repository.ExecuteStoredProcedure("EXEC  sp_GetInterfaceMapping @daId", new SqlParameter("daId", SqlDbType.Int) { Value = daId }).ToList();
            }
            catch (Exception)
            {

                throw;
            }

        }

        //public List<sp_GetTransactionsForHeader_Result> GetTransactionHeader(int daId)
        //{
        //    IGenericDataRepository<sp_GetTransactionsForHeader_Result> repository = new GenericDataRepository<sp_GetTransactionsForHeader_Result>();
        //    return repository.ExecuteStoredProcedure("EXEC sp_GetTransactionsForHeader @daId", new SqlParameter("daId", SqlDbType.Int) { Value = daId }).ToList();
        //}

        public IList<tbl_Transactions> GetTransactionHeader(int daId)
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

        public void AddInterfaceAttrMapping(tbl_InterfaceAttrMapping tblInterfaceAttrMapping)
        {
            try
            {
                IGenericDataRepository<tbl_InterfaceAttrMapping> repository = new GenericDataRepository<tbl_InterfaceAttrMapping>();
                repository.Add(tblInterfaceAttrMapping);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void AddInterface(tbl_Interface tblInterface)
        {
            try
            {
                IGenericDataRepository<tbl_Interface> repository = new GenericDataRepository<tbl_Interface>();
                repository.Add(tblInterface);
            }
            catch (Exception)
            {

                throw;
            }

        }

        //Save changes

        public int SaveDataMapping(IList<tbl_InterfaceAttrMapping> tblInterfaceAttributeMapping)
        {
            try
            {
                IGenericDataRepository<tbl_InterfaceAttrMapping> repository = new GenericDataRepository<tbl_InterfaceAttrMapping>();
                foreach (var item in tblInterfaceAttributeMapping)
                    repository.Add(item);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }

        }
        //public int SaveDataInterface(IList<tbl_Interface> tblInterface)
        //{
        //    try
        //    {
        //        IGenericDataRepository<tbl_Interface> repository = new GenericDataRepository<tbl_Interface>();
        //        foreach (var item in tblInterface)
        //            repository.Add(item);
        //        return 1;
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }

        //}


        public void DeleteInterfaceAttr(int id)
        {
            try
            {
                tbl_Interface objtblinterface = new tbl_Interface();
                tbl_InterfaceAttrMapping objinterfaceMapping = new tbl_InterfaceAttrMapping();
                IGenericDataRepository<tbl_Interface> repository = new GenericDataRepository<tbl_Interface>();
                IList<tbl_Interface> lstInterfaceid = repository.GetList(q => q.InterfaceID.Equals(id));
                IGenericDataRepository<tbl_InterfaceAttrMapping> repository1 = new GenericDataRepository<tbl_InterfaceAttrMapping>();

                if (lstInterfaceid != null)
                {
                    foreach (var item in lstInterfaceid)
                    {
                        objtblinterface.daId = item.daId;
                        objtblinterface.InterfaceAttrMapID = item.InterfaceAttrMapID;
                        objtblinterface.InterfaceID = item.InterfaceID;
                        objtblinterface.TransactionSeq = item.TransactionSeq;
                        objtblinterface.EntityState = EntityState.Deleted;

                        IList<tbl_InterfaceAttrMapping> lstInterfaceMapId = repository1.GetList(q => q.InterfaceAttrMapID.Equals(item.InterfaceAttrMapID));

                        if (lstInterfaceMapId != null)
                        {
                            foreach (var item1 in lstInterfaceMapId)
                            {
                                objinterfaceMapping.AttrID1 = item1.AttrID1;
                                objinterfaceMapping.AttrID2 = item1.AttrID2;
                                objinterfaceMapping.AttrID3 = item1.AttrID3;
                                objinterfaceMapping.AttrID4 = item1.AttrID4;
                                objinterfaceMapping.AttrID5 = item1.AttrID5;
                                objinterfaceMapping.AttrID6 = item1.AttrID6;
                                objinterfaceMapping.AttrID7 = item1.AttrID7;
                                objinterfaceMapping.AttrID8 = item1.AttrID8;
                                objinterfaceMapping.AttrID9 = item1.AttrID9;
                                objinterfaceMapping.AttrID10 = item1.AttrID10;
                                objinterfaceMapping.AttrValueID1 = item1.AttrValueID1;
                                objinterfaceMapping.AttrValueID2 = item1.AttrValueID2;
                                objinterfaceMapping.AttrValueID3 = item1.AttrValueID3;
                                objinterfaceMapping.AttrValueID4 = item1.AttrValueID4;
                                objinterfaceMapping.AttrValueID5 = item1.AttrValueID5;
                                objinterfaceMapping.AttrValueID6 = item1.AttrValueID6;
                                objinterfaceMapping.AttrValueID7 = item1.AttrValueID7;
                                objinterfaceMapping.AttrValueID8 = item1.AttrValueID8;
                                objinterfaceMapping.AttrValueID9 = item1.AttrValueID9;
                                objinterfaceMapping.AttrValueID10 = item1.AttrValueID10;
                                objinterfaceMapping.InterfaceAttrMapID = item1.InterfaceAttrMapID;
                                objinterfaceMapping.InterfaceDesc = item1.InterfaceDesc;
                                objinterfaceMapping.ModeTypeId = item1.ModeTypeId;
                                objinterfaceMapping.ReqReference = item1.ReqReference;
                                objinterfaceMapping.SourceId = item1.SourceId;
                                objinterfaceMapping.DestinationId = item1.DestinationId;

                                objinterfaceMapping.EntityState = EntityState.Deleted;

                                repository1.Remove(objinterfaceMapping);
                            }
                        }

                        repository.Remove(objtblinterface);
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
