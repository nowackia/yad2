using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.General.Messaging
{
    public enum MessageType : byte
    {
        #region General messages
        Numeric = 0,
        Text,
        #endregion

        GameInit,
        Move,
        Destroy,
        CreateUnit,
        Build,
        Harvest,
        Attack,
        Control,

        #region Chat

        //wysyla aktualnych uzytkownikow czatu
        ChatUsers,
        //kasuje uzytkownika
        DeleteChatUser,
        //nowy uzytkownik
        NewChatUser,
        //tresc 
        ChatText,
   
        ChatEntry,
        ChatExit,

        #endregion

        #region Game Choosing

        /// <summary>
        /// Message is sent when player enters join game room
        /// </summary>
        ChooseGameEntry,

        /// <summary>
        /// Server response with public games list
        /// </summary>
        GamesList,

        /// <summary>
        /// Message with new public game data
        /// </summary>
        NewGame,

        /// <summary>
        /// Message with the information of deletion of public game 
        /// </summary>
        DeleteGame,

        #endregion

        #region GameCreating

        CreateGameMessage, 
        CreateGameSuccessful,
        CreateGameUnsuccessful,

        #endregion

        #region Game Joining

        PlayersList,
        NewPlayer,
        DeletePlayer,
        UpdatePlayer,
        StartGame,
        StartGameSuccesful,
        StartGameUnsuccessful,
        JoinGameEntry,
        JoinGameExit,
        JoinGameSuccessful,
        JoinGameUnsuccessful,
        GameParams,

        #endregion

        #region Client login messages
        Login,
        LoginSuccessful,
        LoginUnsuccessful,
        Register,
        RegisterSuccessful,
        RegisterUnsuccessful,
        Remind,
        RemindSuccessful,
        RemindUnsuccessful,
        #endregion

        #region Client menu messages
        //ChooseGameEntry,
        //JoinGameEntry,
        //JoinGameExit,
        Logout,
        GameCreate,
        #endregion

        #region Unknown messages
        Unknown
        #endregion
    }
}
