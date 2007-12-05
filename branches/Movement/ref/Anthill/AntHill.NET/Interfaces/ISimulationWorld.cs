using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace AntHill.NET
{
    /// <summary>
    /// This interface will be send to objects in Maintain() function,
    /// this is how creatures will interact with environment.
    /// This is a subject to change - particularly functions' arguments
    /// </summary>
    public interface ISimulationWorld
    {
        bool CreateAnt(Position position);
        bool CreateWarrior(Position position);
        bool CreateWorker(Position position);
        bool CreateSpider(Position position);    //This might be unnecessary because only Simulation creates Spiders
        bool CreateFood(Position position, int quantity);      //This might be used in Ants' Destroy() function, or at Simulation level -  after creature's death
        bool CreateMessage(Position position, MessageType mt, Position targetLocation);
        bool CreateEgg(Position position);
        bool Attack(Creature cAttacking, Creature cAttacked);
        bool FeedQueen(Worker w);

        //Element wants to get some info...
        LIList<Ant> GetVisibleAnts(Element e);
        LIList<Food> GetVisibleFood(Element e);
        LIList<Spider> GetVisibleSpiders(Element e);
        LIList<Message> GetVisibleMessages(Element e);

        bool DeleteEgg(Egg egg);
        bool DeleteRain();
        bool DeleteFood(Food food);
        bool DeleteAnt(Ant ant);
        bool DeleteSpider(Spider spider);
        Map GetMap();
    }
}
