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
using System.Data.SqlClient;
using System.Configuration;
using DA.BusinessLayer;
using DA.DomainModel;


namespace DesignAccelerator.Controllers
{
    public class ExportDesignAcceleratorController : Controller
    {
        //public IList<tbl_Products> lstprods { get; set; }
        // GET: Mapping
        //[NoDirectAccess]
        int count = 0;
        public string ExportDAFile(int id)
        {
            try
            {


                ExportDesignAcceleratorViewModel ExportDAViewModel = new ExportDesignAcceleratorViewModel();

                ProductsViewModel prodviewmodel = new ProductsViewModel();
                //if (id == null)
                //    id = (int)TempData["daId"];
                var lstprods = prodviewmodel.GetProducts(id);

                var lstExportProducts = ExportDAViewModel.GetProducts(id);

                var lstExportTransactions = ExportDAViewModel.GetTransactions(id);

                IList<tbl_Attribute> lstExportAttributes = ExportDAViewModel.GetAttributes(id);


                List<string> lstSheetNames = new List<string>();
                lstSheetNames.Add("Products");
                lstSheetNames.Add("Transactions");
                lstSheetNames.Add("Transaction Attributes");
                lstSheetNames.Add("Business Rules");
                lstSheetNames.Add("Interfaces");
                lstSheetNames.Add("Channels");
                lstSheetNames.Add("Reports");

                int i = 1;
                string tbl = "";

                using (ExcelPackage objExcelPackage = new ExcelPackage())
                {
                    foreach (var sheet in lstSheetNames)
                    {
                        DataTable dt = new DataTable();

                        switch (sheet)
                        {
                            case "Products":
                                dt = CreateProductTable();
                                break;
                            case "Transactions":
                                dt = CreateTransactionsTable();
                                break;
                            case "Transaction Attributes":
                                dt = CreateAttributesTable(lstExportAttributes, id);
                                break;
                            case "Business Rules":
                                dt = CreateBusinessRulesTable(id, tbl);
                                break;
                            case "Interfaces":
                                dt = CreateInterfaceTable(id);
                                break;
                            case "Channels":
                                dt = CreateChannelsAlertsTable(id);
                                break;
                            case "Reports":
                                dt = CreateReportsTable(id);
                                break;
                            default:
                                break;
                        }

                        ExcelWorksheet ws = CreateSheet(objExcelPackage, sheet, dt, i);


                        if (sheet == "Transaction Attributes" || sheet == "Interfaces" || sheet == "Channels" || sheet == "Reports" || sheet == "Business Rules")
                        {


                            ws.Cells[2, 1].Value = sheet;
                            ws.Cells[2, 1, 2, 3].Merge = true; //Merge columns start and end range
                            ws.Cells[2, 1, 2, 3].Style.Font.Bold = true; //Font should be bold
                            ws.Cells[2, 1, 2, 3].Style.Font.Size = 20;
                            ws.Cells[2, 1, 2, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; // Alignment is center
                            ws.Cells[2, 1, 2, 3].Style.Fill.PatternType = ExcelFillStyle.Solid; // Alignment is center
                            ws.Cells[2, 1, 2, 3].Style.Fill.BackgroundColor.SetColor(Color.LightGray); // Alignment is center

                            var border = ws.Cells[2, 1, 2, 3].Style.Border;
                            border.Bottom.Style = ExcelBorderStyle.Medium;
                            border.Top.Style = ExcelBorderStyle.Thin;
                            border.Left.Style = ExcelBorderStyle.None;
                            border.Right.Style = ExcelBorderStyle.None;

                            int colIndex, rowIndex;
                            if (sheet == "Business Rules")
                            {
                                CreateTableHeaderForBR(dt, ws, out colIndex, out rowIndex, "brtbl", count);
                            }
                            else if (sheet == "Reports")
                            {
                                CreateTableHeaderForReports(dt, ws, out colIndex, out rowIndex, "Reports", count);
                            }
                            else if (sheet == "Transaction Attributes")
                            {
                                CreateTableHeaderForBR(dt, ws, out colIndex, out rowIndex, "txnAttr", count);
                            }
                            else if (sheet == "Interfaces")
                            {
                                CreateTableHeaderForBR(dt, ws, out colIndex, out rowIndex, "Interfaces", count);
                            }
                            else if (sheet == "Channels")
                            {
                                CreateTableHeaderForBR(dt, ws, out colIndex, out rowIndex, "Channels", count);
                            }

                            else
                            {
                                CreateTableHeaderForBR(dt, ws, out colIndex, out rowIndex, "", count);
                            }

                            AddRowsHLT(dt, ws, ref colIndex, ref rowIndex, count, id);



                            ws.View.ShowGridLines = false;
                            ws.View.ZoomScale = 80;
                            ws.Cells.AutoFitColumns();
                            int j;

                            if (sheet == "Business Rules")
                            {
                                for (j = 5; j <= count + 4; j++)
                                {
                                    ws.Column(j).Width = 3.57;
                                }

                            }
                            else if (sheet == "Reports")
                            {
                                for (j = 6; j <= count + 5; j++)
                                {
                                    ws.Column(j).Width = 3.57;
                                }
                            }
                            else if (sheet == "Transaction Attributes")
                            {
                                for (j = 4; j <= count + 3; j++)
                                {
                                    ws.Column(j).Width = 3.57;
                                }
                            }
                            else if (sheet == "Interfaces")
                            {
                                for (j = 8; j <= count + 7; j++)
                                {
                                    ws.Column(j).Width = 3.57;
                                }
                            }
                            else if (sheet == "Channels")
                            {
                                for (j = 5; j <= count + 4; j++)
                                {
                                    ws.Column(j).Width = 3.57;
                                }
                            }

                            if (sheet == "Business Rules")
                            {
                                ws.DeleteColumn(3);
                            }
                            else if (sheet == "Interfaces")
                            {
                                ws.DeleteColumn(4);
                            }
                            else if (sheet == "Channels")
                            {
                                var cnt = ws.Dimension.End.Column;
                                ws.DeleteColumn(cnt);
                            }
                            else if (sheet == "Transaction Attributes")
                            {
                                ws.DeleteColumn(count + 6);
                            }
                            else if (sheet == "Reports")
                            {
                                ws.DeleteColumn(4);
                                var cnt = ws.Dimension.End.Column;
                                ws.DeleteColumn(cnt);
                            }


                            i++;

                        }

                        else
                        {


                            ws.Cells[2, 1].Value = sheet;
                            ws.Cells[2, 1, 2, 3].Merge = true; //Merge columns start and end range
                            ws.Cells[2, 1, 2, 3].Style.Font.Bold = true; //Font should be bold
                            ws.Cells[2, 1, 2, 3].Style.Font.Size = 20;
                            ws.Cells[2, 1, 2, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; // Alignment is center
                            ws.Cells[2, 1, 2, 3].Style.Fill.PatternType = ExcelFillStyle.Solid; // Alignment is center
                            ws.Cells[2, 1, 2, 3].Style.Fill.BackgroundColor.SetColor(Color.LightGray); // Alignment is center

                            var border = ws.Cells[2, 1, 2, 3].Style.Border;
                            border.Bottom.Style = ExcelBorderStyle.Medium;
                            border.Top.Style = ExcelBorderStyle.Thin;
                            border.Left.Style = ExcelBorderStyle.None;
                            border.Right.Style = ExcelBorderStyle.None;

                            int colIndex, rowIndex;

                            CreateTableHeader(dt, ws, out colIndex, out rowIndex, tbl);
                            AddRows(dt, ws, ref colIndex, ref rowIndex);


                            ws.View.ShowGridLines = false;
                            ws.View.ZoomScale = 80;
                            ws.Cells.AutoFitColumns();

                            i++;

                        }
                    }
                    tbl_DesignAccelerator da = new tbl_DesignAccelerator();
                    DAManager daManager = new DAManager();

                    da = daManager.FindDA(id);
                    ExcelCommonFunctions excelCommonfunctions = new ExcelCommonFunctions();
                    string filePath = excelCommonfunctions.SaveFile(objExcelPackage, da.daName, "", "ExportDA");

                    return filePath;

                }
            }
            catch (Exception)
            {
                throw;

            }

        }

        private static DataTable CreateProductTable()
        {
            try
            {


                string constr = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
                DataTable dtdb;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_ExportExcelProduct", con))
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;

                        using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                        {
                            dtdb = new DataTable();
                            dtdb.Columns.AddRange(new DataColumn[1] { new DataColumn("S.No") });
                            dtdb.Columns["S.No"].AutoIncrement = true;
                            dtdb.Columns["S.No"].AutoIncrementSeed = 0;
                            dtdb.Columns["S.No"].AutoIncrementStep = 1;
                            adp.Fill(dtdb);
                        }
                    }
                }


                return dtdb;
            }
            catch (Exception)
            {
                throw;
            }

        }

