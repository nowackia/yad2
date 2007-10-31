using System;
using System.Collections.Generic;
using System.Text;

namespace AntHill.NET
{
    /// <summary>
    /// This is a configuration class containing simulation parameters.
    /// </summary>
    public static class AntHillConfig
    {
        #region Simulation_Parameters

        //World map
        static public int mapRowCount, mapColCount;
        static public Tile[,] tiles;

        //Ants
        static public int antMaxLife, antMaxLifeWithoutFood, antTurnNumberToBecomeHungry,
                    antStrength, antForgettingTime, antSightRadius,
                    workerStartCount, warriorStartCount;

        //Queen
        static public float queenLayEggProbability;
        static public int queenXPosition, queenYPosition;

        //Egg
        static public int eggHatchTime;
        static public float eggHatchWarriorProbability; //value from 0 to 1

        //Spider
        static public int spiderMaxHealth, spiderFoodQuantityAfterDeath;
        static public float spiderProbability;

        //Rain
        static public int rainWidth, rainMaxDuration;
        static public float rainProbability;

        //Food
        static public float foodProbability;

        //Message (referred as 'signal' in config file)
        static public int messageLifeTime, messageRadius;

        #endregion 

        #region Simulation_Additional_Parameters

        //If we need to configure/store some settings then do it here
        
        static public int antMaxHealth = 1; //same as in spider
        static public int antFoodQuantityAfterDeath = 1;
        static public int foodRandomMaxQuantity = 4;

        static public float curMagnitude = 1.0f;
        static public float signalHighestDensity = 10.0f;
        static public int signalInitialAlpha = 2;

        #endregion
    }
}
