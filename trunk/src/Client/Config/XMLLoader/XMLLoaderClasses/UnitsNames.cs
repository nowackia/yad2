using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.Classes
{
    [XmlType(TypeName = "UnitsNames", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class UnitsNames
    {
        [System.Runtime.InteropServices.DispIdAttribute(-4)]
        public IEnumerator GetEnumerator()
        {
            return nameCollection.GetEnumerator();
        }

        public string Add(string obj)
        {
            return nameCollection.Add(obj);
        }

        [XmlIgnore]
        public string this[int index]
        {
            get { return (string)nameCollection[index]; }
        }

        [XmlIgnore]
        public int Count
        {
            get { return nameCollection.Count; }
        }

        public void Clear()
        {
            nameCollection.Clear();
        }

        public string Remove(int index)
        {
            string obj = nameCollection[index];
            nameCollection.Remove(obj);
            return obj;
        }

        public void Remove(object obj)
        {
            nameCollection.Remove(obj);
        }

        [XmlElement(Type = typeof(string), ElementName = "Name", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public NameCollection __nameCollection;

        [XmlIgnore]
        public NameCollection nameCollection
        {
            get
            {
                if (__nameCollection == null) __nameCollection = new NameCollection();
                return __nameCollection;
            }
            set { __nameCollection = value; }
        }

        public UnitsNames()
        {
        }
    }

}
