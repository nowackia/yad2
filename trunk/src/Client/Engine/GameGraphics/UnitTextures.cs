using System;
using System.Collections.Generic;
using System.Text;
using Yad.Properties.Client;

namespace Yad.Engine.GameGraphics
{
	enum EUnitTexture
	{
		Carryall,
		Devastator,
		DevastatorBase,
		Frigate,
		Harvester,
		HarvesterSand,
		Infantry,
		Launcher,
		MCV,
		Ornithopter,
		Quad,
		Saboteur,
		Sandworm,
		SiegeTank,
		SiegeTankBase,
		SonicTank,
		Tank,
		TankBase,
		Trike,
		Trooper
	}
	static class UnitTextures
	{
		private static Dictionary<EUnitTexture, String> unitTextures;
		
		private static void initialTextureMap()
		{
			unitTextures = new Dictionary<EUnitTexture, string>();
			unitTextures[EUnitTexture.Carryall] = Settings.Default.TextureUnitCarryall;
			unitTextures[EUnitTexture.Devastator] = Settings.Default.TextureUnitDevastator;
			unitTextures[EUnitTexture.DevastatorBase] = Settings.Default.TextureUnitDevastatorBase;
			unitTextures[EUnitTexture.Quad] = Settings.Default.TextureUnitQuad;
			unitTextures[EUnitTexture.Frigate] = Settings.Default.TextureUnitFrigate;
			unitTextures[EUnitTexture.Harvester] = Settings.Default.TextureUnitHarvester;
			unitTextures[EUnitTexture.HarvesterSand] = Settings.Default.TextureUnitHarvesterSand;
			unitTextures[EUnitTexture.Infantry] = Settings.Default.TextureUnitInfantry;
			unitTextures[EUnitTexture.Launcher] = Settings.Default.TextureUnitLauncher;
			unitTextures[EUnitTexture.MCV] = Settings.Default.TextureUnitMCV;
			unitTextures[EUnitTexture.Ornithopter] = Settings.Default.TextureUnitOrnithopter;
			unitTextures[EUnitTexture.Saboteur] = Settings.Default.TextureUnitSaboteur;
			unitTextures[EUnitTexture.Sandworm] = Settings.Default.TextureUnitSandworm;
			unitTextures[EUnitTexture.SiegeTank] = Settings.Default.TextureUnitSiegeTank;
			unitTextures[EUnitTexture.SiegeTankBase] = Settings.Default.TextureUnitSiegeTankBase;
			unitTextures[EUnitTexture.SonicTank] = Settings.Default.TextureUnitSonicTank;
			unitTextures[EUnitTexture.Tank] = Settings.Default.TextureUnitTank;
			unitTextures[EUnitTexture.TankBase] = Settings.Default.TextureUnitTankBase;
			unitTextures[EUnitTexture.Trike] = Settings.Default.TextureUnitTrike;
			unitTextures[EUnitTexture.Trooper] = Settings.Default.TextureUnitTrooper;
		}

		public static int Count
		{
			get
			{
				if (unitTextures == null)
					initialTextureMap();
				return unitTextures.Count;
			}
		}
		
		public static string getFileName(EUnitTexture texture)
		{
			if (unitTextures == null)
				initialTextureMap();
			return unitTextures[texture];
		}

	}

}
