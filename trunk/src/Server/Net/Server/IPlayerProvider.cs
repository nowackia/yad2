using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.Server {
    interface IPlayerProvider {
        Player GetPlayer(int id);
        IEnumerator<KeyValuePair<int,Player>> GetPlayers();
    }
}
