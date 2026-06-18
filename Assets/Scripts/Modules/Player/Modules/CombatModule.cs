using System;
using Modules.Player.Modules.Comands;

namespace Modules.Player.Modules
{
    public class CombatModule : BasePlayerModule,IPlayerModuleStats
    {
        private int _attack;
        private int _defense;
        public event Action<TypeStatsView, string> UpdateProperty;
        public int Attack => _attack;
        public int Defense => _defense;
        
        public override void Apply(IPlayerCommand command)
        {
            if (command is not ICombatCommand combatCommand)
                return;

            switch (combatCommand)
            {
                case PCMAddAttack addAttack:
                    _attack += addAttack.Amount;

                    if (_attack<1)
                    {
                        _attack = 1;
                    }
                    UpdateProperty?.Invoke(TypeStatsView.attack, _attack.ToString());
                    break;        
                case PCMAddArmor addArmor:
                    
                    _defense +=addArmor.Amount;
                    if (_defense < 1)
                    {
                        _defense = 1;
                    }
                    UpdateProperty?.Invoke(TypeStatsView.defense, _defense.ToString());
                    break;          
                default:
                    return;
            }
        }
        
        
    }
}
