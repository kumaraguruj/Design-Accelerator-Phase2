using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.IO;
using DA.BusinessLayer;
using DA.DomainModel;
using DesignAccelerator.Models.ViewModel;
using System.Text;
using OfficeOpenXml.Table;
using System.Reflection;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Threading.Tasks;

namespace DesignAccelerator.Controllers
{
    public class TestDesignController
    {
        DataTable dtTD; //TestDesign
        public string txID = "";
        ScenarioBuilderViewModel testDesignVM = new ScenarioBuilderViewModel();

        MappingViewModel mappingViewModel = new MappingViewModel();
        IList<MappingViewModel> lstMappingViewModel = new List<MappingViewModel>();

        ExcelCommonFunctions excelCommonFunctions = new ExcelCommonFunctions();

        // GET: TestDesign
        public async Task<string> GenerateTestDesign(int daId, HttpPostedFileBase txmPath, HttpPostedFileBase ScbPath)
        {
            try
            {


                Dictionary<string, List<TestScenarioMembers>> testScenarios = new Dictionary<string, List<TestScenarioMembers>>();

                IList<tbl_Attribute> lstAttributes = new List<tbl_Attribute>();

                TransactionMatrix transactionMatrix = new TransactionMatrix();

                testDesignVM.lstHighLevelTransactions = testDesignVM.GetTransactionsList(daId).lstTransactions;

                ModuleViewModel moduleViewModel = new ModuleViewModel();

                int colIndex = 1, rowIndex = 0;

                //Create new workbook for Test Design
                using (var package = new ExcelPackage())
                {
                    ExcelWorksheet ws2 = package.Workbook.Worksheets.Add("Design Document");

                    CreateHeadersForTD(ws2, out dtTD);

                    //Read data from Test Scenario
                    using (var objExcelPackage = new ExcelPackage(ScbPath.InputStream))
                    {
                        //string scenarioId = "";
                        //string testConditionId = "";

                        ExcelWorksheet ws = excelCommonFunctions.OpenSheet(objExcelPackage, "Test Scenarios");

                        testScenarios = GetDataTableFromExcel(ScbPath);

                    }

                    //Read data from RuleOfNData
                    DataSet transactionMatrixSheets = new DataSet();

                    //list of the rule reference datatables will come into this list
                    List<DataTable> dtRuleOfNforTestDesignRuleReference = new List<DataTable>();

                    //Dictionary to add the fetched last tables from tm into dtRuleOFNForTestDesignRuleReference and pass as a parameter to addTestDesign method.
                    Dictionary<string, List<DataTable>> ruleReferences = new Dictionary<string, List<DataTable>>();

                    using (var objExcelPackage = new ExcelPackage(txmPath.InputStream))
                    {
                        foreach (var trans in testDesignVM.lstHighLevelTransactions)
                        {
                            int colIdex = 1, rowIdex = 0;
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

                    tbl_DesignAccelerator da = new tbl_DesignAccelerator();
                    DAManager daManager = new DAManager();

                    da = daManager.FindDA(daId);

                    var modId = da.ModuleId;

                    tbl_Module daModule = new tbl_Module();
                    ModuleManager modManager = new ModuleManager();
                    daModule = modManager.FindModuleNameForRunPlan(modId);
                    var moduleName = daModule.ModuleName;

                    string tblName4 = moduleName + " - Test Design Document";
                    ws2.Cells[2, 1].Value = tblName4;

                    //// Format Excel Sheet
                    ws2.Cells[2, 1, 2, 5].Merge = true; //Merge columns start and end range
                    ws2.Cells[2, 1, 2, 5].Style.Font.Bold = true; //Font should be bold
                    ws2.Cells[2, 1, 2, 5].Style.Font.Size = 14;
                    ws2.Cells[2, 1, 2, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; // Alignment is Left
                    ws2.Cells[2, 1, 2, 5].Style.Fill.PatternType = ExcelFillStyle.Solid; // Border
                    ws2.Cells[2, 1, 2, 5].Style.Fill.BackgroundColor.SetColor(Color.LightGray); // Background Color

                    excelCommonFunctions.CreateTableHeader(dtTD, ws2, ref colIndex, ref rowIndex, "tbl4");
                    ws2.Cells["A4:N4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    AddTestDesign(testScenarios, transactionMatrixSheets, ws2, colIndex, rowIndex, dtTD, testDesignVM.lstHighLevelTransactions, ruleReferences);

                    //Format Excel Sheet
                    ws2.View.ShowGridLines = false;
                    ws2.View.ZoomScale = 80;
                    ws2.Cells.AutoFitColumns();
                    ws2.Column(4).Width = 50;
                    ws2.Column(12).Width = 75;
                    ws2.Column(13).Width = 75;
                    ws2.Column(14).Width = 75;

                    string filePath = excelCommonFunctions.SaveFile(package, da.daName, "", "TD");

                    return filePath;

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void CreateHeadersForTD(ExcelWorksheet ws2, out DataTable dtTD)
        {
            try
            {


                dtTD = new DataTable();

                dtTD.Columns.Add("S.No");
                dtTD.Columns.Add("Rule Reference");
                dtTD.Columns.Add("Scenario No.");
                dtTD.Columns.Add("Scenario Description");
                dtTD.Columns.Add("Condition No.");
                dtTD.Columns.Add("Logical Day");
                dtTD.Columns.Add("Batch Frequency");
                dtTD.Columns.Add("Pre-Requisites");
                dtTD.Columns.Add("Functionality");
                dtTD.Columns.Add("Gap Reference");
                dtTD.Columns.Add("Test Case No");
                dtTD.Columns.Add("Test Case Description");
                dtTD.Columns.Add("Test Data");
                dtTD.Columns.Add("Expected Result");

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>Converts a Worksheet data to DataTable in Dictionary</summary>
        /// <param name="path"></param> <param1 name="boolean"></param>
        /// <returns></returns>
        private Dictionary<string, List<TestScenarioMembers>> GetDataTableFromExcel(HttpPostedFileBase tScPath)
        {
            try
            {


                Dictionary<string, List<TestScenarioMembers>> testScenarios = new Dictionary<string, List<TestScenarioMembers>>();
                using (var pck = new ExcelPackage())
                {
                    using (var stream = tScPath.InputStream)
                    {
                        pck.Load(stream);
                    }

                    //Get the worksheet in the workbook 
                    var ws = pck.Workbook.Worksheets[2];

                    int conditionNoCell = 3;
                    int conditionResultCell = 4;
                    //loops through the cell values from row 4 and ConditionNo cell
                    foreach (var firstRowCell in ws.Cells[4, 3, 4, ws.Dimension.End.Column])
                    {
                        if (firstRowCell.Text != "" && !firstRowCell.Text.Contains("tempColumn"))
                        {
                            string HeaderName = firstRowCell.Text;
                            for (int rowNum = 6; rowNum <= ws.Dimension.End.Row; rowNum++)
                            {
                                string scenarioNo = ws.GetValue(rowNum, 2).ToString();
                                string conditionNo = ws.GetValue(rowNum, conditionNoCell) == null ? "" : ws.GetValue(rowNum, conditionNoCell).ToString();
                                string conditionResult = ws.GetValue(rowNum, conditionResultCell) == null ? "" : ws.GetValue(rowNum, conditionResultCell).ToString();
                                TestScenarioMembers testScenarioMembers = new TestScenarioMembers { ScenarioNo = scenarioNo, ConditionNo = conditionNo, ConditionResult = conditionResult };

                                if (testScenarios.ContainsKey(HeaderName) && testScenarios[HeaderName]!=null)
                                {
                                    testScenarios[HeaderName].Add(testScenarioMembers);

                                }
                                else
                                {
                                    List<TestScenarioMembers> lstScenarioBuilderMembers = new List<TestScenarioMembers>();
                                    lstScenarioBuilderMembers.Add(testScenarioMembers);
                                    testScenarios.Add(HeaderName, lstScenarioBuilderMembers);
                                }
                            }

                            conditionNoCell = conditionNoCell + 2;

                            conditionResultCell = conditionResultCell + 2;
                        }
                    }
                    return testScenarios;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddTestDesign(Dictionary<string, List<TestScenarioMembers>> testScenarios, DataSet transactionMatrixSheets,
            ExcelWorksheet ws, int colIndex, int rowIndex, DataTable dtTD, IList<tbl_Transactions> lstHighLevelTransactions, Dictionary<string, List<DataTable>> ruleReferences)
        {
            try
            {


                rowIndex++;
                int testScenarioIndex = 0;// for scenario description
                for (int j = 1; j <= testScenarios.ElementAt(0).Value.Count; j++)
                {

                    foreach (var trans in lstHighLevelTransactions)
                    {
                        colIndex = 1;
                        DataTable transactionMatrixSheet = new DataTable();
                        DataTable temp = new DataTable();
                        StringBuilder strBuilder = new StringBuilder();
                        StringBuilder strAttr = new StringBuilder();
                        StringBuilder ruleReferenceBuilder = new StringBuilder();
                        var dictValues = testScenarios[trans.HighLevelTxnDesc];
                        TestScenarioMembers item = new TestScenarioMembers();
                        int i = 0;
                        foreach (var element in dictValues)
                        {
                            if (i == (j - 1))
                            {
                                if (element.Status == "" && element.ConditionNo != "" && element.ConditionResult != "")
                                {
                                    item = element;
                                    break;
                                }
                            }
                            i++;
                        }

                        if (item != null && item.ConditionResult != null && item.ConditionNo != "" && item.ConditionResult != "")
                        {
                            item.Status = "Processed";
                            var cell = ws.Cells[rowIndex, colIndex];
                            //Serial No.
                            cell.Value = j;
                            ScenarioBuilder.FormatCell(cell);
                            //cell.Style.WrapText = true;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;


                            transactionMatrixSheet = transactionMatrixSheets.Tables[trans.HighLevelTxnDesc];
                            DataRow[] result = transactionMatrixSheet.Select("[Test Condition ID] = '" + item.ConditionNo + "'");



                            string logicalDay = "";
                            string batchFreq = "";
                            string gapReference = "";
                            if (result.Length > 0)
                            {
                                foreach (DataRow dr in result)
                                {
                                    //mandatorily to be in all the fields of test case description
                                    strBuilder.Append("To check " + trans.HighLevelTxnDesc + " with revelant test data");
                                    strBuilder.AppendLine();

                                    if (dr["Business Rule ID"].ToString() != "")
                                    {
                                        List<DataTable> value = new List<DataTable>();
                                        ruleReferences.TryGetValue(trans.HighLevelTxnDesc, out value);//getting the key and value for rule reference table

                                        string[] BusinessRuleIds = dr["Business Rule ID"].ToString().Split(' ','\n','\t');//To split multiple ids that come under the same cell

                                        var conditionNo = dr["Test Condition ID"].ToString();
                                        IEnumerable<DataTable> lastTables = new List<DataTable>();
                                        lastTables = value.Where(e => e.TableName == "Business Rules - " + trans.HighLevelTxnDesc);//under list of tables
                                        DataTable businessRuleTable = new DataTable();//we get a datatable, and because of above condition, we get datatable at 0th index,
                                        businessRuleTable = lastTables.ElementAt(0);//getting the table from that index
                                        DataRow[] dRow = businessRuleTable.Select("[Test_Cond_ID] = '" + conditionNo + "'");//fetching the row based on the test condition id
                                        if (dRow.Length > 0)
                                        {
                                            foreach (DataRow dataRow in dRow)//looping the fetched row based on the test condition id
                                            {

                                                foreach (var businessRule in BusinessRuleIds)
                                                {
                                                    
                                                    if (dataRow["Business_Rule_ID"].ToString() != "" && businessRule == dataRow["Business_Rule_ID"].ToString() && conditionNo.ToString() == item.ConditionNo)
                                                    {


                                                        strBuilder.Append(dataRow.Table.Columns["Business_Rule_ID"].ColumnName + ": " + dataRow["Business_Rule_ID"].ToString());
                                                        strBuilder.AppendLine();
                                                        strBuilder.Append(dataRow.Table.Columns["Rule Description"].ColumnName + ": " + dataRow["Rule Description"].ToString());

                                                        strBuilder.AppendLine();


                                                        ruleReferenceBuilder.Append(dataRow["Business_Rule_ID"].ToString());
                                                        ruleReferenceBuilder.AppendLine();

                                                    }

                                                }


                                            }

                                        }



                                    }

                                    if (dr["Interface ID"].ToString() != "")
                                    {
                                        List<DataTable> value = new List<DataTable>();
                                        ruleReferences.TryGetValue(trans.HighLevelTxnDesc, out value);

                                        string[] interfaceIds = dr["Interface ID"].ToString().Split(' ', '\n', '\t');

                                        var conditionNo = dr["Test Condition ID"].ToString();
                                        IEnumerable<DataTable> lastTables = new List<DataTable>();
                                        lastTables = value.Where(e => e.TableName == "Interface - " + trans.HighLevelTxnDesc);
                                        DataTable businessRuleTable = new DataTable();
                                        businessRuleTable = lastTables.ElementAt(0);
                                        DataRow[] dRow = businessRuleTable.Select("[Test_Cond_ID] = '" + conditionNo + "'");//fetching the row based on the test condition id
                                        if (dRow.Length > 0)
                                        {
                                            foreach (DataRow dataRow in dRow)//looping the fetched row based on the test condition id
                                            {
                                                foreach (var interfaceId in interfaceIds)
                                                {

                                                    if (dataRow["Interface_ID"].ToString() != "" && interfaceId == dataRow["Interface_ID"].ToString() && conditionNo.ToString() == item.ConditionNo)
                                                    {

                                                        strBuilder.Append(dataRow.Table.Columns["Interface_ID"].ColumnName + ": " + dataRow["Interface_ID"].ToString());
                                                        strBuilder.AppendLine();
                                                        strBuilder.Append(dataRow.Table.Columns["Interface Description"].ColumnName + ": " + dataRow["Interface Description"].ToString());

                                                        strBuilder.AppendLine();


                                                        ruleReferenceBuilder.Append(dataRow["Interface_ID"].ToString());
                                                        ruleReferenceBuilder.AppendLine();

                                                    }

                                                }
                                            }
                                        }


                                    }
                                    if (dr["Channel Alert ID"].ToString() != "")
                                    {
                                        List<DataTable> value = new List<DataTable>();
                                        ruleReferences.TryGetValue(trans.HighLevelTxnDesc, out value);

                                        string[] channelAlertIds = dr["Channel Alert ID"].ToString().Split(' ', '\n', '\t');
                                        var conditionNo = dr["Test Condition ID"].ToString();
                                        IEnumerable<DataTable> lastTables = new List<DataTable>();
                                        lastTables = value.Where(e => e.TableName == "Channels and Alerts - " + trans.HighLevelTxnDesc);
                                        DataTable businessRuleTable = new DataTable();
                                        businessRuleTable = lastTables.ElementAt(0);
                                        DataRow[] dRow = businessRuleTable.Select("[Test_Cond_ID] = '" + conditionNo + "'");//fetching the row based on the test condition id
                                        if (dRow.Length > 0)
                                        {
                                            foreach (DataRow dataRow in dRow)//looping the fetched row based on the test condition id
                                            {
                                                foreach (var channelalertId in channelAlertIds)
                                                {

                                                    if (dataRow["ChannelAlert_ID"].ToString() != "" && channelalertId == dataRow["ChannelAlert_ID"].ToString() && conditionNo.ToString() == item.ConditionNo)
                                                    {


                                                        strBuilder.Append(dataRow.Table.Columns["ChannelAlert_ID"].ColumnName + ": " + dataRow["ChannelAlert_ID"].ToString());
                                                        strBuilder.AppendLine();
                                                        strBuilder.Append(dataRow.Table.Columns["Message Description"].ColumnName + ": " + dataRow["Message Description"].ToString());

                                                        strBuilder.AppendLine();

                                                        ruleReferenceBuilder.Append(dataRow["ChannelAlert_ID"].ToString());
                                                        ruleReferenceBuilder.AppendLine();

                                                    }
                                                }
                                            }
                                        }



                                    }
                                    if (dr["Report ID"].ToString() != "")
                                    {
                                        List<DataTable> value = new List<DataTable>();
                                        ruleReferences.TryGetValue(trans.HighLevelTxnDesc, out value);
                                        string[] reportIds = dr["Report ID"].ToString().Split(' ', '\n', '\t');

                                        var conditionNo = dr["Test Condition ID"].ToString();
                                        IEnumerable<DataTable> lastTables = new List<DataTable>();
                                        lastTables = value.Where(e => e.TableName == "Reports - " + trans.HighLevelTxnDesc);
                                        DataTable businessRuleTable = new DataTable();
                                        businessRuleTable = lastTables.ElementAt(0);
                                        DataRow[] dRow = businessRuleTable.Select("[Test_Cond_ID] = '" + conditionNo + "'");//fetching the row based on the test condition id
                                        if (dRow.Length > 0)
                                        {
                                            foreach (DataRow dataRow in dRow)//looping the fetched row based on the test condition id
                                            {
                                                foreach (var reportid in reportIds)
                                                {

                                                    if (dataRow["Report_ID"].ToString() != "" && reportid == dataRow["Report_ID"].ToString() && conditionNo.ToString() == item.ConditionNo)
                                                    {

                                                        strBuilder.Append(dataRow.Table.Columns["Report_ID"].ColumnName + ": " + dataRow["Report_ID"].ToString());
                                                        strBuilder.AppendLine();

                                                        ruleReferenceBuilder.Append(dataRow["Report_ID"].ToString());
                                                        ruleReferenceBuilder.AppendLine();

                                                    }
                                                }
                                            }
                                        }



                                    }

                                    //Validations to be fetched based on the test condition id
                                    if (dr["Business Rule ID"].ToString() != "" || dr["Interface ID"].ToString() != ""
                                         || dr["Channel Alert ID"].ToString() != "" || dr["Report ID"].ToString() != "")
                                    {
                                        //since we need noth column name and value as well
                                        var validation1 = dr.Table.Columns["Validation 1"].ColumnName;
                                        var validation1data = dr["Validation 1"].ToString();

                                        var validation2 = dr.Table.Columns["Validation 2"].ColumnName;
                                        var validation2data = dr["Validation 2"].ToString();

                                        var validation3 = dr.Table.Columns["Validation 3"].ColumnName;
                                        var validation3data = dr["Validation 3"].ToString();

                                        var validation4 = dr.Table.Columns["Validation 4"].ColumnName;
                                        var validation4data = dr["Validation 4"].ToString();

                                        var validation5 = dr.Table.Columns["Validation 5"].ColumnName;
                                        var validation5data = dr["Validation 5"].ToString();

                                        //if value is empty, then do not write the column name and the value
                                        if (validation1data != "")
                                        {
                                            strBuilder.Append(validation1 + ": " + validation1data);
                                            strBuilder.AppendLine();
                                        }

                                        if (validation2data != "")
                                        {
                                            strBuilder.Append(validation2 + ": " + validation2data);
                                            strBuilder.AppendLine();
                                        }

                                        if (validation3data != "")
                                        {
                                            strBuilder.Append(validation3 + ": " + validation3data);
                                            strBuilder.AppendLine();
                                        }

                                        if (validation4data != "")
                                        {
                                            strBuilder.Append(validation4 + ": " + validation4data);
                                            strBuilder.AppendLine();
                                        }

                                        if (validation5data != "")
                                        {
                                            strBuilder.Append(validation5 + ": " + validation5data);
                                            strBuilder.AppendLine();
                                        }
                                    }


                                    logicalDay = dr["Logical Day"].ToString();
                                    batchFreq = dr["Batch Frequency"].ToString();
                                    gapReference = dr["Gap Reference"].ToString();




                                    for (int tranCol = 3; tranCol < dr.Table.Columns.IndexOf("Test Condition Result"); tranCol++)
                                    {
                                        strAttr.Append(dr.Table.Columns[tranCol].ColumnName + ": " + dr[tranCol].ToString());
                                        strAttr.AppendLine();
                                    }
                                    break;

                                }

                            }

                            //Rule reference
                            colIndex++;
                            cell = ws.Cells[rowIndex, colIndex];
                            cell.Value = ruleReferenceBuilder.ToString();
                            ScenarioBuilder.FormatCell(cell);
                            cell.Style.WrapText = true;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            colIndex++;
                            //Scenario No.
                            string scenarioNo = item.ScenarioNo;
                            scenarioNo = scenarioNo.Replace("AT", "SL");
                            cell = ws.Cells[rowIndex, colIndex];
                            cell.Value = scenarioNo;
                            ScenarioBuilder.FormatCell(cell);
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.AutoFitColumns();
                            colIndex++;

                            //Scenario Description

                            string scenarioDesc = GetScenarioDescription(testScenarios, lstHighLevelTransactions, testScenarioIndex);
                            cell = ws.Cells[rowIndex, colIndex];
                            cell.Value = scenarioDesc;
                            ScenarioBuilder.FormatCell(cell);
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.WrapText = true;
                            colIndex++;


                            //Condition No
                            cell = ws.Cells[rowIndex, colIndex];
                            cell.Value = item.ConditionNo;
                            ScenarioBuilder.FormatCell(cell);
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.AutoFitColumns();
                            colIndex++;

                            //Logical Day
                            cell = ws.Cells[rowIndex, colIndex];
                            cell.Value = logicalDay;
                            ScenarioBuilder.FormatCell(cell);
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.AutoFitColumns();
                            colIndex++;

                            //Batch Frequency
                            cell = ws.Cells[rowIndex, colIndex];
                            cell.Value = batchFreq;
                            ScenarioBuilder.FormatCell(cell);
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.AutoFitColumns();
                            colIndex++;

                            //Pre-Requisites
                            cell = ws.Cells[rowIndex, colIndex];
                            cell.Value = "";
                            ScenarioBuilder.FormatCell(cell);
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.AutoFitColumns();
                            colIndex++;

                            //Functionality
                            cell = ws.Cells[rowIndex, colIndex];
                            cell.Value = trans.HighLevelTxnDesc;
                            ScenarioBuilder.FormatCell(cell);
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.AutoFitColumns();
                            colIndex++;

                            //Gap Reference
                            cell = ws.Cells[rowIndex, colIndex];
                            cell.Value = gapReference;
                            ScenarioBuilder.FormatCell(cell);
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.AutoFitColumns();
                            colIndex++;

                            //Test Case No.
                            var condNo = item.ConditionNo.Trim().Substring(item.ConditionNo.Trim().LastIndexOf("TC"));
                            cell = ws.Cells[rowIndex, colIndex];
                            cell.Value = scenarioNo + "_" + condNo;
                            ScenarioBuilder.FormatCell(cell);
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.AutoFitColumns();
                            colIndex++;

                            //Test Case Description.
                            cell = ws.Cells[rowIndex, colIndex];
                            cell.Value = strBuilder.ToString();
                            if (item.ConditionResult == "Negative")
                            {
                                cell.Style.Font.Color.SetColor(Color.Red);
                            }
                            ScenarioBuilder.FormatCell(cell);
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.WrapText = true;
                            colIndex++;

                            //Test Data
                            cell = ws.Cells[rowIndex, colIndex];
                            cell.Value = strAttr.ToString();
                            if (item.ConditionResult == "Negative")
                            {
                                cell.Style.Font.Color.SetColor(Color.Red);
                            }
                            ScenarioBuilder.FormatCell(cell);
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.WrapText = true;
                            colIndex++;

                            //Expected Result
                            cell = ws.Cells[rowIndex, colIndex];
                            cell.Value = trans.HighLevelTxnDesc + " should be created/generated successfully";
                            if (item.ConditionResult == "Negative")
                            {
                                cell.Style.Font.Color.SetColor(Color.Red);
                            }
                            ScenarioBuilder.FormatCell(cell);
                            cell.Style.ShrinkToFit = true;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.AutoFitColumns();

                            rowIndex++;

                        }



                    }
                    testScenarioIndex++;

                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GetScenarioDescription(Dictionary<string, List<TestScenarioMembers>> testScenarios, IList<tbl_Transactions> lstHighLevelTransactions, int i)
        {
            try
            {


                StringBuilder ScenarioDescription = new StringBuilder();
                int scnNo = 1;

                foreach (var trans in lstHighLevelTransactions)
                {

                    var dictValues = testScenarios[trans.HighLevelTxnDesc];
                    var item = dictValues[i];
                    if (item != null && item.ConditionNo != "" && item.ConditionResult != "")
                    {
                        ScenarioDescription.Append(scnNo + "." + trans.HighLevelTxnDesc);
                        ScenarioDescription.AppendLine();
                        scnNo++;
                    }

                }

                return ScenarioDescription.ToString();

            }
            catch (Exception)
            {
                throw;
            }
        }

    }
    public class TestScenarioMembers
    {
        public string ScenarioNo { get; set; }
        public string ConditionNo { get; set; }
        public string ConditionResult { get; set; }
        public string Status { get; set; } = "";
    }
}





