using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.General.Messaging;

namespace Client.Net.General.Messaging {
    class LoginMessage : Message {
        string _login;
        string _password;

        public string Login {
            get { return _login; }
            set { _login = value; }
        }
        
        public string Password {
            get { return _password; }
            set { _password = value; }
        }

        public override void Serialize(System.IO.BinaryWriter writer) {
            base.Serialize(writer);
            Message.WriteString(_login, writer);
            Message.WriteString(_password, writer);
        }

        public override void Deserialize(System.IO.BinaryReader reader) {
            _login = Message.ReadString(reader);
            _password = Message.ReadString(reader);

        }
    }
}
