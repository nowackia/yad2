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

        private short _id;
        private HouseType _house = DefaultHouseType;
        private short _teamID;     
        private string _name;

        public short Id
        {
            get { return _id; }
            set { _id = value; }
        }

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

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            PlayerInfo p = obj as PlayerInfo;
            if ((object)p == null)
                return false;

            return this._id == p._id;
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }

        public static bool operator ==(PlayerInfo a, PlayerInfo b)
        {
            return a.Equals((object)b);
        }

        public static bool operator !=(PlayerInfo a, PlayerInfo b)
        {
            return !(a == b);
        }
    }
}
