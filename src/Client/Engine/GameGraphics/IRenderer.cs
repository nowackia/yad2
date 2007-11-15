using System;
using System.Collections.Generic;
using System.Text;
using Yad.Board.Common;

namespace Yad.Engine.GameGraphics.Client
{
    /// <summary>
    /// Interface for rendering in TAO.
    /// </summary>
    public interface IRenderer
    {
        /// <summary>
        /// render method
        /// </summary>
        void Render(BoardObject boardObject);
    }
}
