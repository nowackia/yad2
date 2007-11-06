using System;
using System.Collections.Generic;
using System.Text;
using Client.Log;
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;

namespace Server.ServerManagement
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
            _serverThread.Interrupt();
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
            _server = new Server(_ServerPortNo);
            _server.Start();
        }

        #endregion
    }
}
