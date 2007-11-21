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

            /* Server Fake only for client testing reasons, change output type to Console Application
             * in project properties */
            //ServerFake.Process();
            /* Uncomment this if you want original server, change output type to Windows Application
             * in project properties */
            Application.Run(consoleForm);


            InfoLog.WriteEnd();
            InfoLog.Close();
        }
    }
}