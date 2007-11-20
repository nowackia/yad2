using System;
using System.Collections.Generic;
using System.Text;
using Yad.Config.Common;

namespace Yad.Config {
    class GlobalSettings {

        private GameSettingsWrapper _gameSettingsWrapper;
        private static GlobalSettings _instance;
        private short _defaultHouse;
        private string[] _houseNames;

        public short DefaultHouse {
            get { return _defaultHouse; }
            set { _defaultHouse = value; }
        }

        public string[] HouseNames {
            get {
                if (_houseNames == null) {
                    _houseNames = new string[_gameSettingsWrapper.racesMap.Values.Count];
                    int index = 0;
                    foreach (RaceData rd in _gameSettingsWrapper.racesMap.Values) {
                        _houseNames[index++] = rd.Name;
                    }
                }
                return _houseNames;
            }
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

        public static short DeafultHouse {
            get {
                return Instance._defaultHouse;
            }
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
