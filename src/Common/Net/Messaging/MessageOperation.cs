using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.Messaging.Common {
    public enum MessageOperation : byte {
        Add = 0,
        Remove,
        List,
        Modify,
        Info
    }
}
