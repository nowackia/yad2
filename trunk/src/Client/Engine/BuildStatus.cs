using System;
using System.Collections.Generic;
using System.Text;
using Yad.UI;
using Yad.Board;

namespace Yad.Engine {

    public enum BuildType {
        Unit,
        Building
    }
    public class BuildStatus {
        StripButtonState _state;
        BuildType _buildType;

        public BuildType BuildType {
            get { return _buildType; }
            set { _buildType = value; }
        }
        public StripButtonState State {
            get { return _state; }
            set { _state = value; }
        }
        int _objectId;
        short _typeid;

        public short Typeid {
            get { return _typeid; }
            set { _typeid = value; }
        }

        public short Percent {
            get {
                return (short)((100 *(int)_actualTurn) / (int)_turnsToBuild);
            }
        }
        short _turnsToBuild;
        short _actualTurn;

        public BuildStatus(int objectid, short typeid) {
            _objectId = objectid;
            _typeid = typeid;
            _state = StripButtonState.Active;
        }
    }
}
