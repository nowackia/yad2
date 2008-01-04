using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SyncGuard
{
    public partial class SyncGuard : Form
    {
        public SyncGuard()
        {
            InitializeComponent();
        }

        private StreamReader sr1, sr2;

        private void loadFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK && openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                bool ok = true;
                StringBuilder sb1 = new StringBuilder(), sb2 = new StringBuilder();
                string st1, st2;
                Stream s1 = openFileDialog1.OpenFile(), 
                        s2 = openFileDialog2.OpenFile();
                
                sr1 = new StreamReader(s1);
                sr2 = new StreamReader(s2);
                while ((st1 = sr1.ReadLine()) != null && (st2 = sr2.ReadLine()) != null)
                {
                    if (st1.Equals(st2) && ok)
                        continue;
                    ok = false;
                    sb1.AppendLine(st1);
                    sb2.AppendLine(st2);
                }
                richTextBox1.Text = sb1.ToString();
                richTextBox2.Text = sb2.ToString();
            }
        }

    }
}