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
    delegate void SetMouseOverEffectCallback(bool value);
    delegate void SetFontCallback(Font font);
    delegate void ApplyStateCallback();
	public class OwnerDrawPictureButton : PictureButton {
		private int percentage = 50;
        private StripButtonState state = StripButtonState.Active;

        private static Brush inActiveBrush = new SolidBrush(Color.FromArgb(150, 150, 150, 150));
        private readonly string textReady = "Place";
        private bool isVisible = true;
        private Font readyFont = new Font("Arial", 12, FontStyle.Bold);
        private Font normalFont;
        private int id;
  
        public int Id {
            get { return Id; }
            set { id = value; }
        }

        public bool IsVisible {
            get { return isVisible; }
            set { isVisible = value; }
        }

        public void SetStateWithoutInvoke(StripButtonState state) {
            StripButtonState oldstate = state;
            this.state = state;
            if (this.state != oldstate) {
                OnStateChangeWithoutInvoke(state);
            }
            else
                if (state == StripButtonState.Percantage)
                    Refresh();
            
                
        }

        public StripButtonState State {
            get { return state; }
            set {
                StripButtonState oldstate = state;
                state = value;
                if (state != oldstate)
                    OnStateChange(state);
                else
                    if (state == StripButtonState.Percantage)
                        InvokeRefresh();
            }
        }

		public OwnerDrawPictureButton() : base() {
            this.normalFont = this.Font;
        }
		public OwnerDrawPictureButton(Image backgroundImage, Image pressedImage) : base(backgroundImage, pressedImage) {
            this.normalFont = this.Font;
        }
		public OwnerDrawPictureButton(string name, string text) : base(name, text) {
            this.normalFont = this.Font;
        }
		public OwnerDrawPictureButton(string name, string text, Image backgroundImage, Image pressedImage) : base(name, text, backgroundImage, pressedImage) {
            this.normalFont = this.Font;
        }


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
                    InvokeState(new ApplyStateCallback(ApplyInactive));
                    break;
                case StripButtonState.Ready:
                    InvokeState(new ApplyStateCallback(ApplyReady));
                    break;
                case StripButtonState.Active:
                    InvokeState(new ApplyStateCallback(ApplyActive));
                    break;
                case StripButtonState.Percantage:
                    InvokeState(new ApplyStateCallback(ApplyPercent));
                    break;
            }
        }
        private void OnStateChangeWithoutInvoke(StripButtonState state) {
            switch (state) {
                case StripButtonState.Inactive:
                    ApplyInactive();
                    break;
                case StripButtonState.Active:
                    ApplyActive();
                    break;
                case StripButtonState.Ready:
                    ApplyReady();
                    break;
                case StripButtonState.Percantage:
                    ApplyPercent();
                    break;
            }
            Refresh();
        }
        private void InvokeState(ApplyStateCallback asc) {
            if (InvokeRequired)
                this.BeginInvoke(asc);
            else
                asc();
        }
        public void ApplyReady() {
            this.Text = textReady;
            this.Font = readyFont;
            Refresh();
        }
        public void ApplyActive() {
            this.MouseOverEffect = true;
            this.Text = this.Name;
            this.Font = normalFont;
            Refresh();
        }
        public void ApplyPercent() {
            this.Text = "";
            this.Font = readyFont;
            Refresh();
        }
        public void ApplyInactive() {
            this.Font = normalFont;
            this.MouseOverEffect = true;
            Refresh();
        }
       
        
		public int Percentage {
			get { return percentage; }
            set { percentage = value; InvokeRefresh(); }
		}

        public void SetPercentage(int percent) {
            this.Percentage = percent;
        }


        private void InvokeSetText(string text) {
            if (this.InvokeRequired) {
                this.Invoke(new SetTextCallBack(SetText), new object[] { text });
            }
            else
                SetText(text);
        }

        private void InvokeSetFont(Font font) {
            if (this.InvokeRequired)
                this.Invoke(new SetFontCallback(SetFont), new object[] { font });
            else
                SetFont(font);
        }
        private void InvokeSetMouseOverEffect(bool value) {
            if (this.InvokeRequired)
                this.Invoke(new SetMouseOverEffectCallback(SetMouseOverEffect), new object[] { value });
            else
                SetMouseOverEffect(value);
        }
        public void InvokeRefresh() {
            if (this.InvokeRequired) {
                this.BeginInvoke(new ApplyStateCallback(Refresh));
            } else {
                Refresh();
            }
        }

        private void SetText(string text) {
            this.Text = text;
        }

        private void SetFont(Font font) {
            this.Font = font;
        }

        private void SetMouseOverEffect(bool value) {
            this.MouseOverEffect = value;
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
            g.FillRectangle(new SolidBrush(Color.FromArgb(180, 70, 170, 70)), r);
            g.DrawString(percentage + "%", Font, Brushes.Black, new RectangleF(r.X+1, r.Y+1, w-1, r.Height-1));
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
