using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using Yad.Log;
using Yad.Net;
using Yad.Net.Client;
using Yad.Net.Common;
using Yad.Net.Messaging.Common;
using Yad.Log.Common;
using Yad.UI.Client;

namespace Yad.Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            InfoLog.WriteStart();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

			MiniForm miniForm = new MiniForm();
            miniForm.Hide();

            UIManager uiManager = new UIManager(miniForm);
            uiManager.Start();

            Application.Run(miniForm);

            Connection.Instance.CloseConnection();

            InfoLog.WriteEnd();
            InfoLog.Close();
        }
    }
}
