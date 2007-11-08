using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Net.Server {
    class PlayerData {
        int _id;

        public int Id {
            get { return _id; }
            set { _id = value; }
        }
        string _login;
        int _lossNo;
        int _winNo;

        public string Login {
            get { return _login; }
            set { _login = value; }
        }
        

        public int WinNo {
            get { return _winNo; }
            set { _winNo = value; }
        }
        

        public int LossNo {
            get { return _lossNo; }
            set { _lossNo = value; }
        }
    }
}
