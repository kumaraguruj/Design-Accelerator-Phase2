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
using System.Text.RegularExpressions;
using System.Web;

namespace DesignAccelerator.Controllers
{
    public class RunPlan
    {
        RunPlanViewModel runPlanVm = new RunPlanViewModel();
        TransactionMatrix transactionMatrix = new TransactionMatrix();
        DataSet transactionMatrixSheets = new DataSet();
        Dictionary<string, List<TestScenarioMembers>> testDesignData = new Dictionary<string, List<TestScenarioMembers>>();
        DataTable dtRPData = new DataTable();
        DataTable dtRPstatus = new DataTable();
        ExcelPackage package;
        ExcelCommonFunctions excelCommonFunctions = new ExcelCommonFunctions();

        public string CreateRunPlanFile(int daId, HttpPostedFileBase rpFilePath)
        {
            try
            {



                DataTable getTestDesignTable = new DataTable();
                using (package = new ExcelPackage())
                {


                    using (var objExcelPackage = new ExcelPackage(rpFilePath.InputStream))
                    {

                        ExcelWorksheet ws = excelCommonFunctions.OpenSheet(objExcelPackage, "Design Document");

                        //testDesignData

                        getTestDesignTable = GetDataTableFromExcel(rpFilePath, true);

                    }


                    tbl_DesignAccelerator da = new tbl_DesignAccelerator();
                    DAManager daManager = new DAManager();
                    da = daManager.FindDA(daId);
                    var modId = da.ModuleId;

                    tbl_Module daModule = new tbl_Module();
                    ModuleManager modManager = new ModuleManager();
                    daModule = modManager.FindModuleNameForRunPlan(modId);
                    var moduleName = daModule.ModuleName;

                    AddRunPlanData(getTestDesignTable, daId, moduleName);


                    string filePath = excelCommonFunctions.SaveFile(package, da.daName, "", "RunPlanFile");



                    return filePath;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable GetDataTableFromExcel(HttpPostedFileBase tRunPlanPath, bool hasHeader = true)
        {
            try
            {


                using (var pck = new ExcelPackage())
                {
                    using (var stream = tRunPlanPath.InputStream)
                    {
                        pck.Load(stream);
                    }
                    var ws = pck.Workbook.Worksheets.First();
                    DataTable testDesignTable = new DataTable();
                    foreach (var firstRowCell in ws.Cells[4, 1, 1, ws.Dimension.End.Column])
                    {
                        testDesignTable.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                    }

                    DataColumn TestCondition = new DataColumn("TestCondition");
                    testDesignTable.Columns.Add(TestCondition);

                    var startRow = hasHeader ? 5 : 4;
                    for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                    {
                        var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                        DataRow row = testDesignTable.Rows.Add();
                        foreach (var cell in wsRow)
                        {
                            string Negativecolor = cell.Style.Font.Color.Rgb;

                            if (!string.IsNullOrEmpty(Negativecolor))
                            {
                                row[cell.End.Column] = "Negative";
                            }
                            else
                            {
                                row[cell.End.Column] = "Positive";
                            }
                            row[cell.Start.Column - 1] = cell.Text;

                        }
                    }
                    return testDesignTable;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void AddRunPlanData(DataTable testDesignTableData, int daId, string moduleName)
        {
            try
            {
                int colIndex = 1;
                int rowIndex = 11;

                string scenarioNo = string.Empty;
                string functionality = string.Empty;
                string testCaseDesc = string.Empty;
                string preReqs = string.Empty;
                string testCaseId = string.Empty;
                string testScenarioId = string.Empty;
                string module = string.Empty;
                string status = string.Empty;
                string defectId = string.Empty;
                string remarks = string.Empty;
                string conditionNo = string.Empty;
                string Testcondition = string.Empty;
                int counter = 1;

                int noRunCount = 1;
                int passCount = 0;
                int failCount = 0;
                int blockedCount = 0;
                int incompleteCount = 0;
                int notApplicableCount = 0;
                ExcelWorksheet runPlanws = null;
                Dictionary<string, List<DataRow>> totalDays = new Dictionary<string, List<DataRow>>();

                
                int i = 0;
                for (; i < testDesignTableData.Rows.Count; i++)
                {


                    var logicalDay = testDesignTableData.Rows[i]["Logical Day"].ToString();


                    logicalDay = Regex.Replace(logicalDay, @"\t|\n|\r", "");
                    logicalDay = logicalDay.Trim();

                    var checkExistingWs = package.Workbook.Worksheets.FirstOrDefault(w => w.Name == logicalDay.ToString());

                    // package.Workbook.Worksheets.OrderBy(package.Workbook.Worksheets);

                    if (checkExistingWs == null)
                    {

                        runPlanws = package.Workbook.Worksheets.Add(logicalDay.ToString());

                        CreateHeadersForRunPlan(runPlanws, out dtRPstatus);

                        CreateHeadersForRunPlanData(runPlanws, out dtRPData);

                        string displayName = "Run Plan";
                        runPlanws.Cells[2, 1].Value = displayName;


                        var cellFormat = runPlanws.Cells[2, 1, 2, 4];

                        cellFormat.Merge = true;
                        cellFormat.Style.Font.Bold = true;
                        cellFormat.Style.Font.Size = 20;
                        cellFormat.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cellFormat.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        cellFormat.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                        excelCommonFunctions.CreateTableHeader(dtRPstatus, runPlanws, ref colIndex, ref rowIndex, "RunPlanStatus");

                        excelCommonFunctions.CreateTableHeader(dtRPData, runPlanws, ref colIndex, ref rowIndex, "RunPlanData");

                        colIndex = 1;
                        rowIndex++;


                        runPlanws.View.ShowGridLines = false;
                        runPlanws.View.ZoomScale = 80;
                        runPlanws.Cells.AutoFitColumns();
                        runPlanws.Column(1).Width = 20;
                        runPlanws.Column(2).Width = 30;
                        runPlanws.Column(3).Width = 20;
                        runPlanws.Column(4).Width = 20;
                        runPlanws.Column(5).Width = 20;
                        runPlanws.Column(6).Width = 20;
                        runPlanws.Column(7).Width = 55;
                        runPlanws.Column(8).Width = 17;
                        runPlanws.Column(9).Width = 18;
                        runPlanws.Column(10).Width = 15;



                        DataRow[] dRow = testDesignTableData.Select("[Logical Day]= '" + logicalDay + "'");

                        var count = dRow.Length;



                        if (dRow.Length > 0)
                        {
                            foreach (var dr in dRow)
                            {

                                scenarioNo = dr["Scenario No."].ToString();
                                functionality = dr["Functionality"].ToString();
                                testCaseDesc = dr["Test Case Description"].ToString();
                                preReqs = dr["Pre-Requisites"].ToString();
                                conditionNo = dr["Condition No."].ToString();
                                Testcondition = dr["TestCondition"].ToString();

                                if (totalDays.ContainsKey(logicalDay.ToString()))
                                {
                                    totalDays[logicalDay.ToString()].Add(dr);



                                    //for main data table

                                    //count i.e serial No.
                                    var cell = runPlanws.Cells[rowIndex, colIndex];
                                    cell.Value = counter;
                                    ScenarioBuilder.FormatCell(cell);
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    cell.Style.WrapText = true;

                                    colIndex++;

                                    //Module name
                                    cell = runPlanws.Cells[rowIndex, colIndex];
                                    cell.Value = moduleName;
                                    ScenarioBuilder.FormatCell(cell);
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    cell.Style.WrapText = true;

                                    colIndex++;

                                    //functionality
                                    cell = runPlanws.Cells[rowIndex, colIndex];
                                    cell.Value = functionality;
                                    ScenarioBuilder.FormatCell(cell);
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    cell.Style.WrapText = true;

                                    colIndex++;

                                    //Test Scenario Id.
                                    string scnId = scenarioNo;
                                    cell = runPlanws.Cells[rowIndex, colIndex];
                                    cell.Value = scnId;
                                    ScenarioBuilder.FormatCell(cell);
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    cell.Style.WrapText = true;

                                    colIndex++;

                                    //Prerequisites
                                    cell = runPlanws.Cells[rowIndex, colIndex];
                                    cell.Value = preReqs;
                                    ScenarioBuilder.FormatCell(cell);
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    cell.Style.WrapText = true;

                                    colIndex++;

                                    //Test Case Id
                                    string testcaseId = conditionNo.Substring(conditionNo.Length - 6);
                                    cell = runPlanws.Cells[rowIndex, colIndex];
                                    if (scnId == "")
                                    {
                                        cell.Value = "";
                                    }
                                    else
                                    {
                                        cell.Value = scnId + "_" + testcaseId;
                                    }

                                    ScenarioBuilder.FormatCell(cell);
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    cell.Style.WrapText = true;

                                    colIndex++;

                                    //Test Case Description.
                                    cell = runPlanws.Cells[rowIndex, colIndex];
                                    cell.Value = testCaseDesc;
                                    if (Testcondition == "Negative")
                                    {
                                        cell.Style.Font.Color.SetColor(Color.Red);
                                    }
                                    ScenarioBuilder.FormatCell(cell);
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    cell.Style.WrapText = true;

                                    colIndex++;

                                    //Status


                                    var formulaCell = runPlanws.Cells[rowIndex, colIndex];
                                    formulaCell.Value = "No Run";
                                    var selectStatus = runPlanws.DataValidations.AddListValidation(formulaCell.ToString());
                                    selectStatus.ShowErrorMessage = true;
                                    selectStatus.ErrorStyle = OfficeOpenXml.DataValidation.ExcelDataValidationWarningStyle.stop;
                                    selectStatus.ErrorTitle = "Error";
                                    selectStatus.Error = "Please select a value present in the dropdown list";
                                    selectStatus.Formula.Values.Add("Pass");
                                    selectStatus.Formula.Values.Add("Fail");
                                    selectStatus.Formula.Values.Add("No Run");
                                    selectStatus.Formula.Values.Add("Blocked");
                                    selectStatus.Formula.Values.Add("Not Completed");
                                    selectStatus.Formula.Values.Add("Not Applicable");


                                    ScenarioBuilder.FormatCell(formulaCell);
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    cell.Style.WrapText = true;

                                    colIndex++;

                                    //Defect Id
                                    cell = runPlanws.Cells[rowIndex, colIndex];
                                    cell.Value = "";
                                    ScenarioBuilder.FormatCell(cell);
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    cell.Style.WrapText = true;

                                    colIndex++;

                                    //Remarks
                                    cell = runPlanws.Cells[rowIndex, colIndex];
                                    cell.Value = "";
                                    ScenarioBuilder.FormatCell(cell);
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    cell.Style.WrapText = true;


                                    //for status table

                                    int statusRowno = 5;
                                    int statusColno = 1;

                                    //module name
                                    var statusCell = runPlanws.Cells[statusRowno, statusColno++];
                                    statusCell.Value = moduleName;
                                    ScenarioBuilder.FormatCell(statusCell);
                                    statusCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    statusCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                                    //total cases(planned cases)
                                    statusCell = runPlanws.Cells[statusRowno, statusColno++];
                                    statusCell.Value = count;
                                    ScenarioBuilder.FormatCell(statusCell);
                                    statusCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    statusCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                                    //pass
                                    statusCell = runPlanws.Cells[statusRowno, statusColno++];
                                    statusCell.Value = passCount;
                                    statusCell.Formula = "COUNTIF(" + rowIndex + ":" + colIndex + "," + @"""Pass""" + ")";
                                    ScenarioBuilder.FormatCell(statusCell);
                                    statusCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    statusCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                                    //fail
                                    statusCell = runPlanws.Cells[statusRowno, statusColno++];
                                    statusCell.Value = failCount;
                                    statusCell.Formula = "COUNTIF(" + rowIndex + ":" + colIndex + "," + @"""Fail""" + ")";
                                    ScenarioBuilder.FormatCell(statusCell);
                                    statusCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    statusCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                                    //Blocked
                                    statusCell = runPlanws.Cells[statusRowno, statusColno++];
                                    statusCell.Value = blockedCount;
                                    statusCell.Formula = "COUNTIF(" + rowIndex + ":" + colIndex + "," + @"""Blocked""" + ")";
                                    ScenarioBuilder.FormatCell(statusCell);
                                    statusCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    statusCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                                    //not completed
                                    statusCell = runPlanws.Cells[statusRowno, statusColno++];
                                    statusCell.Value = 0;
                                    statusCell.Formula = "COUNTIF(" + rowIndex + ":" + colIndex + "," + @"""Not Completed""" + ")";
                                    ScenarioBuilder.FormatCell(statusCell);
                                    statusCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    statusCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                                    //No run
                                    statusCell = runPlanws.Cells[statusRowno, statusColno++];
                                    statusCell.Value = count;
                                    statusCell.Formula = "COUNTIF(" + rowIndex + ":" + colIndex + "," + @"""No Run""" + ")";
                                    ScenarioBuilder.FormatCell(statusCell);
                                    statusCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    statusCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                                    //not applicable
                                    statusCell = runPlanws.Cells[statusRowno, statusColno++];
                                    statusCell.Value = notApplicableCount;
                                    statusCell.Formula = "COUNTIF(" + rowIndex + ":" + colIndex + "," + @"""Not Applicable""" + ")";
                                    ScenarioBuilder.FormatCell(statusCell);
                                    statusCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    statusCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                                    //Total executed cases
                                    statusCell = runPlanws.Cells[statusRowno, statusColno++];
                                    statusCell.Value = count;
                                    ScenarioBuilder.FormatCell(statusCell);
                                    statusCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    statusCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                                    rowIndex++;
                                    colIndex = 1;

                                }

                                else
                                {
                                    totalDays.Add(logicalDay.ToString(), new List<DataRow> { dr });


                                    //for main data table

                                    //count i.e serial No.
                                    var cell = runPlanws.Cells[rowIndex, colIndex];
                                    cell.Value = counter;
                                    ScenarioBuilder.FormatCell(cell);
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    colIndex++;

                                    //Module name
                                    cell = runPlanws.Cells[rowIndex, colIndex];
                                    cell.Value = moduleName;
                                    ScenarioBuilder.FormatCell(cell);
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    colIndex++;

                                    //functionality
                                    cell = runPlanws.Cells[rowIndex, colIndex];
                                    cell.Value = functionality;
                                    ScenarioBuilder.FormatCell(cell);
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    colIndex++;

                                    //Test Scenario Id.
                                    string scnId = scenarioNo;
                                    cell = runPlanws.Cells[rowIndex, colIndex];
                                    cell.Value = scnId;
                                    ScenarioBuilder.FormatCell(cell);
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    colIndex++;

                                    //Prerequisites
                                    cell = runPlanws.Cells[rowIndex, colIndex];
                                    cell.Value = preReqs;
                                    ScenarioBuilder.FormatCell(cell);
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    colIndex++;

                                    //Test Case Id
                                    string testcaseId = conditionNo.Substring(conditionNo.Length - 6);
                                    cell = runPlanws.Cells[rowIndex, colIndex];

                                    // since test case id depends on the scenario id, so if scenarioid is blank then test case id too will be blank.
                                    if (scnId == "")
                                    {
                                        cell.Value = "";
                                    }
                                    else
                                    {
                                        cell.Value = scnId + "_" + testcaseId;
                                    }

                                    ScenarioBuilder.FormatCell(cell);
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    colIndex++;

                                    //Test Case Description.
                                    cell = runPlanws.Cells[rowIndex, colIndex];
                                    cell.Value = testCaseDesc;
                                    if (Testcondition == "Negative")
                                    {
                                        cell.Style.Font.Color.SetColor(Color.Red);
                                    }
                                    ScenarioBuilder.FormatCell(cell);
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    colIndex++;


                                    //Status
                                    cell = runPlanws.Cells[rowIndex, colIndex];
                                    cell.Value = "No Run";
                                    var selectStatus = runPlanws.DataValidations.AddListValidation(cell.ToString());
                                    selectStatus.Formula.Values.Add("Pass");
                                    selectStatus.Formula.Values.Add("Fail");
                                    selectStatus.Formula.Values.Add("No Run");
                                    selectStatus.Formula.Values.Add("Blocked");
                                    selectStatus.Formula.Values.Add("Not Completed");
                                    selectStatus.Formula.Values.Add("Not Applicable");

                                    ScenarioBuilder.FormatCell(cell);
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    colIndex++;

                                    //Defect Id
                                    cell = runPlanws.Cells[rowIndex, colIndex];
                                    cell.Value = "";
                                    ScenarioBuilder.FormatCell(cell);
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    colIndex++;

                                    //Remarks
                                    cell = runPlanws.Cells[rowIndex, colIndex];
                                    cell.Value = "";
                                    ScenarioBuilder.FormatCell(cell);
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;


                                    //for status table

                                    int statusRowno = 5;
                                    int statusColno = 1;

                                    //module name
                                    var statusCell = runPlanws.Cells[statusRowno, statusColno++];
                                    statusCell.Value = moduleName;
                                    ScenarioBuilder.FormatCell(statusCell);
                                    statusCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    statusCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                                    //total cases(planned cases)
                                    statusCell = runPlanws.Cells[statusRowno, statusColno++];
                                    statusCell.Value = count;
                                    ScenarioBuilder.FormatCell(statusCell);
                                    statusCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    statusCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                                    //pass
                                    statusCell = runPlanws.Cells[statusRowno, statusColno++];
                                    statusCell.Value = passCount;
                                    statusCell.Formula = "COUNTIF(" + rowIndex + ":" + colIndex + "," + @"""Pass""" + ")";
                                    ScenarioBuilder.FormatCell(statusCell);
                                    statusCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    statusCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                                    //fail
                                    statusCell = runPlanws.Cells[statusRowno, statusColno++];
                                    statusCell.Value = failCount;
                                    statusCell.Formula = "COUNTIF(" + rowIndex + ":" + colIndex + "," + @"""Fail""" + ")";
                                    ScenarioBuilder.FormatCell(statusCell);
                                    statusCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    statusCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                                    //Blocked
                                    statusCell = runPlanws.Cells[statusRowno, statusColno++];
                                    statusCell.Value = blockedCount;
                                    statusCell.Formula = "COUNTIF(" + rowIndex + ":" + colIndex + "," + @"""Blocked""" + ")";
                                    ScenarioBuilder.FormatCell(statusCell);
                                    statusCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    statusCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                                    //Not completed
                                    statusCell = runPlanws.Cells[statusRowno, statusColno++];
                                    statusCell.Value = 0;
                                    statusCell.Formula = "COUNTIF(" + rowIndex + ":" + colIndex + "," + @"""Not completed""" + ")";
                                    ScenarioBuilder.FormatCell(statusCell);
                                    statusCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    statusCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                                    //No run
                                    statusCell = runPlanws.Cells[statusRowno, statusColno++];
                                    statusCell.Value = count;
                                    statusCell.Formula = "COUNTIF(" + rowIndex + ":" + colIndex + "," + @"""No Run""" + ")";
                                    ScenarioBuilder.FormatCell(statusCell);
                                    statusCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    statusCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;


                                    //Not applicable
                                    statusCell = runPlanws.Cells[statusRowno, statusColno++];
                                    statusCell.Value = notApplicableCount;
                                    statusCell.Formula = "COUNTIF(" + rowIndex + ":" + colIndex + "," + @"""Not Applicable""" + ")";

                                    ScenarioBuilder.FormatCell(statusCell);
                                    statusCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    statusCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;


                                    //Total executed cases
                                    statusCell = runPlanws.Cells[statusRowno, statusColno++];
                                    statusCell.Value = count;
                                    ScenarioBuilder.FormatCell(statusCell);
                                    statusCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    statusCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;



                                    rowIndex++;
                                    colIndex = 1;

                                }
                                //Run No.
                                counter++;

                            }

                        }
                        //resetting the counter for another new sheet
                        counter = 1;

                        //resetting the count for another sheet
                        noRunCount = 1;
                        failCount = 1;
                        passCount = 1;
                        incompleteCount = 1;
                        blockedCount = 1;
                        notApplicableCount = 1;


                    }

                }
                //sorting sheets in order using linq lambda
                var a = package.Workbook.Worksheets.OrderBy(w => PadNumbers(w.ToString()));//using this PadNumbers method to sort out the sheets in order,it is assigning in order although.

                //looping and arranging the sorted sheets back.
                foreach (var item1 in a)
                {
                    package.Workbook.Worksheets.MoveToEnd(item1.Index);
                }
            }
            catch (Exception)
            {
                throw;
            }


        }
        //uses regular expressions to match and remove any new line, tab or space
        public static string PadNumbers(string input)
        {
            return Regex.Replace(input, "[0-9]+", match => match.Value.PadLeft(10, '0'));
        }

        private static void CreateHeadersForRunPlan(ExcelWorksheet ws2, out DataTable dtRPstatus)
        {
            try
            {


                dtRPstatus = new DataTable();

                dtRPstatus.Columns.Add("Module");
                dtRPstatus.Columns.Add("Planned Cases");
                dtRPstatus.Columns.Add("Pass");
                dtRPstatus.Columns.Add("Fail");
                dtRPstatus.Columns.Add("Blocked");
                dtRPstatus.Columns.Add("Not Completed");
                dtRPstatus.Columns.Add("No Run");
                dtRPstatus.Columns.Add("Not Applicable");
                dtRPstatus.Columns.Add("Total Executed Cases");

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static void CreateHeadersForRunPlanData(ExcelWorksheet ws2, out DataTable dtRP)
        {
            try
            {


                dtRP = new DataTable();

                dtRP.Columns.Add("Run Number");
                dtRP.Columns.Add("Module");
                dtRP.Columns.Add("Functionality");
                dtRP.Columns.Add("Test Scenario ID");
                dtRP.Columns.Add("Prerequisites");
                dtRP.Columns.Add("Test Case ID");
                dtRP.Columns.Add("Test Case Description");
                dtRP.Columns.Add("Status");
                dtRP.Columns.Add("Defect Id");
                dtRP.Columns.Add("Remarks");

            }
            catch (Exception)
            {
                throw;
            }
        }


    }

}