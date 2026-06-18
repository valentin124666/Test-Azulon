using Managers.Interfaces;
using UnityEngine;
using Zenject;

namespace Services
{
    public class UIContext : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;

        [Inject]
        public void Init(DiContainer container)
        {
            container.Resolve<IUIManager>().SetMainCanvas(canvas);
        }
    }
}
