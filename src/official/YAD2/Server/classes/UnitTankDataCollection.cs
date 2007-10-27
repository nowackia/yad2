using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.classes
{
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class UnitTankDataCollection : ArrayList
    {
        public Server.classes.UnitTankData Add(Server.classes.UnitTankData obj)
        {
            base.Add(obj);
            return obj;
        }

        public Server.classes.UnitTankData Add()
        {
            return Add(new Server.classes.UnitTankData());
        }

        public void Insert(int index, Server.classes.UnitTankData obj)
        {
            base.Insert(index, obj);
        }

        public void Remove(Server.classes.UnitTankData obj)
        {
            base.Remove(obj);
        }

        new public Server.classes.UnitTankData this[int index]
        {
            get { return (Server.classes.UnitTankData)base[index]; }
            set { base[index] = value; }
        }
    }

}
