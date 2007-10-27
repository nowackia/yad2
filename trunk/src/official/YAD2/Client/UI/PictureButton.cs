using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Client.UI
{
    public class PictureButton : ButtonBase
    {
        private Image backgroundImage, pressedImage;
        private bool isPressed;

        private bool mouseOverEffect;
        private bool imageStretch;

        public PictureButton(string name, string text)
            : this(name, text, null, null)
        { }

        public PictureButton(Image backgroundImage, image pressedImage)
            : this(string.Empty, string.Empty, backgroundImage, pressedImage)
        { }

        public PictureButton(string name, string text, Image backgroundImage, image pressedImage)
            : base()
        {
            this.Name = name;
            this.Text = text;
            this.BackgroundImage = backgroundImage;
            this.PressedImage = pressedImage;

            isPressed = false;
            mouseOverEffect = true;
            imageStretch = true;
        }

        /// <summary>
        ///  Property for the background image to be drawn behind the button text.
        /// </summary>
        public override Image BackgroundImage
        {
            get
            { return backgroundImage; }
            set
            { backgroundImage = value; }
        }

        /// <summary>
        /// Property for the background image to be drawn behind the button text when
        /// the button is pressed.
        /// </summary>
        public Image PressedImage
        {
            get
            { return pressedImage; }
            set
            { pressedImage = value; }
        }

        public void ResetToBackgroundImageSize()
        {
            this.Size = backgroundImage.Size;
            imageStretch = false;
        }

        public void ResetToPressedImageSize()
        {
            this.Size = pressedImage.Size;
            imageStretch = false;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            isPressed = true;
            Invalidate();
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            isPressed = false;
            Invalidate();
            base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (isPressed && pressedImage != null)
                e.Graphics.DrawImage(pressedImage, 0, 0);
            else
                e.Graphics.DrawImage(backgroundImage, 0, 0);

            // Draw the text if there is any.
            if (this.Text.Length > 0)
            {
                SizeF size = e.Graphics.MeasureString(this.Text, this.Font);

                // Center the text inside the client area of the PictureButton.
                e.Graphics.DrawString(this.Text,
                    this.Font,
                    new SolidBrush(this.ForeColor),
                    (this.ClientSize.Width - size.Width) / 2,
                    (this.ClientSize.Height - size.Height) / 2);
            }

            //base.OnPaint(e);
        }
    }
}
