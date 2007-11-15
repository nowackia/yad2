using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Engine.GameGraphics.Client {
    public class ClipRectangle {
        private float x;
        private float y;
        private float width;
        private float height;

        public ClipRectangle() {
            x = y = 0.0f;
            width = height = 1.0f;
        }

        public float X {
            get { return x; }
            set { x = value; }
        }

        public float Y {
            get { return y; }
            set { y = value; }
        }

        public float Width {
            get { return width; }
            set { width = value; }
        }

        public float Height {
            get { return height; }
            set { height = value; }
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
    }
}
