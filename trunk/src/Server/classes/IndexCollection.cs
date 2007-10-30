using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.Classes
{
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class IndexCollection : ArrayList
    {
        public int Add(int obj)
        {
            base.Add(obj);
            return obj;
        }

        public int Add()
        {
            return Add(new int());
        }

        public void Insert(int index, int obj)
        {
            base.Insert(index, obj);
        }

        public void Remove(int obj)
        {
            base.Remove(obj);
        }

        new public int this[int index]
        {
            get { return (int)base[index]; }
            set { base[index] = value; }
        }
    }

}