        private static DataTable CreateTransactionsTable()
        {
            try
            {


                string constr = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
                DataTable dtdb;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_ExportExcelTransaction", con))
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                        {
                            dtdb = new DataTable();
                            dtdb.Columns.AddRange(new DataColumn[1] { new DataColumn("S.No") });
                            dtdb.Columns["S.No"].AutoIncrement = true;
                            dtdb.Columns["S.No"].AutoIncrementSeed = 0;
                            dtdb.Columns["S.No"].AutoIncrementStep = 1;
                            adp.Fill(dtdb);
                        }
                    }
                }
                return dtdb;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable CreateAttributesTable(IList<tbl_Attribute> lstAttributes, int id)
        {
            try
            {


                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[1] { new DataColumn("S.No") });
                dt.Columns["S.No"].AutoIncrement = true;
                dt.Columns["S.No"].AutoIncrementSeed = 1;
                dt.Columns["S.No"].AutoIncrementStep = 1;

                dt.Columns.Add("Requirement Reference");
                dt.Columns.Add("Function/Module");

                TransactionsManager brm = new TransactionsManager();
                IList<tbl_Transactions> lst = brm.GetAllTransactions(id);

                var moduleId = lstAttributes.Select(q => q.tbl_DesignAccelerator.ModuleId);

                tbl_Module tblModule = new tbl_Module();
                ModuleManager moduleManager = new ModuleManager();
                tblModule = moduleManager.FindModuleNameForRunPlan(Convert.ToInt32(moduleId.First()));

                var moduleName = tblModule.ModuleName;


                foreach (var item in lst)
                {
                    var a = item.HighLevelTxnID + "_" + item.HighLevelTxnDesc;
                    dt.Columns.Add(a);
                }

                count = lst.Count;

                dt.Columns.Add("Attribute Description");
                dt.Columns.Add("Attribute Type Description");
                dt.Columns.Add("AttributeID");

                dt.Columns.Add("Value1");
                dt.Columns.Add("Value2");
                dt.Columns.Add("Value3");
                dt.Columns.Add("Value4");
                dt.Columns.Add("Value5");
                dt.Columns.Add("Value6");
                dt.Columns.Add("Value7");
                dt.Columns.Add("Value8");
                dt.Columns.Add("Value9");
                dt.Columns.Add("Value10");
                dt.Columns.Add("Value11");
                dt.Columns.Add("Value12");
                dt.Columns.Add("Value13");
                dt.Columns.Add("Value14");
                dt.Columns.Add("Value15");



                foreach (var item in lstAttributes)
                {

                    DataRow dr = dt.NewRow();
                    dr[count + 3] = item.AttributeDesc;
                    dr[count + 4] = item.tbl_AttributeType.AttributeTypeDesc;
                    dr[count + 5] = item.AttributeID;
                    dr["Function/Module"] = moduleName;

                    dr["Requirement Reference"] = item.tbl_TxnAttributeMapping.Where(q => q.AttributeID == item.AttributeID).Select(q => q.ReqReference).FirstOrDefault();

                    // dr["isLinked"] = item.tbl_TxnAttributeMapping.Select(q => q.isLinked);

                    int i = count + 6;
                    foreach (var item1 in item.tbl_AttributeValues)
                    {
                        dr[i] = item1.AttributeValue;
                        i++;
                    }

                    dt.Rows.Add(dr);
                }

                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }


