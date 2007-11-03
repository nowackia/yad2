using System;
using System.Collections.Generic;
using System.Text;

namespace Client.MessageManagement
{
    public enum MessageType
    {
        Numeric,
        Text,
        GameInit,
        Move,
        Destroy,
        CreateUnit,
        Build,
        Harvest,
        Attack,
        Control
    }
}
