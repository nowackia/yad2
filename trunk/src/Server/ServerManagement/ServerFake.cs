using Client.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Server.ServerManagement
{
    class ServerFake
    {
        private static TcpListener listener;

        private ServerFake()
        { }

        public static void Process()
        {
            listener = new TcpListener(15000);
            listener.Start();

            InfoLog.WriteInfo("Server Fake waiting for client", EPrefix.ServerInformation);

            TcpClient client = listener.AcceptTcpClient();
            BinaryReader reader = new BinaryReader(client.GetStream());

            InfoLog.WriteInfo("Server Fake begin processing", EPrefix.ServerInformation);

            while (true)
            {
                try
                {
                    byte type = reader.ReadByte();
                    InfoLog.WriteInfo("Received message type: " + type, EPrefix.ServerInformation);
                }
                catch (Exception ex)
                {
                    InfoLog.WriteInfo("Read info error " + ex.Message);
                    return;
                }
            }
        }
    }
}
