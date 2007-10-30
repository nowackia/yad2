using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.Classes
{
    [XmlType(TypeName = "UnitMCVsData", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class UnitMCVsData
    {
        [System.Runtime.InteropServices.DispIdAttribute(-4)]
        public IEnumerator GetEnumerator()
        {
            return UnitMCVDataCollection.GetEnumerator();
        }

        public Server.Classes.UnitMCVData Add(Server.Classes.UnitMCVData obj)
        {
            return UnitMCVDataCollection.Add(obj);
        }

        [XmlIgnore]
        public Server.Classes.UnitMCVData this[int index]
        {
            get { return (Server.Classes.UnitMCVData)UnitMCVDataCollection[index]; }
        }

        [XmlIgnore]
        public int Count
        {
            get { return UnitMCVDataCollection.Count; }
        }

        public void Clear()
        {
            UnitMCVDataCollection.Clear();
        }

        public Server.Classes.UnitMCVData Remove(int index)
        {
            Server.Classes.UnitMCVData obj = UnitMCVDataCollection[index];
            UnitMCVDataCollection.Remove(obj);
            return obj;
        }

        public void Remove(object obj)
        {
            UnitMCVDataCollection.Remove(obj);
        }

        [XmlElement(Type = typeof(Server.Classes.UnitMCVData), ElementName = "UnitMCVData", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public UnitMCVDataCollection __UnitMCVDataCollection;

        [XmlIgnore]
        public UnitMCVDataCollection UnitMCVDataCollection
        {
            get
            {
                if (__UnitMCVDataCollection == null) __UnitMCVDataCollection = new UnitMCVDataCollection();
                return __UnitMCVDataCollection;
            }
            set { __UnitMCVDataCollection = value; }
        }

        public UnitMCVsData()
        {
        }
    }

}
