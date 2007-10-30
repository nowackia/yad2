using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.Classes
{
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class UnitTrooperDataCollection : ArrayList
    {
        public Server.Classes.UnitTrooperData Add(Server.Classes.UnitTrooperData obj)
        {
            base.Add(obj);
            return obj;
        }

        public Server.Classes.UnitTrooperData Add()
        {
            return Add(new Server.Classes.UnitTrooperData());
        }

        public void Insert(int index, Server.Classes.UnitTrooperData obj)
        {
            base.Insert(index, obj);
        }

        public void Remove(Server.Classes.UnitTrooperData obj)
        {
            base.Remove(obj);
        }

        new public Server.Classes.UnitTrooperData this[int index]
        {
            get { return (Server.Classes.UnitTrooperData)base[index]; }
            set { base[index] = value; }
        }
    }

}
