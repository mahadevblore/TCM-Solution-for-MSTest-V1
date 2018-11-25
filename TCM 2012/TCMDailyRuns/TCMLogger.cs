using System.IO;
namespace TCMDailyRuns
{
    public class Logger
    {
        public static string tempPath =GetTempPath();

        public static string GetTempPath()
        {
            string path = System.Environment.GetEnvironmentVariable("TEMP");
            if (!path.EndsWith("\\")) path += "\\";
            return path;                    
        }

        public static void LogMessageToFile(string msg)
        {
            System.IO.StreamWriter sw = System.IO.File.AppendText(
                tempPath + "TCMLogFile.txt");
            try
            {
                string logLine = System.String.Format(
                    "{0:G}: {1}.", System.DateTime.Now, msg);
                sw.WriteLine(logLine);  
            }
            finally
            {
                sw.Close();
            }
        }

        public static void LogBatFileOutput(string[] cntrlines, string batFilePath)
        {
            LogMessageToFile("Bat file was created at location : " + batFilePath);
            foreach (string str in cntrlines)
            {
                LogMessageToFile(str);
            }
        }

        public static void MtmLogFiles(string[] cntrlines, string runId)
        {
            LogMessageToFile("The log for the Test Run Id : "+runId);
            foreach (string str in cntrlines)
            {
                LogMessageToFile(str);
            }
        }
    }
}