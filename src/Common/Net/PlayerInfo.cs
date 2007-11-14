using System;
using System.Collections.Generic;
using System.Text;
using Yad.Engine.Common;

namespace Yad.Net.Common
{
    public class PlayerInfo
    {

        public const int MaxTeamNo = 8;
        private const HouseType DefaultHouseType = HouseType.Atreides;
        HouseType _house = DefaultHouseType;
        short _teamID; 
       
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
        
        public short TeamID {
            get { return _teamID; }
            set { _teamID = value; }
        }
    }
}
