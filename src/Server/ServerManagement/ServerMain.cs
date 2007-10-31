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

        private BackgroundWorker _worker = null;

        #endregion

        #region Konstruktory

        public ServerMain()
        {
            _worker = new BackgroundWorker();
            _worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            _worker.WorkerSupportsCancellation = true;
            _worker.RunWorkerAsync();
        }

        #endregion

        #region Metody publiczne

        public void Stop()
        {
            _worker.CancelAsync();
        }

        #endregion

        #region Metody prywatne

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            InfoLog.WriteInfo("Pocz¹tek pêtli serwera...", EPrefix.Initialization);
            while (!_worker.CancellationPending)
                ServerProcess();
            e.Cancel = true;
        }

        private void ServerProcess()
        {
            Thread.Sleep(1000);
        }

        #endregion
    }
}
