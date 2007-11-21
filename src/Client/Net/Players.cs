using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Common;

namespace Yad.Net
{
    public class Players
    {
        private Dictionary<short, PlayerInfo> players;

        public Players()
        {
            players = new Dictionary<short, PlayerInfo>();
        }

        public void Clear()
        {
            lock (players)
            { players.Clear(); }
        }

        public void Add(PlayerInfo playerInfo)
        {
            lock (players)
            { players.Add(playerInfo.Id, playerInfo); }
        }

        public void Add(PlayerInfo[] playerInfos)
        {
            lock (players)
            {
                foreach (PlayerInfo playerInfo in playerInfos)
                    players.Add(playerInfo.Id, playerInfo);
            }
        }

        public void Modify(PlayerInfo playerInfo)
        {
            lock (players)
            {
                if (players.ContainsKey(playerInfo.Id))
                    players[playerInfo.Id] = playerInfo;
            }
        }

        public void Modify(PlayerInfo[] playerInfos)
        {
            lock (players)
            {
                foreach (PlayerInfo playerInfo in playerInfos)
                {
                    if (players.ContainsKey(playerInfo.Id))
                        players[playerInfo.Id] = playerInfo;
                }
            }
        }

        public void Remove(short playerId)
        {
            lock (players)
            { players.Remove(playerId); }
        }

        public void Remove(PlayerInfo playerInfo)
        {
            this.Remove(playerInfo.Id);
        }

        public void Remove(PlayerInfo[] playerInfos)
        {
            lock (players)
            {
                foreach (PlayerInfo playerInfo in playerInfos)
                    players.Remove(playerInfo.Id);
            }
        }

        public PlayerInfo GetPlayerInfo(short playerId)
        {
            lock (players)
            { return players[playerId]; }
        }

        public PlayerInfo[] GetPlayerInfos()
        {
            lock (players)
            {
                Dictionary<short, PlayerInfo>.ValueCollection valueCollection = players.Values;
                PlayerInfo[] playerInfos = new PlayerInfo[valueCollection.Count];
                valueCollection.CopyTo(playerInfos, 0);

                return playerInfos;
            }
        }

        public short[] GetPlayerIDs()
        {
            lock (players)
            {
                Dictionary<short, PlayerInfo>.KeyCollection keyCollection = players.Keys;
                short[] playerIDs = new short[keyCollection.Count];
                keyCollection.CopyTo(playerIDs, 0);

                return playerIDs;
            }
        }

        public int Count
        {
            get
            {
                lock (players)
                { return players.Count; }
            }
        }
    }
}
