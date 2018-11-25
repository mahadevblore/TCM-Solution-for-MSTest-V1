using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMDailyRuns
{
    public class TCMFunctions : Program
    {

        /// <summary>
        /// TCM function to create a test run using query method
        /// </summary>
        /// <returns></returns>
        public static string TCMCreateUsingSuiteId(string sId)
        {
            LogMessageToFile("TCM Create Using Suite Id Function started");
            Console.WriteLine("TCM Create Using Suite Id Function started...");
            string queryText = null;

            queryText = ExtractQueryForSuiteIDs(sId, queryText);

            LogMessageToFile("The Query Text that was passed to TCM run create function is : " + queryText);

            string[] lines = {   "cd /",
                                 "cd "+tcmPath,                                  
                                 "tcm run /create /title:"+"\""+ ConfigFile.Runtitle +recent+"\""+" /planid:"
                                 +ConfigFile.pId+" /querytext:"+queryText
                                 +" /builddir:"+"\""+destFilepath+"\""+" /collection:"+ConfigFile.col+" /teamproject:"
                                 +"\""+ConfigFile.proj+"\""+" /settingsname:"+"\""+ConfigFile.settingsnme+"\""+" /include"
                                 };
            string tcmBat = curDir + tmp + "tcmCreate.bat";
            LogBatFileOutput(lines, tcmBat);
            System.IO.File.WriteAllLines(tcmBat, lines);
            string strTmp = CmdLine.GetCmdOut(tcmBat, true);
            LogMessageToFile(strTmp);
            if (strTmp.Contains("created with ID: "))
            {
                LogMessageToFile("Run Id was created successfully using tcm create using suite id function");

                sp.curRunId = sp.runId = strTmp.Split(new string[] { "created with ID: " }, StringSplitOptions.None)[1].Split('.')[0].Trim();
                Console.WriteLine("Run ID generated is : " + sp.runId);
                LogMessageToFile("The Run ID generated was : " + sp.runId);
            }
            else
            {
                LogMessageToFile("Run Id was created successfully using tcm create using suite id function for controller triggered run");
                sp.curRunId = sp.runId = TCMCreateController();
                Console.WriteLine("Run ID generated is : " + sp.runId);
            }
            Console.WriteLine("Done");
            return sp.runId;
        }

        /// <summary>
        /// Function that constructs query for supporting test case gathering
        /// </summary>
        /// <param name="sId"></param>
        /// <param name="queryText"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string ExtractQueryForSuiteIDs(string sId,string queryText,bool type =false)
        {
            if (sId.Contains(','))
            {
                string sIdQuery = null;
                string[] sIds = sId.Split(',');
                for (int i = 0; i <= sIds.Count() - 1; i++)
                {
                    if (i == sIds.Count() - 1)
                    {
                        if (string.IsNullOrWhiteSpace(sIds[i]))
                        {
                            sIdQuery.Substring(sIdQuery.Length - 14, 0);
                        }
                        else
                        {
                            sIdQuery += sIds[i];
                        }
                    }
                    else
                    {
                        sIdQuery += sIds[i] + " or SuiteId =";
                    }
                }
                if (!type)
                {
                    queryText = "\"" + "SELECT * FROM TestPoint WHERE (SuiteId=" + sIdQuery + ") and (ConfigurationId="
                                     + ConfigFile.cId + " and PlanId =" + ConfigFile.pId + ")\"";
                }
                else
                {
                    queryText = "SELECT * FROM TestPoint WHERE (SuiteId=" + sIdQuery + ") and (ConfigurationId="
                                        + ConfigFile.cId + " and PlanId =" + ConfigFile.pId+")";
                }
            }
            else
            {
                if (!type)
                {
                    queryText = "\"" + "SELECT * FROM TestPoint WHERE (SuiteId=" + sId + ") and (ConfigurationId="
                        + ConfigFile.cId + " and PlanId =" + ConfigFile.pId + ")\"";
                }
                else
                {
                    queryText = "SELECT * FROM TestPoint WHERE (SuiteId=" + sId + ") and (ConfigurationId="
                            + ConfigFile.cId + " and PlanId =" + ConfigFile.pId+")";
                }
            }
            return queryText;
        }

        /// <summary>
        /// TCM function to create a test run using query method
        /// </summary>
        /// <returns></returns>
        public static string TCMCreate()
        {
            LogMessageToFile("TCM Create Function started");
            Console.WriteLine("TCM Create Function started...");

            string[] lines = {   "cd /",
                                 "cd "+tcmPath,                                  
                                 "tcm run /create /title:"+"\""+ConfigFile.Runtitle+recent+"\""+" /planid:"
                                 +ConfigFile.pId+" /querytext:"+"\""
                                 +"SELECT * FROM TestPoint WHERE PlanId="+ConfigFile.pId+" and ConfigurationId="+ConfigFile.cId+"\""
                                 +" /builddir:"+"\""+destFilepath+"\""+" /collection:"+ConfigFile.col+" /teamproject:"
                                 +"\""+ConfigFile.proj+"\""+" /settingsname:"+"\""+ConfigFile.settingsnme+"\""+" /include"
                                 };
            string tcmBat = curDir + tmp + "tcmCreate.bat";
            LogBatFileOutput(lines, tcmBat);
            System.IO.File.WriteAllLines(tcmBat, lines);
            string strTmp = CmdLine.GetCmdOut(tcmBat,true);

            if (strTmp.Contains("created with ID: "))
            {
                LogMessageToFile("Run Id was created successfully using tcm create function");
                sp.curRunId = sp.runId = strTmp.Split(new string[] { "created with ID: " }, StringSplitOptions.None)[1].Split('.')[0].Trim();
                Console.WriteLine("Run ID generated is : " + sp.runId);
                LogMessageToFile("The Run ID generated was : " + sp.runId);
            }
            else
            {
                LogMessageToFile("Run Id was created successfully using tcm create function for controller triggered run");
                sp.curRunId = sp.runId = TCMCreateController();
                Console.WriteLine("Run ID generated is : " + sp.runId);
            }
            Console.WriteLine("Done");
            return sp.runId;
        }

        /// <summary>
        /// TCM function to create a test run using query method
        /// </summary>
        /// <returns></returns>
        public static string TCMCreateByPriority()
        {
            LogMessageToFile("TCMCreateByPriority Function started");
            Console.WriteLine("TCMCreateByPriority Function started...");

            string[] lines = {   "cd /",
                                 "cd "+tcmPath,                                  
                                 "tcm run /create /title:"+"\""+"Daily Run_"+recent+"\""+" /planid:"
                                 +ConfigFile.pId+" /querytext:"+"\""
                                 +"SELECT * FROM TestPoint WHERE PlanId="+ConfigFile.pId+" and ConfigurationId="+ConfigFile.cId+" order by Priority\""
                                 +" /builddir:"+"\""+destFilepath+"\""+" /collection:"+ConfigFile.col+" /teamproject:"
                                 +"\""+ConfigFile.proj+"\""+" /settingsname:"+"\""+ConfigFile.settingsnme+"\""+" /include"
                                 };
            string tcmBat = curDir + tmp + "tcmCreate.bat";
            LogBatFileOutput(lines, tcmBat);
            System.IO.File.WriteAllLines(tcmBat, lines);
            string strTmp = CmdLine.GetCmdOut(tcmBat,true);

            if (strTmp.Contains("created with ID: "))
            {
                LogMessageToFile("Run Id was created successfully using tcm create function");
                sp.curRunId = sp.runId = strTmp.Split(new string[] { "created with ID: " }, StringSplitOptions.None)[1].Split('.')[0].Trim();
                Console.WriteLine("Run ID generated is : " + sp.runId);
                LogMessageToFile("The Run ID generated was : " + sp.runId);
            }
            else
            {
                LogMessageToFile("Run Id was created successfully using tcm create function for controller triggered run");
                sp.curRunId = sp.runId = TCMCreateController();
                Console.WriteLine("Run ID generated is : " + sp.runId);
            }
            Console.WriteLine("Done");
            return sp.runId;
        }

        /// <summary>
        /// TCM function to create test run for particular suite. 
        /// Could be used to run pre-requisite flows
        /// </summary>
        /// <param name="testMode"></param>
        /// <returns></returns>
        public static string TCMPRCreateAndExecute(string sId,string pId,string cId)
        {
            string prTitle = "Pre-Requisite_"+recent;
            string[] lines = {   "cd /",
                                 "cd "+tcmPath,                                  
                                 "tcm run /create /title:"+"\""+prTitle+"\""+" /planid:"
                                 +pId+" /suiteid:"+sId +" /configid:"+cId
                                 +" /builddir:"+"\""+destFilepath+"\""+" /collection:"+ConfigFile.col+" /teamproject:"
                                 +"\""+ConfigFile.proj+"\""+" /settingsname:"+"\""+ConfigFile.settingsnme+"\""+" /include"};
            string tcmPRBat = curDir + tmp + "tcmPRCreate.bat";
            System.IO.File.WriteAllLines(tcmPRBat, lines);
            LogBatFileOutput(lines, tcmPRBat);
            string strTmp = CmdLine.GetCmdOutput(tcmPRBat);
            if (strTmp.Contains("created with ID: "))
            {
                LogMessageToFile("Pre Requisite run Id was created successfully using tcm create overload function");
                sp.prRunId = strTmp.Split(new string[] { "created with ID: " }, StringSplitOptions.None)[1].Split('.')[0].Trim();
                Console.WriteLine("Pre Requisite run ID generated is : " + sp.prRunId);
                LogMessageToFile("Pre Requisite run ID generated is : " + sp.prRunId);
            }
            TCMPrExecute(sp.prRunId);

            return sp.prRunId;
        }

        /// <summary>
        /// TCM function to create test run for particular suite. 
        /// Could be used to run pre-requisite flows
        /// </summary>
        /// <param name="testMode"></param>
        /// <returns></returns>
        public static string TCMPRCreateAndExecuteM2M(string sId, string pId, string cId)
        {
            string prTitle = "Pre-Requisite_" + recent;
            string[] lines = {   "cd /",
                                 "cd "+tcmPath,                                  
                                 "tcm run /create /title:"+"\""+prTitle+"\""+" /planid:"
                                 +pId+" /suiteid:"+sId +" /configid:"+cId
                                 +" /builddir:"+"\""+destFilepath+"\""+" /collection:"+ConfigFile.col+" /teamproject:"
                                 +"\""+ConfigFile.proj+"\""+" /settingsname:"+"\""+ConfigFile.settingsnme+"\""+" /include"};
            string tcmPRBat = curDir + tmp + "tcmPRCreate.bat";
            System.IO.File.WriteAllLines(tcmPRBat, lines);
            LogBatFileOutput(lines, tcmPRBat);
            string strTmp = CmdLine.GetCmdOut(tcmPRBat,true);
            if (strTmp.Contains("created with ID: "))
            {
                LogMessageToFile("Pre Requisite run Id was created successfully using tcm create overload function");
                sp.prRunId = strTmp.Split(new string[] { "created with ID: " }, StringSplitOptions.None)[1].Split('.')[0].Trim();
                Console.WriteLine("Pre Requisite run ID generated is : " + sp.prRunId);
                LogMessageToFile("Pre Requisite run ID generated is : " + sp.prRunId);
            }
            TCMPrExecute(sp.prRunId);

            if (!TFSFunctions.GetTestRunState(prTitle).Contains("Completed"))
            {
                throw new Exception("PR Suite Failed");
            }

            return sp.prRunId;
        }

        /// <summary>
        /// Creating Controller triggered test run
        /// </summary>
        /// <returns></returns>
        public static string TCMCreateController()
        {
            string[] cntrlines = { "cd /",
                                 "cd "+tcmPath, 
                                 "tcm run /create /title:"+"\""+"Daily Run_"+recent+"\""+" /planid:"
                                 +ConfigFile.pId+" /querytext:"+"\""
                                 +"SELECT * FROM TestPoint WHERE PlanId="+ConfigFile.pId+" and ConfigurationId="+ConfigFile.cId+"\""
                                 +" /builddir:"+"\""+destFilepath+"\""+" /collection:"+ConfigFile.col+" /teamproject:"
                                 +"\""+ConfigFile.proj+"\""+" /flavor:"+ConfigFile.Flav+" /platform:"+ConfigFile.platfrm
                                 +" /testenvironment:"+ConfigFile.environmnt+" /settingsname:"+"\""+ConfigFile.settingsnme+"\""+" /include"
                                  };
            string tcmControllerBat = curDir + tmp + "tcmControlCreate.bat";
            LogBatFileOutput(cntrlines, tcmControllerBat);
            System.IO.File.WriteAllLines(tcmControllerBat, cntrlines);
            string tcmCreateControllerOut = CmdLine.GetCmdOut(tcmControllerBat,true);
            LogMessageToFile("The Command Output is : " + tcmCreateControllerOut);
            if (tcmCreateControllerOut.Contains("created with ID: "))
            {
                controller = true;
                sp.curRunId = sp.runId = tcmCreateControllerOut.Split(new string[] { "created with ID: " }, StringSplitOptions.None)[1].Split('.')[0].Trim();
            }
            return sp.runId;
        }

        /// <summary>
        /// TCM Command line to execute the test run using TCM Create Command
        /// </summary>
        /// <returns>Output of the command line</returns>
        public static string TCMExecute(string rId)
        {
            LogMessageToFile("TCM Execute Function started");
            Console.WriteLine("TCM Execute Function started...");

            string[] cntrlines = {"cd "+tcmPath,    
                                  "for /f "+"\""+"delims="+"\""+" %%a in ('"+"\""+"powershell get-date -format G"+"\""+"') do set t=%%a",
                                  "call:While",
                                  ":While",
                                  "tcm run /execute /id:"+rId+" /collection:"+ConfigFile.col+" /teamproject:"+"\""+ConfigFile.proj+"\"",
                                  "if %ERRORLEVEL% GTR 0 goto While",
                                  "cd %windir%",
                                  "cd system32",
                                  "Taskkill /F /IM IEDriverServer.exe",
                                  "Taskkill /F /IM iexplore.exe",
                                  "Taskkill /F /IM chromedriver.exe",
                                  "Taskkill /F /IM chrome.exe",
                                  "Taskkill /F /IM firefox.exe",
                                  "exit"
                                 };
            string tcmExecuteBat = curDir + tmp + "tcmExecute.bat";
            LogBatFileOutput(cntrlines, tcmExecuteBat);
            System.IO.File.WriteAllLines(tcmExecuteBat, cntrlines);
            string tcmExecuteOut = CmdLine.GetCmdOut(tcmExecuteBat,true);
            LogMessageToFile("The Command Output is : " + tcmExecuteOut);
            Console.WriteLine("Done");
            Console.WriteLine("The Output of the execution is : "+tcmExecuteOut);
            return tcmExecuteOut;
        }

        /// <summary>
        /// TCM Command line to execute the test run using TCM Create Command
        /// </summary>
        /// <returns>Output of the command line</returns>
        public static string TCMPrExecute(string rId)
        {
            LogMessageToFile("TCM Execute Function started");
            Console.WriteLine("TCM Execute Function started...");

            string[] cntrlines = {"cd "+tcmPath,
                                  "tcm run /execute /id:"+rId+" /collection:"+ConfigFile.col+" /teamproject:"+"\""+ConfigFile.proj+"\"",
                                  "cd %windir%",
                                  "cd system32",
                                  "Taskkill /F /IM IEDriverServer.exe",
                                  "Taskkill /F /IM iexplore.exe",
                                  "Taskkill /F /IM chromedriver.exe",
                                  "Taskkill /F /IM chrome.exe",
                                  "Taskkill /F /IM firefox.exe",
                                  "exit"
                                 };
            string tcmExecuteBat = curDir + tmp + "tcmExecute.bat";
            LogBatFileOutput(cntrlines, tcmExecuteBat);
            System.IO.File.WriteAllLines(tcmExecuteBat, cntrlines);
            string tcmExecuteOut = CmdLine.GetCmdOut(tcmExecuteBat,true);
            LogMessageToFile("The Command Output is : "+tcmExecuteOut);
            Console.WriteLine("Done");
            Console.WriteLine("The Output of the execution is : " + tcmExecuteOut);
            return tcmExecuteOut;
        }

        /// <summary>
        /// TCM Function to export the trx file from the test run
        /// </summary>
        /// <returns></returns>
        public static string TCMExport()
        {
            LogMessageToFile("TCM Export Function started");
            Console.WriteLine("TCM Export Function started...");
            resultFile = curDir + tmp + @"\latest" + DateTime.Now.ToFileTime().ToString().Substring(0, 6) + ".trx";
            string[] cntrlines = {"cd "+tcmPath, 
                                  "tcm run /export /id:"+ sp.runId+" /resultsfile:"+"\""+resultFile+"\""
                                  +" /collection:"+ConfigFile.col+" /teamproject:"+"\""+ConfigFile.proj+"\""
                                 };
            string tcmExportBat = curDir + tmp + "tcmExport.bat";
            LogBatFileOutput(cntrlines, tcmExportBat);
            System.IO.File.WriteAllLines(tcmExportBat, cntrlines);
            string tcmExport = CmdLine.GetCmdOut(tcmExportBat,true);
            LogMessageToFile("The Command Output is : "+tcmExport);
            Console.WriteLine("Done");
            if (tcmExport.Contains("Completed"))
            {
                Console.WriteLine("Initiated TRX File parsing");
                TRXFileParser.TRXParser();
                firstTry = true;
                Console.WriteLine("The value of the first try of parsing is : "+firstTry.ToString());
            }
            return tcmExport;
        }   
    }
}
