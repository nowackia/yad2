using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Yad.Net.Messaging.Common
{
    public class RegisterMessage : Message
    {
        private string login;
        private string password;
        private string mail;

        public RegisterMessage()
            : base(MessageType.Register)
        { }

        public RegisterMessage(MessageType msgType)
            : base(msgType)
        { }

        public string Login
        {
            get { return login; }
            set { login = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string Mail
        {
            get { return mail; }
            set { mail = value; }
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            base.WriteString(login, writer);
            base.WriteString(password, writer);
            base.WriteString(mail, writer);
        }

        public override void Deserialize(BinaryReader reader)
        {
            base.Deserialize(reader);
            login = base.ReadString(reader);
            password = base.ReadString(reader);
            mail = base.ReadString(reader);
        }
    }
}
