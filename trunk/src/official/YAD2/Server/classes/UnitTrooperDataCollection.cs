using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;


namespace Server.classes
{
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class UnitTrooperDataCollection : ArrayList
    {
        public Server.classes.UnitTrooperData Add(Server.classes.UnitTrooperData obj)
        {
            base.Add(obj);
            return obj;
        }

        public Server.classes.UnitTrooperData Add()
        {
            return Add(new Server.classes.UnitTrooperData());
        }

        public void Insert(int index, Server.classes.UnitTrooperData obj)
        {
            base.Insert(index, obj);
        }

        public void Remove(Server.classes.UnitTrooperData obj)
        {
            base.Remove(obj);
        }

        new public Server.classes.UnitTrooperData this[int index]
        {
            get { return (Server.classes.UnitTrooperData)base[index]; }
            set { base[index] = value; }
        }
    }

}
