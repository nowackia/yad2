using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace Yad.Log.Common {

    public class InfoLog2 {

        #region Pola prywatne

        /// <summary>
        /// Nazwa pliku logu
        /// </summary>
        public const string ErrorLogFilename = "ErrorLog2.txt";

        /// <summary>
        /// Procent maksymalnej dlugosci loga, jaki pozostanie bo redukcji jego dlugosci
        /// </summary>
        private const double cutPercent = 0.75;

        /// <summary>
        /// Pakiet danych transportowany przy przesuwaniu pliku w metodzie
        /// TruncFileBeginnig
        /// </summary>
        private const int dataPackSize = 8096;

        /// <summary>
        /// Maksymalna wielkosc pliku
        /// </summary>
        private const long maxFilesize = 10485760L;

        /// <summary>
        /// Singleton logu
        /// </summary>
        private static InfoLog2 _infoLog = null;

        /// <summary>
        /// Strumien do zapisu
        /// </summary>
        private static MultiStream _writer = null;

        /// <summary>
        /// Zarzadzanie prefiksami
        /// </summary>
        private static InfoLogPrefix _infoLogPrefix = null;

        /// <summary>
        /// Czy jest aktywny
        /// </summary>
        private bool _isEnabled = true;

        #endregion

        #region Konstruktory

        private InfoLog2(MultiStream writer) {
            _writer = writer;
            _infoLogPrefix = new InfoLogPrefix();

            foreach (Object obj in Enum.GetValues(typeof(EPrefix)))
                _infoLogPrefix.AddFilter((EPrefix)obj);
            //_infoLogPrefix.RemoveFilter(EPrefix.ServerSendMessageInfo);
            //_infoLogPrefix.RemoveFilter(EPrefix.SimulationInfo);
            //_infoLogPrefix.AddFilter(EPrefix.MessageReceivedInfo);
            //_infoLogPrefix.AddFilter(EPrefix.GameMessageProccesing);
            _infoLogPrefix.RemoveFilter(EPrefix.BMan);

            /*_infoLogPrefix.RemoveFilter(EPrefix.GObj);
            _infoLogPrefix.RemoveFilter(EPrefix.AStar);
            _infoLogPrefix.RemoveFilter(EPrefix.Move);
            _infoLogPrefix.RemoveFilter(EPrefix.AI);
            _infoLogPrefix.AddFilter(EPrefix.ServerSendMessageInfo);
            _infoLogPrefix.AddFilter(EPrefix.MessageReceivedInfo);
            _infoLogPrefix.AddFilter(EPrefix.GameMessageProccesing);
            _infoLogPrefix.AddFilter(EPrefix.SimulationInfo);
            _infoLogPrefix.AddFilter(EPrefix.AudioEngine);
            _infoLogPrefix.AddFilter(EPrefix.ClientInformation);
            _infoLogPrefix.AddFilter(EPrefix.ClientSimulation);
            _infoLogPrefix.AddFilter(EPrefix.GameGraphics);
            _infoLogPrefix.AddFilter(EPrefix.GameLogic);*/
        }

        #endregion

        #region Atrybuty

        /// <summary>
        /// Instancja inflogu
        /// </summary>
        public static InfoLog2 Instance {
            get {
                if (null == _infoLog) {
                    _infoLog = new InfoLog2(GetWriter());
                    return _infoLog;
                }
                return _infoLog;
            }
        }

        #endregion

        #region Metody prywatne

        private static MultiStream GetWriter() {
            MultiStream writer = null;
            writer = new MultiStream(ErrorLogFilename);
            return writer;
        }

        private void WriteIns(string s) {
            _writer.WriteLine(s);
        }

        private void WriteSingleExceptionIns(Exception ex) {
            if (_isEnabled) {
                _writer.WriteLine("Message: " + (ex.Message == null ? "null" : ex.Message));
                _writer.WriteLine("Stack:");
                _writer.WriteLine(ex.StackTrace == null ? "null" : ex.StackTrace);
            }
        }

        private void WriteExceptionIns(Exception ex) {
            if (_isEnabled) {
                _writer.WriteLine("-- EXCEPTION ---" + DateTime.Now.ToString() + "--------------");
                WriteSingleExceptionIns(ex);
                if (ex.InnerException != null) {
                    _writer.WriteLine("---- InnerException: " + ex.InnerException.ToString());
                    WriteSingleExceptionIns(ex.InnerException);
                }
                _writer.WriteLine("------------------------------------------------------------");
            }
        }

        private void WriteErrorIns(string s) {
            if (_isEnabled) {
                _writer.WriteLine("-- ERROR ---" + DateTime.Now.ToString() + "-----------------");
                _writer.WriteLine("Message: " + s);
                _writer.WriteLine("------------------------------------------------------------");
            }
        }

        private void WriteErrorIns(string s, EPrefix prefix) {
            if (_isEnabled) {
                if (!_infoLogPrefix.IsFiltred(prefix)) {
                    _writer.WriteLine("-- ERROR ---" + DateTime.Now.ToString() + "-----------------");
                    s = _infoLogPrefix.AddFilterString(s, prefix);
                    _writer.WriteLine("Message: " + s);
                    _writer.WriteLine("------------------------------------------------------------");
                }
            }
        }

        private void WriteInfoIns(string s) {
            if (_isEnabled) {
                _writer.WriteLine("#I:# " + DateTime.Now.ToString() + "  " + s);
            }
        }

        private void WriteInfoIns(string s, EPrefix prefix) {
            if (_isEnabled) {
                if (!_infoLogPrefix.IsFiltred(prefix)) {
                    s = _infoLogPrefix.AddFilterString(s, prefix);
                    _writer.WriteLine("#I:# " + DateTime.Now.ToString() + "  " + s);
                }
            }
        }

        private void CloseIns() {
            _writer.Close();
            TruncFileBeginning();
        }

        private void DisableIns() {
            _isEnabled = false;
        }

        private void EnableIns() {
            _isEnabled = true;
        }

        private static void TruncFileBeginning() {
            FileStream fs = null;
            try {
                fs = new FileStream(ErrorLogFilename, FileMode.Open);
            }
            catch (Exception) {
                return;
            }
            if (fs.Length <= maxFilesize) {
                try {
                    fs.Close();
                }
                catch (Exception) {
                }
                return;
            }
            int cut_filesize = (int)(maxFilesize * cutPercent);
            int toCut = (int)(fs.Length - cut_filesize);
            int times = cut_filesize / dataPackSize;
            int rest = cut_filesize % dataPackSize;
            int position = toCut;
            byte[] data = new byte[dataPackSize];
            int i = 0;
            try {
                for (i = 0; i < times; ++i, position += dataPackSize) {
                    fs.Seek(position, SeekOrigin.Begin);
                    fs.Read(data, 0, dataPackSize);
                    fs.Seek(i * dataPackSize, SeekOrigin.Begin);
                    fs.Write(data, 0, dataPackSize);
                }
                if (rest > 0) {
                    fs.Seek(position, SeekOrigin.Begin);
                    fs.Read(data, 0, rest);
                    fs.Seek(i * dataPackSize, SeekOrigin.Begin);
                    fs.Write(data, 0, rest);
                }
            }
            catch (Exception) {
            }
            finally {
                try {
                    fs.SetLength(cut_filesize);
                    fs.Close();
                }
                catch (Exception) {
                }
            }
        }

        #endregion

        #region Metody publiczne

        public OnWriteLineDelegate OnWriteLine {
            get {
                if (_writer != null)
                    return _writer.OnWriteLine;
                return null;
            }
            set {
                if (_writer != null)
                    _writer.OnWriteLine = value;
            }

        }

        public static void WriteStart() {
            InfoLog2.Write("____________________________________________");
            InfoLog2.Write("Application BEGIN " +
                Assembly.GetExecutingAssembly().GetName().ToString() + " : "
                + Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

        public static void WriteEnd() {
            InfoLog2.Write("Application END");
            InfoLog2.Write("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
        }

        public static void Write(string s) {
            Instance.WriteIns(s);
        }

        public static void WriteException(Exception ex) {
            Instance.WriteExceptionIns(ex);
        }

        public static void WriteError(string s, EPrefix prefix) {
            Instance.WriteErrorIns(s, prefix);
        }

        public static void WriteError(string s) {
            Instance.WriteErrorIns(s);
        }

        public static void WriteInfo(string s, EPrefix prefix) {
            Instance.WriteInfoIns(s, prefix);
        }

        public static void WriteInfo(string s) {
            Instance.WriteInfoIns(s);
        }

        public static void Enable() {
            Instance.EnableIns();
        }

        public static void Disable() {
            Instance.DisableIns();
        }
        public static void Close() {
            Instance.CloseIns();
        }

        #endregion
    }

}