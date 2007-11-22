using System;
using System.Collections.Generic;
using System.Text;
using Yad.Engine.Common;
using System.Drawing;

namespace Yad.Net.Common
{
    public class PlayerInfo
    {
        public const int MaxTeamNo = 8;

        private short _id;
        private short _house;
        private short _teamID;
        private string _name;
        private Color _color;

        public PlayerInfo()
        {
            _id = -1;
            _house = -1;
            _teamID = -1;
            _name = string.Empty;
            _color = Color.Red;
        }

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

        public short House
        {
            get { return _house; }
            set { _house = value; }
        }

        public short TeamID
        {
            get { return _teamID; }
            set { _teamID = value; }
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
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
            if (System.Object.ReferenceEquals(a, b))
                return true;

            if (((object)a == null) || ((object)b == null))
                return false;

            return a.Equals((object)b);
        }

        public static bool operator !=(PlayerInfo a, PlayerInfo b)
        {
            return !(a == b);
        }
    }
}
