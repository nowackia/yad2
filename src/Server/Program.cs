using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Server.UI;
using Client.Log;

namespace Server
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
            Application.Run(new MainForm());

            InfoLog.WriteEnd();
            InfoLog.Close();
        }
    }
}