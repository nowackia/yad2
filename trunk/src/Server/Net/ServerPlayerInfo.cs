using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Common;

namespace Yad.Net.Server {
    class ServerPlayerInfo : PlayerInfo, IPlayerID {
        bool _startedClicked = false;

        public bool StartedClicked {
            get { return _startedClicked; }
            set { _startedClicked = value; }
        }

        public ServerPlayerInfo(short id, string name) {
            this.Id = id;
            this.Name = name;
        }

        public PlayerInfo GePlayerInfo() {
            PlayerInfo mi = new PlayerInfo();
            mi.Id = this.Id;
            mi.Name = this.Name;
            mi.TeamID = this.TeamID;
            mi.House = this.House;
            return mi;
        }


        #region IPlayerID Members

        public short GetID() {
            return this.Id;
        }

        #endregion
    }
}
