using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.Classes
{
    [XmlRoot(ElementName = "GameSettings", Namespace = Declarations.SchemaVersion, IsNullable = false), Serializable]
    [XmlType(TypeName = "GameSettings", Namespace = Declarations.SchemaVersion)]
    public class GameSettings
    {

        [XmlElement(Type = typeof(Server.Classes.UnitTroopersData), ElementName = "UnitTroopersData", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Server.Classes.UnitTroopersData __UnitTroopersData;

        [XmlIgnore]
        public Server.Classes.UnitTroopersData UnitTroopersData
        {
            get
            {
                if (__UnitTroopersData == null) __UnitTroopersData = new Server.Classes.UnitTroopersData();
                return __UnitTroopersData;
            }
            set { __UnitTroopersData = value; }
        }

        [XmlElement(Type = typeof(Server.Classes.UnitTanksData), ElementName = "UnitTanksData", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Server.Classes.UnitTanksData __UnitTanksData;

        [XmlIgnore]
        public Server.Classes.UnitTanksData UnitTanksData
        {
            get
            {
                if (__UnitTanksData == null) __UnitTanksData = new Server.Classes.UnitTanksData();
                return __UnitTanksData;
            }
            set { __UnitTanksData = value; }
        }

        [XmlElement(Type = typeof(Server.Classes.UnitHarvestersData), ElementName = "UnitHarvestersData", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Server.Classes.UnitHarvestersData __UnitHarvestersData;

        [XmlIgnore]
        public Server.Classes.UnitHarvestersData UnitHarvestersData
        {
            get
            {
                if (__UnitHarvestersData == null) __UnitHarvestersData = new Server.Classes.UnitHarvestersData();
                return __UnitHarvestersData;
            }
            set { __UnitHarvestersData = value; }
        }

        [XmlElement(Type = typeof(Server.Classes.UnitMCVsData), ElementName = "UnitMCVsData", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Server.Classes.UnitMCVsData __UnitMCVsData;

        [XmlIgnore]
        public Server.Classes.UnitMCVsData UnitMCVsData
        {
            get
            {
                if (__UnitMCVsData == null) __UnitMCVsData = new Server.Classes.UnitMCVsData();
                return __UnitMCVsData;
            }
            set { __UnitMCVsData = value; }
        }

        [XmlElement(Type = typeof(Server.Classes.UnitSandwormsData), ElementName = "UnitSandwormsData", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Server.Classes.UnitSandwormsData __UnitSandwormsData;

        [XmlIgnore]
        public Server.Classes.UnitSandwormsData UnitSandwormsData
        {
            get
            {
                if (__UnitSandwormsData == null) __UnitSandwormsData = new Server.Classes.UnitSandwormsData();
                return __UnitSandwormsData;
            }
            set { __UnitSandwormsData = value; }
        }

        [XmlElement(Type = typeof(Server.Classes.AmmosData), ElementName = "AmmosData", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Server.Classes.AmmosData __AmmosData;

        [XmlIgnore]
        public Server.Classes.AmmosData AmmosData
        {
            get
            {
                if (__AmmosData == null) __AmmosData = new Server.Classes.AmmosData();
                return __AmmosData;
            }
            set { __AmmosData = value; }
        }

        [XmlElement(Type = typeof(Server.Classes.BuildingsData), ElementName = "BuildingsData", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Server.Classes.BuildingsData __BuildingsData;

        [XmlIgnore]
        public Server.Classes.BuildingsData BuildingsData
        {
            get
            {
                if (__BuildingsData == null) __BuildingsData = new Server.Classes.BuildingsData();
                return __BuildingsData;
            }
            set { __BuildingsData = value; }
        }

        [XmlElement(Type = typeof(Server.Classes.RacesData), ElementName = "RacesData", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Server.Classes.RacesData __RacesData;

        [XmlIgnore]
        public Server.Classes.RacesData RacesData
        {
            get
            {
                if (__RacesData == null) __RacesData = new Server.Classes.RacesData();
                return __RacesData;
            }
            set { __RacesData = value; }
        }

        public GameSettings()
        {
        }
    }

}
