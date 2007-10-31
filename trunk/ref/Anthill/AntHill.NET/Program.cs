using System;
using System.Windows.Forms;

namespace AntHill.NET
{
    static class Program
    {
        [STAThread]
        static void Main()
        {            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm mf = null;
            try
            {
                mf = new MainForm();
            }
            catch
            {
                MessageBox.Show(Properties.Resources.errorFatal);
                Application.Exit();
            }
            Application.Run(mf);
        }
    }
}
