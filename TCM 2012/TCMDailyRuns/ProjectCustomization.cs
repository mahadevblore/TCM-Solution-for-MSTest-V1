using Microsoft.VisualStudio.TestTools.UITesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace TCMDailyRuns
{
    public class ProjectCustomization:Program
    {
        public static Dictionary<string, List<string>> rerunMapping = new Dictionary<string, List<string>>();
        public static void InitiateCustomization()
        {
            Console.WriteLine("Initiating Project Customization.....");

            switch (ConfigFile.proj.ToUpper())
            {
                case "<SPL_CASES>":
                    //if (ConfigFile.type.ToUpper() == "CONTROLLER")
                    if (ConfigFile.order == 0)
                    {
                        TCMFunctions.TCMPRCreateAndExecute(ConfigFile.prsId, ConfigFile.pId, ConfigFile.cId);
                    }
                    break;
               
                default:
                    break;
            }
            LogMessageToFile("Initiation Completed");
        }    

        public static void ReRunCustomization()
        {
            Console.WriteLine("Initiating Re Run Customization.....");

            switch (ConfigFile.proj.ToUpper())
            {
                case "<SPL_RERUN_CASES>":
                    LogMessageToFile("Running the precondition BAT file for Factory MES Project");
                    try
                    {
                        string echo = CmdLine.GetCmdOut(ConfigFile.restoreDbPath);
                        Console.WriteLine(echo);

                        System.Threading.Thread.Sleep(300000);

                    }
                    catch (Exception ex)
                    {
                        LogMessageToFile("Exception found while running precondition BAT file for Factory MES is " + ex.ToString());
                    }

                    break;


                default:
                    break;
            }
        }
    }
}
