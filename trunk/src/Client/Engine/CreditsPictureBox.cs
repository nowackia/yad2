using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Drawing.Text;
using Yad.Properties.Client;
using System.ComponentModel;


namespace Yad.Engine.Client {
	class CreditsPictureBox : PictureBox {
		private Bitmap bmp = null;
		private int value = 0;
		private int ypos;
		private int xpos;
		private int destHeigth;
		private int dstDim;
		private int srcDim;

		[Category("SpecificData"), DefaultValue("20")]
		public int DestinationHeight {
			get {
				return destHeigth;
			}
			set {
				this.destHeigth = value;
			}
		}

		[Category("SpecificData"), DefaultValue(0)]
		public int Value {
			get {
				return value;
			}
			set {
				this.value = Math.Abs(value);
				Invalidate();
			}
		}

		[Category("SpecificData")]
		public Bitmap DigitsBitmap {
			get {
				return bmp;
			}
			set {
				bmp = value;
				calculateDigits();
				Invalidate();
			}
		}

		private void calculateDigits() {
			if (bmp == null)
				return;
			ypos = Math.Max((this.Height - destHeigth) / 2, 0);
			
			srcDim = (int)(bmp.Width / 10);
			dstDim = (int)(Math.Max(0,Math.Min(destHeigth,this.Height-ypos)));
			xpos = this.Width - dstDim ;
		}

		protected override void OnResize(EventArgs e) {
		    base.OnResize(e);
			calculateDigits();
		}

		protected override void OnPaint(PaintEventArgs pe) {
			base.OnPaint(pe);
			int tmpx = xpos;
			if (bmp != null) {
				if (value == 0) {
					pe.Graphics.DrawImage(bmp, new Rectangle(tmpx, ypos, dstDim, dstDim), new Rectangle(0, 0, srcDim, bmp.Height), GraphicsUnit.Pixel);
					xpos -= dstDim;
				} else {
					int tmp = value;
					while (xpos >= 0 && tmp > 0) {
						pe.Graphics.DrawImage(bmp, new Rectangle(tmpx, ypos, dstDim, dstDim), new Rectangle((tmp%10)*srcDim, 0, srcDim-1, bmp.Height), GraphicsUnit.Pixel);
						tmpx -= dstDim;
						tmp = (int)tmp / 10;
					}

				}

			}
		}
	}
}
