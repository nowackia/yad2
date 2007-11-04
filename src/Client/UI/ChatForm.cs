using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Client.UI
{
    public partial class ChatForm : UIManageable
    {
        private GroupBox groupBox;

        private ListBox listBoxChat;
        private ListBox listBoxUsers;

        private Button buttonGame;
        private Button buttonBackChat;

        private TextBox textBoxChat;

        private Label labelLogin;
        private Label labelWin;
        private Label labelLoss;
        private Label labelWinValue;
        private Label labelLossValue;


        public ChatForm(string name)
        {
            InitializeComponent();

            #region Controls Initialization
            buttonGame = new Button();
            buttonGame.Location = new System.Drawing.Point(137, 12);
            buttonGame.Name = "buttonGame";
            buttonGame.Size = new System.Drawing.Size(75, 23);
            buttonGame.TabIndex = 0;
            buttonGame.Text = "Game";
            buttonGame.UseVisualStyleBackColor = true;
            buttonGame.Click += new EventHandler(buttonGame_Click);

            listBoxChat = new ListBox();
            listBoxChat.FormattingEnabled = true;
            listBoxChat.Location = new System.Drawing.Point(6, 41);
            listBoxChat.Name = "listBoxChat";
            listBoxChat.Size = new System.Drawing.Size(206, 199);
            listBoxChat.TabIndex = 1;

            listBoxUsers = new ListBox();
            listBoxUsers.FormattingEnabled = true;
            listBoxUsers.Location = new System.Drawing.Point(218, 41);
            listBoxUsers.Name = "listBoxUsers";
            listBoxUsers.Size = new System.Drawing.Size(120, 199);
            listBoxUsers.TabIndex = 2;

            buttonBackChat = new Button();
            buttonBackChat.Location = new System.Drawing.Point(263, 246);
            buttonBackChat.Name = "buttonBackChat";
            buttonBackChat.Size = new System.Drawing.Size(75, 23);
            buttonBackChat.TabIndex = 3;
            buttonBackChat.Text = "Back";
            buttonBackChat.UseVisualStyleBackColor = true;
            buttonBackChat.Click += new EventHandler(buttonBackChat_Click);

            textBoxChat = new TextBox();
            textBoxChat.Location = new System.Drawing.Point(6, 248);
            textBoxChat.Name = "textBoxChat";
            textBoxChat.Size = new System.Drawing.Size(206, 20);
            textBoxChat.TabIndex = 4;

            labelLogin = new Label();
            labelLogin.AutoSize = true;
            labelLogin.Location = new System.Drawing.Point(120, 50);
            labelLogin.Name = "labelLogin";
            labelLogin.Size = new System.Drawing.Size(10, 13);
            labelLogin.TabIndex = 0;
            labelLogin.Text = " ";

            labelWin = new Label();
            labelWin.AutoSize = true;
            labelWin.Location = new System.Drawing.Point(120, 110);
            labelWin.Name = "labelWin";
            labelWin.Size = new System.Drawing.Size(26, 13);
            labelWin.TabIndex = 1;
            labelWin.Text = "win:";

            labelLoss = new Label();
            labelLoss.AutoSize = true;
            labelLoss.Location = new System.Drawing.Point(120, 139);
            labelLoss.Name = "labelLoss";
            labelLoss.Size = new System.Drawing.Size(25, 13);
            labelLoss.TabIndex = 2;
            labelLoss.Text = "loss";

            labelWinValue = new Label();
            labelWinValue.AutoSize = true;
            labelWinValue.Location = new System.Drawing.Point(152, 110);
            labelWinValue.Name = "labelWinValue";
            labelWinValue.Size = new System.Drawing.Size(13, 13);
            labelWinValue.TabIndex = 3;
            labelWinValue.Text = "0";

            labelLossValue = new Label();
            labelLossValue.AutoSize = true;
            labelLossValue.Location = new System.Drawing.Point(152, 139);
            labelLossValue.Name = "labelLossValue";
            labelLossValue.Size = new System.Drawing.Size(13, 13);
            labelLossValue.TabIndex = 4;
            labelLossValue.Text = "0";
            #endregion

            GroupBoxName = name;
            this.FormBorderStyle = FormBorderStyle.None;
        }

        void buttonBackChat_Click(object sender, EventArgs e) {
            OnOptionChoosen(MenuOption.Back);
        }

        void buttonGame_Click(object sender, EventArgs e) {
            OnOptionChoosen(MenuOption.Game);
        }

        public string Login
        {
            get
            { return labelLogin.Text; }
            set
            { labelLogin.Text = value; }
        }

        public int Loss
        {
            get
            { return int.Parse(labelLossValue.Text); }
            set
            { labelLossValue.Text = value.ToString(); }
        }

        public int Wins
        {
            get
            { return int.Parse(labelWinValue.Text); }
            set
            { labelWinValue.Text = value.ToString(); }
        }

        private GroupBox GetGroupBox(string name)
        {
            List<GroupBox> groupBoxList = new List<GroupBox>();
            GroupBox groupBox = null;

            groupBox = new GroupBox();
            groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox.Location = new System.Drawing.Point(0, 0);
            groupBox.Name = "groupBoxChat";
            groupBox.Size = new System.Drawing.Size(344, 275);
            groupBox.TabIndex = 0;
            groupBox.TabStop = false;
            groupBox.Text = "Chat";
            if (name == groupBox.Name)
            {
                groupBox.Controls.Add(textBoxChat);
                groupBox.Controls.Add(buttonBackChat);
                groupBox.Controls.Add(listBoxUsers);
                groupBox.Controls.Add(listBoxChat);
                groupBox.Controls.Add(buttonGame);
            }
            groupBoxList.Add(groupBox);

            groupBox = new GroupBox();
            groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox.Location = new System.Drawing.Point(0, 0);
            groupBox.Name = "groupBoxInfo";
            groupBox.Size = new System.Drawing.Size(344, 275);
            groupBox.TabIndex = 0;
            groupBox.TabStop = false;
            groupBox.Text = "Information";
            if (name == groupBox.Name)
            {
                groupBox.Controls.Add(labelLogin);
                groupBox.Controls.Add(labelWin);
                groupBox.Controls.Add(labelWinValue);
                groupBox.Controls.Add(labelLoss);
                groupBox.Controls.Add(labelLossValue);
            }
            groupBoxList.Add(groupBox);

            foreach (GroupBox gb in groupBoxList)
            {
                if (gb.Name == name)
                    return gb;
            }

            throw new ArgumentException("No group box with name " + name + " found");
        }

        public String GroupBoxName
        {
            get
            { return groupBox.Name; }
            set
            {
                Controls.Remove(groupBox);
                groupBox = GetGroupBox(value);
                Controls.Add(groupBox);
            }
        }
    }
}