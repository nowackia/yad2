using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.Classes
{
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class BuildingNameCollection : ArrayList
    {
        public string Add(string obj)
        {
            base.Add(obj);
            return obj;
        }

        public void Insert(int index, string obj)
        {
            base.Insert(index, obj);
        }

        public void Remove(string obj)
        {
            base.Remove(obj);
        }

        new public string this[int index]
        {
            get { return (string)base[index]; }
            set { base[index] = value; }
        }
    }

}
