using System;
using Managers;
using Managers.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UIElements.Popup
{
    public class EndGamePopup : MonoBehaviour, IUIPopup
    {
        [SerializeField] private Button endGameButton;
        
        public bool IsActive  => gameObject.activeSelf;

        private IGameplayManager _gameplayManager;
        
        [Inject]
        public void Init(IGameplayManager gameplayManager)
        {
            _gameplayManager = gameplayManager;
            endGameButton.onClick.AddListener(RestartAction);

        }

        private void RestartAction()
        {
            Hide();
            _gameplayManager.RestartScenario();
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
