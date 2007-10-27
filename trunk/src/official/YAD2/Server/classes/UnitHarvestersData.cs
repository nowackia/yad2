using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.classes
{
    [XmlType(TypeName = "UnitHarvestersData", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class UnitHarvestersData
    {
        [System.Runtime.InteropServices.DispIdAttribute(-4)]
        public IEnumerator GetEnumerator()
        {
            return UnitHarvesterDataCollection.GetEnumerator();
        }

        public Server.classes.UnitHarvesterData Add(Server.classes.UnitHarvesterData obj)
        {
            return UnitHarvesterDataCollection.Add(obj);
        }

        [XmlIgnore]
        public Server.classes.UnitHarvesterData this[int index]
        {
            get { return (Server.classes.UnitHarvesterData)UnitHarvesterDataCollection[index]; }
        }

        [XmlIgnore]
        public int Count
        {
            get { return UnitHarvesterDataCollection.Count; }
        }

        public void Clear()
        {
            UnitHarvesterDataCollection.Clear();
        }

        public Server.classes.UnitHarvesterData Remove(int index)
        {
            Server.classes.UnitHarvesterData obj = UnitHarvesterDataCollection[index];
            UnitHarvesterDataCollection.Remove(obj);
            return obj;
        }

        public void Remove(object obj)
        {
            UnitHarvesterDataCollection.Remove(obj);
        }

        [XmlElement(Type = typeof(Server.classes.UnitHarvesterData), ElementName = "UnitHarvesterData", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public UnitHarvesterDataCollection __UnitHarvesterDataCollection;

        [XmlIgnore]
        public UnitHarvesterDataCollection UnitHarvesterDataCollection
        {
            get
            {
                if (__UnitHarvesterDataCollection == null) __UnitHarvesterDataCollection = new UnitHarvesterDataCollection();
                return __UnitHarvesterDataCollection;
            }
            set { __UnitHarvesterDataCollection = value; }
        }

        public UnitHarvestersData()
        {
        }
    }

}
