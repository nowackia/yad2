using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Yad.Properties.Client;
using Yad.Log.Common;
using Yad.UI.Common;
using Yad.Utilities.Common;

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

        private FMOD.System system = null;
        private FMOD.Channel channel = null;

        private List<FMOD.Sound>[] music;
        private short[] indices;
        private MusicType musicType;

        private bool manualMusicEnd;
        private bool isMuted;
        private float volume;

        public Music(FMOD.System system, FMOD.Channel channel)
        {
            this.system = system;
            this.channel = channel;

            manualMusicEnd = false;
            isMuted = false;
            musicType = MusicType.Peace;

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
                if(channel != null)
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
            FMOD.RESULT result;
            music = new List<FMOD.Sound>[Enum.GetValues(typeof(MusicType)).Length];
            indices = new short[music.Length];
            this.Volume = (int)Settings.Default.MusicDefaultVolume;

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
                if(!FMOD.ERROR.ERRCHECK(result))
                    MessageBoxEx.Show(FMOD.ERROR.String(result), "FMOD Error");
                music[(short)MusicType.Fight].Add(sound);
            }

            directoryInfo = new DirectoryInfo(Settings.Default.MusicLose);
            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                sound = new FMOD.Sound();
                result = system.createSound(fileInfo.FullName, FMOD.MODE.SOFTWARE | FMOD.MODE.CREATESTREAM, ref sound);
                if(!FMOD.ERROR.ERRCHECK(result))
                    MessageBoxEx.Show(FMOD.ERROR.String(result), "FMOD Error");
                music[(short)MusicType.Lose].Add(sound);
            }

            directoryInfo = new DirectoryInfo(Settings.Default.MusicPeace);
            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                sound = new FMOD.Sound();
                result = system.createSound(fileInfo.FullName, FMOD.MODE.SOFTWARE | FMOD.MODE.CREATESTREAM, ref sound);
                if(!FMOD.ERROR.ERRCHECK(result))
                    MessageBoxEx.Show(FMOD.ERROR.String(result), "FMOD Error");
                music[(short)MusicType.Peace].Add(sound);
            }

            directoryInfo = new DirectoryInfo(Settings.Default.MusicWin);
            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                sound = new FMOD.Sound();
                result = system.createSound(fileInfo.FullName, FMOD.MODE.SOFTWARE | FMOD.MODE.CREATESTREAM, ref sound);
                if(!FMOD.ERROR.ERRCHECK(result))
                    MessageBoxEx.Show(FMOD.ERROR.String(result), "FMOD Error");
                music[(short)MusicType.Win].Add(sound);
            }

            InfoLog.WriteInfo("Music loaded successfully", EPrefix.AudioEngine);
        }

        private FMOD.RESULT endPlayCallbackFunction(IntPtr channelRaw, FMOD.CHANNEL_CALLBACKTYPE tipo, int comando, uint datoComando1, uint datoComando2)
        {
            if (MusicEnd != null && manualMusicEnd)
                MusicEnd(this, new MusicEndEventArgs(musicType));
            return FMOD.RESULT.OK;
        }

        private bool Play(FMOD.Sound sound)
        {
            bool isPlaying = false;

            if (channel != null)
                channel.isPlaying(ref isPlaying);
            else
                isPlaying = false;

            if (isPlaying && channel != null)
            {
                manualMusicEnd = false;
                channel.stop();
                channel = null;
            }

            system.update();

            FMOD.RESULT result = system.playSound(FMOD.CHANNELINDEX.FREE, sound, false, ref channel);
            channel.setVolume(volume);
            channel.setCallback(FMOD.CHANNEL_CALLBACKTYPE.END, endPlayCallback, 0);

            return FMOD.ERROR.ERRCHECK(result);
        }

        public bool Play(MusicType mt)
        {
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
            List<FMOD.Sound> tracks = music[(short)mt];
            if (tracks.Count == 0)
                return false;

            musicType = mt;

            short index = indices[(short)mt] = (short)((indices[(short)mt] + 1) % tracks.Count);

            return this.Play(tracks[index]);
        }

        public bool PlayRandom()
        {
            return this.PlayRandom(musicType);
        }

        public bool PlayRandom(MusicType mt)
        {
            List<FMOD.Sound> tracks = music[(short)mt];
            if (tracks.Count == 0)
                return false;

            musicType = mt;

            short index = indices[(short)mt] = Randomizer.NextShort((short)tracks.Count);

            return this.Play(tracks[index]);
        }

        public bool Stop()
        {
            manualMusicEnd = true;
            if (channel != null)
                return FMOD.ERROR.ERRCHECK(channel.stop());
            else
                return false;
        }

        public void Mute()
        {
            isMuted = true;
            this.Volume = 0;

            InfoLog.WriteInfo("Sound muted", EPrefix.AudioEngine);
        }
    }
}
