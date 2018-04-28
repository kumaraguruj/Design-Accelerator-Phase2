using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DA.BusinessLayer;
using DA.DomainModel;
using DesignAccelerator.Models.ViewModel;
using System.Data;
using System.Data.SqlClient;
using OfficeOpenXml;
using System.IO;
using System.Threading.Tasks;


namespace DesignAccelerator.Controllers
{
    public class LinkingUIController : Controller
    {
        #region PublicProperties
        ErrorLogViewModel errorlogviewmodel;
        #endregion

        // GET: LinkingUI
        [NoDirectAccess]
        public ActionResult Index(int? id)
        {
            try
            {
                if (id == null)
                    id = (int)TempData["daId"];

                LinkingUIViewModel linkingUIViewModel = new LinkingUIViewModel();
                linkingUIViewModel.GetUIScreenData();

                CommonFunctions comfuns = new CommonFunctions();

                var da = comfuns.FindDA((int)id);
                linkingUIViewModel.daId = (int)id;
                linkingUIViewModel.ModuleId = da.ModuleId;
                linkingUIViewModel.daName = da.DAName;
                TempData["daId"] = linkingUIViewModel.daId;

                int clientId;
                int projectId;
                int applicationId;

                string projectName;
                string appName;
                string modName;

                comfuns.GetModuleName(da.ModuleId, out applicationId, out modName);
                linkingUIViewModel.ApplicationID = applicationId;
                linkingUIViewModel.ModuleName = modName;

                comfuns.GetApplicationName(applicationId, out projectId, out appName);
                linkingUIViewModel.ProjectID = projectId;
                linkingUIViewModel.ApplicationName = appName;

                comfuns.GetProjectName(projectId, out clientId, out projectName);
                linkingUIViewModel.ClientID = clientId;
                linkingUIViewModel.ProjectName = projectName;

                linkingUIViewModel.ClientName = comfuns.GetClientName(clientId);

                return View(linkingUIViewModel);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<ActionResult>Matrix(ProcessCategory processCategory)//
        {
            try
            {
                string result = string.Empty;
                int daId = (int)TempData["daId"];
                TempData["daId"] = daId;
                string filePath = string.Empty;
                HttpPostedFileBase tmFile;
                HttpPostedFileBase tmFile1;
                //HttpPostedFileBase tmFile3;

                switch (processCategory.sProcess)
                {
                    case "Rule of 1 - Txn Matrix":
                        break;
                    case "Rule of N - Txn Matrix":
                        TransactionMatrix transactionMatrix = new TransactionMatrix();

                        Task<string> fipath1 = transactionMatrix.GenerateTransactionMatrix(daId);
                        await Task.WhenAll(fipath1); //
                        fipath1.Wait();
                        if (fipath1.Status == TaskStatus.RanToCompletion)
                            filePath = fipath1.Result;
                        else if (fipath1.Status == TaskStatus.Running)
                        {
                            fipath1.Wait();
                            filePath = fipath1.Result;
                        }
                        break;
                    case "Business Rule":
                        tmFile = processCategory.file.ElementAt(0);
                        GenerateBusinessRules generateBusinessRules = new GenerateBusinessRules();
                        filePath = generateBusinessRules.GenerateBusinessRuleMappingTable(daId, tmFile);
                        break;
                    case "Interface":
                        tmFile = processCategory.file.ElementAt(0);
                        GenerateInterfaces generateInterfaces = new GenerateInterfaces();
                        filePath = generateInterfaces.GenerateInterfaceMappingTable(daId, tmFile);
                        break;
                    case "Channels&Alerts":
                        tmFile = processCategory.file.ElementAt(0);
                        GenerateChannelsAlerts generateChannelsAlerts = new GenerateChannelsAlerts();
                        filePath = generateChannelsAlerts.GenerateChannelsAlertsMappingTable(daId, tmFile);
                        break;
                    case "Reports":
                        tmFile = processCategory.file.ElementAt(0);
                        GenerateReports generateReports = new GenerateReports();
                        filePath = generateReports.GenerateReportsMappingTable(daId, tmFile);
                        break;
                    //To generate scenario builder Template file
                    case "Scenario Builder Template":
                        ScenarioBuilder scenarioBuilderTemplate = new ScenarioBuilder();
                        filePath = scenarioBuilderTemplate.CreateSBTemplateFile(daId);
                        break;
                    case "Scenario Stitching":
                        tmFile = processCategory.file.ElementAt(0);
                        tmFile1 = processCategory.file.ElementAt(1);
                        ScenarioBuilder scenarioBuilder = new ScenarioBuilder();
                        //filePath = scenarioBuilder.GenerateScenarioBuilder(daId, tmFile, tmFile1); //
                        Task<string> fipath = scenarioBuilder.GenerateScenarioBuilder(daId, tmFile, tmFile1);
                        await Task.WhenAll(fipath); //
                        fipath.Wait();
                        if (fipath.Status == TaskStatus.RanToCompletion)
                            filePath = fipath.Result;
                        else if (fipath.Status == TaskStatus.Running)
                        {
                            fipath.Wait();
                            filePath = fipath.Result;
                        }
                        break;
                    case "Test Design":
                        tmFile = processCategory.file.ElementAt(0);
                        tmFile1 = processCategory.file.ElementAt(1);
                        TestDesignController testDesign = new TestDesignController();
                        // filePath = testDesign.GenerateTestDesign(daId, tmFile,tmFile1); //
                        Task<string> fpath = testDesign.GenerateTestDesign(daId, tmFile, tmFile1);
                        await Task.WhenAll(fpath); //
                        fpath.Wait();
                        if (fpath.Status == TaskStatus.RanToCompletion)
                            filePath = fpath.Result;
                        else if (fpath.Status == TaskStatus.Running)
                        {
                            fpath.Wait();
                            filePath = fpath.Result;
                        }
                        break;
                    case "Traceability Matrix":
                        tmFile = processCategory.file.ElementAt(0);
                        tmFile1 = processCategory.file.ElementAt(1);
                        TraceabilityMatrix traceabilityMatrix = new TraceabilityMatrix();
                        filePath = traceabilityMatrix.GenerateTraceabilityMatrix(daId, tmFile, tmFile1); //
                        break;
                    case "Run Plan":
                        tmFile = processCategory.file.ElementAt(2);
                        RunPlan runPlan = new RunPlan();
                        filePath = runPlan.CreateRunPlanFile(daId, tmFile); //
                        break;
                    case "Export DA":
                        ExportDesignAcceleratorController exportDA = new ExportDesignAcceleratorController();
                        filePath = exportDA.ExportDAFile(daId);
                        break;

                }


                var errorMessage = "you can return the errors in here!";

                //return the Excel file name
                return Json(new { fileName = filePath, errorMessage = "" });
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }

        [HttpGet]
        [DeleteFileAttribute] //Action Filter, it will auto delete the file after download, 
                             
        public ActionResult Download(string filePath)
        {
            try
            {
                int daId = (int)TempData["daId"];
                TempData["daId"] = daId;
                //get the temp folder and file path in server
                //string fullPath = Path.Combine(Server.MapPath("~/temp"), fileName);

                //return the file for download, this is an Excel 
                //so I set the file content type to "application/vnd.ms-excel"
                string fileName = filePath.Substring(8);
                return File(filePath, "application/vnd.ms-excel", fileName);
            }
            catch (Exception ex)
            {
                errorlogviewmodel = new ErrorLogViewModel();
                errorlogviewmodel.LogError(ex);
                return View("Error");
            }
        }
        
    }
    public class ProcessCategory
    {
        public string sProcess { get; set; }
        public IEnumerable<HttpPostedFileBase> file { get; set; }


    }
}