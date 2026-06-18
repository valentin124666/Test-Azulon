
using UnityEngine;

namespace Level
{
    public interface ILevel
    {
        void SetActive(bool active);

        void OnDestroy();
    }
}
