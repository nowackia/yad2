using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.Classes
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

        public Server.Classes.UnitSandwormData Add(Server.Classes.UnitSandwormData obj)
        {
            return UnitSandwormDataCollection.Add(obj);
        }

        [XmlIgnore]
        public Server.Classes.UnitSandwormData this[int index]
        {
            get { return (Server.Classes.UnitSandwormData)UnitSandwormDataCollection[index]; }
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

        public Server.Classes.UnitSandwormData Remove(int index)
        {
            Server.Classes.UnitSandwormData obj = UnitSandwormDataCollection[index];
            UnitSandwormDataCollection.Remove(obj);
            return obj;
        }

        public void Remove(object obj)
        {
            UnitSandwormDataCollection.Remove(obj);
        }

        [XmlElement(Type = typeof(Server.Classes.UnitSandwormData), ElementName = "UnitSandwormData", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
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
