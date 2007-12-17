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


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AudioEngine.Instance.Init();
            AudioEngine.Instance.Music.LoadMusic();
            AudioEngine.Instance.Sound.LoadSounds();

            AudioEngine.Instance.Sound.PlayHouse(Yad.Config.GlobalSettings.Instance.DefaultHouse, new Yad.Engine.HouseSoundType[] { Yad.Engine.HouseSoundType.Approach, Yad.Engine.HouseSoundType.East });
            AudioEngine.Instance.Sound.PlayHouse(Yad.Config.GlobalSettings.Instance.DefaultHouse, new Yad.Engine.HouseSoundType[] { Yad.Engine.HouseSoundType.Warning, Yad.Engine.HouseSoundType.West });

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
