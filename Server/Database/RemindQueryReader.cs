using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Database {
    class RemindQueryReader : IQueryReader{

        private string _email;
        private string _password;
        private bool _result = true;

        public bool Result {
            get { return _result; }
            set { _result = value; }
        }

        public string Email {
            get { return _email; }
            set { _email = value; }
        }

        public string Password {
            get { return _password; }
            set { _password = value; }
        }

        #region IQueryReader Members

        public void ReadData(System.Data.IDataReader reader) {
            try {
                if (reader.Read()) {
                    _password = reader.GetString(0);
                    _email = reader.GetString(1);
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
    }
}
