using Cysharp.Threading.Tasks;
using Level;
using Settings;

namespace Managers.Interfaces
{
    public interface ISceneLoaderManager
    {
        UniTask<LevelPresenterView> LoadLevelSceneRoutine(string scenePath);
    }
}
