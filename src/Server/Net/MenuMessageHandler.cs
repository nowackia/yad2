using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Yad.Net.Common;
using Yad.Net.Messaging.Common;
using Yad.Log;
using Yad.Log.Common;
using Yad.Database.Server;
using Yad.Properties;
using Yad.Properties.Server;
using Yad.Mail;

namespace Yad.Net.Server
{
    class MenuMessageHandler : MessageHandler {

        #region Private Members

        private Server _server;
        private const string ProcessStringFormat = "Processing {0} for player: {1}...";
        #endregion 

        #region Constructors

        public MenuMessageHandler(Server server)
            : base() {
            _server = server;
        }

        #endregion

        #region Message processing

        /// <summary>
        /// Main processing function - processes all menu messages from clients
        /// </summary>
        /// <param name="msg">Message to process. It must constains userid of the user who
        /// send it.</param>
        public override void ProcessItem(Message msg) {
            InfoLog.WriteInfo(msg.Type.ToString(), EPrefix.MessageReceivedInfo);
            switch (msg.Type) {
                case MessageType.Register:
                    ProcessRegister((RegisterMessage)msg);
                    break;
                case MessageType.Login:
                    ProcessLogin((LoginMessage)msg);
                    break;
                case MessageType.Entry:
                    ProcessEntry(msg);
                    break;
                case MessageType.ChatText:
                    ProcessChatText((TextMessage)msg);
                    break;
                case MessageType.CreateGame:
                    ProcessCreateGame((GameInfoMessage)msg);
                    break;
                case MessageType.JoinGame:
                    ProcessJoinGame((TextMessage)msg);
                    break;
                case MessageType.Players:
                    ProcessPlayers((PlayersMessage)msg);
                    break;
                case MessageType.StartGame:
                    ProcessStartGame(msg);
                    break;
                case MessageType.PlayerInfo:
                    ProcessPlayerInfoMessage((NumericMessage)msg);
                    break;
                case MessageType.Remind:
                    ProcessRemindMessage((TextMessage)msg);
                    break;
            
            }

        }

        private void ProcessRemindMessage(TextMessage textMessage) {
            InfoLog.WriteInfo("Processing remind message", EPrefix.ServerProcessInfo);
            string login = textMessage.Text;
            string email = null;
            string password = null;
            if (Settings.Default.DBAvail)
                if (YadDB.Remind(login, out email, out password)) {
                    YadMail.SendRemindMail(login, email, password);
                    ResultMessage resMsg = Utils.CreateResultMessage(ResponseType.Remind, ResultType.Successful);
                    SendMessage(resMsg, textMessage.SenderId);
                    return;
                }
           SendMessage(Utils.CreateResultMessage(ResponseType.Remind, ResultType.Unsuccesful), textMessage.SenderId);
        }

        private void ProcessPlayerInfoMessage(NumericMessage numericMessage) {
            InfoLog.WriteInfo("Processing player info message", EPrefix.ServerProcessInfo);
            PlayerInfoMessage pimsg = (PlayerInfoMessage)MessageFactory.Create(MessageType.PlayerInfoResponse);
            Player p = _server.GetPlayer((short)numericMessage.Number);
            if (p == null)
                pimsg.PlayerData = null;
            else
                pimsg.PlayerData = (PlayerData)p.PlayerData.Clone();
            SendMessage(pimsg, numericMessage.SenderId);
        }

       
        #endregion 

        #region Private Methods

        private void ProcessRegister(RegisterMessage msg) {
            if (Settings.Default.DBAvail) {
                InfoLog.WriteInfo(string.Format(ProcessStringFormat, "Register", msg.SenderId), EPrefix.ServerProcessInfo);
                if (YadDB.Register(msg.Login, msg.Password, msg.Mail)) {
                    InfoLog.WriteInfo("Player registered successfully", EPrefix.ServerProcessInfo);
                    SendMessage(Utils.CreateResultMessage(ResponseType.Register, ResultType.Successful), msg.SenderId);
                }
                InfoLog.WriteInfo("Player registered unsucessfully", EPrefix.ServerProcessInfo);
            }
            else
                SendMessage(Utils.CreateResultMessage(ResponseType.Register, ResultType.Successful), msg.SenderId);
        }

        /// <summary>
        /// Process information about clicked button "start game"
        /// </summary>
        /// <param name="msg"></param>
        private void ProcessStartGame(Message msg) {
            InfoLog.WriteInfo(string.Format(ProcessStringFormat, "Start Game", msg.SenderId), EPrefix.ServerProcessInfo);
            _server.GameManager.StartGame(msg.SenderId);
        }

