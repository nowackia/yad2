using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.Classes
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

        public Server.Classes.BuildingData Add(Server.Classes.BuildingData obj)
        {
            return BuildingDataCollection.Add(obj);
        }

        [XmlIgnore]
        public Server.Classes.BuildingData this[int index]
        {
            get { return (Server.Classes.BuildingData)BuildingDataCollection[index]; }
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

        public Server.Classes.BuildingData Remove(int index)
        {
            Server.Classes.BuildingData obj = BuildingDataCollection[index];
            BuildingDataCollection.Remove(obj);
            return obj;
        }

        public void Remove(object obj)
        {
            BuildingDataCollection.Remove(obj);
        }

        [XmlElement(Type = typeof(Server.Classes.BuildingData), ElementName = "BuildingData", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
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
