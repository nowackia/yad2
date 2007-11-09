using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.Common {
    public class GameInfo : ICloneable {

        #region Private Members

        private short _mapId;
        private string _name;
        private bool _isPrivate;    
        private short _maxPlayerNumber;

        #endregion

        #region Properties

        public short MapId {
            get { return _mapId; }
            set { _mapId = value; }
        }
        public string Name {
            get { return _name; }
            set { _name = value; }
        }

        public bool IsPrivate {
            get { return _isPrivate; }
            set { _isPrivate = value; }
        }

        public short MaxPlayerNumber {
            get { return _maxPlayerNumber; }
            set { _maxPlayerNumber = value; }
        }

        #endregion


        #region ICloneable Members

        public object Clone() {
            GameInfo gi = new GameInfo();
            gi.Name = _name;
            gi.MapId = _mapId;
            gi.IsPrivate = _isPrivate;
            gi._maxPlayerNumber = _maxPlayerNumber;
            return (object)gi;
        }

        #endregion
    }
}
