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
using Yad.Engine;
using Yad.Board;

namespace Yad.UI.Client {

	public partial class BuildStripe : UserControl, IManageableStripe {

		delegate void UpdatePicture(OwnerDrawPictureButton odpb);
		delegate void ClearPicture();
		delegate void UpdateControlSize(Control c, Size s);
        delegate void ShowHideOwnerDrawButton(OwnerDrawPictureButton button, bool isHide);
        delegate void SuspendResumeCallBack(FlowLayoutPanel flp, bool isSuspend);
        delegate void RefreshCallback(FlowLayoutPanel flp);
        delegate void ShowHideButtons(List<int> toShow, List<int> toHide);
        delegate void RemoveAddButtonsCallBack(OwnerDrawPictureButton[] buttons, bool isRemove);
        delegate void PerformLayoutCallBack(FlowLayoutPanel flp);
        delegate void SetScrollingLocation(Point pt);
        delegate void SwitchDelegate(Dictionary<short, StateWrapper> statusList, bool rewind);
		public delegate void ChoiceHandler(string name);


		public event BuildingChosenHandler onBuildingChosen;
		public event UnitChosenHandler onUnitChosen;

		public static int WIDTH = 90;
		public static int HEIGHT = 60;
		private List<int> ids = new List<int>();

		public List<int> Ids {
			get { return ids; }
			set { ids = value; }
		}

		private Dictionary<int, OwnerDrawPictureButton> buttons = new Dictionary<int, OwnerDrawPictureButton>();
		private Dictionary<int, bool> isBuilding = new Dictionary<int, bool>();
		private Dictionary<OwnerDrawPictureButton, int> buttonsToId = new Dictionary<OwnerDrawPictureButton, int>();
		
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
			viewable = this.contentPanel.Height / HEIGHT;
			scrollingPanel.Location = new Point(0, top);
			scrollingPanel.Size = new Size(WIDTH, num * HEIGHT);
		}

        public void DeactivateAll() {
            foreach (KeyValuePair<int, OwnerDrawPictureButton> kp in buttons) {
                if (kp.Value.IsVisible)
                    kp.Value.State = StripButtonState.Inactive;
            }
        }

        public void ActivateAll() {
            foreach (KeyValuePair<int, OwnerDrawPictureButton> kp in buttons) {
                if (kp.Value.IsVisible)
                    kp.Value.State = StripButtonState.Active;
            }
        }

        public void VisibilityUpdate(Dictionary<short, StateWrapper> statusList, bool rewind) {
            SuspendFlowLayout();
            List<int> toShow = new List<int>();
            List<int> toHide = new List<int>();
            foreach (KeyValuePair<int, OwnerDrawPictureButton> kp in buttons) {
                if (statusList.ContainsKey((short)kp.Key)) {
                    if (!kp.Value.IsVisible)
                        toShow.Add(kp.Key);
                }
                else
                    toHide.Add(kp.Key);
            }
            InvokeShowHide(toShow, toHide);
            int oldnum = num;
            num = statusList.Keys.Count;
            if (rewind)
                ShowUpper(int.MaxValue);
            if (oldnum != num)
                UpdateFlowLayoutPanelSize();
            ResumeFlowLayout();
        }

        public void SetState(int key, StripButtonState state) {
            buttons[key].State = state;
        }
        public void SwitchUpdate(Dictionary<short, StateWrapper> statusList, bool rewind){
            foreach (short key in statusList.Keys) {
                buttons[key].State = statusList[key].State;
                if (statusList[key].State == StripButtonState.Percantage)
                    buttons[key].Percentage = statusList[key].Percent;
            }
            VisibilityUpdate(statusList, rewind);
        }

        public void SwitchUpdateGroup(Dictionary<short, StateWrapper> statusList, bool rewind) {
            if (this.InvokeRequired)
                this.BeginInvoke(new SwitchDelegate(Switch), new object[] { statusList, rewind });
            else
                Switch(statusList, rewind);
        }

