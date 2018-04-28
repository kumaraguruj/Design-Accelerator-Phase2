using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DA.DomainModel;
using DA.BusinessLayer;

namespace DesignAccelerator.Models.ViewModel
{
    public class MappingViewModel
    {
        #region public properties
        //Col1 - High Level Transaction
        public string TransactionID { get; set; }
        public string TransactionDesc { get; set; }

        //Col2 - Value1 - Value12
        public int AttributeID { get; set; }

        //Attribute Name
        public string AttributeName { get; set; }
        public string Value { get; set; }

        //AttributeType
        public int AttributeTypeID { get; set; }
        public string AttributeTypeDesc { get; set; }

        //List of Attribute Values
        public IList<tbl_AttributeValues> lstAttributeValues { get; set; }

        public IList<tbl_Attribute> lstAttributesMapped { get; set; }

        public IList<tbl_AttributeValues> lstNegAttrValues { get; set; }

        public IList<tbl_Attribute> lstCriticalAttributes { get; set; }

        public IList<tbl_Transactions> lstHighLevelTransactions { get; set; }
        //List of Col2
        public IList<string> lstValues { get; set; }

        public Dictionary<string, IList<tbl_AttributeValues>> dicAttributesanditsValues { get; set; }

        #endregion

        public IList<sp_GetMappingViewModelData_Result> GetMappedData(int daId)
        {
            AttributeValueManager attributeValueManager = new AttributeValueManager();
            TransactionsManager transactionsManager = new TransactionsManager();
            MappingManager mappingManager = new MappingManager();
            dicAttributesanditsValues = new Dictionary<string, IList<tbl_AttributeValues>>();

            List<sp_GetMappingViewModelData_Result> lstMappingViewModel = new List<sp_GetMappingViewModelData_Result>();

            AddValues();
            //var transactionAttributeMapping = mappingManager.GetMappingDetails(daId);

            MappingViewModelManager mappingViewModelManager = new MappingViewModelManager();
            lstMappingViewModel = mappingViewModelManager.GetMappingViewModelData(daId);

            AttributeListManager attributeListManager = new AttributeListManager();

            try
            {
                var Attributes = attributeListManager.GetAttributeList(daId);
                foreach (var attribute in Attributes)
                {
                    IList<tbl_AttributeValues> lstAttValues = new List<tbl_AttributeValues>();
                    lstAttValues = attributeValueManager.GetAttributeValList(attribute.AttributeID);
                    dicAttributesanditsValues.Add(attribute.AttributeDesc, lstAttValues);
                }
            }
            catch (Exception)
            {

                throw;
            }

            return lstMappingViewModel;
        }

        public IList<string> AddValues()
        {
            try
            {
                lstValues = new List<string>();

                for (int i = 1; i <= 15; i++)
                {
                    lstValues.Add("Value" + i);
                }

                return lstValues;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public IList<tbl_AttributeValues> GetNegativeAttributeValues(int daId)
        {
            try
            {
                MappingManager mappingManager = new MappingManager();
                lstNegAttrValues = mappingManager.GetNegativeAttributeValue(daId);
                return lstNegAttrValues;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
