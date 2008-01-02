using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using Yad.Config.Common;

namespace Yad.Config.XMLLoader.Common {
	public static class XMLLoader {

		public static GameSettingsWrapper get(String configFilePath, String configFileXSDPath) {
			try
            {
#pragma warning disable 0618
                FileStream sr = new FileStream(configFilePath, FileMode.Open);
				XmlSerializer xmlSer = new XmlSerializer(typeof(GameSettings));
				XmlReader xr = new XmlTextReader(sr);
				XmlValidatingReader xvr = new XmlValidatingReader(xr);
				xvr.Schemas.Add(Declarations.SchemaVersion, configFileXSDPath);
				GameSettings gameSettings = (GameSettings)xmlSer.Deserialize(xvr);
				GameSettingsWrapper gsw = new GameSettingsWrapper(gameSettings);
#pragma warning restore 0618
                xvr.Close();
				xr.Close();
				sr.Close();

				return gsw;
			} catch (Exception e) {
				throw new XMLLoaderException(e);
			}
		}
	}
}
