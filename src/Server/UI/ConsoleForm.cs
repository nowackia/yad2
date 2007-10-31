using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Client.Log;
using System.Threading;
using Server.ServerManagement;

namespace Server.UI
{
    public partial class ConsoleForm : Form
    {
        ServerMain _serverProcess;
        delegate void SetTextCallback(string text);
        public ConsoleForm()
        {
            InitializeComponent();
        }
        public void AppendText(string s)
        {
            if (this.textBox.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(AppendText);
                this.Invoke(d, new object[] { s });
            }
            else
            {
                if (!this.textBox.IsDisposed)
                    this.textBox.AppendText(s + "\n");
            }
        }


        private void endServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _serverProcess.Stop();
            this.Close();
        }

        private void ConsoleForm_Load(object sender, EventArgs e)
        {
            _serverProcess = new ServerMain();
        }
    }
}