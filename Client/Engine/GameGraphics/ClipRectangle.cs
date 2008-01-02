using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Engine.GameGraphics.Client {
	/// <summary>
	/// Helper class for GameGraphics.
	/// </summary>
    public class ClipRectangle {
        float _x, _y, _width, _height;

		#region constructor
		public ClipRectangle() {
            _x = _y = 0.0f;
            _width = _height = 1.0f;
		}
		#endregion

		#region accessors
		public float X {
            get { return _x; }
            set { _x = value; }
        }

        public float Y {
            get { return _y; }
            set { _y = value; }
        }

        public float Width {
            get { return _width; }
            set { _width = value; }
        }

        public float Height {
            get { return _height; }
            set { _height = value; }
        }

        public float Left {
            get { return X; }
            set { X = value; }
        }

        public float Right {
            get { return X + Width; }
            set { X = value - Width; }
        }

        public float Top {
            get { return Y + Height; }
            set { Y = value - Height; }
        }

        public float Bottom {
            get { return Y; }
            set { Y = value; }
		}
		#endregion
	}
}
