using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.classes
{
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class BuildingsNamesCollection : ArrayList
    {
        public Server.classes.BuildingsNames Add(Server.classes.BuildingsNames obj)
        {
            base.Add(obj);
            return obj;
        }

        public Server.classes.BuildingsNames Add()
        {
            return Add(new Server.classes.BuildingsNames());
        }

        public void Insert(int index, Server.classes.BuildingsNames obj)
        {
            base.Insert(index, obj);
        }

        public void Remove(Server.classes.BuildingsNames obj)
        {
            base.Remove(obj);
        }

        new public Server.classes.BuildingsNames this[int index]
        {
            get { return (Server.classes.BuildingsNames)base[index]; }
            set { base[index] = value; }
        }
    }
}
