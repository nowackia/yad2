using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.classes
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

        public Server.classes.RaceData Add(Server.classes.RaceData obj)
        {
            return RaceDataCollection.Add(obj);
        }

        [XmlIgnore]
        public Server.classes.RaceData this[int index]
        {
            get { return (Server.classes.RaceData)RaceDataCollection[index]; }
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

        public Server.classes.RaceData Remove(int index)
        {
            Server.classes.RaceData obj = RaceDataCollection[index];
            RaceDataCollection.Remove(obj);
            return obj;
        }

        public void Remove(object obj)
        {
            RaceDataCollection.Remove(obj);
        }

        [XmlElement(Type = typeof(Server.classes.RaceData), ElementName = "RaceData", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
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
