using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Tests
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IniReader ini = new IniReader("test.ini");
            Console.WriteLine(ini.ReadInteger("Section1", "KeyInt"));

            ini.Write("Section1", "KeyString", "MyString");
            ini.Write("Section1", "KeyInt", 5);
            ini.Write("Section2", "KeyBool", true);
            ini.Write("Section2", "KeyBytes", new byte[] { 0, 123, 255 });
            ini.Write("Section3", "KeyLong", (long)123456789101112);
            ini.Section = "Section1";

            Console.WriteLine("Tests finished");
            /*Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TestForm());*/
        }
    }
}