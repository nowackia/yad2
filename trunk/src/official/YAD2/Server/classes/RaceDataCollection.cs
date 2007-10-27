using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.classes
{
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class RaceDataCollection : ArrayList
    {
        public Server.classes.RaceData Add(Server.classes.RaceData obj)
        {
            base.Add(obj);
            return obj;
        }

        public Server.classes.RaceData Add()
        {
            return Add(new Server.classes.RaceData());
        }

        public void Insert(int index, Server.classes.RaceData obj)
        {
            base.Insert(index, obj);
        }

        public void Remove(Server.classes.RaceData obj)
        {
            base.Remove(obj);
        }

        new public Server.classes.RaceData this[int index]
        {
            get { return (Server.classes.RaceData)base[index]; }
            set { base[index] = value; }
        }
    }
}
