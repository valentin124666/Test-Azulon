using Core;
using Managers.Interfaces;
using Zenject;

namespace UIElements.Pages
{
    public class MainMenuPagePresenter : PagePresenters
    {
        private IGameplayManager _gameplayManager;

        public MainMenuPagePresenter(PagePresentersView view, ResourceLoader resourceLoader) : base(view, resourceLoader)
        {
            ((MainMenuPagePresenterView)view).TapToStart += StartGamePlay;
        }

        [Inject]
        public void Init(DiContainer container)
        {
            _gameplayManager = container.Resolve<IGameplayManager>();
        }

        private void StartGamePlay()
        {
            _gameplayManager.StartScenario();
        }
    }
}