using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.DataAccessLayer;

using System.Web;
using DA.DomainModel;

namespace DA.BusinessLayer
{
    public class TransactionsManager
    {
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

        public void AddTransaction(tbl_Transactions tblTransactions)
        {
            try
            {
                IGenericDataRepository<tbl_Transactions> repository = new GenericDataRepository<tbl_Transactions>();
                repository.Add(tblTransactions);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void DeleteTransaction(tbl_Transactions tblTransactions)
        {
            try
            {
                IGenericDataRepository<tbl_Transactions> repository = new GenericDataRepository<tbl_Transactions>();
                repository.Remove(tblTransactions);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UpdateTransaction(tbl_Transactions tblTransactions)
        {
            try
            {
                IGenericDataRepository<tbl_Transactions> repository = new GenericDataRepository<tbl_Transactions>();
                repository.Update(tblTransactions);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public tbl_Transactions FindTransaction(int? TransactionSeq)
        {
            try
            {
                IGenericDataRepository<tbl_Transactions> repository = new GenericDataRepository<tbl_Transactions>();
                tbl_Transactions tblTransactions = repository.GetSingle(b => b.TransactionSeq == TransactionSeq);
                return tblTransactions;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public tbl_Transactions FindHLTransaction(string highLevelTransactionId, int lifeCycleId, string highLevelTransactionDesc, string reqRef, int daid)
        //, int lifeCycleId, string highLevelTransactionDesc, string reqRef
        {
            try
            {
                IGenericDataRepository<tbl_Transactions> repository = new GenericDataRepository<tbl_Transactions>();
                tbl_Transactions tblTransactions = repository.GetSingle(b => b.HighLevelTxnID.ToUpper() == highLevelTransactionId.ToUpper() && b.LifeCycleID == lifeCycleId
                                                    && b.HighLevelTxnDesc.ToUpper() == highLevelTransactionDesc.ToUpper() && b.ReqReference.ToUpper() == reqRef.ToUpper());

                //tbl_Transactions tblTransactions = repository.GetSingle(b => b.HighLevelTxnID.ToUpper() == highLevelTransactionId.ToUpper() && b.daId == daid);
                return tblTransactions;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public tbl_Transactions FindReqRefAndHLT(string temp, int? daid)
        {
            try
            {
                IGenericDataRepository<tbl_Transactions> repository = new GenericDataRepository<tbl_Transactions>();
                tbl_Transactions tblTransactions = repository.GetSingle(a => a.HighLevelTxnID == temp && a.daId == daid);

                return tblTransactions;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IList<tbl_Transactions> FindReqRef(int? daid)
        {
            try
            {
                IGenericDataRepository<tbl_Transactions> repository = new GenericDataRepository<tbl_Transactions>();
                IList<tbl_Transactions> tblTransactions = repository.GetList(q => q.daId.Equals(daid));

                return tblTransactions;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
