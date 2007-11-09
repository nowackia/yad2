using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Yad.Log;
using Yad.Net.Common;
using Yad.Net.Messaging.Common;
using Yad.Log.Common;

namespace Client.Net
{
    static class Connection
    {
        private static TcpClient tcpClient;

        private static MessageReceiver receiver;
        private static MessageSender sender;

        static Connection()
        {
            receiver = new MessageReceiver();
            sender = new MessageSender();
            tcpClient = new TcpClient();
        }

        public static void InitConnection(string hostname, int port)
        {
            if (tcpClient.Connected)
                InfoLog.WriteInfo("Already connected", EPrefix.ClientInformation);
            else
            {
                try
                {
                    InfoLog.WriteInfo("Connecting to " + hostname + " on port " + port + " ...", EPrefix.ClientInformation);
                    tcpClient.Connect(hostname, port);
                    InfoLog.WriteInfo("Connected succesfully", EPrefix.ClientInformation);

                    sender.Stream = tcpClient.GetStream();
                    sender.Start();
                    InfoLog.WriteInfo("Sender run succesfully", EPrefix.ClientInformation);

                    receiver.Stream = tcpClient.GetStream();
                    receiver.Start();
                }
                catch (Exception ex)
                {
                    InfoLog.WriteException(ex);
                }
            }
        }
       
        public static bool Connected
        {
            get
            { return tcpClient.Connected; }
        }

        public static event ConnectionLostEventHandler ConnectionLost
        {
            add
            {
                sender.ConnectionLost += value;
                receiver.ConnectionLost += value;
            }
            remove
            {
                sender.ConnectionLost -= value;
                receiver.ConnectionLost -= value;
            }
        }

        public static event MessageEventHandler MessageReceive
        {
            add
            { receiver.MessageReceive += value; }
            remove
            { receiver.MessageReceive += value; }
        }

        public static event MessageEventHandler MessageSend
        {
            add
            { sender.MessageSend += value; }
            remove
            { sender.MessageSend += value; }
        }

        public static void CloseConnection()
        {
            if (tcpClient.Connected)
            {
                sender.Stop();
                receiver.Stop();
                tcpClient.Close();
                tcpClient = new TcpClient();
            }
        }

        public static IMessageHandler MessageHandler
        {
            get
            { return receiver.MessageHandler; }
            set
            { receiver.MessageHandler = value; }
        }

        public static void SendMessage(Message message)
        {
            if (tcpClient.Connected)
                sender.AddItem(message);
        }
    }
}
