using System;
using System.Windows.Forms;

namespace Dune_2_Remade
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            Application.Run(new MainMenu());
            if (GlobalData.isPlaying == true)
            {
                DuneGame game = new DuneGame();
                game.Run();
            }
        }
    }
}

