using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Client.Engine.GameGraphics
{
    class MapHolder
    {
        #region constructors
        public MapHolder(string mapFilePath)
        {
            try
            {
                StreamReader sr = new StreamReader(mapFilePath);
            }
            catch (Exception ex)
            {
                throw new MapHolderException(ex);
            }
        }
        public MapHolder() { }
        #endregion
    }
}
