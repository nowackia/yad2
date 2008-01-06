using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using Yad.Engine.Client;
using Yad.Log.Common;
using Yad.Net.Client;
using Yad.Net.Common;
using Yad.Net.Messaging.Common;
using Yad.UI.Client;
using Yad.Log;

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
            InfoLog.Instance.InitializeLogFile(LogFiles.ProcessMsgLog);
            InfoLog.Instance.InitializeLogFile(LogFiles.IncomingMsgLog);
            InfoLog.Instance.InitializeLogFile(LogFiles.BuildManagerLog);
            InfoLog.Instance.InitializeLogFile(LogFiles.AudioEngineLog);
            InfoLog.Instance.InitializeLogFile(LogFiles.Astar);

            InfoLog.Instance.AddRedirection(EPrefix.BMan, LogFiles.BuildManagerLog);
            InfoLog.Instance.AddRedirection(EPrefix.AStar, LogFiles.Astar);
            InfoLog.Instance.AddRedirection(EPrefix.AudioEngine, LogFiles.AudioEngineLog);
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
