using Cysharp.Threading.Tasks;
using Data;
using Managers;
using Managers.Controller;
using Managers.Interfaces;
using Settings;
using UnityEngine;
using Zenject;

public class MainApp : MonoBehaviour
{
    private async void Start()
    {
        var gameplayManager = ProjectContext.Instance.Container.Resolve<IGameplayManager>();
        var gameDataManager = ProjectContext.Instance.Container.Resolve<IGameDataManager>();

        await UniTask.WhenAll(
            UniTask.WaitUntil(() => gameplayManager.IsInitialized),
            UniTask.WaitUntil(() => gameDataManager.IsInitialized)
        );
        var gameData = ProjectContext.Instance.Container.Resolve<IGameDataManager>().GetDataScriptable<GameData>();
        // Debug.Log(gameData.ScenarioData.StateSceneBinding[0].levelPrefab.AssetGUID);
        // Debug.Log(gameData.ScenarioData.StateSceneBinding[0].levelPrefab.Asset);
        // Debug.Log(gameData.ScenarioData.StateSceneBinding[0].levelPrefab.editorAsset);
        
        gameplayManager.GetController<ScenarioController>().DownloadInitialScenario();
        gameplayManager.ChangeAppState(Enumerators.AppState.InMainMenu);
    }
}