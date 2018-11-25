using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using TCMDailyRuns;

namespace TCMDailyRuns
{
    public struct SelectedPlan
    {
        //Overall testcases
        public int passed;
        public int failed;
        public int blocked;
        public int active;
        public int error;
        public int Inconclusive;
        public int aborted;
        public int other;
        public int timeout;
        public int notExecuted;
        public int total;
        //p1 testcases
        public int p1Passed;
        public int p1Failed;
        public int p1Blocked;
        public int p1Active;
        public int p1Error;
        public int p1Inconclusive;
        public int p1Aborted;
        public int p1Other;
        public int p1Timeout;
        public int p1Total;
        //p2 testcases
        public int p2Passed;
        public int p2Failed;
        public int p2Blocked;
        public int p2Active;
        public int p2Error;
        public int p2Inconclusive;
        public int p2Aborted;
        public int p2Other;
        public int p2Timeout;
        public int p2Total;
        //p3 testcases
        public int p3Passed;
        public int p3Failed;
        public int p3Blocked;
        public int p3Active;
        public int p3Error;
        public int p3Inconclusive;
        public int p3Aborted;
        public int p3Other;
        public int p3Timeout;
        public int p3Total;
        //Time and Id
        public string StartTime;
        public string EndTime;
        public string runId;
        public string prRunId;
        public string curRunId;
    }

    public class XML:Program
    {
        public static string xmlDirectory;
        public static string splogFilename;
        public static string combinedPath;

        public static void ClearSelectedPlan()
        {
            sp.passed = 0;
            sp.failed = 0;
            sp.blocked = 0;
            sp.active = 0;
            sp.error = 0;
            sp.Inconclusive = 0;
            sp.aborted = 0;
            sp.other = 0;
            sp.timeout = 0;
            sp.notExecuted = 0;
            sp.total = 0;
            //p1 testcases
            sp.p1Passed = 0;
            sp.p1Failed = 0;
            sp.p1Blocked = 0;
            sp.p1Active = 0;
            sp.p1Error = 0;
            sp.p1Inconclusive = 0;
            sp.p1Aborted = 0;
            sp.p1Other = 0;
            sp.p1Timeout = 0;
            sp.p1Total = 0;
            //p2 testcases
            sp.p2Passed = 0;
            sp.p2Failed = 0;
            sp.p2Blocked = 0;
            sp.p2Active = 0;
            sp.p2Error = 0;
            sp.p2Inconclusive = 0;
            sp.p2Aborted = 0;
            sp.p2Other = 0;
            sp.p2Timeout = 0;
            sp.p2Total = 0;
            //p3 testcases
            sp.p3Passed = 0;
            sp.p3Failed = 0;
            sp.p3Blocked = 0;
            sp.p3Active = 0;
            sp.p3Error = 0;
            sp.p3Inconclusive = 0;
            sp.p3Aborted = 0;
            sp.p3Other = 0;
            sp.p3Timeout = 0;
            sp.p3Total = 0;
        }

        public static bool ReadGeneratedSplogFileAndAssertOutcome(string splogPath)
        {
            Console.WriteLine("Loading generated config file ....");
            LogMessageToFile("Started reading generated splog file");
            XmlDocument doc = new XmlDocument();
            doc.Load(splogPath);
            LogMessageToFile("The splog file was read from the directory path : " + splogPath);
            XmlNode node = doc.SelectSingleNode(@"//TestResult");
            XmlElement element = (XmlElement)node;
            string total = element.GetAttribute("OTotal").Trim();
            string failed = element.GetAttribute("OFailed").Trim();
            string passed = element.GetAttribute("OPassed").Trim();
            string runId = element.GetAttribute("OTestRunId");

            //Logging results
            LogMessageToFile("The Result of the run : "+ runId);
            LogMessageToFile("Total Tests: "+total);
            LogMessageToFile("Total failed Tests: " + failed);
            LogMessageToFile("Successfully read splog file");
            Console.WriteLine("Done");

            if (Convert.ToInt32(passed) == 0)
            {
                LogMessageToFile("The Test Execution Outcome is : True");
                testExecutionFailureOutcome = true;
                return testExecutionFailureOutcome;
            }
            else
            {
                LogMessageToFile("The Test Execution Outcome is : False");
                testExecutionFailureOutcome = false;
                LogMessageToFile("The Test Execution Outcome is : "+testExecutionFailureOutcome.ToString());
                return testExecutionFailureOutcome;
            }
        }

