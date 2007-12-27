using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Yad.Log.Common
{
    public enum EPrefix
    {
        /// <summary>
        /// Initialization information
        /// </summary>
        [Description(" #INI: ")]
        Initialization = 0,

        ///<summary>
        /// Menu information
        /// </summary>
        [Description(" #MENU: ")]
        Menu,
        [Description(" #GRF: ")]
		GameGraphics,
		[Description(" #AUD: ")]
		AudioEngine,
		[Description(" #GLOG: ")]
		GameLogic,
        [Description(" #FIN:")]
        Finalization,
        [Description(" #UIM: ")]
        UIManager,
        [Description(" #SERV_INFO: ")]
        ServerInformation,
        [Description(" #CLIENT_INFO: ")]
        ClientInformation,
        [Description(" #MSG_RCV: ")]
        MessageReceivedInfo,
        [Description(" #SEND_MSG_INFO: ")]
        ServerSendMessageInfo,
		[Description(" #SIM: ")]
		SimulationInfo,
		[Description(" #CLI_SIM: ")]
		ClientSimulation,
        [Description(" #STR: ")]
        Stripe,
        [Description(" #SPI: ")]
        ServerProcessInfo,
        [Description(" #SVA: ")]
        ServerAction,
        [Description(" #GMP: ")]
        GameMessageProccesing,
        [Description(" #DBI: ")]
        DatebaseInfo,
        [Description(" #A*: ")]
        AStar,
        [Description(" #LOK: ")]
        LockInfo,
        [Description(" #GEN: ")]
        GObj,
        [Description(" #MOV: ")]
        Move,
        [Description(" #AI: ")]
        AI,
        [Description(" #BM: ")]
        BMan
        

        // Wartosc kontrolna - musi byc zawsze na koncu
        //_NUMBER_PREFIXES
    };


    class InfoLogPrefix
    {
        //private string[] _prefixes = { " #INI: ", " #MNU: ", " #GRF: ", " #FIN: ", "#UIM: ",  "#SERV_INFO: ", " #MSG_RCV: " };
        bool[] _filters = null;
        public InfoLogPrefix()
        {
            _filters = new bool[Enum.GetValues(typeof(EPrefix)).Length];
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

        private string GetDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }

        public string AddFilterString(string message, EPrefix prefix)
        {
            return GetDescription(prefix) + message;
        }
    }
}
