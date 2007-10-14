using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dune_2_Remade
{
    public static class Houses
    {
        public enum House { Atreides=0, Harkonnen=1, Ordos=2, None=3};        
        static private float AtreidesSpeedBonus =1.0f, AtreidesHealthBonus =1.0f,
            OrdosSpeedBonus =1.1f, OrdosHealthBonus =0.9f,
            HarkonnenSpeedBonus =0.9f, HarkonnenHealthBonus=1.1f;

        public static int CalculateHealth(House race, int health)
        {
            switch (race)
            {
                case House.Atreides:
                    return (int)((float)health * AtreidesHealthBonus);
                case House.Harkonnen:
                    return (int)((float)health * HarkonnenHealthBonus);
                case House.Ordos:
                    return (int)((float)health * OrdosHealthBonus);
            }
            return health;
        }
        public static Color GetColor(House house)
        {
            switch (house)
            {
                case House.Atreides:
                    return Color.LightBlue;
                case House.Harkonnen:
                    return Color.Pink;
                case House.Ordos:
                    return Color.LightGreen;
            }
            return Color.White;
        }
        public static float CalculateSpeed(House race, float speed)
        {
            switch (race)
            {
                case House.Atreides:
                    return speed * AtreidesSpeedBonus;
                case House.Harkonnen:
                    return speed * HarkonnenSpeedBonus;
                case House.Ordos:
                    return speed * OrdosSpeedBonus;
            }
            return speed;
        }
    }
}
