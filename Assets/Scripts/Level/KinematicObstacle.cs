using UnityEngine;

namespace Level
{
    public class KinematicObstacle : MonoBehaviour, IObstacle
    {
        [SerializeField] private float damage  = 10 ;

        public float GetDamage() => damage;

    }
}
