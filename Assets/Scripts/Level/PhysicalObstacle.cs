using UnityEngine;

namespace Level
{
    public class PhysicalObstacle : MonoBehaviour , IObstacle
    {
        [SerializeField] private Rigidbody rb;

        [SerializeField] private float forceMultiplier = 10f;
        [SerializeField] private float damage  = 10 ;

        private void Awake()
        {
            // Вимикаємо Rigidbody спочатку
            if (rb != null)
            {
                rb.isKinematic = true;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<PhysicalObstacle>(out var obstacle))
            {
                obstacle.AddForce(rb.linearVelocity);
            }
        }

        public void AddForce(Vector3 direction)
        {
            if (rb == null) return;

            // Увімкнути фізику
            rb.isKinematic = false;

            // Очистити попередню швидкість (опціонально)
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // Додати силу
            rb.AddForce(direction.normalized * forceMultiplier, ForceMode.Impulse);
            Destroy(gameObject, 5);
        }

        public float GetDamage() => damage;

    }
}