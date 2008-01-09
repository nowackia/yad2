using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using Yad.Properties.Client;
using Yad.Log.Common;
using Yad.UI.Common;
using Yad.Utilities.Common;
using Yad.Config.INILoader.Common;

namespace Yad.Engine.Client
{
    public enum MusicType : short
    {
        Fight = 0,
        Lose,
        Win,
        Peace
    }

    public delegate void MusicEndEventHandler(object sender, MusicEndEventArgs e);
    public delegate bool PlayMusicEventHandler(FMOD.Sound sound);

    public class MusicEndEventArgs : EventArgs
    {
        public MusicType musicType;

        public MusicEndEventArgs(MusicType musicType)
        {
            this.musicType = musicType;
        }
    }

    public class Music
    {
        public event MusicEndEventHandler MusicEnd;
        private FMOD.CHANNEL_CALLBACK endPlayCallback;

        private FMOD.Channel channel = null;
        private FMOD.System system = null;

        private List<FMOD.Sound>[] music;
        private short[] indices;
        private MusicType musicType;

        private bool manualMusicEnd;
        private bool isMuted;
        private float volume;
        private ISynchronizeInvoke invoker;
        private bool isInitialized;

        public Music()
            : this(null, null, false)
        { }

        public Music(FMOD.System system, ISynchronizeInvoke invoker)
            : this(system, invoker, true)
        { }

        public Music(FMOD.System system, ISynchronizeInvoke invoker, bool isInitialized)
        {
            this.system = system;
            this.invoker = invoker;
            this.isInitialized = isInitialized;

            if (isInitialized && system == null)
                isInitialized = false;

            musicType = MusicType.Peace;
            manualMusicEnd = false;
            isMuted = false;

            endPlayCallback = new FMOD.CHANNEL_CALLBACK(endPlayCallbackFunction);
        }

        public bool IsMuted
        {
            get
            { return isMuted; }
        }

        public int Volume
        {
            get
            { return (int)Math.Round(volume * 100.0f); }
            set
            {
                isMuted = false;
                volume = (float)value / 100.0f;

                if (!isMuted && channel != null)
                    channel.setVolume(volume);

                InfoLog.WriteInfo("Music volume set to: " + volume, EPrefix.AudioEngine);
            }
        }

        public MusicType MusictType
        {
            get
            { return musicType; }
        }

        public void LoadMusic()
        {
            if (!isInitialized)
                return;

            FMOD.RESULT result;
            music = new List<FMOD.Sound>[Enum.GetValues(typeof(MusicType)).Length];
            indices = new short[music.Length];

            this.Volume = InitializationSettings.Instance.MusicVolume;
            if (InitializationSettings.Instance.IsMusicMuted)
                this.Mute();

            for (int i = 0; i < music.Length; i++)
            {
                music[i] = new List<FMOD.Sound>();
                indices[i] = 0;
            }

            DirectoryInfo directoryInfo = null;
            FMOD.Sound sound = null;

            directoryInfo = new DirectoryInfo(Settings.Default.MusicFight);
            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                sound = new FMOD.Sound();
                result = system.createSound(fileInfo.FullName, FMOD.MODE.SOFTWARE | FMOD.MODE.CREATESTREAM, ref sound);
                if (!FMOD.ERROR.ERRCHECK(result))
                    InfoLog.WriteError(fileInfo.Name + ": " + FMOD.ERROR.String(result), EPrefix.AudioEngine);
                music[(short)MusicType.Fight].Add(sound);
            }

            directoryInfo = new DirectoryInfo(Settings.Default.MusicLose);
            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                sound = new FMOD.Sound();
                result = system.createSound(fileInfo.FullName, FMOD.MODE.SOFTWARE | FMOD.MODE.CREATESTREAM, ref sound);
                if (!FMOD.ERROR.ERRCHECK(result))
                    InfoLog.WriteError(fileInfo.Name + ": " + FMOD.ERROR.String(result), EPrefix.AudioEngine);
                music[(short)MusicType.Lose].Add(sound);
            }

