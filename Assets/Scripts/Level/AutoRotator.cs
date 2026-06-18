using DG.Tweening;
using UnityEngine;

namespace Level
{
    public class AutoRotator : MonoBehaviour
    {
        [SerializeField] private Vector3 directionRotation = new Vector3(0, 1, 0); // вісь обертання
        [SerializeField] private Transform objectTurn; // об’єкт для обертання
        [SerializeField] private float speed = 1f; // швидкість
        [SerializeField] private RotateMode rotateMode = RotateMode.FastBeyond360; // режим кручення
        [SerializeField] private LoopType loopType = LoopType.Restart; // режим зациклення

        private Tween rotationTween;
        private Transform targetTransform;

        private void Awake()
        {
            targetTransform = objectTurn != null ? objectTurn : transform;
        }

        private void Start()
        {
            StartTurn();
        }

        public void StartTurn()
        {
            rotationTween?.Kill(); // на всяк випадок зупиняємо попередній твін

            rotationTween = targetTransform
                .DOLocalRotate(directionRotation * 360f, 1f / speed, rotateMode) // один поворот
                .SetEase(Ease.Linear)
                .SetLoops(-1, loopType); // зациклення
        }

        public void StopTurn()
        {
            rotationTween?.Kill();
        }

        public void PauseTurn()
        {
            rotationTween?.Pause();
        }

        public void ResumeTurn()
        {
            rotationTween?.Play();
        }

        public void ResetTurn()
        {
            StopTurn();
            targetTransform.localRotation = Quaternion.identity;
        }

        public void RestartTurn()
        {
            ResetTurn();
            StartTurn();
        }
    }
}
