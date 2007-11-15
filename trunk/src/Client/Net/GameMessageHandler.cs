using System;
using System.Collections.Generic;
using System.Text;
using Yad.Log.Common;
using Yad.Net.Common;
using Yad.Net.Messaging.Common;
using Yad.UI.Client;

namespace Yad.Net.Client
{
    public delegate void GameInitEventHandler(object sender, GameInitEventArgs e);
    public delegate void DoTurnEventHandler(object sender, EventArgs e);

    public class GameInitEventArgs : EventArgs
    {
        public GameInitInfo[] gameInitInfo;

        public GameInitEventArgs(GameInitInfo[] gameInitInfo)
        {
            this.gameInitInfo = gameInitInfo;
        }
    }

    public class GameMessageHandler : IMessageHandler
    {
        /// <summary>
        /// Excecuted in another thread, different from the subscribing one.
        /// Using this event needs caution and proper thread synchronization mechanisms.
        /// </summary>
        public event GameInitEventHandler GameInitialization;
        /// <summary>
        /// Excecuted in another thread, different from the subscribing one.
        /// Using this event needs caution and proper thread synchronization mechanisms.
        /// </summary>
        public event DoTurnEventHandler DoTurnPermission;


        private static GameMessageHandler instance = new GameMessageHandler();

        public static GameMessageHandler Instance
        {
            get
            { return instance; }
        }

        public void ProcessMessage(Message message)
        {
            switch (message.Type)
            {
                case MessageType.GameInit:
                    {
                        GameInitMessage gameInitMessage = message as GameInitMessage;
                        int columns = gameInitMessage.PlayerStartPoints.GetLength(0);
                        GameInitInfo[] infoTab = new GameInitInfo[columns];

                        for (int i = 0; i < columns; i++)
                            infoTab[i] = new GameInitInfo(gameInitMessage.PlayerStartPoints[i, 2], gameInitMessage.PlayerStartPoints[i, 0], gameInitMessage.PlayerStartPoints[i, 1]);

                        if (GameInitialization != null)
                            GameInitialization(this, new GameInitEventArgs(infoTab));
                    }
                    break;

                case MessageType.Move:
                case MessageType.Destroy:
                case MessageType.CreateUnit:
                case MessageType.Build:
                case MessageType.Harvest:
                case MessageType.Attack:
                    GameForm.sim.AddGameMessage((GameMessage)message);
                    break;

                case MessageType.DoTurn:
                    if (DoTurnPermission != null)
                        DoTurnPermission(this, EventArgs.Empty);
                    break;

                case MessageType.Control:
                    throw new NotImplementedException("Control message handling has not been implemented yet - is it needed ?");

                default:
                    InfoLog.WriteInfo("GameMessageHandler received unknown message type: " + message.Type, EPrefix.ClientInformation);
                    break;
            }
        }
    }
}