        public static void ReadConfigFile()
        {
            Console.WriteLine("Loading execution config file ....");
            LogMessageToFile("Started reading ExeConfig.xml file");
            XmlDocument doc = new XmlDocument();
            doc.Load(exePath+"\\"+"ExeConfig.xml");
            LogMessageToFile("The Config file was read from the directory path : " + exePath + "\\" + "ExeConfig.xml");
            XmlNode node = doc.SelectSingleNode(@"//Main/Configuration");
            XmlElement element = (XmlElement)node;
            ConfigFile.proj = element.GetAttribute("Project");
            ConfigFile.pId = element.GetAttribute("PlanID");
            ConfigFile.sId = element.GetAttribute("SuiteID");
            ConfigFile.prsId = element.GetAttribute("PRSuiteID");
            ConfigFile.cId = element.GetAttribute("ConfigurationID");
            ConfigFile.col = element.GetAttribute("Collection");
            ConfigFile.actDrpPath = element.GetAttribute("DropLocation");
            ConfigFile.def = element.GetAttribute("BuildDefinition");
            ConfigFile.Flav = element.GetAttribute("Flavor");
            ConfigFile.platfrm = element.GetAttribute("Platform");
            ConfigFile.environmnt = element.GetAttribute("TestEnvironment");
            ConfigFile.settingsnme = element.GetAttribute("TestSettingName");
            ConfigFile.settingsId = Convert.ToInt32(element.GetAttribute("TestSettingsId"));
            ConfigFile.iterations = Convert.ToBoolean(element.GetAttribute("Iterations").ToLower());

            LogMessageToFile("Project Name : " + ConfigFile.proj);
            LogMessageToFile("Plan Id : " + ConfigFile.pId);
            LogMessageToFile("Suite Id : " + ConfigFile.sId);
            LogMessageToFile("Pre - requisite Suite Id : " + ConfigFile.prsId);
            LogMessageToFile("Configuration Id : " + ConfigFile.cId);
            LogMessageToFile("Collection Name : " + ConfigFile.col);
            LogMessageToFile("Drop Location of the compiler : " + ConfigFile.actDrpPath);
            LogMessageToFile("Build Definition to be used : " + ConfigFile.def);
            LogMessageToFile("Flavor to include : " + ConfigFile.Flav);
            LogMessageToFile("Platform to run : " + ConfigFile.platfrm);
            LogMessageToFile("Test Environment Name : " + ConfigFile.environmnt);
            LogMessageToFile("Test Settings Name : " + ConfigFile.settingsnme);
            LogMessageToFile("Iteration enabled/disabled ? : " + ConfigFile.iterations);

            if (element.HasAttribute("TestingMode"))
            {
                ConfigFile.testingMode = Convert.ToBoolean(element.GetAttribute("TestingMode").ToLower());
            }

            if (element.HasAttribute("OrderByPriority"))
            {
                ConfigFile.orderByPriority = Convert.ToBoolean(element.GetAttribute("OrderByPriority").ToLower());
            }
            if (element.HasAttribute("RetryAttempt"))
            {
                try
                {
                    ConfigFile.retryAttemptForFailure = Convert.ToInt32(element.GetAttribute("RetryAttempt"));
                }
                catch
                { }

                LogMessageToFile("The retry attempt defined for the test execution is : " + ConfigFile.retryAttemptForFailure);
            }
            else
            {
                ConfigFile.retryAttemptForFailure = 5;
            }
            if (element.HasAttribute("UserName"))
            {
                ConfigFile.userName = element.GetAttribute("UserName");
            }
            if (element.HasAttribute("PassWord"))
            {
                ConfigFile.passWord = element.GetAttribute("PassWord");
            }
            if (element.HasAttribute("RemoteMachine"))
            {
                ConfigFile.remoteMachineName = element.GetAttribute("RemoteMachine");
            }
            if (element.HasAttribute("DBRestorePath"))
            {
                ConfigFile.restoreDbPath = element.GetAttribute("DBRestorePath");
                LogMessageToFile("The Restore Path mentioned in the ExeConfig.xml is : " + ConfigFile.restoreDbPath);
            }

            if (element.HasAttribute("ConfigScript"))
            {
                ConfigFile.configScript = element.GetAttribute("ConfigScript");
                LogMessageToFile("The Config Script mentioned in the ExeConfig.xml is : " + ConfigFile.configScript);
            }

            if (element.HasAttribute("Runtitle"))
            {
                ConfigFile.Runtitle = element.GetAttribute("Runtitle");
                LogMessageToFile("The Runtitle mentioned in the ExeConfig.xml is : " + ConfigFile.Runtitle);
            }

            if (element.HasAttribute("RuntitleExecutor"))
            {
                ConfigFile.RuntitleExecutor = element.GetAttribute("RuntitleExecutor");
                LogMessageToFile("The RuntitleExecutor mentioned in the ExeConfig.xml is : " + ConfigFile.RuntitleExecutor);
            }

            if (element.HasAttribute("WaitForReRun"))
            {
                ConfigFile.waitForReRun = Convert.ToBoolean(element.GetAttribute("WaitForReRun").ToLower());
                LogMessageToFile("The WaitForReRun mentioned in the ExeConfig.xml is : " + ConfigFile.waitForReRun.ToString());
            }
            if (element.HasAttribute("SkipReRun"))
            {
                ConfigFile.skipReRun = Convert.ToBoolean(element.GetAttribute("SkipReRun").ToLower());
                LogMessageToFile("The SkipReRun mentioned in the ExeConfig.xml is : " + ConfigFile.skipReRun.ToString());
            }
            ConfigFile.distribute = Convert.ToBoolean(element.GetAttribute("Distribute").ToString().ToLower());
            LogMessageToFile("Distribution enabled ? : " + ConfigFile.distribute);

            if (ConfigFile.distribute)
            {
                string machineType = null;
                //ConfigFile.type = element.GetAttribute("Type");
                ConfigFile.systems = Convert.ToInt32(element.GetAttribute("Systems"));
                ConfigFile.order = Convert.ToInt32(element.GetAttribute("Order"));
                if(ConfigFile.order == 0)
                {
                    machineType = "Controller";
                }
                else
                {
                    machineType = "Executor";
                }
                //LogMessageToFile("The System type of the current test run for distributed cases is : " + ConfigFile.type);
                LogMessageToFile("The System type of the current test run for distributed cases is : " + machineType);
                LogMessageToFile("The Number of systems that are being used for the test run is : " + ConfigFile.systems);
                LogMessageToFile("The Order of the system being used is : " + ConfigFile.order);
            }

            ConfigFile.splitRun = Convert.ToBoolean(element.GetAttribute("SplitRun").ToLower());
            if (ConfigFile.splitRun)
            {
                ConfigFile.systems = Convert.ToInt32(element.GetAttribute("Systems"));
                ConfigFile.order = Convert.ToInt32(element.GetAttribute("Order"));
                ConfigFile.controllerSuite = element.GetAttribute("ControllerSuite");
                ConfigFile.executorSuite = element.GetAttribute("ExecutorSuite");
                LogMessageToFile("The Controller Suites are : " + ConfigFile.controllerSuite);
                LogMessageToFile("The Executor Suites are : " + ConfigFile.executorSuite);
            }

            LogMessageToFile("Successfully read EnvConfig.xml file");
            Console.WriteLine("Done");
        }

