using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Server;
using Yad.Net.Client;
using Yad.Net.Messaging.Common;
using Yad.Net.Common;
using System.Threading;

namespace CommunicationTest {
    public class ClientSendingThread{
        ClientConn cc;
        int milisToSleep = 1000;
        public ClientSendingThread(ClientConn conn){
            cc = conn;
        }

        public void ThreadProcess(){
            while(true){
                cc.SendMessage(MessageFactory.Create(MessageType.TurnAsk));
                Thread.Sleep(milisToSleep);
            }
        }
    }
    class Program {
        static ServerMain _sm;
        static int clientNo = 4;
        static ClientConn[] _clients;
        static TurnAskTimeCounter[] _askCounters;
        static string _gameName = "dupa";
        static void Main(string[] args) {
            _sm = new ServerMain();
            _clients = new ClientConn[clientNo];
            _askCounters = new TurnAskTimeCounter[clientNo];
            for (int i = 0; i < clientNo; ++i) {
                _clients[i] = new ClientConn();
                _clients[i].InitConnection("127.0.0.1", 1734);
                _askCounters[i] = new TurnAskTimeCounter();
                _clients[i].MessageSend += new MessageEventHandler(_askCounters[i].OnMessageSend);
                _clients[i].MessageReceive += new MessageEventHandler(_askCounters[i].OnMessageRecieve);
                InitSend(i);
            }
            _clients[0].SendMessage(CreateCreateGameMessage());
            for (int i = 1; i < clientNo; ++i) {
                _clients[i].SendMessage(CreateJoinMessage());
            }
            Thread.Sleep(1000);
            for (int i = 0; i < clientNo; ++i) {
                _clients[i].SendMessage(MessageFactory.Create(MessageType.StartGame));
            }
            for (int i = 0; i < clientNo; ++i) {
                ClientSendingThread cst = new ClientSendingThread(_clients[i]);
                Thread th = new Thread(new ThreadStart(cst.ThreadProcess));
                th.Start();
            }
           
            
            
        }
        public static void InitSend(int i) {
                _clients[i].SendMessage(CreateLoginMessage(i));
                _clients[i].SendMessage(MessageFactory.Create(MessageType.ChatEntry));
                _clients[i].SendMessage(MessageFactory.Create(MessageType.ChooseGameEntry));   
        }

        public static Message CreateLoginMessage(int i) {
            LoginMessage lm = MessageFactory.Create(MessageType.Login) as LoginMessage;
            lm.Login=i.ToString();
            lm.Password = i.ToString();
            return lm;
        }

        public static Message CreateCreateGameMessage() {
            GameInfoMessage gim = MessageFactory.Create(MessageType.CreateGame) as GameInfoMessage;
            GameInfo _gameInfo = new GameInfo();
            _gameInfo.Name = _gameName;
            _gameInfo.MapName = "mala.map";
            _gameInfo.MaxPlayerNumber = (short)clientNo;
            _gameInfo.GameType = GameType.Public;
            gim.GameInfo = _gameInfo;
            return gim;
        }

        public static Message CreateJoinMessage() {
            TextMessage joinGameMsg = MessageFactory.Create(MessageType.JoinGame) as TextMessage;
            joinGameMsg.Text = _gameName;
            return joinGameMsg;
            
        }
    }
}
