using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Data;
using Level;
using Managers.Interfaces;
using Settings;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Managers
{
    public class SceneLoaderManager : ISceneLoaderManager
    {
        private readonly ZenjectSceneLoader _sceneLoader;

        private ScenarioData _scenarioData;

        private string _currentLevelSceneName;

        public SceneLoaderManager(ZenjectSceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public void LoadLevelScene(string levelSceneName)
        {
            LoadLevelSceneRoutine(levelSceneName).Forget();
        }

        public async UniTask<LevelPresenterView> LoadLevelSceneRoutine(string sceneName)
        {
            if (!string.IsNullOrEmpty(_currentLevelSceneName))
            {
                SceneManager.UnloadSceneAsync(_currentLevelSceneName);
            }

            await _sceneLoader.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            Scene newScene = SceneManager.GetSceneByName(sceneName);

            if (newScene.IsValid())
            {
                SceneManager.SetActiveScene(newScene);
            }

            _currentLevelSceneName = sceneName;

            LevelPresenterView presenterLevel = null;

            foreach (var root in newScene.GetRootGameObjects())
            {
                presenterLevel = root.GetComponentInChildren<LevelPresenterView>();
                if (presenterLevel != null) break;
            }

            if (presenterLevel == null)
            {
                Debug.LogError(
                    "LevelPresenterView was not found in the scene. " 
                );
            }

            return presenterLevel;
        }

        public void UnloadCurrentLevel()
        {
            if (!string.IsNullOrEmpty(_currentLevelSceneName))
            {
                SceneManager.UnloadSceneAsync(_currentLevelSceneName);
                _currentLevelSceneName = null;
            }
        }

        public void LoadGameScene(Enumerators.AppState appState)
        {
            throw new NotImplementedException();
        }
    }
}