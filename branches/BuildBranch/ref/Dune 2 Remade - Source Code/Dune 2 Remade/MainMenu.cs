using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Dune_2_Remade
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();

            GlobalData.me.name = "DunePlayer";

            this.btnExit.Text = Properties.Resources.MM_Exit;
            this.btnHost.Text = Properties.Resources.MM_HostGame;
            this.btnJoin.Text = Properties.Resources.MM_JoinGame;
            this.btnOptions.Text = Properties.Resources.MM_Options;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOptions_Click(object sender, EventArgs e)
        {
            OptionsMenu o = new OptionsMenu();
            o.PlayerNick = GlobalData.me.name;
            
            if (o.ShowDialog() == DialogResult.OK)
            {
                GlobalData.me.name = o.PlayerNick;
                //cos
            }
        }

        private void OnGameLobby(object sender, EventArgs e)
        {
            //this.Visible = false;

            GlobalData.usersList.Clear();
            GlobalData.me.userID = User.GetNewID();
            //GlobalData.usersList.Add(GlobalData.me);

            Network.connections.Clear();
            Network.messages.Clear();

            bool isHost = ( ((Button)sender).Tag.ToString() == "Host") ? true : false;
            LobbyMenu l = new LobbyMenu(isHost);
            l.ShowDialog();
            if (GlobalData.isPlaying == true)
                Close();

            //Visible = true;
        }
    }
}