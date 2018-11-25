using System.IO;
namespace TCMExecutor
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
                tempPath + "ExecutorLogFile.txt");
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
            LogMessageToFile("----------------------- BAT FILE TEXT START --------------------------");
            foreach (string str in cntrlines)
            {
                LogMessageToFile(str);
            }
            LogMessageToFile("----------------------- BAT FILE TEXT END ----------------------------");
        }

        public static void WriteIntoCacheXML(string[] cntrlines,string Path)
        {
            System.IO.StreamWriter sw = null;
            foreach (string str in cntrlines)
            {
                try
                {
                    sw = System.IO.File.AppendText(Path);
                    string logLine = System.String.Format(str);
                    sw.WriteLine(logLine);
                }
                finally
                {
                    sw.Close();
                }
            }
        }
    }
}