        public static void GetEmailConfig()
        {
            Console.WriteLine("Loading email config file ....");
            LogMessageToFile("Started reading EmailConfig.xml file");
            XmlDocument doc = new XmlDocument();
            doc.Load(exePath + "\\" + "EmailConfig.xml");
            LogMessageToFile("The Config file was read from the directory path : " + exePath + "\\" + "EmailConfig.xml");
            XmlNode node = doc.SelectSingleNode("//Email/" + ConfigFile.proj.Replace(" ", string.Empty));
            XmlElement element = (XmlElement)node;
            ConfigFile.toEmailID = element.GetAttribute("EmailAddress");
            ConfigFile.ccEmailID = element.GetAttribute("CCAddress");
            ConfigFile.mtmLink = element.GetAttribute("MTMLink");
            XmlNode serverIPnode = doc.SelectSingleNode("//Email/SMTPServer");
            XmlElement serverIPelement = (XmlElement)serverIPnode;
            ConfigFile.smtpServerIP = serverIPelement.GetAttribute("IP");
            LogMessageToFile("Finished reading EmailConfig.xml file");
        }

        public static void Load(string xml)
        {
            Console.WriteLine("Loading environmental variables config file ....");
            LogMessageToFile("Started reading EnvConfig.xml file");
            XmlDocument doc = new XmlDocument();
            doc.Load(xml);

            if (string.IsNullOrWhiteSpace(ConfigFile.testingMode.ToString()) == false && ConfigFile.testingMode == true)
            {
                XmlNode node = doc.SelectSingleNode("//projects/TestPath");
                XmlElement element = (XmlElement)node;
                Dashboard.xmlPath = element.GetAttribute("SpLogTest");
                Dashboard.jsonPath = element.GetAttribute("JsonLogTest");
                Dashboard.xmlBackUp = element.GetAttribute("SpLogBackUp");
                Dashboard.jsonBackUp = element.GetAttribute("JsonLogBackUp");
            }
            else
            {
                XmlNode node = doc.SelectSingleNode("//projects/" + ConfigFile.proj.Replace(" ", string.Empty));
                XmlElement element = (XmlElement)node;
                Dashboard.xmlPath = element.GetAttribute("SpLog");
                Dashboard.jsonPath = element.GetAttribute("JsonLog");
                Dashboard.xmlBackUp = element.GetAttribute("SpLogBackUp");
                Dashboard.jsonBackUp = element.GetAttribute("JsonLogBackUp");
            }
         
            //Logging results
            LogMessageToFile("XML Path : " + Dashboard.xmlPath);
            LogMessageToFile("JSON Path : " + Dashboard.jsonPath);
            LogMessageToFile("XML Back Up Path : " + Dashboard.xmlBackUp);
            LogMessageToFile("JSON Back Up Path : " + Dashboard.jsonBackUp);
            LogMessageToFile("Successfully read ExeConfig.xml file");
            Console.WriteLine("Done");
        }

