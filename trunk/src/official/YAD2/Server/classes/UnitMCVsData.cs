using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.classes
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

        public Server.classes.UnitMCVData Add(Server.classes.UnitMCVData obj)
        {
            return UnitMCVDataCollection.Add(obj);
        }

        [XmlIgnore]
        public Server.classes.UnitMCVData this[int index]
        {
            get { return (Server.classes.UnitMCVData)UnitMCVDataCollection[index]; }
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

        public Server.classes.UnitMCVData Remove(int index)
        {
            Server.classes.UnitMCVData obj = UnitMCVDataCollection[index];
            UnitMCVDataCollection.Remove(obj);
            return obj;
        }

        public void Remove(object obj)
        {
            UnitMCVDataCollection.Remove(obj);
        }

        [XmlElement(Type = typeof(Server.classes.UnitMCVData), ElementName = "UnitMCVData", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
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
