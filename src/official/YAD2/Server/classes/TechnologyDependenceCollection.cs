using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;
namespace Server.classes
{
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class TechnologyDependenceCollection : ArrayList
    {
        public Server.classes.TechnologyDependence Add(Server.classes.TechnologyDependence obj)
        {
            base.Add(obj);
            return obj;
        }

        public Server.classes.TechnologyDependence Add()
        {
            return Add(new Server.classes.TechnologyDependence());
        }

        public void Insert(int index, Server.classes.TechnologyDependence obj)
        {
            base.Insert(index, obj);
        }

        public void Remove(Server.classes.TechnologyDependence obj)
        {
            base.Remove(obj);
        }

        new public Server.classes.TechnologyDependence this[int index]
        {
            get { return (Server.classes.TechnologyDependence)base[index]; }
            set { base[index] = value; }
        }
    }
}
