using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Engine.GameGraphics.Client
{
    enum ETextures
    {
        Dunes,
        Hidden,
        Mountain,
        Rock,
        RockDamage,
        RockExt,
        Sand,
        SandDamage,
        SandExt,
        Spice,
        ThickSpice
    }
    static class TextureFiles
    {
        private static Dictionary<ETextures, string> textureMap= null;
        public static string getFileName(ETextures texture)
        {
            if (textureMap == null)
                initialTextureMap();
            return textureMap[texture];
        }

        private static void initialTextureMap()
        {
            textureMap = new Dictionary<ETextures, string>();
            textureMap[ETextures.Dunes] = "Dunes.png";
            textureMap[ETextures.Hidden] = "Hidden.png";
            textureMap[ETextures.Mountain] = "Mountain.png";
            textureMap[ETextures.Rock] = "Rock.png";
            textureMap[ETextures.RockDamage] = "RockDamage.png";
            textureMap[ETextures.RockExt] = "RockExt.png";
            textureMap[ETextures.Sand] = "Sand.png";
            textureMap[ETextures.SandDamage] = "SandDamage.png";
            textureMap[ETextures.SandExt] = "SandExt.png";
            textureMap[ETextures.Spice] = "Spice.png";
            textureMap[ETextures.ThickSpice] = "ThickSpice.png";
        }

        public static int Count
        {
            get
            {
				if (textureMap == null)
					initialTextureMap();
                return textureMap.Count;
            }
        }
    }
}
