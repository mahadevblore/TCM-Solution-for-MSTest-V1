namespace TCMExecutor
{
    partial class TestCaseTable
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
            this.TestCaseGrid = new System.Windows.Forms.DataGridView();
            this.Submit = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.TestCaseGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // TestCaseGrid
            // 
            this.TestCaseGrid.AllowUserToOrderColumns = true;
            this.TestCaseGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TestCaseGrid.Location = new System.Drawing.Point(12, 12);
            this.TestCaseGrid.Name = "TestCaseGrid";
            this.TestCaseGrid.Size = new System.Drawing.Size(486, 652);
            this.TestCaseGrid.TabIndex = 0;
            // 
            // Submit
            // 
            this.Submit.Location = new System.Drawing.Point(12, 670);
            this.Submit.Name = "Submit";
            this.Submit.Size = new System.Drawing.Size(123, 23);
            this.Submit.TabIndex = 1;
            this.Submit.Text = "Submit";
            this.Submit.UseVisualStyleBackColor = true;
            this.Submit.Click += new System.EventHandler(this.Submit_Click);
            // 
            // TestCaseTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 700);
            this.Controls.Add(this.Submit);
            this.Controls.Add(this.TestCaseGrid);
            this.Name = "TestCaseTable";
            this.Text = "SuiteTable";
            ((System.ComponentModel.ISupportInitialize)(this.TestCaseGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView TestCaseGrid;
        private System.Windows.Forms.Button Submit;
    }
}