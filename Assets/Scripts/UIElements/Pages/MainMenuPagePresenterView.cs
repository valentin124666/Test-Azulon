using System;
using Core;
using UnityEngine.EventSystems;

namespace UIElements.Pages
{
    [OverrideTypes(typeof(MainMenuPagePresenter),typeof(MainMenuPagePresenterView))]
    public class MainMenuPagePresenterView : PagePresentersView , IPointerClickHandler
    {
        public event Action TapToStart;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            TapToStart?.Invoke();
        }
    }
}
