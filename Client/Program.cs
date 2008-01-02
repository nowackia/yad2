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
			InfoLog2.WriteStart();
			InfoLog3.WriteStart();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AudioEngine.Instance.Init();
            AudioEngine.Instance.Music.LoadMusic();
            AudioEngine.Instance.Sound.LoadSounds();

			MiniForm miniForm = new MiniForm();
            miniForm.Hide();

            UIManager uiManager = new UIManager(miniForm);
            uiManager.Start();

            Application.Run(miniForm);

            Connection.Instance.CloseConnection();


			InfoLog.WriteEnd();
			InfoLog.Close();
			InfoLog2.WriteEnd();
			InfoLog2.Close();
			InfoLog3.WriteEnd();
			InfoLog3.Close();
        }
    }
}
