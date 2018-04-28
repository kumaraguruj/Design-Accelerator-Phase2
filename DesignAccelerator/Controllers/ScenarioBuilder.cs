using System;
using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;
using System.Drawing;
using System.IO;
using DA.DomainModel;
using DA.BusinessLayer;
using DesignAccelerator.Models.ViewModel;
using System.Web;
using System.Text;
using System.Collections;
using System.Threading.Tasks;

namespace DesignAccelerator.Controllers
{
    public class ScenarioBuilder
    {
        ExcelCommonFunctions excelCommonFunctions = new ExcelCommonFunctions();
        DataTable dtSB;
        DataTable dtTM;
        Dictionary<string, List<ScenarioBuilderMembers>> testScenarios = new Dictionary<string, List<ScenarioBuilderMembers>>();
        Dictionary<string, List<string>> missedConditions = new Dictionary<string, List<string>>();
        DataTable scenarioBuilder;

        // GET: ScenarioBuilder
        public async Task<string> GenerateScenarioBuilder(int daId, HttpPostedFileBase txmPath, HttpPostedFileBase ScbPath)
        {
            try
            {


                TransactionMatrix transactionMatrix = new TransactionMatrix();

                ScenarioBuilderViewModel sbviewmodel = new ScenarioBuilderViewModel();

                sbviewmodel.lstHighLevelTransactions = sbviewmodel.GetTransactionsList(daId).lstTransactions;

                AttributeListManager attributeListManager = new AttributeListManager();
                IList<tbl_Attribute> lstAttribute = attributeListManager.GetCommonriticalAttributes(daId);

                int colIndex = 1, rowIndex = 0;

                //Read data from Scenario Builder
                using (var package = new ExcelPackage(ScbPath.InputStream))
                {
                    ExcelWorksheet ws2 = excelCommonFunctions.CreateSheet(package, "Test Scenarios", 1);

                    CreateHeadersForTS(ws2, out dtSB, out dtTM, sbviewmodel.lstHighLevelTransactions);

                    scenarioBuilder = GetDataTableFromExcel(ScbPath, true);

                    DataSet transactionMatrixSheets = new DataSet();
                    using (var objExcelPackage = new ExcelPackage(txmPath.InputStream))
                    {
                        foreach (var trans in sbviewmodel.lstHighLevelTransactions)
                        {
                            int colIdex = 1, rowIdex = 0;
                            ExcelWorksheet ws = excelCommonFunctions.OpenSheet(objExcelPackage, trans.HighLevelTxnDesc);
                            DataTable dtRuleOfN = transactionMatrix.GetRuleOfNData(ws, ref colIdex, ref rowIdex);
                            dtRuleOfN.TableName = trans.HighLevelTxnDesc;
                            System.Data.DataColumn newColumn = new System.Data.DataColumn("Status", typeof(System.String));
                            newColumn.DefaultValue = "";
                            dtRuleOfN.Columns.Add(newColumn);
                            newColumn = new System.Data.DataColumn("Missed", typeof(System.String));
                            newColumn.DefaultValue = "";
                            dtRuleOfN.Columns.Add(newColumn);
                            newColumn = new System.Data.DataColumn("NegativeDone", typeof(System.String));
                            newColumn.DefaultValue = "";
                            dtRuleOfN.Columns.Add(newColumn);
                            transactionMatrixSheets.Tables.Add(dtRuleOfN);
                        }
                    }

                    // for missed conditions 

                    DataTable transactionMatrixSheet = new DataTable();
                    string baseSheetName = "";
                    string sheetName = "";
                    string firstNotEmptyCell = "";
                    string testConditionResult = "";
                    string testConditionid = "";
                    int transactionMatrixSheetRowNo = -1;
                    string mainSheetName = scenarioBuilder.Columns[1].ToString().Trim().Substring(scenarioBuilder.Columns[1].ToString().LastIndexOf("_") + 1);
                    transactionMatrixSheet = transactionMatrixSheets.Tables[mainSheetName];
                    int mainSheetRowCount = transactionMatrixSheet.Rows.Count - 1;
                    //Go through each row in Scenario Builder
                    foreach (DataRow scenarioBuilderRow in scenarioBuilder.Rows)
                    {
                        //Since 1000 of records are coming in scenario builder better check first column value
                        if (scenarioBuilderRow[0].ToString() != "")
                        {
                            int firstNotEmptyCellNo;
                            firstNotEmptyCell = GetFirstNotEmptyCell(scenarioBuilderRow, out firstNotEmptyCellNo);

                            Dictionary<string, string> commonAttributes = new Dictionary<string, string>();
                            Dictionary<string, string> nonCommonAttributes = new Dictionary<string, string>();


                            //Proceed to pick the next sheet from TM here

                            baseSheetName = scenarioBuilder.Columns[firstNotEmptyCellNo].ToString().Trim().Substring(scenarioBuilder.Columns[firstNotEmptyCellNo].ToString().LastIndexOf("_") + 1);

                            //select the same sheet ie data table from Transaction Matrix dataset
                            transactionMatrixSheet = transactionMatrixSheets.Tables[baseSheetName];

                            if (baseSheetName == mainSheetName)
                            {

                                if (transactionMatrixSheetRowNo == mainSheetRowCount)
                                {
                                    transactionMatrixSheetRowNo = 0;
                                }
                                //increment the row count for main sheet
                                else
                                    transactionMatrixSheetRowNo++;
                            }

                            if (baseSheetName == mainSheetName)
                            {
                                int columnIndex = 2;
                                //looping through the fetched common attributes list from the database
                                foreach (var item in lstAttribute)
                                {
                                    commonAttributes.Add(item.AttributeDesc, transactionMatrixSheet.Rows[transactionMatrixSheetRowNo][item.AttributeDesc].ToString());
                                    columnIndex++;
                                }

                                testConditionResult = transactionMatrixSheet.Rows[transactionMatrixSheetRowNo]["Test Condition Result"].ToString();
                                testConditionid = transactionMatrixSheet.Rows[transactionMatrixSheetRowNo]["Test Condition ID"].ToString();

                                //looping through non common attributes until test condition result is reached
                                for (; columnIndex < transactionMatrixSheet.Columns.IndexOf("Test Condition Result"); columnIndex++)
                                {
                                    nonCommonAttributes.Add(transactionMatrixSheet.Rows[transactionMatrixSheetRowNo].Table.Columns[columnIndex].ColumnName, transactionMatrixSheet.Rows[transactionMatrixSheetRowNo][columnIndex].ToString());

                                }
                                transactionMatrixSheet.Rows[transactionMatrixSheetRowNo]["Missed"] = "No";
                                transactionMatrixSheet.Rows[transactionMatrixSheetRowNo].AcceptChanges();
                            }
                            else
                            {
                                DataRow[] result = transactionMatrixSheet.Select("[Status] = ''");
                                if (result.Length > 0)
                                {
                                    //Take First 
                                    var firstRecord = result.First();
                                    int columnIndex = 2;
                                    //looping through the fetched common attributes list from the database
                                    foreach (var item in lstAttribute)
                                    {
                                        commonAttributes.Add(item.AttributeDesc, firstRecord[item.AttributeDesc].ToString());
                                        columnIndex++;
                                    }

                                    testConditionResult = firstRecord["Test Condition Result"].ToString();
                                    testConditionid = firstRecord["Test Condition ID"].ToString();

                                    //looping through non common attributes until test condition result is reached
                                    for (; columnIndex < firstRecord.Table.Columns.IndexOf("Test Condition Result"); columnIndex++)
                                    {
                                        nonCommonAttributes.Add(firstRecord.Table.Columns[columnIndex].ColumnName, firstRecord[columnIndex].ToString());

                                    }
                                    firstRecord["Status"] = "Processed";
                                    firstRecord["Missed"] = "No";
                                    firstRecord.AcceptChanges();
                                }
                                else
                                {
                                    //reset all processed to empty status and take first
                                    foreach (DataRow row in transactionMatrixSheet.Rows)
                                    {
                                        row["Status"] = "";
                                        row.AcceptChanges();
                                    }
                                    int columnIndex = 2;
                                    //looping through the fetched common attributes list from the database
                                    foreach (var item in lstAttribute)
                                    {
                                        commonAttributes.Add(item.AttributeDesc, transactionMatrixSheet.Rows[0][item.AttributeDesc].ToString());
                                        columnIndex++;
                                    }

                                    testConditionResult = transactionMatrixSheet.Rows[0]["Test Condition Result"].ToString();
                                    testConditionid = transactionMatrixSheet.Rows[0]["Test Condition ID"].ToString();

                                    //looping through non common attributes until test condition result is reached
                                    for (; columnIndex < transactionMatrixSheet.Columns.IndexOf("Test Condition Result"); columnIndex++)
                                    {
                                        nonCommonAttributes.Add(transactionMatrixSheet.Rows[0].Table.Columns[columnIndex].ColumnName, transactionMatrixSheet.Rows[0][columnIndex].ToString());

                                    }
                                    transactionMatrixSheet.Rows[0]["Status"] = "Processed";
                                    transactionMatrixSheet.Rows[0]["Missed"] = "No";
                                    transactionMatrixSheet.Rows[0].AcceptChanges();

                                }

                            }
                            //check for the cell with 'color' - if 'color' then proceed to process for the negative and add in the test scenario dictionary.
                            if (firstNotEmptyCell != "Color")
                            {
                                //Add case of base sheet
                                ScenarioBuilderMembers sbMember = new ScenarioBuilderMembers { ConditionNo = testConditionid.ToString(), ConditionResult = testConditionResult };
                                if (testScenarios.ContainsKey(baseSheetName))
                                {
                                    testScenarios[baseSheetName].Add(sbMember);

                                }
                                else
                                {
                                    List<ScenarioBuilderMembers> lstScenarioBuilderMembers = new List<ScenarioBuilderMembers>();
                                    lstScenarioBuilderMembers.Add(sbMember);
                                    testScenarios.Add(baseSheetName, lstScenarioBuilderMembers);
                                }
                                //looping through the scenario builder rows we get the next HLT or sheetname
                                for (int k = firstNotEmptyCellNo + 1; k <= scenarioBuilderRow.ItemArray.Length - 1; k++)
                                {
                                    //fetching the sheetname ie the HLT from scenario builder
                                    sheetName = scenarioBuilder.Columns[k].ToString().Trim().Substring(scenarioBuilder.Columns[k].ToString().LastIndexOf("_") + 1);
                                    if (scenarioBuilderRow[k].ToString() != "")
                                    {
                                        //this method compares the columns and values and adds in the test scenario dictionary
                                        GenerateTestScenarios(transactionMatrixSheets, scenarioBuilder, commonAttributes, sheetName, nonCommonAttributes, scenarioBuilderRow[k].ToString(), lstAttribute, k, scenarioBuilderRow);

                                    }
                                    //need to add empty values too
                                    else
                                    {
                                        sbMember = new ScenarioBuilderMembers { ConditionNo = string.Empty, ConditionResult = string.Empty };
                                        if (testScenarios.ContainsKey(sheetName))
                                        {
                                            testScenarios[sheetName].Add(sbMember);

                                        }
                                        else
                                        {
                                            List<ScenarioBuilderMembers> lstScenarioBuilderMembers = new List<ScenarioBuilderMembers>();
                                            lstScenarioBuilderMembers.Add(sbMember);
                                            testScenarios.Add(sheetName, lstScenarioBuilderMembers);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //Add Negative Scenarion in Red
                                ScenarioBuilderMembers sbMember = new ScenarioBuilderMembers { ConditionNo = testConditionid.ToString(), ConditionResult = testConditionResult };
                                if (testScenarios.ContainsKey(baseSheetName))
                                {
                                    testScenarios[baseSheetName].Add(sbMember);

                                }
                                else
                                {
                                    List<ScenarioBuilderMembers> lstScenarioBuilderMembers = new List<ScenarioBuilderMembers>();
                                    lstScenarioBuilderMembers.Add(sbMember);
                                    testScenarios.Add(baseSheetName, lstScenarioBuilderMembers);
                                }
                                //And Add blank for all
                                for (int k = firstNotEmptyCellNo + 1; k <= scenarioBuilderRow.ItemArray.Length - 1; k++)
                                {
                                    sheetName = scenarioBuilder.Columns[k].ToString().Trim().Substring(scenarioBuilder.Columns[k].ToString().LastIndexOf("_") + 1);
                                    sbMember = new ScenarioBuilderMembers { ConditionNo = string.Empty, ConditionResult = string.Empty };
                                    if (testScenarios.ContainsKey(sheetName))
                                    {
                                        testScenarios[sheetName].Add(sbMember);

                                    }

                                    else
                                    {
                                        List<ScenarioBuilderMembers> lstScenarioBuilderMembers = new List<ScenarioBuilderMembers>();
                                        lstScenarioBuilderMembers.Add(sbMember);
                                        testScenarios.Add(sheetName, lstScenarioBuilderMembers);
                                    }

                                }



                            }

                        }
                    }
                    ws2.Cells[2, 1].Value = "Scenario Stitching";

                    //// Format Excel Sheet
                    ws2.Cells[2, 1, 2, 3].Merge = true; //Merge columns start and end range
                    ws2.Cells[2, 1, 2, 3].Style.Font.Bold = true; //Font should be bold
                    ws2.Cells[2, 1, 2, 3].Style.Font.Size = 16;
                    ws2.Cells[2, 1, 2, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; // Alignment is Left
                    ws2.Cells[2, 1, 2, 3].Style.Fill.PatternType = ExcelFillStyle.Solid; // Border
                                                                                         //ws2.Cells[1, 1, 1, 3].Style.Fill.BackgroundColor.SetColor(Color.LightGray); // Background Color
                    ws2.Cells[2, 1, 2, 3].Style.Fill.BackgroundColor.SetColor(Color.LightGray); // Background Color

                    colIndex = 1;
                    rowIndex = rowIndex + 1;

                    excelCommonFunctions.CreateTableHeaderSB(dtTM, ws2, ref colIndex, ref rowIndex, "tbl1");
                    excelCommonFunctions.CreateTableHeaderSB(dtSB, ws2, ref colIndex, ref rowIndex, "tbl2");
                    AddTestScenarios(testScenarios, ws2, colIndex, rowIndex, sbviewmodel.lstHighLevelTransactions);
                    //Format Excel Sheet
                    ws2.View.ShowGridLines = false;
                    ws2.View.ZoomScale = 80;
                    ws2.Cells.AutoFitColumns();
                    ExcelWorksheet wsValidations = excelCommonFunctions.CreateSheet(package, "Validations", 2);
                    ExcelWorksheet wsMissedConditions = excelCommonFunctions.CreateSheet(package, "Missed Conditions", 3);
                    GenerateMissedConditions(sbviewmodel.lstHighLevelTransactions, transactionMatrixSheets);
                    AddMissedConditions(missedConditions, wsMissedConditions);
                    // Code for Validations
                    AddValidations(testScenarios, transactionMatrixSheets, sbviewmodel.lstHighLevelTransactions, wsValidations, scenarioBuilder, lstAttribute);

                    //Format Excel Sheet
                    wsMissedConditions.View.ShowGridLines = false;
                    wsMissedConditions.View.ZoomScale = 80;
                    wsMissedConditions.Cells.AutoFitColumns();

                    //For Validation sheet
                    wsValidations.View.ShowGridLines = true;
                    wsValidations.View.ZoomScale = 80;
                    wsValidations.Cells.AutoFitColumns();
                    tbl_DesignAccelerator da = new tbl_DesignAccelerator();
                    DAManager daManager = new DAManager();

                    da = daManager.FindDA(daId);
                    string filePath = excelCommonFunctions.SaveFile(package, da.daName, "", "SB");


                    return filePath;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }



        private string GetFirstNotEmptyCell(DataRow scenarioBuilderRow, out int firstNotEmptyCellNo)
        {
            try
            {



                string nonEmptyCell = "";
                int j = 0;
                for (j = 1; j < scenarioBuilderRow.ItemArray.Length; j++)
                {
                    //if next cell after login is not empty
                    if (scenarioBuilderRow[j].ToString() != "")
                    {
                        nonEmptyCell = scenarioBuilderRow[j].ToString();
                        break;
                    }
                    //Add the empty rows
                    else
                    {
                        string baseSheetName = scenarioBuilder.Columns[j].ToString().Trim().Substring(scenarioBuilder.Columns[j].ToString().LastIndexOf("_") + 1);
                        ScenarioBuilderMembers sbMember = new ScenarioBuilderMembers { ConditionNo = string.Empty, ConditionResult = string.Empty };
                        if (testScenarios.ContainsKey(baseSheetName))
                        {
                            testScenarios[baseSheetName].Add(sbMember);

                        }
                        else
                        {
                            List<ScenarioBuilderMembers> lstScenarioBuilderMembers = new List<ScenarioBuilderMembers>();
                            lstScenarioBuilderMembers.Add(sbMember);
                            testScenarios.Add(baseSheetName, lstScenarioBuilderMembers);

                        }
                    }
                }
                firstNotEmptyCellNo = j;
                return nonEmptyCell;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void GenerateTestScenarios(DataSet transactionMatrixSheets, DataTable scenarioBuilder,
            Dictionary<string, string> commonAttributes, string sheetName, Dictionary<string, string> nonCommonAttributes, string cellValue,
            IList<tbl_Attribute> lstAttribute, int cellNumber, DataRow scenarioBuilderRow)
        {
            try
            {


                //select the same sheet ie data table from Transaction Matrix dataset
                DataTable transactionMatrtixSheet = transactionMatrixSheets.Tables[sheetName];
                StringBuilder condition = new StringBuilder();
                ScenarioBuilderMembers sbMember;
                foreach (var item in commonAttributes)
                {
                    //if the cell value is color then test condition result would be negative
                    if (cellValue == "Color")
                        condition.Append("[" + item.Key + "] = '" + item.Value + "' AND [Test Condition Result] = 'Negative' AND ");
                    else
                        //if cell value is text then the test condition result would be positive
                        condition.Append("[" + item.Key + "] = '" + item.Value + "' AND [Test Condition Result] = 'Positive' AND ");
                }

                int lstIndex = condition.ToString().LastIndexOf("AND");
                string selectCondition = condition.ToString().Remove(lstIndex, 4);


                DataRow[] result = transactionMatrtixSheet.Select(selectCondition);
                condition = null;
                bool valuesMatch = true;

                if (result.Length > 0 && cellValue != "Color")
                {
                    //looping through the columns and rows to compare
                    foreach (DataRow dr in result)
                    {
                        string testConditionid = dr["Test Condition ID"].ToString();
                        string testConditionResult = dr["Test Condition Result"].ToString();

                        foreach (var item in nonCommonAttributes)
                        {
                            if (dr.Table.Columns.Contains(item.Key))
                            {
                                if (dr[item.Key].ToString() == item.Value)
                                {
                                    valuesMatch = true;
                                }
                                else
                                {
                                    //Add to Log
                                    valuesMatch = false;
                                    break;
                                }

                            }
                        }
                        //if valuesMatch is true then add the values to the test scenarios.
                        if (valuesMatch)
                        {
                            sbMember = new ScenarioBuilderMembers { ConditionNo = testConditionid.ToString(), ConditionResult = testConditionResult };
                            if (testScenarios.ContainsKey(sheetName))
                            {
                                testScenarios[sheetName].Add(sbMember);

                            }
                            else
                            {
                                List<ScenarioBuilderMembers> lstScenarioBuilderMembers = new List<ScenarioBuilderMembers>();
                                lstScenarioBuilderMembers.Add(sbMember);
                                testScenarios.Add(sheetName, lstScenarioBuilderMembers);
                            }

                        }
                        // if the valuesMatch is false then the values should be added to the log
                        else
                        {

                            //Add to log
                            sbMember = new ScenarioBuilderMembers { ConditionNo = testConditionid.ToString(), ConditionResult = testConditionResult, MisMatch = "Yes" };
                            if (testScenarios.ContainsKey(sheetName))
                            {
                                testScenarios[sheetName].Add(sbMember);

                            }
                            else
                            {
                                List<ScenarioBuilderMembers> lstScenarioBuilderMembers = new List<ScenarioBuilderMembers>();
                                lstScenarioBuilderMembers.Add(sbMember);
                                testScenarios.Add(sheetName, lstScenarioBuilderMembers);
                            }
                        }
                        dr["Status"] = "Processed";
                        dr["Missed"] = "No";
                        dr.AcceptChanges();

                        break;

                    }
                }
                else
                {


                    if (cellValue == "Color")
                    {
                        commonAttributes = null;

                        commonAttributes = new Dictionary<string, string>();

                        //If Negative go back and update
                        result = transactionMatrtixSheet.Select("[Test Condition Result] = 'Negative' AND [NegativeDone] = ''");
                        if (result.Length > 0)
                        {
                            //Take First 
                            var firstRecord = result.First();
                            int columnIndex = 2;
                            //looping through the fetched common attributes list from the database
                            foreach (var item in lstAttribute)
                            {
                                commonAttributes.Add(item.AttributeDesc, firstRecord[item.AttributeDesc].ToString());
                                columnIndex++;
                            }
                            string testConditionid = firstRecord["Test Condition ID"].ToString();
                            string testConditionResult = firstRecord["Test Condition Result"].ToString();

                            sbMember = new ScenarioBuilderMembers { ConditionNo = testConditionid, ConditionResult = testConditionResult };
                            if (testScenarios.ContainsKey(sheetName))
                            {
                                testScenarios[sheetName].Add(sbMember);

                            }
                            else
                            {
                                List<ScenarioBuilderMembers> lstScenarioBuilderMembers = new List<ScenarioBuilderMembers>();
                                lstScenarioBuilderMembers.Add(sbMember);
                                testScenarios.Add(sheetName, lstScenarioBuilderMembers);
                            }

                            //Go back in TransactionMatrix Sheets
                            cellNumber--;
                            while (cellNumber > 0)
                            {

                                //fetching the sheetname ie the HLT from scenario builder
                                sheetName = scenarioBuilder.Columns[cellNumber].ToString().Trim().Substring(scenarioBuilder.Columns[cellNumber].ToString().LastIndexOf("_") + 1);
                                if (scenarioBuilderRow[cellNumber].ToString() != "")
                                {
                                    condition = null;
                                    condition = new StringBuilder();
                                    //Search matching attributes in the Transaction Matrix and Update in the Dictionary
                                    foreach (var item in commonAttributes)
                                    {

                                        condition.Append("[" + item.Key + "] = '" + item.Value + "' AND [Test Condition Result] = 'Positive' AND ");
                                    }

                                    lstIndex = condition.ToString().LastIndexOf("AND");
                                    selectCondition = condition.ToString().Remove(lstIndex, 4);

                                    transactionMatrtixSheet = transactionMatrixSheets.Tables[sheetName];
                                    result = transactionMatrtixSheet.Select(selectCondition);
                                    condition = null;
                                    valuesMatch = true;

                                    if (result.Length > 0)
                                    {
                                        //looping through the columns and rows to compare
                                        var firstRow = result.First();
                                        testConditionid = firstRow["Test Condition ID"].ToString();
                                        testConditionResult = firstRow["Test Condition Result"].ToString();

                                        sbMember = new ScenarioBuilderMembers { ConditionNo = testConditionid, ConditionResult = testConditionResult };

                                        if (testScenarios.ContainsKey(sheetName))
                                        {
                                            testScenarios[sheetName][testScenarios[sheetName].Count - 1] = sbMember;

                                        }
                                        else
                                        {
                                            List<ScenarioBuilderMembers> lstScenarioBuilderMembers = new List<ScenarioBuilderMembers>();
                                            lstScenarioBuilderMembers.Add(sbMember);
                                            testScenarios.Add(sheetName, lstScenarioBuilderMembers);
                                        }

                                    }
                                    else
                                    {
                                        //Update with empty and peach color
                                        sbMember = new ScenarioBuilderMembers { ConditionNo = string.Empty, ConditionResult = string.Empty, MisMatch = "None" };
                                        if (testScenarios.ContainsKey(sheetName))
                                        {
                                            testScenarios[sheetName][testScenarios[sheetName].Count - 1] = sbMember;

                                        }
                                        else
                                        {
                                            List<ScenarioBuilderMembers> lstScenarioBuilderMembers = new List<ScenarioBuilderMembers>();
                                            lstScenarioBuilderMembers.Add(sbMember);
                                            testScenarios.Add(sheetName, lstScenarioBuilderMembers);
                                        }
                                    }

                                }
                                //need to update empty values too
                                else
                                {
                                    sbMember = new ScenarioBuilderMembers { ConditionNo = string.Empty, ConditionResult = string.Empty };
                                    if (testScenarios.ContainsKey(sheetName))
                                    {

                                        testScenarios[sheetName][testScenarios[sheetName].Count - 1] = sbMember;


                                    }
                                    else
                                    {
                                        List<ScenarioBuilderMembers> lstScenarioBuilderMembers = new List<ScenarioBuilderMembers>();
                                        lstScenarioBuilderMembers.Add(sbMember);
                                        testScenarios.Add(sheetName, lstScenarioBuilderMembers);
                                    }
                                }

                                cellNumber--;
                            }
                            firstRecord["NegativeDone"] = "Yes";
                            firstRecord["Missed"] = "No";
                            firstRecord.AcceptChanges();
                        }


                    }
                    else
                    {
                        sbMember = new ScenarioBuilderMembers { ConditionNo = string.Empty, ConditionResult = string.Empty, MisMatch = "None" };
                        if (testScenarios.ContainsKey(sheetName))
                        {
                            testScenarios[sheetName].Add(sbMember);

                        }
                        else
                        {
                            List<ScenarioBuilderMembers> lstScenarioBuilderMembers = new List<ScenarioBuilderMembers>();
                            lstScenarioBuilderMembers.Add(sbMember);
                            testScenarios.Add(sheetName, lstScenarioBuilderMembers);
                        }
                    }




                }



            }
            catch (Exception)
            {
                throw;
            }
        }

        private void GenerateMissedConditions(IList<tbl_Transactions> lstHighLevelTransactions, DataSet transactionMatrtixSheets)
        {
            //Add to log
            try
            {


                foreach (var trans in lstHighLevelTransactions)
                {
                    DataTable transactionMatrixSheet = new DataTable();
                    transactionMatrixSheet = transactionMatrtixSheets.Tables[trans.HighLevelTxnDesc];
                    DataRow[] result = transactionMatrixSheet.Select("[Missed] = ''");
                    if (result.Length > 0)
                    {
                        foreach (DataRow dr in result)
                        {
                            if (missedConditions.ContainsKey(trans.HighLevelTxnDesc))
                            {
                                missedConditions[trans.HighLevelTxnDesc].Add(dr["Test Condition ID"].ToString());
                            }
                            else
                            {
                                List<string> misCond = new List<string>();
                                misCond.Add(dr["Test Condition ID"].ToString());
                                missedConditions.Add(trans.HighLevelTxnDesc, misCond);
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

        private static void CreateHeadersForTS(ExcelWorksheet ws2, out DataTable dtSB, out DataTable dtTM, IList<tbl_Transactions> lstHighLevelTransactions)
        {
            try
            {


                dtSB = new DataTable();
                dtTM = new DataTable();

                int rowstart = 4;
                int colstart = 3;
                int rowend = rowstart;
                int colend = colstart + 1;// dtTM.Columns.Count;

                dtTM.Columns.Add("Scenario No");
                ws2.Cells["B4:B5"].Merge = true;
                ws2.Cells["B4:B5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Justify;

                int j = 1;
                foreach (var att in lstHighLevelTransactions)
                {
                    dtTM.Columns.Add(att.HighLevelTxnDesc);
                    dtTM.Columns.Add("tempColumn" + j.ToString());

                    ws2.Cells[rowstart, colstart, rowend, colend].Merge = true;
                    ws2.Cells[rowstart, colstart, rowend, colend].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Alignment is center
                    colstart += 2;
                    colend = colstart + 1;
                    j++;
                }

                int i = 1;
                DataRow dr = dtSB.NewRow();

                dtSB.Columns.Add("Scenario ID");
                for (i = 1; i <= lstHighLevelTransactions.Count; i++)
                {
                    dtSB.Columns.Add("Condition No." + (i).ToString());
                    dtSB.Columns.Add("Condition Result" + (i).ToString());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static string ParseWorksheetValue(ExcelWorksheet workSheet, Dictionary<string, int> header, int rowIndex, string columnName)
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

        public static DataTable GetDataTableFromExcel(HttpPostedFileBase scbPath, bool hasHeader = true)
        {
            try
            {


                using (var pck = new OfficeOpenXml.ExcelPackage())
                {
                    using (var stream = scbPath.InputStream)
                    {
                        pck.Load(stream);
                    }

                    var ws = pck.Workbook.Worksheets.First();
                    DataTable tbl = new DataTable();
                    //need to set a fixed value for columns of scenario builder 
                    foreach (var firstRowCell in ws.Cells[5, 2, 5, ws.Dimension.End.Column])
                    {

                        tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                    }

                    var startRow = hasHeader ? 6 : 1;
                    for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                    {
                        var wsRow = ws.Cells[rowNum, 2, rowNum, ws.Dimension.End.Column];
                        DataRow row = tbl.Rows.Add();
                        foreach (var cell in wsRow)
                        {
                            //check for the font color
                            var hex = cell.Style.Font.Color;
                            //if it is empty then text else assign 'color'
                            if (hex.Rgb != null && hex.Rgb != "" && cell.Text != "")
                            {
                                row[cell.Start.Column - 2] = "Color";
                            }
                            else
                            {

                                row[cell.Start.Column - 2] = cell.Text;
                            }
                        }
                    }
                    return tbl;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddTestScenarios(Dictionary<string, List<ScenarioBuilderMembers>> testScenarios, ExcelWorksheet ws, int colIndex, int rowIndex, IList<tbl_Transactions> lstHighLevelTransactions)
        {
            try
            {


                rowIndex++;
                int i = rowIndex;
                foreach (var key in lstHighLevelTransactions) // Adding Data into rows
                {
                    var lstRow = testScenarios[key.HighLevelTxnDesc];
                    rowIndex = i;
                    //Find the key = column in excel sheet
                    colIndex = ws.GetColumnByName(key.HighLevelTxnDesc);
                    foreach (var item in lstRow)
                    {
                        var cellConditionNo = ws.Cells[rowIndex, colIndex];
                        cellConditionNo.Value = item.ConditionNo;
                        FormatCell(cellConditionNo);

                        var cellConditionResult = ws.Cells[rowIndex, colIndex + 1];
                        cellConditionResult.Value = item.ConditionResult;
                        FormatCell(cellConditionResult);
                        if (item.MisMatch == "Yes")
                        {
                            var fillYellow = cellConditionNo.Style.Fill;
                            fillYellow.PatternType = ExcelFillStyle.Solid;
                            fillYellow.BackgroundColor.SetColor(Color.Yellow);
                            fillYellow = cellConditionResult.Style.Fill;
                            fillYellow.PatternType = ExcelFillStyle.Solid;
                            fillYellow.BackgroundColor.SetColor(Color.Yellow);
                        }
                        if (item.MisMatch == "None")
                        {
                            var fillOrange = cellConditionNo.Style.Fill;
                            fillOrange.PatternType = ExcelFillStyle.Solid;
                            fillOrange.BackgroundColor.SetColor(Color.PeachPuff);
                            fillOrange = cellConditionResult.Style.Fill;
                            fillOrange.PatternType = ExcelFillStyle.Solid;
                            fillOrange.BackgroundColor.SetColor(Color.PeachPuff);
                        }
                        if (cellConditionResult.Value.ToString() == "Negative")
                        {
                            cellConditionNo.Style.Font.Color.SetColor(Color.Red);
                            cellConditionResult.Style.Font.Color.SetColor(Color.Red);
                        }
                        rowIndex++;
                    }

                }
                colIndex = ws.GetColumnByName("Scenario No");
                rowIndex = i;
                for (int j = 1; j <= ws.Dimension.End.Row - i + 1; j++)
                {
                    var cellScenarioNo = ws.Cells[rowIndex, colIndex];
                    cellScenarioNo.Value = "AT_SCN_" + j.ToString("000");
                    FormatCell(cellScenarioNo);
                    rowIndex++;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddMissedConditions(Dictionary<string, List<string>> missedConditions, ExcelWorksheet wsMissedConditions)
        {
            try
            {


                int colIndex = 2;
                foreach (var key in missedConditions.Keys) // Adding Data into rows
                {
                    int rowIndex = 4;
                    var lstRow = missedConditions[key];
                    var cellHeader = wsMissedConditions.Cells[rowIndex, colIndex];
                    cellHeader.Value = key;
                    FormatCell(cellHeader);
                    rowIndex++;
                    foreach (var item in lstRow)
                    {
                        var cellValue = wsMissedConditions.Cells[rowIndex, colIndex];
                        cellValue.Value = item.ToString();
                        FormatCell(cellValue);
                        rowIndex++;
                    }
                    colIndex++;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddValidations(Dictionary<string, List<ScenarioBuilderMembers>> testScenarios,
            DataSet transactionMatrixSheets, IList<tbl_Transactions> lstHighLevelTransactions,
            ExcelWorksheet wsValidations, DataTable scenarioBuilder, IList<tbl_Attribute> lstAttribute)
        {
            try
            {


                int i = 0;
                int k = 1;
                string sheetName = "";
                int colIndex = 1, rowIndex = 2, originalRowIndex = 2;

                DataTable transactionMatrtixSheet;
                ScenarioBuilderMembers scenarioBuilderMembers;
                ArrayList commonAttributes = new ArrayList();
                foreach (var item in lstAttribute)
                {
                    commonAttributes.Add(item.AttributeDesc);
                }
                foreach (DataRow scenarioBuilderRow in scenarioBuilder.Rows)
                {
                    if (scenarioBuilderRow[0].ToString() != "")
                    {
                        ExcelRange scenarioNoCell;
                        if (wsValidations.Dimension != null)
                        {
                            originalRowIndex = wsValidations.Dimension.End.Row + 4;
                            scenarioNoCell = wsValidations.Cells[originalRowIndex, colIndex];
                        }
                        else
                            scenarioNoCell = wsValidations.Cells[rowIndex, colIndex];

                        scenarioNoCell.Value = "AT_SCN_" + k.ToString("000");
                        k++;
                        for (int j = 1; j <= scenarioBuilderRow.ItemArray.Length - 1; j++)
                        {
                            if (scenarioBuilderRow[j].ToString() != "")
                            {
                                sheetName = scenarioBuilder.Columns[j].ToString().Trim().Substring(scenarioBuilder.Columns[j].ToString().LastIndexOf("_") + 1);

                                scenarioBuilderMembers = testScenarios[sheetName].ElementAt(i);

                                transactionMatrtixSheet = transactionMatrixSheets.Tables[sheetName];

                                DataRow[] result = transactionMatrtixSheet.Select("[Test Condition ID] ='" + scenarioBuilderMembers.ConditionNo + "' AND [Test Condition Result] ='" + scenarioBuilderMembers.ConditionResult + "'");

                                if (result.Length > 0)
                                {
                                    rowIndex = originalRowIndex + 1;
                                    foreach (DataRow dr in result)
                                    {

                                        var HeadingCell = wsValidations.Cells[rowIndex, colIndex];
                                        HeadingCell.Value = sheetName;
                                        wsValidations.Cells[rowIndex, colIndex, rowIndex, colIndex + 1].Merge = true;

                                        wsValidations.Cells[rowIndex, colIndex].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        wsValidations.Cells[rowIndex, colIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                        foreach (DataColumn c in dr.Table.Columns)  //loop through the columns. 
                                        {
                                            if (c.ColumnName == "Transaction ID" || c.ColumnName == "Test Condition ID")
                                                continue;
                                            if (c.ColumnName == "Business Rule ID")
                                                break;
                                            else
                                            {

                                                rowIndex++;
                                                var subHeading = wsValidations.Cells[rowIndex, colIndex];
                                                subHeading.Value = c.ColumnName;
                                                subHeading.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                                subHeading.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                                var subHeadingValue = wsValidations.Cells[rowIndex, colIndex + 1];
                                                subHeadingValue.Value = dr[c.ColumnName].ToString();

                                                subHeading.Style.Font.Color.SetColor(Color.White);
                                                subHeading.Style.Font.Bold = true;

                                                var border = subHeadingValue.Style.Border;
                                                border.Bottom.Style =
                                                    border.Top.Style =
                                                    border.Left.Style =
                                                    border.Right.Style = ExcelBorderStyle.Thin;

                                                var border1 = subHeading.Style.Border;
                                                border1.Bottom.Style =
                                                    border1.Top.Style =
                                                    border1.Left.Style =
                                                    border1.Right.Style = ExcelBorderStyle.Thin;

                                                var fillRed = subHeading.Style.Fill;
                                                fillRed.PatternType = ExcelFillStyle.Solid;
                                                fillRed.BackgroundColor.SetColor(Color.Red);

                                                if (commonAttributes.Contains(c.ColumnName))
                                                {
                                                    var fillOrange = subHeading.Style.Fill;
                                                    fillOrange.PatternType = ExcelFillStyle.Solid;
                                                    fillOrange.BackgroundColor.SetColor(Color.Orange);
                                                }

                                            }
                                        }
                                        colIndex = colIndex + 2;
                                    }
                                }
                                else if (result.Length == 0 && scenarioBuilderMembers.MisMatch == "None")
                                {
                                    var HeadingCell = wsValidations.Cells[rowIndex, colIndex];
                                    HeadingCell.Value = sheetName;
                                    wsValidations.Cells[rowIndex, colIndex, rowIndex, colIndex + 1].Merge = true;

                                    wsValidations.Cells[rowIndex, colIndex].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    wsValidations.Cells[rowIndex, colIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    var fillBlue = wsValidations.Cells[rowIndex, colIndex].Style.Fill;
                                    fillBlue.PatternType = ExcelFillStyle.Solid;
                                    fillBlue.BackgroundColor.SetColor(Color.CadetBlue);
                                    colIndex = colIndex + 2;
                                }

                            }

                        }

                        i++;
                        colIndex = 1;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static void FormatCell(ExcelRange cellConditionNo)
        {
            try
            {


                cellConditionNo.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                //Setting borders of cell
                var border = cellConditionNo.Style.Border;
                border.Bottom.Style =
                border.Top.Style =
                border.Left.Style =
                border.Right.Style = ExcelBorderStyle.Thin;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string CreateSBTemplateFile(int daId)
        {
            try
            {


                ScenarioBuilderViewModel sbviewmodel = new ScenarioBuilderViewModel();

                sbviewmodel.lstHighLevelTransactions = sbviewmodel.GetTransactionsList(daId).lstTransactions;

                using (var sbTemplate = new ExcelPackage())
                {
                    ExcelWorksheet scenarioBuilderTemplate = excelCommonFunctions.CreateSheet(sbTemplate, "Scenario Builder", 0);

                    //fixed for all clients
                    int row = 5;
                    int column = 3;
                    int colValue = 0;
                    scenarioBuilderTemplate.Cells[4, 2, 5, 2].Merge = true;
                    //scenarioBuilderTemplate.Cells[4, 2, 5, 2].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    #region scenarioNo
                    //For Scenario Number only//
                    var scn = scenarioBuilderTemplate.Cells[4, 2, 5, 2];
                    scn.Value = "Scenario No.";
                    scenarioBuilderTemplate.Column(2).Width = 15;


                    scn.Style.Font.Size = 11;
                    scn.Style.Font.Color.SetColor(Color.White);



                    var fillscn = scn.Style.Fill;
                    fillscn.PatternType = ExcelFillStyle.Solid;
                    fillscn.BackgroundColor.SetColor(Color.Red);
                    scn.Style.Font.Bold = true;


                    //Setting Top/left,right/bottom borders.
                    var borderscn = scn.Style.Border;
                    borderscn.Bottom.Style = ExcelBorderStyle.Thin;
                    borderscn.Top.Style = ExcelBorderStyle.Medium;
                    borderscn.Left.Style = ExcelBorderStyle.Medium;
                    borderscn.Right.Style = ExcelBorderStyle.Thin;

                    scn.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    scn.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    #endregion
                    //for all the high level transactions
                    foreach (var item in sbviewmodel.lstHighLevelTransactions)
                    {

                        var cell = scenarioBuilderTemplate.Cells[row, column];

                        cell.Style.Font.Color.SetColor(Color.White);


                        //Setting the background color of header cells to Gray
                        var fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.Red);


                        //Setting Top/left,right/bottom borders.
                        var border = cell.Style.Border;
                        border.Bottom.Style = ExcelBorderStyle.Thin;
                        border.Top.Style = ExcelBorderStyle.Medium;
                        border.Left.Style = ExcelBorderStyle.Thin;
                        border.Right.Style = ExcelBorderStyle.Thin;

                        //Setting Value in cell
                        cell.Value = item.HighLevelTxnID + "_" + item.HighLevelTxnDesc;
                        cell.Style.TextRotation = 90;
                        cell.AutoFitColumns();

                        cell.Style.Font.Size = 11;

                        colValue = column++;
                    }

                    //font style is calibri
                    scenarioBuilderTemplate.Cells.Style.Font.Name = "Calibri";

                    #region ScenarioBuilderTitle
                    //Name of the sheet on the top left and bold woth light gray color
                    var titleSB = scenarioBuilderTemplate.Cells[2, 1, 2, 4];
                    titleSB.Merge = true;
                    titleSB.Value = "Scenario Builder";
                    scenarioBuilderTemplate.Cells[4, 2, 4, colValue].Style.Font.Bold = true;

                    titleSB.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    titleSB.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    titleSB.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    titleSB.Style.Font.Color.SetColor(Color.Black);
                    titleSB.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    titleSB.Style.Font.Bold = true;
                    titleSB.Style.Font.Size = 20;
                    #endregion

                    //setting the last header column right border to medium
                    scenarioBuilderTemplate.Cells[5, colValue].Style.Border.Right.Style = ExcelBorderStyle.Medium;


                    scenarioBuilderTemplate.Cells[4, colValue].Style.Font.Bold = true;

                    #region HLTHeader
                    var HLT = scenarioBuilderTemplate.Cells[4, 3, 4, colValue];
                    HLT.Value = "High Level Transactions";

                    //border and formatting for high level transactions
                    HLT.Style.Font.Color.SetColor(Color.White);
                    HLT.Style.Font.Bold = true;
                    HLT.Style.Font.Size = 11;

                    //Setting the background color of header cells to Gray
                    var fillHlt = HLT.Style.Fill;
                    fillHlt.PatternType = ExcelFillStyle.Solid;
                    fillHlt.BackgroundColor.SetColor(Color.Red);


                    //Setting Top/left,right/bottom borders.
                    var borderHlt = HLT.Style.Border;
                    borderHlt.Bottom.Style = ExcelBorderStyle.Medium;
                    borderHlt.Top.Style = ExcelBorderStyle.Medium;
                    borderHlt.Left.Style = ExcelBorderStyle.Thin;
                    borderHlt.Right.Style = ExcelBorderStyle.Thin;

                    scenarioBuilderTemplate.Cells[4, 3, 4, colValue].Merge = true;
                    HLT.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion

                    scenarioBuilderTemplate.Cells[4, 3, 5, colValue].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    scenarioBuilderTemplate.Cells[4, colValue].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    scenarioBuilderTemplate.Cells[4, colValue].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;



                    scenarioBuilderTemplate.View.ShowGridLines = false;
                    scenarioBuilderTemplate.View.ZoomScale = 80;
                    // scenarioBuilderTemplate.Cells.AutoFitColumns();


                    tbl_DesignAccelerator da = new tbl_DesignAccelerator();
                    DAManager daManager = new DAManager();

                    da = daManager.FindDA(daId);
                    string filePath = excelCommonFunctions.SaveFile(sbTemplate, da.daName, "", "ScenarioBuilderTemplateFile");


                    return filePath;

                }


            }
            catch (Exception)
            {
                throw;
            }
        }

    }
    public static class EPPlusExtensionMethod
    {

        public static int GetColumnByName(this ExcelWorksheet ws, string columnName)
        {
            try
            {


                if (ws == null) throw new ArgumentNullException(nameof(ws));
                return ws
                .Cells[4, 2, 4, ws.Dimension.End.Column]
                .First(c => c.Value.ToString() == columnName)
                .Start
                .Column; ;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
    public class ScenarioBuilderMembers
    {

        public string ConditionNo { get; set; }
        public string ConditionResult { get; set; }
        public string MisMatch { get; set; } = "";
    }
}