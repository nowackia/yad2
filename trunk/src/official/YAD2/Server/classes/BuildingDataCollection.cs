using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;


namespace Server.classes
{
    public struct Declarations
    {
        public const string SchemaVersion = "http://www.example.org/dune";
    }


    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class BuildingDataCollection : ArrayList
    {
        public Server.classes.BuildingData Add(Server.classes.BuildingData obj)
        {
            base.Add(obj);
            return obj;
        }

        public Server.classes.BuildingData Add()
        {
            return Add(new Server.classes.BuildingData());
        }

        public void Insert(int index, Server.classes.BuildingData obj)
        {
            base.Insert(index, obj);
        }

        public void Remove(Server.classes.BuildingData obj)
        {
            base.Remove(obj);
        }

        new public Server.classes.BuildingData this[int index]
        {
            get { return (Server.classes.BuildingData)base[index]; }
            set { base[index] = value; }
        }
    }
}
