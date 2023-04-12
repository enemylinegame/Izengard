using System;
using UnityEngine;


namespace CombatSystem
{
    public class BulletController : IBulletController
    {
        private const float COEFFICIENT = 0.9f;
        
        public event Action<IBulletController> BulletFlightIsOver;
        public GameObject Bullet { get; private set; }
        public Damageable CurrentTarget { get; private set; }

        private readonly float _speed;
        private bool _isActive;
        private Vector3 _target;
        private Vector3 _startPosition;


        public BulletController(float speed)
        {
            _speed = speed;
            Bullet = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("TestBullet"));
        }

        public void OnFixedUpdate(float fixedDeltaTime)
        {
            if (_isActive)
            {
                Bullet.transform.rotation = Quaternion.LookRotation((_target - _startPosition).normalized);
                Bullet.transform.Translate(_speed * fixedDeltaTime * Vector3.forward, Space.Self);
                _target += Vector3.down * (_speed * fixedDeltaTime * COEFFICIENT) * 1.5f;
                var distance = Vector3.Distance(CurrentTarget.transform.position, _startPosition) - 
                    Vector3.Distance(_startPosition, Bullet.transform.position);
                if (distance < 0)
                {
                    _isActive = false;
                    BulletFlightIsOver?.Invoke(this);
                }
            }
        }

        public void StartFlight(Damageable target, Vector3 startPosition)
        {
            _startPosition = startPosition;
            _target = target.gameObject.transform.position + Vector3.up *
                (Vector3.Distance(startPosition, target.transform.position) * COEFFICIENT);
            _isActive = true;
            CurrentTarget = target;
            Bullet.transform.position = startPosition;
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(Bullet);
        }
    }
}