using System;
using System.Collections.Generic;
using System.Text;
using Yad.UI;
using Yad.Board;

namespace Yad.Engine {

    public class BuildStatusStrip: BuildStatus {
        StripButtonState _state;
       
        public StripButtonState State {
            get { return _state; }
            set { _state = value; }
        }
      
        public BuildStatusStrip(int objectid, short typeid, short turnsToBuild, BuildType type) : base(objectid, typeid, turnsToBuild, type) {
            _state = StripButtonState.Active;
        }
    }
}
