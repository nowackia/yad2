using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Client.UI;
using Client.Log;
using System.Reflection;
using System.Threading;
using Yad.Net;
using Yad.Net.General.Messaging;
using System.Net.Sockets;
using System.IO;
using Client.Net.General.Messaging;

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

            Connection.InitConnection("127.0.0.1", 1734);
            LoginMessage loginMessage = new LoginMessage();
            loginMessage.Login = "test";
            loginMessage.Password = "testpsw";
            loginMessage.UserId = 6;
            Connection.SendMessage(loginMessage);

            InfoLog.Write("Press any key to continue ...");
            Console.Read();

            Connection.CloseConnection();

			MiniForm miniForm = new MiniForm();
            miniForm.Hide();

            UIManager uiManager = new UIManager(miniForm);
            uiManager.Start();

            Application.Run(miniForm);

            InfoLog.WriteEnd();
            InfoLog.Close();
        }
    }
}
