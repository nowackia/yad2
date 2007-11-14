using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;

namespace Yad.Net.Common
{
    public class GameInfo
    {

        #region Private Members

        private short _mapId;
        private string _name;
        private short _maxPlayerNumber;
        private byte _type;

        #endregion

        #region Properties

        public short MapId
        {
            get { return _mapId; }
            set { _mapId = value; }
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
                return "MapId: " + MapId + Environment.NewLine
                     + "Name: " + Name + Environment.NewLine
                     + "Maximum players number: " + MaxPlayerNumber + Environment.NewLine
                     + "Type: " + this.GameType.ToString();
            }
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion

    }
}
