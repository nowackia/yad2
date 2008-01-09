using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;

namespace Yad.Net.Common
{
    public class GameInfo
    {

        #region Private Members

        private string _mapName;
        private string _name;
        private short _maxPlayerNumber;
        private byte _type;

        #endregion

        #region Properties

        public string MapName
        {
            get { return _mapName; }
            set { _mapName = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public short MaxPlayerNumber
        {
            get { return _maxPlayerNumber; }
            set { _maxPlayerNumber = value; }
        }

        public GameType GameType
        {
            get { return (GameType)_type; }
            set { _type = (byte)value; }
        }

        public string Description
        {
            get
            {
                return "Map Name: " + MapName + Environment.NewLine
                     + "Game Name: " + Name + Environment.NewLine
                     + "Maximum players number: " + MaxPlayerNumber + Environment.NewLine
                     + "Type: " + this.GameType.ToString();
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            GameInfo gi = obj as GameInfo;
            if ((object)gi == null)
                return false;

            return this._mapName == gi._mapName && this._maxPlayerNumber == gi._maxPlayerNumber
                && this._name == gi._name && this._type == gi._type;
        }

        public override int GetHashCode()
        {
            return _mapName.GetHashCode();
        }

        public static bool operator ==(GameInfo a, GameInfo b)
        {
            if (System.Object.ReferenceEquals(a, b)) {
                return true;
            }

            if (((object)a == null) || ((object)b == null)) {
                return false;
            }

            return a.Equals((object)b);
        }

        public static bool operator !=(GameInfo a, GameInfo b)
        {
            return !(a == b);
        }

        #endregion

    }
}