        /// <summary>
        /// Process plater data modification
        /// </summary>
        /// <param name="playersMessage"></param>
        private void ProcessPlayers(PlayersMessage playersMessage) {
            Player player = _server.GetPlayer(playersMessage.SenderId);
            InfoLog.WriteInfo(string.Format(ProcessStringFormat, "Start Game", player.Login), EPrefix.ServerProcessInfo);
            if (player == null || player.State != MenuState.GameJoin)
                return;
            _server.GameManager.ModifyPlayer(player.Id, playersMessage.PlayerList[0]);
        }

        /// <summary>
        /// Process join game
        /// </summary>
        /// <param name="textMessage"></param>
        private void ProcessJoinGame(TextMessage textMessage) {
            Player player = _server.GetPlayer(textMessage.SenderId);
            InfoLog.WriteInfo(string.Format(ProcessStringFormat, "Join Game", textMessage.SenderId), EPrefix.ServerProcessInfo);
            if (player == null)
                return;
            MenuState state = PlayerStateMachine.Transform(player.State, MenuAction.GameJoinEntry);
            if (state == MenuState.Invalid)
                SendMessage(Utils.CreateResultMessage(ResponseType.JoinGame, ResultType.Unsuccesful), player.Id);

            ResultType result = _server.GameManager.IsJoinGamePossible(textMessage.Text, _server.GetPlayer(textMessage.SenderId));
            
            if (result == ResultType.Successful) {
                SendMessage(Utils.CreateResultMessage(ResponseType.JoinGame, result), player.Id);
                _server.GameManager.JoinGame(textMessage.Text, player);
                player.State = state;
            }
            else
                SendMessage(Utils.CreateResultMessage(ResponseType.JoinGame, result), player.Id);
        }

        private void ProcessChatText(TextMessage msg) {
            InfoLog.WriteInfo(string.Format(ProcessStringFormat, "Chat Text", msg.SenderId), EPrefix.ServerProcessInfo);
            _server.Chat.AddTextMessage(msg);
        }

        private void ProcessLogin(LoginMessage msg) {

            //Pobranie gracza z listy niezalogowanych
            Player player = _server.GetPlayerUnlogged(msg.SenderId);
            InfoLog.WriteInfo(string.Format(ProcessStringFormat, "Login", msg.SenderId), EPrefix.ServerProcessInfo);
            //Jesli gracza nie ma, lub jego stan jest jakis dziwny - koniec obslugi
            if (null == player || player.State != MenuState.Unlogged)
                return;

            if (Login(msg.SenderId, msg.Login, msg.Password) || !Settings.Default.DBAvail) {
                MenuState state = PlayerStateMachine.Transform(player.State, MenuAction.Login);
                if (state == MenuState.Invalid) {
                    SendMessage(Utils.CreateResultMessage(ResponseType.Login, ResultType.Unsuccesful), player.Id);
                    return;
                }
                LoggingTransfer(player);
                player.State = state;
                if (!Settings.Default.DBAvail) {
                    PlayerData pd = new PlayerData();
                    pd.Login = msg.Login;
                    pd.LossNo = pd.WinNo = 0;
                    pd.Id = msg.SenderId;
                    player.SetData(pd);
                }
                

            }
            else {
                SendMessage(Utils.CreateResultMessage(ResponseType.Login, ResultType.Unsuccesful), player.Id);
                InfoLog.WriteInfo("Player: " + msg.Login + "failed to login", EPrefix.ServerProcessInfo);
                return;
            }

            SendMessage(Utils.CreateResultMessage(ResponseType.Login, ResultType.Successful), player.Id);
            NumericMessage idmsg = (NumericMessage)MessageFactory.Create(MessageType.IdInformation);
            idmsg.Number = player.Id;
            SendMessage(idmsg, player.Id);
        }

        private void ProcessEntry(Message msg) {
            EntryMessage emsg = msg as EntryMessage;
            InfoLog.WriteInfo(string.Format(ProcessStringFormat, "Entry", msg.SenderId), EPrefix.ServerProcessInfo);
            switch ((ServerRoom)emsg.ServerRoom) {
                case ServerRoom.Chat:
                    ProcessChatEntry(emsg);
                    break;
                case ServerRoom.GameChoose:
                    ProcessGameChooseEntry(emsg);
                    break;
            }
        }

        public void ProcessGameChooseEntry(Message msg) {
            Player player = _server.GetPlayer(msg.SenderId);
            InfoLog.WriteInfo(string.Format(ProcessStringFormat, "Game Choose Entry", player.Login), EPrefix.ServerProcessInfo);
            if (null == player || player.State == MenuState.GameChoose)
                return;

            MenuState state = PlayerStateMachine.Transform(player.State, MenuAction.GameChooseEntry);
            if (state == MenuState.Invalid) {
                InfoLog.WriteInfo("GameChoose entry unsuccesful by player: " + player.Login, EPrefix.ServerInformation);
                return;
            }
            
            //wejscie moze nastapic z czatu albo z game joina
            switch (player.State) {
                case MenuState.Chat:
                    MoveFromChatToGameRoom(player);
                    break;
                case MenuState.GameJoin:
                    RemoveFromGameJoin(player);
                    break;
            }
            lock (player.SyncRoot) {
                player.State = state;
            }
        }

