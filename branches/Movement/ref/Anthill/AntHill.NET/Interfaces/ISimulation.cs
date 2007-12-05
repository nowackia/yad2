using System;

namespace AntHill.NET
{
    public interface ISimulationUser
    {
        bool DoTurn();
        void Reset();
        void Start();
        void Stop();
        int GetAntsCount();
        int GetSignalsCount();
        int GetTurnsCount();
        int GetSpidersCount();
    }
}