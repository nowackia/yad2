using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config.Common;

namespace Yad.Engine.Common {
	[Obsolete("Wywaliæ. Rasy wczytywane z pliku konfiguracyjnego i przechowywane w GameSettings.RacesData.RaceDataCollection. Posiada short TypeID i string Name")]
    public enum HouseType : byte {
        Ordos = 0,
        Harkonnen,
        Atreides
    }
}
