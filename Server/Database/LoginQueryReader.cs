using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Database {

    class LoginQueryReader : IQueryReader, IDisposable {
        ushort _winno = 0;
        bool _result = true;
        ushort _lossno = 0;

        public ushort Winno {
            get { return _winno; }
            set { _winno = value; }
        }

        public ushort Lossno {
            get { return _lossno; }
            set { _lossno = value; }
        }


        public bool Result {
            get { return _result; }
            set { _result = value; }
        }

        #region IQueryReader Members

        public void ReadData(System.Data.IDataReader reader) {
            try {
                if (reader.Read()) {
                    _winno = (ushort)reader.GetInt32(0);
                    _lossno = (ushort)reader.GetInt32(1);
                }
                else
                    SetFailure();
            }
            catch (Exception) {
                SetFailure();
            }
        }

        public void SetFailure() {
            _result = false;
        }

        #endregion

        #region IDisposable Members

        public void Dispose() {
            
        }

        #endregion
    }
}
