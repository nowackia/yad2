using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.Classes
{
    [XmlType(TypeName = "UnitMCVData", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class UnitMCVData
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

        [XmlElement(ElementName = "Cost", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "int", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public int __Cost;

        [XmlIgnore]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __CostSpecified;

        [XmlIgnore]
        public int Cost
        {
            get { return __Cost; }
            set { __Cost = value; __CostSpecified = true; }
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

        [XmlElement(ElementName = "ViewRange", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "int", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public int __ViewRange;

        [XmlIgnore]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __ViewRangeSpecified;

        [XmlIgnore]
        public int ViewRange
        {
            get { return __ViewRange; }
            set { __ViewRange = value; __ViewRangeSpecified = true; }
        }

        [XmlElement(ElementName = "BuildSpeed", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "int", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public int __BuildSpeed;

        [XmlIgnore]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __BuildSpeedSpecified;

        [XmlIgnore]
        public int BuildSpeed
        {
            get { return __BuildSpeed; }
            set { __BuildSpeed = value; __BuildSpeedSpecified = true; }
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

        [XmlElement(ElementName = "BuildingCanProduce", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public string __BuildingCanProduce;

        [XmlIgnore]
        public string BuildingCanProduce
        {
            get { return __BuildingCanProduce; }
            set { __BuildingCanProduce = value; }
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

        [XmlElement(ElementName = "Picture", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public string __Picture;

        [XmlIgnore]
        public string Picture
        {
            get { return __Picture; }
            set { __Picture = value; }
        }

        public UnitMCVData()
        {
        }
    }

}
