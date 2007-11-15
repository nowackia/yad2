using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Yad.Net.Messaging.Common;
using Yad.Net.Client;
using Yad.Board;
using Yad.UI.Client;

namespace Yad.Engine.Client
{
	static class GameLogic
	{
		/// <summary>
		/// if gamer wants to locate builing on the map
		/// </summary>
		private static bool isLocatingBuilding = true;

		/// <summary>
		/// Waiting for building a building
		/// </summary>
		private static bool isWaitingForBuildingBuilt = false;

		/// <summary>
		/// Waiting for umit creation
		/// </summary>
		private static bool isWaitingForUnitCreation = false;
		
		/// <summary>
		/// Reaction on left mouse button on the map
		/// </summary>
		/// <param name="e"></param>
		public static void MauseLeftClick(MouseEventArgs e)
		{
			if (isLocatingBuilding)
			{
				BuildMessage bm = (BuildMessage)Yad.Net.Client.Utils.CreateMessageWithPlayerId(MessageType.Build);
				bm.BuildingID = GameForm.currPlayer.GenerateObjectID();
				bm.PlayerId = GameForm.currPlayer.ID;
				bm.BuildingType = GameForm.sim.GameSettingsWrapper.GameSettings.BuildingsData.BuildingDataCollection[0].__TypeID;
				bm.Type = MessageType.Build;
				bm.Position = GameGraphics.TranslateMousePosition(e.Location); ;
				bm.IdTurn = GameForm.sim.CurrentTurn + GameForm.sim.Delta;
				GameForm.conn.SendMessage(bm);
				isLocatingBuilding = false;
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
				return IsWaitingForUnitCreation;
			}
		}

		public static void LocateBuilding()
		{
			isLocatingBuilding = true;
		}
	}
}
