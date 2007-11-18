using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.Server {
    public interface IPlayerProvider {
        Player GetPlayer(short id);
        IEnumerator<KeyValuePair<short,Player>> GetPlayers();
        object PlayerLock {
            get;
        }
    }
}
