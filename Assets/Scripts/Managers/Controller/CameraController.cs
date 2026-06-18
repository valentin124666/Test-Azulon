using Managers.Interfaces;
using Modules.Player.Commands;
using Services;
using UnityEngine;
using Zenject;

namespace Managers.Controller
{
    public class CameraController : IController
    {
        public bool IsActive;

        private PlayerController _playerController;
        private UpdateService updateService;

        private Camera _mainCamera;
        private Transform _cameraTransform;

        private Vector3 _velocity;

        private Vector3 _followOffset = new(0, 5, -8);
        private float _smoothTime = 0.2f;
        
        public Transform CameraTransform => _cameraTransform;

        public bool IsInit { get; private set; }

        public void Init(DiContainer container)
        {
            if (Camera.main != null)
            {
                _mainCamera = Camera.main;
                _cameraTransform = _mainCamera.transform;
            }

            var gameManager = container.Resolve<IGameplayManager>();
            _playerController = gameManager.GetController<PlayerController>();


            updateService = container.Resolve<UpdateService>();
            updateService.LateUpdateEvent += LateUpdate;

            IsInit = true;
        }

        private void LateUpdate()
        {
            if (!IsActive || _playerController == null)
                return;

            var cameraAnchor = (PDRCameraAnchor)_playerController.RetrievePlayerInfo(new PDCGetCameraAnchor());

            Vector3 targetPosition = cameraAnchor.followCamera.position;

            _cameraTransform.position = Vector3.SmoothDamp(
                _cameraTransform.position,
                targetPosition,
                ref _velocity,
                _smoothTime);
        }

        public void ActivationCameraTrack()
        {

            IsActive = true;

            if (_playerController == null || _cameraTransform == null)
                return;

            var cameraAnchor = (PDRCameraAnchor)_playerController.RetrievePlayerInfo(new PDCGetCameraAnchor());

            Vector3 targetPosition = cameraAnchor.followCamera.position;

            _cameraTransform.position = targetPosition;

            _cameraTransform.LookAt(cameraAnchor.lookAtCamera);
            _velocity = Vector3.zero;
        }

        public void ResetCamera()
        {
            _velocity = Vector3.zero;
        }
    }
}