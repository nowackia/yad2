using System;
using System.Collections.Generic;
using System.Text;
using Yad.Board.Common;
using Yad.Properties;

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
            textureMap[ETextures.Dunes] = Settings.Default.TextureDunes;
            textureMap[ETextures.Mountain] = Settings.Default.TextureMountain;
            textureMap[ETextures.Rock] = Settings.Default.TextureRock;
            textureMap[ETextures.RockDamage] = Settings.Default.TextureRockDamage;
            textureMap[ETextures.RockExt] = Settings.Default.TextureRockExt;
            textureMap[ETextures.Sand] = Settings.Default.TextureSand;
			textureMap[ETextures.Hidden] = Settings.Default.TextureHidden;
            textureMap[ETextures.SandDamage] = Settings.Default.TextureSandDamage;
            textureMap[ETextures.SandExt] = Settings.Default.TextureSandExt;
            textureMap[ETextures.Spice] = Settings.Default.TextureSpice;
            textureMap[ETextures.ThickSpice] = Settings.Default.TextureThickSpice;
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
