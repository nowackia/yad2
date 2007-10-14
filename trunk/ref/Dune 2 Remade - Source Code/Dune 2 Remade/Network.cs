using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Runtime.Serialization;

namespace Dune_2_Remade
{
    [Serializable]
    public class NetworkMessage
    {
        public int senderID;
    }
    [Serializable]
    class TextMessage : NetworkMessage
    {
        public string str;
    }
    [Serializable]
    class AttackMessage : NetworkMessage
    {
        public ID idAttacker, idAttacked;
        public int damage;
    }
    [Serializable]
    class MoveMessage : NetworkMessage
    {
        public ID objectID;
        public List<Point> way; //jeœli wysy³a client, to tylko position i destination ma tu byæ
    }
    [Serializable]
    class CreateMessage : NetworkMessage
    {
        public CreateMessage(ID id, string name, Point position, bool isBuilding)
        {
            objectID = id;
            this.name = name;
            this.position = position;
            this.isBuilding = isBuilding;
        }
        public ID objectID;
        public string name;
        public Point position;
        public bool isBuilding;
    }
    [Serializable]
    class DestroyMessage : NetworkMessage
    {
        public ID objectID;
    }

    [Serializable]
    class IntroduceMessage : NetworkMessage
    {
        public string playerNick;
        public Houses.House house;
    }
    [Serializable]
    class ResponseMessage : NetworkMessage
    {
        public int NewID;
    }
    [Serializable]
    class PlayersListMessage : NetworkMessage
    {
        public List<User> players;
    }
    [Serializable]
    class StartGameMessage : NetworkMessage { }    

    [Serializable]
    class ReadyToPlayMessage : NetworkMessage { }
    public class Network
    {        
        public static Queue<NetworkMessage> messages=new Queue<NetworkMessage>();
        static int Port = 52987;
        public static bool isHost = false;
        public static Dictionary<TcpClient, int> connections = new Dictionary<TcpClient,int>();
        public static TcpListener listener = null;        
        static bool listening = true;
        
        public static void DisconnectFromServer()
        {
            TcpClient[] ac = new TcpClient[1];
            Network.connections.Keys.CopyTo(ac, 0);
            ac[0].Client.Close();
        }

        public static bool Join(string IPAddress)
        {
            TcpClient client = new TcpClient();
            try
            {
                client.Connect(IPAddress, Port);
            }
            catch
            {
                return false;
            }
            isHost = false;

            ConnectionThread ct = new ConnectionThread();
            ct.client = client;
            ThreadPool.QueueUserWorkItem(new WaitCallback(ct.ClientHandleConnection));

            return true;
        }
        public static void stopListener()
        {
            listening = false;
        }
        public static void StopServer()
        {
            foreach (TcpClient c in connections.Keys)
            {
                c.Client.Close();
                c.Close();
            }
            //connections.Clear();
        }

        public static void SendMessage(NetworkMessage nm)
        {
            foreach (TcpClient c in connections.Keys )
            {
                if (c.Connected)
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(c.GetStream(), nm);
                }
            }            
        }
        
