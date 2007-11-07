using Client.Log;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using Yad.Net.General.Messaging;

namespace Yad.Net.General
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

    class MessageReceiver
    {
        private Thread thread = null;
        private BinaryReader readStream;
        private bool isProcessing;

        public event MessageEventHandler MessageReceive;
        public event ConnectionLostEventHandler ConnectionLost;


        public MessageReceiver(NetworkStream netStream)
        {
            readStream = new BinaryReader(netStream);
            isProcessing = false;
        }

        public void Start()
        {
            if (isProcessing)
                throw new ThreadStateException("The Message Receiver thread is already running");

            thread = new Thread(new ThreadStart(Process));
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

        public void Process()
        {
            byte type;

            isProcessing = true;

            while (true)
            {
                if (!isProcessing)
                    return;

                try
                { type = readStream.ReadByte(); }
                catch (Exception)
                {
                    lock (ConnectionLost)
                    {
                        if (ConnectionLost != null)
                            ConnectionLost(this, EventArgs.Empty);
                    }
                    return;
                }

                InfoLog.WriteInfo("Client received message", EPrefix.ClientInformation);

                Message msg = MessageFactory.Create((MessageType)type);
                if (msg == null)
                {
                    InfoLog.WriteInfo("Received unknown message", EPrefix.MessageReceivedInfo);
                    continue;
                }

                msg.Deserialize(readStream);

                lock (MessageReceive)
                {
                    if (MessageReceive != null)
                        MessageReceive(this, new MessageEventArgs((byte)msg.Type, msg));
                }
            }
        }
    }
}