        public static void Load(string xml,string projectName)
        {
            Console.WriteLine("Loading env config....");
            LogMessageToFile("Started reading EnvConfig.xml file");
            XmlDocument doc = new XmlDocument();
            doc.Load(xml);
            XmlNode node = doc.SelectSingleNode("//projects/" + projectName);
            XmlElement element = (XmlElement)node;
            Dashboard.xmlPath = element.GetAttribute("SpLog");
            Dashboard.jsonPath = element.GetAttribute("JsonLog");
            Dashboard.xmlBackUp = element.GetAttribute("SpLogBackUp");
            Dashboard.jsonBackUp = element.GetAttribute("JsonLogBackUp");

            //Logging results
            LogMessageToFile("XML Path : " + Dashboard.xmlPath);
            LogMessageToFile("JSON Path : " + Dashboard.jsonPath);
            LogMessageToFile("XML Back Up Path : " + Dashboard.xmlBackUp);
            LogMessageToFile("JSON Back Up Path : " + Dashboard.jsonBackUp);
            LogMessageToFile("Successfully read EnvConfig.xml file");
            Console.WriteLine("Done");
        }

        public static void CreateXMLFile()
        {
            Console.WriteLine("Creating XML file ...");
            LogMessageToFile("Creating XML file for Splunk Log");
            XmlDocument document = new XmlDocument();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;
            settings.NewLineOnAttributes = true;

            splogFilename = ConfigFile.proj + "_" + Dns.GetHostName() + ".splog";
            
            xmlDirectory = curDir + tmp + @"\";
            combinedPath = xmlDirectory + splogFilename;
            XmlElement testElement = document.CreateElement("TestResult");
            document.AppendChild(testElement);
           
            testElement.SetAttribute("ProjectName", ConfigFile.proj);
            
           
            if (recent != null)
            {
                testElement.SetAttribute("BuildNumber", recent);
                testElement.SetAttribute("SuiteID", ConfigFile.sId);
            }
            LogMessageToFile("XML data: "+ConfigFile.proj+" : " +recent+" : " +ConfigFile.sId);
            document.AppendChild(testElement);
            XmlWriter writer = XmlWriter.Create(combinedPath, settings);
            document.Save(writer);
            writer.Close();
            LogMessageToFile("Splunk log path created temporarily is : " + combinedPath);
            Console.WriteLine("Done");
        }

