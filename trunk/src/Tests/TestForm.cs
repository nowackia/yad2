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
using Tao.OpenAl;

namespace Tests
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();

            // Initialize OpenAL and clear the error bit.
            Alut.alutInit();
            Al.alGetError();

            // Load the wav data.
            if (!LoadALData())
            {
                Console.WriteLine("Error loading data.");
                return;
            }

            // Initialize the listener in OpenAL.
            SetListenerValues();
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
        private static int buffer; // Buffers to hold sound data.
        private static int source; // Sources are points of emitting sound.

        private static float[] sourcePosition = { 0, 0, 0 }; // Position of the source sound.
        private static float[] sourceVelocity = { 0, 0, 0 }; // Velocity of the source sound.
        private static float[] listenerPosition = { 0, 0, 0 }; // Position of the Listener.
        private static float[] listenerVelocity = { 0, 0, 0 }; // Velocity of the Listener.

        private static float[] listenerOrientation = { 0, 0, -1, 0, 1, 0 };

        /*
        * We have allocated memory for our buffers and sources which needs
        * to be returned to the system. This function frees that memory.
        */
        private static void KillALData()
        {
            Al.alDeleteBuffers(1, ref buffer);
            Al.alDeleteSources(1, ref source);
            Alut.alutExit();
        }

        /*
         * This function will load our sample data from the disk using the Alut
         * utility and send the data into OpenAL as a buffer. A source is then
         * also created to play that buffer.
         */
        private static bool LoadALData()
        {
            // Variables to load into.
            int format;
            int size;
            IntPtr data = IntPtr.Zero;
            float frequency;

            // Generate an OpenAL buffer.
            Al.alGenBuffers(1, out buffer);
            if (Al.alGetError() != Al.AL_NO_ERROR)
                return false;

            // Attempt to locate the file.
            string fileName = "OpenAlExamples.Lesson01.FancyPants.wav";
            if (!File.Exists(fileName))
                return false;

            // Load wav.
            data = Alut.alutLoadMemoryFromFile(fileName, out format, out size, out frequency);
            if (data == IntPtr.Zero)
                return false;

            // Load wav data into the generated buffer.
            Al.alBufferData(buffer, format, data, size, (int)frequency);

            // Generate an OpenAL source.
            Al.alGenSources(1, out source);
            if (Al.alGetError() != Al.AL_NO_ERROR)
                return false;

            // Bind the buffer with the source.
            Al.alSourcei(source, Al.AL_BUFFER, buffer);
            Al.alSourcef(source, Al.AL_PITCH, 1.0f);
            Al.alSourcef(source, Al.AL_GAIN, 1.0f);
            Al.alSourcefv(source, Al.AL_POSITION, sourcePosition);
            Al.alSourcefv(source, Al.AL_VELOCITY, sourceVelocity);
            Al.alSourcei(source, Al.AL_LOOPING, 0);

            // Do a final error check and then return.
            if (Al.alGetError() == Al.AL_NO_ERROR)
                return true;

            return false;
        }

        /*
         * We already defined certain values for the Listener, but we need
         * to tell OpenAL to use that data. This function does just that.
         */
        private static void SetListenerValues()
        {
            Al.alListenerfv(Al.AL_POSITION, listenerPosition);
            Al.alListenerfv(Al.AL_VELOCITY, listenerVelocity);
            Al.alListenerfv(Al.AL_ORIENTATION, listenerOrientation);
        }
        #endregion

        #region Audio
        private void btnRandom_Click(object sender, EventArgs e)
        {
            AudioEngine.MusicType mt;
            if (rbFight.Checked)
            {
                mt = Yad.Engine.Client.AudioEngine.MusicType.Fight;
            }
            else if (rbLose.Checked)
            {
                mt = AudioEngine.MusicType.Lose;
            }
            else if (rbPeace.Checked)
            {
                mt = AudioEngine.MusicType.Peace;
            }
            else
            {
                mt = AudioEngine.MusicType.Win;
            }
            AudioEngine.PlayRandom(mt);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            AudioEngine.MusicType mt;
            if (rbFight.Checked)
            {
                mt = Yad.Engine.Client.AudioEngine.MusicType.Fight;
            }
            else if (rbLose.Checked)
            {
                mt = AudioEngine.MusicType.Lose;
            }
            else if (rbPeace.Checked)
            {
                mt = AudioEngine.MusicType.Peace;
            }
            else
            {
                mt = AudioEngine.MusicType.Win;
            }
            AudioEngine.PlayNext(mt);
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
            MessageBoxEx.Show(this, "test");
            Al.alSourcePlay(source);
        }

        private void TestForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            KillALData();
        }
    }
}