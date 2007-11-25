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
using Yad.Net.Client;

namespace Tests {
	public partial class TestForm : Form {
		public TestForm() {
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e) {
			AudioEngine.Init();
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
			Map map = new Map();
			map.LoadMap(Path.Combine("Resources/Maps", "test.map"));
			sim = new ClientSimulation(map);
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

		#region Audio
		private void btnRandom_Click(object sender, EventArgs e) {
			AudioEngine.MusicType mt;
			if (rbFight.Checked) {
				mt = Yad.Engine.Client.AudioEngine.MusicType.Fight;
			} else if (rbLose.Checked) {
				mt = AudioEngine.MusicType.Lose;
			} else if (rbPeace.Checked) {
				mt = AudioEngine.MusicType.Peace;
			} else {
				mt = AudioEngine.MusicType.Win;
			}
			AudioEngine.PlayRandom(mt);
		}

		private void btnNext_Click(object sender, EventArgs e) {
			AudioEngine.MusicType mt;
			if (rbFight.Checked) {
				mt = Yad.Engine.Client.AudioEngine.MusicType.Fight;
			} else if (rbLose.Checked) {
				mt = AudioEngine.MusicType.Lose;
			} else if (rbPeace.Checked) {
				mt = AudioEngine.MusicType.Peace;
			} else {
				mt = AudioEngine.MusicType.Win;
			}
			AudioEngine.PlayNext(mt);
		}

		#endregion

		#region dictionary & enums
		public void testDictionary() {
			Dictionary<int, Object> testDict = new Dictionary<int, object>();
			int len = 1000000;
			for (int i = 0; i < len; i++) {
				testDict.Add(i, new Object());
			}

			Object[] a = new Object[len];
			int beg = Environment.TickCount;
			int c = 0;
			Dictionary<int, Object>.Enumerator en = testDict.GetEnumerator();
			while (en.MoveNext()) {
				a[c] = en.Current.Value;
				c++;
			}
			int end = Environment.TickCount;
			en.Dispose();
			Console.Out.WriteLine((end - beg).ToString());
			beg = Environment.TickCount;
			c = 0;
			foreach (Object o in testDict.Values) {
				a[c] = o;
				c++;
			}
			end = Environment.TickCount;
			Console.Out.WriteLine((end - beg).ToString());

			for (int z = 0; z < 1000; z++) {
				c = 0;
				foreach (Object o in testDict.Values) {
					if (a[c] != o) {
						Console.Out.WriteLine("Difference");
					}
					c++;
				}
			}
		}
		#endregion

		private void btnTestDict_Click(object sender, EventArgs e) {
			this.testDictionary();
		}
	}
}