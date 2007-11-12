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
        /* Depreciated */ PlayerInfoSuccessful,
        /* Depreciated */ PlayerInfoUnsuccessful,
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
        /* Depreciated */ CreateGameSuccessful,
        /* Depreciated */ CreateGameUnsuccessful,
        CreateGameResult,
        #endregion

        #region Game Joining
        Players,
        NewPlayer,
        DeletePlayer,
        UpdatePlayer,
        /* Depreciated */ StartGameSuccessful,
        /* Depreciated */ StartGameUnsuccessful,
        StartGameResult,
        JoinGameEntry,
        /* Depreciated */ JoinGameSuccessful,
        /* Depreciated */ JoinGameUnsuccessful,
        JoinGameResult,
        GameParams,
        StartGame,
        #endregion

        #region Client login messages
        Login,
        /* Depreciated */ LoginSuccessful,
        /* Depreciated */ LoginUnsuccessful,
        LoginResult,
        Register,
        /* Depreciated */ RegisterSuccessful,
        /* Depreciated */ RegisterUnsuccessful,
        RegisterResult,
        Remind,
        /* Depreciated */ RemindSuccessful,
        /* Depreciated */ RemindUnsuccessful,
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
