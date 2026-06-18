using Interfaces;
using Managers;
using Managers.Controller;
using Modules.Interfaces;
using Services;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Modules.Player
{
    public class CharacterMovementView : MonoBehaviour, IPlayerModule, IUpdatePlayerView
    {
        private JoystickService _joystickService;
        private CameraController _cameraController;
        private PlayerController _playerController;

        private PlayerPresenterView _presenterView;

        private float _moveSpeed => _playerController.CurrentSpeed;
        [SerializeField] private float _rotationSpeed = 10f;

        public void Init(DiContainer controller, PlayerPresenterView playerView)
        {
            _presenterView = playerView;
            _joystickService = controller.Resolve<JoystickService>();
            _cameraController = controller.Resolve<GameplayManager>().GetController<CameraController>();
            _playerController = controller.Resolve<GameplayManager>().GetController<PlayerController>();
        }

        public void Reset()
        {
        }

        public void UpdateCustom()
        {
            Vector3 inputDir = GetInputDirection();

            if (inputDir.sqrMagnitude < 0.01f)
                return;

            Move(inputDir);
            Rotate(inputDir);
        }

        private Vector3 GetInputDirection()
        {
            var (dir, magnitude) = _joystickService.GetJoystickDirectionAndMagnitude();

            Vector3 joystickDir = new Vector3(dir.x, 0, dir.z) * magnitude;

            Vector2 keyboard = Vector2.zero;

            if (Keyboard.current != null)
            {
                if (Keyboard.current.aKey.isPressed) keyboard.x -= 1;
                if (Keyboard.current.dKey.isPressed) keyboard.x += 1;
                if (Keyboard.current.sKey.isPressed) keyboard.y -= 1;
                if (Keyboard.current.wKey.isPressed) keyboard.y += 1;
            }

            Vector3 input = new Vector3(keyboard.x, 0, keyboard.y) + joystickDir;

            Transform cam = Camera.main.transform;

            Vector3 forward = cam.forward;
            Vector3 right = cam.right;

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            return forward * input.z + right * input.x;
        }

        private void Move(Vector3 direction)
        {
            transform.position += direction.normalized * _moveSpeed * Time.deltaTime;
        }

        private void Rotate(Vector3 direction)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);

            _presenterView.ModelTransform.rotation = Quaternion.Slerp(
                _presenterView.ModelTransform.rotation,
                targetRot,
                _rotationSpeed * Time.deltaTime
            );
        }
    }
}