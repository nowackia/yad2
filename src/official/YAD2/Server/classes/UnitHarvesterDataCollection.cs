using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.Classes
{
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class UnitHarvesterDataCollection : ArrayList
    {
        public Server.Classes.UnitHarvesterData Add(Server.Classes.UnitHarvesterData obj)
        {
            base.Add(obj);
            return obj;
        }

        public Server.Classes.UnitHarvesterData Add()
        {
            return Add(new Server.Classes.UnitHarvesterData());
        }

        public void Insert(int index, Server.Classes.UnitHarvesterData obj)
        {
            base.Insert(index, obj);
        }

        public void Remove(Server.Classes.UnitHarvesterData obj)
        {
            base.Remove(obj);
        }

        new public Server.Classes.UnitHarvesterData this[int index]
        {
            get { return (Server.Classes.UnitHarvesterData)base[index]; }
            set { base[index] = value; }
        }
    }

}
