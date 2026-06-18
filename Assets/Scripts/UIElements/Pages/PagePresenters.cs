using Core;
using Managers.Interfaces;

namespace UIElements.Pages
{
    public class PagePresenters : SimpleUIPresenter<PagePresenters,PagePresentersView>,IUIElement
    {
        public PagePresenters(PagePresentersView view, ResourceLoader resourceLoader) : base(view, resourceLoader)
        {
        }

        public virtual void Reset()
        {
            
        }
    }
}
