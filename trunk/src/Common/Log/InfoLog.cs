using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace Yad.Log.Common {

    public class InfoLog {

        #region Pola prywatne

        /// <summary>
        /// Zarzadzanie prefiksami
        /// </summary>
        private static InfoLogPrefix _globalInfoLogPrefix = null;

        /// <summary>
        /// Czy jest aktywny
        /// </summary>
        private bool _isEnabled = true;

        /// <summary>
        /// Slownik plikow logowania
        /// </summary>
        private Dictionary<LogFiles, LogFile> _logFiles = null;

        /// <summary>
        /// Domyslny log
        /// </summary>
        private LogFile _defaultLog = new LogFile(LogFiles.DefaultInfoLog);

        /// <summary>
        /// Przekierowania
        /// </summary>
        private Dictionary<EPrefix, LogFiles> _fileRedirection = null;

        private static InfoLog _infoLog = null;

        #endregion

        #region Constructors

        private InfoLog() {
            _globalInfoLogPrefix = new InfoLogPrefix();
            _logFiles = new Dictionary<LogFiles, LogFile>();
            _logFiles.Add(LogFiles.DefaultInfoLog, _defaultLog);
            _fileRedirection = new Dictionary<EPrefix, LogFiles>();
        }


        #endregion

        #region Atrybuty

        /// <summary>
        /// Instancja infologu
        /// </summary>
        public static InfoLog Instance {
            get {
                if (null == _infoLog) {
                    _infoLog = new InfoLog();
                    return _infoLog;
                }
                return _infoLog;
            }
        }

        public LogFile this[LogFiles lf] {
            get {
                if (!_logFiles.ContainsKey(lf))
                    _logFiles.Add(lf, new LogFile(lf));
                return _logFiles[lf];
            }
        }

        public void AddRedirection(EPrefix prefix, LogFiles logFiles) {
            _fileRedirection.Add(prefix, logFiles);
        }

        public void RemoveRedirection(EPrefix prefix, LogFile logFiles) {
            _fileRedirection.Remove(prefix);
        }

        public void InitializeLogFile(LogFiles logFiles) {
            if (!_logFiles.ContainsKey(logFiles))
                _logFiles.Add(logFiles, new LogFile(logFiles));
        }

        #endregion

        #region Metody prywatne

        private void WriteAllIns(string s) {
            if (_isEnabled) {
                foreach (LogFile lg in _logFiles.Values)
                    lg.Write(s);
            }
        }
        private void WriteIns(string s) {
            if (_isEnabled) {
                _defaultLog.Write(s);
            }
        }

        private void WriteIns(string s, LogFile logf) {
            if (_isEnabled) {
                logf.Write(s);
            }
        }

        private void WriteSingleExceptionIns(Exception ex) {
            WriteSingleExceptionIns(ex, _defaultLog);
        }

        private void WriteSingleExceptionIns(Exception ex, LogFile lf) {
            if (_isEnabled) {
                lf.Write("Message: " + (ex.Message == null ? "null" : ex.Message));
                lf.Write("Stack:");
                lf.Write(ex.StackTrace == null ? "null" : ex.StackTrace);
            }
        }

        private void WriteExceptionIns(Exception ex) {
            WriteExceptionIns(ex, _defaultLog);
        }

        private void WriteExceptionIns(Exception ex, LogFile lf) {
            if (_isEnabled) {
                lf.Write("-- EXCEPTION ---" + DateTime.Now.ToString() + "--------------");
                WriteSingleExceptionIns(ex,lf);
                if (ex.InnerException != null) {
                    lf.Write("---- InnerException: " + ex.InnerException.ToString());
                    WriteSingleExceptionIns(ex.InnerException,lf);
                }
                lf.Write("------------------------------------------------------------");
            }
        }

        private void WriteErrorIns(string s) {
            WriteErrorIns(s, _defaultLog);
        }

        private void WriteErrorIns(string s, LogFile lf) {
            if (_isEnabled) {
                lf.Write("-- ERROR ---" + DateTime.Now.ToString() + "-----------------");
                lf.Write("Message: " + s);
                lf.Write("------------------------------------------------------------");
            }
        }

        private void WriteErrorIns(string s, EPrefix prefix, LogFile lf) {
            if (_isEnabled) {
                if (!_globalInfoLogPrefix.IsFiltred(prefix)) {
                    LogFile toWrite = GetLogFileFromArgs(prefix, lf);
                    toWrite.Write("-- ERROR ---" + DateTime.Now.ToString() + "-----------------", prefix);
                    toWrite.Write(s, prefix);
                    toWrite.Write("------------------------------------------------------------", prefix);
                }
            }
        }

        private LogFile GetLogFileFromArgs(EPrefix prefix, LogFile lf) {
            LogFile toWrite = null;
            if (null == lf) {
                if (_fileRedirection.ContainsKey(prefix))
                    toWrite = this[_fileRedirection[prefix]];
                else
                    toWrite = _defaultLog;
            }
            else
                toWrite = lf;
            return toWrite;
        }

        private void WriteInfoIns(string s) {
            WriteInfoIns(s, _defaultLog);
        }

        private void WriteInfoIns(string s, LogFile lf) {
            if (_isEnabled) {
                lf.Write(s);
            }
        }
      
        private void WriteInfoIns(string s, EPrefix prefix, LogFile lf) {
            if (_isEnabled) {
                if (!_globalInfoLogPrefix.IsFiltred(prefix)) {
                    LogFile toWrite = GetLogFileFromArgs(prefix, lf);
                    toWrite.Write(s, prefix);
                }
            }
        }

        private void CloseIns() {
            foreach (LogFile log in _logFiles.Values)
                log.Close();
        }

        private void DisableIns() {
            _isEnabled = false;
        }

        private void EnableIns() {
            _isEnabled = true;
        }

        #endregion

        #region Metody publiczne

        public OnWriteLineDelegate OnWriteLine {
            get {
                    return _defaultLog.OnWriteLine;
            }
            set {
                    _defaultLog.OnWriteLine = value;
            }
        }

        public static void WriteStart() {
            InfoLog.WriteAll("____________________________________________");
            InfoLog.WriteAll("Application BEGIN " +
                Assembly.GetExecutingAssembly().GetName().ToString() + " : "
                + Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

        public static void WriteEnd() {
            InfoLog.WriteAll("Application END");
            InfoLog.WriteAll("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
        }

        public static void Write(string s) {
            Instance.WriteIns(s);
        }

        public static void WriteAll(string s) {
            Instance.WriteAllIns(s);
        }

        public static void Write(string s, LogFiles logf) {
            Instance.WriteIns(s, Instance[logf]);
        }

        public static void WriteException(Exception ex) {
            Instance.WriteExceptionIns(ex);
        }

        public static void WriteException(Exception ex, LogFiles lf) {
            Instance.WriteExceptionIns(ex, Instance[lf]);
        }

        public static void WriteError(string s) {
            Instance.WriteErrorIns(s);
        }

        public static void WriteError(string s, EPrefix prefix) {
            Instance.WriteErrorIns(s, prefix, null);
        }

        public static void WriteError(string s, EPrefix prefix, LogFiles logFiles) {
            Instance.WriteErrorIns(s, prefix, Instance[logFiles]);
        }

        public static void WriteInfo(string s) {
            Instance.WriteInfoIns(s);
        }

        public static void WriteInfo(string s, EPrefix prefix) {
            Instance.WriteInfoIns(s, prefix, null);
        }

        public static void WriteInfo(string s, LogFiles lf) {
            Instance.WriteInfoIns(s, Instance[lf]);
        }



        public static void WriteInfo(string s, EPrefix prefix, LogFiles logFiles) {
            Instance.WriteInfoIns(s, prefix, Instance[logFiles]);
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
