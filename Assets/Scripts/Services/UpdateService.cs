using System;
using UnityEngine;

namespace Services
{
    public class UpdateService : MonoBehaviour
    {
        public event Action LateUpdateEvent;
        public event Action FixedUpdateEvent;
        public event Action UpdateEvent;

        private void LateUpdate()
        {
            LateUpdateEvent?.Invoke();
        }

        private void FixedUpdate()
        {
            FixedUpdateEvent?.Invoke();
        }

        private void Update()
        {
            UpdateEvent?.Invoke();
        }
    }
}