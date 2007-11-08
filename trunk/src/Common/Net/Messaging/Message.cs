using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Yad.Net.Messaging.Common
{
    public class Message
    {
        private MessageType type;
        private short playerId;

        public Message()
            : this(MessageType.Unknown, -1)
        { }

        public Message(MessageType msgType)
            : this(msgType, -1)
        { }

        public Message(MessageType msgType, int id)
        {
            type = msgType;
            userId = id;
        }
        public short PlayerId
        {
            get { return playerId; }
            set { playerId = value; }
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
            writer.Write(itype);
        }

        public MessageType Type
        {
            get { return type; }
            set { type = value; }
        }

        public virtual void Deserialize(BinaryReader reader)
        {
        }

        public virtual void Serialize(BinaryWriter writer)
        {
            WriteMessageHeader(type, writer);
        }
    }
}
