using System;
using System.Collections.Generic;
using System.Text;
using Yad.Board.Common;

namespace Yad.Engine.GameGraphics.Client
{
    enum ETextures
    {
        Dunes,
        Mountain = TileType.Mountain,
        Rock = TileType.Rock,
        RockDamage,
        RockExt,
        Sand = TileType.Sand,
		Hidden,
        SandDamage,
        SandExt,
        Spice,
        ThickSpice,
		Whatever
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
            
            textureMap[ETextures.Mountain] = "Mountain.png";
            textureMap[ETextures.Rock] = "Rock.png";
            textureMap[ETextures.RockDamage] = "RockDamage.png";
            textureMap[ETextures.RockExt] = "RockExt.png";
            textureMap[ETextures.Sand] = "Sand.png";
			textureMap[ETextures.Hidden] = "Hidden.png";
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
