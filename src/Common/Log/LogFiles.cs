using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Yad.Log {
    public enum LogFiles {
        [Description("ErrorLog.txt")]
        DefaultInfoLog,
        [Description("BMLog.txt")]
        BuildManagerLog,
        [Description("IncomingMsgLog.txt")]
        IncomingMsgLog,
        [Description("ProcessMsgLog.txt")]
        ProcessMsgLog,
        [Description("AudioEngineLog.txt")]
        AudioEngineLog,
        [Description("AStar.txt")]
        Astar,
		[Description("FullSimulationLog.txt")]
		FullSimulationLog
    }
}
