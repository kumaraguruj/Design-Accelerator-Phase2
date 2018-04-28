using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DA.BusinessLayer;
using DA.DomainModel;

namespace DesignAccelerator.Models.ViewModel
{
    public class LinkingUIViewModel
    {
        #region Public Properties
        public int daId { get; set; }
        public string daName { get; set; }

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

        public List<string> lstProcess { get; set; }
        #endregion

        public LinkingUIViewModel GetUIScreenData()
        {
            try
            {
                lstProcess = new List<string>();
                //lstProcess.Add("Rule of 1 - Txn Matrix");
                lstProcess.Add("Rule of N - Txn Matrix");
                lstProcess.Add("Business Rule");
                lstProcess.Add("Interface");
                lstProcess.Add("Channels&Alerts");
                lstProcess.Add("Reports");
                lstProcess.Add("Scenario Builder Template");
                lstProcess.Add("Scenario Stitching");
                lstProcess.Add("Test Design");
                lstProcess.Add("Traceability Matrix");
                lstProcess.Add("Run Plan");
                lstProcess.Add("Export DA");

                LinkingUIViewModel lstLinkVM = new LinkingUIViewModel();
                lstLinkVM.lstProcess = lstProcess;

                return lstLinkVM;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}