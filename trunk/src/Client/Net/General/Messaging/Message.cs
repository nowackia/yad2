using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Yad.Net.General.Messaging
{
    public class Message
    {
        private MessageType type;
        private int userId;

        public Message(MessageType msgType) {
            type = msgType;
        }

        public Message() {
            type = MessageType.Unknown;
        }

        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        protected void WriteString(string text, BinaryWriter writer)
        {
            byte b = (byte)text.Length;
            writer.Write(b);
            writer.Write(text.ToCharArray());
        }

        protected string ReadString(BinaryReader reader) {
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

        public virtual void Deserialize(BinaryReader reader) {
        }

        public virtual void Serialize(BinaryWriter writer) {
            WriteMessageHeader(type, writer);
        }
    }
}
