using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Common;

namespace Yad.Net.Messaging.Common {
    /// <summary>
    /// Format wiadomosci
    /// Typ (byte)
    /// Ilosc _chatUsers (int)
    ///  dla kazdego chatUsera
    /// id (int)
    /// dlugosc nazwy (int)
    /// nazwa (char[])
    /// </summary>
    public class ChatUsersMessage : Message {
        List<ChatUser> _chatUsers;

        public List<ChatUser> ChatUsers
        {
            get { return _chatUsers; }
            set { _chatUsers = value; }
        }

        public ChatUsersMessage(MessageType msgType)
            : base(msgType) {
            _chatUsers = new List<ChatUser>();
        }
     

        public override void Serialize(System.IO.BinaryWriter writer) {
            base.Serialize(writer);
            writer.Write(_chatUsers.Count);
            foreach (ChatUser cu in _chatUsers) {
                writer.Write(cu.Id);
                base.WriteString(cu.Name, writer);
            }
        }

        public override void Deserialize(System.IO.BinaryReader reader) {
            base.Deserialize(reader);
            int count = reader.ReadInt32();
            int id;
            string name;
            for (int i = 0; i < count; ++i) {
                id = reader.ReadInt32();
                name = base.ReadString(reader);
                _chatUsers.Add(new ChatUser(id, name));
            }
        }
    }
}
