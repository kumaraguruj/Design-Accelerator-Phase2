using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DA.BusinessLayer;
using DA.DomainModel;
using DesignAccelerator.Controllers;
using System.ComponentModel.DataAnnotations;

namespace DesignAccelerator.Models.ViewModel
{
    public class MISReports
    {
        #region Public properties
        
        public int UserId { get; set; }
        public string UserName { get; set; }

        public int CreatorId { get; set; }
        public string CreatorName { get; set; }

        public int AuthId { get; set; }
        public string AuthName { get; set; }

        public DateTime CreateDate { get; set; }
        public bool Status { get; set; }
        public string Fromdate { get; set; }
        public string ToDate { get; set; }

        public string actionName { get; set; }

        public int ActionID { get; set; }
        public string MisReports { get; set; }

        public int ClientId { get; set; }
        public string ClientName { get; set;}

        public int projectId { get; set; }
        public string projectName { get; set;}

        public int ApplicationId { get; set; }
        public string ApplicationName { get; set; }

        public string appVersion { get; set; }

        public string BankTypeName { get; set; }

        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public List<SelectListItem> RegionList { get; set; }

        public enum BankTypes
        {
            Conventional = 1,
            Islamic = 2
        }

        public bool UserManagement
        {
            get;set;
          
        }
        public bool ClientInformation { get; set; }
                

        //public List<MisReports> UserTypes { get; set; }
        public IList<KeyValuePair<string, int>> BankTypeList { get; set; }
        public IList<tbl_Region> lstRegion { get; set; }
        public IList<tbl_Applications> lstApplication { get; set; }
        public IList<tbl_Clients> lstClients { get; set; }
        public IList<tbl_Projects> lstProjects { get; set; }
        public IList<tbl_AppVersion> lstAppVersion { get; set; }

        public IList<tbl_UserData> lstUserData { get; set; }
        public IList<tbl_UserActionArchives> lstUserActions { get; set; }
        public IList<tbl_Actions> lstActions { get; set; }
        
        #endregion

        public void GetUserDetails()
        {
            try
            {
                MISReportsManager misManager = new MISReportsManager();

                lstUserData = new List<tbl_UserData>();
                
                lstUserData = misManager.GetUserDetails();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void GetUserActions()
        {
            try
            {
                MISReportsManager misManager = new MISReportsManager();

                lstUserActions = new List<tbl_UserActionArchives>();

                lstUserActions = misManager.GetUserActions();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void GetActions()
        {
            try
            {
                MISReportsManager misManager = new MISReportsManager();

                lstActions = new List<tbl_Actions>();

                lstActions = misManager.GetActions();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void GetAllDetails()
        {
            try
            {
                MISReportsManager misReportsManager = new MISReportsManager();

                lstClients = new List<tbl_Clients>();
                lstProjects = new List<tbl_Projects>();
                lstApplication = new List<tbl_Applications>();
                lstAppVersion = new List<tbl_AppVersion>();
                lstRegion = new List<tbl_Region>();

                lstClients = misReportsManager.GetAllClientDetailsforMIS();
                lstProjects = misReportsManager.GetAllProjectDetailsforMIS();
                lstApplication = misReportsManager.GetAllApplicationDetailsforMIS();
                lstAppVersion = misReportsManager.GetAllAppVersionDetailsforMIS();
                lstRegion = misReportsManager.GetAllRegionDetailsforMIS();

                BankTypeList = new List<KeyValuePair<string, int>>();

                BankTypeList = GetEnumList<BankTypes>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<KeyValuePair<string, int>> GetEnumList<T>()
        {
            try
            {
                var list = new List<KeyValuePair<string, int>>();
                foreach (var e in Enum.GetValues(typeof(T)))
                {
                    list.Add(new KeyValuePair<string, int>(e.ToString(), (int)e));
                }
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void GetApplicationRegionDetails()//int projectId
        {
            try
            {
                MISReportsManager misReportsManager = new MISReportsManager();
                lstRegion = new List<tbl_Region>();
                lstApplication = new List<tbl_Applications>();
                lstRegion = misReportsManager.GetAllRegionDetailsforMIS();
                lstApplication = misReportsManager.GetAllApplicationDetailsforMIS();//projectId
            }
            catch (Exception)
            {

                throw;
            }

        }

        public IList<tbl_Region> GetRegions(MISReports misReports)
        {
            try
            {
                RegionManager regionManager = new RegionManager();
                misReports.lstRegion = regionManager.GetRegionDetails();
                return misReports.lstRegion;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}