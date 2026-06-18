using System;
using UnityEngine;

namespace Level
{
    [Serializable]
    public  class ParticleSystemGroup
    {
        public bool isPlaying;
        public ParticleSystem[] FBX;

        public void Play()
        {
            isPlaying = true;
            foreach (var item in FBX)
            {
                item.Play();
            }
        }

        public void Stop()
        {
            isPlaying = false;

            foreach (var item in FBX)
            {
                item.Stop();
            }
        }
    }
}
