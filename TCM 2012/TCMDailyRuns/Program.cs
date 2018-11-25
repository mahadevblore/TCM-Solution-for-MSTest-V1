using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TCMDailyRuns;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.TestManagement.Client;
using System.Net;

namespace TCMDailyRuns
{
    public class Program:Logger
    {
        //public static string curDir = Path.GetPathRoot(Environment.SystemDirectory);
        public static string sTime = DateTime.Now.ToString("MM/dd/yyyy");
        public static string curDir = GetTempPath();
        public static string exePath =System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
        public static string tmp = @"Temp_DTR\";
        public static string recent;
        public static string destFilepath;
        public static string trxFilePath;
        public static string RunId;
        public static string sourceFilePath;
        public static string tcmPath;
        public static string resultFile;
        public static string buildPath;
        public static bool controller = false;
        public static bool firstTry = false;
        public static bool prFailure = false;
        public static SelectedPlan sp = new SelectedPlan();
        public static bool testExecutionFailureOutcome = true;
        public static bool testReRunOutcome = true;

        static void Main(string[] args)
    {
            LogMessageToFile("------------------------------------------------------------------");
            LogMessageToFile("Started New Session");
            LogMessageToFile("Path of the exe started is " + exePath);

            try
            {
                
                ExecutionBlock();

                //TFSFunctions.ReadLogFile("7781");
                //TFSFunctions.ResetCount("7792");

                PostExecutionEvents();
                
            }
            catch (Exception ex)
            {
                LogMessageToFile("Severe exception caused the exe to shutdown. The exception details are :" + ex.ToString() + " : " + ex.StackTrace.ToString());
            }
            finally
            {
                if (File.Exists(curDir + "\\" + tmp))
                {
                    Directory.Delete(curDir + "\\" + tmp);
                }
            }
        }

        private static void ExecutionBlock()
        {
            try
            {
                InitialCleanUp();

                ReadAndCreateXMLs();

                CopyBuildBinaries();

                SetTCMPath();

                //TFSFunctions.WaitForController();

                ProjectCustomization.InitiateCustomization();

                TestRunLogicBlock();

                ExecutionValidation();
            }
            catch (Exception ex)
            {
                LogMessageToFile("The exception caused at the execution block is : " + ex.ToString() +Environment.NewLine+
                    "The stack trace of the exception is - "+ Environment.NewLine+ ex.StackTrace.ToString());
                testExecutionFailureOutcome = true;
                ExecutionValidation();
            }
        }

        private static void PostExecutionEvents()
        {
            try
            {
                if ((ConfigFile.order == 0))
                {
                    try
                    {
                        EmailNotification.SendEmail();
                    }
                    catch (Exception e)
                    {
                        LogMessageToFile("The exception caused at the post execution block while sending email. The exception is : " + e.Message);
                    }
                }
            }
            catch//catch if order is not mentioned
            {
                try
                {
                    EmailNotification.SendEmail();
                }
                catch (Exception e)
                {
                    LogMessageToFile("The exception caused at the post execution block while sending email. The exception is : " + e.Message);
                }
            }
            if (testExecutionFailureOutcome==true)
            {
                LogMessageToFile("Test Failure Outcome was True");
                XML.GenerateForFailure(0, sp, "O");

                try
                {
                    //if (ConfigFile.type.ToUpper() == "CONTROLLER")
					if (ConfigFile.order == 0)
                    {
                        ReportToSplunk();
                    }
                }
                catch
                { ReportToSplunk(); }

                ResetSplunkVariables();

                int k = 1;
                while (testExecutionFailureOutcome==true)
                {
                    ProjectCustomization.ReRunCustomization();
                    ExecutionBlock();
                    k++;
                    if (k == ConfigFile.retryAttemptForFailure)
                    {
                        break;
                    }
                }
            }
            else
            {
                //if (ConfigFile.type.ToUpper() == "CONTROLLER")
                if (ConfigFile.order == 0)
                {
                    ReportToSplunk();
                }
            }
            LogMessageToFile("Session Ended");
            LogMessageToFile("------------------------------------------------------------------");
        }

        private static void ResetSplunkVariables()
        {
            //Re-Initialising the variables
            sp = new SelectedPlan();
            recent = null;
            destFilepath = null;
            trxFilePath = null;
            RunId = null;
            sourceFilePath = null;
            tcmPath = null;
            resultFile = null;
            buildPath = null;
            controller = false;
            firstTry = false;
            prFailure = false;
        }

