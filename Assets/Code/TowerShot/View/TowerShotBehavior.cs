using System;
using UnityEngine;

namespace Code.TowerShot
{
    public class TowerShotBehavior : MonoBehaviour
    {
        //public SphereCollider TurretTrigger;
        public BoxCollider TurretTrigger;

        public Action<Collider> Trigger;
        public Action<Collider> TriggerExit; 

        [SerializeField] private Transform[] _bulletPoint; // точки, откуда ведется стрельба
        [SerializeField] private Transform _turretRotation; // объект вращения, башня турели
        [SerializeField] private Transform _center; // центр между пушками, для поискового луча

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                Trigger?.Invoke(other);
            }
        }private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                TriggerExit?.Invoke(other);
            }
        }

        public Transform[] BulletPoint => _bulletPoint;
        public Transform TurretRotation => _turretRotation;
        public Transform Center => _center;
    }
}