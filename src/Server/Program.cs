using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Server.UI;
using Client.Log;
using System.Threading;
using Server.ServerManagement;

namespace Server
{
    static class Program
    {
        public static bool IsApplicationEnd = false;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        ////
        [STAThread]
        static void Main()
        {
            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();

            ConsoleForm consoleForm = new ConsoleForm();
            OnWriteLineDelegate owd = new OnWriteLineDelegate(consoleForm.AppendText);
            InfoLog.Instance.OnWriteLine += owd;
            
            InfoLog.WriteStart();

            Application.Run(consoleForm);

            InfoLog.WriteEnd();
            InfoLog.Close();
        }
    }
}