        private static void ReportToSplunk()
        {
            Console.WriteLine("Clean Up for the current exe was initiated...");
            File.Copy(XML.combinedPath, Dashboard.xmlPath + XML.splogFilename, true);
            LogMessageToFile("Copied file : " + XML.combinedPath + " to the location : " + Dashboard.xmlPath + XML.splogFilename);
            File.Copy(XML.combinedPath, Dashboard.xmlBackUp + XML.splogFilename, true);
            LogMessageToFile("Copied file : " + XML.combinedPath + " to the location : " + Dashboard.xmlPath + XML.splogFilename);
            Directory.Delete(curDir + tmp, true);
            Console.WriteLine("Done");
        }

        private static void ExecutionValidation()
        {
			string type = "CONTROLLER";

            try
            {
                if (ConfigFile.order != 0)
                {
                    type = "EXECUTOR";
                }
            }
            catch (Exception e)
            {
                type = "CONTROLLER";
            }
            testExecutionFailureOutcome = XML.ReadGeneratedSplogFileAndAssertOutcome(XML.combinedPath);

            if (ConfigFile.waitForReRun)
            {
                if (ConfigFile.splitRun || ConfigFile.distribute)
                {
                    switch (type)
                    {
                        case "CONTROLLER":
                            for (int i = 1; i <= ConfigFile.systems - 1; i++)
                            {
                                if (ConfigFile.splitRun != true)
                                {
                                    TFSFunctions.WaitForExecutor(i);
                                }
                                else
                                {
                                    TFSFunctions.WaitForExecutor(i, true);
                                }
                            }
                            break;

                        case "EXECUTOR":
                            if (ConfigFile.splitRun != true)
                            {
                                TFSFunctions.WaitForController();
                            }
                            else
                            {
                                TFSFunctions.WaitForController(true);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            int runOutcome = TFSFunctions.EvaluateResultFromTFS(sp.curRunId);

            if (runOutcome==2||runOutcome==3)
            {
                ProjectCustomization.ReRunCustomization();
                LogMessageToFile("RunOutcome was "+runOutcome.ToString());
            }
            CollectResultsAfterReRun();
        }

        private static void TestRunLogicBlock()
        {
            if (ConfigFile.splitRun)
            {
                RunWithSplitDistribution();
            }
            else if (ConfigFile.distribute)
            {
                RunWithDistribution();
            }
            else
            {
                RunWithoutDistribution();
            }
        }
        
        private static void RunWithDistribution()
        {
            int sCount = 0;
            int eCount = 0;

            TFSFunctions.GetTestPointsCount();

            LogMessageToFile("The total number of test cases are - " + TFSFunctions.totalPoints.ToString());

            int average = (TFSFunctions.totalPoints / ConfigFile.systems);

            //if (ConfigFile.type.ToUpper() == "Controller".ToUpper())
            if (ConfigFile.order == 0)
            {
                eCount = average;
            }
            else if (ConfigFile.order > 0 && ConfigFile.order < ConfigFile.systems - 1)
            {
                sCount = (average * ConfigFile.order) + 1;
                eCount = average * (ConfigFile.order + 1);
            }
            else
            {
                sCount = (average * ConfigFile.order) + 1;
                eCount = TFSFunctions.totalPoints-1;
            }

			if (ConfigFile.Runtitle != null)
            {
                ConfigFile.Runtitle = ConfigFile.Runtitle + "_DR_";
                TFSFunctions.CreateTestRun(ConfigFile.sId, sCount, eCount); //change due to run title
            }
            else
            {
                ConfigFile.Runtitle = "DR_";
                TFSFunctions.CreateTestRun(ConfigFile.sId, sCount, eCount);
            }

            sp.StartTime = DateTime.Now.ToString();

            TCMFunctions.TCMExecute(sp.runId);

            TFSFunctions.CollectResultFromTFS(sp.runId);

            //if (ConfigFile.type.ToUpper() == "Controller".ToUpper())
            if(ConfigFile.order == 0)
            {
                //Handling the controller run failure. If condition to check the controller's run information.
                XML.Generate(sp, "O");
                if(XML.ReadGeneratedSplogFileAndAssertOutcome(XML.combinedPath))
                {
                    throw new Exception("Test Outcome Failure was True.");
                }

                for (int i = 1; i <= ConfigFile.systems - 1; i++)
                {
                    sp.EndTime = DateTime.Now.ToString();
                    TFSFunctions.CollectResultFromTFS(i);
                }
            }
            XML.Generate(sp, "O");
        }

        private static void CollectResultsAfterReRun()
        {
            LogMessageToFile("Started ReRun result collection.");

            //if (ConfigFile.distribute && !ConfigFile.splitRun && ConfigFile.type.ToUpper() == "Controller".ToUpper())
            
            //else if (ConfigFile.splitRun && ConfigFile.type.ToUpper() == "Controller".ToUpper())
            if (ConfigFile.splitRun && ConfigFile.order == 0)
            {
                LogMessageToFile("Logic has entered SplitRun-True Module");

                if (!ConfigFile.skipReRun)
                {
                    for (int i = 1; i <= ConfigFile.systems - 1; i++)
                    {
                        LogMessageToFile("The int value of the order is : " + i.ToString());
                        TFSFunctions.WaitForReRunResults(i, true);

                    }

                    LogMessageToFile("ReRun resetting selected plan values.");
                }
                XML.ClearSelectedPlan();
                LogMessageToFile("ReRun resetting selected plan values. - completed");
                TFSFunctions.CollectResultFromTFS(sp.curRunId);
                for (int i = 1; i <= ConfigFile.systems - 1; i++)

                {
                    LogMessageToFile("The int value of the order is : " + i.ToString());

                    sp.EndTime = DateTime.Now.ToString();
                    TFSFunctions.CollectResultFromTFS(i, true);

                }
                XML.Generate(sp, "O");
            }
            else if (ConfigFile.distribute && ConfigFile.order == 0)
            {
                LogMessageToFile("Logic has entered SplitRun-False Module");

                if (!ConfigFile.skipReRun)
                {
                    for (int i = 1; i <= ConfigFile.systems - 1; i++)
                    {
                        LogMessageToFile("The int value of the order is : " + i.ToString());
                        TFSFunctions.WaitForReRunResults(i);

                    }

                    LogMessageToFile("ReRun resetting selected plan values.");
                }
                XML.ClearSelectedPlan();
                LogMessageToFile("ReRun resetting selected plan values. - completed");
                TFSFunctions.CollectResultFromTFS(sp.curRunId);
                for (int j = 1; j <= ConfigFile.systems - 1; j++)

                {
                    LogMessageToFile("The int value of the order is : " + j.ToString());

                    sp.EndTime = DateTime.Now.ToString();
                    TFSFunctions.CollectResultFromTFS(j);

                }
                XML.Generate(sp, "O");
            }
            else
            {
                LogMessageToFile("ReRun resetting selected plan values.");
                XML.ClearSelectedPlan();
                LogMessageToFile("ReRun resetting selected plan values. - completed");
                TFSFunctions.CollectResultFromTFS(sp.curRunId);
                XML.Generate(sp, "O");
            }
        } 

        private static void RunWithSplitDistribution()
        {
            //if (ConfigFile.type.ToUpper() == "Controller".ToUpper())
            if (ConfigFile.order == 0) //i.e. controller
            {
                LogMessageToFile("The Machine is the controller");
				if(ConfigFile.Runtitle != null)
                {
                    ConfigFile.Runtitle = ConfigFile.Runtitle + "_SR_";
                    TFSFunctions.CreateTestRun(ConfigFile.controllerSuite, 0, 0, true);  //change due to run title
                }
                else
                {
                    ConfigFile.Runtitle = "SR_";
                    TFSFunctions.CreateTestRun(ConfigFile.controllerSuite, 0, 0, true);  //change due to run title
                }
                sp.StartTime = DateTime.Now.ToString();

                TCMFunctions.TCMExecute(sp.runId);

                TFSFunctions.CollectResultFromTFS(sp.runId);

                    //Handling the controller run failure. If condition to check the controller's run information.
                    XML.Generate(sp, "O");
                    if (XML.ReadGeneratedSplogFileAndAssertOutcome(XML.combinedPath))
                    {
                        throw new Exception("Test Outcome Failure was True.");
                    }

                    //for (int i = 1; i <= ConfigFile.systems - 1; i++)
                    //{
                        sp.EndTime = DateTime.Now.ToString();
                        //TFSFunctions.CollectResultFromTFS(i, true);
                    //}
            }
            else
            {
                LogMessageToFile("The Machine is not the controller");
                string[] sysSuites = ConfigFile.executorSuite.Split(':');
                string sId = sysSuites[ConfigFile.order-1];
				if (ConfigFile.Runtitle != null)
                {
                    ConfigFile.Runtitle = ConfigFile.Runtitle + "_SR_";
                    TFSFunctions.CreateTestRun(sId, 0, 0, true);  //change due to run title
                }
                else
                {
                    ConfigFile.Runtitle = "SR_";
                    TFSFunctions.CreateTestRun(sId, 0, 0, true); //change due to run title
                }
                TCMFunctions.TCMExecute(sp.runId);
                TFSFunctions.CollectResultFromTFS(sp.runId);
            }
            XML.Generate(sp, "O");
        }

        private static void RunWithoutDistribution()
        {
			if(ConfigFile.Runtitle != null)
            {
                ConfigFile.Runtitle = ConfigFile.Runtitle + "_Daily Run_";
            }
            else
            {
                ConfigFile.Runtitle = "Daily Run_";
            }
            //ConfigFile.type = "Controller";
            if (!string.IsNullOrWhiteSpace(ConfigFile.sId))
            {
                TCMFunctions.TCMCreateUsingSuiteId(ConfigFile.sId);
            }
            else
            {
                TCMFunctions.TCMCreate();
            }

            sp.StartTime = DateTime.Now.ToString();

            TCMFunctions.TCMExecute(sp.runId);

            sp.EndTime = DateTime.Now.ToString();

            Console.WriteLine("TFS Data Collection Started ...");
            TFSFunctions.CollectResultFromTFS(sp.curRunId);
            XML.Generate(sp, "O");
            LogMessageToFile("TFS Data Collection Started");
            Console.WriteLine("Done");
        }

        private static void ExportTRXAndUpdateXML()
        {
            if (string.IsNullOrWhiteSpace(resultFile))
            {
                TCMFunctions.TCMExport();
            }
            if (firstTry == false)
            {
                int counter = 0;
                while (!File.Exists(resultFile))
                {
                    counter += 1;
                    System.Threading.Thread.Sleep(30000);
                    Console.WriteLine("Counter : " + counter.ToString() + " - Trying to export result for the current test run : " + sp.runId);
                    TCMFunctions.TCMExport();
                }
            }
        }

        private static void ReadAndCreateXMLs()
        {
            //Loading Xmls
            XML.ReadConfigFile();
            XML.CreateXMLFile();
        }

        private static void CopyBuildBinaries()
        {
            //Added foreach loop to download multiple build drop - binaries
            string goodBuildPath = null;
            string[] definitions = null;
            string[] buildPaths = null;

            definitions = ConfigFile.def.Split(',');
            buildPaths = ConfigFile.actDrpPath.Split(',');

            for (int i = 0; i <= definitions.Count()-1; i++)
            {
                goodBuildPath = TFSFunctions.GetBuildPath(goodBuildPath,definitions[i],buildPaths[i]);
                LogMessageToFile("Good Build path is : " + goodBuildPath);
                Console.WriteLine("Good Build path is : " + goodBuildPath);
                LogMessageToFile("HostName and destination are different");
                buildPath = destFilepath = curDir + @"Temp_DTR\Drops\" + ConfigFile.proj + "_" + recent;
                Directory.CreateDirectory(destFilepath);
                Console.WriteLine("Initiated ROBOCOPY for source : " + sourceFilePath + " and Destination :" + destFilepath);
                CmdLine.RBCopy();
                Console.WriteLine("Destination Path is : " + destFilepath);
                LogMessageToFile("Destination Path is : " + destFilepath);
            }

            //Removing the last directory for fixing multiple build issue
            destFilepath = destFilepath.Split(new string[] { ConfigFile.proj+"_" }, StringSplitOptions.None)[0];
            destFilepath = destFilepath.Substring(0, destFilepath.ToCharArray().Count() - 1);
        }

        private static void SetTCMPath()
        {
            if (File.Exists(@"C:\Program Files\Microsoft Visual Studio 11.0\Common7\IDE\TCM.exe"))
            {
                tcmPath = @"C:\Program Files\Microsoft Visual Studio 11.0\Common7\IDE\";
            }
            else if (!File.Exists(@"C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\IDE\TCM.exe"))
            {
                string[] tcmPaths = CmdLine.GetTcmPath().Split(new string[] { "Directory of" }, StringSplitOptions.None)[1].Split(new string[] { "IDE" }, StringSplitOptions.None);
                tcmPath = tcmPaths[0].Trim() + @"IDE\";
            }
            else
            {
                tcmPath = @"C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\IDE\";
            }
            LogMessageToFile("The TCM executable path is : " + tcmPath);
            Console.WriteLine("The TCM Path is : " + tcmPath);
        }

        private static void InitialCleanUp()
        {
            if (Directory.Exists(curDir + "\\" + tmp))
            {
                Directory.Delete(curDir + "\\" + tmp, true);
            }
            Directory.CreateDirectory(curDir + "\\" + tmp);

            LogMessageToFile("Creating directory : " + curDir + "\\" + tmp);
        }
    }
}