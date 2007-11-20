using System;
using System.Collections.Generic;
using System.Text;
using Yad.Log;
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;
using Yad.Log.Common;
using Yad.Database.Server;
using Yad.Properties.Server;

namespace Yad.Net.Server
{
    class ServerMain
    {
        #region Pola prywatne

        private Thread _serverThread = null;
        private Server _server;
        private const int _ServerPortNo = 1734;

        #endregion

        #region Konstruktory

        public ServerMain()
        {
            _serverThread = new Thread(new ThreadStart(ServerThread));
            _serverThread.Start();
        }

        #endregion

        #region Metody publiczne

        public void Stop()
        {
            InfoLog.Instance.OnWriteLine = null;
            _server.Stop();
            //_serverThread.Interrupt();
            _serverThread.Join();
        }

        #endregion

        #region Metody prywatne

        private void ServerThread()
        {
            InfoLog.WriteInfo("Server loop starts...", EPrefix.Initialization);
            ServerProcess();
            InfoLog.WriteInfo("Server loop ends...", EPrefix.Finalization);
        }

        private void ServerProcess()
        {
            if (Settings.Default.DBAvail) {
                switch (YadDB.Init()) {
                    case InitDBResult.Successful:
                        InfoLog.WriteInfo("Database initialized successfully...", EPrefix.DatebaseInfo);
                        _server = new Server(_ServerPortNo);
                        _server.Start();
                        break;
                    case InitDBResult.CreateMDBFileFailed:
                        InfoLog.WriteInfo("Unable to create .mdb file...", EPrefix.DatebaseInfo);
                        break;
                    case InitDBResult.CreateDBFailed:
                        InfoLog.WriteInfo("Unablie to create database...", EPrefix.DatebaseInfo);
                        break;
                }
            }
            else {
                _server = new Server(_ServerPortNo);
                _server.Start();
            }
        }

        #endregion
    }
}
