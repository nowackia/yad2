using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.Classes
{
    [XmlType(TypeName = "UnitTroopersData", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class UnitTroopersData
    {
        [System.Runtime.InteropServices.DispIdAttribute(-4)]
        public IEnumerator GetEnumerator()
        {
            return UnitTrooperDataCollection.GetEnumerator();
        }

        public Server.Classes.UnitTrooperData Add(Server.Classes.UnitTrooperData obj)
        {
            return UnitTrooperDataCollection.Add(obj);
        }

        [XmlIgnore]
        public Server.Classes.UnitTrooperData this[int index]
        {
            get { return (Server.Classes.UnitTrooperData)UnitTrooperDataCollection[index]; }
        }

        [XmlIgnore]
        public int Count
        {
            get { return UnitTrooperDataCollection.Count; }
        }

        public void Clear()
        {
            UnitTrooperDataCollection.Clear();
        }

        public Server.Classes.UnitTrooperData Remove(int index)
        {
            Server.Classes.UnitTrooperData obj = UnitTrooperDataCollection[index];
            UnitTrooperDataCollection.Remove(obj);
            return obj;
        }

        public void Remove(object obj)
        {
            UnitTrooperDataCollection.Remove(obj);
        }

        [XmlElement(Type = typeof(Server.Classes.UnitTrooperData), ElementName = "UnitTrooperData", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public UnitTrooperDataCollection __UnitTrooperDataCollection;

        [XmlIgnore]
        public UnitTrooperDataCollection UnitTrooperDataCollection
        {
            get
            {
                if (__UnitTrooperDataCollection == null) __UnitTrooperDataCollection = new UnitTrooperDataCollection();
                return __UnitTrooperDataCollection;
            }
            set { __UnitTrooperDataCollection = value; }
        }

        public UnitTroopersData()
        {
        }
    }

}
