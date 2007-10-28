using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Client.UI;
using Client.Log;
using System.Reflection;

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
            Application.Run(new MenuForm("groupBoxOptions"));

            InfoLog.WriteEnd();
            InfoLog.Close();

        }
    }
}