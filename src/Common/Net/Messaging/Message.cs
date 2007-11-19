using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Yad.Log.Common;

namespace Yad.Net.Messaging.Common
{
    public class Message
    {
        private MessageType type;
        private short senderId;

        public Message()
            : this(MessageType.Unknown, -1)
        { }

        public Message(MessageType msgType)
            : this(msgType, -1)
        { }

        public Message(MessageType msgType, short id)
        {
            type = msgType;
            senderId = id;
        }
        public short SenderId
        {
            get { return senderId; }
            set { senderId = value; }
        }

        protected void WriteString(string text, BinaryWriter writer)
        {
            byte b = (byte)text.Length;
            writer.Write(b);
            writer.Write(text.ToCharArray());
        }

        protected string ReadString(BinaryReader reader)
        {
            byte lenght = reader.ReadByte();
            char[] charray = reader.ReadChars(lenght);
            return new string(charray);
        }

        protected void WriteMessageHeader(MessageType type, BinaryWriter writer)
        {
            byte itype = (byte)type;
			//InfoLog.WriteInfo("Message header: " + itype);
            writer.Write(itype);
        }

        public MessageType Type
        {
            get { return type; }
            set { type = value; }
        }

        public virtual void Deserialize(BinaryReader reader)
        {
			this.senderId = reader.ReadInt16();
		}

        public virtual void Serialize(BinaryWriter writer)
        {
            WriteMessageHeader(type, writer);
			writer.Write(this.senderId);
        }
    }
}
