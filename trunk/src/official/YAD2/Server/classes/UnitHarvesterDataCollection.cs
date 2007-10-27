using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.classes
{
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class UnitHarvesterDataCollection : ArrayList
    {
        public Server.classes.UnitHarvesterData Add(Server.classes.UnitHarvesterData obj)
        {
            base.Add(obj);
            return obj;
        }

        public Server.classes.UnitHarvesterData Add()
        {
            return Add(new Server.classes.UnitHarvesterData());
        }

        public void Insert(int index, Server.classes.UnitHarvesterData obj)
        {
            base.Insert(index, obj);
        }

        public void Remove(Server.classes.UnitHarvesterData obj)
        {
            base.Remove(obj);
        }

        new public Server.classes.UnitHarvesterData this[int index]
        {
            get { return (Server.classes.UnitHarvesterData)base[index]; }
            set { base[index] = value; }
        }
    }

}
