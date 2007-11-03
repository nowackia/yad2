using System;
using System.Collections.Generic;
using System.Text;

namespace Client.MessageManagement
{
    /// <summary>
    /// message from chat(?)
    /// </summary>
    public class TextMessage : ControlMessage
    {
        private String text;
        //TODO: RS: from?
        public String Text
        {
            get { return text; }
            set { text = value; }
        }


    }
}
