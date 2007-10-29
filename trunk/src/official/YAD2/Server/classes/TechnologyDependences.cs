using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.Classes
{
    [XmlType(TypeName = "TechnologyDependences", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class TechnologyDependences
    {
        [System.Runtime.InteropServices.DispIdAttribute(-4)]
        public IEnumerator GetEnumerator()
        {
            return TechnologyDependenceCollection.GetEnumerator();
        }

        public Server.Classes.TechnologyDependence Add(Server.Classes.TechnologyDependence obj)
        {
            return TechnologyDependenceCollection.Add(obj);
        }

        [XmlIgnore]
        public Server.Classes.TechnologyDependence this[int index]
        {
            get { return (Server.Classes.TechnologyDependence)TechnologyDependenceCollection[index]; }
        }

        [XmlIgnore]
        public int Count
        {
            get { return TechnologyDependenceCollection.Count; }
        }

        public void Clear()
        {
            TechnologyDependenceCollection.Clear();
        }

        public Server.Classes.TechnologyDependence Remove(int index)
        {
            Server.Classes.TechnologyDependence obj = TechnologyDependenceCollection[index];
            TechnologyDependenceCollection.Remove(obj);
            return obj;
        }

        public void Remove(object obj)
        {
            TechnologyDependenceCollection.Remove(obj);
        }

        [XmlElement(Type = typeof(Server.Classes.TechnologyDependence), ElementName = "TechnologyDependence", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public TechnologyDependenceCollection __TechnologyDependenceCollection;

        [XmlIgnore]
        public TechnologyDependenceCollection TechnologyDependenceCollection
        {
            get
            {
                if (__TechnologyDependenceCollection == null) __TechnologyDependenceCollection = new TechnologyDependenceCollection();
                return __TechnologyDependenceCollection;
            }
            set { __TechnologyDependenceCollection = value; }
        }

        public TechnologyDependences()
        {
        }
    }

}
