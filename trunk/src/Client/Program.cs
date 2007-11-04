using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Client.UI;
using Client.Log;
using System.Reflection;
using System.Threading;

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
