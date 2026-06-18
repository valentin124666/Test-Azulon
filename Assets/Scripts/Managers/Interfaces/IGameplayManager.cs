using System;
using Cysharp.Threading.Tasks;
using Settings;

namespace Managers.Interfaces
{
    public interface IGameplayManager : IInitializableState
    {
        T GetController<T>() where T : IController;

        event Action<Enumerators.AppState> ChangedState;
        Enumerators.AppState CurrentState { get; }

        bool IsPause { get; }
        void StartScenario();
        void NextScenario();
        void RestartScenario();
        
        void ChangeAppState(Enumerators.AppState stateTo);
    }
}