        public void Switch(Dictionary<short, StateWrapper> statusList, bool rewind) {
            foreach (short key in statusList.Keys) {
                buttons[key].SetStateWithoutInvoke(statusList[key].State);
                if (statusList[key].State == StripButtonState.Percantage)
                    buttons[key].SetPercentage(statusList[key].Percent);
            }
            this.SuspendFlowLayout();
            List<int> toShow = new List<int>();
            List<int> toHide = new List<int>();
            foreach (KeyValuePair<int, OwnerDrawPictureButton> kp in buttons) {
                if (statusList.ContainsKey((short)kp.Key)) {
                    if (!kp.Value.IsVisible)
                        toShow.Add(kp.Key);
                }
                else
                    toHide.Add(kp.Key);
            }
            ShowHide(toShow, toHide);
            int oldnum = num;
            num = statusList.Keys.Count;
            if (rewind)
                ShowUpperWithoutInvoke(int.MaxValue);
            if (oldnum != num) {
                this.scrollingPanel.Size = new Size(WIDTH, (num) * HEIGHT);
                this.flowLayoutPanel1.Size = new Size(WIDTH, (num) * HEIGHT);
            }
                
            this.ResumeFlowLayout();
        }

        public void UpdatePercent(short type, int percent) {
            InfoLog.WriteInfo("Enter Update percent in BuildStripe", EPrefix.Stripe);
            buttons[type].State = StripButtonState.Percantage;
            buttons[type].Percentage = percent;
            //InvokeShow(type);
            //UpdateFlowLayoutPanelSize();
            InfoLog.WriteInfo("Left Update percent in BuildStripe",EPrefix.Stripe);
        }
        public void Update(StripButtonState buildStatus, short type) {
            SuspendFlowLayout();
            buttons[type].State = buildStatus;
            
            InvokeShow(type);

            UpdateFlowLayoutPanelSize();
            ResumeFlowLayout();
        }

        public void HideButton(int id) {
            if (buttons[id].IsVisible) {
                ProcessHideButton(id);
                UpdateFlowLayoutPanelSize();
            }
            
        }

        private void ProcessHideButton(int id)
        {
            if (buttons[id].InvokeRequired)
            {
                bool isHide = true;
                buttons[id].Invoke(new ShowHideOwnerDrawButton(ShowHideButtonHelper),
                    new object[] { buttons[id], isHide });
            }
            else
                buttons[id].Hide();
            buttons[id].IsVisible = false;
        }

        public void HideAll() {
            List<int> toHide = new List<int>();
            List<int> toShow = new List<int>();
            foreach (int id in buttons.Keys) {
                if (buttons[id].IsVisible)
                    toHide.Add(id);
            }
            InvokeShowHide(toShow, toHide);
        }

        public void SuspendFlowLayout() {
            if (flowLayoutPanel1.InvokeRequired) {
                bool isSuspend = true;
                flowLayoutPanel1.BeginInvoke(new SuspendResumeCallBack(this.ResumeSuspendHelper),
                    new object[] { flowLayoutPanel1, isSuspend });
            }
            else
                flowLayoutPanel1.SuspendLayout();
        }

        public void ResumeFlowLayout() {
            if (flowLayoutPanel1.InvokeRequired) {
                bool isSuspend = false;
                flowLayoutPanel1.BeginInvoke(new SuspendResumeCallBack(this.ResumeSuspendHelper),
                    new object[] { flowLayoutPanel1, isSuspend });
            }
            else
                flowLayoutPanel1.ResumeLayout();
        }
        public void ShowRange(int[] id) {
            SuspendFlowLayout();
            for (int i = 0; i < id.Length; ++i) {
                InvokeShow(id[i]);
            }
           
            
            ShowUpper(int.MaxValue);
            UpdateFlowLayoutPanelSize();
            ResumeFlowLayout();
           
            
        }
        public void ShowButton(int id) {
            SuspendFlowLayout();
            InvokeShow(id);
            /*OwnerDrawPictureButton[] but = new OwnerDrawPictureButton[1];
            but[0] = buttons[id];
            if (but[0].IsVisible)
                return;

            RemoveAddButtons(but, false);
            num++;*/
            //ShowUpper(int.MaxValue);
            
            UpdateFlowLayoutPanelSize();
            ResumeFlowLayout();
            
        }

        public bool Contains(int key) {
            return this.buttons.ContainsKey(key);
        }

