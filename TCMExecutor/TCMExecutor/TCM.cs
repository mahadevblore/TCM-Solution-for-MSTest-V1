using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMExecutor
{
    public class TCMFunctions :Logger
    {
        public static string ExtractQueryForSuiteIDs(string sId,string cId,string pId, string queryText,bool type =false)
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
                        sIdQuery += sIds[i] + " or SuiteID =";
                    }
                }
                if (!type)
                {
                    queryText = "\"" + "SELECT * FROM TestPoint WHERE SuiteId=" + sIdQuery + " and ConfigurationId="
                                     + cId + " and PlanId =" + pId + "\"";
                }
                else
                {
                    queryText = "SELECT * FROM TestPoint WHERE SuiteId=" + sIdQuery + " and ConfigurationId="
                                        + cId + " and PlanId =" + pId;
                }
            }
            else
            {
                if (!type)
                {
                    queryText = "\"" + "SELECT * FROM TestPoint WHERE SuiteId=" + sId + " and ConfigurationId="
                        + cId + " and PlanId =" + pId + "\"";
                }
                else
                {
                    queryText = "SELECT * FROM TestPoint WHERE SuiteId=" + sId + " and ConfigurationId="
                            + cId + " and PlanId =" + pId;
                }
            }
            return queryText;
        }

        public static string ExtractQueryForSuiteIDsAndTestIDs(string sId, string tId, string cId, string pId, string queryText, bool type = false)
         {
             string tIdQuery = null;
             tIdQuery = ExtractTIDQuery(tId, tIdQuery);

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
                     queryText = "\"" + "SELECT * FROM TestPoint WHERE (SuiteId=" + sIdQuery + ") and (TestCaseId ="+tIdQuery+") and (ConfigurationId="
                                      + cId + " and PlanId =" + pId + ")\"";
                 }
                 else
                 {
                     queryText = "SELECT * FROM TestPoint WHERE (SuiteId=" + sIdQuery + ") and (TestCaseId =" + tIdQuery + ") and (ConfigurationId="
                                         + cId + " and PlanId =" + pId+")";
                 }
             }
             else
             {
                 if (!type)
                 {
                     queryText = "\"" + "SELECT * FROM TestPoint WHERE (SuiteId=" + sId + ") and (TestCaseId =" + tIdQuery + ") and (ConfigurationId="
                         + cId + " and PlanId =" + pId + ")\"";
                 }
                 else
                 {
                     queryText = "SELECT * FROM TestPoint WHERE (SuiteId=" + sId + ") and (TestCaseId =" + tIdQuery + ") and (ConfigurationId="
                             + cId + " and PlanId =" + pId+")";
                 }
             }
             return queryText;
         }

        private static string ExtractTIDQuery(string tId, string tIdQuery)
         {
             if (tId.Contains(','))
             {
                 string[] tIds = tId.Split(',');
                 for (int i = 0; i <= tIds.Count() - 1; i++)
                 {
                     if (i == tIds.Count() - 1)
                     {
                         if (string.IsNullOrWhiteSpace(tIds[i]))
                         {
                             tIdQuery.Substring(tIdQuery.Length - 14, 0);
                         }
                         else
                         {
                             tIdQuery += tIds[i];
                         }
                     }
                     else
                     {
                         tIdQuery += tIds[i] + " or TestCaseId =";
                     }
                 }
             }
             else
             {
                 tIdQuery = tId;
             }
             return tIdQuery;
         }

        public static string TCMSuiteExecutionCreateAndExecute(TCMParameters tcmParam)
        {
            string sIdQuery = null; sIdQuery = ExtractQueryForSuiteIDs(tcmParam.suiteId, tcmParam.configId, tcmParam.planId, sIdQuery);
            string buildPath = tcmParam.buildPath;
            string col = tcmParam.collection;
            string proj = tcmParam.projectName;
            string runId = null;
            string tcmSuiteExecBat = tempPath + Program.tmp + "tcmSuiteExecCreate_" + DateTime.Now.ToString("MM_dd_yyyy_hh_mm_ss_tt")+".bat";
            if (tcmParam.settingsName != null)
            {
                string[] lines = {   "cd /",
                                 "cd "+Program.tcmPath,                                  
                                 "tcm run /create /title:"+"\""+tcmParam.runTitle+"\""+" /planid:"
                                 +tcmParam.planId+" /querytext:"+sIdQuery+" /builddir:"+"\""+buildPath+"\""+" /collection:\""+col+"\" /teamproject:"
                                 +"\""+proj+"\""+" /settingsname:"+"\""+tcmParam.settingsName+"\""+" /include"};
                System.IO.File.WriteAllLines(tcmSuiteExecBat, lines);
                LogBatFileOutput(lines, tcmSuiteExecBat);
            }
            else
            {
                string[] lines = {   "cd /",
                                 "cd "+Program.tcmPath,                                  
                                 "tcm run /create /title:"+"\""+tcmParam.runTitle+"\""+" /planid:"
                                 +tcmParam.planId+" /querytext:"+sIdQuery +" /builddir:"+"\""+buildPath+"\""+" /collection:\""+col+"\" /teamproject:"
                                 +"\""+proj+"\" /include"};
                System.IO.File.WriteAllLines(tcmSuiteExecBat, lines);
                LogBatFileOutput(lines, tcmSuiteExecBat);
            }

            string strTmp = Utils.GetCmdOut(tcmSuiteExecBat);
            File.Delete(tcmSuiteExecBat);
            if (strTmp.Contains("created with ID: "))
            {
                LogMessageToFile("Run Id for SuiteExecution Function was created successfully using TCM create command line");
                runId = strTmp.Split(new string[] { "created with ID: " }, StringSplitOptions.None)[1].Split('.')[0].Trim();
                Console.WriteLine("Run ID generated is : " +runId);
                LogMessageToFile("Run ID generated is : " + runId);
            }
            return strTmp+"\r\n"+"\r\n"+TCMExecute(runId, col, proj);
        }

        public static string TCMTestExecutionExecutionCreateAndExecute(TCMParameters tcmParam)
        {
            string tIdQuery = null; tIdQuery = ExtractQueryForSuiteIDsAndTestIDs(tcmParam.suiteId, tcmParam.testCaseId, tcmParam.configId,tcmParam.planId, tIdQuery);
            string buildPath = tcmParam.buildPath;
            string col = tcmParam.collection;
            string proj = tcmParam.projectName;
            string runId = null;
            string tcmTestExecBat = tempPath + Program.tmp + "tcmTestExecCreate_"+ DateTime.Now.ToString("MM_dd_yyyy_hh_mm_ss_tt")+".bat";
            if (tcmParam.settingsName != null)
            {
                string[] lines = {   "cd /",
                                 "cd "+Program.tcmPath,                                  
                                 "tcm run /create /title:"+"\""+tcmParam.runTitle+"\""+" /planid:"
                                 +tcmParam.planId+" /querytext:"+tIdQuery+" /builddir:"+"\""+buildPath+"\""+" /collection:\""+col+"\" /teamproject:"
                                 +"\""+proj+"\""+" /settingsname:"+"\""+tcmParam.settingsName+"\""+" /include"};
                System.IO.File.WriteAllLines(tcmTestExecBat, lines);
                LogBatFileOutput(lines, tcmTestExecBat);
            }
            else
            {
                string[] lines = {   "cd /",
                                 "cd "+Program.tcmPath,                                  
                                 "tcm run /create /title:"+"\""+tcmParam.runTitle+"\""+" /planid:"
                                 +tcmParam.planId+" /querytext:"+tIdQuery +" /builddir:"+"\""+buildPath+"\""+" /collection:\""+col+"\" /teamproject:"
                                 +"\""+proj+"\" /include"};
                System.IO.File.WriteAllLines(tcmTestExecBat, lines);
                LogBatFileOutput(lines, tcmTestExecBat);
            }

            string strTmp = Utils.GetCmdOut(tcmTestExecBat);
            File.Delete(tcmTestExecBat);
            if (strTmp.Contains("created with ID: "))
            {
                LogMessageToFile("Run Id for TestCasaeExecution Function was created successfully using TCM create command line");
                runId = strTmp.Split(new string[] { "created with ID: " }, StringSplitOptions.None)[1].Split('.')[0].Trim();
                Console.WriteLine("Run ID generated is : " + runId);
                LogMessageToFile("Run ID generated is : " + runId);
            }
            TCMExecute(runId, col, proj);
            return runId;
        }

        /// <summary>
        /// TCM Command line to execute the test run using TCM Create Command
        /// </summary>
        /// <returns>Output of the command line</returns>
        public static string TCMExecute(string rId,string col,string proj)
        {
            LogMessageToFile("TCM Execute Function started");
            Console.WriteLine("TCM Execute Function started...");

            string[] cntrlines = {"cd "+Program.tcmPath,    
                                  "call:While",
                                  ":While",
                                  "tcm run /execute /id:"+rId+" /collection:\""+col+"\" /teamproject:"+"\""+proj+"\"",
                                  "if %ERRORLEVEL% GTR 0 goto While",
                                  "exit"
                                 };
            string tcmExecutorBat = tempPath + Program.tmp + "tcmExecutor" + DateTime.Now.ToString("MM_dd_yyyy_hh_mm_ss_tt") + ".bat";
            LogBatFileOutput(cntrlines, tcmExecutorBat);
            System.IO.File.WriteAllLines(tcmExecutorBat, cntrlines);
            string tcmExecuteOut = Utils.GetCmdOut(tcmExecutorBat);
            LogMessageToFile("The Command Output is : " + tcmExecuteOut);
            Console.WriteLine("Done");
            Console.WriteLine("The Output of the execution is : "+tcmExecuteOut);
            File.Delete(tcmExecutorBat);
            return tcmExecuteOut;
        }

        /// <summary>
        /// TCM Function to export the trx file from the test run
        /// </summary>
        /// <returns></returns>
        public static string TCMExport(string runId,string col,string proj)
        {
            LogMessageToFile("TCM Export Function started");
            Console.WriteLine("TCM Export Function started...");
            string resultFile = tempPath + Program.tmp + "TRX_"+runId+ ".trx";
            string[] cntrlines = {"cd "+Program.tcmPath, 
                                  "tcm run /export /id:"+ runId+" /resultsfile:"+"\""+resultFile+"\""
                                  +" /collection:"+col+" /teamproject:"+"\""+proj+"\""
                                 };
            string tcmExportBat = tempPath + Program.tmp + "tcmExport.bat";
            LogBatFileOutput(cntrlines, tcmExportBat);
            System.IO.File.WriteAllLines(tcmExportBat, cntrlines);
            string tcmExport = Utils.GetCmdOut(tcmExportBat);
            LogMessageToFile("The Command Output is : "+tcmExport);
            Console.WriteLine("Done");
            return tcmExport;
        }   
    }
}
