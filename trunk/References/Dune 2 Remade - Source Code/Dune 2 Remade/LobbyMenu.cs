using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace Dune_2_Remade
{
    public partial class LobbyMenu : Form
    {        
        public LobbyMenu(bool isHost)
        {
            InitializeComponent();           

            btnDisconnect.Enabled = false;
            if (isHost == true)
                pnlJoin.Visible = false;
            else
                pnlHost.Visible = false;
            
            Network.isHost = isHost;
            GlobalData.isPlaying = false;

            if (isHost == true)
                Network.Host();

            lstPlayers.DisplayMember = "name";
            GlobalData.usersList.Clear();
            GlobalData.usersList.Add(GlobalData.me);
            lstPlayers.DataSource = GlobalData.usersList;

            ConnectionThread.NewMessage += new EventHandler(OnNewMessage);
            ConnectionThread.PlayersUpdate += new EventHandler(OnPlayersUpdate);

            if (isHost == false)
                ConnectionThread.DisconnectedFromServer += new EventHandler(OnDisconnectedFromServer);

            btnClose.Text = Properties.Resources.LM_Close;
            btnDisconnect.Text = Properties.Resources.LM_Disconnect;
            btnJoin.Text = Properties.Resources.LM_Join;
            btnStart.Text = Properties.Resources.LM_StartGame;
        }

        private void OnDisconnectedFromServer(object sender, EventArgs e)
        {
            btnJoin.Enabled = true;
            btnDisconnect.Enabled = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (Network.isHost)
            {
                Network.stopListener();
                Network.StopServer();
            }
            Close();
            
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            ConnectionThread.NewMessage -= new EventHandler(OnNewMessage);
            ConnectionThread.PlayersUpdate -= new EventHandler(OnPlayersUpdate);
            GlobalData.playersReadyToPlay = new bool[Network.connections.Count];
            for (int i = 0; i < GlobalData.playersReadyToPlay.Length; i++)
                GlobalData.playersReadyToPlay[i] = false;
            if (Network.isHost == false)
                ConnectionThread.DisconnectedFromServer -= new EventHandler(OnDisconnectedFromServer);

            GlobalData.isPlaying = true;
            Network.SendMessage(new StartGameMessage());
            Close();
        }

        private void btnJoin_Click(object sender, EventArgs e)
        {
            IPAddress ip;
            if (IPAddress.TryParse(txtIP.Text, out ip) == true)
            {
                if (Network.Join(txtIP.Text) == true)
                {
                    btnJoin.Enabled = false;
                    btnDisconnect.Enabled = true;
                }
                else
                {
                    txtChat.Text += "Connection Timeout\r\n";
                }
            }
            else
            {
                txtChat.Text += "Wrong IP\r\n";
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            Network.DisconnectFromServer();
            btnDisconnect.Enabled = false;
            btnJoin.Enabled = true;
        }

        private void HouseChangeUpdate()
        {
            if (GlobalData.me.userID > 0)
            {
                if (!Network.isHost)
                {
                    PlayersListMessage pl = new PlayersListMessage();
                    List<User> l = new List<User>();
                    l.Add(GlobalData.me);
                    pl.players = l;
                    Network.SendMessage(pl);
                }
                else
                {
                    GlobalData.usersList[0].House = GlobalData.me.House;
                    PlayersListMessage pl = new PlayersListMessage();
                    pl.players = GlobalData.usersList;
                    Network.SendMessage(pl);
                }
            }
            UpdatePlayerList();
        }

        private void OnPlayersUpdate(object sender, EventArgs e)
        {
            lstPlayers.Invoke(new UpdatePlayerListDelegate(UpdatePlayerList), null);
        }
        private delegate void UpdatePlayerListDelegate();
        private void UpdatePlayerList()
        {
            lstPlayers.DataSource = null;
            lstPlayers.DataSource = GlobalData.usersList;
            //lstPlayers.Refresh();
        }
        private void OnGameStart(object sender, EventArgs e)
        {
            Close();
        }
        private void OnNewMessage(object sender, EventArgs e)
        {
            NetworkMessage nm = null;
            try
            {
                lock (Network.messages)
                {
                    if (Network.messages.Count > 0)
                        nm = Network.messages.Dequeue();
                    else
                        return;
                }
                if (nm is TextMessage)
                {
                    TextMessage tm = nm as TextMessage;

                    txtChat.Invoke(new UpdateChatBoxDelegate(UpdateChatBox), new object[] { GlobalData.GetNameByID(tm.senderID) + ": " + tm.str + "\r\n" });
                }
                if (nm is StartGameMessage)
                {
                    ConnectionThread.NewMessage -= new EventHandler(OnNewMessage);
                    ConnectionThread.PlayersUpdate -= new EventHandler(OnPlayersUpdate);
                    if (Network.isHost == false)
                        ConnectionThread.DisconnectedFromServer -= new EventHandler(OnDisconnectedFromServer);
                    GlobalData.isPlaying = true;
                    this.Invoke(new EventHandler(OnGameStart), null);                    
                }
            }
            catch (Exception)
            {

            }
        }
        private delegate void UpdateChatBoxDelegate(string str);
        private void UpdateChatBox(string str)
        {
            txtChat.Text += str;
        }

        private void txtChatInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                if(Network.isHost)
                    txtChat.Text += GlobalData.me.name + ": " +  txtChatInput.Text + "\r\n";
                TextMessage tm = new TextMessage();
                tm.senderID = GlobalData.me.userID;
                tm.str = txtChatInput.Text;
                try
                {
                    Network.SendMessage(tm);
                }
                catch (Exception exc) { MessageBox.Show(exc.Message); }
                txtChatInput.Text = null;
            }
        }

        private void LobbyMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            ConnectionThread.NewMessage -= new EventHandler(OnNewMessage);
            ConnectionThread.PlayersUpdate -= new EventHandler(OnPlayersUpdate);
            if (Network.isHost == false)
                ConnectionThread.DisconnectedFromServer -= new EventHandler(OnDisconnectedFromServer);

            Network.stopListener();
            //lstPlayers.DataSource = null;
            //GlobalData.usersList.Clear();
        }

        private void LobbyMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Network.killListener();
        }

        private void OnButtonHouseClicked(object sender, EventArgs e)
        {
            GlobalData.me.House = (Houses.House)Enum.Parse(typeof(Houses.House), (string)((Button)sender).Tag);
            HouseChangeUpdate();
        }
    }
}