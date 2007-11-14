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
        Result,
        Entry,
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
        /* For MessageFactory Only */ ChatEntry,
        ChatUsers,
        DeleteChatUser,
        NewChatUser,
        ChatText,
        #endregion

        #region Player Info
        PlayerInfo,
        [Obsolete("Use PlayerInfoResponse instead")]
        PlayerInfoSuccessful,
        [Obsolete("Use PlayerInfoResponse instead")]
        PlayerInfoUnsuccessful,
        PlayerInfoResponse,
        #endregion

        #region Game Choosing
        /* For MessageFactory Only */ ChooseGameEntry,
        Games,
        NewGame,
        DeleteGame,
        #endregion

        #region Game Creating
        CreateGame,
        [Obsolete("Use CreateGameResult instead")]
        CreateGameSuccessful,
        [Obsolete("Use CreateGameResult instead")]
        CreateGameUnsuccessful,
        CreateGameResult,
        #endregion

        #region Game Joining
        Players,
        NewPlayer,
        DeletePlayer,
        UpdatePlayer,
        GameParams,
        JoinGame,
        [Obsolete("Use JoinGameResult instead")]
        JoinGameSuccessful,
        [Obsolete("Use JoinGameResult instead")]
        JoinGameUnsuccessful,
        JoinGameResult,
        StartGame,
        [Obsolete("Use StartGameResult instead")]
        StartGameSuccessful,
        [Obsolete("Use StartGameResult instead")]
        StartGameUnsuccessful,

        #endregion

        #region Client login messages
        Login,
        [Obsolete("Use LoginResult instead")]
        LoginSuccessful,
        [Obsolete("Use LoginResult instead")]
        LoginUnsuccessful,
        LoginResult,
        Register,
        [Obsolete("Use RegisterResult instead")]
        RegisterSuccessful,
        [Obsolete("Use RegisterResult instead")]
        RegisterUnsuccessful,
        RegisterResult,
        Remind,
        [Obsolete("Use RemindResult instead")]
        RemindSuccessful,
        [Obsolete("Use RemindResult instead")]
        RemindUnsuccessful,
        RemindResult,
        #endregion

        #region Client menu messages
        Logout,
        #endregion

        #region Unknown messages
        Unknown
        #endregion
    }
}
