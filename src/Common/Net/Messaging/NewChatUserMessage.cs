using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.Messaging.Common
{
    public class NewChatUserMessage : Message
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
        public NewChatUserMessage(string nick, short id)
            : base(MessageType.NewChatUser)
        {
            _nick = nick;
            PlayerId = id;
        }



        public override void Serialize(System.IO.BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.Write(PlayerId);
            WriteString(_nick, writer);
        }

        public override void Deserialize(System.IO.BinaryReader reader)
        {
            base.Deserialize(reader);
            this.PlayerId = reader.ReadInt16();
            _nick = ReadString(reader);
        }
    }
}
