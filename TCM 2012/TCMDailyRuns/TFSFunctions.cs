using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.TestManagement.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TCMDailyRuns
{
    public class TFSFunctions : Program
    {
        public static int totalPoints;
        public static int errorCounter = 0;
        
        public static string GetBuildPath(string goodBuildPath,string definition,string buildPath)
        {
            try
            {
                goodBuildPath = null;
                NetworkCredential cred = new NetworkCredential("SERVICE_ACCOUNT_NAME", "SERVICE_ACCOUNT_PASSWORD", "DOMAIN");
                TfsTeamProjectCollection tfs = new TfsTeamProjectCollection(TfsTeamProjectCollection.GetFullyQualifiedUriForName(ConfigFile.col), cred);
                ITestManagementService tms = tfs.GetService<ITestManagementService>();
                ITestManagementTeamProject proj = tms.GetTeamProject(ConfigFile.proj);
                IBuildServer tfsBuildServer = tfs.GetService<IBuildServer>();
                // Reading from XML
                IBuildDefinition buildDef = tfsBuildServer.GetBuildDefinition(ConfigFile.proj, definition);
                IBuildDetail[] bDetails = buildDef.QueryBuilds();
                IBuildDetail bDetail = bDetails.OrderBy(x => x.CompilationStatus == BuildPhaseStatus.Succeeded).Last();
                goodBuildPath = bDetail.DropLocation;
            }
            catch (Exception ex)
            {

            }
            if (Directory.Exists(goodBuildPath))
            {
                DirectoryInfo dInfor = new DirectoryInfo(goodBuildPath);
                goodBuildPath= sourceFilePath = dInfor.FullName;
                recent = dInfor.Name;
                LogMessageToFile("The Good Build Path exists at : " + goodBuildPath);
            }
            else
            {
                DirectoryInfo dInforTemp = new DirectoryInfo(buildPath).GetDirectories()
                        .OrderByDescending(d => d.LastWriteTimeUtc).First();
                goodBuildPath = sourceFilePath = dInforTemp.FullName;
                recent = dInforTemp.Name;
                LogMessageToFile("Good build path was found using most recent folder method and directory path is : " + goodBuildPath);
            }
            return goodBuildPath;
        }

        public static void CollectResultFromTFS(string runId)
        {
            try
            {
                NetworkCredential cred = new NetworkCredential("SERVICE_ACCOUNT_NAME", "SERVICE_ACCOUNT_PASSWORD", "DOMAIN");
                TfsTeamProjectCollection tfs = new TfsTeamProjectCollection(TfsTeamProjectCollection.GetFullyQualifiedUriForName(ConfigFile.col), cred);
                ITestManagementService tms = tfs.GetService<ITestManagementService>();
                ITestManagementTeamProject proj = tms.GetTeamProject(ConfigFile.proj);
                ITestRun trun = proj.TestRuns.Find(Convert.ToInt32(runId));

                while (trun.State == TestRunState.InProgress || trun.State == TestRunState.NotStarted || trun.State == TestRunState.Waiting)
                {
                    LogMessageToFile("The Test Run is still in Progress or Waiting. The Run Id generated is : " + trun.Id);
                    trun = proj.TestRuns.Find(Convert.ToInt32(runId));
                    System.Threading.Thread.Sleep(15000);
                }
                sp.total += trun.QueryResults().Count();
                sp.failed += trun.QueryResultsByOutcome(TestOutcome.Failed).Count();
                sp.passed += trun.QueryResultsByOutcome(TestOutcome.Passed).Count();
                sp.Inconclusive += trun.QueryResultsByOutcome(TestOutcome.Inconclusive).Count();
                sp.aborted += trun.QueryResultsByOutcome(TestOutcome.Aborted).Count();
                sp.error += trun.QueryResultsByOutcome(TestOutcome.Error).Count();
                sp.timeout += trun.QueryResultsByOutcome(TestOutcome.Timeout).Count();
                sp.notExecuted += trun.QueryResultsByOutcome(TestOutcome.NotExecuted).Count();
                sp.other += sp.total - sp.failed - sp.passed - sp.Inconclusive - sp.aborted - sp.error - sp.timeout;

                ITestCaseResultHelper iTCRHelper = proj.TestResults;

                sp.p1Total += iTCRHelper.Query(String.Format("Select * from TestResult where TestRunId='{0}' and Priority=1", Convert.ToInt32(runId))).Count();
                sp.p1Failed += iTCRHelper.Query(String.Format("Select * from TestResult where TestRunId='{0}' and Priority=1 and Outcome='{1}'", Convert.ToInt32(runId), TestOutcome.Failed)).Count();
                sp.p1Passed += iTCRHelper.Query(String.Format("Select * from TestResult where TestRunId='{0}' and Priority=1 and Outcome='{1}'", Convert.ToInt32(runId), TestOutcome.Passed)).Count();
                sp.p1Inconclusive += iTCRHelper.Query(String.Format("Select * from TestResult where TestRunId='{0}' and Priority=1 and Outcome='{1}'", Convert.ToInt32(runId), TestOutcome.Inconclusive)).Count();
                sp.p1Aborted += iTCRHelper.Query(String.Format("Select * from TestResult where TestRunId='{0}' and Priority=1 and Outcome='{1}'", Convert.ToInt32(runId), TestOutcome.Aborted)).Count();
                sp.p1Error += iTCRHelper.Query(String.Format("Select * from TestResult where TestRunId='{0}' and Priority=1 and Outcome='{1}'", Convert.ToInt32(runId), TestOutcome.Error)).Count();
                sp.p1Timeout += iTCRHelper.Query(String.Format("Select * from TestResult where TestRunId='{0}' and Priority=1 and Outcome='{1}'", Convert.ToInt32(runId), TestOutcome.Timeout)).Count();
                sp.p1Other += sp.p1Total - sp.p1Failed - sp.p1Passed - sp.p1Inconclusive - sp.p1Aborted - sp.p1Error - sp.p1Timeout;

                LogMessageToFile(runId + " : " + "P1 Total Test Cases : " + sp.p1Total);
                LogMessageToFile(runId + " : " + "P1 Failed Test Cases : " + sp.p1Failed);
                LogMessageToFile(runId + " : " + "P1 Passed Test Cases : " + sp.p1Passed);
                LogMessageToFile(runId + " : " + "P1 Inconclusive Test Cases : " + sp.p1Inconclusive);
                LogMessageToFile(runId + " : " + "P1 aborted Test Cases : " + sp.p1Aborted);
                LogMessageToFile(runId + " : " + "P1 error Test Cases : " + sp.p1Error);
                LogMessageToFile(runId + " : " + "P1 timeout Test Cases : " + sp.p1Timeout);
                LogMessageToFile(runId + " : " + "P1 other Test Cases : " + sp.p1Other);

                sp.p2Total += iTCRHelper.Query(String.Format("Select * from TestResult where TestRunId='{0}' and Priority=2", Convert.ToInt32(runId))).Count();
                sp.p2Failed += iTCRHelper.Query(String.Format("Select * from TestResult where TestRunId='{0}' and Priority=2 and Outcome='{1}'", Convert.ToInt32(runId), TestOutcome.Failed)).Count();
                sp.p2Passed += iTCRHelper.Query(String.Format("Select * from TestResult where TestRunId='{0}' and Priority=2 and Outcome='{1}'", Convert.ToInt32(runId), TestOutcome.Passed)).Count();
                sp.p2Inconclusive += iTCRHelper.Query(String.Format("Select * from TestResult where TestRunId='{0}' and Priority=2 and Outcome='{1}'", Convert.ToInt32(runId), TestOutcome.Inconclusive)).Count();
                sp.p2Aborted += iTCRHelper.Query(String.Format("Select * from TestResult where TestRunId='{0}' and Priority=2 and Outcome='{1}'", Convert.ToInt32(runId), TestOutcome.Aborted)).Count();
                sp.p2Error += iTCRHelper.Query(String.Format("Select * from TestResult where TestRunId='{0}' and Priority=2 and Outcome='{1}'", Convert.ToInt32(runId), TestOutcome.Error)).Count();
                sp.p2Timeout += iTCRHelper.Query(String.Format("Select * from TestResult where TestRunId='{0}' and Priority=2 and Outcome='{1}'", Convert.ToInt32(runId), TestOutcome.Timeout)).Count();
                sp.p2Other += sp.p2Total - sp.p2Failed - sp.p2Passed - sp.p2Inconclusive - sp.p2Aborted - sp.p2Error - sp.p2Timeout;

                LogMessageToFile(runId + " : " + "P2 Total Test Cases : " + sp.p2Total);
                LogMessageToFile(runId + " : " + "P2 Failed Test Cases : " + sp.p2Failed);
                LogMessageToFile(runId + " : " + "P2 Passed Test Cases : " + sp.p2Passed);
                LogMessageToFile(runId + " : " + "P2 Inconclusive Test Cases : " + sp.p2Inconclusive);
                LogMessageToFile(runId + " : " + "P2 aborted Test Cases : " + sp.p2Aborted);
                LogMessageToFile(runId + " : " + "P2 error Test Cases : " + sp.p2Error);
                LogMessageToFile(runId + " : " + "P2 timeout Test Cases : " + sp.p2Timeout);
                LogMessageToFile(runId + " : " + "P2 other Test Cases : " + sp.p2Other);

                sp.p3Total += iTCRHelper.Query(String.Format("Select * from TestResult where TestRunId='{0}' and Priority=3", Convert.ToInt32(runId))).Count();
                sp.p3Failed += iTCRHelper.Query(String.Format("Select * from TestResult where TestRunId='{0}' and Priority=3 and Outcome='{1}'", Convert.ToInt32(runId), TestOutcome.Failed)).Count();
                sp.p3Passed += iTCRHelper.Query(String.Format("Select * from TestResult where TestRunId='{0}' and Priority=3 and Outcome='{1}'", Convert.ToInt32(runId), TestOutcome.Passed)).Count();
                sp.p3Inconclusive += iTCRHelper.Query(String.Format("Select * from TestResult where TestRunId='{0}' and Priority=3 and Outcome='{1}'", Convert.ToInt32(runId), TestOutcome.Inconclusive)).Count();
                sp.p3Aborted += iTCRHelper.Query(String.Format("Select * from TestResult where TestRunId='{0}' and Priority=3 and Outcome='{1}'", Convert.ToInt32(runId), TestOutcome.Aborted)).Count();
                sp.p3Error += iTCRHelper.Query(String.Format("Select * from TestResult where TestRunId='{0}' and Priority=3 and Outcome='{1}'", Convert.ToInt32(runId), TestOutcome.Error)).Count();
                sp.p3Timeout += iTCRHelper.Query(String.Format("Select * from TestResult where TestRunId='{0}' and Priority=3 and Outcome='{1}'", Convert.ToInt32(runId), TestOutcome.Timeout)).Count();
                sp.p3Other += sp.p3Total - sp.p3Failed - sp.p3Passed - sp.p3Inconclusive - sp.p3Aborted - sp.p3Error - sp.p3Timeout;

                LogMessageToFile(runId + " : " + "P3 Total Test Cases : " + sp.p3Total);
                LogMessageToFile(runId + " : " + "P3 Failed Test Cases : " + sp.p3Failed);
                LogMessageToFile(runId + " : " + "P3 Passed Test Cases : " + sp.p3Passed);
                LogMessageToFile(runId + " : " + "P3 Inconclusive Test Cases : " + sp.p3Inconclusive);
                LogMessageToFile(runId + " : " + "P3 aborted Test Cases : " + sp.p3Aborted);
                LogMessageToFile(runId + " : " + "P3 error Test Cases : " + sp.p3Error);
                LogMessageToFile(runId + " : " + "P3 timeout Test Cases : " + sp.p3Timeout);
                LogMessageToFile(runId + " : " + "P3 other Test Cases : " + sp.p3Other);
            }
            catch (Exception ex)
            {
                LogMessageToFile("The exception caught at 'public static void CollectResultFromTFS(string runId)' function is :" + ex.ToString() + " : " + ex.StackTrace.ToString());
            }
        }

        public static int EvaluateResultFromTFS(string runId)
        {
            try
            {
                NetworkCredential cred = new NetworkCredential("SERVICE_ACCOUNT_NAME", "SERVICE_ACCOUNT_PASSWORD", "DOMAIN");
                TfsTeamProjectCollection tfs = new TfsTeamProjectCollection(TfsTeamProjectCollection.GetFullyQualifiedUriForName(ConfigFile.col), cred);
                ITestManagementService tms = tfs.GetService<ITestManagementService>();
                ITestManagementTeamProject proj = tms.GetTeamProject(ConfigFile.proj);
                ITestRun trun = proj.TestRuns.Find(Convert.ToInt32(runId));

                SelectedPlan spTemp = new SelectedPlan();

                spTemp.total += trun.QueryResults().Count();
                spTemp.failed += trun.QueryResultsByOutcome(TestOutcome.Failed).Count();
                spTemp.passed += trun.QueryResultsByOutcome(TestOutcome.Passed).Count();

                LogMessageToFile(runId + " : " + "Total Test Cases : " + spTemp.total);
                LogMessageToFile(runId + " : " + "Passed Test Cases : " + spTemp.passed);
                LogMessageToFile(runId + " : " + "Failed Test Cases : " + spTemp.failed);

                if (spTemp.passed == 0)
                {
                    LogMessageToFile("All Test Cases Failed.");
                    return 0;
                }
                else if (spTemp.total == spTemp.passed)
                {
                    LogMessageToFile("All Test Cases Passed.");
                    return 1;
                }
                else
                {
                    LogMessageToFile("Some Test Cases Failed.");
                    return 2;
                }
            }
            catch (Exception ex)
            {
                LogMessageToFile("The exception caught at 'public static void EvaluateResultFromTFS(string runId)' function is :" + ex.ToString() + " : " + ex.StackTrace.ToString());
                return 3;
            }
        }

        public static void ResetCount(string runId)
        {
            bool resetFlag = false;
            LogMessageToFile("Trying to Get Reset Status for the run id " + runId);
            NetworkCredential cred = new NetworkCredential("SERVICE_ACCOUNT_NAME", "SERVICE_ACCOUNT_PASSWORD", "DOMAIN");
            TfsTeamProjectCollection tfs = new TfsTeamProjectCollection(TfsTeamProjectCollection.GetFullyQualifiedUriForName(ConfigFile.col), cred);
            ITestManagementService tms = tfs.GetService<ITestManagementService>();
            ITestManagementTeamProject proj = tms.GetTeamProject(ConfigFile.proj);
            ITestRun trun = proj.TestRuns.Find(Convert.ToInt32(runId));
            LogMessageToFile("TestRun exists for the given run id : "+ runId);
            ITestCaseResultHelper iTCRHelper = proj.TestResults;

           
            if (iTCRHelper.Query(string.Format("Select * from TestResult where TestRunId='{0}' and ResetCount>0",Convert.ToInt32(runId))).Count() > 0) //Where(x => x.ResetCount > 0).
            {
                resetFlag = true;
            }
            
        }

        public static void ResetFailureTestCasesAndRun(string runId)
        {
            try
            {
                LogMessageToFile("Trying to Execute Failed test cases for the run id "+runId);
                NetworkCredential cred = new NetworkCredential("SERVICE_ACCOUNT_NAME", "SERVICE_ACCOUNT_PASSWORD", "DOMAIN");
                TfsTeamProjectCollection tfs = new TfsTeamProjectCollection(TfsTeamProjectCollection.GetFullyQualifiedUriForName(ConfigFile.col), cred);
                ITestManagementService tms = tfs.GetService<ITestManagementService>();
                ITestManagementTeamProject proj = tms.GetTeamProject(ConfigFile.proj);
                ITestRun trun = proj.TestRuns.Find(Convert.ToInt32(runId));
                LogMessageToFile("TestRun exists for the given run id : "+ runId);
                ITestCaseResultHelper iTCRHelper = proj.TestResults;

                int failureCount = 0;
                string query = null;
                query = "Select * from TestResult where TestRunId='{0}' and Outcome<>'{1}'";

                failureCount = iTCRHelper.Query(String.Format(query, Convert.ToInt32(runId), TestOutcome.Passed)).Count();
                
                int errorOrNotExecutedCount = iTCRHelper.Query(String.Format("Select * from TestResult where TestRunId='{0}' and Outcome='{1}' and Outcome='{2}'", Convert.ToInt32(runId),
                    TestOutcome.Error,TestOutcome.NotExecuted)).Count();

                LogMessageToFile("The failure test cases count is : "+ failureCount.ToString() + " and the error count is : "+errorOrNotExecutedCount.ToString());

                if (failureCount > 0)
                {
                    LogMessageToFile("Trying to reset all the failure outcome test cases");
                    foreach (var result in iTCRHelper.Query(string.Format(query, Convert.ToInt32(runId), TestOutcome.Passed)))
                    {
                        result.Reset();
                    }
                    LogMessageToFile("Test Cases reset was successful.");
                }
                LogMessageToFile("Trying to execute the run id : "+ runId+" after reset.");

                TCMFunctions.TCMExecute(runId);
                LogMessageToFile("Execution after test run reset was successful.");
            }
            catch (Exception ex)
            {
                LogMessageToFile("The exception caught at 'public static void ResetFailureTestCases(string runId)' function is :" + ex.ToString() + " : " + ex.StackTrace.ToString());
            }
        }

        public static void CollectResultFromTFS(int orderId,bool splitRun = false)
        {
            try
            {
                NetworkCredential cred = new NetworkCredential("SERVICE_ACCOUNT_NAME", "SERVICE_ACCOUNT_PASSWORD", "DOMAIN");
                TfsTeamProjectCollection tfs = new TfsTeamProjectCollection(TfsTeamProjectCollection.GetFullyQualifiedUriForName(ConfigFile.col), cred);
                ITestManagementService tms = tfs.GetService<ITestManagementService>();
                ITestManagementTeamProject proj = tms.GetTeamProject(ConfigFile.proj);
                ITestRunHelper tRuns = proj.TestRuns;

                string runTitle = null;

                if (splitRun)
                {
                    runTitle = "SR_" + Convert.ToString(orderId);
                }
                else
                {
                    runTitle = "DR_"+ Convert.ToString(orderId);
                }
                string tRunQString = null;
                if (ConfigFile.RuntitleExecutor != null && ConfigFile.RuntitleExecutor != "")
                {
                    runTitle = ConfigFile.RuntitleExecutor.Split(':')[orderId - 1] + "_" + runTitle;
                    tRunQString = "Select * from TestRun where Title Contains '" + runTitle + "' and CreationDate >= '" + sTime + "'";
                }
                else
                {
                    tRunQString = "Select * from TestRun where Title Contains '" + runTitle + "' and CreationDate >= '" + sTime + "'";
                }           
                IEnumerable<ITestRun> tRunQuery = tRuns.Query(tRunQString);
                LogMessageToFile("The query string passed to the Test Runs query is : " + tRunQString);
                ITestRun tRun = tRunQuery.OrderBy(x => x.DateCreated).LastOrDefault();
                string trunId = tRun.Id.ToString();
                Console.WriteLine("The Run id queried is : " + tRun.Id);
                LogMessageToFile("The Title of the Run-" + tRun.Title + " : Date Created " + tRun.DateCreated + " : Date Completed " + tRun.DateCompleted);

                while (tRun.State == TestRunState.InProgress || tRun.State == TestRunState.NotStarted || tRun.State == TestRunState.Waiting)
                {
                    tRuns = proj.TestRuns;
                    tRunQuery = tRuns.Query(tRunQString);
                    tRun = tRunQuery.OrderBy(x => x.DateCreated).LastOrDefault();

                    LogMessageToFile("The State of the Test Runs is : " + tRun.State);
                    Console.WriteLine("The State of the Test Runs is : " + tRun.State);
                    System.Threading.Thread.Sleep(10000);
                }

				if (!sp.runId.Contains(tRun.Id.ToString()))
                {
                    sp.runId += "," + tRun.Id;
                }
                //sp.runId += sp.runId + "," + tRun.Id;
                sp.total += tRun.QueryResults().Count();
                sp.failed += tRun.QueryResultsByOutcome(TestOutcome.Failed).Count();
                sp.passed += tRun.QueryResultsByOutcome(TestOutcome.Passed).Count();
                sp.Inconclusive += tRun.QueryResultsByOutcome(TestOutcome.Inconclusive).Count();
                sp.aborted += tRun.QueryResultsByOutcome(TestOutcome.Aborted).Count();
                sp.error += tRun.QueryResultsByOutcome(TestOutcome.Error).Count();
                sp.timeout += tRun.QueryResultsByOutcome(TestOutcome.Timeout).Count();
                sp.other += sp.total - sp.failed - sp.passed - sp.Inconclusive - sp.aborted - sp.error - sp.timeout;

                LogMessageToFile(trunId + " : " + "Total Test Cases : " + sp.total);
                LogMessageToFile(trunId + " : " + "Failed Test Cases : " + sp.failed);
                LogMessageToFile(trunId + " : " + "Passed Test Cases : " + sp.passed);
                LogMessageToFile(trunId + " : " + "Inconclusive Test Cases : " + sp.Inconclusive);
                LogMessageToFile(trunId + " : " + "aborted Test Cases : " + sp.aborted);
                LogMessageToFile(trunId + " : " + "error Test Cases : " + sp.error);
                LogMessageToFile(trunId + " : " + "timeout Test Cases : " + sp.timeout);
                LogMessageToFile(trunId + " : " + "other Test Cases : " + sp.other);

                ITestCaseResultHelper iTCRHelper = proj.TestResults;

                sp.p1Total += iTCRHelper.Query(String.Format("Select * from TestResult where TestrunId='{0}' and Priority=1", Convert.ToInt32(trunId))).Count();
                sp.p1Failed += iTCRHelper.Query(String.Format("Select * from TestResult where TestrunId='{0}' and Priority=1 and Outcome='{1}'", Convert.ToInt32(trunId), TestOutcome.Failed)).Count();
                sp.p1Passed += iTCRHelper.Query(String.Format("Select * from TestResult where TestrunId='{0}' and Priority=1 and Outcome='{1}'", Convert.ToInt32(trunId), TestOutcome.Passed)).Count();
                sp.p1Inconclusive += iTCRHelper.Query(String.Format("Select * from TestResult where TestrunId='{0}' and Priority=1 and Outcome='{1}'", Convert.ToInt32(trunId), TestOutcome.Inconclusive)).Count();
                sp.p1Aborted += iTCRHelper.Query(String.Format("Select * from TestResult where TestrunId='{0}' and Priority=1 and Outcome='{1}'", Convert.ToInt32(trunId), TestOutcome.Aborted)).Count();
                sp.p1Error += iTCRHelper.Query(String.Format("Select * from TestResult where TestrunId='{0}' and Priority=1 and Outcome='{1}'", Convert.ToInt32(trunId), TestOutcome.Error)).Count();
                sp.p1Timeout += iTCRHelper.Query(String.Format("Select * from TestResult where TestrunId='{0}' and Priority=1 and Outcome='{1}'", Convert.ToInt32(trunId), TestOutcome.Timeout)).Count();
                sp.p1Other += sp.p1Total - sp.p1Failed - sp.p1Passed - sp.p1Inconclusive - sp.p1Aborted - sp.p1Error - sp.p1Timeout;

                LogMessageToFile(trunId + " : " + "P1 Total Test Cases : " + sp.p1Total);
                LogMessageToFile(trunId + " : " + "P1 Failed Test Cases : " + sp.p1Failed);
                LogMessageToFile(trunId + " : " + "P1 Passed Test Cases : " + sp.p1Passed);
                LogMessageToFile(trunId + " : " + "P1 Inconclusive Test Cases : " + sp.p1Inconclusive);
                LogMessageToFile(trunId + " : " + "P1 aborted Test Cases : " + sp.p1Aborted);
                LogMessageToFile(trunId + " : " + "P1 error Test Cases : " + sp.p1Error);
                LogMessageToFile(trunId + " : " + "P1 timeout Test Cases : " + sp.p1Timeout);
                LogMessageToFile(trunId + " : " + "P1 other Test Cases : " + sp.p1Other);

                sp.p2Total += iTCRHelper.Query(String.Format("Select * from TestResult where TestrunId='{0}' and Priority=2", Convert.ToInt32(trunId))).Count();
                sp.p2Failed += iTCRHelper.Query(String.Format("Select * from TestResult where TestrunId='{0}' and Priority=2 and Outcome='{1}'", Convert.ToInt32(trunId), TestOutcome.Failed)).Count();
                sp.p2Passed += iTCRHelper.Query(String.Format("Select * from TestResult where TestrunId='{0}' and Priority=2 and Outcome='{1}'", Convert.ToInt32(trunId), TestOutcome.Passed)).Count();
                sp.p2Inconclusive += iTCRHelper.Query(String.Format("Select * from TestResult where TestrunId='{0}' and Priority=2 and Outcome='{1}'", Convert.ToInt32(trunId), TestOutcome.Inconclusive)).Count();
                sp.p2Aborted += iTCRHelper.Query(String.Format("Select * from TestResult where TestrunId='{0}' and Priority=2 and Outcome='{1}'", Convert.ToInt32(trunId), TestOutcome.Aborted)).Count();
                sp.p2Error += iTCRHelper.Query(String.Format("Select * from TestResult where TestrunId='{0}' and Priority=2 and Outcome='{1}'", Convert.ToInt32(trunId), TestOutcome.Error)).Count();
                sp.p2Timeout += iTCRHelper.Query(String.Format("Select * from TestResult where TestrunId='{0}' and Priority=2 and Outcome='{1}'", Convert.ToInt32(trunId), TestOutcome.Timeout)).Count();
                sp.p2Other += sp.p2Total - sp.p2Failed - sp.p2Passed - sp.p2Inconclusive - sp.p2Aborted - sp.p2Error - sp.p2Timeout;

                LogMessageToFile(trunId + " : " + "P2 Total Test Cases : " + sp.p2Total);
                LogMessageToFile(trunId + " : " + "P2 Failed Test Cases : " + sp.p2Failed);
                LogMessageToFile(trunId + " : " + "P2 Passed Test Cases : " + sp.p2Passed);
                LogMessageToFile(trunId + " : " + "P2 Inconclusive Test Cases : " + sp.p2Inconclusive);
                LogMessageToFile(trunId + " : " + "P2 aborted Test Cases : " + sp.p2Aborted);
                LogMessageToFile(trunId + " : " + "P2 error Test Cases : " + sp.p2Error);
                LogMessageToFile(trunId + " : " + "P2 timeout Test Cases : " + sp.p2Timeout);
                LogMessageToFile(trunId + " : " + "P2 other Test Cases : " + sp.p2Other);

                sp.p3Total += iTCRHelper.Query(String.Format("Select * from TestResult where TestrunId='{0}' and Priority=3", Convert.ToInt32(trunId))).Count();
                sp.p3Failed += iTCRHelper.Query(String.Format("Select * from TestResult where TestrunId='{0}' and Priority=3 and Outcome='{1}'", Convert.ToInt32(trunId), TestOutcome.Failed)).Count();
                sp.p3Passed += iTCRHelper.Query(String.Format("Select * from TestResult where TestrunId='{0}' and Priority=3 and Outcome='{1}'", Convert.ToInt32(trunId), TestOutcome.Passed)).Count();
                sp.p3Inconclusive += iTCRHelper.Query(String.Format("Select * from TestResult where TestrunId='{0}' and Priority=3 and Outcome='{1}'", Convert.ToInt32(trunId), TestOutcome.Inconclusive)).Count();
                sp.p3Aborted += iTCRHelper.Query(String.Format("Select * from TestResult where TestrunId='{0}' and Priority=3 and Outcome='{1}'", Convert.ToInt32(trunId), TestOutcome.Aborted)).Count();
                sp.p3Error += iTCRHelper.Query(String.Format("Select * from TestResult where TestrunId='{0}' and Priority=3 and Outcome='{1}'", Convert.ToInt32(trunId), TestOutcome.Error)).Count();
                sp.p3Timeout += iTCRHelper.Query(String.Format("Select * from TestResult where TestrunId='{0}' and Priority=3 and Outcome='{1}'", Convert.ToInt32(trunId), TestOutcome.Timeout)).Count();
                sp.p3Other += sp.p3Total - sp.p3Failed - sp.p3Passed - sp.p3Inconclusive - sp.p3Aborted - sp.p3Error - sp.p3Timeout;

                LogMessageToFile(trunId + " : " + "P3 Total Test Cases : " + sp.p3Total);
                LogMessageToFile(trunId + " : " + "P3 Failed Test Cases : " + sp.p3Failed);
                LogMessageToFile(trunId + " : " + "P3 Passed Test Cases : " + sp.p3Passed);
                LogMessageToFile(trunId + " : " + "P3 Inconclusive Test Cases : " + sp.p3Inconclusive);
                LogMessageToFile(trunId + " : " + "P3 aborted Test Cases : " + sp.p3Aborted);
                LogMessageToFile(trunId + " : " + "P3 error Test Cases : " + sp.p3Error);
                LogMessageToFile(trunId + " : " + "P3 timeout Test Cases : " + sp.p3Timeout);
                LogMessageToFile(trunId + " : " + "P3 other Test Cases : " + sp.p3Other);

            }
            catch (Exception ex)
            {
                LogMessageToFile("The exception caught at 'public static void CollectResultFromTFS(int orderId,bool splitRun = false)' function is :" + ex.ToString() + " : " + ex.StackTrace.ToString());
            }
        }

        public static void PublishResultsInMail(int order, bool splitRun = false)
        {
            NetworkCredential cred = new NetworkCredential("SERVICE_ACCOUNT_NAME", "SERVICE_ACCOUNT_PASSWORD", "DOMAIN");
            TfsTeamProjectCollection tfs = new TfsTeamProjectCollection(TfsTeamProjectCollection.GetFullyQualifiedUriForName(ConfigFile.col), cred);
            ITestManagementService tms = tfs.GetService<ITestManagementService>();
            ITestManagementTeamProject proj = tms.GetTeamProject(ConfigFile.proj);
            ITestRunHelper tRuns = proj.TestRuns;

            string runTitle = null;

            if (splitRun)
            {
                
                runTitle = "SR_" + Convert.ToString(order);
            }
            else
            {
                runTitle = "DR_" + Convert.ToString(order);
            }
            string tRunQString = null;
            if (ConfigFile.RuntitleExecutor != null && ConfigFile.RuntitleExecutor != "")
            {
                runTitle = ConfigFile.RuntitleExecutor.Split(':')[order - 1] + "_" + runTitle;
                tRunQString = "Select * from TestRun where Title Contains '" + runTitle + "' and CreationDate >= '" + sTime + "'";
            }
            else
            {
                tRunQString = "Select * from TestRun where Title Contains '" + runTitle + "' and CreationDate >= '" + sTime + "'";
            }
            IEnumerable<ITestRun> tRunQuery = tRuns.Query(tRunQString);
            LogMessageToFile("The query string passed to the Test Runs query is : " + tRunQString);
            ITestRun tRun = tRunQuery.OrderBy(x => x.DateCreated).LastOrDefault();
            string trunId = tRun.Id.ToString();

            int total = tRun.QueryResults().Count();
            int failed = tRun.QueryResultsByOutcome(TestOutcome.Failed).Count() + tRun.QueryResultsByOutcome(TestOutcome.Inconclusive).Count() + tRun.QueryResultsByOutcome(TestOutcome.Aborted).Count() + tRun.QueryResultsByOutcome(TestOutcome.Error).Count() + tRun.QueryResultsByOutcome(TestOutcome.Timeout).Count() + tRun.QueryResultsByOutcome(TestOutcome.NotExecuted).Count();
            int passed = tRun.QueryResultsByOutcome(TestOutcome.Passed).Count();
            double passPercentage = Math.Round((double)passed * 100 / (double)total, 2, MidpointRounding.AwayFromZero);
            EmailNotification.msgBody += "<tr><td>" + trunId + "</td><td>" + tRun.Title + "</td><td>" + total + "</td><td>" + passed + "</td><td>" + failed + "</td><td>" + passPercentage + "%</td><td><a href='" + ConfigFile.mtmLink + trunId + "'>MTM Link</a></td></tr>";
        }

        public static void PublishResultsInMail(string runId)
        {
            NetworkCredential cred = new NetworkCredential("SERVICE_ACCOUNT_NAME", "SERVICE_ACCOUNT_PASSWORD", "DOMAIN");
            TfsTeamProjectCollection tfs = new TfsTeamProjectCollection(TfsTeamProjectCollection.GetFullyQualifiedUriForName(ConfigFile.col), cred);
            ITestManagementService tms = tfs.GetService<ITestManagementService>();
            ITestManagementTeamProject proj = tms.GetTeamProject(ConfigFile.proj);
            ITestRun trun = proj.TestRuns.Find(Convert.ToInt32(runId));
            int total = trun.QueryResults().Count();
            int failed = trun.QueryResultsByOutcome(TestOutcome.Failed).Count() + trun.QueryResultsByOutcome(TestOutcome.Inconclusive).Count() + trun.QueryResultsByOutcome(TestOutcome.Aborted).Count() + trun.QueryResultsByOutcome(TestOutcome.Error).Count() + trun.QueryResultsByOutcome(TestOutcome.Timeout).Count() + trun.QueryResultsByOutcome(TestOutcome.NotExecuted).Count();
            int passed = trun.QueryResultsByOutcome(TestOutcome.Passed).Count();
            double passPercentage = Math.Round((double)passed * 100 / (double)total, 2, MidpointRounding.AwayFromZero);
            EmailNotification.msgBody += "<tr><td>" + runId + "</td><td>" + trun.Title + "</td><td>" + total + "</td><td>" + passed + "</td><td>" + failed + "</td><td>" + passPercentage + "%</td><td><a href='" + ConfigFile.mtmLink + runId + "'>MTM Link</a></td></tr>";
        }

        public static string GetTestRunState(string runTitle)
        {
            NetworkCredential cred = new NetworkCredential("SERVICE_ACCOUNT_NAME", "SERVICE_ACCOUNT_PASSWORD", "DOMAIN");
            TfsTeamProjectCollection tfs = new TfsTeamProjectCollection(TfsTeamProjectCollection.GetFullyQualifiedUriForName(ConfigFile.col), cred);
            ITestManagementService tms = tfs.GetService<ITestManagementService>();
            ITestManagementTeamProject proj = tms.GetTeamProject(ConfigFile.proj);
            ITestRunHelper tRuns = proj.TestRuns;

            string tRunQString = "Select * from TestRun where Title = '" + runTitle + "'";
            IEnumerable<ITestRun> tRunQuery = tRuns.Query(tRunQString);
            LogMessageToFile("The query string passed to the Test Runs query is : " + tRunQString);
            ITestRun tRun = tRunQuery.OrderBy(x => x.DateCreated).LastOrDefault();
            string trunId = tRun.Id.ToString();
            Console.WriteLine("The Run id queried is : " + tRun.Id);
            LogMessageToFile("The Title of the Run-" + tRun.Title + " : Date Created " + tRun.DateCreated + " : Date Completed " + tRun.DateCompleted);
            return tRun.State.ToString(); ;
        }

        public static void CreateTestRun(string sId,int sCount, int eCount,bool splitMode = false)
        {
            NetworkCredential cred = new NetworkCredential("SERVICE_ACCOUNT_NAME", "SERVICE_ACCOUNT_PASSWORD", "DOMAIN");
            TfsTeamProjectCollection tfs = new TfsTeamProjectCollection(TfsTeamProjectCollection.GetFullyQualifiedUriForName(ConfigFile.col), cred);
            LogMessageToFile(tfs.HasAuthenticated.ToString() + " -  The authentication status to tfs .");
            ITestManagementService tms = tfs.GetService<ITestManagementService>();
            ITestManagementTeamProject proj = tms.GetTeamProject(ConfigFile.proj);
            ITestPlan tPlan = proj.TestPlans.Find(Convert.ToInt32(ConfigFile.pId));
            ITestPointCollection tpc = null;
            if (string.IsNullOrWhiteSpace(sId))
            {
                tpc = tPlan.QueryTestPoints("Select * from TestPoint where PlanId=" + ConfigFile.pId + " and ConfigurationId=" + ConfigFile.cId);
                LogMessageToFile("The QueryText passed is : " + "Select * from TestPoint where PlanId=" + ConfigFile.pId + " and ConfigurationId=" + ConfigFile.cId);
            }
            else
            {
                string queryText = null; queryText = TCMFunctions.ExtractQueryForSuiteIDs(sId, queryText, true);
                LogMessageToFile("The QueryText passed is : " + queryText);
                tpc = tPlan.QueryTestPoints(queryText);
            }
            ITestRun tRun = tPlan.CreateTestRun(true);
            if (splitMode)
            {
                foreach (ITestPoint tp in tpc)
                {
                    tRun.AddTestPoint(tp, null);
                }
                LogMessageToFile("Added the test points for split run. The Suite Ids are : " + sId);
               
                tRun.Title = ConfigFile.Runtitle + Convert.ToString(ConfigFile.order);
                
            }
            else
            {
                for (int i = sCount; i <= eCount; i++)
                {
                    tRun.AddTestPoint(tpc[i], null);
                }
                LogMessageToFile("Added the test points starting from : " + sCount + " to : " + eCount);
                tRun.Title = ConfigFile.Runtitle + Convert.ToString(ConfigFile.order);
            }
            ITestSettingsHelper iTSH = proj.TestSettings;
            ITestSettings iTs = iTSH.Find(ConfigFile.settingsId);
            tRun.CopyTestSettings(iTs);
            tRun.BuildDirectory = destFilepath;
            LogMessageToFile("The Build Path assigned for this run is : " + destFilepath);

            tRun.Save();
            sp.curRunId = sp.runId = tRun.Id.ToString();
            LogMessageToFile("The Run Id generated is : " + sp.runId);
        }

        public static int GetTestPointsCount()
        {
            NetworkCredential cred = new NetworkCredential("SERVICE_ACCOUNT_NAME", "SERVICE_ACCOUNT_PASSWORD", "DOMAIN");
            TfsTeamProjectCollection tfs = new TfsTeamProjectCollection(TfsTeamProjectCollection.GetFullyQualifiedUriForName(ConfigFile.col), cred);
            ITestManagementService tms = tfs.GetService<ITestManagementService>();
            ITestManagementTeamProject proj = tms.GetTeamProject(ConfigFile.proj);
            ITestPlan tPlan = proj.TestPlans.Find(Convert.ToInt32(ConfigFile.pId));
            ITestPointCollection tpc = null;
            if (string.IsNullOrWhiteSpace(ConfigFile.sId))
            {
                tpc = tPlan.QueryTestPoints("Select * from TestPoint where PlanId=" + ConfigFile.pId + " and ConfigurationId=" + ConfigFile.cId);
                LogMessageToFile("The QueryText passed is : " + "Select * from TestPoint where PlanId=" + ConfigFile.pId + " and ConfigurationId=" + ConfigFile.cId);
            }
            else
            { 
                string queryText = null; queryText = TCMFunctions.ExtractQueryForSuiteIDs(ConfigFile.sId,queryText,true);
                LogMessageToFile("The QueryText passed is : " + queryText);
                tpc = tPlan.QueryTestPoints(queryText);
            }
            totalPoints = tpc.Count();
            LogMessageToFile("The Total Number of TestPoints available are : " + totalPoints);
            return totalPoints;
        }

        public static TestMessageLogEntryCollection ReadLogFile(string runId)
        {
            LogMessageToFile("Trying to Get Reset Status for the run id " + runId);
            NetworkCredential cred = new NetworkCredential("SERVICE_ACCOUNT_NAME", "SERVICE_ACCOUNT_PASSWORD", "DOMAIN");
            TfsTeamProjectCollection tfs = new TfsTeamProjectCollection(TfsTeamProjectCollection.GetFullyQualifiedUriForName(ConfigFile.col), cred);
            ITestManagementService tms = tfs.GetService<ITestManagementService>();
            ITestManagementTeamProject proj = tms.GetTeamProject(ConfigFile.proj);
            ITestRun trun = proj.TestRuns.Find(Convert.ToInt32(runId));
            LogMessageToFile("TestRun exists for the given run id : " + runId);

            try
            {
                LogMessageToFile("Trying to fetch the log messages for the test run id : " + runId);
                return trun.TestMessageLogEntries;
            }
            catch (Exception exe)
            {
                LogMessageToFile("Failed to get the LogFile for runId : " + runId);
                return null;
            }

            //tmessageLog
        }

        public static void WaitForController(bool splitRun = false)
        {
            try
            {
                LogMessageToFile("Current Method executing is 'public static void WaitForController(bool splitRun = false)'");
                NetworkCredential cred = new NetworkCredential("SERVICE_ACCOUNT_NAME", "SERVICE_ACCOUNT_PASSWORD", "DOMAIN");
                TfsTeamProjectCollection tfs = new TfsTeamProjectCollection(TfsTeamProjectCollection.GetFullyQualifiedUriForName(ConfigFile.col), cred);
                ITestManagementService tms = tfs.GetService<ITestManagementService>();
                ITestManagementTeamProject proj = tms.GetTeamProject(ConfigFile.proj);
                ITestRunHelper tRuns = proj.TestRuns;

                LogMessageToFile("Executing Wait For Controller");

                string runTitle = null;

                if (splitRun)
                {
                   
                    runTitle = "SR_0";
                }
                else
                {
                    runTitle = "DR_0";

                }
                string tRunQString = "Select * from TestRun where Title Contains '" + runTitle + "' order by CreationDate desc";
                IEnumerable<ITestRun> tRunQuery = tRuns.Query(tRunQString);
                LogMessageToFile("The query string passed to the Test Runs query is : " + tRunQString);
                ITestRun tRun = tRunQuery.OrderBy(x => x.DateCreated).LastOrDefault();
                string trunId = tRun.Id.ToString();
                Console.WriteLine("The Run id queried is : " + tRun.Id);
                LogMessageToFile("The Title of the Run-" + tRun.Title + " : Date Created " + tRun.DateCreated + " : Date Completed " + tRun.DateCompleted);

                TestMessageLogEntryCollection tMessage = ReadLogFile(trunId);
                LogMessageToFile("The Number of message read is : " + tMessage.Count());
                string[] str = string.Join(",", tMessage).Split(new string[] { "Preparing to execute test run" }, StringSplitOptions.None);
                MtmLogFiles(str, trunId);
                int count = str.Count() - 1;
                LogMessageToFile("The Count of the logic is : " + count.ToString());
                bool testReset = count == 2;
                LogMessageToFile("The boolean value of the truth logic is : " + testReset.ToString());
                testReset = (tRun.State == TestRunState.Completed) ? true : false;
                if (tRun.State == TestRunState.Completed)
                {

                    testReset = true;
                    LogMessageToFile("The State of Run is :" + tRun.State);
                }
				
                while (testReset == false && (!(tRun.State == TestRunState.Completed)))
                {
                    tMessage = ReadLogFile(trunId);
                    string[] str1 = string.Join(",", tMessage).Split(new string[] { "Preparing to execute test run" }, StringSplitOptions.None);
                    //MtmLogFiles(str1, trunId);
                    count = str1.Count() - 1;
                    LogMessageToFile("The Count of the logic is : " + count.ToString());
                    testReset = count == 2;
                    LogMessageToFile("The boolean value of the truth logic is : " + (count == 2).ToString());
                    LogMessageToFile("The boolean value of the truth logic is : " + testReset.ToString());
                    LogMessageToFile("The State of the Test Runs is : " + tRun.State);
                    Console.WriteLine("The State of the Test Runs is : " + tRun.State);
                    System.Threading.Thread.Sleep(500000);
                }
            }
            catch (Exception ex)
            {
                LogMessageToFile("Exception caught while waiting for controller is " + ex.ToString() + " " + ex.StackTrace.ToString());
            }
        }

        public static void WaitForReRunResults(int orderId,bool splitRun = false)
        {
            try
            {
                LogMessageToFile("Current Method executing is 'public static void WaitForReRunResults(int orderId,bool splitRun = false)'");
                NetworkCredential cred = new NetworkCredential("SERVICE_ACCOUNT_NAME", "SERVICE_ACCOUNT_PASSWORD", "DOMAIN");
                TfsTeamProjectCollection tfs = new TfsTeamProjectCollection(TfsTeamProjectCollection.GetFullyQualifiedUriForName(ConfigFile.col), cred);
                ITestManagementService tms = tfs.GetService<ITestManagementService>();
                ITestManagementTeamProject proj = tms.GetTeamProject(ConfigFile.proj);
                ITestRunHelper tRuns = proj.TestRuns;

                LogMessageToFile("Executing Wait For Reset");

                string runTitle = null;

                if (splitRun)
                {
                    runTitle = "SR_" + Convert.ToString(orderId);
                }
                else
                {
                    runTitle = "DR_" + Convert.ToString(orderId);
                }
                string tRunQString = "Select * from TestRun where Title Contains '" + runTitle + "' order by CreationDate desc";
                IEnumerable<ITestRun> tRunQuery = tRuns.Query(tRunQString);
                LogMessageToFile("The query string passed to the Test Runs query is : " + tRunQString);
                ITestRun tRun = tRunQuery.OrderBy(x => x.DateCreated).LastOrDefault();
                string trunId = tRun.Id.ToString();
                Console.WriteLine("The Run id queried is : " + tRun.Id);
                LogMessageToFile("The Title of the Run-" + tRun.Title + " : Date Created " + tRun.DateCreated + " : Date Completed " + tRun.DateCompleted);

                TestMessageLogEntryCollection tMessage = ReadLogFile(trunId);
                LogMessageToFile("The Number of message read is : " + tMessage.Count());
                string[] str = string.Join(",", tMessage).Split(new string[] { "Preparing to execute test run" }, StringSplitOptions.None);
                MtmLogFiles(str, trunId);
                int count = str.Count() - 1;
                LogMessageToFile("The Count of the logic is : "+count.ToString());
                bool testReset = count >= 2;
                LogMessageToFile("The boolean value of the truth logic is : " + testReset.ToString());
                while (testReset == false && (!(tRun.State == TestRunState.Completed)))
                {
                    tMessage = ReadLogFile(trunId);
                    string[] str1 = string.Join(",", tMessage).Split(new string[] { "Preparing to execute test run" }, StringSplitOptions.None);
                    //MtmLogFiles(str1, trunId);
                    count = str1.Count() - 1;
                    LogMessageToFile("The Count of the logic is : " + count.ToString());
                    testReset = count == 2;
                    LogMessageToFile("The boolean value of the truth logic is : " + (count == 2).ToString());
                    LogMessageToFile("The boolean value of the truth logic is : " + testReset.ToString());
                    LogMessageToFile("The State of the Test Runs is : " + tRun.State);
                    Console.WriteLine("The State of the Test Runs is : " + tRun.State);
                    System.Threading.Thread.Sleep(500000);
                }
            }
            catch (Exception ex)
            {
                LogMessageToFile("Exception caught while waiting for controller is " + ex.ToString() + " " + ex.StackTrace.ToString());
            }
        }

        public static void WaitForExecutor(int orderId, bool splitRun = false)
        {
            try
            {
                NetworkCredential cred = new NetworkCredential("SERVICE_ACCOUNT_NAME", "SERVICE_ACCOUNT_PASSWORD", "DOMAIN");
                TfsTeamProjectCollection tfs = new TfsTeamProjectCollection(TfsTeamProjectCollection.GetFullyQualifiedUriForName(ConfigFile.col), cred);
                ITestManagementService tms = tfs.GetService<ITestManagementService>();
                ITestManagementTeamProject proj = tms.GetTeamProject(ConfigFile.proj);
                ITestRunHelper tRuns = proj.TestRuns;

                LogMessageToFile("Executing Wait For Executors");

                string runTitle = null;

                if (splitRun)
                {
					runTitle = "SR_" + Convert.ToString(orderId);
                }
                else
                {
                    runTitle = "DR_" + Convert.ToString(orderId);
                }
                string tRunQString = "Select * from TestRun where Title Contains '" + runTitle + "' order by CreationDate desc";
                IEnumerable<ITestRun> tRunQuery = tRuns.Query(tRunQString);
                LogMessageToFile("The query string passed to the Test Runs query is : " + tRunQString);
                ITestRun tRun = tRunQuery.OrderBy(x => x.DateCreated).LastOrDefault();
                string trunId = tRun.Id.ToString();
                Console.WriteLine("The Run id queried is : " + tRun.Id);
                LogMessageToFile("The Title of the Run-" + tRun.Title + " : Date Created " + tRun.DateCreated + " : Date Completed " + tRun.DateCompleted);

                while (tRun.State == TestRunState.InProgress || tRun.State == TestRunState.NotStarted || tRun.State == TestRunState.Waiting)
                {
                    tRuns = proj.TestRuns;
                    tRunQuery = tRuns.Query(tRunQString);
                    tRun = tRunQuery.OrderBy(x => x.DateCreated).LastOrDefault();

                    LogMessageToFile("The State of the Test Runs is : " + tRun.State);
                    Console.WriteLine("The State of the Test Runs is : " + tRun.State);
                    System.Threading.Thread.Sleep(500000);
                }
            }
            catch (Exception ex)
            {
                LogMessageToFile("Exception caught while waiting for controller is " + ex.ToString() + " " + ex.StackTrace.ToString());
            }
        }

        public static void ResetTestCasesForKnovaAndRun(string runId)
        {
            try
            {
                LogMessageToFile("Trying to reset Failed test cases for knova for the run id " + runId);
                NetworkCredential cred = new NetworkCredential("SERVICE_ACCOUNT_NAME", "SERVICE_ACCOUNT_PASSWORD", "DOMAIN");
                TfsTeamProjectCollection tfs = new TfsTeamProjectCollection(TfsTeamProjectCollection.GetFullyQualifiedUriForName(ConfigFile.col), cred);
                ITestManagementService tms = tfs.GetService<ITestManagementService>();
                ITestManagementTeamProject proj = tms.GetTeamProject(ConfigFile.proj);
                ITestRun trun = proj.TestRuns.Find(Convert.ToInt32(runId));
                LogMessageToFile("TestRun exists for the given run id : " + runId);
                ITestCaseResultHelper iTCRHelper = proj.TestResults;
                var results = iTCRHelper.Query(String.Format("Select * from TestResult where TestRunId='{0}' and Outcome<>'{1}' and Priority='{2}'", Convert.ToInt32(runId), TestOutcome.Passed,"1"));
                int errorOrNotExecutedCount = results.Count();

                if (errorOrNotExecutedCount > 0)
                {
                    LogMessageToFile("Trying to reset all the failure outcome test cases for Knova");
                    foreach (var result in results)
                    {
                        string id = result.TestCaseId.ToString();
                        if (ProjectCustomization.rerunMapping.ContainsKey(id))
                        {
                            LogMessageToFile("The Test case " + id + " is contained in the dictionary of Rerun ");
                            foreach (string testcase in ProjectCustomization.rerunMapping[id])
                            {
                                LogMessageToFile("Trying to apply Reset for the pre-requisite test case mentioned " + testcase);
                                foreach (var reset in iTCRHelper.Query(string.Format("Select * from TestResult where TestRunId='{0}' and TestCaseId='{1}'", Convert.ToInt32(runId), testcase)))
                                {
                                    reset.Reset();
                                }
                            }
                        }
                        result.Reset();
                    }
                    LogMessageToFile("Test Cases reset was successful.");
                    TCMFunctions.TCMExecute(runId);
                }
                LogMessageToFile("Trying to execute the run id : " + runId + " after reset.");
                LogMessageToFile("Execution after test run reset was successful.");
            }
            catch (Exception ex)
            {
                LogMessageToFile("The exception caught at 'public static void ResetFailureTestCases(string runId)' function is :" + ex.ToString() + " : " + ex.StackTrace.ToString());
            }
        }
    }
}
