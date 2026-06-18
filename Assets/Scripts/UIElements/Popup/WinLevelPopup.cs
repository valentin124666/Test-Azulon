using System;
using Managers.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UIElements.Popup
{
    public class WinLevelPopup : MonoBehaviour, IUIPopup
    {
        [SerializeField] private Button nextLevelButton;
    
        public bool IsActive  => gameObject.activeSelf;

        private IGameplayManager _gameplayManager;

        [Inject]
        public void Init(IGameplayManager gameplayManager)
        {
            _gameplayManager = gameplayManager;
            nextLevelButton.onClick.AddListener(NextLevelAction);
        }

        private void NextLevelAction()
        {
            Hide();
            _gameplayManager.NextScenario();
        }
    
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Show(Action callback)
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Reset()
        {
        
        }
    }
}
