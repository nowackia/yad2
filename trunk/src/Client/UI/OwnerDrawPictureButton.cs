using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.Extended;
using System.Drawing;
using System.Threading;
using Yad.Engine;
using Yad.Board;

namespace Yad.UI {
    public enum StripButtonState {
        Inactive,
        Active,
        Percantage,
        Ready
    }
    delegate void SetTextCallBack(string text);
	public class OwnerDrawPictureButton : PictureButton {
		private int percentage = 50;
        private StripButtonState state = StripButtonState.Active;

        private static Brush inActiveBrush = new SolidBrush(Color.FromArgb(150, 150, 150, 150));
        private readonly string textReady = "Place";
        private bool isVisible = true;
        private int id;
  
        public int Id {
            get { return Id; }
            set { id = value; }
        }

        public bool IsVisible {
            get { return isVisible; }
            set { isVisible = value; }
        }
        public StripButtonState State {
            get { return state; }
            set { 
                state = value;
                OnStateChange(state);
            }
        }

		public OwnerDrawPictureButton() : base() { }
		public OwnerDrawPictureButton(Image backgroundImage, Image pressedImage) : base(backgroundImage, pressedImage) { }
		public OwnerDrawPictureButton(string name, string text) : base(name, text) { }
		public OwnerDrawPictureButton(string name, string text, Image backgroundImage, Image pressedImage) : base(name, text, backgroundImage, pressedImage) { }


		/*public bool DrawPercentage {
			get { return drawPercentage; }
			set {
				drawPercentage = value;
                InvokeRefresh();
			}
		}*/


        private void OnStateChange(StripButtonState state) {
            switch (state) {
                case StripButtonState.Inactive:
                    base.MouseOverEffect = false;
                    break;
                case StripButtonState.Ready:
                    this.Text = this.textReady;
                    break;
                case StripButtonState.Active:
                    base.MouseOverEffect = true;
                    this.Text = this.Name;
                    break;
            }
            InvokeRefresh();
        }
		public int Percentage {
			get { return percentage; }
            set { percentage = value; InvokeRefresh(); }
		}


        public void InvokeRefresh() {
            if (this.InvokeRequired) {
                this.Invoke(new ThreadStart(Refresh));
            } else {
                Refresh();
            }
        }

		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e) {
			base.OnPaint(e);
            switch (state) {
                case StripButtonState.Percantage:
                    DrawPercentage(e.Graphics);
                    break;
                case StripButtonState.Inactive:
                    DrawInactive(e.Graphics);
                    break;
                case StripButtonState.Ready:
                    DrawReady(e.Graphics);
                    break;
            }

		} 

        private void DrawPercentage(Graphics g) {
            Rectangle r = new Rectangle(0, 0, Width, Height);
            float w = r.Width;
            r.Width = (int)(r.Width * (percentage / 100.0));
            g.FillRectangle(new SolidBrush(Color.FromArgb(70, 255, 0, 0)), r);
            g.DrawString(percentage + "%", Font, Brushes.Black, new RectangleF(r.X, r.Y, w, r.Height));
        }

        private void DrawInactive(Graphics g) {
            Rectangle r = new Rectangle(0, 0, Width, Height);
            g.FillRectangle(inActiveBrush, r);

        }

        private void DrawReady(Graphics g) {
             
        }

        private void InitializeComponent() {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }
	}
}
