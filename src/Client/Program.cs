using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Client.UI;
using Client.Log;
using System.Reflection;
using System.Threading;
using Yad.Net;
using Yad.Net.General.Messaging;

namespace Client
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

            Connection.InitConnection("127.0.0.1", 15000);
            Connection.SendMessage(new TextMessage());
            Connection.CloseConnection();

            /*MiniForm miniForm = new MiniForm();
            miniForm.Hide();

            UIManager uiManager = new UIManager(miniForm);
            uiManager.Start();

            Application.Run(miniForm);*/

            InfoLog.WriteEnd();
            InfoLog.Close();
        }
    }
}
