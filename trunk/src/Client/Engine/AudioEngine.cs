using System;
using System.Collections.Generic;
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
        private FMOD.Channel musicChannel;
        private FMOD.Channel soundChannel;

        private Timer timer;

        private Sound sound;
        private Music music;

        public AudioEngine()
        {
            timer = new Timer();
            //timer.Interval = 100;
            //timer.Tick += new EventHandler(timer_Tick);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            system.update();
        }

        public void Init()
        {
            uint version = 0;
            FMOD.RESULT result;

            /* Create a System object and initialize. */
            result = FMOD.Factory.System_Create(ref system);
            if (!FMOD.ERROR.ERRCHECK(result))
                MessageBoxEx.Show(FMOD.ERROR.String(result), "FMOD Error");

            result = system.getVersion(ref version);
            if (!FMOD.ERROR.ERRCHECK(result))
                MessageBoxEx.Show(FMOD.ERROR.String(result), "FMOD Error");
            if (version < FMOD.VERSION.number)
                MessageBoxEx.Show("Error!  You are using an old version of FMOD " + version.ToString("X") + ".  This program requires " + FMOD.VERSION.number.ToString("X") + ".");

            result = system.init(32, FMOD.INITFLAG.NORMAL, (IntPtr)null);
            if (!FMOD.ERROR.ERRCHECK(result))
                MessageBoxEx.Show(FMOD.ERROR.String(result), "FMOD Error");

            music = new Music(system, musicChannel);
            sound = new Sound(system, soundChannel);

            //timer.Start();

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
