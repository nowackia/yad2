using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.Classes
{
    [XmlType(TypeName = "AmmosData", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class AmmosData
    {
        [System.Runtime.InteropServices.DispIdAttribute(-4)]
        public IEnumerator GetEnumerator()
        {
            return AmmoDataCollection.GetEnumerator();
        }

        public Server.Classes.AmmoData Add(Server.Classes.AmmoData obj)
        {
            return AmmoDataCollection.Add(obj);
        }

        [XmlIgnore]
        public Server.Classes.AmmoData this[int index]
        {
            get { return (Server.Classes.AmmoData)AmmoDataCollection[index]; }
        }

        [XmlIgnore]
        public int Count
        {
            get { return AmmoDataCollection.Count; }
        }

        public void Clear()
        {
            AmmoDataCollection.Clear();
        }

        public Server.Classes.AmmoData Remove(int index)
        {
            Server.Classes.AmmoData obj = AmmoDataCollection[index];
            AmmoDataCollection.Remove(obj);
            return obj;
        }

        public void Remove(object obj)
        {
            AmmoDataCollection.Remove(obj);
        }

        [XmlElement(Type = typeof(Server.Classes.AmmoData), ElementName = "AmmoData", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public AmmoDataCollection __AmmoDataCollection;

        [XmlIgnore]
        public AmmoDataCollection AmmoDataCollection
        {
            get
            {
                if (__AmmoDataCollection == null) __AmmoDataCollection = new AmmoDataCollection();
                return __AmmoDataCollection;
            }
            set { __AmmoDataCollection = value; }
        }

        public AmmosData()
        {
        }
    }

}
