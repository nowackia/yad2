using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config.Common;

namespace Yad.Config {
    public class GlobalSettings {

        private GameSettingsWrapper _gameSettingsWrapper;
        private static GlobalSettings _instance;
        private short _defaultHouse;

        public short DefaultHouse {
            get { return _defaultHouse; }
            set { _defaultHouse = value; }
        }

        public string DefaultHouseName
        {
            get { return GetHouseName(_defaultHouse); }
        }

        public string GetHouseName(short id){
            return _gameSettingsWrapper.racesMap[id].Name;
        }

        private GlobalSettings() {
            LoadData();
            InitDefaultHouse();  
        }

        private void LoadData() {
            _gameSettingsWrapper = XMLLoader.Common.XMLLoader.get(
                Yad.Properties.Common.Settings.Default.ConfigFile,
                Yad.Properties.Common.Settings.Default.ConfigFileXSD);
        }

        private void InitDefaultHouse() {
            foreach (short key in _gameSettingsWrapper.racesMap.Keys) {
                _defaultHouse = key;
                break;
            }
        }

        public short[] GetHouseIDs() {
            short[] result = new short[_gameSettingsWrapper.racesMap.Count];
            int i = 0;
            foreach (short key in _gameSettingsWrapper.racesMap.Keys)
                result[i++] = key;
            return result;
        }

        public static GlobalSettings Instance {
            get {
                if (_instance == null) {
                    _instance = new GlobalSettings();
                }
                return _instance;
            }
        }

        public static GameSettingsWrapper Wrapper {
            get {
                return Instance._gameSettingsWrapper;
            }
        }

       
    }
}
