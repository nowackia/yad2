using Yad.Net.Server;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using Yad.Log.Common;
using Yad.UI.Server;
using System.Reflection;
using System.Net.Sockets;

namespace Yad.Server
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
            consoleForm consoleForm = new consoleForm();
            OnWriteLineDelegate owd = new OnWriteLineDelegate(consoleForm.AppendText);
            InfoLog.Instance.OnWriteLine += owd;
            
            InfoLog.WriteStart();
            //InfoLog.Disable();
            Application.Run(consoleForm);


            InfoLog.WriteEnd();
            InfoLog.Close();
        }
    }
}