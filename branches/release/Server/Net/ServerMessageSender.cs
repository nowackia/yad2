using System;
using System.Collections.Generic;
using System.Text;
using Yad.Log;
using Yad.Net.Common;
using Yad.Net.Messaging.Common;
using Yad.Log.Common;
using Yad.Net.Utils;

namespace Yad.Net.Server {
    public class ServerMessageSender : ThreadListProcessor<PostMessage>, IMessageSender {
        IPlayerProvider _pprovider = null;
        
        public void AddProvider(IPlayerProvider Provider) {
            _pprovider = Provider;
        }

        public void BroadcastMessage(Message msg) {
            if (_pprovider != null) {
                lock (_pprovider.PlayerLock) {
                    Player[] players = _pprovider.GetPlayersArray();
                    for (int i = 0; i < players.Length; ++i)
                        players[i].SendMessage(msg);

                }
            }
            else
                InfoLog.WriteInfo("Message broadcast unsuccessful.", EPrefix.ServerSendMessageInfo);
        }

        public void SendMessage(Message msg, short recipient) {
            if (_pprovider != null) {
                Player p = null;
                lock (_pprovider.PlayerLock) {
                    p = _pprovider.GetPlayer(recipient);
                    p.SendMessage(msg);
                    InfoLog.WriteInfo("Message type: " + msg.Type + " has been send to user " + p.Id,
                    EPrefix.ServerSendMessageInfo);
                }
                
            }
            else
                InfoLog.WriteInfo("Message sent unsuccessful.", EPrefix.ServerSendMessageInfo);
        }

        public override void ProcessItem(PostMessage item) {
            if (item.Recipient == PostMessage.BroadCastMessage)
                BroadcastMessage(item.Message);
            else
                SendMessage(item.Message, item.Recipient);
        }

        #region IMessageSender Members

        public void MessagePost(Message msg, short recipient) {
            PostMessage pmsg = new PostMessage();
            pmsg.Recipient = recipient;
            pmsg.Message = msg;
            pmsg.Priority = 1;
            AddItem(pmsg);
        }

        #endregion
    }
}
