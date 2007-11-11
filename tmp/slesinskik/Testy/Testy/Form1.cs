using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Testy {
	public partial class Form1 : Form {

		Semaphore s = new Semaphore(1, 1);

		public Form1() {
			InitializeComponent();

		}

		private void lockingtest() {
			lock (s) {
				Console.Out.WriteLine("entered");
				Console.Out.WriteLine(s.WaitOne());
				Console.Out.WriteLine(s.WaitOne());
				Console.Out.WriteLine("exited");
				Console.Out.WriteLine("entered");
				Console.Out.WriteLine(s.WaitOne(0, false));
				Console.Out.WriteLine("exited");
				Console.Out.WriteLine(s.Release());
				Console.Out.WriteLine(s.Release());
				Console.Out.WriteLine(s.Release());
			}
		}

		private void Form1_Load(object sender, EventArgs e) {
			//lockingtest();
			threadtest();
		}

		private void threadtest() {
			Thread t = new Thread(new ParameterizedThreadStart(run));
			t.Start();

			t.Abort();
			t.Start();
		}

		void run(Object o) {
			Console.Out.WriteLine("start");
			while (true) { };
		}
	}
}