using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.Classes
{
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class AmmoDataCollection : ArrayList
    {
        public Server.Classes.AmmoData Add(Server.Classes.AmmoData obj)
        {
            base.Add(obj);
            return obj;
        }

        public Server.Classes.AmmoData Add()
        {
            return Add(new Server.Classes.AmmoData());
        }

        public void Insert(int index, Server.Classes.AmmoData obj)
        {
            base.Insert(index, obj);
        }

        public void Remove(Server.Classes.AmmoData obj)
        {
            base.Remove(obj);
        }

        new public Server.Classes.AmmoData this[int index]
        {
            get { return (Server.Classes.AmmoData)base[index]; }
            set { base[index] = value; }
        }
    }
}
