using Core.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core
{
    public abstract class SimplePresenter<TP, TV> : IPresenter where TP : SimplePresenter<TP, TV> where TV : SimplePresenterView<TP, TV>
    {
        protected TV View { get; }
        public bool IsDestroyed { get; protected set; }
        public virtual bool IsActive => View.gameObject.activeSelf;

        protected readonly ResourceLoader ResourceLoader;

        protected SimplePresenter(TV view, ResourceLoader resourceLoader)
        {
            View = view;
            ((IView)View).SetPresenter((TP)this);
            View.Init();
            ResourceLoader = resourceLoader;
        }

        public void SetParent(Transform parent)
        {
            View.transform.SetParent(parent, false);
        }

        public virtual void SetActive(bool active)
        {
            View.SetActive(active);
        }

        public void Translate(Vector3 pos)
        {
            View.transform.Translate(pos);
        }

        public virtual void SetPositionAndRotation(Vector3 pos, Quaternion rot)
        {
            View.transform.SetPositionAndRotation(pos, rot);
        }

        public virtual void OnDestroy()
        {
            if (View != null)
                ResourceLoader.ReleaseInstance(View.gameObject);
        }
    }
}