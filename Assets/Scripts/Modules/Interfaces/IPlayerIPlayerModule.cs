using Managers.Controller;
using Zenject;

namespace Modules.Interfaces
{
    public interface IPlayerModule
    {
        void Init(DiContainer controller, PlayerPresenterView playerView);
        void Reset();
    }
}
