using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Drawing.Text;
using Yad.Properties.Client;
using System.ComponentModel;


namespace Yad.Engine.Client
{
    class CreditsPictureBox : PictureBox
    {
        private Bitmap bmp = null;
        private int value = 0;
        private int ypos;
        private int xpos;
        private int destHeigth;
        private int dstDim;
        private int srcDim;
        private int xOffset;
        private int yOffset;

        [Category("SpecificData"), DefaultValue("20")]
        public int DestinationHeight
        {
            get
            {
                return destHeigth;
            }
            set
            {
                this.destHeigth = value;
            }
        }

        [Category("SpecificData"), DefaultValue(0)]
        public int Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = Math.Abs(value);
                Invalidate();
            }
        }

        [Category("SpecificData")]
        public Bitmap DigitsBitmap
        {
            get
            {
                return bmp;
            }
            set
            {
                bmp = value;
                CalculateDigits();
                Invalidate();
            }
        }

        [Category("SpecificData"), DefaultValue(0)]
        public int DrawingOffsetX
        {
            get
            { return xOffset; }
            set
            {
                xOffset = value;
                CalculateDigits();
                Invalidate();
            }
        }

        [Category("SpecificData"), DefaultValue(0)]
        public int DrawingOffsetY
        {
            get
            { return yOffset; }
            set
            {
                yOffset = value;
                CalculateDigits();
                Invalidate();
            }
        }

        private void CalculateDigits()
        {
            if (bmp == null)
                return;
            ypos = Math.Max((this.Height - destHeigth) / 2, 0) + yOffset;

            srcDim = (int)(bmp.Width / 10);
            dstDim = (int)(Math.Max(0, Math.Min(destHeigth, this.Height - ypos)));
            xpos = this.Width - dstDim - xOffset;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CalculateDigits();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            int tmpx = xpos;
            if (bmp != null)
            {
                if (value == 0)
                {
                    pe.Graphics.DrawImage(bmp, new Rectangle(tmpx, ypos, dstDim, dstDim), new Rectangle(0, 0, srcDim, bmp.Height), GraphicsUnit.Pixel);
                    tmpx -= dstDim;
                }
                else
                {
                    int tmp = value;
                    while (xpos >= 0 && tmp > 0)
                    {
                        pe.Graphics.DrawImage(bmp, new Rectangle(tmpx, ypos, dstDim, dstDim), new Rectangle((tmp % 10) * srcDim, 0, srcDim - 1, bmp.Height), GraphicsUnit.Pixel);
                        tmpx -= dstDim;
                        tmp = (int)tmp / 10;
                    }

                }

            }
        }
    }
}
