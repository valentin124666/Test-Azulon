namespace Core
{
    public abstract class SimpleUIPresenter<TP, TV> : SimplePresenter<TP, TV> where TP : SimpleUIPresenter<TP, TV> where TV : SimpleUIPresenterView<TP, TV>
    {
        public SimpleUIPresenter(TV view, ResourceLoader resourceLoader) : base(view, resourceLoader)
        {
        }
        public virtual void Show()
        {
            View.SetActive(true);
        }

        public virtual void Hide()
        {
            View.SetActive(false);
        }
    }
}