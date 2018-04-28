using System;
using System.Collections.Generic;
using System.Linq;
using DesignAccelerator.Models.ViewModel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;
using System.Drawing;
using System.IO;
using DA.DomainModel;
using DA.BusinessLayer;
using System.Threading.Tasks;

namespace DesignAccelerator.Controllers
{
    public class TransactionMatrix
    {
        IList<tbl_AttributeValues> lstNegativeAttributeValues;

        public async Task<string> GenerateTransactionMatrix(int daId)
        {
            try
            {


                IList<sp_GetMappingViewModelData_Result> lstMappingViewModel = new List<sp_GetMappingViewModelData_Result>();
                TransactionsManager transactions = new TransactionsManager();

                MappingViewModel mappingViewModel = new MappingViewModel();
                lstNegativeAttributeValues = mappingViewModel.GetNegativeAttributeValues(daId);

                lstMappingViewModel = mappingViewModel.GetMappedData(daId);

                var highLevelTransactions = transactions.GetAllTransactions(daId);

                ExcelCommonFunctions excelCommonFunctions = new ExcelCommonFunctions();

                IList<Required> rList = ReturnRequired(lstMappingViewModel, mappingViewModel, highLevelTransactions);
                string filePath = "";
                using (ExcelPackage objExcelPackage = new ExcelPackage())
                { // Format Excel Sheet
                    int i = 0;
                    foreach (var item in highLevelTransactions)
                    {

                        var dtMappingTable = from a in rList
                                             where a.dtMappingTable.TableName == item.HighLevelTxnDesc
                                             select a;

                        var dtTM = from a in rList
                                   where a.dtTM.TableName == item.HighLevelTxnDesc
                                   select a;

                        ExcelWorksheet ws = excelCommonFunctions.CreateSheet(objExcelPackage, item.HighLevelTxnDesc, i);

                        ws.Cells[1, 1].Value = "Transaction - " + item.HighLevelTxnDesc;

                        // Format Excel Sheet
                        ws.Cells[1, 1, 1, 7].Merge = true; //Merge columns start and end range
                        ws.Cells[1, 1, 1, 7].Style.Font.Bold = true; //Font should be bold
                        ws.Cells[1, 1, 1, 7].Style.Font.Size = 20;
                        ws.Cells[1, 1, 1, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; // Alignment is center
                        ws.Cells[1, 1, 1, 7].Style.Fill.PatternType = ExcelFillStyle.Solid; // Border
                        ws.Cells[1, 1, 1, 7].Style.Fill.BackgroundColor.SetColor(Color.LightGray); // Background Color

                        int colIndex = 1, rowIndex = 0;


                        excelCommonFunctions.CreateTableHeader(dtMappingTable.First().dtMappingTable, ws, ref colIndex, ref rowIndex, "tbl1");
                        excelCommonFunctions.AddRows(dtMappingTable.First().dtMappingTable, ws, ref colIndex, ref rowIndex);

                        excelCommonFunctions.CreateTableHeader(dtTM.First().dtTM, ws, ref colIndex, ref rowIndex, "tbl2");
                        excelCommonFunctions.AddRows(dtTM.First().dtTM, ws, ref colIndex, ref rowIndex);

                        //Format Excel Sheet
                        ws.View.ShowGridLines = false;
                        ws.View.ZoomScale = 80;
                        ws.Cells.AutoFitColumns();


                        i++;
                    }
                    tbl_DesignAccelerator da = new tbl_DesignAccelerator();
                    DAManager daManager = new DAManager();

                    da = daManager.FindDA(daId);

                    filePath = excelCommonFunctions.SaveFile(objExcelPackage, da.daName, "", "Rule of N - Txn Matrix");

                }


                return filePath;
            }
            catch (Exception)
            {
                throw;
            }
        }


        private IList<Required> ReturnRequired(IList<sp_GetMappingViewModelData_Result> lstMappingViewModel, MappingViewModel mappingViewModel, IList<tbl_Transactions> highLevelTransactions)
        {
            try
            {


                List<Required> rList = new List<Required>();
                Parallel.ForEach(highLevelTransactions, item =>
                {
                    Required r = new Required();

                    string txnID = item.HighLevelTxnID;
                    r.dtMappingTable.TableName = item.HighLevelTxnDesc;
                    r.dtTM.TableName = item.HighLevelTxnDesc;
                    r = GenerateTransactionMatrix(lstMappingViewModel, r, txnID, mappingViewModel);
                    rList.Add(r);
                //using (StreamWriter writer =
                //                                new StreamWriter(@"D:\Temp\" + item.HighLevelTxnDesc + ".txt"))
                //{
                //    writer.Write("Done ");

                //}

            });
                return rList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private Required GenerateTransactionMatrix(IList<sp_GetMappingViewModelData_Result> lstMappingViewModel, Required r, string txnID, MappingViewModel mappingViewModel)
        {
            try
            {


                List<string> lstAttributes = GetListofAttributesMapped(lstMappingViewModel, txnID, mappingViewModel, r);

                int tmCount = 1;
                foreach (var item in r.dicCriticalAttributes)
                {
                    tmCount *= item.Value;
                }

                CreateHeadersForTM(r, lstAttributes);

                GenerateMappingTable(lstMappingViewModel, r, lstAttributes, txnID);

                int rowCount = RuleofN(lstAttributes, tmCount, txnID, r, Callback);

                rowCount = GenerateNegativeCases(r, lstAttributes, rowCount, txnID);

                return r;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public delegate int CallBack(List<string> lstAttributes, int tmCount, string txnID, Required r);

        private int GenerateNegativeCases(Required r, List<string> lstAttributes, int rowCount, string txnID)
        {
            try
            {


                bool bCheck = false;
                int i = 1;
                #region NegativeValues
                foreach (var item in lstNegativeAttributeValues)
                {
                    DataRow dr = r.dtTM.NewRow();
                    dr["Transaction ID"] = txnID;
                    dr["Test Condition ID"] = txnID + "_TC_" + rowCount.ToString("D3");
                    dr["Test Condition Result"] = "Negative";
                    i = 1;
                    foreach (var att in lstAttributes)
                    {
                        if (att == item.tbl_Attribute.AttributeDesc)
                        {
                            try
                            {
                                dr[item.tbl_Attribute.AttributeDesc] = item.AttributeValue;
                                dr["isNegative" + i] = item.isNegative;
                                bCheck = true;

                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {
                            //IList<tbl_AttributeValues> lstNegValues = new List<tbl_AttributeValues>();

                            //if (dicAttributeValue.TryGetValue(item.tbl_Attribute.AttributeDesc, out lstNegValues))
                            //{
                            //    if (lstNegValues.Count == 2)
                            //    {
                            //        dr[att] = "Positive Values";
                            //    }
                            //    else
                            //    {
                            dr[att] = "Any";
                            //dr["isNegative" + i] = item.isNegative;
                            // dr["isNegative"] = "1"; Please check if red color is required for any also 
                            //    }
                            //}
                        }
                        i++;
                    }

                    if (bCheck)
                    {
                        bCheck = false;
                        rowCount++;
                        r.dtTM.Rows.Add(dr);
                    }
                }
                #endregion
                return rowCount;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private int Callback(List<string> lstAttributes, int tmCount, string txnID, Required r)
        {
            try
            {


                #region RowsForTM

                int iCriticalAttributeCount = 1;
                int rowCount = 1;

                for (int k = 0; k < lstAttributes.Count; k++)
                {
                    if (r.dicCriticalAttributes.ContainsKey(lstAttributes[k]))
                    {
                        #region CriticalAttribute

                        IList<DA.DomainModel.tbl_AttributeValues> lstAttValues = new List<DA.DomainModel.tbl_AttributeValues>();
                        r.dicAttributeValue.TryGetValue(lstAttributes[k], out lstAttValues);

                        if (iCriticalAttributeCount > 1)
                        {
                            int valueRepeatCount, totalRepeat;
                            GetRepeatCount(tmCount, iCriticalAttributeCount, lstAttValues, out valueRepeatCount, out totalRepeat, r.dicCriticalAttributes);
                            int row = 0;

                            for (int m = 0; m < totalRepeat; m++)
                            {
                                foreach (var item in lstAttValues)
                                {
                                    for (int i = 0; i < valueRepeatCount; i++)
                                    {
                                        try
                                        {
                                            r.dtTM.Rows[row][lstAttributes[k]] = item.AttributeValue;
                                            r.dtTM.Rows[row]["isNegative" + (k + 1)] = item.isNegative;
                                        }
                                        catch (Exception ex)
                                        {

                                        }

                                        row++;
                                    }
                                }
                            }

                        }
                        else
                        {
                            int repeatCount = tmCount / lstAttValues.Count;

                            foreach (var item in lstAttValues)
                            {
                                for (int i = 0; i < repeatCount; i++)
                                {
                                    DataRow dr = r.dtTM.NewRow();
                                    dr["Transaction ID"] = txnID;
                                    dr["Test Condition ID"] = txnID + "_TC_" + rowCount.ToString("D3");
                                    dr["Test Condition Result"] = "Positive";
                                    try
                                    {
                                        dr[lstAttributes[k]] = item.AttributeValue;
                                        dr["isNegative" + (k + 1)] = item.isNegative;
                                    }
                                    catch (Exception ex)
                                    {
                                        dr[lstAttributes[k]] = "";
                                    }

                                    rowCount++;
                                    r.dtTM.Rows.Add(dr);
                                }
                            }
                        }

                        iCriticalAttributeCount++;
                        #endregion
                    }
                    else
                    {
                        #region CommonAttribute
                        IList<DA.DomainModel.tbl_AttributeValues> lstAttValues = new List<DA.DomainModel.tbl_AttributeValues>();
                        r.dicAttributeValue.TryGetValue(lstAttributes[k], out lstAttValues);

                        //to store positive value
                        Dictionary<string, string> dicpositiveVal = new Dictionary<string, string>();

                        int pCount = 0, nCount = 0;
                        for (int p = 0; p < lstAttValues.Count; p++)
                        {
                            if (lstAttValues[p].isNegative == "0")
                                pCount++;
                            else
                                nCount++;
                        }
                        for (int p = 0; p < lstAttValues.Count; p++)
                        {
                            if (pCount != 0 && nCount != 0)
                            {
                                if (lstAttValues[p].isNegative == "0" && pCount == 1)
                                {
                                    dicpositiveVal.Add(lstAttributes[k], lstAttValues[p].AttributeValue);
                                }
                            }

                        }



                        for (int i = 0; i < tmCount; i++)
                        {
                            try
                            {
                                //To repeat positive value in case of single positive and negative value 
                                if (dicpositiveVal.Count != 0 && dicpositiveVal.ContainsKey(lstAttributes[k]))
                                    r.dtTM.Rows[i][lstAttributes[k]] = dicpositiveVal[lstAttributes[k]];
                                else
                                {
                                    if (lstAttValues[i].isNegative == "0")

                                        r.dtTM.Rows[i][lstAttributes[k]] = lstAttValues[i].AttributeValue;
                                    else
                                        r.dtTM.Rows[i][lstAttributes[k]] = "Any";
                                    //Here also you check if red color is required
                                }

                            }
                            catch (Exception ex)
                            {
                                r.dtTM.Rows[i][lstAttributes[k]] = "Any";
                            }
                        }
                        #endregion
                    }
                }

                #endregion

                return rowCount;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private int RuleofN(List<string> lstAttributes, int tmCount, string txnID, Required r, CallBack obj)
        {
            try
            {


                IAsyncResult async = obj.BeginInvoke(lstAttributes, tmCount, txnID, r, null, null);
                int i = obj.EndInvoke(async);
                return i;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void GetRepeatCount(int tmCount, int iCriticalAttributeCount, IList<tbl_AttributeValues> lstAttValues, out int valueRepeatCount, out int totalRepeat, Dictionary<string, int> dicCriticalAttributes)
        {
            try
            {


                valueRepeatCount = 1;
                for (int i = iCriticalAttributeCount; i < dicCriticalAttributes.Count; i++)
                {
                    valueRepeatCount *= dicCriticalAttributes.Values.ElementAt(i);
                }

                totalRepeat = tmCount / lstAttValues.Count;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void GenerateMappingTable(IList<sp_GetMappingViewModelData_Result> lstMappingViewModel, Required r, List<string> lstAttributes,
            string txnID)
        {
            try
            {

                #region RowsForMappingTable
                for (int i = 0; i < 12; i++)
                {
                    DataRow dr = r.dtMappingTable.NewRow();

                    dr["Transaction ID"] = txnID;
                    dr["Attributes"] = "Value" + (i + 1).ToString();//lstMappingViewModel[i].ATTRIBUTEDESC;

                    for (int k = 0; k < lstAttributes.Count; k++)
                    {
                        try
                        {
                            //Adding Row for Mapping table
                            dr[lstAttributes[k]] = r.dicAttributeValue[lstAttributes[k]][i].AttributeValue;
                            dr["isNegative" + (k + 1)] = r.dicAttributeValue[lstAttributes[k]][i].isNegative;
                            //here also u need to update isNegative in dtMapping table, it is there in dicAttributeValue 
                        }
                        catch (Exception ex)
                        {

                        }
                    }



                    r.dtMappingTable.Rows.Add(dr);
                }
                #endregion
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<string> GetListofAttributesMapped(IList<sp_GetMappingViewModelData_Result> lstMappingViewModel, string txnID,
           MappingViewModel mappingViewModel, Required r)
        {
            try
            {


                //dicAttributeValue = new Dictionary<string, IList<DA.DomainModel.tbl_AttributeValues>>();
                //dicCriticalAttributes = new Dictionary<string, int>();
                List<string> lstAttributes = new List<string>();

                var lstMapViewModel = from s in lstMappingViewModel
                                      where s.HIGHLEVELTXNID == txnID
                                      orderby s.ATTRIBUTEID ascending
                                      select s;

                foreach (var item in lstMapViewModel)
                {
                    lstAttributes.Add(item.ATTRIBUTEDESC);

                    if (!r.dicAttributeValue.Keys.Contains(item.ATTRIBUTEDESC))
                    {
                        switch (item.ATTRIBUTETYPEDESC)
                        {
                            case "Common":
                                r.dicCriticalAttributes.Add(item.ATTRIBUTEDESC, mappingViewModel.dicAttributesanditsValues[item.ATTRIBUTEDESC].Count());
                                break;
                            case "Common&Critical":
                                r.dicCriticalAttributes.Add(item.ATTRIBUTEDESC, mappingViewModel.dicAttributesanditsValues[item.ATTRIBUTEDESC].Count());
                                break;
                            default:
                                break;
                        }

                        r.dicAttributeValue.Add(item.ATTRIBUTEDESC, mappingViewModel.dicAttributesanditsValues[item.ATTRIBUTEDESC]);
                    }

                }



                return lstAttributes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CreateHeadersForTM(Required r, List<string> lstAttributes)
        {

            try
            {


                //Mapping table
                r.dtMappingTable.Columns.Add("Transaction ID");
                r.dtMappingTable.Columns.Add("Attributes");
                //TM Table
                r.dtTM.Columns.Add("Transaction ID");
                r.dtTM.Columns.Add("Test Condition ID");

                //Both Mapping & TM Table
                int i = 1;
                foreach (var att in lstAttributes)
                {
                    r.dtMappingTable.Columns.Add(att);
                    r.dtTM.Columns.Add(att);
                    r.dtTM.Columns.Add("isNegative" + i);
                    r.dtMappingTable.Columns.Add("isNegative" + i);
                    i++;
                }
                r.dtTM.Columns.Add("Test Condition Result");
                r.dtTM.Columns.Add("Business Rule ID");
                r.dtTM.Columns.Add("Interface ID");
                r.dtTM.Columns.Add("Channel Alert ID");
                r.dtTM.Columns.Add("Report ID");
                r.dtTM.Columns.Add("Validation 1");
                r.dtTM.Columns.Add("Validation 2");
                r.dtTM.Columns.Add("Validation 3");
                r.dtTM.Columns.Add("Validation 4");
                r.dtTM.Columns.Add("Validation 5");
                r.dtTM.Columns.Add("Logical Day");
                r.dtTM.Columns.Add("Batch Frequency");
                r.dtTM.Columns.Add("Gap Reference");
            }
            catch (Exception)
            {
                throw;
            }
        }

        //To check if br table already exists, if yes then it will be deleted.
        public void DeleteBRTableIfExists(ExcelWorksheet workSheet, ref int colIndex, ref int rowIndex, string hlt)
        {
            try
            {



                //to check for a BR table already exists or not
                for (int rowInd = 17; rowInd <= workSheet.Dimension.End.Row; rowInd++)
                {
                    for (int colind = workSheet.Dimension.Start.Column; colind <= workSheet.Dimension.Start.Column; colind++)
                    {
                        if (workSheet.Cells[rowInd, colind].Value != null)
                        {
                            //fetch the column name
                            string columnName = workSheet.Cells[rowInd, colind].Value.ToString();
                            //enter only if it is any one among the below
                            if (columnName == hlt)
                            {
                                workSheet.DeleteRow(rowInd, workSheet.Dimension.End.Row);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void DeleteInterfaceTableIfExists(ExcelWorksheet workSheet, ref int colIndex, ref int rowIndex, string hlt)
        {
            try
            {



                //to check for a BR CH Interface and report table already exists or not
                for (int rowInd = 17; rowInd <= workSheet.Dimension.End.Row; rowInd++)
                {
                    for (int colind = workSheet.Dimension.Start.Column; colind <= workSheet.Dimension.Start.Column; colind++)
                    {
                        if (workSheet.Cells[rowInd, colind].Value != null)
                        {
                            //fetch the column name
                            string columnName = workSheet.Cells[rowInd, colind].Value.ToString();
                            //enter only if it is any one among the below
                            if (columnName == hlt)
                            {
                                workSheet.DeleteRow(rowInd, workSheet.Dimension.End.Row);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void DeleteCATableIfExists(ExcelWorksheet workSheet, ref int colIndex, ref int rowIndex, string hlt)
        {
            try
            {



                //to check for a BR CH Interface and report table already exists or not
                for (int rowInd = 17; rowInd <= workSheet.Dimension.End.Row; rowInd++)
                {
                    for (int colind = workSheet.Dimension.Start.Column; colind <= workSheet.Dimension.Start.Column; colind++)
                    {
                        if (workSheet.Cells[rowInd, colind].Value != null)
                        {
                            //fetch the column name
                            string columnName = workSheet.Cells[rowInd, colind].Value.ToString();
                            //enter only if it is any one among the below
                            if (columnName == hlt)
                            {
                                workSheet.DeleteRow(rowInd, workSheet.Dimension.End.Row);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteReportsTableIfExists(ExcelWorksheet workSheet, ref int colIndex, ref int rowIndex, string hlt)
        {

            try
            {


                //to check for a BR CH Interface and report table already exists or not
                for (int rowInd = 17; rowInd <= workSheet.Dimension.End.Row; rowInd++)
                {
                    for (int colind = workSheet.Dimension.Start.Column; colind <= workSheet.Dimension.Start.Column; colind++)
                    {
                        if (workSheet.Cells[rowInd, colind].Value != null)
                        {
                            //fetch the column name
                            string columnName = workSheet.Cells[rowInd, colind].Value.ToString();
                            //enter only if it is any one among the below
                            if (columnName == hlt)
                            {
                                workSheet.DeleteRow(rowInd, workSheet.Dimension.End.Row);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetRuleOfNData(ExcelWorksheet workSheet, ref int colIndex, ref int rowIndex)
        {
            try
            {


                bool firstRowHeader = true;
                bool reachedEnd = false;
                DataTable dtRuleN = new DataTable();

                //To check weather the table exists, if yes, then the table is deleted and a new table is generated i.e overwritten
                // DeleteTableIfExists(workSheet, ref colIndex, ref rowIndex);

                if (workSheet != null)
                {
                    Dictionary<string, int> header = new Dictionary<string, int>();

                    for (rowIndex = 17; rowIndex <= workSheet.Dimension.End.Row && reachedEnd == false; rowIndex++)
                    {
                        //Assume the first row is the header. Then use the column match ups by name to determine the index.
                        //This will allow you to have the order of the columns change without any affect.


                        if (rowIndex == 17 && firstRowHeader)
                        {
                            for (int columnIndex = workSheet.Dimension.Start.Column; columnIndex <= workSheet.Dimension.End.Column; columnIndex++)
                            {
                                if (workSheet.Cells[rowIndex, columnIndex].Value != null)
                                {
                                    string columnName = workSheet.Cells[rowIndex, columnIndex].Value.ToString();

                                    if (!header.ContainsKey(columnName) && !string.IsNullOrEmpty(columnName))
                                    {
                                        header.Add(columnName, columnIndex);
                                        dtRuleN.Columns.Add(columnName);
                                    }
                                }
                            }
                        }
                        else
                        {
                            DataRow dr = dtRuleN.NewRow();
                            for (int k = 0; k < dtRuleN.Columns.Count; k++)
                            {
                                if (workSheet.Cells[rowIndex, 1].Value == null)
                                {
                                    reachedEnd = true;
                                    break;
                                }
                                dr[dtRuleN.Columns[k].ToString()] = ParseWorksheetValue(workSheet, header, rowIndex, dtRuleN.Columns[k].ToString());

                            }
                            if (!reachedEnd)
                                dtRuleN.Rows.Add(dr);

                        }
                    }
                }
                if (workSheet != null)
                {
                    rowIndex = workSheet.Dimension.End.Row;
                }
                return dtRuleN;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string ParseWorksheetValue(ExcelWorksheet workSheet, Dictionary<string, int> header, int rowIndex, string columnName)
        {
            try
            {


                string value = string.Empty;
                int? columnIndex = header.ContainsKey(columnName) ? header[columnName] : (int?)null;

                if (workSheet != null && columnIndex != null && workSheet.Cells[rowIndex, columnIndex.Value].Value != null)
                {
                    value = workSheet.Cells[rowIndex, columnIndex.Value].Value.ToString();
                }

                return value;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Looping for the last most tables in the Transactrion Matrix sheet
        public List<DataTable> GetRuleOfNDataForTestDesignRuleReferene(ExcelWorksheet workSheet, ref int colIndex, ref int rowIndex)
        {
            try
            {


                DataTable dt = new DataTable();
                List<DataTable> listdt = new List<DataTable>();

                List<DataTable> listofTMTables = new List<DataTable>();
                var start = workSheet.Dimension.Start;
                var end = workSheet.Dimension.End;
                bool firstRowHeader = true;
                string tableName = "";

                //read from row 17 and not 1 since we already know transactiom matrix tables start from row 17, we can skip the previous rows
                for (rowIndex = 17; rowIndex <= end.Row; rowIndex++)
                {
                    for (colIndex = start.Column; colIndex <= start.Column; colIndex++)//keep reading just first columns
                    {
                        if (workSheet.Cells[rowIndex, colIndex].Value != null)
                        {
                            //fetch the column name
                            string columnName = workSheet.Cells[rowIndex, colIndex].Value.ToString();
                            //enter only if it is any one among the below
                            if (columnName == "Business Rules - " + workSheet.ToString() || columnName == "Channels and Alerts - " + workSheet.ToString() ||
                                columnName == "Reports - " + workSheet.ToString() || columnName == "Interface - " + workSheet.ToString())
                            {
                                //to keep record of table name
                                tableName = columnName;
                                dt = GetLastTablesFromTM(workSheet, colIndex, ref rowIndex, firstRowHeader, columnName);
                                dt.TableName = tableName;


                                listofTMTables.Add(dt);


                                rowIndex--;//decrement row since it will again increment back, we need this to read the next table name 
                                firstRowHeader = true;//set to true to read the next table header
                            }

                        }
                    }

                }


                return listofTMTables;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DataTable GetLastTablesFromTM(ExcelWorksheet workSheet, int colIndex, ref int rowIndex, bool firstRowHeader, string columnName)
        {
            try
            {


                bool reachedEnd = false;
                DataTable dtRuleN = new DataTable();
                Dictionary<string, int> header = new Dictionary<string, int>();

                var start = workSheet.Dimension.Start;
                var end = workSheet.Dimension.End;


                //After it finds BR||CH||Interface||reports, the next row is empty, hence increment by 2
                rowIndex = rowIndex + 2;
                int currentRow = rowIndex;// to know that we are reading the header
                for (; rowIndex <= end.Row && reachedEnd == false; rowIndex++)
                {
                    if (currentRow == rowIndex && firstRowHeader)
                    {
                        for (int columnIndex = start.Row; columnIndex <= end.Column; columnIndex++)
                        {
                            if (workSheet.Cells[rowIndex, columnIndex].Value != null)
                            {
                                columnName = workSheet.Cells[rowIndex, columnIndex].Value.ToString();

                                if (!header.ContainsKey(columnName) && !string.IsNullOrEmpty(columnName))
                                {
                                    header.Add(columnName, columnIndex);
                                    dtRuleN.Columns.Add(columnName);
                                }
                            }
                        }
                    }
                    //for rows
                    else
                    {
                        DataRow dr = dtRuleN.NewRow();
                        for (int k = 0; k < dtRuleN.Columns.Count; k++)
                        {
                            if (workSheet.Cells[rowIndex, 1].Value == null)
                            {
                                reachedEnd = true;
                                break;
                            }
                            dr[dtRuleN.Columns[k].ToString()] = ParseWorksheetValue(workSheet, header, rowIndex, dtRuleN.Columns[k].ToString());
                        }
                        if (!reachedEnd)
                            dtRuleN.Rows.Add(dr);
                    }
                }
                return dtRuleN;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetRuleOfNDataForAllTables(ExcelWorksheet workSheet, ref int colIndex, ref int rowIndex, string highleveltrans)
        {
            try
            {


                bool firstRowHeader = true;
                bool reachedEnd = false;
                DataTable dtRuleN = new DataTable();

                //To check weather the table exists, if yes, then the table is deleted and a new table is generated i.e overwritten
                if (highleveltrans.Contains("Business Rules"))
                {
                    DeleteBRTableIfExists(workSheet, ref colIndex, ref rowIndex, highleveltrans);

                }
                else if (highleveltrans.Contains("Interface"))
                {
                    DeleteInterfaceTableIfExists(workSheet, ref colIndex, ref rowIndex, highleveltrans);
                }
                else if (highleveltrans.Contains("Channels and Alerts"))
                {
                    DeleteCATableIfExists(workSheet, ref colIndex, ref rowIndex, highleveltrans);

                }
                else if (highleveltrans.Contains("Reports"))
                {
                    DeleteReportsTableIfExists(workSheet, ref colIndex, ref rowIndex, highleveltrans);

                }
                if (workSheet != null)
                {
                    Dictionary<string, int> header = new Dictionary<string, int>();

                    for (rowIndex = 17; rowIndex <= workSheet.Dimension.End.Row && reachedEnd == false; rowIndex++)
                    {
                        //Assume the first row is the header. Then use the column match ups by name to determine the index.
                        //This will allow you to have the order of the columns change without any affect.


                        if (rowIndex == 17 && firstRowHeader)
                        {
                            for (int columnIndex = workSheet.Dimension.Start.Column; columnIndex <= workSheet.Dimension.End.Column; columnIndex++)
                            {
                                if (workSheet.Cells[rowIndex, columnIndex].Value != null)
                                {
                                    string columnName = workSheet.Cells[rowIndex, columnIndex].Value.ToString();

                                    if (!header.ContainsKey(columnName) && !string.IsNullOrEmpty(columnName))
                                    {
                                        header.Add(columnName, columnIndex);
                                        dtRuleN.Columns.Add(columnName);
                                    }
                                }
                            }
                        }
                        else
                        {
                            DataRow dr = dtRuleN.NewRow();
                            for (int k = 0; k < dtRuleN.Columns.Count; k++)
                            {
                                if (workSheet.Cells[rowIndex, 1].Value == null)
                                {
                                    reachedEnd = true;
                                    break;
                                }
                                dr[dtRuleN.Columns[k].ToString()] = ParseWorksheetValue(workSheet, header, rowIndex, dtRuleN.Columns[k].ToString());

                            }
                            if (!reachedEnd)
                                dtRuleN.Rows.Add(dr);

                        }
                    }
                }

                if (workSheet != null)
                {
                    rowIndex = workSheet.Dimension.End.Row;
                }
                return dtRuleN;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}