using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCMExecutor
{
    public static class Program
    {
        public static string exePath = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath.Split(new string[] {"TCMExecutor"},StringSplitOptions.None)[0];
        //public static string newPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
        public static string tempPath = Logger.GetTempPath();
        public static string cacheFolder = "ExecutorCache";
        public static string cacheXml = "Cache.xml";
        public static string cacheXmlPath = tempPath + "\\" + cacheFolder + "\\" + cacheXml;
        public static string tcmPath = Utils.SetTCMPath();
        public static string tmp = @"Temp_Executor\";
        public static bool isCached = false;
        public static List<string> suiteList = new List<string>();
        public static List<string> testList = new List<string>();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TELauncher());
        }
    }
}
