using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DA.DomainModel;
using DA.BusinessLayer;

namespace DesignAccelerator.Models.ViewModel
{
    public class TransactionsViewModel
    {
        #region Public Properties 
        public int TransactionSeq { get; set; }

        [Required(ErrorMessage = "Requirement reference required")]
        [RegularExpression(@"^[a-zA-Z0-9._ ]*$", ErrorMessage = "Special Characters are not allowed in this field")]
        public string ReqReference { get; set; }

        public string HighLevelTxnID { get; set; }

        [Required(ErrorMessage = "High level transaction name required")]
        [RegularExpression(@"^[a-zA-Z0-9_ ]*$", ErrorMessage = "Special Characters are not allowed in this field")]
        public string HighLevelTxnDesc { get; set; }

        public int LifeCycleID { get; set; }
        public int daId { get; set; }

        public int roleId { get; set; }
        public string RoleName { get; set; }

        public bool AddPermmission = false;
        public bool EdiPermission = false;
        public bool DeletePermission = false;

        public string LifeCycleDesc { get; set; }
        public IList<tbl_LifeCycle> lstLifeCycle { get; set; }
        public IList<tbl_Transactions> lstTransactions { get; set; }

        //Flow Implementation
        public int ClientID { get; set; }
        public string ClientName { get; set; }
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int ApplicationID { get; set; }
        public string ApplicationName { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string daName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        #endregion

        public TransactionsViewModel GetTransactions(int? daId)
        {
            try
            {
                TransactionsViewModel transviewmodel = new TransactionsViewModel();
                TransactionsManager transmanager = new TransactionsManager();
                LifeCycleManager LCManager = new LifeCycleManager();

                transviewmodel.lstLifeCycle = LCManager.GetLifeCycles(daId);

                transviewmodel.lstTransactions = transmanager.GetAllTransactions(daId);

                return transviewmodel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void AddTrans(TransactionsViewModel transviewmodel)
        {
            try
            {
                tbl_Transactions tbltrans = new tbl_Transactions();

                tbltrans.HighLevelTxnID = transviewmodel.HighLevelTxnID;
                tbltrans.HighLevelTxnDesc = transviewmodel.HighLevelTxnDesc;
                tbltrans.LifeCycleID = transviewmodel.LifeCycleID;
                if (transviewmodel.ReqReference != null)
                {
                    tbltrans.ReqReference = transviewmodel.ReqReference;
                }
                tbltrans.daId = transviewmodel.daId;

                tbltrans.EntityState = DA.DomainModel.EntityState.Added;

                TransactionsManager transmanager = new TransactionsManager();
                transmanager.AddTransaction(tbltrans);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool DeleteTrans(TransactionsViewModel transviewmodel)
        {
            try
            {
                tbl_Transactions tbltrans = new tbl_Transactions();

                tbltrans.TransactionSeq = transviewmodel.TransactionSeq;
                tbltrans.EntityState = DA.DomainModel.EntityState.Deleted;

                TransactionsManager transmanager = new TransactionsManager();
                transmanager.DeleteTransaction(tbltrans);

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UpdateTrans(TransactionsViewModel transviewmodel)
        {
            try
            {
                tbl_Transactions tbltrans = new tbl_Transactions();

                tbltrans.TransactionSeq = transviewmodel.TransactionSeq;
                tbltrans.HighLevelTxnID = transviewmodel.HighLevelTxnID;
                tbltrans.HighLevelTxnDesc = transviewmodel.HighLevelTxnDesc;
                tbltrans.LifeCycleID = transviewmodel.LifeCycleID;
                tbltrans.ReqReference = transviewmodel.ReqReference;
                tbltrans.daId = transviewmodel.daId; //1

                tbltrans.EntityState = DA.DomainModel.EntityState.Modified;

                TransactionsManager transmanager = new TransactionsManager();
                transmanager.UpdateTransaction(tbltrans);
            }
            catch (Exception)
            {

                throw;
            }
        }

        //public DAViewModel FindDA(int? daID)
        //{
        //    DAViewModel dAViewModel = new DAViewModel();
        //    DAManager daManager = new DAManager();
        //    var da = daManager.FindDA(daID);
        //    dAViewModel.DAID = da.daid;
        //    dAViewModel.DAName = da.daName;
        //    dAViewModel.ModuleId = (int)da.ModuleId;
        //    return dAViewModel;
        //}

        public TransactionsViewModel FindTrans(int? Transq)
        {
            try
            {
                TransactionsViewModel transvm = new TransactionsViewModel();
                TransactionsManager transmanager = new TransactionsManager();

                var trans1 = transmanager.FindTransaction(Transq);
                var daId1 = trans1.daId;

                LifeCycleManager LCManager = new LifeCycleManager();
                transvm.lstLifeCycle = LCManager.GetLifeCycles(daId1);
                transvm.TransactionSeq = trans1.TransactionSeq;
                transvm.HighLevelTxnID = trans1.HighLevelTxnID;
                transvm.HighLevelTxnDesc = trans1.HighLevelTxnDesc;
                transvm.ReqReference = trans1.ReqReference;
                transvm.LifeCycleID = trans1.LifeCycleID;
                transvm.LifeCycleDesc = transvm.lstLifeCycle.Where(e => e.LifeCycleID.Equals(trans1.LifeCycleID)).First().LifeCycleDesc;
                transvm.daId = trans1.daId;

                //transvm.lstTransactions = (IList<tbl_Transactions>)trans1;

                return transvm;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool CheckDuplicate(TransactionsViewModel TransVM)
        {
            try
            {
                TransactionsManager transmanager = new TransactionsManager();

                var Transaction = transmanager.FindHLTransaction(TransVM.HighLevelTxnID, TransVM.LifeCycleID, TransVM.HighLevelTxnDesc, TransVM.ReqReference, TransVM.daId);

                if (Transaction != null && Transaction.TransactionSeq == TransVM.TransactionSeq && Transaction.HighLevelTxnID.ToUpper() != TransVM.HighLevelTxnID.ToUpper()
                    && Transaction.LifeCycleID == TransVM.LifeCycleID && Transaction.HighLevelTxnDesc.ToUpper() == TransVM.HighLevelTxnDesc.ToUpper()
                    && Transaction.ReqReference.ToUpper() == TransVM.ReqReference.ToUpper())

                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IList<tbl_LifeCycle> GetLifeCycle(TransactionsViewModel TransVM)
        {
            try
            {
                LifeCycleManager lifeCycleManager = new LifeCycleManager();
                TransVM.lstLifeCycle = lifeCycleManager.GetLifeCycles(daId);
                return TransVM.lstLifeCycle;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public IList<tbl_Transactions> GetReqRef(int id)
        {
            try
            {
                TransactionsManager TxnManager = new TransactionsManager();
                IList<tbl_Transactions> TxnReqRef = TxnManager.FindReqRef(id);

                return TxnReqRef;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void GetScreenAccessRights(string screenName)
        {
            try
            {
                tbl_UserData currentloggedinuserdata = (tbl_UserData)HttpContext.Current.Session["CurrentLoggedInUserDetails"];
                roleId = currentloggedinuserdata.RoleID;

                RoleManager roleManager = new RoleManager();
                var userrolepermissions = roleManager.GetUserViewAccessPermissions(screenName, roleId);

                foreach (var item in userrolepermissions)
                {
                    if (item.ActionType == "Add")
                        AddPermmission = true;
                    else if (item.ActionType == "Edit")
                        EdiPermission = true;
                    else if (item.ActionType == "Delete")
                        DeletePermission = true;

                    RoleName = item.RoleName;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

