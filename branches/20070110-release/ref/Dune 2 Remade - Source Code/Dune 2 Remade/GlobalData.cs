using System;
using System.Collections.Generic;
using System.Text;

namespace Dune_2_Remade
{
    public static class GlobalData
    {
        public static ExtendedTexture objectSelected;
        public static ExtendedTexture [][]Structure_Building=new ExtendedTexture[4][];
        public static List<User> usersList = new List<User>();
        public static User me = new User();
        public static bool isPlaying = false;
        public static bool[] playersReadyToPlay=null;
        public static GameSettings gameSettings = GameSettings.LoadSettings(null);
        public static Houses.House GetHouse(int userID)
        {
            for (int i = 0; i < usersList.Count; i++)
            {
                if (usersList[i].userID == userID)
                    return usersList[i].House;
            }
            return Houses.House.None;
        }
        static GlobalData()
        {
            for (int i = 0; i < Structure_Building.Length; i++)
                Structure_Building[i] = new ExtendedTexture[4];
        }
        public static string GetNameByID(int userID)
        {       
            for (int i = 0; i < usersList.Count; i++)
            {
                if(usersList[i].userID==userID)
                    return usersList[i].name;
            }
            return null;
        }

        public static bool DeleteUserByID(int userID)
        {
            for (int i = 0; i < usersList.Count; i++)
            {
                if (usersList[i].userID == userID)
                {
                    usersList.Remove(usersList[i]);
                    return true;
                }
            }
            return false;
        }
    }
}
