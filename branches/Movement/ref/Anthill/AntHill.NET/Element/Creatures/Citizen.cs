using System;
using System.Collections.Generic;
using AntHill.NET.Utilities;

namespace AntHill.NET
{
    public abstract class Citizen : Ant
    {
        protected Message[] _messages = new Message[4];
        protected int[] _intensities = new int[4];
        protected int[] _forgetting = new int[4];

        public Citizen(Position pos) : base(pos)
        {
            for (int i = 0; i < 4; i++)
                _messages[i] = null;
        }

        public virtual void AddToSet(Message m, int intensity)
        {
            int t = (int)m.GetMessageType;
            if (_intensities[t] < intensity)
            {
                _intensities[t] = intensity;
                _forgetting[t] = AntHillConfig.antForgettingTime;
                _messages[t] = m;
            }
        }

        protected bool FindEqualSignal(MessageType mt, Position location)
        {
            return (_messages[(int)mt] != null ? _messages[(int)mt].TargetPosition == location : false);
        }
        
        public virtual void SpreadSignal(ISimulationWorld isw)
        {
            MaintainRememberedMessages();

            for (int i = 0; i < 4; i++)
            {
                if(_messages[i] != null)
                    _messages[i].Spread(isw, this.Position, _intensities[i] - 1);                
            }
        }

        void MaintainRememberedMessages()
        {
            for (int i = 0; i < 4; i++)
            {
                if (_messages[i] != null)
                {
                    if (--_intensities[i] <= 0 || --_forgetting[i] <= 0 || _messages[i].Empty)
                        _messages[i] = null;
                }
            }
        }

        protected Food GetNearestFood(LIList<Food> foods)
        {
            if (foods.Count == 0) return null;

            Food bestFood = null;
            int min = Int32.MaxValue;
            int tmp;
            LIList<Food>.Enumerator e = foods.GetEnumerator();
            while(e.MoveNext())
            {
                if ((tmp = DistanceMeasurer.Taxi(this.Position, e.Current.Position)) < min)
                {
                    bestFood = e.Current;
                    min = tmp;
                }
            }
            return bestFood;
        }
    }
}
