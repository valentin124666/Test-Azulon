using System;
using Modules.Player.Modules.Comands;

namespace Modules.Player.Modules
{
    public class HealthModule : BasePlayerModule , IPlayerModuleStats
    {
        private int _currentHealth;
        private int _maxHealth = 10;

        public event Action<TypeStatsView, string> UpdateProperty;
        public int MaxHealth => _maxHealth;
        public int CurrentHealth => _currentHealth;

        public override void Apply(IPlayerCommand command)
        {
            if (command is not IHealthCommand healthCommand)
                return;

            switch (healthCommand)
            {
                case PCMHeal heal:
                    _currentHealth += heal.Amount;
                    if (_currentHealth > _maxHealth)
                    {
                        _currentHealth = _maxHealth;
                    }

                    if (_currentHealth <= 0)
                    {
                        _currentHealth = 0;
                    }

                    break;
                case PCMAddMaxHealth addMaxHealth:

                    _maxHealth += addMaxHealth.Amount;
                    if (_maxHealth < 1)
                    {
                        _maxHealth = 1;
                    }

                    break;
                default:
                    return;
            }

            UpdateProperty?.Invoke(TypeStatsView.health, $"{_currentHealth}/{_maxHealth}");
        }
    }
}