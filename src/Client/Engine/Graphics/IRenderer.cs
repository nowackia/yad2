using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Board
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
