using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.Extended;
using System.Drawing;

namespace Yad.UI {
	class OwnerDrawPictureButton : PictureButton {
		private int percentage = 50;
		private bool drawPercentage = false;

		public OwnerDrawPictureButton() : base() { }
		public OwnerDrawPictureButton(Image backgroundImage, Image pressedImage) : base(backgroundImage, pressedImage) { }
		public OwnerDrawPictureButton(string name, string text) : base(name, text) { }
		public OwnerDrawPictureButton(string name, string text, Image backgroundImage, Image pressedImage) : base(name, text, backgroundImage, pressedImage) { }


		public bool DrawPercentage {
			get { return drawPercentage; }
			set {
				drawPercentage = value;
				/* Refresh(); */
			}
		}

		public int Percentage {
			get { return percentage; }
			set { percentage = value; /* Refresh(); */ }
		}

		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e) {
			base.OnPaint(e);
			if (drawPercentage) {
				Rectangle r = this.Bounds;
				float w = r.Width;
				r.Width = (int)(r.Width * (percentage / 100.0));
				e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(70, 255, 0, 0)), r);
				e.Graphics.DrawString(percentage + "%", Font, Brushes.Black, new RectangleF(r.X, r.Y, w, r.Height));
			}

		}
	}
}
