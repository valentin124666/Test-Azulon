using System;
using Modules.Player.Modules.Comands;

namespace Modules.Player.Modules
{
    public class StatsModule :BasePlayerModule ,IPlayerModuleStats
    {
        private float _currentSpeed = 5;
        private float _xpBoost;
        
        public event Action<TypeStatsView, string> UpdateProperty;
        public float XpBoost => _xpBoost;
        public float CurrentSpeed => _currentSpeed;
        
        public override void Apply(IPlayerCommand command)
        {
            if (command is not IStatsCommand statsCommand)
                return;

            switch (statsCommand)
            {
                case PCMAddSpeed addSpeed:
                    _currentSpeed += addSpeed.Multiplier;

                    if (_currentSpeed < 1)
                    {
                        _currentSpeed = 1;
                    }
                    UpdateProperty?.Invoke(TypeStatsView.speed, _currentSpeed.ToString());
                    break;        
                case PCMAddXPBoost addXpBoost:
                    _xpBoost += addXpBoost.Multiplier;

                    if (_xpBoost < 1)
                    {
                        _xpBoost = 1;
                    }
                    UpdateProperty?.Invoke(TypeStatsView.xpBoost, _xpBoost.ToString());
                    break;          
                default:
                    return;
            }
        }

    }
}
