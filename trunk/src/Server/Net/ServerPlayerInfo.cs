using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Common;
using Yad.Config.Common;
using Yad.Config;


namespace Yad.Net.Server {
    class ServerPlayerInfo : PlayerInfo, IPlayerID {

        private bool _startedClicked = false;
        private Player _player;

        public Player Player {
            get { return _player; }
            set { _player = value; }
        }

        public bool StartedClicked {
            get { return _startedClicked; }
            set { _startedClicked = value; }
        }

        public ServerPlayerInfo(Player player) {
            this.Id = player.Id;
            this.Name = player.Login;
            this.House = GlobalSettings.DeafultHouse;
            _player = player;
            
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