        public static void waitForClients(object state)
        {
            if (listener == null) listener = new TcpListener(Port);
            listener.Start();
            //Console.WriteLine("Waiting for clients...");
            while (listening)
            {
                while (!listener.Pending() && listening)
                {
                    Thread.Sleep(100);
                }
                if (listening)
                {
                    ConnectionThread ct = new ConnectionThread();
                    ct.client = listener.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ct.ServerHandleConnection));
                }
            }
            listener.Server.Close();
            listener.Stop();            
            listener = null;
        }
        public static void Host()
        {
            isHost = true;
            listening = true;
            ThreadPool.QueueUserWorkItem(new WaitCallback(waitForClients));
        }
    }

    public class ConnectionThread
    {
        public static event EventHandler NewMessage;
        public static event EventHandler PlayersUpdate;
        public static event EventHandler DisconnectedFromServer;

        public TcpClient client;

        public void ServerHandleConnection(object state)
        {
            NetworkStream ns = client.GetStream();

            BinaryFormatter bf = new BinaryFormatter();
            NetworkMessage nm;
            try
            {
                while (true)
                {
                    nm = (NetworkMessage)bf.Deserialize(ns);
                    if (nm == null) continue;

                    if (nm is IntroduceMessage)
                    {
                        IntroduceMessage im = nm as IntroduceMessage;

                        int userID = User.GetNewID();
                        Network.connections.Add(client, userID);

                        ResponseMessage rm = new ResponseMessage();
                        rm.senderID = GlobalData.me.userID;
                        rm.NewID = userID;
                        bf.Serialize(ns, rm);

                        User u = new User();
                        u.name = im.playerNick;
                        u.House = im.house;
                        u.userID = userID;

                        GlobalData.usersList.Add(u);
                        //PlayersListMessage playerList = new PlayersListMessage();
                        //playerList.players = GlobalData.usersList;
                        //bf.Serialize(ns, playerList);
                        PlayersListMessage pl = new PlayersListMessage();
                        pl.players = GlobalData.usersList;
                        pl.senderID = GlobalData.me.userID;
                        Network.SendMessage(pl);

                        if (PlayersUpdate != null)
                            PlayersUpdate(this, new EventArgs());
                    }
                    if (nm is TextMessage)
                    {
                        lock (Network.messages)
                        {
                            Network.messages.Enqueue(nm);
                        }
                        if (NewMessage != null)
                            NewMessage(this, new EventArgs());
                        Network.SendMessage(nm);
                    }
                    if (nm is PlayersListMessage)
                    {
                        if (((PlayersListMessage)nm).players.Count == 1)
                        {
                            User u = ((PlayersListMessage)nm).players[0];
                            int i;
                            for (i = 0; i < GlobalData.usersList.Count; i++)
                            {
                                if (GlobalData.usersList[i].userID == u.userID)
                                {
                                    GlobalData.usersList[i].name = u.name;
                                    GlobalData.usersList[i].House = u.House;
                                    break;
                                }
                            }
                            //tego nie powinno byæ
                            if (i == GlobalData.usersList.Count)
                            {
                                client.Close();
                                Network.connections.Remove(client);
                                throw new Exception(); //Not on the list!
                            }
                            PlayersListMessage pl = new PlayersListMessage();
                            pl.players = GlobalData.usersList;
                            pl.senderID = GlobalData.me.userID;
                            Network.SendMessage(pl);

                            if (PlayersUpdate != null)
                                PlayersUpdate(this, new EventArgs());
                        }
                    }
                    if (nm is ReadyToPlayMessage)
                    {
                        int u=Network.connections[client];
                        for(int i=0;i<GlobalData.usersList.Count;i++)
                        {
                            if (u==GlobalData.usersList[i].userID)
                            {
                                GlobalData.playersReadyToPlay[i-1] = true;
                                break;
                            }
                        }
                    }
                    if (nm is CreateMessage)
                    {
                        if (GlobalData.isPlaying)
                        {
                            lock (Network.messages)
                            {
                                Network.messages.Enqueue((CreateMessage)nm);
                            }
                            //Network.SendMessage(nm);
                        }
                    }
                    if (nm is DestroyMessage)
                    {
                        if (GlobalData.isPlaying)
                        {
                            lock (Network.messages)
                            {
                                Network.messages.Enqueue((DestroyMessage)nm);
                            }
                            Network.SendMessage(nm);
                        }
                    }
                    if (nm is MoveMessage)
                    {
                        if (GlobalData.isPlaying)
                        {
                            lock (Network.messages)
                            {
                                Network.messages.Enqueue((MoveMessage)nm);
                            }                            
                        }
                    }
                    if (nm is AttackMessage)
                    {
                        if (GlobalData.isPlaying)
                        {
                            lock (Network.messages)
                            {
                                Network.messages.Enqueue((AttackMessage)nm);
                            }
                            Network.SendMessage(nm);
                        }
                    }
                }
            }
            catch (Exception)
            {
                //MessageBox.Show(exc.Message);
            }
            finally
            {
                ns.Close();
                AfterDisconnection();

                PlayersListMessage pl = new PlayersListMessage();
                pl.players = GlobalData.usersList;
                pl.senderID = GlobalData.me.userID;
                Network.SendMessage(pl);
            }
        }

        public void ClientHandleConnection(object state)
        {
            //   TcpClient            
            NetworkStream ns = client.GetStream();

            BinaryFormatter bf = new BinaryFormatter();
            IntroduceMessage i = new IntroduceMessage();
            i.senderID = -1;
            i.playerNick = GlobalData.me.name;
            i.house = GlobalData.me.House;
            bf.Serialize(ns, i);
            NetworkMessage nm;
            nm = bf.Deserialize(ns) as NetworkMessage;
            if (nm != null)
            {
                if (nm is ResponseMessage)
                {
                    GlobalData.me.userID = ((ResponseMessage)nm).NewID;
                    Network.connections.Add(client, GlobalData.me.userID);
                }
            }
            try
            {
                while (true)
                {
                    nm = (NetworkMessage)bf.Deserialize(ns);
                    if (nm == null) continue;

                    if (nm is TextMessage)
                    {
                        lock (Network.messages) // tu by³o ;
                        {
                            Network.messages.Enqueue(nm);
                        }
                        if (NewMessage != null)
                            NewMessage(this, new EventArgs());
                    }
                    if (nm is PlayersListMessage)
                    {
                        lock (GlobalData.usersList)
                        {
                            GlobalData.usersList = ((PlayersListMessage)nm).players;
                        }
                        if (PlayersUpdate != null)
                            PlayersUpdate(this, new EventArgs());
                    }
                    if (nm is StartGameMessage)
                    {
                        lock (Network.messages)
                        {
                            Network.messages.Enqueue((StartGameMessage)nm);
                        }
                        GlobalData.playersReadyToPlay = new bool[1];
                        if (NewMessage != null)
                            NewMessage(this, new EventArgs());
                    }
                    if (nm is CreateMessage)
                    {
                        if (GlobalData.isPlaying)
                        {
                            lock(Network.messages)
                            {
                                Network.messages.Enqueue((CreateMessage)nm);
                            }                          
                        }
                    }
                    if (nm is DestroyMessage)
                    {
                        if (GlobalData.isPlaying)
                        {
                            lock (Network.messages)
                            {
                                Network.messages.Enqueue((DestroyMessage)nm);
                            }                     
                        }
                    }
                    if (nm is MoveMessage)
                    {
                        if (GlobalData.isPlaying)
                        {
                            lock (Network.messages)
                            {
                                Network.messages.Enqueue((MoveMessage)nm);
                            }
                        }
                    }
                    if (nm is AttackMessage)
                    {
                        if (GlobalData.isPlaying)
                        {
                            lock (Network.messages)
                            {
                                Network.messages.Enqueue((AttackMessage)nm);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                //MessageBox.Show(exc.Message);
            }
            finally
            {
                ns.Close();
                AfterDisconnection();
            }
        }

        private void AfterDisconnection()
        {
            GlobalData.DeleteUserByID(Network.connections[client]);
            Network.connections.Remove(client);
            try //can be already closed by 'DisconnectFromServer'
            {
                client.Client.Close();
            }
            catch{}

            client.Close();

            if (PlayersUpdate != null)
                PlayersUpdate(this, new EventArgs());
            if (DisconnectedFromServer != null)
                DisconnectedFromServer(this, new EventArgs());
        }
    }
}