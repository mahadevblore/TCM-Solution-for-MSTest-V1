namespace TCMExecutor
{
    partial class TCMExecutor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TFSColls = new System.Windows.Forms.ComboBox();
            this.TFSCollection = new System.Windows.Forms.Label();
            this.DateT = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TFSProject = new System.Windows.Forms.Label();
            this.TFSProj = new System.Windows.Forms.ComboBox();
            this.TFSPlan = new System.Windows.Forms.Label();
            this.TFSPln = new System.Windows.Forms.ComboBox();
            this.TestSetting = new System.Windows.Forms.Label();
            this.Error = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.TestStngs = new System.Windows.Forms.ComboBox();
            this.ExecTestCase = new System.Windows.Forms.Button();
            this.ExecuteSuite = new System.Windows.Forms.Button();
            this.TestCaseTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuiteTextBox = new System.Windows.Forms.TextBox();
            this.SuitesID = new System.Windows.Forms.Label();
            this.ConfigID = new System.Windows.Forms.ComboBox();
            this.ConfigurationID = new System.Windows.Forms.Label();
            this.TFSDefs = new System.Windows.Forms.ComboBox();
            this.TFSDefinition = new System.Windows.Forms.Label();
            this.Builds = new System.Windows.Forms.ComboBox();
            this.CompiledBuilds = new System.Windows.Forms.Label();
            this.SuiteViewer = new System.Windows.Forms.Button();
            this.TestCaseViewer = new System.Windows.Forms.Button();
            this.BuildPath = new System.Windows.Forms.TextBox();
            this.BuildLocation = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.ClearCache = new System.Windows.Forms.Button();
            this.SuiteExecPB = new System.Windows.Forms.ProgressBar();
            this.TestCaseExexPB = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // TFSColls
            // 
            this.TFSColls.FormattingEnabled = true;
            this.TFSColls.Location = new System.Drawing.Point(186, 61);
            this.TFSColls.Name = "TFSColls";
            this.TFSColls.Size = new System.Drawing.Size(121, 21);
            this.TFSColls.TabIndex = 0;
            this.TFSColls.DropDown += new System.EventHandler(this.TFSColls_DropDown);
            this.TFSColls.SelectionChangeCommitted += new System.EventHandler(this.TFSColls_SelectedIndexChanged);
            // 
            // TFSCollection
            // 
            this.TFSCollection.AutoSize = true;
            this.TFSCollection.ForeColor = System.Drawing.Color.Red;
            this.TFSCollection.Location = new System.Drawing.Point(90, 64);
            this.TFSCollection.Name = "TFSCollection";
            this.TFSCollection.Size = new System.Drawing.Size(76, 13);
            this.TFSCollection.TabIndex = 1;
            this.TFSCollection.Text = "TFS Collection";
            // 
            // DateT
            // 
            this.DateT.AutoSize = true;
            this.DateT.Location = new System.Drawing.Point(654, 18);
            this.DateT.Name = "DateT";
            this.DateT.Size = new System.Drawing.Size(53, 13);
            this.DateT.TabIndex = 2;
            this.DateT.Text = "DateTime";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(87, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(147, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Test Case Execution Program";
            // 
            // TFSProject
            // 
            this.TFSProject.AutoSize = true;
            this.TFSProject.ForeColor = System.Drawing.Color.Red;
            this.TFSProject.Location = new System.Drawing.Point(90, 108);
            this.TFSProject.Name = "TFSProject";
            this.TFSProject.Size = new System.Drawing.Size(63, 13);
            this.TFSProject.TabIndex = 4;
            this.TFSProject.Text = "TFS Project";
            // 
            // TFSProj
            // 
            this.TFSProj.FormattingEnabled = true;
            this.TFSProj.Location = new System.Drawing.Point(186, 105);
            this.TFSProj.Name = "TFSProj";
            this.TFSProj.Size = new System.Drawing.Size(300, 21);
            this.TFSProj.TabIndex = 5;
            this.TFSProj.DropDown += new System.EventHandler(this.TFSProj_DropDown);
            this.TFSProj.SelectionChangeCommitted += new System.EventHandler(this.TFSProj_SelectedIndexChanged);
            // 
            // TFSPlan
            // 
            this.TFSPlan.AutoSize = true;
            this.TFSPlan.ForeColor = System.Drawing.Color.Red;
            this.TFSPlan.Location = new System.Drawing.Point(89, 149);
            this.TFSPlan.Name = "TFSPlan";
            this.TFSPlan.Size = new System.Drawing.Size(51, 13);
            this.TFSPlan.TabIndex = 7;
            this.TFSPlan.Text = "TFS Plan";
            // 
            // TFSPln
            // 
            this.TFSPln.FormattingEnabled = true;
            this.TFSPln.Location = new System.Drawing.Point(185, 146);
            this.TFSPln.Name = "TFSPln";
            this.TFSPln.Size = new System.Drawing.Size(300, 21);
            this.TFSPln.TabIndex = 6;
            this.TFSPln.DropDown += new System.EventHandler(this.TFSPln_DropDown);
            this.TFSPln.SelectionChangeCommitted += new System.EventHandler(this.TFSPln_SelectedIndexChanged);
            // 
            // TestSetting
            // 
            this.TestSetting.AutoSize = true;
            this.TestSetting.ForeColor = System.Drawing.Color.Blue;
            this.TestSetting.Location = new System.Drawing.Point(514, 130);
            this.TestSetting.Name = "TestSetting";
            this.TestSetting.Size = new System.Drawing.Size(64, 13);
            this.TestSetting.TabIndex = 17;
            this.TestSetting.Text = "Test Setting";
            // 
            // Error
            // 
            this.Error.AutoSize = true;
            this.Error.Location = new System.Drawing.Point(93, 432);
            this.Error.Name = "Error";
            this.Error.Size = new System.Drawing.Size(66, 13);
            this.Error.TabIndex = 22;
            this.Error.Text = "Error/Output";
            this.Error.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(96, 448);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(390, 147);
            this.richTextBox1.TabIndex = 23;
            this.richTextBox1.Text = "";
            // 
            // TestStngs
            // 
            this.TestStngs.FormattingEnabled = true;
            this.TestStngs.Location = new System.Drawing.Point(517, 146);
            this.TestStngs.Name = "TestStngs";
            this.TestStngs.Size = new System.Drawing.Size(358, 21);
            this.TestStngs.TabIndex = 24;
            this.TestStngs.DropDown += new System.EventHandler(this.TestStngs_DropDown);
            this.TestStngs.SelectionChangeCommitted += new System.EventHandler(this.TestStngs_SelectedIndexChanged);
            // 
            // ExecTestCase
            // 
            this.ExecTestCase.Location = new System.Drawing.Point(500, 341);
            this.ExecTestCase.Name = "ExecTestCase";
            this.ExecTestCase.Size = new System.Drawing.Size(85, 23);
            this.ExecTestCase.TabIndex = 30;
            this.ExecTestCase.Text = "ExecTestCase";
            this.ExecTestCase.UseVisualStyleBackColor = true;
            this.ExecTestCase.Click += new System.EventHandler(this.ExecTestCase_Click);
            // 
            // ExecuteSuite
            // 
            this.ExecuteSuite.Location = new System.Drawing.Point(500, 272);
            this.ExecuteSuite.Name = "ExecuteSuite";
            this.ExecuteSuite.Size = new System.Drawing.Size(85, 23);
            this.ExecuteSuite.TabIndex = 29;
            this.ExecuteSuite.Text = "ExecuteSuite";
            this.ExecuteSuite.UseVisualStyleBackColor = true;
            this.ExecuteSuite.Click += new System.EventHandler(this.ExecuteSuite_Click);
            // 
            // TestCaseTextBox
            // 
            this.TestCaseTextBox.Location = new System.Drawing.Point(185, 343);
            this.TestCaseTextBox.Name = "TestCaseTextBox";
            this.TestCaseTextBox.Size = new System.Drawing.Size(301, 20);
            this.TestCaseTextBox.TabIndex = 28;
            this.TestCaseTextBox.TextChanged += new System.EventHandler(this.TestCaseTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(89, 346);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "TestCases ID";
            // 
            // SuiteTextBox
            // 
            this.SuiteTextBox.Location = new System.Drawing.Point(185, 274);
            this.SuiteTextBox.Name = "SuiteTextBox";
            this.SuiteTextBox.Size = new System.Drawing.Size(301, 20);
            this.SuiteTextBox.TabIndex = 26;
            this.SuiteTextBox.TextChanged += new System.EventHandler(this.SuiteTextBox_TextChanged);
            // 
            // SuitesID
            // 
            this.SuitesID.AutoSize = true;
            this.SuitesID.ForeColor = System.Drawing.Color.Red;
            this.SuitesID.Location = new System.Drawing.Point(90, 277);
            this.SuitesID.Name = "SuitesID";
            this.SuitesID.Size = new System.Drawing.Size(50, 13);
            this.SuitesID.TabIndex = 25;
            this.SuitesID.Text = "Suites ID";
            // 
            // ConfigID
            // 
            this.ConfigID.FormattingEnabled = true;
            this.ConfigID.Location = new System.Drawing.Point(186, 230);
            this.ConfigID.Name = "ConfigID";
            this.ConfigID.Size = new System.Drawing.Size(300, 21);
            this.ConfigID.TabIndex = 32;
            this.ConfigID.DropDown += new System.EventHandler(this.ConfigID_DropDown);
            this.ConfigID.SelectionChangeCommitted += new System.EventHandler(this.ConfigID_SelectedIndexChanged);
            // 
            // ConfigurationID
            // 
            this.ConfigurationID.AutoSize = true;
            this.ConfigurationID.ForeColor = System.Drawing.Color.Red;
            this.ConfigurationID.Location = new System.Drawing.Point(90, 233);
            this.ConfigurationID.Name = "ConfigurationID";
            this.ConfigurationID.Size = new System.Drawing.Size(69, 13);
            this.ConfigurationID.TabIndex = 31;
            this.ConfigurationID.Text = "Configuration";
            // 
            // TFSDefs
            // 
            this.TFSDefs.FormattingEnabled = true;
            this.TFSDefs.Location = new System.Drawing.Point(185, 187);
            this.TFSDefs.Name = "TFSDefs";
            this.TFSDefs.Size = new System.Drawing.Size(301, 21);
            this.TFSDefs.TabIndex = 34;
            this.TFSDefs.DropDown += new System.EventHandler(this.TFSDefs_DropDown);
            this.TFSDefs.SelectionChangeCommitted += new System.EventHandler(this.TFSDefs_SelectedIndexChanged);
            // 
            // TFSDefinition
            // 
            this.TFSDefinition.AutoSize = true;
            this.TFSDefinition.CausesValidation = false;
            this.TFSDefinition.ForeColor = System.Drawing.Color.Red;
            this.TFSDefinition.Location = new System.Drawing.Point(90, 190);
            this.TFSDefinition.Name = "TFSDefinition";
            this.TFSDefinition.Size = new System.Drawing.Size(74, 13);
            this.TFSDefinition.TabIndex = 33;
            this.TFSDefinition.Text = "TFS Definition";
            // 
            // Builds
            // 
            this.Builds.FormattingEnabled = true;
            this.Builds.Location = new System.Drawing.Point(517, 192);
            this.Builds.Name = "Builds";
            this.Builds.Size = new System.Drawing.Size(358, 21);
            this.Builds.TabIndex = 36;
            this.Builds.DropDown += new System.EventHandler(this.Builds_DropDown);
            this.Builds.SelectionChangeCommitted += new System.EventHandler(this.Builds_SelectedIndexChanged);
            // 
            // CompiledBuilds
            // 
            this.CompiledBuilds.AutoSize = true;
            this.CompiledBuilds.ForeColor = System.Drawing.Color.Blue;
            this.CompiledBuilds.Location = new System.Drawing.Point(514, 176);
            this.CompiledBuilds.Name = "CompiledBuilds";
            this.CompiledBuilds.Size = new System.Drawing.Size(81, 13);
            this.CompiledBuilds.TabIndex = 35;
            this.CompiledBuilds.Text = "Compiled Builds";
            // 
            // SuiteViewer
            // 
            this.SuiteViewer.Location = new System.Drawing.Point(185, 300);
            this.SuiteViewer.Name = "SuiteViewer";
            this.SuiteViewer.Size = new System.Drawing.Size(97, 23);
            this.SuiteViewer.TabIndex = 37;
            this.SuiteViewer.Text = "SuiteViewer";
            this.SuiteViewer.UseVisualStyleBackColor = true;
            this.SuiteViewer.Click += new System.EventHandler(this.SuiteViewer_Click);
            // 
            // TestCaseViewer
            // 
            this.TestCaseViewer.Location = new System.Drawing.Point(186, 369);
            this.TestCaseViewer.Name = "TestCaseViewer";
            this.TestCaseViewer.Size = new System.Drawing.Size(96, 23);
            this.TestCaseViewer.TabIndex = 38;
            this.TestCaseViewer.Text = "TestCaseViewer";
            this.TestCaseViewer.UseVisualStyleBackColor = true;
            this.TestCaseViewer.Click += new System.EventHandler(this.TestCaseViewer_Click);
            // 
            // BuildPath
            // 
            this.BuildPath.Location = new System.Drawing.Point(517, 477);
            this.BuildPath.Name = "BuildPath";
            this.BuildPath.Size = new System.Drawing.Size(358, 20);
            this.BuildPath.TabIndex = 40;
            // 
            // BuildLocation
            // 
            this.BuildLocation.AutoSize = true;
            this.BuildLocation.ForeColor = System.Drawing.Color.Blue;
            this.BuildLocation.Location = new System.Drawing.Point(514, 451);
            this.BuildLocation.Name = "BuildLocation";
            this.BuildLocation.Size = new System.Drawing.Size(117, 13);
            this.BuildLocation.TabIndex = 39;
            this.BuildLocation.Text = "Override Build Location";
            // 
            // ClearCache
            // 
            this.ClearCache.Location = new System.Drawing.Point(96, 630);
            this.ClearCache.Name = "ClearCache";
            this.ClearCache.Size = new System.Drawing.Size(75, 23);
            this.ClearCache.TabIndex = 43;
            this.ClearCache.Text = "ClearCache";
            this.ClearCache.UseVisualStyleBackColor = true;
            this.ClearCache.Click += new System.EventHandler(this.ClearCache_Click);
            // 
            // SuiteExecPB
            // 
            this.SuiteExecPB.Location = new System.Drawing.Point(717, 272);
            this.SuiteExecPB.Name = "SuiteExecPB";
            this.SuiteExecPB.Size = new System.Drawing.Size(158, 23);
            this.SuiteExecPB.TabIndex = 44;
            this.SuiteExecPB.Visible = false;
            // 
            // TestCaseExexPB
            // 
            this.TestCaseExexPB.Location = new System.Drawing.Point(717, 341);
            this.TestCaseExexPB.Name = "TestCaseExexPB";
            this.TestCaseExexPB.Size = new System.Drawing.Size(158, 23);
            this.TestCaseExexPB.TabIndex = 45;
            this.TestCaseExexPB.Visible = false;
            // 
            // TCMExecutor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(887, 665);
            this.Controls.Add(this.TestCaseExexPB);
            this.Controls.Add(this.SuiteExecPB);
            this.Controls.Add(this.ClearCache);
            this.Controls.Add(this.BuildPath);
            this.Controls.Add(this.BuildLocation);
            this.Controls.Add(this.TestCaseViewer);
            this.Controls.Add(this.SuiteViewer);
            this.Controls.Add(this.Builds);
            this.Controls.Add(this.CompiledBuilds);
            this.Controls.Add(this.TFSDefs);
            this.Controls.Add(this.TFSDefinition);
            this.Controls.Add(this.ConfigID);
            this.Controls.Add(this.ConfigurationID);
            this.Controls.Add(this.ExecTestCase);
            this.Controls.Add(this.ExecuteSuite);
            this.Controls.Add(this.TestCaseTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SuiteTextBox);
            this.Controls.Add(this.SuitesID);
            this.Controls.Add(this.TestStngs);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.Error);
            this.Controls.Add(this.TestSetting);
            this.Controls.Add(this.TFSPlan);
            this.Controls.Add(this.TFSPln);
            this.Controls.Add(this.TFSProj);
            this.Controls.Add(this.TFSProject);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.DateT);
            this.Controls.Add(this.TFSCollection);
            this.Controls.Add(this.TFSColls);
            this.Name = "TCMExecutor";
            this.Text = "TCMExecutor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TCMExecutor_FormClosed);
            this.Load += new System.EventHandler(this.TCMExecutor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox TFSColls;
        private System.Windows.Forms.Label TFSCollection;
        private System.Windows.Forms.Label DateT;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label TFSProject;
        private System.Windows.Forms.ComboBox TFSProj;
        private System.Windows.Forms.Label TFSPlan;
        private System.Windows.Forms.ComboBox TFSPln;
        private System.Windows.Forms.Label TestSetting;
        private System.Windows.Forms.Label Error;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ComboBox TestStngs;
        private System.Windows.Forms.Button ExecTestCase;
        private System.Windows.Forms.Button ExecuteSuite;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label SuitesID;
        private System.Windows.Forms.ComboBox ConfigID;
        private System.Windows.Forms.Label ConfigurationID;
        private System.Windows.Forms.ComboBox TFSDefs;
        private System.Windows.Forms.Label TFSDefinition;
        private System.Windows.Forms.ComboBox Builds;
        private System.Windows.Forms.Label CompiledBuilds;
        private System.Windows.Forms.Button SuiteViewer;
        private System.Windows.Forms.Button TestCaseViewer;
        private System.Windows.Forms.TextBox BuildPath;
        private System.Windows.Forms.Label BuildLocation;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button ClearCache;
        private System.Windows.Forms.ProgressBar SuiteExecPB;
        private System.Windows.Forms.ProgressBar TestCaseExexPB;
        public System.Windows.Forms.TextBox SuiteTextBox;
        public System.Windows.Forms.TextBox TestCaseTextBox;
    }
}

