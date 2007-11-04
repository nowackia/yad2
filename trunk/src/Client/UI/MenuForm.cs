using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Client.UI {
    public partial class MenuForm : UIManageable {
        private GroupBox groupBox;

        Button buttonNewGame;
        Button buttonCredits;
        Button buttonExit;
        Button buttonOptions;

        Button buttonPause;
        Button buttonContinue;

        Button buttonCancel;
        Button buttonOK;

        


        CheckBox checkBoxMusicMute;
        CheckBox checkBoxSoundMute;
        NumericUpDown numericUpDownMusicVolume;
        NumericUpDown numericUpDownSoundVolume;
        Label labelMusicVolume;
        Label labelSoundVolume;

        // connected with returning from option groupbox to GameMenu or to MainMenu
        private Views lastView = Views.None;

        public Views LastView {
            get { return lastView; }
            set { lastView = value; }
        }

        public MenuForm(string name) {
            InitializeComponent();

            #region Controls Initialization
            buttonCredits = new Button();
            buttonCredits.Location = new System.Drawing.Point(6, 110);
            buttonCredits.Name = "buttonCredits";
            buttonCredits.Size = new System.Drawing.Size(332, 23);
            buttonCredits.TabIndex = 2;
            buttonCredits.Text = "Credits";
            buttonCredits.UseVisualStyleBackColor = true;

            buttonNewGame = new Button();
            buttonNewGame.Location = new System.Drawing.Point(6, 52);
            buttonNewGame.Name = "buttonNewGame";
            buttonNewGame.Size = new System.Drawing.Size(332, 23);
            buttonNewGame.TabIndex = 0;
            buttonNewGame.Text = "New Game";
            buttonNewGame.UseVisualStyleBackColor = true;
            buttonNewGame.Click += new EventHandler(buttonNewGame_Click);

            buttonExit = new Button();
            buttonExit.Location = new System.Drawing.Point(6, 245);
            buttonExit.Name = "buttonExit";
            buttonExit.Size = new System.Drawing.Size(332, 23);
            buttonExit.TabIndex = 3;
            buttonExit.Text = "Exit";
            buttonExit.UseVisualStyleBackColor = true;
            buttonExit.Click += new EventHandler(buttonExit_Click);

            buttonOptions = new Button();
            buttonOptions.Location = new System.Drawing.Point(6, 81);
            buttonOptions.Name = "buttonOptions";
            buttonOptions.Size = new System.Drawing.Size(332, 23);
            buttonOptions.TabIndex = 1;
            buttonOptions.Text = "Options";
            buttonOptions.UseVisualStyleBackColor = true;
            buttonOptions.Click += new EventHandler(buttonOptions_Click);

            buttonCancel = new Button();
            buttonCancel.Location = new System.Drawing.Point(237, 245);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new System.Drawing.Size(100, 23);
            buttonCancel.TabIndex = 4;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += new EventHandler(buttonCancel_Click);

            buttonOK = new Button();
            buttonOK.Location = new System.Drawing.Point(6, 245);
            buttonOK.Name = "buttonOK";
            buttonOK.Size = new System.Drawing.Size(100, 23);
            buttonOK.TabIndex = 3;
            buttonOK.Text = "OK";
            buttonOK.UseVisualStyleBackColor = true;
            buttonOK.Click += new EventHandler(buttonOK_Click);

            buttonContinue = new Button();
            buttonContinue.Location = new System.Drawing.Point(6, 52);
            buttonContinue.Name = "buttonContinue";
            buttonContinue.Size = new System.Drawing.Size(332, 23);
            buttonContinue.TabIndex = 0;
            buttonContinue.Text = "Continue";
            buttonContinue.UseVisualStyleBackColor = true;
            buttonContinue.Click += new EventHandler(buttonContinue_Click);

            buttonPause = new Button();
            buttonPause.Location = new System.Drawing.Point(6, 52);
            buttonPause.Name = "buttonPause";
            buttonPause.Size = new System.Drawing.Size(332, 23);
            buttonPause.TabIndex = 0;
            buttonPause.Text = "Pause";
            buttonPause.UseVisualStyleBackColor = true;
            buttonPause.Click += new EventHandler(buttonPause_Click);

            checkBoxMusicMute = new CheckBox();
            checkBoxMusicMute.AutoSize = true;
            checkBoxMusicMute.Location = new System.Drawing.Point(175, 127);
            checkBoxMusicMute.Name = "checkBoxMusicMute";
            checkBoxMusicMute.Size = new System.Drawing.Size(50, 17);
            checkBoxMusicMute.TabIndex = 10;
            checkBoxMusicMute.Text = "Mute";
            checkBoxMusicMute.UseVisualStyleBackColor = true;
            checkBoxMusicMute.CheckedChanged += new System.EventHandler(checkBoxMusicMute_CheckedChanged);

            checkBoxSoundMute = new CheckBox();
            checkBoxSoundMute.AutoSize = true;
            checkBoxSoundMute.Location = new System.Drawing.Point(175, 57);
            checkBoxSoundMute.Name = "checkBoxSoundMute";
            checkBoxSoundMute.Size = new System.Drawing.Size(50, 17);
            checkBoxSoundMute.TabIndex = 9;
            checkBoxSoundMute.Text = "Mute";
            checkBoxSoundMute.UseVisualStyleBackColor = true;
            checkBoxSoundMute.CheckedChanged += new System.EventHandler(checkBoxSoundMute_CheckedChanged);

            numericUpDownMusicVolume = new NumericUpDown();
            numericUpDownMusicVolume.Increment = new decimal(new int[] { 5, 0, 0, 0 });
            numericUpDownMusicVolume.Location = new System.Drawing.Point(175, 101);
            numericUpDownMusicVolume.Name = "numericUpDownMusicVolume";
            numericUpDownMusicVolume.Size = new System.Drawing.Size(58, 20);
            numericUpDownMusicVolume.TabIndex = 8;

            numericUpDownSoundVolume = new NumericUpDown();
            numericUpDownSoundVolume.Increment = new decimal(new int[] { 5, 0, 0, 0 });
            numericUpDownSoundVolume.Location = new System.Drawing.Point(175, 31);
            numericUpDownSoundVolume.Name = "numericUpDownSoundVolume";
            numericUpDownSoundVolume.Size = new System.Drawing.Size(58, 20);
            numericUpDownSoundVolume.TabIndex = 7;

            labelMusicVolume = new Label();
            labelMusicVolume.AutoSize = true;
            labelMusicVolume.Location = new System.Drawing.Point(19, 103);
            labelMusicVolume.Name = "labelMusicVolume";
            labelMusicVolume.Size = new System.Drawing.Size(76, 13);
            labelMusicVolume.TabIndex = 6;
            labelMusicVolume.Text = "Music Volume:";

            labelSoundVolume = new Label();
            labelSoundVolume.AutoSize = true;
            labelSoundVolume.Location = new System.Drawing.Point(19, 33);
            labelSoundVolume.Name = "labelSoundVolume";
            labelSoundVolume.Size = new System.Drawing.Size(79, 13);
            labelSoundVolume.TabIndex = 5;
            labelSoundVolume.Text = "Sound Volume:";
            #endregion

            GroupBoxName = name;
            this.FormBorderStyle = FormBorderStyle.None;
        }

        void buttonOK_Click(object sender, EventArgs e) {
            if (this.lastView == Views.GameMenuForm) {
                OnOptionChoosen(MenuOption.OkToGameMenu);
            } else if (this.lastView == Views.PauseForm) {
                OnOptionChoosen(MenuOption.OkToPauseMenu);
            } else {
                OnOptionChoosen(MenuOption.Ok);
            }
        }

        void buttonContinue_Click(object sender, EventArgs e) {
            OnOptionChoosen(MenuOption.Continue);
        }

        void buttonPause_Click(object sender, EventArgs e) {
            OnOptionChoosen(MenuOption.Pause);
        }

        void buttonCancel_Click(object sender, EventArgs e) {
            if (this.lastView == Views.GameMenuForm) {
                OnOptionChoosen(MenuOption.CancelToGameMenu);
            } else if (this.lastView == Views.PauseForm) {
                OnOptionChoosen(MenuOption.CancelToPauseMenu);
            } else {
                OnOptionChoosen(MenuOption.Cancel);
            }
        }

        void buttonOptions_Click(object sender, EventArgs e) {
            OnOptionChoosen(MenuOption.Options);
        }

        void buttonExit_Click(object sender, EventArgs e) {
            OnOptionChoosen(MenuOption.Exit);
        }



        private GroupBox GetGroupBox(string name) {
            List<GroupBox> groupBoxList = new List<GroupBox>();
            GroupBox groupBox = null;

            groupBox = new GroupBox();
            groupBox.Location = new System.Drawing.Point(12, 12);
            groupBox.Name = "groupBoxMainMenu";
            groupBox.Dock = DockStyle.Fill;
            groupBox.TabIndex = 0;
            groupBox.TabStop = false;
            groupBox.Text = "Main Menu";
            if (name == groupBox.Name) {
                groupBox.Controls.Add(buttonNewGame);
                groupBox.Controls.Add(buttonCredits);
                groupBox.Controls.Add(buttonOptions);
                groupBox.Controls.Add(buttonExit);
            }
            groupBoxList.Add(groupBox);

            groupBox = new GroupBox();
            groupBox.Location = new System.Drawing.Point(12, 12);
            groupBox.Name = "groupBoxOptions";
            groupBox.Dock = DockStyle.Fill;
            groupBox.TabIndex = 1;
            groupBox.TabStop = false;
            groupBox.Text = "Options";
            if (name == groupBox.Name) {
                groupBox.Controls.Add(checkBoxMusicMute);
                groupBox.Controls.Add(checkBoxSoundMute);
                groupBox.Controls.Add(numericUpDownMusicVolume);
                groupBox.Controls.Add(numericUpDownSoundVolume);
                groupBox.Controls.Add(labelMusicVolume);
                groupBox.Controls.Add(labelSoundVolume);
                groupBox.Controls.Add(buttonCancel);
                groupBox.Controls.Add(buttonOK);
            }
            groupBoxList.Add(groupBox);

            groupBox = new GroupBox();
            groupBox.Location = new System.Drawing.Point(12, 12);
            groupBox.Name = "groupBoxInGame";
            groupBox.Dock = DockStyle.Fill;
            groupBox.TabIndex = 2;
            groupBox.TabStop = false;
            groupBox.Text = "Continue";
            if (name == groupBox.Name) {
                groupBox.Controls.Add(buttonPause);
                groupBox.Controls.Add(buttonOptions);
                groupBox.Controls.Add(buttonExit);
            }
            groupBoxList.Add(groupBox);

            groupBox = new GroupBox();
            groupBox.Location = new System.Drawing.Point(12, 12);
            groupBox.Name = "groupBoxPause";
            groupBox.Dock = DockStyle.Fill;
            groupBox.TabIndex = 2;
            groupBox.TabStop = false;
            groupBox.Text = "Pause";
            if (name == groupBox.Name) {
                groupBox.Controls.Add(buttonContinue);
                groupBox.Controls.Add(buttonOptions);
                groupBox.Controls.Add(buttonExit);
            }
            groupBoxList.Add(groupBox);

            foreach (GroupBox gb in groupBoxList) {
                if (gb.Name == name)
                    return gb;
            }

            throw new ArgumentException("No group box with name " + name + " found");
        }

        public String GroupBoxName {
            get { return groupBox.Name; }
            set {
                lastView = Views.None;
                Controls.Remove(groupBox);
                groupBox = GetGroupBox(value);
                Controls.Add(groupBox);
            }
        }

        private void checkBoxSoundMute_CheckedChanged(object sender, EventArgs e) {
            CheckBox checkBox = sender as CheckBox;

            numericUpDownSoundVolume.Enabled = !checkBox.Checked;
        }

        private void checkBoxMusicMute_CheckedChanged(object sender, EventArgs e) {
            CheckBox checkBox = sender as CheckBox;

            numericUpDownMusicVolume.Enabled = !checkBox.Checked;
        }

        void buttonNewGame_Click(object sender, EventArgs e) {
            OnOptionChoosen(MenuOption.NewGame);
        }
    }
}