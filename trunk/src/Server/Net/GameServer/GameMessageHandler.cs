using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;
using Yad.Log.Common;
using Yad.Engine.Server;
using Yad.Properties;
using Yad.Net.Server;

namespace Yad.Net.GameServer.Server {

    /// <summary>
    /// Class that handles game messages
    /// </summary>
    class GameMessageHandler : MessageHandler {

        #region Private members

        private GameServer _gameServer;

        #endregion

        public GameMessageHandler(GameServer gameServer)
            : base() {
            _gameServer = gameServer;
        }
        /// <summary>
        /// Main processing method
        /// </summary>
        /// <param name="item">Message to process</param>
        public override void ProcessItem(Message item) {
            InfoLog.WriteInfo(string.Format(Resources.GameProcessStringFormat, item.Type.ToString(), 
                _gameServer.Name, _gameServer.GetPlayer(item.PlayerId).Login),
                EPrefix.GameMessageProccesing);

        }
    }
}
