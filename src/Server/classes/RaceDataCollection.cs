using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.Classes
{
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class RaceDataCollection : ArrayList
    {
        public Server.Classes.RaceData Add(Server.Classes.RaceData obj)
        {
            base.Add(obj);
            return obj;
        }

        public Server.Classes.RaceData Add()
        {
            return Add(new Server.Classes.RaceData());
        }

        public void Insert(int index, Server.Classes.RaceData obj)
        {
            base.Insert(index, obj);
        }

        public void Remove(Server.Classes.RaceData obj)
        {
            base.Remove(obj);
        }

        new public Server.Classes.RaceData this[int index]
        {
            get { return (Server.Classes.RaceData)base[index]; }
            set { base[index] = value; }
        }
    }
}
