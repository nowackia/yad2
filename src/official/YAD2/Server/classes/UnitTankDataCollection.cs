using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.Classes
{
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class UnitTankDataCollection : ArrayList
    {
        public Server.Classes.UnitTankData Add(Server.Classes.UnitTankData obj)
        {
            base.Add(obj);
            return obj;
        }

        public Server.Classes.UnitTankData Add()
        {
            return Add(new Server.Classes.UnitTankData());
        }

        public void Insert(int index, Server.Classes.UnitTankData obj)
        {
            base.Insert(index, obj);
        }

        public void Remove(Server.Classes.UnitTankData obj)
        {
            base.Remove(obj);
        }

        new public Server.Classes.UnitTankData this[int index]
        {
            get { return (Server.Classes.UnitTankData)base[index]; }
            set { base[index] = value; }
        }
    }

}
