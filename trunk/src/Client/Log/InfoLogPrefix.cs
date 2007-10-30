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
        /// <summary>
        /// Wartosc kontrolna - musi byc zawsze na koncu
        /// </summary>
        _NUMBER_PREFIXES
    };


    class InfoLogPrefix
    {
        private string[] _prefixes = { " #INI: ", " #MNU: " };
        bool[] _filters = null;
        public InfoLogPrefix()
        {
            _filters = new bool[(int)EPrefix._NUMBER_PREFIXES];
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

        public bool isFiltred(EPrefix prefix)
        {
            return _filters[(int)prefix];
        }

        public string AddFilterString(string message, EPrefix prefix)
        {
            return _prefixes[(int)prefix] + message;
        }
    }
}
