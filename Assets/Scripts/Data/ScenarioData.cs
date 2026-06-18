using System;
using Level;
using Modules;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Data
{
    [CreateAssetMenu(menuName = "Custom menu/Scenario/ScenarioData")]
    public class ScenarioData : ScriptableObject
    {
        public StateSceneBinding[] bindings;
        public int requiredScenario;
    }
    [Serializable]
    public class StateSceneBinding
    {
       public PlayerPrefab playerPrefab;
       public LevelPrefab levelPrefab;
    }
    [Serializable]
    public class PlayerPrefab
    {
        public string presenterType;
        public string viewType;
        public string locationSuffix;

        public PlayerPresenterView playerPrefab;

        public AssetReference playerReferencePrefab;
    }  

    [Serializable]
    public class LevelPrefab
    {
        public string sceneName;
#if UNITY_EDITOR
        public SceneAsset sceneAsset;
#endif
    }
}
