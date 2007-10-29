using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.Classes
{
    [XmlType(TypeName = "RacesData", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class RacesData
    {
        [System.Runtime.InteropServices.DispIdAttribute(-4)]
        public IEnumerator GetEnumerator()
        {
            return RaceDataCollection.GetEnumerator();
        }

        public Server.Classes.RaceData Add(Server.Classes.RaceData obj)
        {
            return RaceDataCollection.Add(obj);
        }

        [XmlIgnore]
        public Server.Classes.RaceData this[int index]
        {
            get { return (Server.Classes.RaceData)RaceDataCollection[index]; }
        }

        [XmlIgnore]
        public int Count
        {
            get { return RaceDataCollection.Count; }
        }

        public void Clear()
        {
            RaceDataCollection.Clear();
        }

        public Server.Classes.RaceData Remove(int index)
        {
            Server.Classes.RaceData obj = RaceDataCollection[index];
            RaceDataCollection.Remove(obj);
            return obj;
        }

        public void Remove(object obj)
        {
            RaceDataCollection.Remove(obj);
        }

        [XmlElement(Type = typeof(Server.Classes.RaceData), ElementName = "RaceData", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public RaceDataCollection __RaceDataCollection;

        [XmlIgnore]
        public RaceDataCollection RaceDataCollection
        {
            get
            {
                if (__RaceDataCollection == null) __RaceDataCollection = new RaceDataCollection();
                return __RaceDataCollection;
            }
            set { __RaceDataCollection = value; }
        }

        public RacesData()
        {
        }
    }

}
