using System;
using System.Collections.Generic;
using System.Text;
using Yad.Net.Common;

namespace Yad.Net.Messaging.Common
{
    public class GameInitMessage : ControlMessage
    {
        //Lista pozycji, posortowana po id gracza
        private PositionData[] _posData = null;

        public GameInitMessage()
            : base(MessageType.GameInit)
        { }

        public PositionData[] PositionData
        {
            get { return _posData; }
            set { _posData = value; }
        }

        public override void Serialize(System.IO.BinaryWriter writer) {
            base.Serialize(writer);
            if (_posData == null || _posData.Length == 0) {
                writer.Write((int)0);
                return;
            }
            for (int i = 0; i < _posData.Length; ++i) {
                writer.Write(_posData[i].PlayerId);
                writer.Write(_posData[i].X);
                writer.Write(_posData[i].Y);
            }
        }

        public override void Deserialize(System.IO.BinaryReader reader) {
            base.Deserialize(reader);
            int count = reader.ReadInt32();
            if (count == 0) {
                _posData = null;
                return;
            }
            _posData = new PositionData[count];
            short id = -1;
            int x = -1, y = -1;

            for (int i = 0; i < count; ++i) {
                id = reader.ReadInt16();
                x = reader.ReadInt32();
                y = reader.ReadInt32();
                PositionData pd = new PositionData(id, x, y);
                _posData[i] = pd;
            }
        }
    }
}
