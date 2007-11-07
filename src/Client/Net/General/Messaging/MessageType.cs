using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.General.Messaging
{
    public enum MessageType: byte
    {
        Numeric = 0,
        Text,
        GameInit,
        Move,
        Destroy,
        CreateUnit,
        Build,
        Harvest,
        Attack,
        Control,
        ChatText, 
        ChatUsers,
        DeleteChatUser,
        Login,
        Register,
        LoginSuccessful,
        Remind,
        LoginUnsuccessful
        Unknown
    }
}