        private static DataTable CreateTransactionAttributeMappingTable(int id)
        {
            try
            {


                string constr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
                DataTable dtdb;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_ExportExcelTransactionAttribute", con))
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@DAId", SqlDbType.Int).Value = id;

                        using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                        {
                            dtdb = new DataTable();
                            dtdb.Columns.AddRange(new DataColumn[1] { new DataColumn("S.No") });
                            dtdb.Columns["S.No"].AutoIncrement = true;
                            dtdb.Columns["S.No"].AutoIncrementSeed = 0;
                            dtdb.Columns["S.No"].AutoIncrementStep = 1;
                            adp.Fill(dtdb);
                        }
                    }
                }
                return dtdb;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CreateTableHeaderForBR(DataTable dt, ExcelWorksheet ws, out int colIndex, out int rowIndex, string tbl, int hltcount)
        {
            try
            {


                colIndex = 1;
                rowIndex = 5;

                var cell1 = ws.Cells[4, 1, 4, dt.Columns.Count];
                ws.Cells[4, 1, 5, 1].Merge = true;

                if (ws.ToString() == "Transaction Attributes")
                {
                    ws.Cells[4, 3, 4, hltcount + 3].Merge = true;
                    ws.Cells[4, 3, 4, hltcount + 3].Value = "High Level Transactions";
                }
                else if (ws.ToString() == "Business Rules")
                {
                    ws.Cells[4, 5, 4, hltcount + 4].Merge = true;
                    ws.Cells[4, 5, 4, hltcount + 4].Value = "High Level Transactions";
                }
                else if (ws.ToString() == "Interfaces")
                {
                    ws.Cells[4, 3, 4, hltcount + 7].Merge = true;
                    ws.Cells[4, 3, 4, hltcount + 7].Value = "High Level Transactions";
                }
                else if (ws.ToString() == "Channels")
                {
                    ws.Cells[4, 4, 4, hltcount + 5].Merge = true;
                    ws.Cells[4, 4, 4, hltcount + 5].Value = "High Level Transactions";
                }

                ws.Cells[4, 1, 5, 1].Value = "S.No";

                //  ws.Cells[4, hltcount + 2, 4, dt.Columns.Count].Merge = true;


                cell1.Style.Font.Color.SetColor(Color.White);
                cell1.Style.Font.Bold = true;

                //Setting the background color of header cells to Gray
                var fill1 = cell1.Style.Fill;
                fill1.PatternType = ExcelFillStyle.Solid;
                fill1.BackgroundColor.SetColor(Color.Red);


                //Setting Top/left,right/bottom borders.
                var border1 = cell1.Style.Border;
                border1.Bottom.Style =
                    border1.Top.Style =
                    border1.Left.Style =
                    border1.Right.Style = ExcelBorderStyle.Thin;

                //Setting Value in cell

                cell1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Alignment is center
                cell1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                foreach (DataColumn dc in dt.Columns) //Creating Headings
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
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Alignment is center
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    colIndex++;

                }
                if (tbl == "brtbl")
                {   //fromrow,fromcol,torow,tocol
                    var hltCell = ws.Cells[5, 5, 5, hltcount + 4].Style.TextRotation = 90;
                    // ws.Cells[4, hltcount + 1, 5, ws.Dimension.End.Column].Merge = true;
                }
                else if (tbl == "txnAttr")
                {
                    var hltCell = ws.Cells[5, 4, 5, hltcount + 3].Style.TextRotation = 90;
                }
                else if (tbl == "Interfaces")
                {
                    var hltCell = ws.Cells[5, 8, 5, hltcount + 7].Style.TextRotation = 90;

                }
                else if (tbl == "Channels")
                {
                    var hltCell = ws.Cells[5, 5, 5, hltcount + 4].Style.TextRotation = 90;
                }

                ws.Row(5).Height = 222.75;

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CreateTableHeaderForReports(DataTable dt, ExcelWorksheet ws, out int colIndex, out int rowIndex, string tbl, int hltcount)
        {
            try
            {


                colIndex = 1;
                rowIndex = 5;

                var cell1 = ws.Cells[4, 1, 4, dt.Columns.Count];
                ws.Cells[4, 1, 5, 1].Merge = true;

                ws.Cells[4, 5, 4, hltcount + 5].Merge = true;

                ws.Cells[4, 5, 4, hltcount + 1].Value = "High Level Transactions";

                ws.Cells[4, 1, 5, 1].Value = "S.No";

                //  ws.Cells[4, hltcount + 2, 4, dt.Columns.Count].Merge = true;


                cell1.Style.Font.Color.SetColor(Color.White);
                cell1.Style.Font.Bold = true;

                //Setting the background color of header cells to Gray
                var fill1 = cell1.Style.Fill;
                fill1.PatternType = ExcelFillStyle.Solid;
                fill1.BackgroundColor.SetColor(Color.Red);


                //Setting Top/left,right/bottom borders.
                var border1 = cell1.Style.Border;
                border1.Bottom.Style =
                    border1.Top.Style =
                    border1.Left.Style =
                    border1.Right.Style = ExcelBorderStyle.Thin;

                //Setting Value in cell

                cell1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Alignment is center
                cell1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                foreach (DataColumn dc in dt.Columns) //Creating Headings
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
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Alignment is center
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    colIndex++;

                }
                if (tbl == "Reports")
                {   //fromrow,fromcol,torow,tocol
                    var hltCell = ws.Cells[5, 6, 5, hltcount + 5].Style.TextRotation = 90;
                    // ws.Cells[4, hltcount + 1, 5, ws.Dimension.End.Column].Merge = true;

                }
                ws.Row(5).Height = 222.75;
                //ws.cells[fromrow, fromcol,torow,tocol]

            }
            catch (Exception)
            {
                throw;
            }
        }

