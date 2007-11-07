using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.General.Messaging
{
    class NewChatUserMessage : Message
    {
        private string _nick;

        public string Nick
        {
            get { return _nick; }
            set { _nick = value; }
        }
        /// <summary>
        /// Userid to identyfikator uczestnika
        /// </summary>
        public NewChatUserMessage(string nick, int id)
            : base(MessageType.NewChatUser)
        {
            _nick = nick;
            UserId = id;
        }



        public override void Serialize(System.IO.BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.Write(UserId);
            WriteString(_nick, writer);
        }

        public override void Deserialize(System.IO.BinaryReader reader)
        {
            base.Deserialize(reader);
            this.UserId = reader.ReadInt32();
            _nick = ReadString(reader);
        }
    }
}
