using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMExecutor
{
    public class Utils : Logger
    {
        public static Dictionary<string, string> ConvertStrToDict(string input)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();
            int index = input.LastIndexOf(",");
            string[] seqNum = new string[] { input.Substring(0, index), input.Substring(index + 1) };
            string one = seqNum[0].Remove(0, 1).Trim();
            int secCount = seqNum[1].ToCharArray().Count();
            string two = seqNum[1].Remove(secCount - 1, 1).Trim();
            output.Add(one, two);
            return output;
        }

        public static string ExtractQueryForSuiteIDs(string sId, string configId,string planId, bool type = false)
        {
            string queryText = null;
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
                                     + configId + " and PlanId =" + planId + "\"";
                }
                else
                {
                    queryText = "SELECT * FROM TestPoint WHERE SuiteId=" + sIdQuery + " and ConfigurationId="
                                        + configId + " and PlanId =" + planId;
                }
            }
            else
            {
                if (!type)
                {
                    queryText = "\"" + "SELECT * FROM TestPoint WHERE SuiteId=" + sId + " and ConfigurationId="
                        + configId + " and PlanId =" + planId + "\"";
                }
                else
                {
                    queryText = "SELECT * FROM TestPoint WHERE SuiteId=" + sId + " and ConfigurationId="
                            + configId + " and PlanId =" + planId;
                }
            }
            return queryText;
        }

        public static string ExtractQueryForTestCases(string sId, string planId, bool type = false)
        {
            string queryText = null;
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
                        sIdQuery += sIds[i] + "' or SuiteID ='";
                    }
                }
                if (!type)
                {
                    queryText = "\"" + "SELECT * FROM TestPoint WHERE SuiteId='" + sIdQuery + " and PlanId =" + planId + "\"";
                }
                else
                {
                    queryText = "SELECT * FROM TestPoint WHERE SuiteId='" + sIdQuery + "' and PlanId ='" + planId + "'";
                }
            }
            else
            {
                if (!type)
                {
                    queryText = "\"" + "SELECT * FROM TestPoint WHERE SuiteId=" + sId + " and PlanId =" + planId + "\"";
                }
                else
                {
                    queryText = "SELECT * FROM TestPoint WHERE SuiteId=" + sId + " and PlanId =" + planId;
                }
            }
            return queryText;
        }

        public static string SetTCMPath()
        {
            string tcmPath = null;
            if (!File.Exists(@"C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\IDE\TCM.exe"))
            {
                string[] tcmPaths = GetTcmPath().Split(new string[] { "Directory of" }, StringSplitOptions.None)[1].Split(new string[] { "IDE" }, StringSplitOptions.None);
                tcmPath = tcmPaths[0].Trim() + @"IDE\";
            }
            else
            {
                tcmPath = @"C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\IDE\";
            }
            LogMessageToFile("The TCM executable path is : " + tcmPath);
            Console.WriteLine("The TCM Path is : " + tcmPath);
            return tcmPath;
        }

        public static long GetDirectorySize(string path)
        {
            string[] files = Directory.GetFiles(path);
            string[] subdirectories = Directory.GetDirectories(path);

            long size = files.Sum(x => new FileInfo(x).Length);
            foreach (string s in subdirectories)
                size += GetDirectorySize(s);
            return size;
        }

        public static string GetTcmPath()
        {
            Console.WriteLine("Initiated search for TCM Path...");
            string[] cntrlines = { "cd /",
                                 "dir tcm.exe /s /p", 
                                 "exit"
                                 };
            string tcmPathBat = tempPath + Program.tmp + "tcmpath.bat";
            LogBatFileOutput(cntrlines, tcmPathBat);
            System.IO.File.WriteAllLines(tcmPathBat, cntrlines);
            Console.WriteLine("Done");
            return GetCmdOut(tcmPathBat);
        }

        public static string GetCmdOut(string target)
        {
            LogMessageToFile("Executing the command file : " + target);
            Console.WriteLine("Batch Command initiating...");
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = target;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.UseShellExecute = false;

            proc.Start();
            proc.WaitForExit();
            string output1 = proc.StandardError.ReadToEnd();
            proc.WaitForExit();
            string output2 = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();
            LogMessageToFile(output1 + ":" + output2);
            LogMessageToFile("Successfully executed batch file command for : " + target);
            Console.WriteLine("Done");
            return output1 + ":" + output2;
        }

        public static string RBCopy(string sourceFilePath,string destFilePath)
        {
            Console.WriteLine("Initiating RoboCopy in command line...");
            LogMessageToFile("Initiated ROBOCOPY copy");
            string[] lines = { "ROBOCOPY " + "\"" + sourceFilePath + "\"" + " " + "\"" + destFilePath + "\"" + " /MIR /E /MT[:2]" };
            string tcmBat = tempPath + Program.tmp + "tcmRBOut.bat";
            System.IO.File.WriteAllLines(tcmBat, lines);
            LogMessageToFile("Successful ROBOCOPY copy");
            Console.WriteLine("Done");
            return GetCmdOut(tcmBat, 1,sourceFilePath,destFilePath);
        }

        public static string GetCmdOut(string target, int numberOfChars,string sourceFilePath,string destFilePath)
        {
            LogMessageToFile("Executing the command file : " + target);
            Console.WriteLine("Batch Command initiating...");
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = target;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.UseShellExecute = false;

            proc.Start();

            while (true)
            {
                if (GetDirectorySize(sourceFilePath) == GetDirectorySize(destFilePath))
                {
                    break;
                }
                else { System.Threading.Thread.Sleep(1500); }
            }

            string output2 = "Completed";
            LogMessageToFile(output2);
            Console.WriteLine("Done");
            return ":" + output2;
        }
    }
}
