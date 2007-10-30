using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.Classes
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

        public Server.Classes.UnitHarvesterData Add(Server.Classes.UnitHarvesterData obj)
        {
            return UnitHarvesterDataCollection.Add(obj);
        }

        [XmlIgnore]
        public Server.Classes.UnitHarvesterData this[int index]
        {
            get { return (Server.Classes.UnitHarvesterData)UnitHarvesterDataCollection[index]; }
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

        public Server.Classes.UnitHarvesterData Remove(int index)
        {
            Server.Classes.UnitHarvesterData obj = UnitHarvesterDataCollection[index];
            UnitHarvesterDataCollection.Remove(obj);
            return obj;
        }

        public void Remove(object obj)
        {
            UnitHarvesterDataCollection.Remove(obj);
        }

        [XmlElement(Type = typeof(Server.Classes.UnitHarvesterData), ElementName = "UnitHarvesterData", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
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