        public void ProcessChatEntry(Message msg) {

            Player player = _server.GetPlayer(msg.SenderId);
            InfoLog.WriteInfo(string.Format(ProcessStringFormat, "Chat entry", player.Login), EPrefix.ServerProcessInfo);
            if (null == player || player.State == MenuState.Chat)
                return;

            MenuState state = PlayerStateMachine.Transform(player.State, MenuAction.ChatEntry);
            if (state == MenuState.Invalid) {
                InfoLog.WriteInfo("Chat entry unsuccesful", EPrefix.ServerInformation);
                return;
            }
            //Mogly zajsc dwie sytuacje - gracz przechodzi ze stanu zalogowany, albo z 
            //game roomu 
            switch (player.State) {
                case MenuState.GameChoose:
                    RemoveFromGameRoom(player);
                break;
            }
            lock (player.SyncRoot) {
                player.State = state;
            }
            AddToChat(player);
        }

        private void ProcessCreateGame(GameInfoMessage msg) {
            Player player = _server.GetPlayer(msg.SenderId);
            InfoLog.WriteInfo(string.Format(ProcessStringFormat, "Create game", msg.SenderId), EPrefix.ServerProcessInfo);
            MenuState state = PlayerStateMachine.Transform(player.State, MenuAction.GameJoinEntry);
            ResultMessage resMsg = Utils.CreateResultMessage(ResponseType.CreateGame, ResultType.Unsuccesful);
            ResultType result = ResultType.Unsuccesful;
            if (state == MenuState.GameJoin) {
                result = CreateGame(msg);
                if (result == ResultType.Successful)
                    lock (player.SyncRoot)
                        player.State = state;
            }
            //resMsg.Result = (byte)result;
            //SendMessage(resMsg, msg.SenderId);
        }

        private ResultType CreateGame(GameInfoMessage msg) {
            ResultType result = _server.GameManager.IsCreateGamePossible(msg.GameInfo);
            SendMessage(Utils.CreateResultMessage(ResponseType.CreateGame, ResultType.Successful), msg.SenderId);
            if (ResultType.Successful == result) {
                _server.GameManager.CreateGameNoError(msg.GameInfo);
                result = _server.GameManager.IsJoinGamePossible(msg.GameInfo.Name, _server.GetPlayer(msg.SenderId));
                if (result == ResultType.Successful)
                    _server.GameManager.JoinGame(msg.GameInfo.Name, _server.GetPlayer(msg.SenderId));
                else
                    InfoLog.WriteError("Join not possible after create!");

            }
            return result;
            /*ResultType resultType = _server.GameManager.CreateGame(msg.GameInfo);
            if (resultType == ResultType.Successful) {
                ResultType joinResult = _server.GameManager.JoinGame(msg.GameInfo.Name, _server.GetPlayer(msg.SenderId));
                if (joinResult != ResultType.Successful) {
                    InfoLog.WriteError("Join not possible after create!");
                    return joinResult;
                }
            }
            return resultType;*/
        }

        #endregion

        public void AddToChat(Player player) {
            _server.Chat.AddPlayer(player);
        }

        public void RemoveFromGameRoom(Player player) {
            _server.GameManager.RemovePlayer(player.Id);
        }

        private void LoggingTransfer(Player player) {
            _server.RemovePlayerUnlogged(player.Id);
            _server.AddPlayer(player.Id, player);
        }

        private void RemoveFromGameJoin(Player player) {
            _server.GameManager.RemoveFromGameJoin(player);
        }

        private void MoveFromChatToGameRoom(Player player) {
            _server.Chat.RemovePlayer(player);
            _server.GameManager.AddPlayer(player);
        }

        public bool Login(short id, string login, string password) {

            ushort winno = 0;
            ushort lossno = 0;

            if (YadDB.Login(login, password, ref winno, ref lossno)) {
                Player player = _server.GetPlayer(id);
                PlayerData pd = new PlayerData();
                pd.Id = id;
                pd.Login = login;
                pd.LossNo = lossno;
                pd.WinNo = winno;
                player.SetData(pd);
                return true;
            }
            return false;
            
        }

        public PlayerData LoadPlayerData(string login) {
            PlayerData pd = new PlayerData();
            pd.Login = login;
            pd.LossNo = 1;
            pd.WinNo = 1;
            return pd;
        }
    }
}
