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
    public partial class TestCaseTable : Form
    {
        TCMExecutor tcmExec;
        public string MyString { get; set; }

        public TestCaseTable(SortedDictionary<string, string> testDict, TCMExecutor newtcmExec)
        {
            InitializeComponent();
            TestCaseGrid.DataSource = testDict.ToArray();
            TestCaseGrid.Columns[0].HeaderText = "TestCaseID";
            TestCaseGrid.Columns[0].Width = 100;
            TestCaseGrid.Columns[1].HeaderText = "TestCaseName";
            TestCaseGrid.Columns[1].Width = 340;
            tcmExec = newtcmExec;
        }

        private void Submit_Click(object sender, EventArgs e)
        {
          DataGridViewSelectedRowCollection dgRows = TestCaseGrid.SelectedRows;
          Program.testList.Clear();
          foreach (DataGridViewRow dgRow in dgRows)
          {
              Program.testList.Add(((KeyValuePair<string, string>)dgRow.DataBoundItem).Key.ToString());
          }
          MyString = string.Join(",", Program.testList.ToArray<string>());
          tcmExec.TestCaseTextBox.Text = MyString;
            this.Close();
        }
    }
}
