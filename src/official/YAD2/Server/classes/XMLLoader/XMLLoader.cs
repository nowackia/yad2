using System;
using System.Collections.Generic;
using System.Text;
using Server.classes.Exceptions;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace Server.classes.XMLLoader
{
    class XMLLoader
    {
        public static string xmlFile = "dune_example.xml";
        public static string schema = "dune.xsd";
        public static string validateNamespace = "http://www.example.org/dune";
        private static GameSettings GS;
        private XMLLoader(){}

        public static GameSettings getGameSettings()
        {
            if (GS == null)
            {
                try
                {
                    System.IO.FileStream sr = new FileStream(xmlFile, FileMode.Open);
                    XmlSerializer xmlSer = new XmlSerializer(typeof(GameSettings));
                    System.Xml.XmlReader xr = new XmlTextReader(sr);
                    XmlValidatingReader xvr = new XmlValidatingReader(xr);
                    xvr.Schemas.Add("http://www.example.org/dune", "dune.xsd");
                    GS = (GameSettings)xmlSer.Deserialize(xvr);
                    xvr.Close();
                    xr.Close();
                    sr.Close();
                }
                catch (Exception e)
                {
                    throw new XMLLoaderException(e);
                }
                
            }
            return GS;

        }

    }
}
