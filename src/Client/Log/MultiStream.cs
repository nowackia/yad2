using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Client.Log
{
    public delegate void OnWriteLineDelegate(string s);
    class MultiStream : IDisposable
    {
        StreamWriter _writer = null;
        OnWriteLineDelegate _onWriteLine = null;

        public OnWriteLineDelegate OnWriteLine
        {
            get { return _onWriteLine; }
            set { _onWriteLine = value; }
        }
        
        public MultiStream(string filepath)
        {
            _writer = new StreamWriter(filepath, true);
        }

        public void WriteLine(string s)
        {
            if (_onWriteLine != null)
                _onWriteLine(s);
            _writer.WriteLine(s);
            _writer.Flush();
        }

        public void Close()
        {
            _writer.Close();
        }

        #region IDisposable Members

        public void Dispose()
        {
            _writer.Close();
            _writer.Dispose();
        }

        #endregion
    }
}
