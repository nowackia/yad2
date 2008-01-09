using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms.Design;
using System.Windows.Forms;

namespace System.Windows.Forms.Extended
{
    public class PictureButton : ButtonBase
    {
        private Image backgroundImage, pressedImage;
        private bool isPressed;
        private bool isOver;

        private bool mouseOverEffect;
        private bool imageStretch;

        private Brush backColorBrush;
        private Brush mouseOverBrush;
        private Color mouseOverColor;

        public PictureButton()
            : this(string.Empty, string.Empty, null, null)
        { }

        public PictureButton(string name, string text)
            : this(name, text, null, null)
        { }

        public PictureButton(Image backgroundImage, Image pressedImage)
            : this(string.Empty, string.Empty, backgroundImage, pressedImage)
        { }

        public PictureButton(string name, string text, Image backgroundImage, Image pressedImage)
            : base()
        {
            this.Name = name;
            this.Text = text;
            this.BackgroundImage = backgroundImage;
            this.PressedImage = pressedImage;

            isPressed = false;
            isOver = false;
            mouseOverEffect = true;
            imageStretch = true;

            this.BackColor = Color.DarkGray;
            MouseOverColor = Color.Black;
        }

        [Category("Appearance")]
        [Description("The background color of the component")]
        [DefaultValue(typeof(Color), "DarkGray")]
        public override Color BackColor
        {
            get
            { return base.BackColor; }
            set
            {
                backColorBrush = new SolidBrush(value);
                base.BackColor = value;
            }

        }

        /// <summary>
        ///  Property for the background image to be drawn behind the button text.
        /// </summary>
        [Category("Appearance")]
        [Description("The background image used for the control")]
        [DefaultValue(typeof(Image), "null")]
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
        [Category("Appearance")]
        [Description("The pressed image used for the control")]
        [DefaultValue(typeof(Image), "null")]
        public Image PressedImage
        {
            get
            { return pressedImage; }
            set
            { pressedImage = value; }
        }

        [Category("Appearance")]
        [Description("Defines whether to stretch the image to fit the control")]
        [DefaultValue(typeof(bool), "true")]
        public bool ImageStretch
        {
            get { return imageStretch; }
            set { imageStretch = value; }
        }

        [Category("Appearance")]
        [Description("Defines whether to show the mouse over effect")]
        [DefaultValue(typeof(bool), "true")]
        public bool MouseOverEffect
        {
            get { return mouseOverEffect; }
            set { mouseOverEffect = value; }
        }

        [Category("Appearance")]
        [Description("Specifies the mouse over effect color")]
        [DefaultValue(typeof(Color), "Black")]
        public Color MouseOverColor
        {
            get { return mouseOverColor; }
            set
            {
                mouseOverColor = value;
                mouseOverBrush = new SolidBrush(Color.FromArgb(40, mouseOverColor));
            }
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

        protected override void OnMouseEnter(EventArgs e)
        {
            isOver = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            isOver = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics grfx = e.Graphics;
            Image drawImage = backgroundImage;

            if (isPressed && pressedImage != null)
                drawImage = pressedImage;

            if (drawImage != null)
            {
                if (imageStretch)
                    grfx.DrawImage(drawImage, 0, 0, Width, Height);
                else
                {
                    grfx.FillRectangle(backColorBrush, 0, 0, Width, Height);
                    grfx.DrawImage(drawImage, 0, 0);
                }
            }
            else
                grfx.FillRectangle(backColorBrush, 0, 0, Width, Height);

            if (isOver && mouseOverEffect)
                grfx.FillRectangle(mouseOverBrush, 0, 0, Width, Height);

            if (Text.Length > 0)
            {
                SizeF size = grfx.MeasureString(this.Text, this.Font);
                grfx.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), (this.ClientSize.Width - size.Width) / 2, (this.ClientSize.Height - size.Height) / 2);
            }
        }

        #region Properties hidden in a Properties window
        [Browsable(false)]
        public override ImageLayout BackgroundImageLayout
        {
            get { return base.BackgroundImageLayout; }
            set { base.BackgroundImageLayout = value; }
        }

        [Browsable(false)]
        public new FlatStyle FlatStyle
        {
            get
            { return base.FlatStyle; }
            set
            { base.FlatStyle = value; }
        }

        [Browsable(false)]
        public new Image Image
        {
            get
            { return base.Image; }
            set
            { base.Image = value; }
        }

        [Browsable(false)]
        public new ContentAlignment ImageAlign
        {
            get
            { return base.ImageAlign; }
            set
            { base.ImageAlign = value; }
        }

        [Browsable(false)]
        public new int ImageIndex
        {
            get
            { return base.ImageIndex; }
            set
            { base.ImageIndex = value; }
        }

        [Browsable(false)]
        public new string ImageKey
        {
            get
            { return base.ImageKey; }
            set
            { base.ImageKey = value; }
        }

        [Browsable(false)]
        public new ImageList ImageList
        {
            get
            { return base.ImageList; }
            set
            { base.ImageList = value; }
        }

        [Browsable(false)]
        public override RightToLeft RightToLeft
        {
            get
            { return base.RightToLeft; }
            set
            { base.RightToLeft = value; }
        }

        [Browsable(false)]
        public new TextImageRelation TextImageRelation
        {
            get
            { return base.TextImageRelation; }
            set
            { base.TextImageRelation = value; }
        }
        #endregion
    }
}
