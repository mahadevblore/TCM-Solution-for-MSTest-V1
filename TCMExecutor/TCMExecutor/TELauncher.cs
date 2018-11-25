using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCMExecutor
{
    public partial class TELauncher : Form
    {
        public TELauncher()
        {
            InitializeComponent();
            timer1.Enabled = true;
        }

        private void Run_Click(object sender, EventArgs e)
        {
            Program.isCached = false;
            timer1.Enabled = false;

            int d;

            for (d = 0; d <= 100; d++)
                progressBar1.Value = d;
            
            LaunchLabel.Text = "Please wait...";
            TCMExecutor tcmExec = new TCMExecutor(this);
            tcmExec.Show();
            
        }

        private void RunCache_Click(object sender, EventArgs e)
        {
            Program.isCached = true;
            timer1.Enabled = false;

            int d;

            for (d = 0; d <= 100; d++)
                progressBar1.Value = d;

            
            LaunchLabel.Text = "Please wait...";
            System.Threading.Thread.Sleep(1000);
            TCMExecutor tcmExec = new TCMExecutor(this);
            tcmExec.Show();
            
        }

        private void TELauncher_Load(object sender, EventArgs e)
        {
            if (!Program.tcmPath.Contains("IDE"))
            {
                MessageBox.Show("Please Install TCM.exe and related assemblies in order to continue to use TCMExecutor");
                Application.Exit();
            }
        }
    }
}
