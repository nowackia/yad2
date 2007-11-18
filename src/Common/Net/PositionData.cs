using System;
using System.Collections.Generic;
using System.Text;


namespace Yad.Net.Common {
    public class PositionData : IComparable {

        #region Private Members

        short _playerId = 0;
        int _y = 0;
        int _x = 0;

        #endregion

        #region Properites

        public short PlayerId {
            get { return _playerId; }
            set { _playerId = value; }
        }
        

        public int X {
            get { return _x; }
            set { _x = value; }
        }
        

        public int Y {
            get { return _y; }
            set { _y = value; }
        }

        #endregion

        #region Constructors

        public PositionData(){ }

        public PositionData(short PlayerID) {
            _playerId = PlayerID;
        }

        public PositionData(short PlayerID, int X, int Y) {
            _playerId = PlayerID;
            _x = X;
            _y = Y;
        }

        #endregion


        #region IComparable Members

        public int CompareTo(object obj) {
            return _playerId.CompareTo(((PositionData)obj).PlayerId);
        }

        #endregion
    }
}
