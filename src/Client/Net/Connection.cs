using Client.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Yad.Net.General.Messaging;

namespace Yad.Net
{
    class Connection
    {
        private static TcpClient tcpClient;
        private static BinaryWriter writer;

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
                writer = new BinaryWriter(tcpClient.GetStream());
            }
            catch (SocketException ex)
            {
                InfoLog.WriteInfo("Connection failed with error number " + ex.ErrorCode, EPrefix.ClientInformation);
            }
        }
       
        public static void CloseConnection()
        {
            writer.Close();
            tcpClient.Close();
        }

        public static void SendMessage(Message message)
        {
            if(tcpClient.Connected && writer != null)
                message.Serialize(writer);
        }
    }
}
