namespace TCMExecutor
{
    partial class TELauncher
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
            this.components = new System.ComponentModel.Container();
            this.Run = new System.Windows.Forms.Button();
            this.RunCache = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.LaunchLabel = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.TeLWait = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Run
            // 
            this.Run.Location = new System.Drawing.Point(71, 77);
            this.Run.Name = "Run";
            this.Run.Size = new System.Drawing.Size(163, 58);
            this.Run.TabIndex = 0;
            this.Run.Text = "Run Executor";
            this.Run.UseVisualStyleBackColor = true;
            this.Run.Click += new System.EventHandler(this.Run_Click);
            // 
            // RunCache
            // 
            this.RunCache.Location = new System.Drawing.Point(71, 178);
            this.RunCache.Name = "RunCache";
            this.RunCache.Size = new System.Drawing.Size(163, 58);
            this.RunCache.TabIndex = 1;
            this.RunCache.Text = "           Run Executor in              Cached Mode";
            this.RunCache.UseVisualStyleBackColor = true;
            this.RunCache.Click += new System.EventHandler(this.RunCache_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(92, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "TCM Executor Launcher";
            // 
            // LaunchLabel
            // 
            this.LaunchLabel.AutoSize = true;
            this.LaunchLabel.Location = new System.Drawing.Point(134, 149);
            this.LaunchLabel.Name = "LaunchLabel";
            this.LaunchLabel.Size = new System.Drawing.Size(0, 13);
            this.LaunchLabel.TabIndex = 3;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 242);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(268, 23);
            this.progressBar1.TabIndex = 4;
            // 
            // TeLWait
            // 
            this.TeLWait.AutoSize = true;
            this.TeLWait.Location = new System.Drawing.Point(134, 149);
            this.TeLWait.Name = "TeLWait";
            this.TeLWait.Size = new System.Drawing.Size(0, 13);
            this.TeLWait.TabIndex = 5;
            // 
            // TELauncher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.TeLWait);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.LaunchLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RunCache);
            this.Controls.Add(this.Run);
            this.Name = "TELauncher";
            this.Text = "TELauncher";
            this.Load += new System.EventHandler(this.TELauncher_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Run;
        private System.Windows.Forms.Button RunCache;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label LaunchLabel;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Timer timer1;
        public System.Windows.Forms.Label TeLWait;
    }
}