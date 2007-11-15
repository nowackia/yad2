using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Yad.Log.Common;
using System.Windows.Forms.Extended;
using Yad.Engine.Client;

namespace Yad.UI.Client {
    /// <summary>
    /// 
    /// </summary>
    public partial class BuildStripe : UserControl, IManageableStripe {

        public static int WIDTH = 90;
        public static int HEIGHT = 60;
        private Dictionary<short, PictureButton> buttons = new Dictionary<short, PictureButton>();
        /// <summary>
        /// number of objects under the 'upper' line
        /// </summary>
        private int delta = 0;
        /// <summary>
        /// number of objects on the stripe
        /// </summary>
        private int num = 0;

        private int top = 0;

		private bool building;

        /// <summary>
        /// number of objects viewable on the stripe
        /// </summary>
        private int viewable;

        public BuildStripe(bool building) {
            InitializeComponent();
			this.building = building;
            viewable = this.contentPanel.Height / HEIGHT;
            scrollingPanel.Location = new Point(0, top);
            scrollingPanel.Size = new Size(WIDTH, num * HEIGHT);

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
            loc.Offset(0, y * HEIGHT);
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
            loc.Offset(0, y*HEIGHT);
            scrollingPanel.Location = loc;
            delta += howMany;
        }

        public void Insert(short id,String name,String pictureName) {
            PictureButton pictureButton = new PictureButton(new Bitmap(pictureName), new Bitmap(pictureName));
            buttons[id] = pictureButton;
            pictureButton.Text = name;
            pictureButton.Name = name;
			pictureButton.Tag = building;
            pictureButton.Size = new Size(WIDTH, HEIGHT);
            //pictureButton.BackColor = Color.Black;
             pictureButton.Margin = new Padding(0, 0, 0, 0);
            //pictureButton.MouseOverColor = Color.DarkGray;
            //pictureButton.MouseOverEffect = true;
            pictureButton.UseVisualStyleBackColor = true;
            pictureButton.Click += new EventHandler(pictureButton_Click);
            num++;
            this.flowLayoutPanel1.Controls.Add(pictureButton);
            this.scrollingPanel.Size = new Size(WIDTH, num * HEIGHT);
            this.flowLayoutPanel1.Size = new Size(WIDTH, num * HEIGHT);
            InfoLog.WriteInfo("Insert: size: " + num * HEIGHT, EPrefix.Stripe);
            
            

        }

        void pictureButton_Click(object sender, EventArgs e) {
			if (!GameLogic.IsWaitingForBuildingBuild && building)
				GameLogic.LocateBuilding(GameForm.sim.GameSettingsWrapper.namesToIds[((PictureButton)sender).Name]);
			else if (!GameLogic.IsWaitingForUnitCreation && !building)
				GameLogic.CreateUnit(GameForm.sim.GameSettingsWrapper.namesToIds[((PictureButton)sender).Name]);
            InfoLog.WriteInfo("pictureButton_Click", EPrefix.Stripe);
        }


        #region IManageableStripe Members

        public void Add(short id,string name, string pictureName) {
            InfoLog.WriteInfo("Add", EPrefix.Stripe);
            if (buttons.ContainsKey(id)) return;
            Insert(id,name,pictureName);
            
        }

        public void Remove(short id) {
            InfoLog.WriteInfo("Remove", EPrefix.Stripe);
        }

        public void AddPercentCounter(short id) {
            InfoLog.WriteInfo("AddPercentCounter", EPrefix.Stripe);
        }

        public void SetPercentValue(short id,int val) {
            InfoLog.WriteInfo("SetPercentValue", EPrefix.Stripe);
        }

        public void RemovePercentCounter() {
            throw new Exception("The method or operation is not implemented.");
        }

        public void RemoveAll() {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        private void contentPanel_SizeChanged(object sender, EventArgs e) {
            viewable = this.contentPanel.Height / HEIGHT;
        }
    }
}
