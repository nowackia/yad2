using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Client.Log;

namespace Client.UI
{
    public partial class MainForm : Form
    {
        //PictureButton button = null;

        public MainForm()
        {
            InitializeComponent();
            InfoLog.WriteInfo("MainForm starts", EPrefix.Menu);
            /*
            button = new PictureButton();
            this.button.Location = new System.Drawing.Point(505, 12);
            this.button.Name = "button";
            this.button.Size = new System.Drawing.Size(75, 23);
            this.button.TabIndex = 0;

            this.button.Text = "Test text";

            this.button.BackgroundImage = Image.FromFile("UI_Up.png");
            this.button.PressedImage = Image.FromFile("UI_Up_Pressed.png");

            this.Controls.Add(this.button);*/
        }
    }
}