using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.Common {
    public class GameInfo {

        #region Private Members

        private short _mapId;
        private string _name;
        private bool _isPrivate;    
        private int _maxPlayerNumber;

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

        public int MaxPlayerNumber {
            get { return _maxPlayerNumber; }
            set { _maxPlayerNumber = value; }
        }

        #endregion

    }
}
