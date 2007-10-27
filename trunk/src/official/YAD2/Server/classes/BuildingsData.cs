using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.classes
{
    [XmlType(TypeName = "BuildingsData", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class BuildingsData
    {
        [System.Runtime.InteropServices.DispIdAttribute(-4)]
        public IEnumerator GetEnumerator()
        {
            return BuildingDataCollection.GetEnumerator();
        }

        public Server.classes.BuildingData Add(Server.classes.BuildingData obj)
        {
            return BuildingDataCollection.Add(obj);
        }

        [XmlIgnore]
        public Server.classes.BuildingData this[int index]
        {
            get { return (Server.classes.BuildingData)BuildingDataCollection[index]; }
        }

        [XmlIgnore]
        public int Count
        {
            get { return BuildingDataCollection.Count; }
        }

        public void Clear()
        {
            BuildingDataCollection.Clear();
        }

        public Server.classes.BuildingData Remove(int index)
        {
            Server.classes.BuildingData obj = BuildingDataCollection[index];
            BuildingDataCollection.Remove(obj);
            return obj;
        }

        public void Remove(object obj)
        {
            BuildingDataCollection.Remove(obj);
        }

        [XmlElement(Type = typeof(Server.classes.BuildingData), ElementName = "BuildingData", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public BuildingDataCollection __BuildingDataCollection;

        [XmlIgnore]
        public BuildingDataCollection BuildingDataCollection
        {
            get
            {
                if (__BuildingDataCollection == null) __BuildingDataCollection = new BuildingDataCollection();
                return __BuildingDataCollection;
            }
            set { __BuildingDataCollection = value; }
        }

        public BuildingsData()
        {
        }
    }

}
