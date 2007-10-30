using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.Classes
{
    [XmlType(TypeName = "BuildingsNames", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class BuildingsNames
    {
        [System.Runtime.InteropServices.DispIdAttribute(-4)]
        public IEnumerator GetEnumerator()
        {
            return NameCollection.GetEnumerator();
        }

        public string Add(string obj)
        {
            return NameCollection.Add(obj);
        }

        [XmlIgnore]
        public string this[int index]
        {
            get { return (string)NameCollection[index]; }
        }

        [XmlIgnore]
        public int Count
        {
            get { return NameCollection.Count; }
        }

        public void Clear()
        {
            NameCollection.Clear();
        }

        public string Remove(int index)
        {
            string obj = NameCollection[index];
            NameCollection.Remove(obj);
            return obj;
        }

        public void Remove(object obj)
        {
            NameCollection.Remove(obj);
        }

        [XmlElement(Type = typeof(string), ElementName = "Name", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public NameCollection __NameCollection;

        [XmlIgnore]
        public NameCollection NameCollection
        {
            get
            {
                if (__NameCollection == null) __NameCollection = new NameCollection();
                return __NameCollection;
            }
            set { __NameCollection = value; }
        }

        public BuildingsNames()
        {
        }
    }

}
