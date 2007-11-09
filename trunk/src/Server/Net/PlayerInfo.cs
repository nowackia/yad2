using System;
using System.Collections.Generic;
using System.Text;
using Yad.Engine.Common;
namespace Yad.Net.Server {
    public class PlayerInfo {
        HouseType _house;

        public HouseType House {
            get { return _house; }
            set { _house = value; }
        }
        int _teamID;
    }
}
