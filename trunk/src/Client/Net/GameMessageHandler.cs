using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Common;
using Yad.Net.Messaging.Common;
using Yad.Log.Common;

namespace Yad.Net.Client
{
    class GameMessageHandler : IMessageHandler
    {
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
                    break;

                case MessageType.Move:
                    break;
        
                case MessageType.Destroy:
                    break;

                case MessageType.CreateUnit:
                    break;

                case MessageType.Build:
                    break;

                case MessageType.Harvest:
                    break;

                case MessageType.Attack:
                    break;

                case MessageType.Control:
                    break;

                case MessageType.DoTurn:
                    break;

                default:
                    InfoLog.WriteInfo("GameMessageHandler received unknown message type: " + message.Type, EPrefix.ClientInformation);
                    break;
            }
        }
    }
}
