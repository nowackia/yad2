using System;
using System.Collections.Generic;
using System.Text;
using Yad.Log.Common;
using Yad.Properties.Client;
using Yad.UI.Common;
using Yad.Utilities.Common;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using Yad.Config;
using Yad.Config.INILoader.Common;

namespace Yad.Engine
{
    #region Misc sound file names
    public enum MiscSoundType : short
    {
        [FileName("acknowledged.wav")]
        Acknowledged = 0,
        [FileName("affirmative.wav")]
        Affirmative,
        [FileName("bloom.wav")]
        Bloom,
        [FileName("buttonClick.wav")]
        ButtonClick,
        [FileName("cannot.wav")]
        Cannot,
        [FileName("comeToPappa.wav")]
        ComeToPappa,
        [FileName("credit.wav")]
        Credit,
        [FileName("crush.wav")]
        Crush,
        [FileName("drop.wav")]
        Drop,
        [FileName("gas.wav")]
        Gas,
        [FileName("gun.wav")]
        Gun,
        [FileName("gunTurret.wav")]
        GunTurret,
        [FileName("infantryOut.wav")]
        InfantryOut,
        [FileName("largeExplosion.wav")]
        LargeExplosion,
        [FileName("machineGun.wav")]
        MachineGun,
        [FileName("mediumExplosion.wav")]
        MediumExplosion,
        [FileName("movingOut.wav")]
        MovingOut,
        [FileName("placeStructure.wav")]
        PlaceStructure,
        [FileName("radar.wav")]
        Radar,
        [FileName("reporting.wav")]
        Reporting,
        [FileName("rocket.wav")]
        Rocket,
        [FileName("scream1.wav")]
        Scream1,
        [FileName("scream2.wav")]
        Scream2,
        [FileName("scream3.wav")]
        Scream3,
        [FileName("scream4.wav")]
        Scream4,
        [FileName("scream5.wav")]
        Scream5,
        [FileName("smallExplosion.wav")]
        SmallExplosion,
        [FileName("smallRocket.wav")]
        SmallRocket,
        [FileName("sonic.wav")]
        Sonic,
        [FileName("structureExplosion.wav")]
        StructureExplosion,
        [FileName("wormAttack.wav")]
        WormAttack,
        [FileName("wormSign.wav")]
        WormSign,
        [FileName("yesSir.wav")]
        YesSir,
    }
    #endregion

    #region House specific sound file names
    public enum HouseSoundType : short
    {
        [FileName("apprch.wav")]
        Approach = 0,
        [FileName("arrive.wav")]
        Arrive,
        [FileName("Atreides.wav")]
        Atreides,
        [FileName("attack.wav")]
        Attack,
        [FileName("bloom.wav")]
        Bloom,
        [FileName("capture.wav")]
        Capture,
        [FileName("const.wav")]
        Const,
        [FileName("deploy.wav")]
        Deploy,
        [FileName("destroy.wav")]
        Destroy,
        [FileName("east.wav")]
        East,
        [FileName("enemy.wav")]
        Enemy,
        [FileName("field.wav")]
        Field,
        [FileName("five.wav")]
        Five,
        [FileName("four.wav")]
        Four,
        [FileName("fremen.wav")]
        Fremen, 
        [FileName("frigate.wav")]
        Frigate,
        [FileName("Harkonnen.wav")]
        Harkonnen,
        [FileName("harvest.wav")]
        Harvest,
        [FileName("house.wav")]
        House,
        [FileName("launch.wav")]
        Launch,
        [FileName("located.wav")]
        Located,
        [FileName("lose.wav")]
        Lose,
        [FileName("merc.wav")]
        Merc,
        [FileName("missile.wav")]
        Missile,
        [FileName("north.wav")]
        North,
        [FileName("off.wav")]
        Off,
        [FileName("on.wav")]
        On,
        [FileName("one.wav")]
        One,
        [FileName("Ordos.wav")]
        Ordos,
        [FileName("radar.wav")]
        Radar,
        [FileName("repair.wav")]
        Repair,
        [FileName("sabot.wav")]
        Sabot,
        [FileName("sard.wav")]
        Sard,
        [FileName("south.wav")]
        South,
        [FileName("struct.wav")]
        Struct,
        [FileName("three.wav")]
        Three,
        [FileName("two.wav")]
        Two,
        [FileName("unit.wav")]
        Unit,
        [FileName("vehicle.wav")]
        Vehicle,
        [FileName("warning.wav")]
        Warning,
        [FileName("west.wav")]
        West,
        [FileName("win.wav")]
        Win,
        [FileName("wormy.wav")]
        Wormy
    }
    #endregion

    public class Sound
    {
        private FMOD.System system = null;
        private FMOD.Channel channel = null;
        private FMOD.CHANNEL_CALLBACK endPlayCallback;

        private short houseId;
        private List<HouseSoundType> sequentialPlayList;

        private FMOD.Sound[] misc;
        private Dictionary<short, FMOD.Sound[]> houses;

        private bool isMuted;
        private float volume;
        private bool isInitialized;

        public Sound(FMOD.Channel channel)
            : this(null, channel, false)
        { }

        public Sound(FMOD.System system, FMOD.Channel channel)
            : this(system, channel, true)
        { }

