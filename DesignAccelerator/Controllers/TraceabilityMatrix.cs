using DA.BusinessLayer;
using DA.DomainModel;
using DesignAccelerator.Models.ViewModel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;

namespace DesignAccelerator.Controllers
{
    public class TraceabilityMatrix
    {
        ExcelCommonFunctions excelCommonFunctions = new ExcelCommonFunctions();
        DataTable dtTraceabilityMtrx = new DataTable();
        DataTable dtControlSheet = new DataTable();
        DataTable dtSummary = new DataTable();

        Dictionary<string, List<DataTable>> ruleReferences = new Dictionary<string, List<DataTable>>();
        TestDesignController testdesignRuleReferenceData = new TestDesignController();
        ScenarioBuilderViewModel testDesignVM = new ScenarioBuilderViewModel();
        List<DataTable> dtRuleOfNforTestDesignRuleReference = new List<DataTable>();
        TransactionMatrix transactionMatrix = new TransactionMatrix();
        DataSet transactionMatrixSheets = new DataSet();

        int totalCount = 0;//to keep the count of total no of rows in the control sheet

        int grandTotal = 0;// for grand total in the summary sheet.

        ExcelWorksheet traceabilityMatrixWs;
        ExcelWorksheet controlSheet;

        public string GenerateTraceabilityMatrix(int daId, HttpPostedFileBase tmFile, HttpPostedFileBase testDesignFile)
        {
            try
            {


                testDesignVM.lstHighLevelTransactions = testDesignVM.GetTransactionsList(daId).lstTransactions;




                using (var objExcelPackage = new ExcelPackage(tmFile.InputStream))
                {
                    foreach (var trans in testDesignVM.lstHighLevelTransactions)
                    {
                        int colIdex = 2, rowIdex = 5;

                        ExcelWorksheet ws = excelCommonFunctions.OpenSheet(objExcelPackage, trans.HighLevelTxnDesc);

                        //For Test design last tables
                        dtRuleOfNforTestDesignRuleReference = transactionMatrix.GetRuleOfNDataForTestDesignRuleReferene(ws, ref colIdex, ref rowIdex);

                        //Adding all the fetched tables into the dictionary from list 
                        foreach (var item in dtRuleOfNforTestDesignRuleReference)
                        {
                            if (ruleReferences.ContainsKey(ws.ToString()))
                            {
                                ruleReferences[ws.ToString()].Add(item);
                            }
                            else
                            {
                                ruleReferences.Add(ws.ToString(), new List<DataTable> { item });
                            }
                        }

                        DataTable dtRuleOfN = transactionMatrix.GetRuleOfNData(ws, ref colIdex, ref rowIdex);
                        dtRuleOfN.TableName = trans.HighLevelTxnDesc;
                        transactionMatrixSheets.Tables.Add(dtRuleOfN);


                    }
                }

                #region traceabilityMatrix
                int colIndex = 1;
                int rowIndex = 11;

                using (var package = new ExcelPackage())
                {
                    using (var objExcelPackage = new ExcelPackage(testDesignFile.InputStream))
                    {
                        traceabilityMatrixWs = package.Workbook.Worksheets.Add("Traceability Matrix");

                        controlSheet = excelCommonFunctions.CreateSheet(package, "Control Sheet", 1);

                        ExcelWorksheet summarySheet = excelCommonFunctions.CreateSheet(package, "Summary", 2);

                        CreateHeadersForSummarySheet(summarySheet, out dtSummary);

                        string summarySheetDisplayName = "Summary";
                        summarySheet.Cells[2, 1].Value = summarySheetDisplayName;

                        var summaryCellFormat = summarySheet.Cells[2, 1, 2, 3];
                        summaryCellFormat.Merge = true;
                        summaryCellFormat.Style.Font.Bold = true;
                        summaryCellFormat.Style.Font.Size = 14;
                        summaryCellFormat.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        summaryCellFormat.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        summaryCellFormat.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                        excelCommonFunctions.CreateTableHeader(dtSummary, summarySheet, ref colIndex, ref rowIndex, "Summary");

                        summarySheet.View.ShowGridLines = false;
                        summarySheet.View.ZoomScale = 80;
                        summarySheet.Cells.AutoFitColumns();



                        CreateHeadersForTraceabilityMatrix(traceabilityMatrixWs, out dtTraceabilityMtrx);

                        CreateHeadersForControlSheet(controlSheet, out dtControlSheet);

                        string controlSheetDisplayName = "Control Sheet";
                        controlSheet.Cells[2, 1].Value = controlSheetDisplayName;

                        var csCellFormat = controlSheet.Cells[2, 1, 2, 3];
                        csCellFormat.Merge = true;
                        csCellFormat.Style.Font.Bold = true;
                        csCellFormat.Style.Font.Size = 14;
                        csCellFormat.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        csCellFormat.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        csCellFormat.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                        excelCommonFunctions.CreateTableHeader(dtControlSheet, controlSheet, ref colIndex, ref rowIndex, "ControlSheet");

                        controlSheet.View.ShowGridLines = false;
                        controlSheet.View.ZoomScale = 80;
                        controlSheet.Cells.AutoFitColumns();

                        string displayName = "Detailed-Traceability Matrix";
                        traceabilityMatrixWs.Cells[2, 1].Value = displayName;


                        var cellFormat = traceabilityMatrixWs.Cells[2, 1, 2, 3];

                        cellFormat.Merge = true;
                        cellFormat.Style.Font.Bold = true;
                        cellFormat.Style.Font.Size = 14;
                        cellFormat.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cellFormat.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        cellFormat.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                        excelCommonFunctions.CreateTableHeader(dtTraceabilityMtrx, traceabilityMatrixWs, ref colIndex, ref rowIndex, "TraceabilityMatrix");


                        traceabilityMatrixWs.View.ShowGridLines = false;
                        traceabilityMatrixWs.View.ZoomScale = 80;
                        traceabilityMatrixWs.Cells.AutoFitColumns();


                        tbl_DesignAccelerator da = new tbl_DesignAccelerator();
                        DAManager daManager = new DAManager();
                        da = daManager.FindDA(daId);


                        tbl_Transactions transactions = new tbl_Transactions();

                        TransactionsManager transactionManager = new TransactionsManager();

                        ExcelWorksheet ws = excelCommonFunctions.OpenSheet(objExcelPackage, "Design Document");



                        int rowIndexTD = 5;
                        int colIndexTD = 2;

                        for (int i = 5; i <= ws.Dimension.Rows; i++)
                        {

                            var scenarioNo = ws.Cells[i, 3].Text;
                            var conditionNo = ws.Cells[i, 5].Text;
                            var gapReference = ws.Cells[i, 10].Text;
                            var functionality = ws.Cells[i, 9].Text;
                            var testCaseNo = ws.Cells[i, 11].Text;

                            string transactionid = conditionNo.Substring(0, conditionNo.Length - 7);

                            transactions = transactionManager.FindReqRefAndHLT(transactionid, daId);

                            string ReqReference = "";
                            if (transactions != null)
                            {
                                ReqReference = transactions.ReqReference;
                            }

                            AddTraceabilityMatrixData(daId, traceabilityMatrixWs, scenarioNo, conditionNo, functionality, gapReference, testCaseNo,
                             ReqReference, rowIndexTD, colIndexTD, transactionid, transactionMatrixSheets, ruleReferences);

                            rowIndexTD++;
                        }


                        #endregion

                        int csRowIndex = 5;
                        int csColIndex = 2;

                        int summaryRowIndex = 5;
                        int summaryColIndex = 2;

                        int rowIndexForgrandTotal = 0;
                        foreach (var trans in testDesignVM.lstHighLevelTransactions)
                        {
                            AddDataInControlSheet(transactionMatrixSheets, controlSheet, ruleReferences, csRowIndex, csColIndex, trans.HighLevelTxnDesc, traceabilityMatrixWs);
                            csRowIndex = totalCount;

                            AddDataInSummarySheet(transactionMatrixSheets, trans.HighLevelTxnDesc, summarySheet, summaryRowIndex, summaryColIndex);
                            summaryRowIndex++;

                            rowIndexForgrandTotal = summaryRowIndex;
                        }
                        AddGrandTotalData(summarySheet, rowIndexForgrandTotal, summaryColIndex);

                        DataTable dtTraceabilityMatrix = new DataTable();
                        int tColIndex = 2, tRowIndex = 1;
                        dtTraceabilityMatrix = GetTraceabilityMatrixData(traceabilityMatrixWs, ref tColIndex, ref tRowIndex);

                        DataTable dtcs = new DataTable();
                        int csColindex = 2, csRowindex = 4;
                        dtcs = GetTraceabilityMatrixData(controlSheet, ref csColindex, ref csRowindex);

                        CompareData(dtTraceabilityMatrix, dtcs, controlSheet, ruleReferences);


                        //formatting the traceability matrix file sheet
                        traceabilityMatrixWs.Column(2).Width = 22;
                        traceabilityMatrixWs.Column(3).Width = 15;
                        traceabilityMatrixWs.Column(4).Width = 15;
                        traceabilityMatrixWs.Column(5).Width = 20;
                        traceabilityMatrixWs.Column(6).Width = 20;
                        traceabilityMatrixWs.Column(7).Width = 15;
                        traceabilityMatrixWs.Column(8).Width = 40;
                        traceabilityMatrixWs.Column(9).Width = 40;
                        traceabilityMatrixWs.Column(10).Width = 40;
                        traceabilityMatrixWs.Column(11).Width = 40;

                        //formatting of control sheet
                        controlSheet.Column(2).Width = 15;
                        controlSheet.Column(3).Width = 20;
                        controlSheet.Column(4).Width = 10;

                        //formatting of summary sheet
                        summarySheet.Column(2).Width = 15;
                        summarySheet.Column(3).Width = 30;
                        summarySheet.Column(4).Width = 10;

                        string filePath = excelCommonFunctions.SaveFile(package, da.daName, "", "TraceabilityMatrix");

                        return filePath;
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddTraceabilityMatrixData(int daid, ExcelWorksheet traceabilityWs, string scnNo, string condNo, string functionality,
            string gapRef, string testCaseNo, string ReqReference, int rowIndex, int colIndex, string transactionId,
            DataSet transactionMatrixSheets, Dictionary<string, List<DataTable>> ruleReferences)
        {
            try
            {

                DataTable transactionMatrixSheet = new DataTable();
                StringBuilder businesstableRuleDesc = new StringBuilder();
                StringBuilder interfaceRuleDesc = new StringBuilder();
                StringBuilder channelsAndAlertsRuleDesc = new StringBuilder();
                StringBuilder reportsRuleDesc = new StringBuilder();


                transactionMatrixSheet = transactionMatrixSheets.Tables[functionality];

                if (transactionMatrixSheet != null)
                {


                    DataRow[] result = transactionMatrixSheet.Select("[Test Condition ID] = '" + condNo + "'");

                    #region for traceability matrix sheet

                    if (result.Length > 0)
                    {
                        foreach (DataRow dr in result)
                        {


                            if (dr["Business Rule ID"].ToString() != "")
                            {
                                {


                                    List<DataTable> value = new List<DataTable>();
                                    ruleReferences.TryGetValue(functionality, out value);//getting the key and value for rule reference table

                                    string[] BusinessRuleIds = dr["Business Rule ID"].ToString().Split(' ', '\n', '\t');//To split multiple ids that come under the same cell

                                    var conditionNo = dr["Test Condition ID"].ToString();
                                    IEnumerable<DataTable> lastTables = new List<DataTable>();
                                    lastTables = value.Where(e => e.TableName == "Business Rules - " + functionality);//under list of tables
                                    DataTable businessRuleTable = new DataTable();//we get a datatable, and because of above condition, we get datatable at 0th index,
                                    businessRuleTable = lastTables.ElementAt(0);//getting the table from that index

                                    DataRow[] dRow = businessRuleTable.Select("[Test_Cond_ID] = '" + conditionNo + "'");//fetching the row based on the test condition id
                                    if (dRow.Length > 0)
                                    {
                                        foreach (DataRow dataRow in dRow)//looping the fetched row based on the test condition id
                                        {


                                            foreach (var businessRule in BusinessRuleIds)
                                            {
                                                if (dataRow["Business_Rule_ID"].ToString() != "" && businessRule == dataRow["Business_Rule_ID"].ToString() && conditionNo.ToString() == condNo)
                                                {
                                                    businesstableRuleDesc.Append(dataRow.Table.Columns["Business_Rule_ID"].ColumnName + ": " + dataRow["Business_Rule_ID"].ToString());
                                                    businesstableRuleDesc.AppendLine();
                                                    businesstableRuleDesc.Append(dataRow.Table.Columns["Rule Description"].ColumnName + ": " + dataRow["Rule Description"].ToString());

                                                    businesstableRuleDesc.AppendLine();


                                                }

                                            }


                                        }

                                    }



                                }
                            }

                            if (dr["Interface ID"].ToString() != "")
                            {
                                {
                                    List<DataTable> value = new List<DataTable>();
                                    ruleReferences.TryGetValue(functionality, out value);//getting the key and value for rule reference table

                                    string[] BusinessRuleIds = dr["Interface ID"].ToString().Split(' ', '\n', '\t');//To split multiple ids that come under the same cell

                                    var conditionNo = dr["Test Condition ID"].ToString();
                                    IEnumerable<DataTable> lastTables = new List<DataTable>();
                                    lastTables = value.Where(e => e.TableName == "Interface - " + functionality);//under list of tables
                                    DataTable businessRuleTable = new DataTable();//we get a datatable, and because of above condition, we get datatable at 0th index,
                                    businessRuleTable = lastTables.ElementAt(0);//getting the table from that index

                                    DataRow[] dRow = businessRuleTable.Select("[Test_Cond_ID] = '" + conditionNo + "'");//fetching the row based on the test condition id
                                    if (dRow.Length > 0)
                                    {
                                        foreach (DataRow dataRow in dRow)//looping the fetched row based on the test condition id
                                        {

                                            foreach (var interfaceId in BusinessRuleIds)
                                            {
                                                if (dataRow["Interface_ID"].ToString() != "" && interfaceId == dataRow["Interface_ID"].ToString() && conditionNo.ToString() == condNo)
                                                {
                                                    interfaceRuleDesc.Append(dataRow.Table.Columns["Interface_ID"].ColumnName + ": " + dataRow["Interface_ID"].ToString());
                                                    interfaceRuleDesc.AppendLine();
                                                    interfaceRuleDesc.Append(dataRow.Table.Columns["Interface Description"].ColumnName + ": " + dataRow["Interface Description"].ToString());

                                                    interfaceRuleDesc.AppendLine();


                                                }

                                            }


                                        }

                                    }



                                }
                            }
                            if (dr["Channel Alert ID"].ToString() != "")
                            {
                                {
                                    List<DataTable> value = new List<DataTable>();
                                    ruleReferences.TryGetValue(functionality, out value);//getting the key and value for rule reference table

                                    string[] BusinessRuleIds = dr["Channel Alert ID"].ToString().Split(' ', '\n', '\t');//To split multiple ids that come under the same cell

                                    var conditionNo = dr["Test Condition ID"].ToString();
                                    IEnumerable<DataTable> lastTables = new List<DataTable>();
                                    lastTables = value.Where(e => e.TableName == "Channels and Alerts - " + functionality);//under list of tables
                                    DataTable businessRuleTable = new DataTable();//we get a datatable, and because of above condition, we get datatable at 0th index,
                                    businessRuleTable = lastTables.ElementAt(0);//getting the table from that index

                                    DataRow[] dRow = businessRuleTable.Select("[Test_Cond_ID] = '" + conditionNo + "'");//fetching the row based on the test condition id
                                    if (dRow.Length > 0)
                                    {
                                        foreach (DataRow dataRow in dRow)//looping the fetched row based on the test condition id
                                        {

                                            foreach (var interfaceId in BusinessRuleIds)
                                            {
                                                if (dataRow["ChannelAlert_ID"].ToString() != "" && interfaceId == dataRow["ChannelAlert_ID"].ToString() && conditionNo.ToString() == condNo)
                                                {
                                                    channelsAndAlertsRuleDesc.Append(dataRow.Table.Columns["ChannelAlert_ID"].ColumnName + ": " + dataRow["ChannelAlert_ID"].ToString());
                                                    channelsAndAlertsRuleDesc.AppendLine();
                                                    channelsAndAlertsRuleDesc.Append(dataRow.Table.Columns["Message Description"].ColumnName + ": " + dataRow["Message Description"].ToString());

                                                    channelsAndAlertsRuleDesc.AppendLine();


                                                }

                                            }


                                        }

                                    }



                                }
                            }
                            if (dr["Report ID"].ToString() != "")
                            {
                                List<DataTable> value = new List<DataTable>();
                                ruleReferences.TryGetValue(functionality, out value);
                                string[] reportIds = dr["Report ID"].ToString().Split(' ', '\n', '\t');

                                var conditionNo = dr["Test Condition ID"].ToString();
                                IEnumerable<DataTable> lastTables = new List<DataTable>();
                                lastTables = value.Where(e => e.TableName == "Reports - " + functionality);
                                DataTable businessRuleTable = new DataTable();
                                businessRuleTable = lastTables.ElementAt(0);
                                DataRow[] dRow = businessRuleTable.Select("[Test_Cond_ID] = '" + conditionNo + "'");//fetching the row based on the test condition id
                                if (dRow.Length > 0)
                                {
                                    foreach (DataRow dataRow in dRow)//looping the fetched row based on the test condition id
                                    {
                                        foreach (var reportid in reportIds)
                                        {

                                            if (dataRow["Report_ID"].ToString() != "" && reportid == dataRow["Report_ID"].ToString() && conditionNo.ToString() == condNo)
                                            {

                                                reportsRuleDesc.Append(dataRow.Table.Columns["Report_ID"].ColumnName + ": " + dataRow["Report_ID"].ToString());
                                                reportsRuleDesc.AppendLine();






                                            }
                                        }
                                    }
                                }



                            }
                        }
                    }
                }
                #endregion

                //requirement reference
                var cell = traceabilityWs.Cells[rowIndex, colIndex];
                cell.Value = ReqReference;
                ScenarioBuilder.FormatCell(cell);
                cell.Style.WrapText = true;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                colIndex++;

                //Transaction id
                cell = traceabilityWs.Cells[rowIndex, colIndex];
                cell.Value = transactionId;
                ScenarioBuilder.FormatCell(cell);
                cell.Style.WrapText = true;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                colIndex++;

                // Scenario No.
                cell = traceabilityWs.Cells[rowIndex, colIndex];
                cell.Value = scnNo;
                ScenarioBuilder.FormatCell(cell);
                cell.Style.WrapText = true;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                colIndex++;

                //Test Condition No.
                cell = traceabilityWs.Cells[rowIndex, colIndex];
                cell.Value = condNo;
                ScenarioBuilder.FormatCell(cell);
                cell.Style.WrapText = true;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                colIndex++;

                //test case No.
                cell = traceabilityWs.Cells[rowIndex, colIndex];
                cell.Value = testCaseNo;
                ScenarioBuilder.FormatCell(cell);
                cell.Style.WrapText = true;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                colIndex++;

                //gap reference
                cell = traceabilityWs.Cells[rowIndex, colIndex];
                cell.Value = gapRef;
                ScenarioBuilder.FormatCell(cell);
                cell.Style.WrapText = true;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                colIndex++;

                //business rule id
                cell = traceabilityWs.Cells[rowIndex, colIndex];
                cell.Value = businesstableRuleDesc.ToString();
                ScenarioBuilder.FormatCell(cell);
                cell.Style.WrapText = true;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                colIndex++;

                //interface id
                cell = traceabilityWs.Cells[rowIndex, colIndex];
                cell.Value = interfaceRuleDesc.ToString();
                ScenarioBuilder.FormatCell(cell);
                cell.Style.WrapText = true;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                colIndex++;


                //channels and alerts id
                cell = traceabilityWs.Cells[rowIndex, colIndex];
                cell.Value = channelsAndAlertsRuleDesc.ToString();
                ScenarioBuilder.FormatCell(cell);
                cell.Style.WrapText = true;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                colIndex++;

                //reports id
                cell = traceabilityWs.Cells[rowIndex, colIndex];
                cell.Value = reportsRuleDesc.ToString();
                ScenarioBuilder.FormatCell(cell);
                cell.Style.WrapText = true;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            }

            catch (Exception)
            {
                throw;
            }
        }

        private static void CreateHeadersForTraceabilityMatrix(ExcelWorksheet traceMtrxWs, out DataTable dtTraceabilityMtrx)
        {
            try
            {


                dtTraceabilityMtrx = new DataTable();


                dtTraceabilityMtrx.Columns.Add("Requirement Reference");
                dtTraceabilityMtrx.Columns.Add("Transaction ID");
                dtTraceabilityMtrx.Columns.Add("Scenario No.");
                dtTraceabilityMtrx.Columns.Add("Test Condition No.");
                dtTraceabilityMtrx.Columns.Add("Test case No.");
                dtTraceabilityMtrx.Columns.Add("Gap Reference");
                dtTraceabilityMtrx.Columns.Add("Business Rule ID");
                dtTraceabilityMtrx.Columns.Add("Interface ID");
                dtTraceabilityMtrx.Columns.Add("Channels and Alerts ID");
                dtTraceabilityMtrx.Columns.Add("Report ID");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void CreateHeadersForControlSheet(ExcelWorksheet controlSheetWs, out DataTable dtControlSheet)
        {
            try
            {


                dtControlSheet = new DataTable();

                dtControlSheet.Columns.Add("Transaction ID");
                dtControlSheet.Columns.Add("Conditions");
                dtControlSheet.Columns.Add("Coverage");

            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void CreateHeadersForSummarySheet(ExcelWorksheet summaryWs, out DataTable dtSummary)
        {
            try
            {


                dtSummary = new DataTable();

                dtSummary.Columns.Add("Transaction ID");
                dtSummary.Columns.Add("Transaction Name");
                dtSummary.Columns.Add("Count");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddDataInControlSheet(DataSet transactionMatrixSheets, ExcelWorksheet controlWs,
            Dictionary<string, List<DataTable>> ruleReferences, int csRowIndex, int csColIndex, string hlt, ExcelWorksheet traceabilityMatrixWs)
        {
            try
            {


                DataTable transactionMatrixSheet = new DataTable();
                string tempVariable = "";
                transactionMatrixSheet = transactionMatrixSheets.Tables[hlt];

                foreach (DataRow item in transactionMatrixSheet.Rows)
                {

                    //transaction id
                    var csCell = controlWs.Cells[csRowIndex, csColIndex];
                    csCell.Value = item["Transaction ID"].ToString();
                    tempVariable = csCell.Value.ToString();
                    ScenarioBuilder.FormatCell(csCell);
                    // csCell.Style.WrapText = true;
                    csCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    csCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    csColIndex++;

                    //conditions
                    csCell = controlWs.Cells[csRowIndex, csColIndex];
                    csCell.Value = item["Test Condition ID"].ToString();
                    ScenarioBuilder.FormatCell(csCell);
                    // csCell.Style.WrapText = true;
                    csCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    csCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    csColIndex++;

                    //coverage
                    csCell = controlWs.Cells[csRowIndex, csColIndex];
                    csCell.Value = "Yes";
                    ScenarioBuilder.FormatCell(csCell);
                    //  csCell.Style.WrapText = true;
                    csCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    csCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    csRowIndex++;
                    csColIndex = 2;
                    totalCount = csRowIndex;

                }

                List<DataTable> table = new List<DataTable>();
                ruleReferences.TryGetValue(hlt, out table);


                if (table != null)
                {
                    if (table.Count > 0)
                    {

                        foreach (DataTable dt in table)
                        {
                            foreach (DataRow item in dt.Rows)
                            {
                                //transaction id
                                var csCellLastTable = controlWs.Cells[csRowIndex, csColIndex];
                                csCellLastTable.Value = tempVariable;
                                ScenarioBuilder.FormatCell(csCellLastTable);
                                //  csCellLastTable.Style.WrapText = true;
                                csCellLastTable.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                csCellLastTable.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                csColIndex++;

                                if (dt.TableName == "Business Rules - " + hlt)
                                {
                                    csCellLastTable = controlWs.Cells[csRowIndex, csColIndex];
                                    csCellLastTable.Value = item["Business_Rule_ID"].ToString();
                                    ScenarioBuilder.FormatCell(csCellLastTable);
                                    //   csCellLastTable.Style.WrapText = true;
                                    csCellLastTable.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    csCellLastTable.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    csColIndex++;
                                }
                                else if (dt.TableName == "Interface - " + hlt)
                                {
                                    csCellLastTable = controlWs.Cells[csRowIndex, csColIndex];
                                    csCellLastTable.Value = item["Interface_ID"].ToString();
                                    ScenarioBuilder.FormatCell(csCellLastTable);
                                    //    csCellLastTable.Style.WrapText = true;
                                    csCellLastTable.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    csCellLastTable.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    csColIndex++;
                                }
                                else if (dt.TableName == "Channels and Alerts - " + hlt)
                                {
                                    csCellLastTable = controlWs.Cells[csRowIndex, csColIndex];
                                    csCellLastTable.Value = item["ChannelAlert_ID"].ToString();
                                    ScenarioBuilder.FormatCell(csCellLastTable);
                                    //   csCellLastTable.Style.WrapText = true;
                                    csCellLastTable.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    csCellLastTable.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    csColIndex++;
                                }
                                else if (dt.TableName == "Reports - " + hlt)
                                {
                                    csCellLastTable = controlWs.Cells[csRowIndex, csColIndex];
                                    csCellLastTable.Value = item["Report_ID"].ToString();
                                    ScenarioBuilder.FormatCell(csCellLastTable);
                                    //    csCellLastTable.Style.WrapText = true;
                                    csCellLastTable.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    csCellLastTable.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    csColIndex++;
                                }



                                csCellLastTable = controlWs.Cells[csRowIndex, csColIndex];
                                csCellLastTable.Value = "Yes";
                                ScenarioBuilder.FormatCell(csCellLastTable);
                                //    csCellLastTable.Style.WrapText = true;
                                csCellLastTable.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                csCellLastTable.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;


                                csRowIndex++;
                                csColIndex = 2;
                                totalCount = csRowIndex;
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

        public void AddDataInSummarySheet(DataSet transactionMatrixSheets, string hlt, ExcelWorksheet summarySheet, int summaryRowIndex, int summaryColIndex)
        {
            try
            {


                DataTable transactionMatrixSheet = new DataTable();

                transactionMatrixSheet = transactionMatrixSheets.Tables[hlt];

                string transactionId = transactionMatrixSheet.Rows[0]["Transaction ID"].ToString();


                //Transaction Id
                var summaryCell = summarySheet.Cells[summaryRowIndex, summaryColIndex];
                summaryCell.Value = transactionId;
                ScenarioBuilder.FormatCell(summaryCell);
                // summaryCell.Style.WrapText = true;
                summaryCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                summaryCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                summaryCell.AutoFitColumns();
                summaryColIndex++;

                //transaction name 
                summaryCell = summarySheet.Cells[summaryRowIndex, summaryColIndex];
                summaryCell.Value = hlt;
                ScenarioBuilder.FormatCell(summaryCell);
                //  summaryCell.Style.WrapText = true;
                summaryCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                summaryCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                summaryCell.AutoFitColumns();
                summaryColIndex++;

                //count
                summaryCell = summarySheet.Cells[summaryRowIndex, summaryColIndex];
                summaryCell.Value = transactionMatrixSheet.Rows.Count;
                ScenarioBuilder.FormatCell(summaryCell);
                // summaryCell.Style.WrapText = true;
                summaryCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                summaryCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                summaryCell.AutoFitColumns();
                summaryColIndex++;

                grandTotal = grandTotal + transactionMatrixSheet.Rows.Count;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddGrandTotalData(ExcelWorksheet summarySheet, int rowIndexForgrandTotal, int summaryColIndex)
        {
            try
            {



                //grand total
                var gtCell = summarySheet.Cells[rowIndexForgrandTotal, summaryColIndex, rowIndexForgrandTotal, summaryColIndex + 1];
                gtCell.Value = "Grand Total";
                gtCell.Merge = true;
                ScenarioBuilder.FormatCell(gtCell);
                //  gtCell.Style.WrapText = true;
                gtCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                gtCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                gtCell.AutoFitColumns();
                summaryColIndex = summaryColIndex + 2;// since the next row is merged, we need to increment row count by 2;

                //grand total count
                gtCell = summarySheet.Cells[rowIndexForgrandTotal, summaryColIndex];
                gtCell.Value = grandTotal;
                ScenarioBuilder.FormatCell(gtCell);
                // gtCell.Style.WrapText = true;
                gtCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                gtCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                gtCell.AutoFitColumns();
                summaryColIndex++;


            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetTraceabilityMatrixData(ExcelWorksheet workSheet, ref int colIndex, ref int rowIndex)
        {
            try
            {

                bool reachedEnd = false;
                DataTable dtRuleN = new DataTable();



                if (workSheet != null)
                {
                    Dictionary<string, int> header = new Dictionary<string, int>();

                    for (rowIndex = 4; rowIndex <= workSheet.Dimension.End.Row && reachedEnd == false; rowIndex++)
                    {
                        //Assume the first row is the header. Then use the column match ups by name to determine the index.
                        //This will allow you to have the order of the columns change without any affect.

                        if (rowIndex == 4)
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

                                dr[dtRuleN.Columns[k].ToString()] = ParseWorksheetValue(workSheet, header, rowIndex, dtRuleN.Columns[k].ToString());

                            }
                            if (!reachedEnd)
                                dtRuleN.Rows.Add(dr);

                        }
                    }
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

        public void CompareData(DataTable traceabilityMtrx, DataTable controlSheet, ExcelWorksheet csWs, Dictionary<string, List<DataTable>> ruleReferences)
        {
            try
            {
                int i = 0;
                string coverage = string.Empty;
                int csRow = 5, csCol = 4;

                foreach (var item in traceabilityMtrx.Rows)
                {
                    int controlSheetTotalCount = controlSheet.Rows.Count;

                    if (i < controlSheetTotalCount)
                    {
                        var condition = controlSheet.Rows[i]["Conditions"].ToString();

                        if (condition.StartsWith("BR"))
                        {
                            for (int brindex = 0; brindex < traceabilityMtrx.Rows.Count; brindex++)
                            {

                                var businessRuleRow = traceabilityMtrx.Rows[brindex]["Business Rule ID"].ToString();

                                if (businessRuleRow != "")
                                {
                                    string[] lines = businessRuleRow.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                                    var toRemove = lines.First();
                                    var businessRuleId = toRemove.Substring(toRemove.LastIndexOf(":") + 1).Trim();

                                    if (businessRuleId == condition)
                                    {
                                        var csCell = csWs.Cells[csRow, csCol];
                                        csCell.Value = "Yes";
                                        ScenarioBuilder.FormatCell(csCell);
                                        // csCell.Style.WrapText = true;
                                        csCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                        csCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;


                                        break;
                                    }
                                    else
                                    {
                                        var csCell = csWs.Cells[csRow, csCol];
                                        csCell.Value = "No";
                                        ScenarioBuilder.FormatCell(csCell);
                                        // csCell.Style.WrapText = true;
                                        csCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                        csCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    }

                                }

                            }

                        }
                        else if (condition.StartsWith("IN"))
                        {

                            for (int brindex = 0; brindex < traceabilityMtrx.Rows.Count; brindex++)
                            {
                                var businessRuleRow = traceabilityMtrx.Rows[brindex]["Interface ID"].ToString();

                                if (businessRuleRow != "")
                                {
                                    string[] lines = businessRuleRow.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                                    var toRemove = lines.First();
                                    var businessRuleId = toRemove.Substring(toRemove.LastIndexOf(":") + 1).Trim();

                                    if (businessRuleId == condition)
                                    {
                                        var csCell = csWs.Cells[csRow, csCol];
                                        csCell.Value = "Yes";
                                        ScenarioBuilder.FormatCell(csCell);
                                        // csCell.Style.WrapText = true;
                                        csCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                        csCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;


                                        break;
                                    }
                                    else
                                    {
                                        var csCell = csWs.Cells[csRow, csCol];
                                        csCell.Value = "No";
                                        ScenarioBuilder.FormatCell(csCell);
                                        // csCell.Style.WrapText = true;
                                        csCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                        csCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    }
                                }

                            }
                        }
                        else if (condition.StartsWith("CA"))
                        {
                            for (int brindex = 0; brindex < traceabilityMtrx.Rows.Count; brindex++)
                            {
                                var businessRuleRow = traceabilityMtrx.Rows[brindex]["Channels and Alerts ID"].ToString();

                                if (businessRuleRow != "")
                                {
                                    string[] lines = businessRuleRow.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                                    var toRemove = lines.First();
                                    var businessRuleId = toRemove.Substring(toRemove.LastIndexOf(":") + 1).Trim();

                                    if (businessRuleId == condition)
                                    {
                                        var csCell = csWs.Cells[csRow, csCol];
                                        csCell.Value = "Yes";
                                        ScenarioBuilder.FormatCell(csCell);
                                        // csCell.Style.WrapText = true;
                                        csCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                        csCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;


                                        break;
                                    }
                                    else
                                    {
                                        var csCell = csWs.Cells[csRow, csCol];
                                        csCell.Value = "No";
                                        ScenarioBuilder.FormatCell(csCell);
                                        // csCell.Style.WrapText = true;
                                        csCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                        csCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    }
                                }

                            }
                        }
                        else if (condition.StartsWith("RE"))
                        {
                            for (int brindex = 0; brindex < traceabilityMtrx.Rows.Count; brindex++)
                            {
                                var businessRuleRow = traceabilityMtrx.Rows[brindex]["Report ID"].ToString();

                                if (businessRuleRow != "")
                                {
                                    string[] lines = businessRuleRow.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                                    var toRemove = lines.First();
                                    var businessRuleId = toRemove.Substring(toRemove.LastIndexOf(":") + 1).Trim();

                                    if (businessRuleId == condition)
                                    {
                                        var csCell = csWs.Cells[csRow, csCol];
                                        csCell.Value = "Yes";
                                        ScenarioBuilder.FormatCell(csCell);
                                        // csCell.Style.WrapText = true;
                                        csCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                        csCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;


                                        break;
                                    }
                                    else
                                    {
                                        var csCell = csWs.Cells[csRow, csCol];
                                        csCell.Value = "No";
                                        ScenarioBuilder.FormatCell(csCell);
                                        // csCell.Style.WrapText = true;
                                        csCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                        csCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    }
                                }

                            }
                        }
                        else
                        {
                            DataRow[] dr = traceabilityMtrx.Select("[Test Condition No.] = '" + condition + "'");
                            if (dr.Length > 0)
                            {
                                var csCell = csWs.Cells[csRow, csCol];
                                csCell.Value = "Yes";
                                ScenarioBuilder.FormatCell(csCell);
                                // csCell.Style.WrapText = true;
                                csCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                csCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;



                            }
                            else
                            {
                                var csCell = csWs.Cells[csRow, csCol];
                                csCell.Value = "No";
                                ScenarioBuilder.FormatCell(csCell);
                                //   csCell.Style.WrapText = true;
                                csCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                csCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            }
                        }

                        i++;
                        csRow++;
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