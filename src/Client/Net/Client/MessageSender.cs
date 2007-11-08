using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Yad.Log;
using Yad.Net.General;
using Yad.Net.General.Messaging;

namespace Client.Net.Client
{
    class MessageSender : ListProcessor<Message>
    {
        private Thread thread = null;
        private BinaryWriter writeStream;

        public event MessageEventHandler MessageSend;
        public event ConnectionLostEventHandler ConnectionLost;

        public MessageSender(NetworkStream netStream)
            : base()
        {
            writeStream = new BinaryWriter(netStream);
        }

        public bool IsProcessing
        {
            get
            { return thread.IsAlive; }
        }

        public void Start()
        {
            thread = new Thread(new ThreadStart(Process));
            thread.IsBackground = true;
            thread.Start();
        }

        public void Stop()
        {
            writeStream.Close();
            this.EndThread();
            thread.Join();
        }

        public override void ProcessItem(Message msg)
        {
            try
            {
                InfoLog.WriteInfo("Sending message : " + (byte)msg.Type, EPrefix.ClientInformation);
                msg.Serialize(writeStream);

                if (MessageSend != null)
                {
                    lock (MessageSend)
                    { MessageSend(this, new MessageEventArgs((byte)msg.Type, msg)); }
                }
            }
            catch (Exception)
            {
                if (ConnectionLost != null)
                {
                    lock (ConnectionLost)
                    { ConnectionLost(this, EventArgs.Empty); }
                }
            }
        }
    }
}