        public Sound(FMOD.System system, FMOD.Channel channel, bool isInitialized)
        {
            this.system = system;
            this.channel = channel;
            this.isInitialized = isInitialized;

            if (isInitialized && system == null)
                isInitialized = false;

            misc = new FMOD.Sound[Enum.GetValues(typeof(MiscSoundType)).Length];
            houses = new Dictionary<short, FMOD.Sound[]>();

            sequentialPlayList = new List<HouseSoundType>();
            endPlayCallback = new FMOD.CHANNEL_CALLBACK(endPlayCallbackFunction);

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
                if (!isInitialized || channel != null)
                    channel.setVolume(volume);

                InfoLog.WriteInfo("Sound volume set to: " + volume, EPrefix.AudioEngine);
            }
        }

        public void LoadSounds()
        {
            if (!isInitialized)
                return;

            FMOD.RESULT result;

            this.Volume = InitializationSettings.Instance.SoundVolume;
            if (InitializationSettings.Instance.IsSoundMuted)
                this.Mute();

             FMOD.Sound sound = null;

            foreach (MiscSoundType miscType in Enum.GetValues(typeof(MiscSoundType)))
            {
                sound = new FMOD.Sound();
                result = system.createSound(Settings.Default.SoundMisc + GetFileName(miscType), FMOD.MODE.HARDWARE, ref sound);
                if (!FMOD.ERROR.ERRCHECK(result))
                    InfoLog.WriteError(GetFileName(miscType) + ": " + FMOD.ERROR.String(result), EPrefix.AudioEngine);
                misc[(short)miscType] = sound;
            }

            foreach (short houseId in GlobalSettings.Instance.GetHouseIDs())
            {
                FMOD.Sound[] tableHouse = new FMOD.Sound[Enum.GetValues(typeof(HouseSoundType)).Length];

                foreach (HouseSoundType houseType in Enum.GetValues(typeof(HouseSoundType)))
                {
                    sound = new FMOD.Sound();
                    result = system.createSound(Settings.Default.Sound + GlobalSettings.Instance.GetHouseName(houseId) + "/" + GetFileName(houseType), FMOD.MODE.HARDWARE, ref sound);
                    if (!FMOD.ERROR.ERRCHECK(result))
                        InfoLog.WriteError(GetFileName(houseType) + ": " + FMOD.ERROR.String(result), EPrefix.AudioEngine);
                    tableHouse[(short)houseType] = sound;
                }

                houses.Add(houseId, tableHouse);
            }

            InfoLog.WriteInfo("Finished loading sounds", EPrefix.AudioEngine);
        }

        private FMOD.RESULT endPlayCallbackFunction(IntPtr channelRaw, FMOD.CHANNEL_CALLBACKTYPE tipo, int comando, uint datoComando1, uint datoComando2)
        {
            lock(sequentialPlayList)
            {
                if (sequentialPlayList.Count == 0)
                    return FMOD.RESULT.ERR_INVALID_PARAM;

                sequentialPlayList.RemoveAt(0);

                if (sequentialPlayList.Count != 0)
                {
                    FMOD.RESULT result = system.playSound(FMOD.CHANNELINDEX.FREE, houses[this.houseId][(short)this.sequentialPlayList[0]], true, ref channel);
                    channel.setVolume(volume);
                    channel.setCallback(FMOD.CHANNEL_CALLBACKTYPE.END, endPlayCallback, 0);
                    channel.setPaused(false);
                }

                return FMOD.RESULT.OK;
            }
        }

        public bool PlayMisc(MiscSoundType miscSound)
        {
            if (!isInitialized)
                return false;

            FMOD.RESULT result = system.playSound(FMOD.CHANNELINDEX.FREE, misc[(short)miscSound], true, ref channel);
            channel.setVolume(volume);
            channel.setPaused(false);

            return FMOD.ERROR.ERRCHECK(result);
        }

        public bool PlayHouse(short houseId, HouseSoundType houseSound)
        {
            if (!isInitialized)
                return false;

            FMOD.RESULT result = system.playSound(FMOD.CHANNELINDEX.FREE, houses[houseId][(short)houseSound], true, ref channel);
            channel.setVolume(volume);
            channel.setPaused(false);

            return FMOD.ERROR.ERRCHECK(result);
        }

        /// <summary>
        /// Plays sequention of sounds. Only one sound queue is available. If called while playing, the specified sounds
        /// are buffered to the queueu and played back as soon as the previous sound finishes playing.
        /// </summary>
        /// <param name="houseId">Id of house to play sound of</param>
        /// <param name="houseSounds">Array of sound types to play</param>
        /// <returns></returns>
        public bool PlayHouse(short houseId, HouseSoundType[] houseSounds)
        {
            if (!isInitialized)
                return false;

            if (houseSounds.Length == 0)
                return false;

            lock (sequentialPlayList)
            {
                bool beginPlay = (sequentialPlayList.Count == 0);

                this.houseId = houseId;
                this.sequentialPlayList.AddRange(houseSounds);

                if (beginPlay)
                {
                    FMOD.RESULT result = system.playSound(FMOD.CHANNELINDEX.FREE, houses[houseId][(short)houseSounds[0]], true, ref channel);
                    channel.setVolume(volume);
                    channel.setCallback(FMOD.CHANNEL_CALLBACKTYPE.END, endPlayCallback, 0);
                    channel.setPaused(false);
                }

                return true;
            }
        }

        public void Mute()
        {
            this.Volume = 0;
            isMuted = true;

            InfoLog.WriteInfo("Sound muted", EPrefix.AudioEngine);
        }

        private string GetFileName(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            FileNameAttribute[] attributes = (FileNameAttribute[])fi.GetCustomAttributes(typeof(FileNameAttribute), false);

            return (attributes.Length > 0) ? attributes[0].Name : value.ToString();
        }
    }
}
