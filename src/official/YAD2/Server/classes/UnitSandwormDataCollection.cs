using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.classes
{
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class UnitSandwormDataCollection : ArrayList
    {
        public Server.classes.UnitSandwormData Add(Server.classes.UnitSandwormData obj)
        {
            base.Add(obj);
            return obj;
        }

        public Server.classes.UnitSandwormData Add()
        {
            return Add(new Server.classes.UnitSandwormData());
        }

        public void Insert(int index, Server.classes.UnitSandwormData obj)
        {
            base.Insert(index, obj);
        }

        public void Remove(Server.classes.UnitSandwormData obj)
        {
            base.Remove(obj);
        }

        new public Server.classes.UnitSandwormData this[int index]
        {
            get { return (Server.classes.UnitSandwormData)base[index]; }
            set { base[index] = value; }
        }
    }

}
