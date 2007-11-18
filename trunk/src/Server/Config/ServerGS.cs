using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Config.Server {
    public class ServerGS {
        private GameSettingsWrapper _gameSettingsWrapper;
        private static ServerGS _instance;
        private ServerGS() {
            _gameSettingsWrapper = XMLLoader.Common.XMLLoader.get(
                Yad.Properties.Common.Settings.Default.ConfigFile,
                Yad.Properties.Common.Settings.Default.ConfigFileXSD);
            
        }

        private static ServerGS Instance {
            get {
                if (_instance == null) {
                    _instance = new ServerGS();
                }
                return _instance;
            }
        }

        private GameSettingsWrapper wrapper {
            get {
                return _gameSettingsWrapper;
            }
        }

        public static GameSettingsWrapper Wrapper {
            get {
                return Instance.wrapper;
            }
        }





    }
}
