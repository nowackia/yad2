using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.Classes
{
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class TechnologyDependenceCollection : ArrayList
    {
        public Server.Classes.TechnologyDependence Add(Server.Classes.TechnologyDependence obj)
        {
            base.Add(obj);
            return obj;
        }

        public Server.Classes.TechnologyDependence Add()
        {
            return Add(new Server.Classes.TechnologyDependence());
        }

        public void Insert(int index, Server.Classes.TechnologyDependence obj)
        {
            base.Insert(index, obj);
        }

        public void Remove(Server.Classes.TechnologyDependence obj)
        {
            base.Remove(obj);
        }

        new public Server.Classes.TechnologyDependence this[int index]
        {
            get { return (Server.Classes.TechnologyDependence)base[index]; }
            set { base[index] = value; }
        }
    }
}
