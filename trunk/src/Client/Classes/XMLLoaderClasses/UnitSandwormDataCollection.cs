using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.Classes
{
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class UnitSandwormDataCollection : ArrayList
    {
        public Server.Classes.UnitSandwormData Add(Server.Classes.UnitSandwormData obj)
        {
            base.Add(obj);
            return obj;
        }

        public Server.Classes.UnitSandwormData Add()
        {
            return Add(new Server.Classes.UnitSandwormData());
        }

        public void Insert(int index, Server.Classes.UnitSandwormData obj)
        {
            base.Insert(index, obj);
        }

        public void Remove(Server.Classes.UnitSandwormData obj)
        {
            base.Remove(obj);
        }

        new public Server.Classes.UnitSandwormData this[int index]
        {
            get { return (Server.Classes.UnitSandwormData)base[index]; }
            set { base[index] = value; }
        }
    }

}
