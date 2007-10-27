using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;
namespace Server.classes
{
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class UnitMCVDataCollection : ArrayList
    {
        public Server.classes.UnitMCVData Add(Server.classes.UnitMCVData obj)
        {
            base.Add(obj);
            return obj;
        }

        public Server.classes.UnitMCVData Add()
        {
            return Add(new Server.classes.UnitMCVData());
        }

        public void Insert(int index, Server.classes.UnitMCVData obj)
        {
            base.Insert(index, obj);
        }

        public void Remove(Server.classes.UnitMCVData obj)
        {
            base.Remove(obj);
        }

        new public Server.classes.UnitMCVData this[int index]
        {
            get { return (Server.classes.UnitMCVData)base[index]; }
            set { base[index] = value; }
        }
    }
}
