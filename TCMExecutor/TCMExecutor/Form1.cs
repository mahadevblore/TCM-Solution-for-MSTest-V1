using Microsoft.TeamFoundation.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCMExecutor
{
    public partial class TCMExecutor : Form
    {
        TELauncher tel;
        SuiteTable sTable;
        TestCaseTable tTable;
        public TCMExecutor(TELauncher newTel)
        {
            InitializeComponent();
            if(!Directory.Exists(Program.tempPath + "\\" + Program.cacheFolder) || !File.Exists(Program.cacheXmlPath))
            {
                string cachePath = Program.tempPath + "\\" + Program.cacheFolder;
                Directory.CreateDirectory(Program.tempPath + "\\" + Program.cacheFolder);
                FileInfo fi = new FileInfo(Program.cacheXmlPath);
                FileStream fStream = fi.Create();
                fStream.Close();
                Logger.WriteIntoCacheXML(XML.EnterIntoCacheXML(), Program.cacheXmlPath);
            }
            if (Program.isCached == false)
            {
                ClearCache.Hide();
            }
            tel = newTel;
            tel.TeLWait.Show();
            tel.Text = "please wait while the app loads ...";
        }

        private void TCMExecutor_Load(object sender, EventArgs e)
        {
            DateT.Text = DateTime.Now.ToString();
            if (Program.isCached)
            {
                Dictionary<string, KeyValuePair<string, string>> cacheDict = XML.GetCacheMemory();
                
                try
                {
                    string col = cacheDict["TFSCollection"].Key;
                    TFSColls_DropDown(sender, e);
                    TFSColls.SelectedItem = col;
                }
                catch { }
                
                try
                {
                    string proj = cacheDict["TFSProject"].Key;
                    TFSProj_DropDown(sender, e);
                    TFSProj.SelectedItem = proj;
                }
                catch { }
                
                try
                {
                    TFSPln_DropDown(sender, e);
                    int pln = TFSPln.Items.IndexOf(cacheDict["TFSPlan"]);
                    TFSPln.SelectedIndex = pln;
                }
                catch { }

                try
                {
                    TFSDefs_DropDown(sender, e);
                    int def = TFSDefs.Items.IndexOf(cacheDict["TFSDefinition"]);
                    TFSDefs.SelectedIndex = def;
                }
                catch { }
               
                try
                {
                    ConfigID_DropDown(sender, e);
                    int config = ConfigID.Items.IndexOf(cacheDict["TFSConfiguration"]);
                    ConfigID.SelectedIndex = config;
                }
                catch { }
              
                try
                {
                    TestStngs_DropDown(sender, e);
                    int setng = TestStngs.Items.IndexOf(cacheDict["TFSSetting"]);
                    TestStngs.SelectedIndex = setng;
                }
                catch { }
                
                try
                {
                    Builds_DropDown(sender, e);
                    int bld = Builds.Items.IndexOf(cacheDict["TFSBuild"]);
                    Builds.SelectedIndex = bld;
                }
                catch { }

                try
                {
                    SuiteTextBox.Text = cacheDict["TFSSuites"].Key;
                }
                catch { }

                try
                {
                    TestCaseTextBox.Text = cacheDict["TFSTestCases"].Key;
                }
                catch { }
            }
            tel.Hide();
            if (!Directory.Exists(Logger.tempPath + Program.tmp))
            {
                Directory.CreateDirectory(Logger.tempPath + Program.tmp);
            }
        }

        private void TFSColls_DropDown(object sender, EventArgs e)
        {
            TFSColls.Items.Clear();
            TFSColls.Items.AddRange(XML.ReadConfigFile());
        }

        private void TFSProj_DropDown(object sender, EventArgs e)
        {
            TFSProj.Items.Clear();
            TFSProj.Text = "";
            if (TFSColls.SelectedItem == null)
            {
                TFSProj.Items.Add("Select Collection");
            }
            else
            {
                List<ProjectInfo> lPInfo = TFSFunctions.GetProjects(TFSColls.SelectedItem.ToString().Trim()).ToList();
                foreach (ProjectInfo prjInfo in lPInfo)
                {
                    TFSProj.Items.Add(prjInfo.Name);
                }
            }
        }

        private void TFSPln_DropDown(object sender, EventArgs e)
        {
            TFSPln.DataBindings.Clear();
            TFSPln.Text = "";
            if (TFSColls.SelectedItem == null || TFSProj.SelectedItem == null)
            {
                TFSPln.Text = "Select Collection/Project";
            }
            else
            {
                TFSPln.DataSource = new BindingSource(TFSFunctions.GetTestPlans(TFSColls.SelectedItem.ToString(),
                    TFSProj.SelectedItem.ToString()), null);
                TFSPln.DisplayMember = "Value";
                TFSPln.ValueMember = "Key";
            }
        }

        private void ConfigID_DropDown(object sender, EventArgs e)
        {
            ConfigID.DataBindings.Clear();
            if (TFSProj.SelectedItem == null || TFSProj.SelectedItem.ToString().Contains("Collection"))
            {
                ConfigID.Text = "Select the Project first";
            }
            else
            {
                ConfigID.DataSource = null;
                ConfigID.Text = "";
                SortedDictionary<string, string> Configs = TFSFunctions.GetConfigurations();
                ConfigID.DataSource = new BindingSource(Configs, null);
                ConfigID.DisplayMember = "Key";
                ConfigID.ValueMember = "Value";
            }
        }

        private void TestStngs_DropDown(object sender, EventArgs e)
        {
            TestStngs.DataBindings.Clear();
            TestStngs.Text = "";
            SortedDictionary<string, string> tSettings = TFSFunctions.GetTestSettings();
            TestStngs.DataSource = new BindingSource(tSettings, null);
            TestStngs.DisplayMember = "Key";
            TestStngs.ValueMember = "Value";
        }

        private void TFSDefs_DropDown(object sender, EventArgs e)
        {
            TFSDefs.DataBindings.Clear();
            TFSDefs.Text = "";
            SortedDictionary<string, string> iBuilds = TFSFunctions.GetBuildDetails();
            TFSDefs.DataSource = new BindingSource(iBuilds, null);
            TFSDefs.DisplayMember = "Key";
            TFSDefs.ValueMember = "Value";
        }

        private void Builds_DropDown(object sender, EventArgs e)
        {
           Builds.DataBindings.Clear();
           Builds.Text = "";
           KeyValuePair<string, string> tmpDict = (KeyValuePair<string, string>)TFSDefs.SelectedItem;
           SortedDictionary<string, string> iBuilds = TFSFunctions.GetBuilds(TFSProj.SelectedItem.ToString(),tmpDict.Key);
           Builds.DataSource = new BindingSource(iBuilds, null);
           Builds.DisplayMember = "Key";
           Builds.ValueMember = "Value";
          
        }

        private void TFSColls_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Program.isCached)
            {
                XML.SetCacheMemory("TFSCollection", TFSColls.SelectedItem.ToString());
            }
        }

        private void TFSProj_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Program.isCached)
            {
                XML.SetCacheMemory("TFSProject", TFSProj.SelectedItem.ToString());
            }
        }

        private void TFSPln_SelectedIndexChanged(object sender, EventArgs e)
        { if (Program.isCached)
            {              
            XML.SetCacheMemory("TFSPlan", TFSPln.SelectedItem.ToString());
        }
        }

        private void TFSDefs_SelectedIndexChanged(object sender, EventArgs e)
        {
             if (Program.isCached)
            {              
                XML.SetCacheMemory("TFSDefinition", TFSDefs.SelectedItem.ToString());
             }
        }

        private void ConfigID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Program.isCached)
            {
                XML.SetCacheMemory("TFSConfiguration", ConfigID.SelectedItem.ToString());
            }
        }

        private void SuiteTextBox_TextChanged(object sender, EventArgs e)
        {
            if (Program.isCached)
            {
                XML.SetCacheMemory("TFSSuites", SuiteTextBox.Text.ToString());
            }
        }

        private void TestCaseTextBox_TextChanged(object sender, EventArgs e)
        {
            if (Program.isCached)
            {
                XML.SetCacheMemory("TFSTestCases", TestCaseTextBox.Text.ToString());
            }
        }

        private void TestStngs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Program.isCached)
            {
                XML.SetCacheMemory("TFSSetting", TestStngs.SelectedItem.ToString());
            }
        }

        private void Builds_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Program.isCached)
            {
                XML.SetCacheMemory("TFSBuild", Builds.SelectedItem.ToString());
            }
        }

        private void ClearCache_Click(object sender, EventArgs e)
        {
            XML.ClearCacheMemory();
            MessageBox.Show("Cleared Cache Memory!");
        }

        private void SuiteViewer_Click(object sender, EventArgs e)
        {
            try
            {
                KeyValuePair<string, string> tmpDict1 = (KeyValuePair<string, string>)TFSPln.SelectedItem;
                //KeyValuePair<string, int> tmpDict2 = (KeyValuePair<string, int>)ConfigID.SelectedItem;
                SortedDictionary<string, string> sViewer = new SortedDictionary<string, string>();
                sViewer = TFSFunctions.GetAllSuites(tmpDict1.Key.ToString());//, tmpDict2.Value.ToString()

                sTable = new SuiteTable(sViewer, this);

                sTable.ShowDialog();
                SuiteTextBox.Text = sTable.MyString;
            }
            catch
            {
                MessageBox.Show("Please select mandatory values. Check for the combination of values.");
            }
        }

        private void TestCaseViewer_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SuiteTextBox.Text))
            {
                MessageBox.Show("Please select Suites from where the Test Case is referred. " +
                "Suite ID is neccessary to execute the test case and report to MTM accordingly");
            }
            else
            {
                KeyValuePair<string, string> tmpDict2 = (KeyValuePair<string, string>)TFSPln.SelectedItem;
                string suiteTB = SuiteTextBox.Text;

                SortedDictionary<string, string> tViewer = new SortedDictionary<string, string>();
                tViewer = TFSFunctions.GetAllTestCases(suiteTB, tmpDict2.Key.ToString());

                tTable = new TestCaseTable(tViewer, this);

                tTable.ShowDialog();

                TestCaseTextBox.Text = tTable.MyString;
            }
        }

        private void TCMExecutor_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void ExecuteSuite_Click(object sender, EventArgs e)
        {
            TCMParameters tcmParams = new TCMParameters();
            tcmParams.projectName = TFSProj.SelectedItem.ToString().Trim();
            tcmParams.definition = GetKeyFromKeyValuePair(TFSDefs);
            tcmParams.collection = XML.GetCollectionURL(TFSColls.SelectedItem.ToString().Trim());
            tcmParams.configId = GetValueFromKeyValuePair(ConfigID);
            tcmParams.buildPath = TFSFunctions.GetBuildPath(GetKeyFromKeyValuePair(Builds), tcmParams.projectName, tcmParams.definition);
            tcmParams.suiteId = SuiteTextBox.Text.ToString().Trim();
            tcmParams.planId = GetKeyFromKeyValuePair(TFSPln);
            tcmParams.runTitle = "TCM Executor Suite/s Run " + DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss tt");
            TCMFunctions.TCMSuiteExecutionCreateAndExecute(tcmParams);
        }

        private void ExecTestCase_Click(object sender, EventArgs e)
        {
            TCMParameters tcmParams = new TCMParameters();
            tcmParams.projectName = TFSProj.SelectedItem.ToString().Trim();
            tcmParams.definition = GetKeyFromKeyValuePair(TFSDefs);
            tcmParams.collection = XML.GetCollectionURL(TFSColls.SelectedItem.ToString().Trim());
            tcmParams.configId = GetValueFromKeyValuePair(ConfigID);
            tcmParams.buildPath = TFSFunctions.GetBuildPath(GetKeyFromKeyValuePair(Builds), tcmParams.projectName, tcmParams.definition);
            tcmParams.suiteId = SuiteTextBox.Text.ToString().Trim();
            tcmParams.testCaseId = TestCaseTextBox.Text.ToString().Trim();
            tcmParams.planId = GetKeyFromKeyValuePair(TFSPln);
            tcmParams.runTitle = "TCM Executor TestCase/s Run " + DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss tt");
            TCMFunctions.TCMTestExecutionExecutionCreateAndExecute(tcmParams);
        }

        private string GetBuildLocation()
        {
            if (string.IsNullOrWhiteSpace(BuildPath.Text))
            {
                return Builds.Text;
            }
            else
            {
                return BuildPath.Text;
            }
        }

        private string GetValueFromKeyValuePair(ComboBox cbx)
        {
            KeyValuePair<string, string> tmpKvp = (KeyValuePair<string, string>)cbx.SelectedItem;
            return tmpKvp.Value;
        }

        private string GetKeyFromKeyValuePair(ComboBox cbx)
        {
            KeyValuePair<string, string> tmpKvp = (KeyValuePair<string, string>)cbx.SelectedItem;
            return tmpKvp.Key;
        }

    }
}
