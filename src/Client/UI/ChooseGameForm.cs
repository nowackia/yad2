using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Client.UI
{
    public partial class ChooseGameForm : Form
    {
        private GroupBox groupBox;

        private Button buttonBack;
        private Button buttonJoinCreate;
        private Button buttonJoin;

        private TextBox textBoxDescription;
        private TextBox textBoxJoinGame;

        private ListBox listBoxGames;

        private ListBox listBoxMaps;

        private RadioButton radioButtonPublic;
        private RadioButton radioButtonPrivate;

        private TextBox textBoxCreateGame;

        private Button buttonCreate;
        private Button buttonCreateCancel;

        private ListBox listBoxPlayersDescription;

        private Button buttonStartGame;
        private Button buttonStartCancel;

        private TextBox textBoxGameDescription;

        public ChooseGameForm(string name)
        {
            InitializeComponent();

            #region Controls Initialization
            textBoxJoinGame = new TextBox();
            textBoxJoinGame.Location = new System.Drawing.Point(7, 219);
            textBoxJoinGame.Name = "textBoxGame";
            textBoxJoinGame.Size = new System.Drawing.Size(147, 20);
            textBoxJoinGame.TabIndex = 4;

            buttonBack = new Button();
            buttonBack.Location = new System.Drawing.Point(263, 246);
            buttonBack.Name = "buttonBack";
            buttonBack.Size = new System.Drawing.Size(75, 23);
            buttonBack.TabIndex = 3;
            buttonBack.Text = "Back";
            buttonBack.UseVisualStyleBackColor = true;

            textBoxDescription = new TextBox();
            textBoxDescription.Location = new System.Drawing.Point(216, 19);
            textBoxDescription.Multiline = true;
            textBoxDescription.Name = "textBoxDescription";
            textBoxDescription.Size = new System.Drawing.Size(122, 221);
            textBoxDescription.TabIndex = 2;

            listBoxGames = new ListBox();
            listBoxGames.FormattingEnabled = true;
            listBoxGames.Location = new System.Drawing.Point(6, 19);
            listBoxGames.Name = "listBoxGames";
            listBoxGames.Size = new System.Drawing.Size(204, 186);
            listBoxGames.TabIndex = 1;

            buttonJoinCreate = new Button();
            buttonJoinCreate.Location = new System.Drawing.Point(6, 246);
            buttonJoinCreate.Name = "buttonCreate";
            buttonJoinCreate.Size = new System.Drawing.Size(75, 23);
            buttonJoinCreate.TabIndex = 0;
            buttonJoinCreate.Text = "Create";
            buttonJoinCreate.UseVisualStyleBackColor = true;
            buttonJoinCreate.Click += new EventHandler(buttonJoinCreate_Click);

            buttonJoin = new Button();
            buttonJoin.Location = new System.Drawing.Point(160, 216);
            buttonJoin.Name = "buttonJoin";
            buttonJoin.Size = new System.Drawing.Size(50, 23);
            buttonJoin.TabIndex = 5;
            buttonJoin.Text = "Join";
            buttonJoin.UseVisualStyleBackColor = true;

            buttonCreate = new Button();
            buttonCreate.Location = new System.Drawing.Point(247, 89);
            buttonCreate.Name = "buttonCreate";
            buttonCreate.Size = new System.Drawing.Size(75, 23);
            buttonCreate.TabIndex = 4;
            buttonCreate.Text = "Create";
            buttonCreate.UseVisualStyleBackColor = true;
            buttonCreate.Click += new EventHandler(buttonCreate_Click);

            listBoxMaps = new ListBox();
            listBoxMaps.FormattingEnabled = true;
            listBoxMaps.Location = new System.Drawing.Point(6, 45);
            listBoxMaps.Name = "listBoxMaps";
            listBoxMaps.Size = new System.Drawing.Size(200, 225);
            listBoxMaps.TabIndex = 3;

            radioButtonPublic = new RadioButton();
            radioButtonPublic.AutoSize = true;
            radioButtonPublic.Checked = true;
            radioButtonPublic.Location = new System.Drawing.Point(247, 42);
            radioButtonPublic.Name = "radioButtonPublic";
            radioButtonPublic.Size = new System.Drawing.Size(53, 17);
            radioButtonPublic.TabIndex = 2;
            radioButtonPublic.TabStop = true;
            radioButtonPublic.Text = "public";
            radioButtonPublic.UseVisualStyleBackColor = true;

            radioButtonPrivate = new RadioButton();
            radioButtonPrivate.AutoSize = true;
            radioButtonPrivate.Location = new System.Drawing.Point(247, 19);
            radioButtonPrivate.Name = "radioButtonPrivate";
            radioButtonPrivate.Size = new System.Drawing.Size(57, 17);
            radioButtonPrivate.TabIndex = 1;
            radioButtonPrivate.Text = "private";
            radioButtonPrivate.UseVisualStyleBackColor = true;

            textBoxCreateGame = new TextBox();
            textBoxCreateGame.Location = new System.Drawing.Point(6, 19);
            textBoxCreateGame.Name = "textBoxCreateGame";
            textBoxCreateGame.Size = new System.Drawing.Size(200, 20);
            textBoxCreateGame.TabIndex = 0;

            buttonCreateCancel = new Button();
            buttonCreateCancel.Location = new System.Drawing.Point(247, 118);
            buttonCreateCancel.Name = "buttonCreateCancel";
            buttonCreateCancel.Size = new System.Drawing.Size(75, 23);
            buttonCreateCancel.TabIndex = 5;
            buttonCreateCancel.Text = "Cancel";
            buttonCreateCancel.UseVisualStyleBackColor = true;
            buttonCreateCancel.Click += new EventHandler(buttonCreateCancel_Click);

            buttonStartCancel = new Button();
            buttonStartCancel.Location = new System.Drawing.Point(263, 246);
            buttonStartCancel.Name = "buttonStartCancel";
            buttonStartCancel.Size = new System.Drawing.Size(75, 23);
            buttonStartCancel.TabIndex = 0;
            buttonStartCancel.Text = "Cancel";
            buttonStartCancel.UseVisualStyleBackColor = true;
            buttonStartCancel.Click += new EventHandler(buttonStartCancel_Click);

            buttonStartGame = new Button();
            buttonStartGame.Location = new System.Drawing.Point(6, 246);
            buttonStartGame.Name = "buttonStartGame";
            buttonStartGame.Size = new System.Drawing.Size(75, 23);
            buttonStartGame.TabIndex = 1;
            buttonStartGame.Text = "Start Game";
            buttonStartGame.UseVisualStyleBackColor = true;
            buttonStartGame.Click += new EventHandler(buttonStartGame_Click);

            listBoxPlayersDescription = new ListBox();
            listBoxPlayersDescription.FormattingEnabled = true;
            listBoxPlayersDescription.Location = new System.Drawing.Point(6, 19);
            listBoxPlayersDescription.Name = "listBoxPlayersDescription";
            listBoxPlayersDescription.Size = new System.Drawing.Size(224, 212);
            listBoxPlayersDescription.TabIndex = 2;

            textBoxGameDescription = new TextBox();
            textBoxGameDescription.Location = new System.Drawing.Point(236, 19);
            textBoxGameDescription.Multiline = true;
            textBoxGameDescription.Name = "textBoxGameDescription";
            textBoxGameDescription.Size = new System.Drawing.Size(102, 212);
            textBoxGameDescription.TabIndex = 3;
            #endregion

            GroupBoxName = name;
        }

        void buttonStartGame_Click(object sender, EventArgs e)
        {
            MainForm mainForm = new MainForm();
            mainForm.Show();
        }

        void buttonStartCancel_Click(object sender, EventArgs e)
        {
            GroupBoxName = "groupBoxJoin";
        }

        void buttonCreate_Click(object sender, EventArgs e)
        {
            GroupBoxName = "groupBoxStart";
        }

        void buttonCreateCancel_Click(object sender, EventArgs e)
        {
            GroupBoxName = "groupBoxJoin";
        }

        void buttonJoinCreate_Click(object sender, EventArgs e)
        {
            GroupBoxName = "groupBoxCreate";
        }

        private GroupBox GetGroupBox(string name)
        {
            List<GroupBox> groupBoxList = new List<GroupBox>();
            GroupBox groupBox = null;

            groupBox = new GroupBox();
            groupBox.Controls.Add(buttonJoin);
            groupBox.Controls.Add(textBoxJoinGame);
            groupBox.Controls.Add(buttonBack);
            groupBox.Controls.Add(textBoxDescription);
            groupBox.Controls.Add(listBoxGames);
            groupBox.Controls.Add(buttonJoinCreate);
            groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox.Location = new System.Drawing.Point(0, 0);
            groupBox.Name = "groupBoxJoin";
            groupBox.Size = new System.Drawing.Size(344, 275);
            groupBox.TabIndex = 0;
            groupBox.TabStop = false;
            groupBox.Text = "Join Game";
            if (name == groupBox.Name)
            {
                groupBox.Controls.Add(buttonJoin);
                groupBox.Controls.Add(textBoxJoinGame);
                groupBox.Controls.Add(buttonBack);
                groupBox.Controls.Add(textBoxDescription);
                groupBox.Controls.Add(listBoxGames);
                groupBox.Controls.Add(buttonJoinCreate);
            }
            groupBoxList.Add(groupBox);

            groupBox = new GroupBox();
            groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox.Location = new System.Drawing.Point(0, 0);
            groupBox.Name = "groupBoxCreate";
            groupBox.Size = new System.Drawing.Size(344, 275);
            groupBox.TabIndex = 0;
            groupBox.TabStop = false;
            groupBox.Text = "Create Game";
            if (name == groupBox.Name)
            {
                groupBox.Controls.Add(buttonCreateCancel);
                groupBox.Controls.Add(buttonCreate);
                groupBox.Controls.Add(listBoxMaps);
                groupBox.Controls.Add(radioButtonPublic);
                groupBox.Controls.Add(radioButtonPrivate);
                groupBox.Controls.Add(textBoxCreateGame);
            }
            groupBoxList.Add(groupBox);

            groupBox = new GroupBox();
            groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox.Location = new System.Drawing.Point(0, 0);
            groupBox.Name = "groupBoxStart";
            groupBox.Size = new System.Drawing.Size(344, 275);
            groupBox.TabIndex = 0;
            groupBox.TabStop = false;
            groupBox.Text = "Start Game";
            if (name == groupBox.Name)
            {
                groupBox.Controls.Add(textBoxGameDescription);
                groupBox.Controls.Add(listBoxPlayersDescription);
                groupBox.Controls.Add(buttonStartGame);
                groupBox.Controls.Add(buttonStartCancel);
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