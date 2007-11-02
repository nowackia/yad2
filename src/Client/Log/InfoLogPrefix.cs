using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Log
{
    public enum EPrefix
    {
        /// <summary>
        /// Informacja o inicjalizacji
        /// </summary>
        Initialization = 0,
        ///<summary>
        /// Informacja o menu
        /// </summary>
        Menu,
		GameGraphics,
        //_NUMBER_PREFIXES
    };


    class InfoLogPrefix
    {
        private string[] _prefixes = { " #INI: ", " #MNU: ", " #GRF: " };
        bool[] _filters = null;
        public InfoLogPrefix()
        {
            //_filters = new bool[(int)EPrefix._NUMBER_PREFIXES];
            _filters = new bool[(int)Enum.GetValues(typeof(EPrefix)).Length];
            for (int i = 0; i < _filters.Length; ++i)
                _filters[i] = false;
        }
        public void AddFilter(EPrefix prefix)
        {
            _filters[(int)prefix] = true;
        }

        public void RemoveFilter(EPrefix prefix)
        {
            _filters[(int)prefix] = false;
        }

        public void Clear()
        {
            for (int i = 0; i < _filters.Length; ++i)
                _filters[i] = false;
        }

        public void FilterAll()
        {
            for (int i = 0; i < _filters.Length; ++i)
                _filters[i] = true;
        }

        public bool IsFiltred(EPrefix prefix)
        {
            return _filters[(int)prefix];
        }

        public string AddFilterString(string message, EPrefix prefix)
        {
            return _prefixes[(int)prefix] + message;
        }
    }
}
