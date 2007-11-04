using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.General.Messaging
{
    /// <summary>
    /// message from chat(?)
    /// </summary>
    public class TextMessage : Message
    {
        private String text;

        public String Text
        {
            get { return text; }
            set { text = value; }
        }


    }
}