        private DataTable CreateBusinessRulesTable(int id, string tbl)
        {
            try
            {


                tbl = "brtbl";

                string constr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
                DataTable dtdb;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_ExportExcelBusinessRules", con))
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@DAId", SqlDbType.Int).Value = id;

                        using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                        {
                            dtdb = new DataTable();
                            dtdb.Columns.AddRange(new DataColumn[1] { new DataColumn("S.No") });
                            dtdb.Columns["S.No"].AutoIncrement = true;
                            dtdb.Columns["S.No"].AutoIncrementSeed = 1;
                            dtdb.Columns["S.No"].AutoIncrementStep = 1;


                            BusinessRulesManager brm = new BusinessRulesManager();
                            IList<tbl_Transactions> lst = brm.GetAllTransactions(id);

                            count = lst.Count;



                            adp.Fill(dtdb);

                            //check this code on monday after taking the latest code and latest database with user table and error table
                            int i = 4;
                            foreach (var item in lst)
                            {
                                var a = item.HighLevelTxnID + "_" + item.HighLevelTxnDesc;

                                DataColumn dc = dtdb.Columns.Add(a);
                                dc.SetOrdinal(i);

                                i++;//the ith value ie the value of the column, start to insert from the 3rd position and henceforth
                            }

                        }
                    }
                }