        private void ShowHide(List<int> keysToShow, List<int> keysToHide) {
            foreach (int key in keysToShow) {
                buttons[key].Show();
                buttons[key].IsVisible = true;
                buttons[key].Visible = true;
            }
            foreach (int key in keysToHide) {
                buttons[key].Hide();
                buttons[key].IsVisible = false;
                buttons[key].Visible = false;
            }
        }
        private void InvokeShowHide(List<int> toShow, List<int> toHide) {
            if (InvokeRequired)
                this.BeginInvoke(new ShowHideButtons(ShowHide), new object[] { toShow, toHide });
            else
                ShowHide(toShow, toHide);
        }
        private void InvokeShow(int id) {
            if (!buttons[id].IsVisible) {
                if (buttons[id].InvokeRequired) {
                    bool isHide = false;
                    buttons[id].BeginInvoke(new ShowHideOwnerDrawButton(ShowHideButtonHelper),
                        new object[] { buttons[id], isHide });
                }
                else {
                    buttons[id].Show();
                    buttons[id].Visible = true;
                    buttons[id].IsVisible = true;
                }
                //num++;
            }
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
        private void ShowUpperWithoutInvoke(int howMany) {
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
		private void ShowUpper(int howMany) {
			if (howMany <= 0) return;
			if (howMany > delta) howMany = delta;
			if (delta == 0) return;
			Point loc = scrollingPanel.Location;
			int y = top;
			y += howMany;
			loc.Offset(0, y * HEIGHT);
            if (scrollingPanel.InvokeRequired)
                scrollingPanel.BeginInvoke(new SetScrollingLocation(SetScrollLoc), new object[] { loc });
            else
                scrollingPanel.Location = loc;
			delta -= howMany;
		}
        private void SetScrollLoc(Point pt) {
            scrollingPanel.Location = pt;
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

		public new void Enabled(short id, bool isEnabled) {
			//budynki o podanym id wyszarza, je�li isEnabled jest false
			//odszarza, je�li isEnabled jest true
		}
       /*
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
            }
            else {
                this.flowLayoutPanel1.Controls.Add(pictureButton);
                this.scrollingPanel.Size = new Size(WIDTH, num * HEIGHT);
                this.flowLayoutPanel1.Size = new Size(WIDTH, num * HEIGHT);
            }


            InfoLog.WriteInfo("Insert: size: " + num * HEIGHT, EPrefix.Stripe);

        }*/
        /*
        public void Insert(short id, String name, bool building, OwnerDrawPictureButton pictureButton) {
            //OwnerDrawPictureButton pictureButton = new OwnerDrawPictureButton(ResourceFactory.GetPicture(name), ResourceFactory.GetPicture(name));
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
            UpdateFlowLayout(pictureButton);


            InfoLog.WriteInfo("Insert: size: " + num * HEIGHT, EPrefix.Stripe);

        }*/

        private void UpdateFlowLayout(OwnerDrawPictureButton pictureButton) {
            if (flowLayoutPanel1.InvokeRequired) {
                flowLayoutPanel1.BeginInvoke(new UpdatePicture(this.AddHelper), new object[] { pictureButton });
                scrollingPanel.BeginInvoke(new UpdateControlSize(this.UpdateControlSizeHelper), new object[] { scrollingPanel, new Size(WIDTH, num * HEIGHT) });
                flowLayoutPanel1.BeginInvoke(new UpdateControlSize(this.UpdateControlSizeHelper), new object[] { flowLayoutPanel1, new Size(WIDTH, num * HEIGHT) });
                //flowLayoutPanel1.Invoke(new PerformLayoutCallBack(this.PerformLayoutHelper), new object[] { flowLayoutPanel1 });
                flowLayoutPanel1.BeginInvoke(new RefreshCallback(this.RefreshHelper), new object[] { flowLayoutPanel1 });
            }
            else {
                this.flowLayoutPanel1.Controls.Add(pictureButton);
                this.scrollingPanel.Size = new Size(WIDTH, num * HEIGHT);
                this.flowLayoutPanel1.Size = new Size(WIDTH, num * HEIGHT);
                //this.flowLayoutPanel1.PerformLayout();
                this.flowLayoutPanel1.Refresh();
            }
        }
        
		public void Insert(int id, String name, String pictureName, bool building) {
            OwnerDrawPictureButton pictureButton = new OwnerDrawPictureButton(ResourceFactory.GetPicture(name), ResourceFactory.GetPicture(name));
			isBuilding[id] = building;
			buttons[id] = pictureButton;
            pictureButton.Id = id;
			buttonsToId[pictureButton] = id;
			ids.Add(id);
			pictureButton.Text = name;
			pictureButton.Name = name;
			pictureButton.Tag = building;
			pictureButton.Size = new Size(WIDTH, HEIGHT);
            pictureButton.IsVisible = true;
			pictureButton.Margin = new Padding(0, 0, 0, 0);
			pictureButton.UseVisualStyleBackColor = true;
			pictureButton.Click += new EventHandler(pictureButton_Click);
			num++;
			//TODO: check?
			//this.flowLayoutPanel1.Controls.Add(pictureButton);
			if (flowLayoutPanel1.InvokeRequired) {
				flowLayoutPanel1.BeginInvoke(new UpdatePicture(this.AddHelper), new object[] { pictureButton });
				scrollingPanel.BeginInvoke(new UpdateControlSize(this.UpdateControlSizeHelper), new object[] { scrollingPanel, new Size(WIDTH, num * HEIGHT) });
				flowLayoutPanel1.BeginInvoke(new UpdateControlSize(this.UpdateControlSizeHelper), new object[] { flowLayoutPanel1, new Size(WIDTH, num * HEIGHT) });
			} else {
				this.flowLayoutPanel1.Controls.Add(pictureButton);
				this.scrollingPanel.Size = new Size(WIDTH, num * HEIGHT);
				this.flowLayoutPanel1.Size = new Size(WIDTH, num * HEIGHT);
			}


			InfoLog.WriteInfo("Insert: size: " + num * HEIGHT, EPrefix.Stripe);

		}

        private void UpdateFlowLayoutPanelSize() {
            if (flowLayoutPanel1.InvokeRequired) {
                scrollingPanel.BeginInvoke(new UpdateControlSize(this.UpdateControlSizeHelper), new object[] { scrollingPanel, new Size(WIDTH, (num ) * HEIGHT) });
                flowLayoutPanel1.BeginInvoke(new UpdateControlSize(this.UpdateControlSizeHelper), new object[] { flowLayoutPanel1, new Size(WIDTH, (num ) * HEIGHT) });
            }
            else {
                this.scrollingPanel.Size = new Size(WIDTH, (num)* HEIGHT);
                this.flowLayoutPanel1.Size = new Size(WIDTH, (num)* HEIGHT);
            }
           
        }

		#region private members
		private void pictureButton_Click(object sender, EventArgs e) {
			OwnerDrawPictureButton picture = (OwnerDrawPictureButton)sender;
			int id = buttonsToId[picture];
			bool b = isBuilding[id];
			if (b) {
				OnBuildChosen(id);
			} else {
				OnUnitChosen(id, ((OwnerDrawPictureButton)sender).Name);
			}
		}

		private void OnBuildChosen(int id) {
			if (onBuildingChosen != null) {
				onBuildingChosen(id);
			}
		}
		private void OnUnitChosen(int id, string name) {
			if (onUnitChosen != null) {
				onUnitChosen(id, name);
			}
		}
		#endregion

		#region IManageableStripe Members

		public void Add(int id, string name, string pictureName, bool building) {
			InfoLog.WriteInfo("Add", EPrefix.Stripe);
			//if (buttons.ContainsKey(id)) return;
			Insert(id, name, pictureName, building);

		}
        /*
        public void Add(short id, string name, bool building, OwnerDrawPictureButton pictureButton) {
            InfoLog.WriteInfo("Add", EPrefix.Stripe);
            if (buttons.ContainsKey(id)) return;
            Insert(id, name, building, pictureButton);

        }*/

      




		public void Remove(int id) {
			OwnerDrawPictureButton but = buttons[id];
            SuspendFlowLayout();
			if (but != null) {
				//this.flowLayoutPanel1.Controls.Remove(but);
                RemoveButton(but);
				this.isBuilding.Remove(id);
				this.buttons.Remove(id);
				this.buttonsToId.Remove(but);
				ids.Remove(id);
			}
			InfoLog.WriteInfo("Remove", EPrefix.Stripe);
            num--;
            UpdateFlowLayoutPanelSize();
            ResumeFlowLayout();
			
		}

        private void RemoveButton(OwnerDrawPictureButton but) {
            if (flowLayoutPanel1.InvokeRequired) {
                flowLayoutPanel1.Invoke(new UpdatePicture(this.RemoveHelper), new object[] { but });
            }
            else {
                this.flowLayoutPanel1.Controls.Remove(but);
            }
        }

 

		public void AddPercentCounter(int id) {
			InfoLog.WriteInfo("AddPercentCounter", EPrefix.Stripe);
			OwnerDrawPictureButton but;
			if (this.buttons.TryGetValue(id, out but)) {
                but.State = StripButtonState.Percantage;
			}
		}

		public void SetPercentValue(int id, int val) {
			InfoLog.WriteInfo("SetPercentValue", EPrefix.Stripe);
		}

		public void RemovePercentCounter(int id) {
			InfoLog.WriteInfo("RemovePercentCounter", EPrefix.Stripe);
			OwnerDrawPictureButton but;
			if (this.buttons.TryGetValue(id, out but)) {
                but.State = StripButtonState.Ready;
			}
		}

		public void RemoveAll() {
			ids.Clear();

			//this.flowLayoutPanel1.Controls.Clear();
            ClearAllButtons();
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

        private void ClearAllButtons() {
            if (flowLayoutPanel1.InvokeRequired) {
                flowLayoutPanel1.Invoke(new ClearPicture(this.RemoveAllHelper));
            }
            else {
                this.flowLayoutPanel1.Controls.Clear();
            }
        }

		private void AddHelper(OwnerDrawPictureButton odpb) {
			this.flowLayoutPanel1.Controls.Add(odpb);
		}

        private void RemoveAddButtons(OwnerDrawPictureButton[] buttons, bool isRemove){
            for (int i = 0; i < buttons.Length; ++i) {
                buttons[i].IsVisible = !isRemove;
                if (!isRemove) {
                    buttons[i].Show();
                    buttons[i].Visible = true;
                }
            }
            if (flowLayoutPanel1.InvokeRequired)
                flowLayoutPanel1.Invoke(new RemoveAddButtonsCallBack(RemoveAddButtonsHelper), new object[] { buttons, isRemove });
            else
                RemoveAddButtonsHelper(buttons, isRemove);
                
                
        }
        private void RemoveAddButtonsHelper(OwnerDrawPictureButton[] buttons, bool isRemove) {
            for (int i = 0; i < buttons.Length; ++i)
                if (!isRemove)
                    this.flowLayoutPanel1.Controls.Add(buttons[i]);
                else
                    this.flowLayoutPanel1.Controls.Remove(buttons[i]);

        }


        private void RemoveHelper(OwnerDrawPictureButton odpb) {
			this.flowLayoutPanel1.Controls.Remove(odpb);
		}

		private void RemoveAllHelper() {
			this.flowLayoutPanel1.Controls.Clear();
		}

        private void ShowHideButtonHelper(OwnerDrawPictureButton button, bool hide) {
            if (hide) {
                button.Hide();
                button.Visible = false;
                button.IsVisible = false;
            }
            else {
                button.Show();
                button.Visible = true;
                button.IsVisible = true;
            }
        }

        private void ResumeSuspendHelper(FlowLayoutPanel flp, bool isSuspend) {
            if (isSuspend)
                flp.SuspendLayout();
            else
                flp.ResumeLayout();
        }

        private void RefreshHelper(FlowLayoutPanel flp) {
            flp.Refresh();
        }
		private void UpdateControlSizeHelper(Control c, Size s) {
			c.Size = s;
		}
        private void PerformLayoutHelper(FlowLayoutPanel flp) {
            flp.PerformLayout();
        }

		#endregion


        #region IManageableStripe Members
#pragma warning disable 0067
        public event BuildStripe.ChoiceHandler OnChoice;
#pragma warning restore 0067
        #endregion
    }
}
