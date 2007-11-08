using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Yad.Log;
using Yad.Net.Messaging.Common;
using Yad.Net.Common;
using Yad.Log.Common;

namespace Client.Net
{
    class MessageSender : ListProcessor<Message>
    {
        private Thread thread = null;
        private BinaryWriter writeStream;

        public event MessageEventHandler MessageSend;
        public event ConnectionLostEventHandler ConnectionLost;

        public MessageSender()
            : this(null)
        { }

        public MessageSender(Stream stream)
            : base()
        {
            if (stream != null)
                writeStream = new BinaryWriter(stream);
        }

        public bool IsProcessing
        {
            get
            { return thread.IsAlive; }
        }

        public Stream Stream
        {
            get
            { return writeStream.BaseStream; }
            set
            { writeStream = new BinaryWriter(value); }
        }

        public void Start()
        {
            if (writeStream == null)
                throw new ArgumentNullException("Writing stream can not be null");

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
            catch (Exception ex)
            {
                InfoLog.WriteException(ex);
                if (ConnectionLost != null)
                {
                    lock (ConnectionLost)
                    { ConnectionLost(this, EventArgs.Empty); }
                }
            }
        }
    }
}
