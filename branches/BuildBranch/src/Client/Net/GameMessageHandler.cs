using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Yad.Log.Common;
using Yad.Net.Common;
using Yad.Net.Messaging.Common;
using Yad.UI.Client;
using Yad.Net.Messaging;

namespace Yad.Net.Client
{
    public delegate void GameInitEventHandler(object sender, GameInitEventArgs e);
    public delegate void GameMessageEventHandler(object sender, GameMessage gameMessage);
    public delegate void DoTurnEventHandler(object sender, DoTurnMessage dtm);

    public class GameInitEventArgs : EventArgs
    {
        public PositionData[] gameInitInfo;

        public GameInitEventArgs(PositionData[] gameInitInfo)
        {
            this.gameInitInfo = gameInitInfo;
        }
    }

    public class GameMessageHandler : IMessageHandler, ISuspender
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
        /// Not using the EventArgs convention because of speed purposes.
        /// </summary>
        public event GameMessageEventHandler GameMessageReceive;


        private static GameMessageHandler instance = new GameMessageHandler();

        private Semaphore handlerSuspender = new Semaphore(1, 1);

        public static GameMessageHandler Instance
        {
            get
            { return instance; }
        }

        public void Suspend()
        {
            handlerSuspender.WaitOne();
        }

        public void Resume()
        {
            handlerSuspender.Release();
        }

        public void ProcessMessage(Message message)
        {
            handlerSuspender.WaitOne();
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
                case MessageType.DeployMCV:
                    if (GameMessageReceive != null)
                        GameMessageReceive(this, (GameMessage)message);
                    break;

                case MessageType.DoTurn:
                    if (DoTurnPermission != null)
                        DoTurnPermission(this, (DoTurnMessage)message);
                    break;

                default:
                    InfoLog.WriteInfo("GameMessageHandler received unknown message type: " + message.Type, EPrefix.ClientInformation);
                    break;
            }
            handlerSuspender.Release();
        }
    }
}
