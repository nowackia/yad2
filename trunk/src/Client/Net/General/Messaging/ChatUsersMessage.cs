using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.General.Messaging;

namespace Client.Net.General.Messaging {
    /// <summary>
    /// Format wiadomosci
    /// Typ (byte)
    /// Ilosc _chatUsers (int)
    ///  dla kazdego chatUsera
    /// id (int)
    /// dlugosc nazwy (int)
    /// nazwa (char[])
    /// </summary>
    class ChatUsersMessage : Message {
        List<ChatUser> _chatUsers;

        public ChatUsersMessage()
            : base(MessageType.ChatUsers) {
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
            int length;
            string name;
            for (int i = 0; i < count; ++i) {
                id = reader.ReadInt32();
                name = base.ReadString(reader);
                _chatUsers.Add(new ChatUser(id, name));
            }
        }
    }
}
