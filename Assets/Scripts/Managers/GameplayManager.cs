using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Data;
using Managers.Controller;
using Managers.Interfaces;
using Player.Commands;
using Services;
using Settings;
using Tools;
using UIElements.Pages;
using UIElements.Popup;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class GameplayManager : IGameplayManager, IInitializable
    {
        private IUIManager _uIManager;
        private List<IController> _controllers;

        public Enumerators.AppState CurrentState { get; private set; }
        public event Action<Enumerators.AppState> ChangedState;

        public bool IsPause { get; private set; }

        private IGameDataManager _gameDataManager;
        private DiContainer _container;

        public event Action InitGameplayManager;

        public bool IsInitialized { get; private set; }

        [Inject]
        public void Init(DiContainer container)
        {
            Debug.Log("GameplayManager Init");
            _container = container;
            _uIManager = container.Resolve<IUIManager>();
            _gameDataManager = container.Resolve<IGameDataManager>();

        }

        public void Initialize()
        {
            Debug.Log($"Init");
            _controllers = new List<IController>()
            {
                _container.Instantiate<LevelController>(),
                _container.Instantiate<PlayerController>(),
                _container.Instantiate<CameraController>(),
                _container.Instantiate<ScenarioController>(),
            };
            foreach (var controller in _controllers)
            {
                controller.Init(_container);
            }

            GetController<PlayerController>().PlayerGlobalEvent += HandlePlayerGlobalEvent;

            CheckInitController().Forget();
        }

        private void HandlePlayerGlobalEvent(BasePlayerGlobalEvent message)
        {
            switch (message)
            {
                case PGEPlayerDied died:
                    EndGame();
                    break;     
                case PGEPlayerCompletedScenario completedScenario:
                    WinGame();
                    break;
            }
        }

        private void EndGame()
        {
            GetController<CameraController>().ResetCamera();

            TaskManager.ExecuteAfterDelay(3, () => _uIManager.DrawPopup<EndGamePopup>());
        }
        private void WinGame()
        {
            TaskManager.ExecuteAfterDelay(0.1f, () => _uIManager.DrawPopup<WinLevelPopup>());

        }

        public T GetController<T>() where T : IController
        {
            return (T)_controllers.Find(controller => controller is T);
        }

        private bool ControllerReadiness()
        {
            return _controllers.All(t => t.IsInit);
        }

        private async UniTask CheckInitController()
        {
            await TaskManager.WaitUntil(ControllerReadiness);
            InitGameplayManager?.Invoke();
            IsInitialized = true;
        }

        public void StartScenario()
        {
            GetController<PlayerController>().StartGameplay();

            ChangeAppState(Enumerators.AppState.InGameScene);
        }

        public void NextScenario()
        {
            GetController<PlayerController>().Reset();

            GetController<ScenarioController>().LoadNextScenario();
            ChangeAppState(Enumerators.AppState.InMainMenu);
        }

        public void RestartScenario()
        {
            GetController<CameraController>().ActivationCameraTrack();
            GetController<PlayerController>().Restart(); 
            GetController<LevelController>().Reset();
            GetController<PlayerController>().StartGameplay();
        }

        public void ChangeAppState(Enumerators.AppState stateTo)
        {
            Debug.Log(stateTo);
            switch (stateTo)
            {
                case Enumerators.AppState.InMainMenu:
                    _uIManager.SetPage<MainMenuPagePresenter>();
                    break;
                case Enumerators.AppState.InGameScene:
                    _uIManager.SetPage<InGamePagePresenter>();

                    break;
            }

            CurrentState = stateTo;
            ChangedState?.Invoke(stateTo);
        }
    }
}