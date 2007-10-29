using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;


namespace Server.Classes
{
    public struct Declarations
    {
        public const string SchemaVersion = "http://www.example.org/dune";
    }


    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class BuildingDataCollection : ArrayList
    {
        public Server.Classes.BuildingData Add(Server.Classes.BuildingData obj)
        {
            base.Add(obj);
            return obj;
        }

        public Server.Classes.BuildingData Add()
        {
            return Add(new Server.Classes.BuildingData());
        }

        public void Insert(int index, Server.Classes.BuildingData obj)
        {
            base.Insert(index, obj);
        }

        public void Remove(Server.Classes.BuildingData obj)
        {
            base.Remove(obj);
        }

        new public Server.Classes.BuildingData this[int index]
        {
            get { return (Server.Classes.BuildingData)base[index]; }
            set { base[index] = value; }
        }
    }
}
