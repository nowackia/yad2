using System;
using System.Collections.Generic;
using System.Text;
using Yad.Properties.Common;

namespace Yad.Config.INILoader.Common
{
    public class InitializationSettings
    {
        private static InitializationSettings instance = new InitializationSettings();

        private IniReader ini = new IniReader(Settings.Default.InitializationFile);

        private int nextServerIPIndex;
        public List<string> serverIPs;

        private InitializationSettings()
        {
            nextServerIPIndex = ini.ReadInteger("Login Settings", "ServerIPCount", 0);
            serverIPs = new List<string>();

            for (int i = 0; i <= nextServerIPIndex; i++)
            {
                string temp = ini.ReadString("Login Settings", "ServerIPAddress" + i, string.Empty);
                if (temp != string.Empty)
                    serverIPs.Add(temp);
            }
        }

        public static InitializationSettings Instance
        {
            get
            { return instance; }
        }

        public string Login
        {
            get
            {
                return ini.ReadString("Login Settings", "Login", string.Empty);
            }
            set
            { ini.Write("Login Settings", "Login", value); }
        }

        public int SoundVolume
        {
            get
            {
                return ini.ReadInteger("Audio Settings", "SoundVolume", (int)Settings.Default.SoundDefaultVolume);
            }
            set
            { ini.Write("Audio Settings", "SoundVolume", value); }
        }

        public bool IsSoundMuted
        {
            get
            {
                return ini.ReadBoolean("Audio Settings", "SoundMuted", false);
            }
            set
            { ini.Write("Audio Settings", "SoundMuted", value); }
        }

        public int MusicVolume
        {
            get
            {
                return ini.ReadInteger("Audio Settings", "MusicVolume", (int)Settings.Default.MusicDefaultVolume);
            }
            set
            { ini.Write("Audio Settings", "MusicVolume", value); }
        }

        public bool IsMusicMuted
        {
            get
            {
                return ini.ReadBoolean("Audio Settings", "MusicMuted", false);
            }
            set
            { ini.Write("Audio Settings", "MusicMuted", value); }
        }

        public void AddServerIP(string serverIP)
        {
            if (!serverIPs.Contains(serverIP))
            {
                serverIPs.Add(serverIP);
                nextServerIPIndex = serverIPs.Count;
                ini.Write("Login Settings", "ServerIPCount", nextServerIPIndex);
                ini.Write("Login Settings", "ServerIPAddress" + nextServerIPIndex, serverIP);
            }
        }

        public string[] ServerIPs
        {
            get
            { return serverIPs.ToArray(); }
        }
    }
}
