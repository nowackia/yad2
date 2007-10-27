using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace Client.Log
{
    public class InfoLog
    {
        private const string ERROR_LOG_FILENAME = "ErrorLog.txt";
        /// <summary>
        /// Procent maksymalnej dlugosci loga, jaki pozostanie bo redukcji jego dlugosci
        /// </summary>
        private const double CUT_PERCENT = 0.75;
        /// <summary>
        /// Pakiet danych transportowany przy przesuwaniu pliku w metodzie
        /// TruncFileBeginnig
        /// </summary>
        private const int DATA_PACK_SIZE = 8096;
        /// <summary>
        /// Maksymalna wielkoœæ pliku
        /// </summary>
        private const long MAX_FILESIZE = 10485760L;
        private static InfoLog _infoLog = null;
        private static MultiStream _writer = null;
        private static InfoLogPrefix _infoLogPrefix = null;


        private InfoLog(MultiStream writer)
        {
            _writer = writer;
            _infoLogPrefix = new InfoLogPrefix();
        }


        public static InfoLog Instance
        {
            get
            {
                if (null == _infoLog)
                {
                    _infoLog = new InfoLog(GetWriter());
                    return _infoLog;
                }
                return _infoLog;
            }
        }

        private static MultiStream GetWriter()
        {
            MultiStream writer = null;
            writer = new MultiStream(ERROR_LOG_FILENAME);
            return writer;
        }

        private void write(string s)
        {
            _writer.WriteLine(s);
        }

        private void writeSingleException(Exception ex)
        {
            _writer.WriteLine("Message: " + (ex.Message == null ? "null" : ex.Message));
            _writer.WriteLine("Stack:");
            _writer.WriteLine(ex.StackTrace == null ? "null" : ex.StackTrace);
        }

        private void writeException(Exception ex)
        {
            _writer.WriteLine("-- WYJATEK ---" + DateTime.Now.ToString() + "--------------");
            writeSingleException(ex);
            if (ex.InnerException != null)
            {
                _writer.WriteLine("---- InnerException: " + ex.InnerException.ToString());
                writeSingleException(ex.InnerException);
            }
            _writer.WriteLine("------------------------------------------------------------");
        }

        private void writeError(string s)
        {
            _writer.WriteLine("-- BLAD ---" + DateTime.Now.ToString() + "-----------------");
            _writer.WriteLine("Message: " + s);
            _writer.WriteLine("------------------------------------------------------------");
        }

        private void writeInfo(string s)
        {
            _writer.WriteLine("#I:# " + DateTime.Now.ToString() + "  " + s);
        }

        private void writeInfo(string s, EPrefix prefix)
        {
            if (!_infoLogPrefix.isFiltred(prefix))
            {
                s = _infoLogPrefix.AddFilterString(s, prefix);
                _writer.WriteLine("#I:# " + DateTime.Now.ToString() + "  " + s);
            }
        }

        public static void WriteStart()
        {
            InfoLog.Write("____________________________________________");
            InfoLog.Write("Start aplikacji " + 
                Assembly.GetExecutingAssembly().GetName().ToString() + " : " 
                + Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

        public static OnWriteLineDelegate OnWriteLine
        {
            get
            {
                return _writer.OnWriteLine;
            }
            set
            {
                _writer.OnWriteLine = value;
            }
        }
        public static void WriteEnd()
        {
            InfoLog.Write("Koniec aplikacji");
            InfoLog.Write("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
        }
        public static void Write(string s)
        {
            Instance.write(s);
        }
        public static void WriteException(Exception ex)
        {
            Instance.writeException(ex);
        }

        public static void WriteError(string s)
        {
            Instance.writeError(s);
        }

        public static void WriteInfo(string s, EPrefix prefix)
        {
            Instance.writeInfo(s, prefix);
        }

        public static void WriteInfo(string s)
        {
            Instance.writeInfo(s);
        }

        public static void Close()
        {
            Instance.close();
        }
        #region Czynnosci finalizacyjne

        public void close()
        {
            _writer.Close();
            truncFileBeginning();
        }

        #endregion

        #region Operacje na pliku

        private static void truncFileBeginning()
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(ERROR_LOG_FILENAME, FileMode.Open);
            }
            catch (Exception)
            {
                return;
            }
            if (fs.Length <= MAX_FILESIZE)
            {
                try
                {
                    fs.Close();
                }
                catch (Exception)
                {
                }
                return;
            }
            int cut_filesize = (int)(MAX_FILESIZE * CUT_PERCENT);
            int toCut = (int)(fs.Length - cut_filesize);
            int times = cut_filesize / DATA_PACK_SIZE;
            int rest = cut_filesize % DATA_PACK_SIZE;
            int position = toCut;
            byte[] data = new byte[DATA_PACK_SIZE];
            int i = 0;
            try
            {
                for (i = 0; i < times; ++i, position += DATA_PACK_SIZE)
                {
                    fs.Seek(position, SeekOrigin.Begin);
                    fs.Read(data, 0, DATA_PACK_SIZE);
                    fs.Seek(i * DATA_PACK_SIZE, SeekOrigin.Begin);
                    fs.Write(data, 0, DATA_PACK_SIZE);
                }
                if (rest > 0)
                {
                    fs.Seek(position, SeekOrigin.Begin);
                    fs.Read(data, 0, rest);
                    fs.Seek(i * DATA_PACK_SIZE, SeekOrigin.Begin);
                    fs.Write(data, 0, rest);
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                try
                {
                    fs.SetLength(cut_filesize);
                    fs.Close();
                }
                catch (Exception)
                {
                }
            }
        }
    }
        #endregion 
}
