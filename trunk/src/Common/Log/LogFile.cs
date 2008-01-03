using System;
using System.Collections.Generic;
using System.Text;
using Yad.Log.Common;
using System.Reflection;
using System.ComponentModel;
using System.IO;

namespace Yad.Log {
    public class LogFile {

        /// <summary>
        /// Nazwa pliku
        /// </summary>
        private string _fileName;

        /// <summary>
        /// Filtry dla pliku
        /// </summary>
        private InfoLogPrefix _infoLogPrefix;

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
        /// Strumien do zapisu
        /// </summary>
        private  MultiStream _writer = null;

        public void AddFilter(EPrefix prefix) {
            _infoLogPrefix.AddFilter(prefix);
        }

        public void RemoveFilter(EPrefix prefix) {
            _infoLogPrefix.RemoveFilter(prefix);
        }

        public LogFile(string fileName) {
            _fileName = fileName;
            _writer = new MultiStream(fileName);
            _infoLogPrefix = new InfoLogPrefix();
        }

        public LogFile(LogFiles eLogFile) : this(GetDescription(eLogFile)) {
            
        }

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


        public void Write(string text) {
            _writer.WriteLine(text);
        }

        public void Write(string text, EPrefix prefix) {
            if (!_infoLogPrefix.IsFiltred(prefix)) {
                _writer.WriteLine(_infoLogPrefix.AddFilterString(text, prefix));
            }
        }
        public void Close() {
            _writer.Close();
            TruncFileBeginning();
        }

        private static string GetDescription(Enum value) {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }

        private void TruncFileBeginning() {
            FileStream fs = null;
            try {
                fs = new FileStream(_fileName, FileMode.Open);
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

    }
}
