using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using Data;
using Managers.Interfaces;
using Settings;
using UIElements.Pages;
using UIElements.Popup;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class UIManager : IUIManager
    {
        private List<IUIElement> _uiPages;
        private List<IUIPopup> _uiPopups;

        private ResourceLoader _resourceLoader;
        private IGameDataManager _gameDataManager;

        private CancellationTokenSource _setPageCTS;
        private Canvas _canvas { get; set; }

        private bool _isUiCreate;
        public IUIElement CurrentPage { get; private set; }

        [Inject]
        public void Init(DiContainer container)
        {
            // _canvas = MainApp.Instance.Canvas;
            _resourceLoader = container.Resolve<ResourceLoader>();
            _gameDataManager = ProjectContext.Instance.Container.Resolve<IGameDataManager>();
            _setPageCTS = new CancellationTokenSource();

            _gameDataManager.DataDownload += CreateUI;

            _uiPages = new List<IUIElement>();

            _uiPopups = new List<IUIPopup>();

        }

        private void CreateUI()
        {
            CreateUI(_gameDataManager.GetDataScriptable<GameData>().UIScreenData.PagePrefabData).Forget();
        }

        private async UniTask CreateUI(PagePrefabData[] pagePrefabData)
        {
            Debug.Log($"CreateUI");
            for (int i = 0; i < pagePrefabData.Length; i++)
            {
                await CreatePage(pagePrefabData[i]);
            }
            
            _uiPopups.Add(await _resourceLoader.Instantiate<EndGamePopup>("Popups/EndGamePopup",_canvas.transform));
            _uiPopups.Add(await _resourceLoader.Instantiate<WinLevelPopup>("Popups/WinLevelPopup",_canvas.transform)); 
            _uiPopups.Add(await _resourceLoader.Instantiate<BackpackPopup>("Popups/BackpackPopup",_canvas.transform));
           
            HideAllPopups();

            HideAllPages();
            _isUiCreate = true;
        }

        private async UniTask SetPageAsync<T>()where T : IUIElement
        {
            await UniTask.WaitUntil(() => _isUiCreate,cancellationToken:_setPageCTS.Token);
            SetPage<T>();
        }

        private async UniTask CreatePage(PagePrefabData pagePrefabData)
        {
            var presenterType = Type.GetType(pagePrefabData.presenterType, true, true);

            _uiPages.Add(await _resourceLoader.Instantiate<PagePresenters, PagePresentersView>(_canvas.transform, pagePrefabData.locationSuffix));
        }

        public void SetMainCanvas(Canvas mainCanvas)
        {
            _canvas = mainCanvas;
        }

        public void ResetAll()
        {
            foreach (var page in _uiPages)
                page.Reset();

            foreach (var popup in _uiPopups)
                popup.Reset();
        }

        public T GetPage<T>() where T : IUIElement
        {
            IUIElement page = null;
            foreach (var _page in _uiPages.OfType<T>())
            {
                page = _page;
                break;
            }

            return (T)page;
        }

        public T GetPopup<T>() where T : IUIPopup
        {
            IUIPopup popup = null;
            foreach (var _popup in _uiPopups.OfType<T>())
            {
                popup = _popup;
                break;
            }

            return (T)popup;
        }

        public void SetPage<T>(bool hideAll = false) where T : IUIElement
        {
            if (!_isUiCreate)
            {
                _setPageCTS.Cancel();
                _setPageCTS = new CancellationTokenSource();
                SetPageAsync<T>().Forget();
                return;
            }
            
            if (hideAll)
            {
                HideAllPages();
            }
            else
            {
                CurrentPage?.Hide();
            }
            foreach (var _page in _uiPages.OfType<T>())
            {
                CurrentPage = _page;
                break;
            }

            CurrentPage.Show();
        }

        public void DrawPopup<T>() where T : IUIPopup
        {
            IUIPopup popup = null;
            foreach (var _popup in _uiPopups.OfType<T>())
            {
                popup = _popup;
                break;
            }

            popup.Show();
        }

        public void HidePopup<T>() where T : IUIPopup
        {
            foreach (var _popup in _uiPopups.OfType<T>())
            {
                _popup.Hide();
                break;
            }
        }

        public void HideAllPages()
        {
            foreach (var _page in _uiPages)
            {
                _page.Hide();
            }
        }

        public void HideAllPopups()
        {
            foreach (var _popup in _uiPopups)
            {
                _popup.Hide();
            }
        }
    }
}