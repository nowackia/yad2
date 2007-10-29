using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.Classes
{
    [XmlType(TypeName = "AmmoData", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class AmmoData
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

        [XmlElement(ElementName = "Speed", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "int", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public int __Speed;

        [XmlIgnore]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __SpeedSpecified;

        [XmlIgnore]
        public int Speed
        {
            get { return __Speed; }
            set { __Speed = value; __SpeedSpecified = true; }
        }

        [XmlElement(ElementName = "DamageDestroyRange", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "int", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public int __DamageDestroyRange;

        [XmlIgnore]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __DamageDestroyRangeSpecified;

        [XmlIgnore]
        public int DamageDestroyRange
        {
            get { return __DamageDestroyRange; }
            set { __DamageDestroyRange = value; __DamageDestroyRangeSpecified = true; }
        }

        [XmlElement(ElementName = "Texture", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public string __Texture;

        [XmlIgnore]
        public string Texture
        {
            get { return __Texture; }
            set { __Texture = value; }
        }

        public AmmoData()
        {
        }
    }

}
