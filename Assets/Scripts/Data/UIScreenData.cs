using System;
using Modules;
using Settings;
using UIElements.Pages;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Custom menu/Scenario/UIScreenData")]
    public class UIScreenData : ScriptableObject
    {
        public PagePrefabData[] PagePrefabData;
    }
    
    [Serializable]
    public class PagePrefabData
    {
        public Enumerators.AppState pageState;
        
        public string presenterType;
        public string locationSuffix;

        public PagePresentersView pagePrefab;  
    } 
}
