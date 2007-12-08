using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Yad.Board;
using Yad.Net.Messaging.Common;
using Yad.Engine.Common;
using Yad.Engine.Client;
using Yad.Config;
using Yad.Board.Common;
using Yad.Config.XMLLoader.Common;
using Yad.Net.Client;
using Yad.UI.Common;
using Yad.DataStructures;
using Yad.AI;
using Yad.AI.General;
using Yad.Config.Common;
using Yad.Algorithms;


namespace Tests
{
    public partial class TestForm : Form, IPositionChecker
    {
        public TestForm()
        {
            TestAStar();
            InitializeComponent();
            InitFMod();
            LoadSounds();
            LoadMidi();
        }

        #region Direction
        public void TestDirection()
        {
            Direction d = new Direction();
            Console.WriteLine(d.ToString());
            d = Direction.East;
            Console.WriteLine(d.ToString());
            d = Direction.South;
            d = Direction.North;
            Console.WriteLine(d.ToString());
            d = Direction.East | Direction.South;
            Console.WriteLine(d.ToString());

            for (int i = 0; i < 32; i++)
            {
                Console.Out.WriteLine(((Direction)i).ToString());
            }
        }
        #endregion

        #region Simulation
        Random rnd = new Random();
        Semaphore s = new Semaphore(0, 1);

        public void TestAStar() {
            Map map = new Map();
            map.LoadMap(Path.Combine("Resources/Maps", "small.map"));
            Simulation sim = new ClientSimulation(map);
            Unit u = new UnitTrooper(new ObjectID(1, 1), GlobalSettings.Wrapper.Troopers[0], new Position(0, 0), map, sim);
            map.Units[0, 1].AddFirst(u);
            map.Units[1, 1].AddFirst(u);
            map.Units[2, 1].AddFirst(u);
            map.Units[3, 0].AddFirst(u);
            map.Units[4, 1].AddFirst(u);
            map.Units[3, 2].AddFirst(u);
            map.Units[4, 2].AddFirst(u);
            map.Units[3, 3].AddFirst(u);
            MapInput mi = new MapInput(map);
            mi.IsMoveable += new MapInput.MoveCheckDelegate(IsMoveable);
            mi.Start = new Position(0, 0);
            mi.Goal = new Position(15, 17);
            Queue<Position> path = AStar.Search<Position>(mi);
            if (path.Count != 0)
                Console.WriteLine("OK");
        }

        public bool IsMoveable(short x, short y, Map map)
        {
            if (map.Units[x, y].Count == 0 && map.Buildings[x, y].Count == 0)
                return true;
            return false;
        }

        public void TestMidpoint(){
            Position pos = UtilsAlgorithm.SurroundSearch(new Position(0, 0), 5, this);
            /*List<Position> list1 = Midpoint.MidpointCircle(1);
            List<Position> list2 = Midpoint.MidpointCircle(2);
            List<Position> list3 = Midpoint.MidpointCircle(3);*/
        }

        public void TestPriorityQueue() {
            PriorityQueue<int> queue = new PriorityQueue<int>(5);
            int[] values = new int[5];
            values[0] = 1;
            values[1] = 2;
            values[2] = 5;
            values[3] = 4;
            values[4] = 3;

            for (int i = 0; i < values.Length; ++i) {
                queue.Insert(values[i]);
            }
            Array.Sort(values);
            for (int i = 0; i < values.Length; ++i) {
                if (values[i] == queue.Remove())
                    Console.WriteLine("Ok");
                else
                    Console.WriteLine("Error");
            }
        }
        public void TestSimulaton()
        {
            Simulation sim;
            Map map = new Map();
            map.LoadMap(Path.Combine("Resources/Maps", "test.map"));
            sim = new ClientSimulation(map);
            sim.onTurnEnd += new SimulationHandler(sim_onTurnEnd);
            sim.StartSimulation();

            int msgCount = 0;

            while (msgCount < 1000)
            {
                GameMessage gm = generate();
                gm.IdTurn = sim.CurrentTurn + 1 + rnd.Next(2 * sim.Delta);
                sim.AddGameMessage(gm);

                Thread.Sleep(rnd.Next(200));

                if (rnd.Next(4) == 0)
                {
                    s.WaitOne();
                    sim.DoTurn();
                }

                msgCount++;
            }
        }

        void sim_onTurnEnd()
        {
            s.Release();
        }

