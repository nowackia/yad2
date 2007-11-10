using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Common;
using Yad.Net.Messaging.Common;

namespace Client.Net
{
    public delegate void RequestReplyEventHandler(object sender, RequestReplyEventArgs e);
    public delegate void ChatEventHandler(object sender, ChatEventArgs e);

    public class RequestReplyEventArgs : EventArgs
    {
        public bool isSuccessful;
        public string reason;

        public RequestReplyEventArgs(bool isSuccessful, string reason)
        {
            this.isSuccessful = isSuccessful;
            this.reason = reason;
        }
    }

    public class ChatEventArgs : EventArgs
    {
        public ChatUser[] chatUsers;
        public string text;

        public ChatEventArgs(string text)
        {
            chatUsers = null;
            this.text = text;
        }

        public ChatEventArgs(ChatUser chatUser, string text)
            : this(new ChatUser[] { chatUser }, text)
        { }

        public ChatEventArgs(ChatUser[] chatUsers, string text)
        {
            this.chatUsers = chatUsers;
            this.text = text;
        }
    }

    public class MenuMessageHandler : IMessageHandler
    {
        public event RequestReplyEventHandler LoginRequestReply;
        public event RequestReplyEventHandler RegisterRequestReply;
        public event RequestReplyEventHandler RemindRequestReply;

        public event ChatEventHandler NewChatUsers;
        public event ChatEventHandler DeleteChatUsers;
        public event ChatEventHandler ResetChatUsers;
        public event ChatEventHandler ChatTextReceive;

        public void ProcessMessage(Message message)
        {
            switch (message.Type)
            {
                case MessageType.LoginSuccessful:
                    if (LoginRequestReply != null)
                        LoginRequestReply(this, new RequestReplyEventArgs(true, "Login successful"));
                    break;

                case MessageType.LoginUnsuccessful:
                    if (LoginRequestReply != null)
                        LoginRequestReply(this, new RequestReplyEventArgs(true, ((TextMessage)message).Text));
                    break;

                case MessageType.RegisterSuccessful:
                    if (RegisterRequestReply != null)
                        RegisterRequestReply(this, new RequestReplyEventArgs(true, "Register successful"));
                    break;

                case MessageType.RegisterUnsuccessful:
                    if (RegisterRequestReply != null)
                        RegisterRequestReply(this, new RequestReplyEventArgs(true, ((TextMessage)message).Text));
                    break;

                case MessageType.RemindSuccessful:
                    if (RemindRequestReply != null)
                        RemindRequestReply(this, new RequestReplyEventArgs(true, "Remind successful"));
                    break;

                case MessageType.RemindUnsuccessful:
                    if (RemindRequestReply != null)
                        RemindRequestReply(this, new RequestReplyEventArgs(true, ((TextMessage)message).Text));
                    break;

                case MessageType.ChatUsers:
                    if (ResetChatUsers != null)
                    {
                        ChatUsersMessage chatUsersMessage = message as ChatUsersMessage;
                        ResetChatUsers(this, new ChatEventArgs(chatUsersMessage.ChatUsers.ToArray(), string.Empty));
                    }
                    break;

                case MessageType.NewChatUser:
                    if (NewChatUsers != null)
                    {
                        NewChatUserMessage chatUsersMessage = message as NewChatUserMessage;
                        NewChatUsers(this, new ChatEventArgs(new ChatUser(chatUsersMessage.PlayerId, chatUsersMessage.Nick), string.Empty));
                    }
                    break;

                case MessageType.DeleteChatUser:
                    //if (DeleteChatUsers != null)
                    //    DeleteChatUsers(this, new ChatEventArgs(this, string.Empty));
                    break;

                case MessageType.ChatText:
                    if (ChatTextReceive != null)
                    {
                        TextMessage textMessage = message as TextMessage;
                        ChatTextReceive(this, new ChatEventArgs(textMessage.PlayerId + " " + textMessage.Text));
                    }
                    break;

                //case MessageType.

                default:
                    break;
            }
        }
    }
}