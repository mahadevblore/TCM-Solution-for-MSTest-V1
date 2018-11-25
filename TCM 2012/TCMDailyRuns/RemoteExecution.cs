using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCMDailyRuns
{
    public class RemoteExecution : Program
    {
        static string batOutput = "";
        static string psBat = "";        
        static bool firewallStatus;
        static string firewallState = "OFF";
        static string psExecPath = Path.Combine(Environment.CurrentDirectory, "External");
        static string tcmBatFilePath=@"C:\Users\<ACCOUNT_NAME>\Desktop\DelTemp.bat";

        private static string GetSessionId()
        {
            batOutput = curDir + tmp + "output.txt";
            System.IO.File.Create(batOutput).Close();   

            //psExec Command To Get The Session Id Of The User
            LogMessageToFile("Getting The User Session ID");
            string[] lines = {   "cd /",
                                 "cd "+psExecPath,   
                                 "psexec \\\\"+Environment.MachineName+"-u DOMAIN\\SERVICE_ACCOUNT_NAME -p SERVICE_ACCOUNT_PASSWORD query session 1>"+batOutput 
                                 };
 
            LogBatFileOutput(lines, psBat);
            System.IO.File.WriteAllLines(psBat, lines);
            CmdLine.GetCmdOut(psBat);
            LogMessageToFile("The Bat File Was Executed Properly And OutPut Is Redirected" + batOutput);
            HashSet<string> strings = new HashSet<string>(File.ReadAllLines(batOutput));

            return strings.Single(x => x.Contains(ConfigFile.userName.Split("\\".ToCharArray())[1])).Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[2];
        }

        public static void firewall()
        {
            try
            {
                //Output.txt File Is Created To Hold All The CommandLine Output
                batOutput = curDir + tmp + "output.txt";
                System.IO.File.Create(batOutput).Close();

                //tcmExecutionBatFile Is Created To Copy It To Remote Mahchine And Trigger It Interactivly
                string tcmExecution = curDir + tmp + "tcmExecution.bat";
                System.IO.File.Create(tcmExecution).Close();

                firewallStatus = true;

                //psExec Command To Find The FireWall Status (Whether Its Turned Off Or On)

                string[] lines = {   "cd /",
                                 "cd "+psExecPath,   
                                 "psexec "+ConfigFile.remoteMachineName+" -u "+ConfigFile.userName+" -p "+ConfigFile.passWord+" netsh advfirewall show allprofiles state 1>"+batOutput 
                                 };

                //Creating The Bat To Execute the psExec Commands
                psBat = curDir + tmp + "PSExecution.bat";
                LogBatFileOutput(lines, psBat);
                System.IO.File.WriteAllLines(psBat, lines);
                CmdLine.GetCmdOut(psBat);

                //Reading The OutPut.txt file
                LogMessageToFile("The Bat File:" + psBat + "Executed Successfully");
                HashSet<string> strings = new HashSet<string>(File.ReadAllLines(batOutput));

                if (strings.Any(x => x.Contains(firewallState)))
                {
                    firewallStatus = false;
                }
                if (firewallStatus)
                {
                    //Cmd To Execute The TCM Bat File 
                    string[] lines2 = {
                                     "NetSh Advfirewall set allprofiles state off",
                                     tcmBatFilePath,
                                     "NetSh Advfirewall set allprofiles state on",
                                  };

                    //Cmd To Copy File To Remote Machine And Execute Interactively
                    string[] script ={
                                     "cd /",
                                     "cd "+psExecPath,
                                     "psexec "+ConfigFile.remoteMachineName +" -u "+ConfigFile.userName+" -p "+ConfigFile.passWord+" -i "+GetSessionId()+" -c -v "+tcmExecution,
                                };
                    LogBatFileOutput(script, psBat);
                    System.IO.File.WriteAllLines(psBat, script);
                    LogBatFileOutput(lines2, tcmExecution);
                    System.IO.File.WriteAllLines(tcmExecution, lines2);
                    LogMessageToFile("The Execution Of TCM Starting.....");
                    CmdLine.GetCmdOut(psBat);
                }
                else
                {
                    MessageBox.Show("Please Turn On the firewall on the system : " + ConfigFile.remoteMachineName);
                }

            }
            catch (Exception exe)
            {
                MessageBox.Show("Exception : " + exe.ToString());
            }
        }

        public static void ConsoleSession()
        {
            LogMessageToFile("Putting Back The Machine Back To Console Session");
            psBat = curDir + tmp + "PSExecution.bat";
            string[] lines = {
                              "cd /",
                              "cd "+psExecPath,
                              "psexec \\\\"+Environment.MachineName +" -u DOMAIN\\SERVICE_ACCOUNT_NAME -p SERVICE_ACCOUNT_PASSWORD -s tscon"+ GetSessionId()+"/dest:console",
                         };
            LogBatFileOutput(lines, psBat);
            System.IO.File.WriteAllLines(psBat, lines);
            CmdLine.GetCmdOut(psBat);
            LogMessageToFile("Machine Has Been Put Back To Console Session");
        }
    }
}
