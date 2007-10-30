using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.Classes
{
    [XmlType(TypeName = "RideableFields", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class RideableFields
    {
        [System.Runtime.InteropServices.DispIdAttribute(-4)]
        public IEnumerator GetEnumerator()
        {
            return indexCollection.GetEnumerator();
        }

        public int Add(int obj)
        {
            return indexCollection.Add(obj);
        }

        [XmlIgnore]
        public int this[int index]
        {
            get { return (int)indexCollection[index]; }
        }

        [XmlIgnore]
        public int Count
        {
            get { return indexCollection.Count; }
        }

        public void Clear()
        {
            indexCollection.Clear();
        }

        public int Remove(int index)
        {
            int obj = indexCollection[index];
            indexCollection.Remove(obj);
            return obj;
        }

        public void Remove(object obj)
        {
            indexCollection.Remove(obj);
        }

        [XmlElement(Type = typeof(int), ElementName = "index", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "int", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IndexCollection __indexCollection;

        [XmlIgnore]
        public IndexCollection indexCollection
        {
            get
            {
                if (__indexCollection == null) __indexCollection = new IndexCollection();
                return __indexCollection;
            }
            set { __indexCollection = value; }
        }

        public RideableFields()
        {
        }
    }

}
