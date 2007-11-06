using System;
using System.Collections.Generic;
using System.Text;
using Server.Classes.Exceptions;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using YAD2Configuration;

namespace Classes.XMLLoader
{
    static class XMLLoader
    {
		/*
        public static string xmlFile = "Config/dune_example.xml";
		public static string schema = "Config/dune.xsd";
        public static string validateNamespace = "http://www.example.org/dune";
		*/
		/*
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
					xvr.Schemas.Add("http://www.example.org/dune", schema);
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
		 * */

		public static GameSettings get(String configFilePath, String configFileXSDPath) {
			try {
				FileStream sr = new FileStream(configFilePath, FileMode.Open);
				XmlSerializer xmlSer = new XmlSerializer(typeof(GameSettings));
				XmlReader xr = new XmlTextReader(sr);
				XmlValidatingReader xvr = new XmlValidatingReader(xr);
				xvr.Schemas.Add(YAD2Configuration.Declarations.SchemaVersion, configFileXSDPath);
				return (GameSettings)xmlSer.Deserialize(xvr);
				xvr.Close();
				xr.Close();
				sr.Close();

			} catch (Exception e) {
				throw new XMLLoaderException(e);
			}
		}
    }
}
