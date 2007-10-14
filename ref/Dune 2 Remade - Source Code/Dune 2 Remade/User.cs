using System;
using System.Collections.Generic;
using System.Text;

namespace Dune_2_Remade
{   
    [Serializable]
    public class User
    {
        static int ServerUserID=0;
        public int userID = -1;
        private Houses.House house = Houses.House.Atreides;
        public Houses.House House
        {
            get { return house; }
            set
            {
                house = value;
                switch (house)
                {
                    case Houses.House.Atreides:
                        abb = "a";
                        break;
                    case Houses.House.Harkonnen:
                        abb = "m";
                        break;
                    case Houses.House.Ordos:
                        abb = "o";
                        break;
                }
            }
        }

        public string abb = "a";
        public string name;
        static public int GetNewID()
        {
            ServerUserID++;
            return ServerUserID;
        }

        public override string ToString()
        {
            return name + " (" + house.ToString() + ")";
        }
    }
}
