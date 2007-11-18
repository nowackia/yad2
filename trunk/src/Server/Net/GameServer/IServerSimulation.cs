using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;
using Yad.Net.Common;

namespace Yad.Net.GameServer.Server {
    interface IServerSimulation {
        int Delta {
            get;
        }
        int  GetPlayerTurn(short id);
        int  GetMinTurn();
        void IncPlayerTurn(short id);
        void AddPlayer(short id, PlayerData pd);
        bool IsPlayerWaiting(short id);
        void SetWaiting(short id);
        void SetEndGame(short id, bool hasWon);
        bool HasGameEnded();
        short[] StopWaiting();
        GamePlayer GetGamePlayer(short id);
        PlayerData[] GetPlayerData();
        //TODO: Tu mozliwe, ze konieczne bedzie kopiowanie wiadomosci
        void AddMessage(Message msg);
    }
}
