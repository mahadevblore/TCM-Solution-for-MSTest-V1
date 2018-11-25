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
    public partial class SuiteTable : Form
    {
        TCMExecutor tcmExec;
        public string MyString { get; set; }

        public SuiteTable(SortedDictionary<string,string> suiteDict,TCMExecutor newtcmExec)
        {
            InitializeComponent();
            SuiteGrid.DataSource = suiteDict.ToArray();
            SuiteGrid.Columns[0].HeaderText = "SuiteID";
            SuiteGrid.Columns[0].Width = 100;
            SuiteGrid.Columns[1].HeaderText = "SuiteName";
            SuiteGrid.Columns[1].Width = 340;
            tcmExec = newtcmExec;
        }

        private void Submit_Click(object sender, EventArgs e)
        {
          DataGridViewSelectedRowCollection dgRows = SuiteGrid.SelectedRows;
          Program.suiteList.Clear();
          foreach (DataGridViewRow dgRow in dgRows)
          {
              Program.suiteList.Add(((KeyValuePair<string,string>) dgRow.DataBoundItem).Key.ToString());
          }
          MyString = string.Join(",",Program.suiteList.ToArray<string>());
          tcmExec.SuiteTextBox.Text = MyString;
            this.Close();
        }
    }
}
