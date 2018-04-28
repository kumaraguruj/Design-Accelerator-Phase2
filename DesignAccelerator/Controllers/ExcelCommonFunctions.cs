using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DesignAccelerator.Models.ViewModel;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System.Data;
using System.Drawing;
using System.IO;
using DA.DomainModel;
using System.Configuration;


namespace DesignAccelerator.Controllers
{
    public class ExcelCommonFunctions
    {
        public void CreateTableHeader(DataTable dt, ExcelWorksheet ws, ref int colIndex, ref int rowIndex, string tbl)
        {
            if (tbl == "tbl1")
            {
                colIndex = 1;
                rowIndex = 3;
            }
            else if (tbl == "tbl2")
            {
                colIndex = 1;
                rowIndex = 17;
            }
            else if (tbl == "tbl3")
            {
                colIndex = 1;
                //rowIndex = rowIndex;
            }
            else if (tbl == "tbl4")
            {
                colIndex = 1;
                rowIndex = 4;
            }
            else if (tbl == "RunPlanStatus")
            {
                colIndex = 1;
                rowIndex = 4;
            }
            else if (tbl == "RunPlanData")
            {
                colIndex = 1;
                rowIndex = 11;
            }

            else if (tbl == "ControlSheet" || tbl == "Summary" || tbl == "TraceabilityMatrix")
            {
                colIndex = 2;
                rowIndex = 4;
            }



            foreach (DataColumn dc in dt.Columns) //Creating Headings
            {
                if (dc.ColumnName.IndexOf("isNegative") < 0)
                {
                    var cell = ws.Cells[rowIndex, colIndex];

                    cell.Style.Font.Color.SetColor(Color.White);
                    cell.Style.Font.Bold = true;

                    //Setting the background color of header cells to Gray
                    var fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.Red);


                    //Setting Top/left,right/bottom borders.
                    var border = cell.Style.Border;
                    border.Bottom.Style =
                        border.Top.Style =
                        border.Left.Style =
                        border.Right.Style = ExcelBorderStyle.Thin;

                    //Setting Value in cell
                    cell.Value = dc.ColumnName;

                    colIndex++;
                }
            }
        }

