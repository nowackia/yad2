using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.Classes
{
    [XmlType(TypeName = "UnitSandwormData", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class UnitSandwormData
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

        [XmlElement(ElementName = "Health", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "int", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public int __Health;

        [XmlIgnore]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __HealthSpecified;

        [XmlIgnore]
        public int Health
        {
            get { return __Health; }
            set { __Health = value; __HealthSpecified = true; }
        }

        [XmlElement(ElementName = "Power", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "int", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public int __Power;

        [XmlIgnore]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __PowerSpecified;

        [XmlIgnore]
        public int Power
        {
            get { return __Power; }
            set { __Power = value; __PowerSpecified = true; }
        }

        [XmlElement(ElementName = "DamageDestroy", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "int", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public int __DamageDestroy;

        [XmlIgnore]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __DamageDestroySpecified;

        [XmlIgnore]
        public int DamageDestroy
        {
            get { return __DamageDestroy; }
            set { __DamageDestroy = value; __DamageDestroySpecified = true; }
        }

        [XmlElement(ElementName = "RotationSpeed", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "int", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public int __RotationSpeed;

        [XmlIgnore]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __RotationSpeedSpecified;

        [XmlIgnore]
        public int RotationSpeed
        {
            get { return __RotationSpeed; }
            set { __RotationSpeed = value; __RotationSpeedSpecified = true; }
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

        public UnitSandwormData()
        {
        }
    }

}
