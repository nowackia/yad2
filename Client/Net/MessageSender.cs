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
using Yad.Net.Messaging;

namespace Yad.Net.Client
{
    public class MessageSender : ListProcessor<Message>
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
            this.BeginThread();
            thread.Start();
        }

        public void Stop()
        {
            writeStream.Close();
            this.EndThread();
        }

        public override void ProcessItem(Message msg)
        {
            try
            {
                InfoLog.WriteInfo("Sending message : " + msg.Type, EPrefix.ClientInformation);
                if (msg.Type == MessageType.Build) {
                    InfoLog.WriteInfo("Sending build message: " + ((BuildMessage)msg).ToString());
                }
                if (msg.Type == MessageType.BuildUnitMessage) {
                    InfoLog.WriteInfo("Sending build unit message: " + ((BuildUnitMessage)msg).ToString());
                }
                msg.Serialize(writeStream);
                writeStream.Flush();
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
