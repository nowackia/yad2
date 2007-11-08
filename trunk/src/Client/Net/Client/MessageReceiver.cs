using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using Yad.Log;
using Yad.Net.General;
using Yad.Net.General.Messaging;

namespace Client.Net.Client
{
    public delegate void MessageEventHandler(object sender, MessageEventArgs e);
    public delegate void ConnectionLostEventHandler(object sender, EventArgs e);

    public class MessageEventArgs : EventArgs
    {
        public byte type;
        public Message message;

        public MessageEventArgs(byte type, Message message)
        {
            this.type = type;
            this.message = message;
        }
    }

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
        public ChatUser chatUser;
        public string text;

        public ChatEventArgs(ChatUser chatUser, string text)
        {
            this.chatUser = chatUser;
            this.text = text;
        }
    }

    public class MenuMessageHandlerClient : IMessageHandler
    {
        public event RequestReplyEventHandler LoginRequestReply;
        public event RequestReplyEventHandler RegisterRequestReply;
        public event RequestReplyEventHandler RemindRequestReply;

        public event ChatEventHandler AddChatUser;
        public event ChatEventHandler DeleteChatUser;
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

                default:
                    break;
            }
        }
    }

    class MessageReceiver
    {
        private Thread thread = null;
        private BinaryReader readStream;
        private volatile bool isProcessing;
        private MenuMessageHandlerClient menuMessageHandler;

        public event MessageEventHandler MessageReceive;
        public event ConnectionLostEventHandler ConnectionLost;

        public MessageReceiver(NetworkStream netStream)
        {
            readStream = new BinaryReader(netStream);
            menuMessageHandler = new MenuMessageHandlerClient();
            isProcessing = false;
        }

        public void Start()
        {
            if (isProcessing)
                throw new ThreadStateException("The Message Receiver thread is already running");

            thread = new Thread(new ThreadStart(Process));
            thread.IsBackground = true;
            thread.Start();
        }

        public void Stop()
        {
            isProcessing = false;

            readStream.Close();

            thread.Join();
        }

        public bool IsProcessing
        {
            get
            { return isProcessing; }
        }

        public MenuMessageHandlerClient MenuMessageHandler
        {
            get
            { return menuMessageHandler; }
        }

        public void Process()
        {
            byte type = (byte)MessageType.Unknown;

            isProcessing = true;

            while (true)
            {
                if (!isProcessing)
                    return;

                try
                { type = readStream.ReadByte(); }
                catch (Exception)
                {
                    if (ConnectionLost != null)
                    {
                        lock (ConnectionLost)
                        { ConnectionLost(this, EventArgs.Empty); }
                    }
                    return;
                }

                InfoLog.WriteInfo("Client received message with type " + type, EPrefix.ClientInformation);

                Message msg = MessageFactory.Create((MessageType)type);
                if (msg == null)
                {
                    InfoLog.WriteInfo("Received unknown message", EPrefix.MessageReceivedInfo);
                    continue;
                }

                msg.Deserialize(readStream);

                menuMessageHandler.ProcessMessage(msg);

                if (MessageReceive != null)
                {
                    lock (MessageReceive)
                    { MessageReceive(this, new MessageEventArgs((byte)msg.Type, msg)); }
                }


            }
        }
    }
}
