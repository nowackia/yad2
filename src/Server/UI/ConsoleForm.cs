using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Yad.Net.Server;
using Yad.Log.Common;

namespace Yad.UI.Server
{
    delegate void SetTextCallback(string text);
    public partial class consoleForm : Form
    {
        #region Pola prywatne

        private ServerMain _serverProcess;
        private bool _isClosedManualy = false;
        private bool _isAppendPossible = true;

        public bool IsAppendPossible {
            get { return _isAppendPossible; }
            set { _isAppendPossible = value; }
        }
        #endregion 

        #region Konstrutkory

        public consoleForm()
        {
            InitializeComponent();
        }

        #endregion

        public void AppendText(string s)
        {
            if (IsAppendPossible) {
                if (this.textBox.InvokeRequired) {
                    SetTextCallback d = new SetTextCallback(AppendText);
                    this.Invoke(d, new object[] { s });
                }
                else {
                    if (!this.textBox.IsDisposed)
                        this.textBox.AppendText(s + "\n");
                }
            }
        }


        private void endServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _serverProcess.Stop();
            _isClosedManualy = true;
            this.Close();
        }

        private void alwaysOnTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem toolStripMenuItem = sender as ToolStripMenuItem;

            toolStripMenuItem.Checked = !toolStripMenuItem.Checked;
            this.TopMost = toolStripMenuItem.Checked;
        }

        private void ConsoleForm_Load(object sender, EventArgs e)
        {
            _serverProcess = new ServerMain();
        }

        private void ConsoleForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_isClosedManualy)
            {
                e.Cancel = true;
                HideConsole();
            }
        }

        private void hideConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Visible)
                HideConsole();
            else
                ShowConsole();
        }

        private void ShowConsole()
        {
            this.Show();
            this.Visible = true;
            contextMenuStrip.Items[0].Text = "Hide console";
        }

        private void HideConsole()
        {
            this.Hide();
            this.Visible = false;
            contextMenuStrip.Items[0].Text = "Show console";
        }
    }
}