        public static void ClearSpLogFile(string suiteName)
        {
            try
            {
                Console.WriteLine("Appending values into XML file for splog..");
                LogMessageToFile("Appending values into XML file for splog..");
                XmlDocument document = new XmlDocument();
                document.Load(combinedPath);
                XmlElement testElement = document.DocumentElement;

                //P1 Results
                testElement.SetAttribute("P1Total", "");
                testElement.SetAttribute("P1Passed", "");
                testElement.SetAttribute("P1Failed", "");
                testElement.SetAttribute("P1Blocked", "");
                testElement.SetAttribute("P1Active", "");
                testElement.SetAttribute("P1Inconclusive", "");
                testElement.SetAttribute("P1Aborted", "");
                testElement.SetAttribute("P1Error", "");
                testElement.SetAttribute("P1Other", "");

                //P2 Results
                testElement.SetAttribute("P2Total", "");
                testElement.SetAttribute("P2Passed", "");
                testElement.SetAttribute("P2Failed", "");
                testElement.SetAttribute("P2Blocked", "");
                testElement.SetAttribute("P2Active", "");
                testElement.SetAttribute("P2Inconclusive", "");
                testElement.SetAttribute("P2Aborted", "");
                testElement.SetAttribute("P2Error", "");
                testElement.SetAttribute("P2Other", "");

                //P3 Results
                testElement.SetAttribute("P3Total", "");
                testElement.SetAttribute("P3Passed", "");
                testElement.SetAttribute("P3Failed", "");
                testElement.SetAttribute("P3Blocked", "");
                testElement.SetAttribute("P3Active", "");
                testElement.SetAttribute("P3Inconclusive", "");
                testElement.SetAttribute("P3Aborted", "");
                testElement.SetAttribute("P3Error", "");
                testElement.SetAttribute("P3Other", "");

                // Overall Result
                testElement.SetAttribute(suiteName + "TestRunId", "");
                testElement.SetAttribute(suiteName + "StartTime", "");
                testElement.SetAttribute(suiteName + "EndTime", "");
                testElement.SetAttribute(suiteName + "Total", "");
                testElement.SetAttribute(suiteName + "Passed", "");
                testElement.SetAttribute(suiteName + "Failed", "");
                testElement.SetAttribute(suiteName + "Blocked", "");
                testElement.SetAttribute(suiteName + "Active", "");
                testElement.SetAttribute(suiteName + "Inconclusive", "");
                testElement.SetAttribute(suiteName + "Aborted", "");
                testElement.SetAttribute(suiteName + "Error", "");
                testElement.SetAttribute(suiteName + "Other", "");
                testElement.SetAttribute(suiteName + "NotExecuted", "");

                document.AppendChild(testElement);

                document.Save(combinedPath);

                Console.WriteLine("Cleared Splog");
            }
            catch (Exception exe)
            {
            }
        }

