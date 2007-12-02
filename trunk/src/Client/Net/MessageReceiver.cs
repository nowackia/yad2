using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using Yad.Log;
using Yad.Net.Common;
using Yad.Net.Messaging.Common;
using Yad.Log.Common;

namespace Yad.Net.Client
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

    public class MessageReceiver
    {
        private Thread thread = null;
        private BinaryReader readStream;
        private volatile bool isProcessing;
        private IMessageHandler messageHandler;

        private object lockObject = new object();

        public event MessageEventHandler MessageReceive;
        public event ConnectionLostEventHandler ConnectionLost;

        public MessageReceiver()
            : this(null)
        { }

        public MessageReceiver(Stream stream)
        {
            if(stream != null)
                readStream = new BinaryReader(stream);
            isProcessing = false;
        }

        public void Start()
        {
            if (isProcessing)
                throw new ThreadStateException("The Message Receiver thread is already running");
            if (readStream == null)
                throw new ArgumentNullException("Reading stream can not be null");

            thread = new Thread(new ThreadStart(Process));
            thread.IsBackground = true;
            thread.Start();
        }

        public void Stop()
        {
            isProcessing = false;
            readStream.Close();

            InfoLog.WriteInfo("Receiver stopped", EPrefix.ClientInformation);
        }

        public bool IsProcessing
        {
            get
            { return isProcessing; }
        }

        public Stream Stream
        {
            get
            { return readStream.BaseStream; }
            set
            { readStream = new BinaryReader(value); }
        }

        public IMessageHandler MessageHandler
        {
            get
            {
                lock (lockObject)
                { return messageHandler; }
            }
            set
            {
                lock (lockObject)
                { messageHandler = value; }
            }
        }

        public void Process()
        {
            byte type = (byte)MessageType.Unknown;

            isProcessing = true;

            InfoLog.WriteInfo("Receiver run succesfully", EPrefix.ClientInformation);

            while (true)
            {
                if (!isProcessing)
                    return;

                try
                { type = readStream.ReadByte(); }
                catch (Exception ex)
                {
                    InfoLog.WriteException(ex);
                    if (ConnectionLost != null && isProcessing)
                    {
                        lock (ConnectionLost)
                        { ConnectionLost(this, EventArgs.Empty); }
                    }
                    return;
                }

                Message msg = MessageFactory.Create((MessageType)type);
                if (msg == null)
                    InfoLog.WriteInfo("Received unknown message", EPrefix.MessageReceivedInfo);
                else
                {
                    InfoLog.WriteInfo("Client received msg with type: " + msg.Type, EPrefix.ClientInformation);

                    try
                    { msg.Deserialize(readStream); }
                    catch (Exception ex)
                    {
                        InfoLog.WriteException(ex);
                        if (ConnectionLost != null)
                        {
                            lock (ConnectionLost)
                            { ConnectionLost(this, EventArgs.Empty); }
                        }
                        return;
                    }

                    if (messageHandler != null)
                        messageHandler.ProcessMessage(msg);

                    if (MessageReceive != null)
                    {
                        lock (MessageReceive)
                        { MessageReceive(this, new MessageEventArgs((byte)msg.Type, msg)); }
                    }
                }
            }
            InfoLog.WriteInfo("Receiver finished its work", EPrefix.ClientInformation);
        }
	}
}
