using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.Classes
{
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class BuildingsNamesCollection : ArrayList
    {
        public Server.Classes.BuildingsNames Add(Server.Classes.BuildingsNames obj)
        {
            base.Add(obj);
            return obj;
        }

        public Server.Classes.BuildingsNames Add()
        {
            return Add(new Server.Classes.BuildingsNames());
        }

        public void Insert(int index, Server.Classes.BuildingsNames obj)
        {
            base.Insert(index, obj);
        }

        public void Remove(Server.Classes.BuildingsNames obj)
        {
            base.Remove(obj);
        }

        new public Server.Classes.BuildingsNames this[int index]
        {
            get { return (Server.Classes.BuildingsNames)base[index]; }
            set { base[index] = value; }
        }
    }
}
