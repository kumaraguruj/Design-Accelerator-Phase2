using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DA.DomainModel;
using DA.BusinessLayer;

namespace DesignAccelerator.Models.ViewModel
{
    public class PeriodTypeViewModel
    {
        public int periodTypeID { get; set; }
        public string periodTypeDesc { get; set; }

        public int ApplicationID { get; set; }
        public string ApplicationName { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }

        public IList<PeriodTypeViewModel> PeriodTypeList { get; set; }

        public void AddPeriodType(PeriodTypeViewModel periodTypeViewModel)
        {
            try
            {
                tbl_PeriodType tblPeriodType = new tbl_PeriodType();

                tblPeriodType.PeriodTypeDesc = periodTypeViewModel.periodTypeDesc;

                tblPeriodType.EntityState = DA.DomainModel.EntityState.Added;

                PeriodTypeManager periodTypeManager = new PeriodTypeManager();
                periodTypeManager.AddPeriodType(tblPeriodType);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void DeletePeriodType(PeriodTypeViewModel periodTypeViewModel)
        {
            try
            {
                tbl_PeriodType tblPeriodType = new tbl_PeriodType();
                tblPeriodType.PeriodTypeID = periodTypeViewModel.periodTypeID;
                tblPeriodType.EntityState = DA.DomainModel.EntityState.Deleted;

                PeriodTypeManager periodTypeManager = new PeriodTypeManager();
                periodTypeManager.DeletePeriodType(tblPeriodType);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void UpdatePeriodType(PeriodTypeViewModel periodTypeiewModel)
        {
            try
            {
                tbl_PeriodType tblPeriodType = new tbl_PeriodType();

                tblPeriodType.PeriodTypeID = periodTypeiewModel.periodTypeID;
                tblPeriodType.PeriodTypeDesc = periodTypeiewModel.periodTypeDesc;
                tblPeriodType.EntityState = DA.DomainModel.EntityState.Modified;

                PeriodTypeManager periodTypeManager = new PeriodTypeManager();
                periodTypeManager.UpdatePeriodType(tblPeriodType);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public IList<PeriodTypeViewModel> GetPeriodTypeDetails(int? daId)
        {
            try
            {
                PeriodTypeManager periodTypeManager = new PeriodTypeManager();
                var periodTypeList = periodTypeManager.GetSPeriodTypeDetails();

                PeriodTypeList = new List<PeriodTypeViewModel>();
                foreach (var item in periodTypeList)
                {
                    PeriodTypeViewModel periodTypeViewModel = new PeriodTypeViewModel();
                    periodTypeViewModel.periodTypeID = item.PeriodTypeID;
                    periodTypeViewModel.periodTypeDesc = item.PeriodTypeDesc;


                    PeriodTypeList.Add(periodTypeViewModel);
                }
                //Trial
                return PeriodTypeList;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public PeriodTypeViewModel FindPeriodType(int? periodTypeID)
        {
            try
            {
                PeriodTypeViewModel periodTypeViewModel = new PeriodTypeViewModel();

                PeriodTypeManager periodTypeManager = new PeriodTypeManager();
                var periodType = periodTypeManager.FindPeriodType(periodTypeID);

                periodTypeViewModel.periodTypeID = periodType.PeriodTypeID;
                periodTypeViewModel.periodTypeDesc = periodType.PeriodTypeDesc;

                return periodTypeViewModel;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}