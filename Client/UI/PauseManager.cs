using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.UI {
    /// <summary>
    /// asc - another stupid class
    /// </summary>
    public class PauseManager {
        const int MAX_PAUSE_PER_GAME = 3;
        static int counter = MAX_PAUSE_PER_GAME;

        public static void Reset() {
            counter = MAX_PAUSE_PER_GAME;
        }

        public static bool TryPause() {
            if (counter > 0) { counter--; return true; }
            return false;
        }

    }
}
