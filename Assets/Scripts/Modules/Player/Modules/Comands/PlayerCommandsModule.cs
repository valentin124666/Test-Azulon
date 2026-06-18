namespace Modules.Player.Modules.Comands
{
    public class PCMHeal : IHealthCommand
    {
        public int Amount { get; }

        public PCMHeal(int amount)
        {
            Amount = amount;
        }
    }

    public class PCMAddMaxHealth : IHealthCommand
    {
        public int Amount { get; }

        public PCMAddMaxHealth(int amount)
        {
            Amount = amount;
        }
    }

    public class PCMAddArmor : ICombatCommand
    {
        public int Amount { get; }

        public PCMAddArmor(int amount)
        {
            Amount = amount;
        }
    }

    public class PCMAddAttack : ICombatCommand
    {
        public int Amount { get; }

        public float Duration { get; }

        public PCMAddAttack(int amount, float duration)
        {
            Amount = amount;
            Duration = duration;
        }
    }

    public class PCMAddSpeed : IStatsCommand
    {
        public float Multiplier { get; }

        public float Duration { get; }

        public PCMAddSpeed(float multiplier, float duration)
        {
            Multiplier = multiplier;
            Duration = duration;
        }
    }

    public class PCMAddXPBoost : IStatsCommand
    {
        public float Multiplier { get; }

        public float Duration { get; }

        public PCMAddXPBoost(float multiplier, float duration)
        {
            Multiplier = multiplier;
            Duration = duration;
        }
    }
}