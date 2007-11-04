using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.General.Messaging;

namespace Client.Net.General.Messaging {
    class LoginMessage : Message {
        string _login;
        string _password;
    }
}
