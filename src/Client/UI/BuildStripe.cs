using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Yad.Log.Common;
using System.Windows.Forms.Extended;

namespace Client.UI {
    /// <summary>
    /// 
    /// </summary>
    public partial class BuildStripe : UserControl {

        public static int SIZE = 72;
        
        /// <summary>
        /// number of objects under the 'upper' line
        /// </summary>
        private int delta = 0;
        /// <summary>
        /// number of objects on the stripe
        /// </summary>
        private int num = 0;

        private int top = 0;

        /// <summary>
        /// number of objects viewable on the stripe
        /// </summary>
        private int viewable;

        public BuildStripe() {
            InitializeComponent();
            viewable = this.contentPanel.Height / SIZE;
            scrollingPanel.Location = new Point(0, top);
            scrollingPanel.Size = new Size(SIZE, num * SIZE);
            Insert();
            Insert();
            Insert();
            Insert();
            Insert();
        }

        private void buttonDown_Click(object sender, EventArgs e) {
            InfoLog.WriteInfo("buttonDown: delta: " + delta + " num: " + num + " viewable: " + viewable, EPrefix.Stripe);
            ShowLower(1);
            Refresh();
        }

        private void buttonUp_Click(object sender, EventArgs e) {
            InfoLog.WriteInfo("buttonUp: delta: " + delta + " num: " + num + " viewable: " + viewable, EPrefix.Stripe);
            ShowUpper(1);
            Refresh();
        }

        private void ShowUpper(int howMany) {
            if (howMany <= 0) return;
            if (howMany > delta) howMany = delta;
            if (delta == 0) return;
            Point loc = scrollingPanel.Location;
            int y = top;
            y += howMany;
            loc.Offset(0, y * SIZE);
            scrollingPanel.Location = loc;
            delta -= howMany;
        }

        private void ShowLower(int howMany) {
            if (howMany <= 0) return;
            int delta2 = num - viewable - delta;
            if (howMany > delta2) howMany = delta;
            if (delta2 == 0) return;
            Point loc = scrollingPanel.Location;
            int y = top;
            y -= howMany;
            loc.Offset(0, y*SIZE);
            scrollingPanel.Location = loc;
            delta += howMany;
        }

        public void Insert() {
            PictureButton pictureButton = new PictureButton();
            pictureButton.Text = "hoho " + num;
            pictureButton.Name = "hoho " + num;
            pictureButton.Image = null;
            pictureButton.Size = new Size(SIZE, SIZE);
            //pictureButton.BackColor = Color.Black;
            pictureButton.Margin = new Padding(0, 0, 0, 0);
            //pictureButton.MouseOverColor = Color.DarkGray;
            //pictureButton.MouseOverEffect = true;
            pictureButton.UseVisualStyleBackColor = true;
            pictureButton.Click += new EventHandler(pictureButton_Click);
            num++;
            this.flowLayoutPanel1.Controls.Add(pictureButton);
            this.scrollingPanel.Size = new Size(SIZE, num * SIZE);
            this.flowLayoutPanel1.Size = new Size(SIZE, num * SIZE);
            InfoLog.WriteInfo("Insert: size: " + num * SIZE, EPrefix.Stripe);
            
            

        }

        void pictureButton_Click(object sender, EventArgs e) {
            InfoLog.WriteInfo("pictureButton_Click", EPrefix.Stripe);
        }

    }
}
