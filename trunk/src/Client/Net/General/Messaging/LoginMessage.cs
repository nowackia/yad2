using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Yad.Net.General.Messaging;

namespace Yad.Net.General.Messaging
{
    class LoginMessage : Message
    {
        private string login;
        private string password;

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

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            base.WriteString(login, writer);
            base.WriteString(password, writer);
        }

        public override void Deserialize(BinaryReader reader)
        {
            base.Deserialize(reader);
            login = base.ReadString(reader);
            password = base.ReadString(reader);
        }
    }
}
