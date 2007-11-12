using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.Messaging.Common {
    public enum ResponseType : byte {
        Login = 0,
        Register,
        Remind,
        CreateGame,
        JoinGame
    }
    public enum ResultType : byte {
        Successful = 0,
        Unsuccesful,

        #region CreateGameMessage
        NameExistsError,
        InvalidPlayerNoError,
        InvalidMapIdNoError,
        MaxServerGameError,
        #endregion

        #region JoinGameMessage
        GameNotExists,
        GameStarted,
        GameFull,
        #endregion
    }
}
