using System.Collections.Generic;
using Core;
using Managers;
using Managers.Controller;
using Managers.Interfaces;
using Modules.Player.Commands;
using Services;
using UIElements.Popup;
using Unity.Collections.LowLevel.Unsafe;
using Zenject;

namespace UIElements.Pages
{
    public class InGamePagePresenter : PagePresenters
    {
        private UpdateService _updateService;
        private PlayerController _playerController;
        
        private IUIManager _uiManager;

        private readonly InGamePagePresenterView _currentView;

        public InGamePagePresenter(PagePresentersView view, ResourceLoader resourceLoader) : base(view, resourceLoader)
        {
            _currentView = (InGamePagePresenterView)view;
        }

        [Inject]
        public void Init(DiContainer container)
        {
            _updateService = container.Resolve<UpdateService>();
            _playerController = container.Resolve<GameplayManager>().GetController<PlayerController>();
            _uiManager =  container.Resolve<IUIManager>();
        }

        public override void Show()
        {
            base.Show();
            _currentView.SetStatsView($"{_playerController.CurrentHealth}/{_playerController.MaxHealth}",
                _playerController.Attack.ToString(), _playerController.Defense.ToString(),
                _playerController.XpBoost.ToString(), _playerController.CurrentSpeed.ToString());

            var stats = ((PDRStatsInterface)_playerController.RetrievePlayerInfo(new PDCGetStatsInterface()))
                .PlayerModuleStats;
            foreach (var stat in stats)
            {
                stat.UpdateProperty += _currentView.UpdateStatView;
            }
            
            _currentView.ConnectActivationBackpack(ActivationBackpack);
        }

        public override void Hide()
        {
            base.Hide();
            var stats = ((PDRStatsInterface)_playerController.RetrievePlayerInfo(new PDCGetStatsInterface()))
                .PlayerModuleStats;
            foreach (var stat in stats)
            {
                stat.UpdateProperty += _currentView.UpdateStatView;
            }
            
            _currentView.UnconnectActivationBackpack(ActivationBackpack);
            Reset();
        }

        private void ActivationBackpack()
        {
            _uiManager.DrawPopup<BackpackPopup>();
        }

        public override void Reset()
        {
            base.Reset();
        }
    }
}