        //Scenario Builder
        public void CreateTableHeaderSB(DataTable dt, ExcelWorksheet ws, ref int colIndex, ref int rowIndex, string tbl)
        {
            if (tbl == "tbl1")
            {
                colIndex = 2;
                rowIndex = 4;
            }
            else if (tbl == "tbl2")
            {
                colIndex = 2;
                rowIndex = 5;
            }

            foreach (DataColumn dc in dt.Columns) //Creating Headings
            {
                var cell = ws.Cells[rowIndex, colIndex];

                cell.Style.Font.Color.SetColor(Color.White);
                cell.Style.Font.Bold = true;

                //Setting the background color of header cells to Red
                var fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.Red);

                //Setting Top/left,right/bottom borders.
                var border = cell.Style.Border;
                border.Bottom.Style =
                    border.Top.Style =
                    border.Left.Style =
                    border.Right.Style = ExcelBorderStyle.Thin;

                // Setting subheaders under merged value in cell
                if (dc.ColumnName.IndexOf("Condition No.") != -1)
                    cell.Value = "Condition No.";
                else if (dc.ColumnName.IndexOf("Condition Result") != -1)
                    cell.Value = "Condition Result";
                else
                    cell.Value = dc.ColumnName;

                colIndex++;
            }
        }

        public ExcelWorksheet CreateSheet(ExcelPackage p, string sheetName, int i)
        {
            p.Workbook.Worksheets.Add(sheetName);
            ExcelWorksheet ws = p.Workbook.Worksheets[i + 1];
            ws.Name = sheetName; //Setting Sheet's name            
            ws.Cells.Style.Font.Size = 12; //Default font size for whole sheet
            ws.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet
            return ws;
        }

        public ExcelWorksheet OpenSheet(ExcelPackage p, string sheetName)
        {
            ExcelWorksheet ws = p.Workbook.Worksheets[sheetName];
            return ws;
        }

        public void AddRows(DataTable dt, ExcelWorksheet ws, ref int colIndex, ref int rowIndex)
        {
            foreach (DataRow dr in dt.Rows) // Adding Data into rows
            {
                colIndex = 1;
                rowIndex++;
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.ColumnName.IndexOf("isNegative") < 0)
                    {
                        var cell = ws.Cells[rowIndex, colIndex];
                        //Setting Value in cell
                        cell.Value = dr[dc.ColumnName];
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        if (dr.Table.Columns.IndexOf(dc.ColumnName) > 1)
                        {
                            int index = dr.Table.Columns.IndexOf(dc.ColumnName);
                            try
                            {
                                string isNegative = dr[index + 1].ToString();
                                if (isNegative == "1")
                                    cell.Style.Font.Color.SetColor(Color.Red);
                            }
                            catch (Exception ex)
                            {

                            }

                        }
                        if (cell.Value.ToString() == "Negative")
                            cell.Style.Font.Color.SetColor(Color.Red);

                        //Setting borders of cell
                        var border = cell.Style.Border;
                        border.Bottom.Style =
                        border.Top.Style =
                        border.Left.Style =
                        border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex++;
                    }
                }
            }

        }

        public void AddRowsMapping(DataTable dt, ExcelWorksheet ws, Dictionary<string, string> dictMapping, int countofRows, ref int colIndex, ref int rowIndex)
        {
            foreach (DataRow dr in dt.Rows) // Adding Data into rows
            {
                colIndex = 1;
                rowIndex++;
                foreach (DataColumn dc in dt.Columns)
                {
                    var cell = ws.Cells[rowIndex, colIndex];
                    //Setting Value in cell
                    cell.Value = dr[dc.ColumnName];// + "\n";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.WrapText = true;

                    //if (dc.ColumnName == "isNegative" && (string)dr[dc.ColumnName] ==  "1")
                    //{
                    //    cell.Style.Font.Color.SetColor(Color.Red);
                    //}

                    //Setting borders of cell
                    var border = cell.Style.Border;
                    border.Bottom.Style =
                    border.Top.Style =
                    border.Left.Style =
                    border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex++;
                }
            }
            if (dictMapping.Count != 0)
            {
                AddMappedID(ws, dictMapping, countofRows);
            }
        }

        //to get mapping ID
        public Dictionary<string, string> GetMappingID(DataTable dt)
        {
            Dictionary<string, string> dictMappingID = new Dictionary<string, string>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Test_Cond_ID"].ToString() != "")
                {
                    string TCID = dt.Rows[i]["Test_Cond_ID"].ToString();
                    string mappedID = dt.Rows[i][0].ToString();
                    if (dictMappingID.ContainsKey(TCID))
                    {
                        dictMappingID[TCID] = dictMappingID[TCID] + " " + mappedID;
                    }
                    else
                        dictMappingID.Add(TCID, mappedID);
                }
            }
            return dictMappingID;
        }

        // Linking Process ID to Mapping Table ID
        public void AddMappedID(ExcelWorksheet ws, Dictionary<string, string> dictMapping, int rowcount)
        {
            int mRindex;//= 20;
            int mCindex;
            string value = string.Empty;
            rowcount = 17 + rowcount;

            foreach (var key in dictMapping.Keys)
            {
                for (mRindex = 17; mRindex <= rowcount; mRindex++)
                {
                    int colInd = 1;
                    for (mCindex = ws.Dimension.Start.Column; mCindex <= ws.Dimension.End.Column; mCindex++)
                    {
                        //ws.Cells.Columns["Interface ID"];                      
                        if (dictMapping[key].StartsWith("IN") && ws.Cells[17, mCindex].Value.ToString() == "Interface ID")
                        {
                            colInd = mCindex;
                        }
                        if (dictMapping[key].StartsWith("BR") && ws.Cells[17, mCindex].Value.ToString() == "Business Rule ID")
                        {
                            colInd = mCindex;
                        }
                        if (dictMapping[key].StartsWith("CA") && ws.Cells[17, mCindex].Value.ToString() == "Channel Alert ID")
                        {
                            colInd = mCindex;
                        }
                        if (dictMapping[key].StartsWith("RE") && ws.Cells[17, mCindex].Value.ToString() == "Report ID")
                        {
                            colInd = mCindex;
                            ws.Cells[17, mCindex].Style.WrapText = true;
                        }
                    }
                    if (ws.Cells[mRindex, 2].Value.ToString() != null)
                    {
                        if (ws.Cells[mRindex, 2].Value.ToString() == key)
                        {
                            ws.Cells[mRindex, colInd].Value = dictMapping[key] + "\n";
                            ws.Cells[mRindex, colInd].Style.WrapText = true;
                        }
                    }
                }
            }
        }

        public string SaveFile(ExcelPackage p, string sDAName, string sFilePath, string sProcess)
        {
            //Generate A File with Random name
            Byte[] bin = p.GetAsByteArray(); //Read the Excel file in a byte array
            string guid = Guid.NewGuid().ToString();
            string file = "";
            object sfolder = "";
            string fileSavingLocation = ConfigurationManager.AppSettings["FileSavingLocation"].ToString();
             
            
            switch (sProcess)
            {
                case "Rule of N - Txn Matrix":
                    file = fileSavingLocation + sDAName + "_TransactionMatrix_" + DateTime.Now.ToString("ddMMMyyyy_HHmmss") + ".xlsx";
                    break;
                case "SB":
                    file = fileSavingLocation + sDAName + "_ScenarioBuilder_" + DateTime.Now.ToString("ddMMMyyyy_HHmmss") + ".xlsx";
                    break;
                case "BR":
                    file = fileSavingLocation + sDAName + "_BusinessRules" + DateTime.Now.ToString("ddMMMyyyy_HHmmss") + ".xlsx";
                    break;
                case "Interfaces":
                    file = fileSavingLocation + sDAName + "_Interface_" + DateTime.Now.ToString("ddMMMyyyy_HHmmss") + ".xlsx";
                    break;
                case "C&A":
                    file = fileSavingLocation + sDAName + "_ChannelsAndAlerts_" + DateTime.Now.ToString("ddMMMyyyy_HHmmss") + ".xlsx";
                    break;
                case "Reports":
                    file = fileSavingLocation + sDAName + "_Reports_" + DateTime.Now.ToString("ddMMMyyyy_HHmmss") + ".xlsx";
                    break;
                case "TD":
                    file = fileSavingLocation + sDAName + "_TestDesign_" + DateTime.Now.ToString("ddMMMyyyy_HHmmss") + ".xlsx";
                    break;
                case "ScenarioBuilderTemplateFile":
                    file = fileSavingLocation + sDAName + "_ScenarioBuilderTemplate_" + DateTime.Now.ToString("ddMMMyyyy_HHmmss") + ".xlsx";
                    break;
                case "TraceabilityMatrix":
                    file = fileSavingLocation + sDAName + "_TraceabilityMatrix_" + DateTime.Now.ToString("ddMMMyyyy_HHmmss") + ".xlsx";
                    break;
                case "RunPlanFile":
                    file = fileSavingLocation + sDAName + "_RunPlan_" + DateTime.Now.ToString("ddMMMyyyy_HHmmss") + ".xlsx";
                    break;
                case "ExportDA":
                    file = fileSavingLocation + sDAName + "_DesignAccelerator_" + DateTime.Now.ToString("ddMMMyyyy_HHmmss") + ".xlsx";
                    break;  
            }


            File.WriteAllBytes(file, bin);

            //System.Diagnostics.Process.Start(file);
            //Console.WriteLine("File Already exists: {0}", e);
            //}
            return file;
        }

    }
}