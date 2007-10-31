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
        public void Stop()
        {
            _worker.CancelAsync();
        }

        BackgroundWorker _worker = new BackgroundWorker();
        public ServerMain()
        {
            _worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            _worker.WorkerSupportsCancellation = true;
            _worker.RunWorkerAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!_worker.CancellationPending)
            {
                Thread.Sleep(2000);
                InfoLog.Write("Raz");
            }
            e.Cancel = true;
        }

        
        public bool Process()
        {
            while (!_worker.CancellationPending)
            {
                //Thread.Sleep(2000);
                InfoLog.Write("Raz");
            }
            return true;
        }
    }
}
