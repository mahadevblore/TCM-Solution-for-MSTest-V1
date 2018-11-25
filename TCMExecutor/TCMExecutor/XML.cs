using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TCMExecutor
{
    public class XML
    {
        public static string[] ReadConfigFile()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Program.exePath + "\\" + "EnvVariables.xml");
            XmlNode node = doc.SelectSingleNode(@"//TFSCollection");
            XmlElement element = (XmlElement)node;
           return element.InnerText.Split(',');
        }

        public static string GetCollectionURL(string collectionName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Program.exePath + "\\" + "ProjectConstants.xml");
            XmlNode node = doc.SelectSingleNode(@"//Main/CollectionURL");
            XmlElement element = (XmlElement)node;
            return element.GetAttribute(collectionName.Trim());
        }

        public static string GetBuildDefinition()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Program.exePath + "\\" + "ProjectConstants.xml");
            XmlNode node = doc.SelectSingleNode(@"//Main/BuildDefinition");
            XmlElement element = (XmlElement)node;
            return element.GetAttribute("Definition");
        }

        public static void SetCacheMemory(string session,string value)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Program.cacheXmlPath);
            XmlNode node = doc.SelectSingleNode(@"//Cache/Sessions");
            value = value.Trim();
            XmlElement Element = (XmlElement)node;
            switch (session)
            {
                case "TFSCollection":
                    Element.RemoveAttribute("TFSCollection");
                    Element.SetAttribute("TFSCollection", value);
                    break;
                case "TFSProject":
                    Element.RemoveAttribute("TFSProject");
                    Element.SetAttribute("TFSProject", value);
                    break;
                case "TFSPlan":
                    Element.RemoveAttribute("TFSPlan");
                      Element.SetAttribute("TFSPlan", value);
                    break;
                case "TFSDefinition":
                    Element.RemoveAttribute("TFSDefinition");
                    Element.SetAttribute("TFSDefinition", value);
                    break;
                case "TFSConfiguration":
                    Element.RemoveAttribute("TFSConfiguration");
                    Element.SetAttribute("TFSConfiguration", value);
                    break;
                case "TFSSetting":
                     Element.RemoveAttribute("TFSSetting");
                     Element.SetAttribute("TFSSetting", value);
                    break;
                case "TFSBuild":
                    Element.RemoveAttribute("TFSBuild");
                    Element.SetAttribute("TFSBuild", value);
                    break;
                case "TFSSuites":
                    Element.RemoveAttribute("TFSSuites");
                    Element.SetAttribute("TFSSuites", value);
                    break;
                case "TFSTestCases":
                    Element.RemoveAttribute("TFSTestCases");
                    Element.SetAttribute("TFSTestCases", value);
                    break;
                default:
                    break;
            }
            doc.DocumentElement.AppendChild(Element);
            doc.Save(Program.cacheXmlPath);
        }

        public static Dictionary<string, KeyValuePair<string, string>> GetCacheMemory()
        {
            Dictionary<string, KeyValuePair<string, string>> cacheStrArr = new Dictionary<string, KeyValuePair<string, string>>();
            XmlDocument doc = new XmlDocument();
            doc.Load(Program.cacheXmlPath);
            XmlNode node = doc.SelectSingleNode(@"//Cache/Sessions");
            XmlElement element = (XmlElement)node;
            if (!string.IsNullOrWhiteSpace(element.GetAttribute("TFSCollection")))
            {
                Dictionary<string, string> tempDict1 = new Dictionary<string, string>();
                tempDict1.Add(element.GetAttribute("TFSCollection"), "1");
                cacheStrArr.Add("TFSCollection", tempDict1.First());
            }
            if (!string.IsNullOrWhiteSpace(element.GetAttribute("TFSProject")))
            {
                Dictionary<string, string> tempDict2 = new Dictionary<string, string>();
                tempDict2.Add(element.GetAttribute("TFSProject"), "2");
                cacheStrArr.Add("TFSProject", tempDict2.First());
            }
            if (!string.IsNullOrWhiteSpace(element.GetAttribute("TFSPlan")))
            {
                cacheStrArr.Add("TFSPlan", Utils.ConvertStrToDict(element.GetAttribute("TFSPlan")).First());
            }
            if (!string.IsNullOrWhiteSpace(element.GetAttribute("TFSDefinition")))
            {
                cacheStrArr.Add("TFSDefinition", Utils.ConvertStrToDict(element.GetAttribute("TFSDefinition")).First());
            }
            if (!string.IsNullOrWhiteSpace(element.GetAttribute("TFSConfiguration")))
            {
                cacheStrArr.Add("TFSConfiguration", Utils.ConvertStrToDict(element.GetAttribute("TFSConfiguration")).First());
            }
            if (!string.IsNullOrWhiteSpace(element.GetAttribute("TFSSetting")))
            {
                cacheStrArr.Add("TFSSetting", Utils.ConvertStrToDict(element.GetAttribute("TFSSetting")).First());
            }
            if (!string.IsNullOrWhiteSpace(element.GetAttribute("TFSBuild")))
            {
                cacheStrArr.Add("TFSBuild", Utils.ConvertStrToDict(element.GetAttribute("TFSBuild")).First());
            }
            if (!string.IsNullOrWhiteSpace(element.GetAttribute("TFSSuites")))
            {
                Dictionary<string, string> tempDict4 = new Dictionary<string, string>();
                tempDict4.Add(element.GetAttribute("TFSSuites"), "4");
                cacheStrArr.Add("TFSSuites", tempDict4.First());
            }
            if (!string.IsNullOrWhiteSpace(element.GetAttribute("TFSTestCases")))
            {
                Dictionary<string, string> tempDict5 = new Dictionary<string, string>();
                tempDict5.Add(element.GetAttribute("TFSTestCases"), "5");
                cacheStrArr.Add("TFSTestCases", tempDict5.First());
            }
            return cacheStrArr;
        }

        public static string[] EnterIntoCacheXML()
        {
            return new string[] { "<?xml version=" +"\""+ "1.0" +"\""+ " encoding=" +"\""+ "utf-8" +"\""+ " ?>", "<Cache>", "<Sessions>", "</Sessions>","</Cache>" };
        }

        public static void ClearCacheMemory()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Program.cacheXmlPath);
                XmlNode node = doc.SelectSingleNode(@"//Cache/Sessions");
                XmlElement Element = (XmlElement)node;               
                Element.RemoveAttribute("TFSCollection");
                Element.RemoveAttribute("TFSProject");                                   
                Element.RemoveAttribute("TFSPlan");                                          
                Element.RemoveAttribute("TFSDefinition");                       
                Element.RemoveAttribute("TFSConfiguration");
                Element.RemoveAttribute("TFSSetting");                       
                Element.RemoveAttribute("TFSBuild");                       
                Element.RemoveAttribute("TFSSuites");                       
                Element.RemoveAttribute("TFSTestCases");                                 
                doc.DocumentElement.AppendChild(Element);
                doc.Save(Program.cacheXmlPath);
            }
            catch { }
        }
    }
}