                return dtdb;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DataTable CreateInterfaceTable(int id)
        {
            try
            {


                string constr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
                DataTable dtdb;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_ExportExcelInterface", con))
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@DAId", SqlDbType.Int).Value = id;

                        using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                        {
                            dtdb = new DataTable();
                            dtdb.Columns.AddRange(new DataColumn[1] { new DataColumn("S.No") });
                            dtdb.Columns["S.No"].AutoIncrement = true;
                            dtdb.Columns["S.No"].AutoIncrementSeed = 1;
                            dtdb.Columns["S.No"].AutoIncrementStep = 1;

                            BusinessRulesManager brm = new BusinessRulesManager();
                            IList<tbl_Transactions> lst = brm.GetAllTransactions(id);

                            count = lst.Count;

                            adp.Fill(dtdb);

                            int i = 7;
                            foreach (var item in lst)
                            {
                                var a = item.HighLevelTxnID + "_" + item.HighLevelTxnDesc;

                                DataColumn dc = dtdb.Columns.Add(a);
                                dc.SetOrdinal(i);

                                i++;//the ith value ie the value of the column, start to insert from the 3rd position and henceforth
                            }
                        }
                    }
                }
                return dtdb;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DataTable CreateChannelsAlertsTable(int id)
        {
            try
            {


                string constr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
                DataTable dtdb;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_ExportExcelChannelsAlerts", con))
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@DAId", SqlDbType.Int).Value = id;

                        using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                        {
                            dtdb = new DataTable();
                            dtdb.Columns.AddRange(new DataColumn[1] { new DataColumn("S.No") });
                            dtdb.Columns["S.No"].AutoIncrement = true;
                            dtdb.Columns["S.No"].AutoIncrementSeed = 1;
                            dtdb.Columns["S.No"].AutoIncrementStep = 1;

                            BusinessRulesManager brm = new BusinessRulesManager();
                            IList<tbl_Transactions> lst = brm.GetAllTransactions(id);

                            count = lst.Count;

                            adp.Fill(dtdb);

                            int i = 4;
                            foreach (var item in lst)
                            {
                                var a = item.HighLevelTxnID + "_" + item.HighLevelTxnDesc;

                                DataColumn dc = dtdb.Columns.Add(a);
                                dc.SetOrdinal(i);

                                i++;//the ith value ie the value of the column, start to insert from the 3rd position and henceforth
                            }
                        }
                    }
                }
                return dtdb;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DataTable CreateReportsTable(int id)
        {
            try
            {


                string constr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
                DataTable dtdb;
                IList<tbl_Transactions> lst = new List<tbl_Transactions>();
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_ExportExcelReports", con))
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@DAId", SqlDbType.Int).Value = id;

                        using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                        {
                            dtdb = new DataTable();
                            dtdb.Columns.AddRange(new DataColumn[1] { new DataColumn("S.No") });
                            dtdb.Columns["S.No"].AutoIncrement = true;
                            dtdb.Columns["S.No"].AutoIncrementSeed = 1;
                            dtdb.Columns["S.No"].AutoIncrementStep = 1;

                            BusinessRulesManager brm = new BusinessRulesManager();
                            lst = brm.GetAllTransactions(id);

                            count = lst.Count;

                            adp.Fill(dtdb);

                            int i = 5;
                            foreach (var item in lst)
                            {
                                var a = item.HighLevelTxnID + "_" + item.HighLevelTxnDesc;

                                DataColumn dc = dtdb.Columns.Add(a);
                                dc.SetOrdinal(i);

                                i++;//the ith value ie the value of the column, start to insert from the 3rd position and henceforth
                            }
                        }
                    }
                }


