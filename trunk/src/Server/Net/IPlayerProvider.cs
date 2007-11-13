using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.Server {
    interface IPlayerProvider {
        Player GetPlayer(short id);
        IEnumerator<KeyValuePair<short,Player>> GetPlayers();
    }
}
