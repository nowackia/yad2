using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.Common
{
    public class ChatUser : IPlayerID
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

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            ChatUser ch = obj as ChatUser;
            if ((object)ch == null)
                return false;

            return this.id == ch.id && this.name == ch.name;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }

        public static bool operator ==(ChatUser a, ChatUser b)
        {
            if (System.Object.ReferenceEquals(a, b)) {
                return true;
            }

            if (((object)a == null) || ((object)b == null)) {
                return false;
            }
            return a.Equals((object)b);
        }

        public static bool operator !=(ChatUser a, ChatUser b)
        {
            return !(a == b);
        }

        #region IPlayerID Members

        public short GetID() {
            return id;
        }

        #endregion
    }
}
