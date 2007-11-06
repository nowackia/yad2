using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.Classes
{
    [XmlType(TypeName = "TechnologyDependence", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class TechnologyDependence
    {

        [XmlElement(Type = typeof(string), ElementName = "BuildingName", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public BuildingNameCollection __BuildingNameCollection;

        [XmlIgnore]
        public BuildingNameCollection BuildingNameCollection
        {
            get
            {
                if (__BuildingNameCollection == null) __BuildingNameCollection = new BuildingNameCollection();
                return __BuildingNameCollection;
            }
            set { __BuildingNameCollection = value; }
        }

        [XmlElement(Type = typeof(Server.Classes.BuildingsNames), ElementName = "RequiredBuildings", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public BuildingsNamesCollection __RequiredBuildingsCollection;

        [XmlIgnore]
        public BuildingsNamesCollection RequiredBuildingsCollection
        {
            get
            {
                if (__RequiredBuildingsCollection == null) __RequiredBuildingsCollection = new BuildingsNamesCollection();
                return __RequiredBuildingsCollection;
            }
            set { __RequiredBuildingsCollection = value; }
        }

        public TechnologyDependence()
        {
        }
    }

}
