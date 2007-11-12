using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Common;

namespace Yad.Net.Server {
    class ServerPlayerInfo : GamePlayerInfo, IPlayerID {
        bool _startedClicked = false;

        public bool StartedClicked {
            get { return _startedClicked; }
            set { _startedClicked = value; }
        }

        public ServerPlayerInfo(short id, string name) {
            this.Id = id;
            this.Name = name;
        }

        public GamePlayerInfo GetGamePlayerInfo() {
            GamePlayerInfo gmi = new GamePlayerInfo();
            gmi.Id = this.Id;
            gmi.Name = this.Name;
            gmi.TeamID = this.TeamID;
            gmi.House = this.House;
            return gmi;
        }


        #region IPlayerID Members

        public short GetID() {
            return this.Id;
        }

        #endregion
    }
}
