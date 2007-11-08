
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Yad.Log;
using Yad.Net.Common;
using Yad.Net.Messaging.Common;
using Yad.Log.Common;

namespace Yad.Net.Client
{
    class Connection
    {
        private static TcpClient tcpClient;

        private static MessageReceiver receiver;
        private static MessageSender sender;

        private Connection()
        { }

        static Connection()
        {
            tcpClient = new TcpClient();
        }

        public static void InitConnection(string hostname, int port)
        {
            try
            {
                InfoLog.WriteInfo("Connecting to " + hostname + " on port " + port + " ...", EPrefix.ClientInformation); 
                tcpClient.Connect(hostname, port);
                InfoLog.WriteInfo("Connected succesfully", EPrefix.ClientInformation);

                receiver = new MessageReceiver(tcpClient.GetStream());
                receiver.Start();
                InfoLog.WriteInfo("Receiver run succesfully", EPrefix.ClientInformation);

                sender = new MessageSender(tcpClient.GetStream());
                sender.Start();
                InfoLog.WriteInfo("Sender run succesfully", EPrefix.ClientInformation);
            }
            catch (Exception ex)
            {
                InfoLog.WriteException(ex);
            }
        }
       
        public static bool Connected
        {
            get
            { return tcpClient.Connected; }
        }

        public static MenuMessageHandlerClient MenuMessageHandler
        {
            get
            { return receiver.MenuMessageHandler; }
        }

        public event ConnectionLostEventHandler ConnectionLost
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

        public event MessageEventHandler MessageReceive
        {
            add
            { receiver.MessageReceive += value; }
            remove
            { receiver.MessageReceive += value; }
        }

        public event MessageEventHandler MessageSend
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
                receiver.Stop();
                tcpClient.Close();
            }
        }

        public static void SendMessage(Message message)
        {
            if (tcpClient.Connected)
                sender.AddItem(message);
        }
    }
}
