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

        ChatUsers,
        DeleteChatUser,
        NewChatUser,
        ChatText,
        ChatEntry,
        ChatExit,

        #endregion

        #region Client login messages
        Login,
        Register,
        Remind,
        #endregion

        #region Client menu messages
        ChooseGameEntry,
        JoinGameEntry,
        JoinGameExit,
        Logout,
        GameCreate,
        #endregion

        LoginSuccessful,
        LoginUnsuccessful,

        #region Unknown messages
        Unknown
        #endregion
    }
}
