using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.Messaging.Common
{
    public enum MessageType : byte
    {
        #region General messages
        Numeric = 0,
        Text,
        #endregion

        #region Game messages
        GameInit,
        Move,
        Destroy,
        CreateUnit,
        Build,
        Harvest,
        Attack,
        Control,
        #endregion

        #region Chat

        ChatEntry,
        //wysyla aktualnych uzytkownikow czatu
        ChatUsers,
        //kasuje uzytkownika
        DeleteChatUser,
        //nowy uzytkownik
        NewChatUser,
        //tresc 
        ChatText,

        PlayerInfo,
        PlayerInfoSuccessful,
        PlayerInfoUnsuccessful,

        #endregion

        #region Game Choosing
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

        #region Game Creating
        CreateGame, 
        CreateGameSuccessful,
        CreateGameUnsuccessful,
        #endregion

        #region Game Joining

        PlayersList,
        NewPlayer,
        DeletePlayer,
        UpdatePlayer,
        StartGame,
        StartGameSuccessful,
        StartGameUnsuccessful,
        JoinGameEntry,
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
        Logout,
        #endregion

        #region Unknown messages
        Unknown
        #endregion
    }
}
