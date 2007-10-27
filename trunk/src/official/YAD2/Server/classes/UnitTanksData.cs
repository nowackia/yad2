using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.classes
{
    [XmlType(TypeName = "UnitTanksData", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class UnitTanksData
    {
        [System.Runtime.InteropServices.DispIdAttribute(-4)]
        public IEnumerator GetEnumerator()
        {
            return UnitTankDataCollection.GetEnumerator();
        }

        public Server.classes.UnitTankData Add(Server.classes.UnitTankData obj)
        {
            return UnitTankDataCollection.Add(obj);
        }

        [XmlIgnore]
        public Server.classes.UnitTankData this[int index]
        {
            get { return (Server.classes.UnitTankData)UnitTankDataCollection[index]; }
        }

        [XmlIgnore]
        public int Count
        {
            get { return UnitTankDataCollection.Count; }
        }

        public void Clear()
        {
            UnitTankDataCollection.Clear();
        }

        public Server.classes.UnitTankData Remove(int index)
        {
            Server.classes.UnitTankData obj = UnitTankDataCollection[index];
            UnitTankDataCollection.Remove(obj);
            return obj;
        }

        public void Remove(object obj)
        {
            UnitTankDataCollection.Remove(obj);
        }

        [XmlElement(Type = typeof(Server.classes.UnitTankData), ElementName = "UnitTankData", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public UnitTankDataCollection __UnitTankDataCollection;

        [XmlIgnore]
        public UnitTankDataCollection UnitTankDataCollection
        {
            get
            {
                if (__UnitTankDataCollection == null) __UnitTankDataCollection = new UnitTankDataCollection();
                return __UnitTankDataCollection;
            }
            set { __UnitTankDataCollection = value; }
        }

        public UnitTanksData()
        {
        }
    }

}
