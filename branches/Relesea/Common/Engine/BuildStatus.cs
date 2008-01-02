using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Engine {
    public enum BuildType {
        Unit,
        Building
    }

    public class BuildStatus {
        BuildType _buildType;

        public BuildType BuildType {
            get { return _buildType; }
            set { _buildType = value; }
        }
        int _objectId;

        public int ObjectId {
            get { return _objectId; }
            set { _objectId = value; }
        }
        short _typeid;

        public short Typeid {
            get { return _typeid; }
            set { _typeid = value; }
        }

        public short Percent {
            get {
                return (short)((100 * (int)_actualTurn) / (int)_turnsToBuild);
            }
        }
        short _turnsToBuild;
        short _actualTurn = 0;

        public BuildStatus(int objectid, short typeid, short turnsToBuild, BuildType type) {
            _objectId = objectid;
            _typeid = typeid;
            _buildType = type;
            _turnsToBuild = turnsToBuild;
        }

        public bool DoTurn() {
            if (_actualTurn == _turnsToBuild) {
                _actualTurn = 0;
                return true;
            }
            else {
                ++_actualTurn;
                return false;
            }
        }
    }
}
