using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.Common {
    public class GameInfo {

        #region Private Members

        private short _mapId;
        private string _name;
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

        public short MaxPlayerNumber {
            get { return _maxPlayerNumber; }
            set { _maxPlayerNumber = value; }
        }

        #endregion

    }
}
