using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Yad.Engine.Common;
using Yad.Engine.Client;
using Yad.Net.Messaging.Common;
using System.Threading;

namespace SimTest {
	public partial class Form1 : Form {
		Random rnd = new Random();
		Semaphore s = new Semaphore(0, 1);

		public Form1() {
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e) {
			Simulation sim = new ClientSimulation();
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
	}
}