                return dtdb;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static ExcelWorksheet CreateSheet(ExcelPackage p, string sheetName, DataTable dt, int i)
        {
            try
            {


                p.Workbook.Worksheets.Add(sheetName);
                ExcelWorksheet ws = p.Workbook.Worksheets[i];
                ws.Name = sheetName; //Setting Sheet's name
                ws.Cells.Style.Font.Size = 12; //Default font size for whole sheet
                ws.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet

                return ws;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void CreateTableHeader(DataTable dt, ExcelWorksheet ws, out int colIndex, out int rowIndex, string tbl)
        {
            try
            {


                colIndex = 1;
                rowIndex = 4;

                foreach (DataColumn dc in dt.Columns) //Creating Headings
                {
                    var cell = ws.Cells[rowIndex, colIndex];
                    cell[rowIndex, colIndex, rowIndex + 1, colIndex].Merge = true;

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
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Alignment is center
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    colIndex++;

                }
                if (tbl == "brtbl")
                {
                    var hltCell = ws.Cells[4, 25, 4, ws.Dimension.End.Column].Style.TextRotation = 90;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void AddRows(DataTable dt, ExcelWorksheet ws, ref int colIndex, ref int rowIndex)
        {
            try
            {


                foreach (DataRow dr in dt.Rows) // Adding Data into rows
                {
                    colIndex = 1;
                    rowIndex++;
                    foreach (DataColumn dc in dt.Columns)
                    {
                        var cell = ws.Cells[rowIndex, colIndex];
                        //Setting Value in cell
                        cell.Value = dr[dc.ColumnName];
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

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
            catch (Exception)
            {
                throw;
            }
        }

        private void AddRowsHLT(DataTable dt, ExcelWorksheet ws, ref int colIndex, ref int rowIndex, int count, int daid)
        {
            try
            {


                foreach (DataRow dr in dt.Rows) // Adding Data into rows
                {
                    colIndex = 1;
                    rowIndex++;
                    foreach (DataColumn dc in dt.Columns)
                    {

                        var cell = ws.Cells[rowIndex, colIndex];
                        //Setting Value in cell

                        if (dc.ColumnName == "Auto Generated/Manual")
                        {
                            if (dr[dc.ColumnName].ToString() == "N")
                            {
                                cell.Value = "Autogenerated";
                            }
                            else
                            {
                                cell.Value = "Manual";
                            }
                        }
                        else
                        {
                            cell.Value = dr[dc.ColumnName];
                        }


                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        #region reports
                        if (ws.ToString() == "Reports")
                        {
                            ExportDAVM exportDAVM = new ExportDAVM();
                            IList<tbl_Reports> lst = exportDAVM.GetLinkedDataReports(daid);

                            var reportId = lst.Where(q => q.IsLinked == "1").Select(q => q.ReportID);
                            var HLT = lst.Select(a => a.tbl_Transactions.HighLevelTxnDesc);

                            var transactionSeq = lst.Where(q => q.IsLinked == "1").Select(q => q.TransactionSeq);

                            //this intersect is used to compare and fetch common elements from 2 lists
                            //var intersect = transactionSeq.Intersect(reportsTranSeq);



                            var reportIDfromSheet = dr["ReportID"].ToString();

                            var transactionSeqfromSheet = dr["TransactionSeq"].ToString();

                            dc.ColumnName = dc.ColumnName.ToString().Trim().Substring(dc.ColumnName.ToString().LastIndexOf("_") + 1);

                            var repidfromDB = lst.Where(e => e.tbl_Transactions.HighLevelTxnDesc.Contains(dc.ColumnName)).Select(e => e.ReportID);

                            foreach (var reportid in repidfromDB)
                            {
                                foreach (var txnSqq in transactionSeq)
                                {
                                    if (txnSqq.ToString() == transactionSeqfromSheet)
                                    {
                                        foreach (var hlt in HLT)
                                        {
                                            if (reportIDfromSheet == reportid.ToString() && dc.ColumnName == hlt)
                                            {
                                                cell.Style.Font.Name = "Wingdings";
                                                cell.Value = "ü";
                                                break;
                                            }
                                        }
                                    }
                                }


                            }

                        }
                        #endregion

                        #region business rules
                        else if (ws.ToString() == "Business Rules")
                        {
                            ExportDAVM exportDAVM = new ExportDAVM();
                            IList<tbl_BusinessRule> lstBRData = exportDAVM.GetLinkedDataBR(daid);



                            var buzRuleId = dr["BuzRuleID"];

                            var valFromDB = lstBRData.Where(q => q.BuzRuleID.ToString() == buzRuleId.ToString()).Select(q => q.tbl_Transactions.HighLevelTxnDesc);

                            dc.ColumnName = dc.ColumnName.ToString().Trim().Substring(dc.ColumnName.ToString().LastIndexOf("_") + 1);

                            foreach (var item in valFromDB)
                            {
                                if (item == dc.ColumnName)
                                {
                                    cell.Style.Font.Name = "Wingdings";
                                    cell.Value = "ü";
                                    break;
                                }
                            }

                        }
                        #endregion

                        #region Interface
                        else if (ws.ToString() == "Interfaces")
                        {
                            ExportDAVM exportDAVM = new ExportDAVM();
                            IList<tbl_Interface> lstBRData = exportDAVM.GetLinkedDataInterface(daid);



                            var interfaceId = dr["InterfaceID"];

                            var valFromDB = lstBRData.Where(q => q.InterfaceID.ToString() == interfaceId.ToString()).Select(q => q.tbl_Transactions.HighLevelTxnDesc);

                            dc.ColumnName = dc.ColumnName.ToString().Trim().Substring(dc.ColumnName.ToString().LastIndexOf("_") + 1);

                            foreach (var item in valFromDB)
                            {
                                if (item == dc.ColumnName)
                                {
                                    cell.Style.Font.Name = "Wingdings";
                                    cell.Value = "ü";
                                    break;
                                }
                            }

                        }
                        #endregion

                        #region Channels
                        else if (ws.ToString() == "Channels")
                        {
                            ExportDAVM exportDAVM = new ExportDAVM();
                            IList<tbl_ChannelAlert> lstBRData = exportDAVM.GetLinkedDataForCA(daid);



                            var channelAlertId = dr["ChannelAlertID"];

                            var valFromDB = lstBRData.Where(q => q.ChannelAlertID.ToString() == channelAlertId.ToString()).Select(q => q.tbl_Transactions.HighLevelTxnDesc);

                            dc.ColumnName = dc.ColumnName.ToString().Trim().Substring(dc.ColumnName.ToString().LastIndexOf("_") + 1);

                            foreach (var item in valFromDB)
                            {
                                if (item == dc.ColumnName)
                                {
                                    cell.Style.Font.Name = "Wingdings";
                                    cell.Value = "ü";
                                    break;
                                }
                            }

                        }
                        #endregion


                        #region Transaction Attributes
                        if (ws.ToString() == "Transaction Attributes")
                        {
                            ExportDAVM exportDAVM = new ExportDAVM();
                            IList<tbl_TxnAttributeMapping> lst = exportDAVM.GetLinkedTransactionAttributes(daid);

                            var AttrId = dr["AttributeID"];

                            var AttrIdFromDB = lst.Where(q => q.AttributeID.ToString() == AttrId.ToString()).Select(q => q.tbl_Transactions.HighLevelTxnDesc);



                            foreach (var item in AttrIdFromDB)
                            {
                                dc.ColumnName = dc.ColumnName.ToString().Trim().Substring(dc.ColumnName.ToString().LastIndexOf("_") + 1);

                                if (item == dc.ColumnName)
                                {
                                    cell.Style.Font.Name = "Wingdings";
                                    cell.Value = "ü";
                                    break;
                                }
                            }

                        }
                        #endregion
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
            catch (Exception)
            {
                throw;
            }
        }

        //For reference purpose
        //private static void SaveFile(ExcelPackage p)
        //{
        //    //Generate A File with Random name
        //    Byte[] bin = p.GetAsByteArray();
        //    string guid = Guid.NewGuid().ToString();
        //    string file = "d:\\ExportExcel" + guid + ".xlsx";
        //    System.IO.File.WriteAllBytes(file, bin);
        //    System.Diagnostics.Process.Start(file);
        //}
    }
}