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
        
        byte _option;
        List<ChatUser> _chatUsers;

        public byte Option {
            get { return _option; }
            set { _option = value; }
        }

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
            writer.Write(_option);
            writer.Write(_chatUsers.Count);
            foreach (ChatUser cu in _chatUsers) {
                writer.Write(cu.Id);
                base.WriteString(cu.Name, writer);
            }
        }

        public override void Deserialize(System.IO.BinaryReader reader) {
            base.Deserialize(reader);
            _option = reader.ReadByte();
            int count = reader.ReadInt32();
            short id;
            string name;
            for (int i = 0; i < count; ++i) {
                id = reader.ReadInt16();
                name = base.ReadString(reader);
                _chatUsers.Add(new ChatUser(id, name));
            }
        }
    }
}
