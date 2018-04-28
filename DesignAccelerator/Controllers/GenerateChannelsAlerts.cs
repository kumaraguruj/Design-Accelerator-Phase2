using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DesignAccelerator.Models.ViewModel;
using System.IO;
using DA.DomainModel;
using DA.BusinessLayer;

namespace DesignAccelerator.Controllers
{
    public class GenerateChannelsAlerts
    {
        public string GenerateChannelsAlertsMappingTable(int daId, HttpPostedFileBase path)
        {
            try
            {


                ExcelCommonFunctions excelCommonFunctions = new ExcelCommonFunctions();
                TransactionMatrix transactionMatrix = new TransactionMatrix();

                InterfaceMappingViewModel interfaceMappingView = new InterfaceMappingViewModel();
                interfaceMappingView.lstHighLevelTxns = interfaceMappingView.GetTransactionsList(daId).lstTransactions;

                ChannelAndAlertsMappingViewModel channelAlertMappingView = new ChannelAndAlertsMappingViewModel();
                channelAlertMappingView.lstChannelAlertsData = channelAlertMappingView.GetChannelAlertsList(daId);

                using (ExcelPackage objExcelPackage = new ExcelPackage(path.InputStream))
                {
                    int cnt = 1;

                    foreach (var trans in interfaceMappingView.lstHighLevelTxns)
                    {
                        int colIndex = 1, rowIndex = 0;
                        int rowCountRuleofN;
                        ExcelWorksheet ws = excelCommonFunctions.OpenSheet(objExcelPackage, trans.HighLevelTxnDesc);

                        DataTable dtRuleOfN = transactionMatrix.GetRuleOfNDataForAllTables(ws, ref colIndex, ref rowIndex, "Channels and Alerts - " + trans.HighLevelTxnDesc);
                        //get merrged cells to find the end row of Rule of N table
                        if (ws.MergedCells.Count == 1)
                        {
                            rowCountRuleofN = dtRuleOfN.Rows.Count;
                        }
                        else
                        {
                            //the header row of Rule of N is fixed as 17     
                            var c = ws.MergedCells[1];//Assuming the first merged cell is of one mapping table.
                            ExcelAddress cellAddr = new ExcelAddress(c);
                            int row = cellAddr.Start.Row;
                            int lastRow = row - 2;
                            rowCountRuleofN = lastRow - 17;
                        }

                        //create datatable for each transaction
                        DataTable dtChannels = ChannelAndAlertsMappingViewModel.CreateChannelAlertDataTable(channelAlertMappingView.lstChannelAlertsData, trans.TransactionSeq, dtRuleOfN);

                        //Table#2
                        if (dtChannels.Rows.Count != 0)
                        {
                            rowIndex = rowIndex + 2;

                            string tblName2 = "Channels and Alerts - " + trans.HighLevelTxnDesc;

                            ws.Cells[rowIndex, 1].Value = tblName2;
                            // ws.Cells[rowIndex, 1, rowIndex, 7].Merge = true; //Merge columns start and end range
                            ws.Cells[rowIndex, 1, rowIndex, 7].Style.Font.Bold = true; //Font should be bold
                            ws.Cells[rowIndex, 1, rowIndex, 7].Style.Font.Size = 20;
                            ws.Cells[rowIndex, 1, rowIndex, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; // Aligmnet is center
                            ws.Cells[rowIndex, 1, rowIndex, 7].Style.Fill.PatternType = ExcelFillStyle.Solid; // Aligmnet is center
                            ws.Cells[rowIndex, 1, rowIndex, 7].Style.Fill.BackgroundColor.SetColor(Color.LightGray); // Aligmnet is center 

                            colIndex = 1;
                            rowIndex = rowIndex + 2;
                            excelCommonFunctions.CreateTableHeader(dtChannels, ws, ref colIndex, ref rowIndex, tblName2);
                            Dictionary<string, string> dictChannels = excelCommonFunctions.GetMappingID(dtChannels);
                            excelCommonFunctions.AddRowsMapping(dtChannels, ws, dictChannels, rowCountRuleofN, ref colIndex, ref rowIndex);
                        }

                        ws.View.ShowGridLines = false;
                        ws.View.ZoomScale = 80;
                        //ws.Cells.AutoFitColumns();
                        //rearranged
                        cnt++;
                    }
                    tbl_DesignAccelerator da = new tbl_DesignAccelerator();
                    DAManager daManager = new DAManager();

                    da = daManager.FindDA(daId);

                    string filePath = excelCommonFunctions.SaveFile(objExcelPackage, da.daName, path.FileName, "C&A");
                    return filePath;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}