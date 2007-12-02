using System;
using System.Collections.Generic;
using System.Text;
using Yad.Log.Common;
using Yad.Properties.Client;

namespace Yad.Engine
{
    public class Sound
    {
        private FMOD.System system = null;
        private FMOD.Channel channel = null;

        private bool isMuted;
        private float volume;

        public Sound(FMOD.System system, FMOD.Channel channel)
        {
            this.system = system;
            this.channel = channel;

            isMuted = false;
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
                if (channel != null)
                    channel.setVolume(volume);

                InfoLog.WriteInfo("Sound volume set to: " + volume, EPrefix.AudioEngine);
            }
        }

        public void LoadSounds()
        {
            this.Volume = (int)Settings.Default.SoundDefaultVolume;

            InfoLog.WriteInfo("Sounds loaded successfully", EPrefix.AudioEngine);
        }

        private bool Play(FMOD.Sound sound)
        {
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
