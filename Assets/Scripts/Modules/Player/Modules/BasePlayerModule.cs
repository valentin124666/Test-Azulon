using System;

namespace Modules.Player.Modules
{
    public abstract class BasePlayerModule
    {
        public abstract void Apply(IPlayerCommand command);
    }

    public interface IPlayerModuleStats
    {
        public event Action<TypeStatsView, string> UpdateProperty;
    }
    public enum TypeStatsView
    {
        health,
        attack,
        defense,
        xpBoost,
        speed
    }
}