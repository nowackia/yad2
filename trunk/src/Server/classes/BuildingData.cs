using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.Classes
{
    [XmlType(TypeName = "BuildingData", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class BuildingData
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

        [XmlElement(Type = typeof(Server.Classes.Size), ElementName = "Size", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Server.Classes.Size __Size;

        [XmlIgnore]
        public Server.Classes.Size Size
        {
            get
            {
                if (__Size == null) __Size = new Server.Classes.Size();
                return __Size;
            }
            set { __Size = value; }
        }

        [XmlElement(ElementName = "EnergyConsumption", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "int", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public int __EnergyConsumption;

        [XmlIgnore]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __EnergyConsumptionSpecified;

        [XmlIgnore]
        public int EnergyConsumption
        {
            get { return __EnergyConsumption; }
            set { __EnergyConsumption = value; __EnergyConsumptionSpecified = true; }
        }

        [XmlElement(Type = typeof(Server.Classes.RideableFields), ElementName = "RideableFields", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Server.Classes.RideableFields __RideableFields;

        [XmlIgnore]
        public Server.Classes.RideableFields RideableFields
        {
            get
            {
                if (__RideableFields == null) __RideableFields = new Server.Classes.RideableFields();
                return __RideableFields;
            }
            set { __RideableFields = value; }
        }

        [XmlElement(Type = typeof(Server.Classes.UnitsNames), ElementName = "UnitsCanProduce", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Server.Classes.UnitsNames __UnitsCanProduce;

        [XmlIgnore]
        public Server.Classes.UnitsNames UnitsCanProduce
        {
            get
            {
                if (__UnitsCanProduce == null) __UnitsCanProduce = new Server.Classes.UnitsNames();
                return __UnitsCanProduce;
            }
            set { __UnitsCanProduce = value; }
        }

        [XmlElement(Type = typeof(Server.Classes.BuildingsNames), ElementName = "BuildingsCanProduce", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Server.Classes.BuildingsNames __BuildingsCanProduce;

        [XmlIgnore]
        public Server.Classes.BuildingsNames BuildingsCanProduce
        {
            get
            {
                if (__BuildingsCanProduce == null) __BuildingsCanProduce = new Server.Classes.BuildingsNames();
                return __BuildingsCanProduce;
            }
            set { __BuildingsCanProduce = value; }
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

        [XmlElement(ElementName = "FireRange", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "int", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public int __FireRange;

        [XmlIgnore]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __FireRangeSpecified;

        [XmlIgnore]
        public int FireRange
        {
            get { return __FireRange; }
            set { __FireRange = value; __FireRangeSpecified = true; }
        }

        [XmlElement(ElementName = "ReloadTime", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "int", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public int __ReloadTime;

        [XmlIgnore]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __ReloadTimeSpecified;

        [XmlIgnore]
        public int ReloadTime
        {
            get { return __ReloadTime; }
            set { __ReloadTime = value; __ReloadTimeSpecified = true; }
        }

        public BuildingData()
        {
        }
    }

}
