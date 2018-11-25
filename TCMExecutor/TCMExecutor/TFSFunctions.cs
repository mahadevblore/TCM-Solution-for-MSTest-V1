using Microsoft.TeamFoundation;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Server;
using Microsoft.TeamFoundation.TestManagement.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TCMExecutor
{
    public struct TCMParameters
    {
        public string suiteId;
        public string planId;
        public string configId;
        public string buildPath;
        public string collection;
        public string projectName;
        public string settingsName;
        public string testCaseId;
        public string definition;
        public string environment;
        public string queryText;
        public string runTitle;
    }

    public class TFSFunctions
    {
        public static TfsTeamProjectCollection tfs = null;
        public static ITestManagementService tms = null;
        public static ITestManagementTeamProject proj = null;
        public static ITestPlanCollection iTPC = null;
        public static ITestConfiguration testConfiguration = null;
        public static ITestConfigurationCollection iTConfigColl = null;
        public static IBuildServer iBServer = null;
        public static ITestPlan iTPlan = null;

        public static ProjectInfo[] GetProjects(string collectionName)
        {
                var tfsSvrFac = TeamFoundationServerFactory.GetServer(XML.GetCollectionURL(collectionName));
                var projectCollection = tfsSvrFac.GetService<ICommonStructureService>();
                return projectCollection.ListAllProjects();
        }

        public static SortedDictionary<string,string> GetTestPlans(string collectionName, string projectName)
        {
             tfs = new TfsTeamProjectCollection(TfsTeamProjectCollection.GetFullyQualifiedUriForName(XML.GetCollectionURL(collectionName)));
             tms = tfs.GetService<ITestManagementService>();
             proj = tms.GetTeamProject(projectName);
             iTPC = proj.TestPlans.Query("Select * From TestPlan");
             SortedDictionary<string, string> iTP = new SortedDictionary<string, string>();
             foreach (ITestPlan itp in iTPC)
             {
                 iTP.Add(itp.Id.ToString(),itp.Name);
             }
             return iTP;
        }

        public static SortedDictionary<string, string> GetConfigurations()
        {
            SortedDictionary<string, string> lsConfigColl = new SortedDictionary<string, string>();
            try
            {
                iTConfigColl = proj.TestConfigurations.Query("Select * from TestConfiguration");
                foreach (ITestConfiguration iTConfig in iTConfigColl)
                {
                    lsConfigColl.Add(iTConfig.Name, iTConfig.Id.ToString());
                }
            }
            catch (Exception ex)
            {
                lsConfigColl.Add("Please Load the Project First", "-1");
            }           
            return lsConfigColl;
        }

        public static SortedDictionary<string, string> GetTestSettings()
        {
            SortedDictionary<string, string> lsSettingsColl = new SortedDictionary<string, string>();
            try
            {
                ITestSettingsHelper iTSH = proj.TestSettings;
                foreach (ITestSettings iTS in iTSH.Query("Select * from TestSettings"))
                {
                    lsSettingsColl.Add(iTS.Name, iTS.Id.ToString());
                }
            }
            catch (Exception ex)
            {
                lsSettingsColl.Add("Please Load the Project First", "-1");
            }
            return lsSettingsColl;
        }

        public static SortedDictionary<string, string> GetBuildDetails()
        {
            SortedDictionary<string, string> lsBuildsColl = new SortedDictionary<string, string>();
            try
            {
                iBServer =(IBuildServer)tfs.GetService(typeof(IBuildServer));
                List<IBuildDefinition> defList = new List<IBuildDefinition>(iBServer.QueryBuildDefinitions(proj.TeamProjectName));
                foreach (IBuildDefinition iBDef in defList)
                {
                    lsBuildsColl.Add(iBDef.Name, iBDef.Id);
                }
            }
            catch (Exception ex)
            {
                lsBuildsColl.Add("Select the project", "-1");
            }
            return lsBuildsColl;
        }

        public static SortedDictionary<string, string> GetBuilds(string projName,string defName)
        {
            SortedDictionary<string, string> lsBuildsQuery = new SortedDictionary<string, string>();
            try
            {

                IBuildServer buildServer = (IBuildServer)tfs.GetService(typeof(IBuildServer));
                ILinking iLinkingService = tfs.GetService<ILinking>(); //can be used later to get build URL link
                
                //Specify query
                IBuildDetailSpec spec = buildServer.CreateBuildDetailSpec(projName.Trim(),defName.Trim());
                spec.InformationTypes = null; // for speed improvement
                spec.MinFinishTime = DateTime.Now.AddDays(-21); //to get only builds of last 2 weeks
                //spec.MaxBuildsPerDefinition = 1; //get only one build per build definintion
                spec.QueryOrder = BuildQueryOrder.FinishTimeDescending; //get the latest build only
                spec.QueryOptions = QueryOptions.All;

                IBuildDetail[] bDetails = buildServer.QueryBuilds(spec).Builds;
                List<IBuildDetail> ibDetails = bDetails.OrderBy(x => x.CompilationStatus == BuildPhaseStatus.Succeeded).OrderByDescending(x => x.StartTime).Take(10).ToList();
                foreach (IBuildDetail ibdet in ibDetails)
                {
                    lsBuildsQuery.Add(ibdet.BuildNumber, ibdet.Uri.Port.ToString());
                }
            }
            catch (Exception ex)
            {
                lsBuildsQuery.Add("Please Load the definition first", "-1");
            }
            return lsBuildsQuery;
        }

        public static SortedDictionary<string, string> GetAllSuites(string planId)
        {
            SortedDictionary<string, string> lsSuitesQuery = new SortedDictionary<string, string>();

            try
            {
                ITestSuiteHelper iTSuiteHelp = proj.TestSuites;
                string qText = string.Format("Select * from TestSuite where PlanId='{0}'", planId);
                ITestSuiteCollection iTSuiteColl = iTSuiteHelp.Query(qText);
                foreach (ITestSuiteBase iTSB in iTSuiteColl)
                {
                    lsSuitesQuery.Add(iTSB.Id.ToString(), iTSB.Title);
                }
            }
            catch (Exception ex)
            {
                lsSuitesQuery.Add("-1", "Error. Load Project,pland and config");
            }
            return lsSuitesQuery;
        }

        public static string GetBuildPath(string buildTitle,string projName,string defName)
        {
             IBuildServer buildServer = (IBuildServer)tfs.GetService(typeof(IBuildServer));                
            //Specify query
            IBuildDetailSpec spec = buildServer.CreateBuildDetailSpec(projName.Trim(),defName.Trim());
            spec.InformationTypes = null; // for speed improvement
            spec.MinFinishTime = DateTime.Now.AddDays(-21); //to get only builds of last 2 weeks
            //spec.MaxBuildsPerDefinition = 1; //get only one build per build definintion
            spec.QueryOrder = BuildQueryOrder.FinishTimeDescending; //get the latest build only
            spec.QueryOptions = QueryOptions.All;

            IBuildDetail bDetail = buildServer.QueryBuilds(spec).Builds.Where(x=>x.DropLocation.Contains(buildTitle)).First();
            return bDetail.DropLocation;
        }

        public static string GetRecentBuildPath(string Path)
        {
            DirectoryInfo dInforTemp = new DirectoryInfo(Path).GetDirectories()
                           .OrderByDescending(d => d.LastWriteTimeUtc).First();
            return dInforTemp.FullName;
        }

        public static SortedDictionary<string, string> GetAllTestCases(string suites,string planId)
        { 
            SortedDictionary<string, string> lsTestCasesQuery = new SortedDictionary<string, string>();
            
            try
            {
                ITestSuiteHelper iTSuiteHelp = proj.TestSuites;
                string[] suiteArray = suites.Split(',');
                foreach (string str in suiteArray)
                {
                    ITestSuiteBase iTSB = iTSuiteHelp.Find(Convert.ToInt32(str));
                    ITestCaseCollection itCColl = iTSB.AllTestCases;
                    
                    foreach (ITestCase iTc in itCColl)
                    {
                        lsTestCasesQuery.Add(iTc.Id.ToString(), iTc.Title.ToString());
                        
                    }
                }
            }
            catch (Exception ex)
            {
                lsTestCasesQuery.Add("-1", "Error. Load Project,pland and config");
            }
            return lsTestCasesQuery;
        }
    }
}