            directoryInfo = new DirectoryInfo(Settings.Default.MusicPeace);
            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                sound = new FMOD.Sound();
                result = system.createSound(fileInfo.FullName, FMOD.MODE.SOFTWARE | FMOD.MODE.CREATESTREAM, ref sound);
                if (!FMOD.ERROR.ERRCHECK(result))
                    InfoLog.WriteError(fileInfo.Name + ": " + FMOD.ERROR.String(result), EPrefix.AudioEngine);
                music[(short)MusicType.Peace].Add(sound);
            }

            directoryInfo = new DirectoryInfo(Settings.Default.MusicWin);
            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                sound = new FMOD.Sound();
                result = system.createSound(fileInfo.FullName, FMOD.MODE.SOFTWARE | FMOD.MODE.CREATESTREAM, ref sound);
                if (!FMOD.ERROR.ERRCHECK(result))
                    InfoLog.WriteError(fileInfo.Name + ": " + FMOD.ERROR.String(result), EPrefix.AudioEngine);
                music[(short)MusicType.Win].Add(sound);
            }

            InfoLog.WriteInfo("Finished loading music", EPrefix.AudioEngine);
        }

        private FMOD.RESULT endPlayCallbackFunction(IntPtr channelRaw, FMOD.CHANNEL_CALLBACKTYPE tipo, int comando, uint datoComando1, uint datoComando2)
        {
            lock (syncMusicCallback)
            {
                InfoLog.WriteInfo(DateTime.Now.ToString() + " [ Before Fire music end event, Manual music end: " + manualMusicEnd, EPrefix.AudioEngine);
                if (MusicEnd != null && !manualMusicEnd)
                {
                    MusicEnd(this, new MusicEndEventArgs(musicType));
                    InfoLog.WriteInfo(DateTime.Now.ToString() + " ] After Fire music end event", EPrefix.AudioEngine);
                }
                manualMusicEnd = false;
                return FMOD.RESULT.OK;
            }
        }

        private bool Play(FMOD.Sound sound)
        {
            if (!isInitialized)
                return false;

            if (invoker != null)
            {
                if (invoker.InvokeRequired)
                    return (bool)invoker.Invoke(new PlayMusicEventHandler(PlayMusicCallback), new object[] { sound });
                else
                    return PlayMusicCallback(sound);
            }
            else
                return false;
        }

        private object syncMusicCallback = new object();

        private bool PlayMusicCallback(FMOD.Sound sound)
        {
            FMOD.RESULT result;

            lock (syncMusicCallback)
            {
                bool isPlaying = false;
                manualMusicEnd = false;

                if (channel != null)
                    channel.isPlaying(ref isPlaying);
                else
                    isPlaying = false;

                if (isPlaying && channel != null)
                {
                    manualMusicEnd = true;
                    InfoLog.WriteInfo(DateTime.Now.ToString() + "   | Music is playing, Manual music end: " + manualMusicEnd, EPrefix.AudioEngine);
                    channel.setMute(true);
                    channel.stop();
                    channel = null;
                }
                else
                    InfoLog.WriteInfo(DateTime.Now.ToString() + "   | Music is not playing, Manual music end: " + manualMusicEnd, EPrefix.AudioEngine);

                result = system.playSound(FMOD.CHANNELINDEX.REUSE, sound, true, ref channel);
                if (result == FMOD.RESULT.OK && channel != null)
                {
                    channel.setVolume(volume);
                    channel.setCallback(FMOD.CHANNEL_CALLBACKTYPE.END, endPlayCallback, 0);
                    InfoLog.WriteInfo(DateTime.Now.ToString() + "   | Before Play music begin", EPrefix.AudioEngine);
                    channel.setPaused(false);
                }
            }

            InfoLog.WriteInfo(DateTime.Now.ToString() + "   | After Play music begin, result: " + result, EPrefix.AudioEngine);

            return FMOD.ERROR.ERRCHECK(result);
        }

        public bool Play(MusicType mt)
        {
            if (!isInitialized)
                return false;

            if (mt != musicType)
            {
                musicType = mt;
                return this.PlayNext(mt);
            }
            else
                return false;
        }

        public bool PlayNext()
        {
            return this.PlayNext(musicType);
        }

        public bool PlayNext(MusicType mt)
        {
            if (!isInitialized)
                return false;

            List<FMOD.Sound> tracks = music[(short)mt];
            if (tracks.Count == 0)
                return false;

            musicType = mt;

            short index = indices[(short)mt] = (short)((indices[(short)mt] + 1) % tracks.Count);

            InfoLog.WriteInfo(DateTime.Now.ToString() + "  - Changed track to: " + index + ", Music Type: " + mt.ToString(), EPrefix.AudioEngine);

            return this.Play(tracks[index]);
        }

        public bool PlayRandom()
        {
            return this.PlayRandom(musicType);
        }

        public bool PlayRandom(MusicType mt)
        {
            if (!isInitialized)
                return false;

            List<FMOD.Sound> tracks = music[(short)mt];
            if (tracks.Count == 0)
                return false;

            musicType = mt;

            short index = indices[(short)mt] = Randomizer.NextShort((short)tracks.Count);

            return this.Play(tracks[index]);
        }

        public bool Stop()
        {
            if (!isInitialized)
                return false;

            manualMusicEnd = true;
            if (channel != null)
                return FMOD.ERROR.ERRCHECK(channel.stop());
            else
                return false;
        }

        public void Mute()
        {
            this.Volume = 0;
            isMuted = true;

            InfoLog.WriteInfo("Music muted", EPrefix.AudioEngine);
        }
    }
}
