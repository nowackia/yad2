using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Board
{
    /// <summary>
    /// base object for all objects placed on map: units, buildings.
    /// base object have animation -> bullet, rocket ..
    /// </summary>
    public class BoardObject : IRenderable
    {
        Animation animation;
        /// <summary>
        /// if we want to render object, we have to set renderer for that object.
        /// </summary>
        private IRenderable renderer;

        public IRenderable Renderer
        {
            get { return renderer; }
            set { renderer = value; }
        }


        #region IRenderable Members

        public void Render()
        {
            if (renderer != null)
                renderer.Render();
        }

        #endregion
    }
}
