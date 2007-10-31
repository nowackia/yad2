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
    delegate void SetTextCallback(string text);
    public partial class ConsoleForm : Form
    {
        #region Pola prywatne

        private ServerMain _serverProcess;
        private bool _isClosedManualy = false;

        #endregion 

        #region Konstrutkory

        public ConsoleForm()
        {
            InitializeComponent();
        }

        #endregion

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
            _isClosedManualy = true;
            this.Close();
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