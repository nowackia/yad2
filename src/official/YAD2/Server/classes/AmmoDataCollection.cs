using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.classes
{
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class AmmoDataCollection : ArrayList
    {
        public Server.classes.AmmoData Add(Server.classes.AmmoData obj)
        {
            base.Add(obj);
            return obj;
        }

        public Server.classes.AmmoData Add()
        {
            return Add(new Server.classes.AmmoData());
        }

        public void Insert(int index, Server.classes.AmmoData obj)
        {
            base.Insert(index, obj);
        }

        public void Remove(Server.classes.AmmoData obj)
        {
            base.Remove(obj);
        }

        new public Server.classes.AmmoData this[int index]
        {
            get { return (Server.classes.AmmoData)base[index]; }
            set { base[index] = value; }
        }
    }
}
