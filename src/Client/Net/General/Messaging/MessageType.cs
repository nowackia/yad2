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
        ChooseGameEntry,
        JoinGameEntry,
        JoinGameExit,
        Logout,
        GameCreate,
        #endregion

        #region Unknown messages
        Unknown
        #endregion
    }
}
