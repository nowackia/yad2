using System;
using System.Collections.Generic;
using System.Text;
using Yad.Engine.Common;

namespace Yad.Net.Common
{
    public class PlayerInfo
    {

        public const int MaxTeamNo = 4;
        private const HouseType DefaultHouseType = HouseType.Atreides;

        private HouseType _house = DefaultHouseType;
        private int _teamID;
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public HouseType House
        {
            get { return _house; }
            set { _house = value; }
        }

        public int TeamID
        {
            get { return _teamID; }
            set { _teamID = value; }
        }
    }
}
