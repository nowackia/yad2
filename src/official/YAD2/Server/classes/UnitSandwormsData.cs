using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.classes
{
    [XmlType(TypeName = "UnitSandwormsData", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class UnitSandwormsData
    {
        [System.Runtime.InteropServices.DispIdAttribute(-4)]
        public IEnumerator GetEnumerator()
        {
            return UnitSandwormDataCollection.GetEnumerator();
        }

        public Server.classes.UnitSandwormData Add(Server.classes.UnitSandwormData obj)
        {
            return UnitSandwormDataCollection.Add(obj);
        }

        [XmlIgnore]
        public Server.classes.UnitSandwormData this[int index]
        {
            get { return (Server.classes.UnitSandwormData)UnitSandwormDataCollection[index]; }
        }

        [XmlIgnore]
        public int Count
        {
            get { return UnitSandwormDataCollection.Count; }
        }

        public void Clear()
        {
            UnitSandwormDataCollection.Clear();
        }

        public Server.classes.UnitSandwormData Remove(int index)
        {
            Server.classes.UnitSandwormData obj = UnitSandwormDataCollection[index];
            UnitSandwormDataCollection.Remove(obj);
            return obj;
        }

        public void Remove(object obj)
        {
            UnitSandwormDataCollection.Remove(obj);
        }

        [XmlElement(Type = typeof(Server.classes.UnitSandwormData), ElementName = "UnitSandwormData", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public UnitSandwormDataCollection __UnitSandwormDataCollection;

        [XmlIgnore]
        public UnitSandwormDataCollection UnitSandwormDataCollection
        {
            get
            {
                if (__UnitSandwormDataCollection == null) __UnitSandwormDataCollection = new UnitSandwormDataCollection();
                return __UnitSandwormDataCollection;
            }
            set { __UnitSandwormDataCollection = value; }
        }

        public UnitSandwormsData()
        {
        }
    }

}
