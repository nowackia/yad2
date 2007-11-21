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

	public partial class BuildStripe : UserControl, IManageableStripe {

		delegate void UpdatePicture(OwnerDrawPictureButton odpb);
		delegate void ClearPicture();
		delegate void UpdateControlSize(Control c, Size s);

		public delegate void ChoiceHandler(string name);
		public event ChoiceHandler OnChoice;

		public event BuildingChosenHandler onBuildingChosen;
		public event UnitChosenHandler onUnitChosen;

		public static int WIDTH = 90;
		public static int HEIGHT = 60;
		private List<short> ids = new List<short>();

		public List<short> Ids {
			get { return ids; }
			set { ids = value; }
		}

		private Dictionary<short, OwnerDrawPictureButton> buttons = new Dictionary<short, OwnerDrawPictureButton>();
		private Dictionary<short, bool> isBuilding = new Dictionary<short, bool>();
		private Dictionary<OwnerDrawPictureButton, short> buttonsToId = new Dictionary<OwnerDrawPictureButton, short>();
		#region rendering
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
			//this.building = building;
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
			loc.Offset(0, y * HEIGHT);
			scrollingPanel.Location = loc;
			delta += howMany;
		}
		private void contentPanel_SizeChanged(object sender, EventArgs e) {
			viewable = this.contentPanel.Height / HEIGHT;
		}

		#endregion

		public void Insert(short id, String name, String pictureName, bool building) {
            OwnerDrawPictureButton pictureButton = new OwnerDrawPictureButton(ResourceFactory.GetPicture(name), ResourceFactory.GetPicture(name));
			isBuilding[id] = building;
			buttons[id] = pictureButton;
			buttonsToId[pictureButton] = id;
			ids.Add(id);
			pictureButton.Text = name;
			pictureButton.Name = name;
			pictureButton.Tag = building;
			pictureButton.Size = new Size(WIDTH, HEIGHT);

			pictureButton.Margin = new Padding(0, 0, 0, 0);
			pictureButton.UseVisualStyleBackColor = true;
			pictureButton.Click += new EventHandler(pictureButton_Click);
			num++;
			//TODO: check?
			//this.flowLayoutPanel1.Controls.Add(pictureButton);
			if (flowLayoutPanel1.InvokeRequired) {
				flowLayoutPanel1.Invoke(new UpdatePicture(this.AddHelper), new object[] { pictureButton });
				scrollingPanel.Invoke(new UpdateControlSize(this.UpdateControlSizeHelper), new object[] { scrollingPanel, new Size(WIDTH, num * HEIGHT) });
				flowLayoutPanel1.Invoke(new UpdateControlSize(this.UpdateControlSizeHelper), new object[] { flowLayoutPanel1, new Size(WIDTH, num * HEIGHT) });
			} else {
				this.flowLayoutPanel1.Controls.Add(pictureButton);
				this.scrollingPanel.Size = new Size(WIDTH, num * HEIGHT);
				this.flowLayoutPanel1.Size = new Size(WIDTH, num * HEIGHT);
			}


			InfoLog.WriteInfo("Insert: size: " + num * HEIGHT, EPrefix.Stripe);

		}

		#region private members
		private void pictureButton_Click(object sender, EventArgs e) {
			OwnerDrawPictureButton picture = (OwnerDrawPictureButton)sender;
			short id = buttonsToId[picture];
			bool b = isBuilding[id];
			if (b) {
				OnBuildChosen(id);
			} else {
				OnUnitChosen(id);
			}
		}

		private void OnBuildChosen(short id) {
			if (onBuildingChosen != null) {
				onBuildingChosen(id);
			}
		}
		private void OnUnitChosen(short id) {
			if (onUnitChosen != null) {
				onUnitChosen(id);
			}
		}
		#endregion

		#region IManageableStripe Members

		public void Add(short id, string name, string pictureName, bool building) {
			InfoLog.WriteInfo("Add", EPrefix.Stripe);
			if (buttons.ContainsKey(id)) return;
			Insert(id, name, pictureName, building);

		}

		public void Remove(short id) {
			OwnerDrawPictureButton but = buttons[id];
			if (but != null) {
				//this.flowLayoutPanel1.Controls.Remove(but);
				if (flowLayoutPanel1.InvokeRequired) {
					flowLayoutPanel1.Invoke(new UpdatePicture(this.RemoveHelper), new object[] { but });
				} else {
					this.flowLayoutPanel1.Controls.Remove(but);
				}
				this.isBuilding.Remove(id);
				this.buttons.Remove(id);
				this.buttonsToId.Remove(but);
				ids.Remove(id);
			}
			InfoLog.WriteInfo("Remove", EPrefix.Stripe);
			num = 0;
			this.scrollingPanel.Size = new Size(WIDTH, num * HEIGHT);
			this.flowLayoutPanel1.Size = new Size(WIDTH, num * HEIGHT);
		}

		public void AddPercentCounter(short id) {
			InfoLog.WriteInfo("AddPercentCounter", EPrefix.Stripe);
			OwnerDrawPictureButton but;
			if (this.buttons.TryGetValue(id, out but)) {
				but.DrawPercentage = true;
			}
		}

		public void SetPercentValue(short id, int val) {
			InfoLog.WriteInfo("SetPercentValue", EPrefix.Stripe);
		}

		public void RemovePercentCounter(short id) {
			InfoLog.WriteInfo("RemovePercentCounter", EPrefix.Stripe);
			OwnerDrawPictureButton but;
			if (this.buttons.TryGetValue(id, out but)) {
				but.DrawPercentage = false;
			}
		}

		public void RemoveAll() {
			ids.Clear();

			//this.flowLayoutPanel1.Controls.Clear();
			if (flowLayoutPanel1.InvokeRequired) {
				flowLayoutPanel1.Invoke(new ClearPicture(this.RemoveAllHelper));
			} else {
				this.flowLayoutPanel1.Controls.Clear();
			}
			buttons.Clear();
			buttonsToId.Clear();
			isBuilding.Clear();
			num = 0;
			if (flowLayoutPanel1.InvokeRequired) {
				scrollingPanel.Invoke(new UpdateControlSize(this.UpdateControlSizeHelper), new object[] { scrollingPanel, new Size(WIDTH, num * HEIGHT) });
				flowLayoutPanel1.Invoke(new UpdateControlSize(this.UpdateControlSizeHelper), new object[] { flowLayoutPanel1, new Size(WIDTH, num * HEIGHT) });
			} else {
				this.scrollingPanel.Size = new Size(WIDTH, num * HEIGHT);
				this.flowLayoutPanel1.Size = new Size(WIDTH, num * HEIGHT);
			}
		}

		private void AddHelper(OwnerDrawPictureButton odpb) {
			this.flowLayoutPanel1.Controls.Add(odpb);
		}

		private void RemoveHelper(OwnerDrawPictureButton odpb) {
			this.flowLayoutPanel1.Controls.Remove(odpb);
		}

		private void RemoveAllHelper() {
			this.flowLayoutPanel1.Controls.Clear();
		}

		private void UpdateControlSizeHelper(Control c, Size s) {
			c.Size = s;
		}

		#endregion
	}
}
