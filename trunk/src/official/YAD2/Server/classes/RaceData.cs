using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.classes
{
    [XmlType(TypeName = "RaceData", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class RaceData
    {

        [XmlElement(ElementName = "Name", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public string __Name;

        [XmlIgnore]
        public string Name
        {
            get { return __Name; }
            set { __Name = value; }
        }

        [XmlElement(Type = typeof(Server.classes.TechnologyDependences), ElementName = "TechnologyDependences", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Server.classes.TechnologyDependences __TechnologyDependences;

        [XmlIgnore]
        public Server.classes.TechnologyDependences TechnologyDependences
        {
            get
            {
                if (__TechnologyDependences == null) __TechnologyDependences = new Server.classes.TechnologyDependences();
                return __TechnologyDependences;
            }
            set { __TechnologyDependences = value; }
        }

        public RaceData()
        {
        }
    }

}
