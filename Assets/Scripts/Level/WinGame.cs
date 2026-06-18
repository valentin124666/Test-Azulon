using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class WinGame : MonoBehaviour
    {
        [SerializeField] private ParticleSystemGroup finishFBX;

        public void ActivationFinishFBX()
        {
            finishFBX?.Play();
        }

        public void ResetFBX()
        {
            finishFBX?.Stop();
        }

    }
}
