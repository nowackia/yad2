using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Yad.Net.Messaging.Common;
using Yad.Net.Client;
using Yad.Board;
using Yad.UI.Client;
using Yad.Config.Common;
using Yad.Engine.Common;
using Yad.Board.Common;

namespace Yad.Engine.Client
{
	static class GameLogic
	{
		public delegate void AddBuildingDelegate(short id, short key);
		public static event AddBuildingDelegate AddBuildingEvent;
		/// <summary>;
		/// if gamer wants to locate builing on the map
		/// </summary>
		private static bool isLocatingBuilding = false;

		private static short buildingToBuild;

		private static short unitToCreate;

		private static bool isCreatingUnit = false;

		/// <summary>
		/// Waiting for building a building
		/// </summary>
		private static bool isWaitingForBuildingBuilt = false;

		/// <summary>
		/// Waiting for umit creation
		/// </summary>
		private static bool isWaitingForUnitCreation = false;

		private static bool buildingPositionOK(Position pos, short buildingTypeId)
		{
			foreach (BuildingData b in GameForm.sim.GameSettingsWrapper.GameSettings.BuildingsData)
			{
				if (b.TypeID == buildingTypeId)
				{
					for(int x = pos.X; x<pos.X+ b.Size.X; x++)
						for (int y = pos.Y; y < pos.Y + b.Size.Y; y++)
						{
							if (GameForm.sim.Map.Tiles[x, GameForm.sim.Map.Height-y-1] != Yad.Board.Common.TileType.Rock)
								return false;
							if (GameForm.sim.Map.Units[x,y].Count>0)
								return false;
							if (GameForm.sim.Map.Buildings[x, y].Count > 0)
								return false;
						}
					return true;	
				}
			}
			return false;
		}
		
		/// <summary>
		/// Reaction on left mouse button on the map
		/// </summary>
		/// <param name="e"></param>
		public static void MouseLeftClick(MouseEventArgs e)
		{
			Position pos = GameGraphics.TranslateMousePosition(e.Location);
			if (isLocatingBuilding)
			{
				if (buildingPositionOK(pos, GameForm.sim.GameSettingsWrapper.GameSettings.BuildingsData.BuildingDataCollection[0].__TypeID))
				{
					//TODO: to jeszcze poprawić w miarę potrzeby
					BuildMessage bm = (BuildMessage)Yad.Net.Client.Utils.CreateMessageWithPlayerId(MessageType.Build);
					bm.BuildingID = GameForm.currPlayer.GenerateObjectID();
					bm.PlayerId = GameForm.currPlayer.ID;
					bm.BuildingType = GameForm.sim.GameSettingsWrapper.GameSettings.BuildingsData.BuildingDataCollection[0].__TypeID;
					bm.Type = MessageType.Build;
					bm.Position = pos;
					bm.IdTurn = GameForm.sim.CurrentTurn + GameForm.sim.Delta;
					GameForm.conn.SendMessage(bm);
					isLocatingBuilding = false;
				}
			}
		}

		public static bool IsWaitingForBuildingBuild
		{
			get
			{
				return isWaitingForBuildingBuilt;
			}
		}

		public static bool IsWaitingForUnitCreation
		{
			get
			{
				return isWaitingForUnitCreation;
			}
		}

		public static void LocateBuilding(short p)
		{
			isLocatingBuilding = true;
			buildingToBuild = p;
		}

		internal static void CreateUnit(short p)
		{
			isCreatingUnit = true;
			unitToCreate = p;
		}

		internal static void InitStripes(string name, short key)
		{
			AddBuildingEvent(GameForm.sim.GameSettingsWrapper.namesToIds[name], key);
		}
	}
}
