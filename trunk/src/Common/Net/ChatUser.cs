using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.Common
{
    public class ChatUser
    {
        private short id;
        private string name;

        public short Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public ChatUser(short id)
            : this(id, String.Empty)
        { }

        public ChatUser(short id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
