using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.Classes
{
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class UnitMCVDataCollection : ArrayList
    {
        public Server.Classes.UnitMCVData Add(Server.Classes.UnitMCVData obj)
        {
            base.Add(obj);
            return obj;
        }

        public Server.Classes.UnitMCVData Add()
        {
            return Add(new Server.Classes.UnitMCVData());
        }

        public void Insert(int index, Server.Classes.UnitMCVData obj)
        {
            base.Insert(index, obj);
        }

        public void Remove(Server.Classes.UnitMCVData obj)
        {
            base.Remove(obj);
        }

        new public Server.Classes.UnitMCVData this[int index]
        {
            get { return (Server.Classes.UnitMCVData)base[index]; }
            set { base[index] = value; }
        }
    }
}