        public static bool Generate(SelectedPlan RunReport, string suiteName)
        {
            try
            {
                Console.WriteLine("Appending values into XML file for splog..");
                LogMessageToFile("Appending values into XML file for splog..");
                XmlDocument document = new XmlDocument();
                document.Load(combinedPath);
                XmlElement testElement = document.DocumentElement;

                //P1 Results
                testElement.SetAttribute("P1Total", RunReport.p1Total.ToString());
                testElement.SetAttribute("P1Passed", RunReport.p1Passed.ToString());
                testElement.SetAttribute("P1Failed", RunReport.p1Failed.ToString());
                testElement.SetAttribute("P1Blocked", RunReport.p1Blocked.ToString());
                testElement.SetAttribute("P1Active", RunReport.p1Active.ToString());
                testElement.SetAttribute("P1Inconclusive", RunReport.p1Inconclusive.ToString());
                testElement.SetAttribute("P1Aborted", RunReport.p1Aborted.ToString());
                testElement.SetAttribute("P1Error", RunReport.p1Error.ToString());
                testElement.SetAttribute("P1Other", RunReport.p1Other.ToString());

                //P2 Results
                testElement.SetAttribute("P2Total", RunReport.p2Total.ToString());
                testElement.SetAttribute("P2Passed", RunReport.p2Passed.ToString());
                testElement.SetAttribute("P2Failed", RunReport.p2Failed.ToString());
                testElement.SetAttribute("P2Blocked", RunReport.p2Blocked.ToString());
                testElement.SetAttribute("P2Active", RunReport.p2Active.ToString());
                testElement.SetAttribute("P2Inconclusive", RunReport.p2Inconclusive.ToString());
                testElement.SetAttribute("P2Aborted", RunReport.p2Aborted.ToString());
                testElement.SetAttribute("P2Error", RunReport.p2Error.ToString());
                testElement.SetAttribute("P2Other", RunReport.p2Other.ToString());

                //P3 Results
                testElement.SetAttribute("P3Total", RunReport.p3Total.ToString());
                testElement.SetAttribute("P3Passed", RunReport.p3Passed.ToString());
                testElement.SetAttribute("P3Failed", RunReport.p3Failed.ToString());
                testElement.SetAttribute("P3Blocked", RunReport.p3Blocked.ToString());
                testElement.SetAttribute("P3Active", RunReport.p3Active.ToString());
                testElement.SetAttribute("P3Inconclusive", RunReport.p3Inconclusive.ToString());
                testElement.SetAttribute("P3Aborted", RunReport.p3Aborted.ToString());
                testElement.SetAttribute("P3Error", RunReport.p3Error.ToString());
                testElement.SetAttribute("P3Other", RunReport.p3Other.ToString());

                // Overall Result
                testElement.SetAttribute(suiteName + "TestRunId", RunReport.runId.ToString());
                testElement.SetAttribute(suiteName + "StartTime", RunReport.StartTime);
                testElement.SetAttribute(suiteName + "EndTime", RunReport.EndTime);
                testElement.SetAttribute(suiteName + "Total", RunReport.total.ToString());
                testElement.SetAttribute(suiteName + "Passed", RunReport.passed.ToString());
                testElement.SetAttribute(suiteName + "Failed", RunReport.failed.ToString());
                testElement.SetAttribute(suiteName + "Blocked", RunReport.blocked.ToString());
                testElement.SetAttribute(suiteName + "Active", RunReport.active.ToString());
                testElement.SetAttribute(suiteName + "Inconclusive", RunReport.Inconclusive.ToString());
                testElement.SetAttribute(suiteName + "Aborted", RunReport.aborted.ToString());
                testElement.SetAttribute(suiteName + "Error", RunReport.error.ToString());
                testElement.SetAttribute(suiteName + "Other", RunReport.other.ToString());
                testElement.SetAttribute(suiteName + "NotExecuted", RunReport.notExecuted.ToString());

                document.AppendChild(testElement);
                
                document.Save(combinedPath);

                //Loggin all the value
                LogMessageToFile("The Execution report :");
                LogMessageToFile("Test Run Id is :" + RunReport.runId.ToString());
                LogMessageToFile("Start Time of testrun is :" + RunReport.StartTime.ToString());
                LogMessageToFile("End Time of the testrun is :" + RunReport.EndTime.ToString());

                LogMessageToFile("Total Test cases is :" + RunReport.total.ToString());
                LogMessageToFile("Total Test cases passed is :" + RunReport.passed.ToString());
                LogMessageToFile("Total Test cases failed is :" + RunReport.failed.ToString());
                LogMessageToFile("Total Test cases blocked is :" + RunReport.blocked.ToString());
                LogMessageToFile("Total Test cases active is :" + RunReport.active.ToString());
                LogMessageToFile("Total Test cases inconclusive is :" + RunReport.Inconclusive.ToString());
                LogMessageToFile("Total Test cases aborted is :" + RunReport.aborted.ToString());
                LogMessageToFile("Total Test cases error is :" + RunReport.error.ToString());
                LogMessageToFile("Total Test cases other is :" + RunReport.other.ToString());

                LogMessageToFile("P1 Total Test cases passed is :" + RunReport.p1Passed.ToString());
                LogMessageToFile("P1 Total Test cases failed is :" + RunReport.p1Failed.ToString());
                LogMessageToFile("P1 Total Test cases blocked is :" + RunReport.p1Blocked.ToString());
                LogMessageToFile("P1 Total Test cases active is :" + RunReport.p1Active.ToString());
                LogMessageToFile("P1 Total Test cases inconclusive is :" + RunReport.p1Inconclusive.ToString());
                LogMessageToFile("P1 Total Test cases aborted is :" + RunReport.p1Aborted.ToString());
                LogMessageToFile("P1 Total Test cases error is :" + RunReport.p1Error.ToString());
                LogMessageToFile("P1 Total Test cases other is :" + RunReport.p1Other.ToString());

                LogMessageToFile("P2 Total Test cases passed is :" + RunReport.p2Passed.ToString());
                LogMessageToFile("P2 Total Test cases failed is :" + RunReport.p2Failed.ToString());
                LogMessageToFile("P2 Total Test cases blocked is :" + RunReport.p2Blocked.ToString());
                LogMessageToFile("P2 Total Test cases active is :" + RunReport.p2Active.ToString());
                LogMessageToFile("P2 Total Test cases inconclusive is :" + RunReport.p2Inconclusive.ToString());
                LogMessageToFile("P2 Total Test cases aborted is :" + RunReport.p2Aborted.ToString());
                LogMessageToFile("P2 Total Test cases error is :" + RunReport.p2Error.ToString());
                LogMessageToFile("P2 Total Test cases other is :" + RunReport.p2Other.ToString());

                LogMessageToFile("P3 Total Test cases passed is :" + RunReport.p3Passed.ToString());
                LogMessageToFile("P3 Total Test cases failed is :" + RunReport.p3Failed.ToString());
                LogMessageToFile("P3 Total Test cases blocked is :" + RunReport.p3Blocked.ToString());
                LogMessageToFile("P3 Total Test cases active is :" + RunReport.p3Active.ToString());
                LogMessageToFile("P3 Total Test cases inconclusive is :" + RunReport.p3Inconclusive.ToString());
                LogMessageToFile("P3 Total Test cases aborted is :" + RunReport.p3Aborted.ToString());
                LogMessageToFile("P3 Total Test cases error is :" + RunReport.p3Error.ToString());
                LogMessageToFile("P3 Total Test cases other is :" + RunReport.p3Other.ToString());

                Console.WriteLine("Done");
            }
            catch (Exception exe)
            {
                return false;
            }
            return true;
        }

