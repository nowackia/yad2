using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Engine.GameGraphics.Client
{
    class MapHolderException : Exception
    {
        public MapHolderException() { }
        public MapHolderException(Exception ex) : base(ex.Message, ex) { }
		public MapHolderException(string s) : base(s) { }
    }
}
