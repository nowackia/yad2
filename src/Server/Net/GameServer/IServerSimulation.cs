using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.GameServer.Server {
    interface IServerSimulation {
        uint GetPlayerTurn(short id);
        uint GetMinTurn();
        void IncPlayerTurn(short id);
        void AddPlayer(short id);
    }
}
