using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;

namespace Yad.Engine.Simulation.Common {
	public interface IOnGameMessage {
		bool HandleMessage(GameMessage gameMessage);
	}
}