        public static bool GenerateForFailure(int i, SelectedPlan RunReport, string suiteName)
        {
            try
            {
                Console.WriteLine("Appending values into XML file for splog as 0 for the test execution failure..");
                XmlDocument document = new XmlDocument();
                document.Load(combinedPath);
                XmlElement testElement = document.DocumentElement;

                //P1 Results
                testElement.SetAttribute("P1Total",i.ToString());
                testElement.SetAttribute("P1Passed", i.ToString());
                testElement.SetAttribute("P1Failed", i.ToString());
                testElement.SetAttribute("P1Blocked", i.ToString());
                testElement.SetAttribute("P1Active", i.ToString());
                testElement.SetAttribute("P1Inconclusive", i.ToString());
                testElement.SetAttribute("P1Aborted", i.ToString());
                testElement.SetAttribute("P1Error", i.ToString());
                testElement.SetAttribute("P1Other", i.ToString());

                //P2 Results
                testElement.SetAttribute("P2Total", i.ToString());
                testElement.SetAttribute("P2Passed", i.ToString());
                testElement.SetAttribute("P2Failed", i.ToString());
                testElement.SetAttribute("P2Blocked", i.ToString());
                testElement.SetAttribute("P2Active", i.ToString());
                testElement.SetAttribute("P2Inconclusive", i.ToString());
                testElement.SetAttribute("P2Aborted", i.ToString());
                testElement.SetAttribute("P2Error", i.ToString());
                testElement.SetAttribute("P2Other", i.ToString());

                //P3 Results
                testElement.SetAttribute("P3Total", i.ToString());
                testElement.SetAttribute("P3Passed", i.ToString());
                testElement.SetAttribute("P3Failed", i.ToString());
                testElement.SetAttribute("P3Blocked", i.ToString());
                testElement.SetAttribute("P3Active", i.ToString());
                testElement.SetAttribute("P3Inconclusive", i.ToString());
                testElement.SetAttribute("P3Aborted", i.ToString());
                testElement.SetAttribute("P3Error", i.ToString());
                testElement.SetAttribute("P3Other", i.ToString());

                // Overall Result
                testElement.SetAttribute(suiteName + "TestRunId", RunReport.runId.ToString());
                testElement.SetAttribute(suiteName + "StartTime", RunReport.StartTime);
                testElement.SetAttribute(suiteName + "EndTime", RunReport.EndTime);
                testElement.SetAttribute(suiteName + "Total", i.ToString());
                testElement.SetAttribute(suiteName + "Passed", i.ToString());
                testElement.SetAttribute(suiteName + "Failed", i.ToString());
                testElement.SetAttribute(suiteName + "Blocked", i.ToString());
                testElement.SetAttribute(suiteName + "Active", i.ToString());
                testElement.SetAttribute(suiteName + "Inconclusive", i.ToString());
                testElement.SetAttribute(suiteName + "Aborted", i.ToString());
                testElement.SetAttribute(suiteName + "Error", i.ToString());
                testElement.SetAttribute(suiteName + "Other", i.ToString());

                document.AppendChild(testElement);

                document.Save(combinedPath);

                //Logging all the value
                LogMessageToFile("The Execution report :");
                LogMessageToFile("Test Run Id is :" + RunReport.runId.ToString());
                LogMessageToFile("Start Time of testrun is :" + RunReport.StartTime.ToString());
                LogMessageToFile("End Time of the testrun is :" + RunReport.EndTime.ToString());
                LogMessageToFile("The int Number passed is : " + i.ToString());
                LogMessageToFile("Test Execution failed");
                Console.WriteLine("Done");
            }
            catch (Exception exe)
            {
                return false;
            }
            return true;
        }

    }
}