        private GameMessage generate()
        {
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

        #region Sound
        private FMOD.System system = null;
        private FMOD.Sound[] sounds = new FMOD.Sound[2];
        private FMOD.Channel channel = null;
        private uint ms = 0;
        private uint lenms = 0;
        private bool playing = false;
        private bool paused = false;
        private int channelsplaying = 0;

        private void InitFMod()
        {
            uint version = 0;
            FMOD.RESULT result;

            /* Create a System object and initialize. */
            result = FMOD.Factory.System_Create(ref system);
            ERRCHECK(result);

            result = system.getVersion(ref version);
            ERRCHECK(result);
            if (version < FMOD.VERSION.number)
            {
                MessageBox.Show("Error!  You are using an old version of FMOD " + version.ToString("X") + ".  This program requires " + FMOD.VERSION.number.ToString("X") + ".");
                Application.Exit();
            }

            result = system.init(32, FMOD.INITFLAG.NORMAL, (IntPtr)null);
            ERRCHECK(result);
        }

        private void LoadSounds()
        {
            FMOD.RESULT result;

            sounds[0] = new FMOD.Sound();
            result = system.createSound("FancyPants.wav", FMOD.MODE.HARDWARE, ref sounds[0]);
            ERRCHECK(result);
        }

        private void LoadMidi()
        {
            FMOD.RESULT result;

            sounds[1] = new FMOD.Sound();
            result = system.createSound("dune_win_01.MID", FMOD.MODE.SOFTWARE | FMOD.MODE.CREATESTREAM, ref sounds[1]);
            ERRCHECK(result);
        }

        private void ERRCHECK(FMOD.RESULT result)
        {
            if (result != FMOD.RESULT.OK)
            {
                MessageBox.Show("FMOD error! " + result + " - " + FMOD.ERROR.String(result));
                Environment.Exit(-1);
            }
        }
        #endregion

        #region Audio
        private void btnRandom_Click(object sender, EventArgs e)
        {
            MusicType mt;
            if (rbFight.Checked)
            {
                mt = Yad.Engine.Client.MusicType.Fight;
            }
            else if (rbLose.Checked)
            {
                mt = MusicType.Lose;
            }
            else if (rbPeace.Checked)
            {
                mt = MusicType.Peace;
            }
            else
            {
                mt = MusicType.Win;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            MusicType mt;
            if (rbFight.Checked)
            {
                mt = Yad.Engine.Client.MusicType.Fight;
            }
            else if (rbLose.Checked)
            {
                mt = MusicType.Lose;
            }
            else if (rbPeace.Checked)
            {
                mt = MusicType.Peace;
            }
            else
            {
                mt = MusicType.Win;
            }
        }

        #endregion

        #region Dictionary & enums
        public void testDictionary()
        {
            Dictionary<int, Object> testDict = new Dictionary<int, object>();
            int len = 1000000;
            for (int i = 0; i < len; i++)
            {
                testDict.Add(i, new Object());
            }

            Object[] a = new Object[len];
            int beg = Environment.TickCount;
            int c = 0;
            Dictionary<int, Object>.Enumerator en = testDict.GetEnumerator();
            while (en.MoveNext())
            {
                a[c] = en.Current.Value;
                c++;
            }
            int end = Environment.TickCount;
            en.Dispose();
            Console.Out.WriteLine((end - beg).ToString());
            beg = Environment.TickCount;
            c = 0;
            foreach (Object o in testDict.Values)
            {
                a[c] = o;
                c++;
            }
            end = Environment.TickCount;
            Console.Out.WriteLine((end - beg).ToString());

            for (int z = 0; z < 1000; z++)
            {
                c = 0;
                foreach (Object o in testDict.Values)
                {
                    if (a[c] != o)
                    {
                        Console.Out.WriteLine("Difference");
                    }
                    c++;
                }
            }
        }
        #endregion

        private void btnTestDict_Click(object sender, EventArgs e)
        {
            FMOD.RESULT result;

            result = system.playSound(FMOD.CHANNELINDEX.FREE, sounds[0], false, ref channel);
            ERRCHECK(result);
        }

        private void buttonTestMidi_Click(object sender, EventArgs e)
        {
            FMOD.RESULT result;

            result = system.playSound(FMOD.CHANNELINDEX.FREE, sounds[1], false, ref channel);
            ERRCHECK(result);
        }




        #region IPositionChecker Members

        bool IPositionChecker.CheckPosition(short x, short y)
        {
            return false;
        }

        #endregion
    }
}