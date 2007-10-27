using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Server.classes.Exceptions
{
    class XMLLoaderException : XmlException
    {
        public XMLLoaderException(string message): base(message)
        { }
        public XMLLoaderException(Exception e)
            : base(e.Message, e)
        { }
    }
}
