using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Messaging.Common;
using Yad.Net.Common;

namespace Yad.Net.Messaging.Common
{
    public class GameInfoMessage : Message
    {
        GameInfo _gi;

        public GameInfoMessage(MessageType msgType)
            : this(msgType, null)
        {
        }

        public GameInfoMessage(MessageType msgType, GameInfo gi)
            : base(msgType)
        {
            _gi = gi;
        }

        public GameInfo GameInfo
        {
            get
            { return _gi; }
            set
            { _gi = value; }
        }

        public override void Serialize(System.IO.BinaryWriter writer)
        {
            base.Serialize(writer);
            this.WriteString(_gi.Name, writer);
            this.WriteString(_gi.MapName, writer);
            writer.Write(_gi.MaxPlayerNumber);
            writer.Write((byte)_gi.GameType);
        }

        public override void Deserialize(System.IO.BinaryReader reader)
        {
            base.Deserialize(reader);
            if (null == _gi)
                _gi = new GameInfo();
            _gi.Name = this.ReadString(reader);
            _gi.MapName = this.ReadString(reader);
            _gi.MaxPlayerNumber = reader.ReadInt16();
            _gi.GameType = (GameType)reader.ReadByte();
        }
    }
}
