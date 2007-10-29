using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Client.UI
{
    public partial class LoginForm : Form
    {
        private GroupBox groupBox;

        private MaskedTextBox maskedTextBoxServer;

        private TextBox textBoxPassword;
        private TextBox textBoxRepeatPassword;
        private TextBox textBoxMail;
        private TextBox textBoxLogin;

        private Label labelServer;
        private Label labelPassword;
        private Label labelRepeatPassword;
        private Label labelLogin;
        private Label labelMail;

        private Button buttonBack;
        private Button buttonCancel;
        private Button buttonLoginRegister;
        private Button buttonLogin;
        private Button buttonRemind;
        private Button buttonRegister;

        public LoginForm(string name)
        {
            InitializeComponent();

            #region Controls Initialization
            buttonBack = new Button();
            buttonBack.Location = new System.Drawing.Point(175, 228);
            buttonBack.Name = "buttonBack";
            buttonBack.Size = new System.Drawing.Size(100, 23);
            buttonBack.TabIndex = 10;
            buttonBack.Text = "Back";
            buttonBack.UseVisualStyleBackColor = true;
            buttonBack.Click += new EventHandler(buttonBack_Click);

            buttonCancel = new Button();
            buttonCancel.Location = new System.Drawing.Point(175, 228);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new System.Drawing.Size(100, 23);
            buttonCancel.TabIndex = 10;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;

            buttonLoginRegister = new Button();
            buttonLoginRegister.Location = new System.Drawing.Point(40, 228);
            buttonLoginRegister.Name = "buttonLoginRegister";
            buttonLoginRegister.Size = new System.Drawing.Size(100, 23);
            buttonLoginRegister.TabIndex = 9;
            buttonLoginRegister.Text = "Register";
            buttonLoginRegister.UseVisualStyleBackColor = true;
            buttonLoginRegister.Click += new EventHandler(buttonLoginRegister_Click);

            buttonRegister = new Button();
            buttonRegister.Location = new System.Drawing.Point(40, 228);
            buttonRegister.Name = "buttonLoginRegister";
            buttonRegister.Size = new System.Drawing.Size(100, 23);
            buttonRegister.TabIndex = 9;
            buttonRegister.Text = "Register";
            buttonRegister.UseVisualStyleBackColor = true;

            buttonLogin = new Button();
            buttonLogin.Location = new System.Drawing.Point(175, 189);
            buttonLogin.Name = "buttonLogin";
            buttonLogin.Size = new System.Drawing.Size(100, 23);
            buttonLogin.TabIndex = 8;
            buttonLogin.Text = "Login";
            buttonLogin.UseVisualStyleBackColor = true;

            buttonRemind = new Button();
            buttonRemind.Location = new System.Drawing.Point(40, 189);
            buttonRemind.Name = "buttonRemind";
            buttonRemind.Size = new System.Drawing.Size(100, 23);
            buttonRemind.TabIndex = 11;
            buttonRemind.Text = "Remind Password";
            buttonRemind.UseVisualStyleBackColor = true;

            maskedTextBoxServer = new MaskedTextBox();
            maskedTextBoxServer.Location = new System.Drawing.Point(175, 97);
            maskedTextBoxServer.Name = "maskedTextBoxServer";
            maskedTextBoxServer.Size = new System.Drawing.Size(140, 20);
            maskedTextBoxServer.TabIndex = 6;

            textBoxPassword = new TextBox();
            textBoxPassword.Location = new System.Drawing.Point(175, 71);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.PasswordChar = '*';
            textBoxPassword.Size = new System.Drawing.Size(140, 20);
            textBoxPassword.TabIndex = 5;

            textBoxRepeatPassword = new TextBox();
            textBoxRepeatPassword.Location = new System.Drawing.Point(175, 97);
            textBoxRepeatPassword.Name = "textBoxRepeatPassword";
            textBoxRepeatPassword.PasswordChar = '*';
            textBoxRepeatPassword.Size = new System.Drawing.Size(140, 20);
            textBoxRepeatPassword.TabIndex = 5;

            textBoxMail = new TextBox();
            textBoxMail.Location = new System.Drawing.Point(175, 123);
            textBoxMail.Name = "textBoxMail";
            textBoxMail.Size = new System.Drawing.Size(140, 20);
            textBoxMail.TabIndex = 3;

            textBoxLogin = new TextBox();
            textBoxLogin.Location = new System.Drawing.Point(175, 45);
            textBoxLogin.Name = "textBoxLogin";
            textBoxLogin.Size = new System.Drawing.Size(140, 20);
            textBoxLogin.TabIndex = 3;

            labelServer = new Label();
            labelServer.AutoSize = true;
            labelServer.Location = new System.Drawing.Point(62, 100);
            labelServer.Name = "labelServer";
            labelServer.Size = new System.Drawing.Size(78, 13);
            labelServer.TabIndex = 2;
            labelServer.Text = "Server address";

            labelPassword = new Label();
            labelPassword.AutoSize = true;
            labelPassword.Location = new System.Drawing.Point(62, 74);
            labelPassword.Name = "labelPassword";
            labelPassword.Size = new System.Drawing.Size(53, 13);
            labelPassword.TabIndex = 1;
            labelPassword.Text = "Password";

            labelLogin = new Label();
            labelLogin.AutoSize = true;
            labelLogin.Location = new System.Drawing.Point(62, 48);
            labelLogin.Name = "labelLogin";
            labelLogin.Size = new System.Drawing.Size(33, 13);
            labelLogin.TabIndex = 0;
            labelLogin.Text = "Login";

            labelRepeatPassword = new Label();
            labelRepeatPassword.AutoSize = true;
            labelRepeatPassword.Location = new System.Drawing.Point(62, 100);
            labelRepeatPassword.Name = "labelRepeatPassword";
            labelRepeatPassword.Size = new System.Drawing.Size(33, 13);
            labelRepeatPassword.TabIndex = 0;
            labelRepeatPassword.Text = "Repeat Password";

            labelMail = new Label();
            labelMail.AutoSize = true;
            labelMail.Location = new System.Drawing.Point(62, 126);
            labelMail.Name = "labelMail";
            labelMail.Size = new System.Drawing.Size(33, 13);
            labelMail.TabIndex = 0;
            labelMail.Text = "Mail";
            #endregion

            GroupBoxName = name;
        }

        void buttonBack_Click(object sender, EventArgs e)
        {
            GroupBoxName = "groupBoxLogin";
        }

        void buttonLoginRegister_Click(object sender, EventArgs e)
        {
            GroupBoxName = "groupBoxRegister";
        }

        private GroupBox GetGroupBox(string name)
        {
            List<GroupBox> groupBoxList = new List<GroupBox>();
            GroupBox groupBox = null;

            groupBox = new GroupBox();
            groupBox.Dock = DockStyle.Fill;
            groupBox.Location = new System.Drawing.Point(0, 0);
            groupBox.Name = "groupBoxLogin";
            groupBox.Size = new System.Drawing.Size(344, 275);
            groupBox.TabIndex = 0;
            groupBox.TabStop = false;
            groupBox.Text = "Login";
            if (name == groupBox.Name)
            {
                groupBox.Controls.Add(buttonRemind);
                groupBox.Controls.Add(buttonCancel);
                groupBox.Controls.Add(buttonLoginRegister);
                groupBox.Controls.Add(buttonLogin);
                groupBox.Controls.Add(maskedTextBoxServer);
                groupBox.Controls.Add(textBoxPassword);
                groupBox.Controls.Add(textBoxLogin);
                groupBox.Controls.Add(labelServer);
                groupBox.Controls.Add(labelPassword);
                groupBox.Controls.Add(labelLogin);
            }
            groupBoxList.Add(groupBox);

            groupBox = new GroupBox();
            groupBox.Dock = DockStyle.Fill;
            groupBox.Location = new System.Drawing.Point(0, 0);
            groupBox.Name = "groupBoxRegister";
            groupBox.Size = new System.Drawing.Size(344, 275);
            groupBox.TabIndex = 0;
            groupBox.TabStop = false;
            groupBox.Text = "Register";
            if (name == groupBox.Name)
            {
                groupBox.Controls.Add(textBoxLogin);
                groupBox.Controls.Add(textBoxPassword);
                groupBox.Controls.Add(textBoxRepeatPassword);
                groupBox.Controls.Add(textBoxMail);

                groupBox.Controls.Add(labelLogin);
                groupBox.Controls.Add(labelPassword);
                groupBox.Controls.Add(labelRepeatPassword);
                groupBox.Controls.Add(labelMail);

                groupBox.Controls.Add(buttonRegister);
                groupBox.Controls.Add(buttonBack);
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