using System;
using Core;
using Cysharp.Threading.Tasks;
using Data;
using Managers.Interfaces;
using UnityEngine;

using UnityEngine.InputSystem;
using Tools;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using UnityEngine;
using Zenject;

namespace Services
{
    public class JoystickService
    {
        public event Action BeganTouch;
        public event Action EndTouch;

        private UpdateService _updateService;
        private JoystickData _joystickData;
        private IGameDataManager _gameDataManager;

        private Vector3 _touchStartPos;
        private Vector3 _touchCurrentPos;
        private Vector3 _joystickDirection;

        public bool IsTouch { get; private set; }
        public bool IsInit { get; private set; }

        [Inject]
        public void Init(DiContainer container)
        {
            _gameDataManager = container.Resolve<IGameDataManager>();

            _gameDataManager.DataDownload += () =>
            {
                _joystickData = _gameDataManager.GetDataScriptable<GameData>().JoystickData;

                // Debug.Log(_joystickData.JoystickSensitivity);
                // Debug.Log(_joystickData.JoystickMaxMagnitude);
                // Debug.Log(_joystickData.MaxDistansStick);
            };
            _updateService = container.Resolve<UpdateService>();

            _updateService.UpdateEvent += Update;
            IsInit = true;
        }

        private void Update()
        {
            if (TouchUtility.TouchCount > 0 && !InternalTools.IsPointerOverUIObject())
            {
                var touch = TouchUtility.GetTouch(0);

                Vector2 currentPos = Pointer.current.position.ReadValue();

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        IsTouch = true;
                        BeganTouch?.Invoke();

                        _touchStartPos = currentPos;
                        _touchCurrentPos = currentPos;
                        break;

                    case TouchPhase.Moved:
                        _touchCurrentPos = currentPos;

                        Vector2 delta = _touchCurrentPos - _touchStartPos;

                        float maxDist = _joystickData.maxDistanceStick;

                        Vector2 clamped = Vector2.ClampMagnitude(delta, maxDist);

                        Vector2 normalized = clamped / maxDist;

                        if (normalized.magnitude < 0.2f)
                        {
                            _joystickDirection = Vector3.zero;
                        }
                        else
                        {
                            _joystickDirection = new Vector3(normalized.x, 0, normalized.y);
                        }

                        break;

                    case TouchPhase.Ended:
                        IsTouch = false;
                        EndTouch?.Invoke();

                        _joystickDirection = Vector3.zero;
                        _touchStartPos = Vector3.zero;
                        _touchCurrentPos = Vector3.zero;
                        break;
                }
            }
            else if (IsTouch)
            {
                IsTouch = false;
                EndTouch?.Invoke();
                _joystickDirection = Vector3.zero;
            }
        }

        public (Vector3, float) GetJoystickDirectionAndMagnitude()
        {
            float rawDeltaX = _touchCurrentPos.x - _touchStartPos.x;
     
            float adjustedMagnitude = Mathf.Abs(rawDeltaX) * _joystickData.joystickSensitivity;

            if (adjustedMagnitude > _joystickData.joystickMaxMagnitude)
                adjustedMagnitude = _joystickData.joystickMaxMagnitude;

            return (_joystickDirection, adjustedMagnitude);
        }
        public (Vector3, float) GetJoystickDirectionAndNormalizedMagnitude()
        {
            float rawDeltaX = _touchCurrentPos.x - _touchStartPos.x;
     
            float adjustedMagnitude = Mathf.Abs(rawDeltaX) * _joystickData.joystickSensitivity /_joystickData.joystickMaxMagnitude;

            if (adjustedMagnitude > 1)
                adjustedMagnitude = 1;

            return (_joystickDirection, adjustedMagnitude);
        }
    }
}