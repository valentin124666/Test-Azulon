using System;
using Cysharp.Threading.Tasks;
using Data;
using Managers.Interfaces;
using ModestTree;
using Tools;
using UnityEngine;
using Zenject;
using static Settings.Enumerators;
using Random = UnityEngine.Random;

namespace Managers.Controller
{
    public class ScenarioController : IController
    {
        private IGameDataManager _gameDataManager;
        private IGameplayManager _gameplayManager;

        private LevelController _levelController;


        private PlayerController _playerController;

        private StateSceneBinding _currentScenario;

        public LevelState CurrentLevelState { get; private set; }
        public event Action<LevelState> ChangedLevelState;
        public bool IsInit { get; private set; }

        public void Init(DiContainer container)
        {
            _gameDataManager = container.Resolve<IGameDataManager>();
            _gameplayManager = container.Resolve<IGameplayManager>();
            // _loadingCurtain =  container.Resolve<LoadingCurtain>();

            _levelController = _gameplayManager.GetController<LevelController>();
            _playerController = _gameplayManager.GetController<PlayerController>();
            IsInit = true;
        }

        private async UniTask LoadGameScenario(StateSceneBinding sceneBinding, float wait = 0)
        {
            if (wait != 0)
            {
                await TaskManager.WaitUntilDelay(wait);
            }

            _currentScenario = sceneBinding;

            await _levelController.CreateLevelSceneLevelAsync(sceneBinding.levelPrefab);
            await _playerController.CreatePlayerPresentersAsync(sceneBinding.playerPrefab);
            _gameplayManager.GetController<CameraController>().ActivationCameraTrack();
            // _loadingCurtain.Hide(0.5f);
        }

        public void LoadNextScenario()
        {
            // _loadingCurtain.Show(0.25f);

            var oldScenario = _currentScenario;
            var newScenario = _currentScenario;

            var scenario = _gameDataManager.GetDataScriptable<GameData>().ScenarioData;

            int numberCurrent = scenario.bindings.IndexOf(oldScenario);

            if (scenario.bindings.Length <= 1)
            {
                _gameplayManager.RestartScenario();
                return;
            }

            while (newScenario == oldScenario)
            {
                var number = 0;

                if (numberCurrent < scenario.bindings.Length - 1)
                {
                    number = numberCurrent + 1;
                }
                else
                {
                    number = Random.Range(0, scenario.bindings.Length);
                }


                newScenario = scenario.bindings[number];
            }

            LoadGameScenario(newScenario, 0.25f).Forget();
        }

        public void DownloadInitialScenario()
        {
            // _loadingCurtain.ShowQuick();

            Debug.Log("DownloadInitialScenario");

            var scenario = _gameDataManager.GetDataScriptable<GameData>().ScenarioData;
            var number = Random.Range(0, scenario.bindings.Length);

            LoadGameScenario(scenario.bindings[scenario.requiredScenario]).Forget();

            // ChangeLevelState(LevelState.InReadyToStartLevel);
        }

        public void ChangeLevelState(LevelState levelStateTo)
        {
            Debug.Log(levelStateTo);
            switch (levelStateTo)
            {
                case LevelState.InReadyToStartLevel:


                    break;
                case LevelState.InLevelToPlay:

                    break;
                case LevelState.InWinLevel:

                    break;
                case LevelState.InLosLevel:

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(levelStateTo), levelStateTo, null);
            }

            CurrentLevelState = levelStateTo;
            ChangedLevelState?.Invoke(levelStateTo);
        }
    }
}