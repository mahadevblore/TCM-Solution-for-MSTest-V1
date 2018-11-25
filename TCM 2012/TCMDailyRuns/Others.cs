using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ServiceProcess;

namespace TCMDailyRuns
{
    public class Dashboard
    {
        public static string xmlPath;
        public static string jsonPath;
        public static string xmlBackUp;
        public static string jsonBackUp;
    }

    public class CmdLine : Program
    {
        public static string GetCmdOutput(string target)
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

        public static string SqlCmdExecution(string target)
        {
            LogMessageToFile("Executing the command file : " + target);
            Console.WriteLine("Batch Command initiating...");
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = target;
            proc.StartInfo.UseShellExecute = false;

            proc.Start();
            proc.WaitForExit();
            //string output1 = proc.StandardError.ReadToEnd();
            //proc.WaitForExit();
            //string output2 = proc.StandardOutput.ReadToEnd();
            //proc.WaitForExit();
            //LogMessageToFile(output1 + ":" + output2);
            LogMessageToFile("Successfully executed batch file command for : " + target);
            Console.WriteLine("Done");
            return "done";
        }

        public static string GetCmdOut(string target,bool output = false)
        {
            //LogMessageToFile("Executing the command file : " + target);
            //Console.WriteLine("Batch Command initiating...");
            //System.Diagnostics.Process proc = new System.Diagnostics.Process();
            //proc.StartInfo.FileName = target;
            //proc.StartInfo.RedirectStandardError = true;
            //proc.StartInfo.RedirectStandardOutput = true;
            //proc.StartInfo.UseShellExecute = false;
            
            //proc.Start();
            //proc.WaitForExit();
            //string output1 = proc.StandardError.ReadToEnd();
            //proc.WaitForExit();
            //string output2 = proc.StandardOutput.ReadToEnd();
            //proc.WaitForExit();
            //LogMessageToFile(output1 + ":" + output2);
            //LogMessageToFile("Successfully executed batch file command for : "+target);
            //Console.WriteLine("Done");
            //return output1 + ":" + output2;
            LogMessageToFile("Start Command Process");
            try
            {
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.FileName = target;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                proc.WaitForExit();
                if (output)
                {
                    string output2 = proc.StandardOutput.ReadToEnd();
                    return output2;
                }
                else
                {
                    return "done";
                }
            }
            catch (Exception exe)
            {
                LogMessageToFile(exe.Message);
                return "error";
            }
        }

        public static string GetCmdOutForFactory(string target)
        {
            LogMessageToFile("Executing the command file : " + target);
            Console.WriteLine("Batch Command initiating...");
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            ProcessStartInfo commandInfo = new ProcessStartInfo(target);
            //commandInfo.WindowStyle = ProcessWindowStyle.Hidden;
            commandInfo.UseShellExecute = false;
            proc.StartInfo = commandInfo;
            proc.Start();
            System.Threading.Thread.Sleep(6500);
            ServiceController sc = new ServiceController("FactoryMES-Triggers");
            while (sc.Status.Equals(ServiceControllerStatus.Running) || sc.Status.Equals(ServiceControllerStatus.StartPending))
            {
                System.Threading.Thread.Sleep(1500);
            }
            LogMessageToFile("Successfully executed batch file command for : " + target);
            Console.WriteLine("Done");
            return "Done";
        }

        public static string RBCopy()
        {
            Console.WriteLine("Initiating RoboCopy using process.start");
            LogMessageToFile("Initiated ROBOCOPY copy");
            //string[] lines = { "ROBOCOPY " + "\"" + sourceFilePath + "\"" + " " + "\"" + destFilepath + "\"" + " /MIR /E /MT[:2]" };
            //string tcmBat = curDir + tmp + "tcmRBOut.bat";
            //System.IO.File.WriteAllLines(tcmBat, lines);
            //Console.WriteLine("Done");
            //return CmdLine.GetCmdOut(tcmBat, 1);
            //LogMessageToFile("Successful ROBOCOPY copy");
            try
            {
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = "ROBOCOPY";
                proc.StartInfo.Arguments = "\"" + sourceFilePath + "\"" + " " + "\"" + destFilepath + "\"" + " /MIR /E /MT[:2]";
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                proc.WaitForExit();
                return "done";
            }
            catch (Exception exe)
            {
                LogMessageToFile(exe.Message);
                return "error";
            }
        }

        public static string GetCmdOut(string target, int numberOfChars)
        {
             string output2 =null;
            try
            {
                LogMessageToFile("Executing the command file : " + target);
                Console.WriteLine("Batch Command initiating...");
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = target;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.UseShellExecute = false;
                proc.Start();

                LogMessageToFile("While loop validation started");

                while (true)
                {
                    try
                    {
                        if (GetDirectorySize(sourceFilePath) == GetDirectorySize(destFilepath))
                        {
                            break;
                        }
                        else { System.Threading.Thread.Sleep(1500); }
                    }
                    catch (Exception ex)
                    {
                        LogMessageToFile(ex.ToString());
                        break;
                    }
                }

                output2 = "Completed";
                LogMessageToFile(output2);
                Console.WriteLine("Done");
            }
            catch (Exception ex)
            {
                LogMessageToFile(ex.ToString());
            }
            return ":" + output2;
        }

        public static string GetCmdRBOut(string target, int numberOfChars)
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
            string output2 = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();
            output2 = output2 + " : " + proc.StandardError.ReadToEnd();
            LogMessageToFile(output2);
            Console.WriteLine("Done");
            return ":" + output2;
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
            string tcmPathBat = curDir + tmp + "tcmpath.bat";
            LogBatFileOutput(cntrlines, tcmPathBat);
            System.IO.File.WriteAllLines(tcmPathBat, cntrlines);
            Console.WriteLine("Done");
            return CmdLine.GetCmdOut(tcmPathBat);
        }
    }

    public class ConfigFile
    {
        public static string proj;
        public static string pId ;
        public static string sId ;
        public static string prsId;
        public static string cId ;
        public static string col;
        public static string actDrpPath ;
        public static string def ;
        public static string Flav ;
        public static string platfrm ;
        public static string environmnt;
        public static string settingsnme ;
        public static int settingsId;
        public static bool iterations ;
        public static bool distribute ;
        //public static string type;
        public static int systems;
        public static int order;
        public static bool splitRun;
        public static string controllerSuite;
        public static string executorSuite;
        public static bool testingMode;
        public static bool orderByPriority;
        public static int retryAttemptForFailure;
        public static string userName;
        public static string passWord;
        public static string remoteMachineName;
        public static string restoreDbPath;
        public static string configScript;
        public static bool waitForReRun;
        public static bool skipReRun;
        public static string Runtitle;
        public static string RuntitleExecutor;
        public static string toEmailID;
        public static string ccEmailID;
        public static string mtmLink;
        public static string smtpServerIP;
    }
}
