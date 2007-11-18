using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Yad.Board;
using System.Threading;
using Yad.Net.Messaging.Common;
using Yad.Engine.Common;
using Yad.Engine.Client;
using Yad.Config;
using Yad.Board.Common;
using Yad.Config.XMLLoader.Common;
using System.IO;

namespace KaleeTests {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e) {
			TestDirection();
		}

		#region direction
		public void TestDirection() {
			Direction d = new Direction();
			Console.WriteLine(d.ToString());
			d = Direction.East;
			Console.WriteLine(d.ToString());
			d = Direction.South;
			d = Direction.North;
			Console.WriteLine(d.ToString());
			d = Direction.East | Direction.South;
			Console.WriteLine(d.ToString());

			for (int i = 0; i < 32; i++) {
				Console.Out.WriteLine(((Direction)i).ToString());
			}
		}
		#endregion


		#region simulation
		Random rnd = new Random();
		Semaphore s = new Semaphore(0, 1);

		public void TestSimulaton() {
			Simulation sim;
			GameSettingsWrapper gameSettingsWrapper = XMLLoader.get("dune_data.xml", "dune.xsd");
			Map map = new Map();
			map.LoadMap(Path.Combine("Resources/Maps", "test.map"));
			sim = new ClientSimulation(gameSettingsWrapper, map, null);
			sim.onTurnEnd += new SimulationHandler(sim_onTurnEnd);
			sim.StartSimulation();

			int msgCount = 0;

			while (msgCount < 1000) {
				GameMessage gm = generate();
				gm.IdTurn = sim.CurrentTurn + 1 + rnd.Next(2 * sim.Delta);
				sim.AddGameMessage(gm);

				Thread.Sleep(rnd.Next(200));

				if (rnd.Next(4) == 0) {
					s.WaitOne();
					sim.DoTurn();
				}

				msgCount++;
			}
		}

		void sim_onTurnEnd() {
			s.Release();
		}

		private GameMessage generate() {
			int r = rnd.Next(6);
			if (r < 1)
				return new MoveMessage();
			if (r < 2)
				return new AttackMessage();
			if (r < 3)
				return new BuildMessage();
			if (r < 4)
				return new CreateUnitMessage();
			if (r < 5)
				return new DestroyMessage();

			return new HarvestMessage();
		}
		#endregion
	}
}