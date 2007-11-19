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
    public delegate void GameMessageEventHandler(object sender, GameMessageEventArgs e);
    public delegate void DoTurnEventHandler(object sender, EventArgs e);

    public class GameInitEventArgs : EventArgs
    {
        public PositionData[] gameInitInfo;

        public GameInitEventArgs(PositionData[] gameInitInfo)
        {
            this.gameInitInfo = gameInitInfo;
        }
    }

    public class GameMessageEventArgs : EventArgs
    {
        public MessageType messageType;
        public GameMessage gameMessage;

        public GameMessageEventArgs(GameMessage gameMessage)
        {
            this.messageType = gameMessage.Type;
            this.gameMessage = gameMessage;
        }
    }

    public class GameMessageHandler : IMessageHandler
    {
        /// <summary>
        /// Event excecuted on game initialization message.
        /// Excecuted in another thread, different from the subscribing one.
        /// Using this event needs caution and proper thread synchronization mechanisms.
        /// </summary>
        public event GameInitEventHandler GameInitialization;
        /// <summary>
        /// Event excecuted on receiving permission to process another turn.
        /// Excecuted in another thread, different from the subscribing one.
        /// Using this event needs caution and proper thread synchronization mechanisms.
        /// </summary>
        public event DoTurnEventHandler DoTurnPermission;
        /// <summary>
        /// Event excecuted on receiving game managment messages during the gameplay.
        /// Excecuted in another thread, different from the subscribing one.
        /// Using this event needs caution and proper thread synchronization mechanisms.
        /// </summary>
        public event GameMessageEventHandler GameMessageReceive;


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
                        PositionData[] infoTab = gameInitMessage.PositionData;
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
                    {
                        GameMessage gameMessage = message as GameMessage;
						InfoLog.WriteInfo("WARNING: playerID = " + gameMessage.IdPlayer, EPrefix.GameMessageProccesing);
                        if(GameMessageReceive != null)
                            GameMessageReceive(this, new GameMessageEventArgs(gameMessage));
                    }
                    break;

                case MessageType.DoTurn:
					//InfoLog.WriteInfo("DoTurn received", EPrefix.ClientInformation);
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
