using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.classes
{
    [XmlRoot(ElementName = "GameSettings", Namespace = Declarations.SchemaVersion, IsNullable = false), Serializable]
    [XmlType(TypeName = "GameSettings", Namespace = Declarations.SchemaVersion)]
    public class GameSettings
    {

        [XmlElement(Type = typeof(Server.classes.UnitTroopersData), ElementName = "UnitTroopersData", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Server.classes.UnitTroopersData __UnitTroopersData;

        [XmlIgnore]
        public Server.classes.UnitTroopersData UnitTroopersData
        {
            get
            {
                if (__UnitTroopersData == null) __UnitTroopersData = new Server.classes.UnitTroopersData();
                return __UnitTroopersData;
            }
            set { __UnitTroopersData = value; }
        }

        [XmlElement(Type = typeof(Server.classes.UnitTanksData), ElementName = "UnitTanksData", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Server.classes.UnitTanksData __UnitTanksData;

        [XmlIgnore]
        public Server.classes.UnitTanksData UnitTanksData
        {
            get
            {
                if (__UnitTanksData == null) __UnitTanksData = new Server.classes.UnitTanksData();
                return __UnitTanksData;
            }
            set { __UnitTanksData = value; }
        }

        [XmlElement(Type = typeof(Server.classes.UnitHarvestersData), ElementName = "UnitHarvestersData", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Server.classes.UnitHarvestersData __UnitHarvestersData;

        [XmlIgnore]
        public Server.classes.UnitHarvestersData UnitHarvestersData
        {
            get
            {
                if (__UnitHarvestersData == null) __UnitHarvestersData = new Server.classes.UnitHarvestersData();
                return __UnitHarvestersData;
            }
            set { __UnitHarvestersData = value; }
        }

        [XmlElement(Type = typeof(Server.classes.UnitMCVsData), ElementName = "UnitMCVsData", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Server.classes.UnitMCVsData __UnitMCVsData;

        [XmlIgnore]
        public Server.classes.UnitMCVsData UnitMCVsData
        {
            get
            {
                if (__UnitMCVsData == null) __UnitMCVsData = new Server.classes.UnitMCVsData();
                return __UnitMCVsData;
            }
            set { __UnitMCVsData = value; }
        }

        [XmlElement(Type = typeof(Server.classes.UnitSandwormsData), ElementName = "UnitSandwormsData", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Server.classes.UnitSandwormsData __UnitSandwormsData;

        [XmlIgnore]
        public Server.classes.UnitSandwormsData UnitSandwormsData
        {
            get
            {
                if (__UnitSandwormsData == null) __UnitSandwormsData = new Server.classes.UnitSandwormsData();
                return __UnitSandwormsData;
            }
            set { __UnitSandwormsData = value; }
        }

        [XmlElement(Type = typeof(Server.classes.AmmosData), ElementName = "AmmosData", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Server.classes.AmmosData __AmmosData;

        [XmlIgnore]
        public Server.classes.AmmosData AmmosData
        {
            get
            {
                if (__AmmosData == null) __AmmosData = new Server.classes.AmmosData();
                return __AmmosData;
            }
            set { __AmmosData = value; }
        }

        [XmlElement(Type = typeof(Server.classes.BuildingsData), ElementName = "BuildingsData", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Server.classes.BuildingsData __BuildingsData;

        [XmlIgnore]
        public Server.classes.BuildingsData BuildingsData
        {
            get
            {
                if (__BuildingsData == null) __BuildingsData = new Server.classes.BuildingsData();
                return __BuildingsData;
            }
            set { __BuildingsData = value; }
        }

        [XmlElement(Type = typeof(Server.classes.RacesData), ElementName = "RacesData", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Server.classes.RacesData __RacesData;

        [XmlIgnore]
        public Server.classes.RacesData RacesData
        {
            get
            {
                if (__RacesData == null) __RacesData = new Server.classes.RacesData();
                return __RacesData;
            }
            set { __RacesData = value; }
        }

        public GameSettings()
        {
        }
    }

}
