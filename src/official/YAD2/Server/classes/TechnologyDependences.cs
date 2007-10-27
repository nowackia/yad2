using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.classes
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

        public Server.classes.TechnologyDependence Add(Server.classes.TechnologyDependence obj)
        {
            return TechnologyDependenceCollection.Add(obj);
        }

        [XmlIgnore]
        public Server.classes.TechnologyDependence this[int index]
        {
            get { return (Server.classes.TechnologyDependence)TechnologyDependenceCollection[index]; }
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

        public Server.classes.TechnologyDependence Remove(int index)
        {
            Server.classes.TechnologyDependence obj = TechnologyDependenceCollection[index];
            TechnologyDependenceCollection.Remove(obj);
            return obj;
        }

        public void Remove(object obj)
        {
            TechnologyDependenceCollection.Remove(obj);
        }

        [XmlElement(Type = typeof(Server.classes.TechnologyDependence), ElementName = "TechnologyDependence", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
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
