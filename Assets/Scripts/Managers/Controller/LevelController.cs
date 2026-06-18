using System;
using Core;
using Cysharp.Threading.Tasks;
using Data;
using Level;
using Level.Command;
using Managers.Interfaces;
using Modules.Player.Commands;
using UnityEngine;
using Zenject;

namespace Managers.Controller
{
    public class LevelController : IController
    {
        public bool IsInit { get; private set; }

        private Transform _poolLevel;

        private ILevel _activeLevel;
        private ResourceLoader _resourceLoader;
        private ISceneLoaderManager _sceneLoaderManager;
        private PlayerController _playerController;

        public event Action CreateLevelPresenter;

        public void Init(DiContainer container)
        { 
            _resourceLoader = container.Resolve<ResourceLoader>();
            _sceneLoaderManager = container.Resolve<ISceneLoaderManager>();
            _playerController = container.Resolve<IGameplayManager>().GetController<PlayerController>();

            _poolLevel = new GameObject("[PoolLevel]").transform;
            IsInit = true;
        }

        public async UniTask CreateLevelSceneLevelAsync(LevelPrefab levelPrefab)
        {
            var levelView = await _sceneLoaderManager.LoadLevelSceneRoutine(levelPrefab.sceneName);

            if (levelView.GetType() == typeof(LevelPresenterView))
            {
                _activeLevel = (LevelPresenter)levelView.Instantiate();
            }

            CreateLevelPresenter?.Invoke();
        }

        public BaseLevelDataRequests RetrieveLevelInfo(BaseLevelDataCommands dataCommands)
        {
            switch (dataCommands)
            {
                default:
                    return new BaseLevelDataRequests();
            }
        }
        
        public void Reset()
        {
       
        }
    }
}