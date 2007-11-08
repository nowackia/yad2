using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.Common
{
    public class ChatUser
    {
        int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public ChatUser(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
