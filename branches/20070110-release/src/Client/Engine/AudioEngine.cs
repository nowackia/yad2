using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Yad.Properties.Client;
using Yad.Utilities.Common;
using Yad.Log.Common;
using Yad.UI.Common;

namespace Yad.Engine.Client
{
    public class AudioEngine
    {
        private static AudioEngine instance = new AudioEngine();

        private FMOD.System system;

        private bool isInitialized;

        private ISynchronizeInvoke invoker;

        private Timer timer;

        private Sound sound;
        private Music music;

        public AudioEngine()
        {
            isInitialized = false;

            timer = new Timer();
            timer.Interval = 100;
            timer.Tick += new EventHandler(timer_Tick);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            system.update();
        }

        private bool InitErrorCheck(FMOD.RESULT result)
        {
            bool isInitialized = FMOD.ERROR.ERRCHECK(result);
            if (!isInitialized)
            {
                isInitialized = false;
                music = new Music();
                sound = new Sound();
                InfoLog.WriteInfo("No audio: " + FMOD.ERROR.String(result), EPrefix.AudioEngine);
                MessageBox.Show(FMOD.ERROR.String(result), "FMOD Error");
            }
            return isInitialized;
        }

        public void Init(ISynchronizeInvoke newInvoker)
        {
            if (!Settings.Default.AudioEngineAvail)
            {
                InitErrorCheck(FMOD.RESULT.ERR_OFF_MANUALLY);
                return;
            }

            uint version = 0;

            /* Create a System object and initialize */
            if(!InitErrorCheck(FMOD.Factory.System_Create(ref system)))
                return;

            if(!InitErrorCheck(system.getVersion(ref version)))
                return;

            if (version < FMOD.VERSION.number)
            {
                InitErrorCheck(FMOD.RESULT.ERR_VERSION);
                return;
            }

            if(!InitErrorCheck(system.init(32, FMOD.INITFLAG.NORMAL, (IntPtr)null)))
                return;

            isInitialized = true;
            invoker = newInvoker;
            music = new Music(system, invoker);
            sound = new Sound(system, invoker);

            timer.Start();

            music.MusicEnd += new MusicEndEventHandler(music_MusicEnd);

            InfoLog.WriteInfo("Audio initialized successfully", EPrefix.AudioEngine);
        }

        void music_MusicEnd(object sender, MusicEndEventArgs e)
        {
            music.PlayNext();
        }

        public static AudioEngine Instance
        {
            get
            { return instance; }
        }

        public ISynchronizeInvoke Invoker
        {
            get { return invoker; }
            set { invoker = value; }
        }

        public bool IsInitialized
        {
            get
            { return isInitialized; }
        }

        public Music Music
        {
            get
            { return music; }
        }

        public Sound Sound
        {
            get
            { return sound; }
        }
    }
}
