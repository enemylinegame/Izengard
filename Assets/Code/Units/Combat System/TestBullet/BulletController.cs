using System;
using UnityEngine;


namespace CombatSystem
{
    public class BulletController : IBulletController
    {
        private const float COEFFICIENT = 0.9f;
        
        public event Action<IBulletController> BulletFlightIsOver;
        public GameObject Bullet { get; private set; }
        public Vector3 CurrentTarget => _destination;

        private readonly float _speed;
        private bool _isActive;
        private Vector3 _target;
        private Vector3 _startPosition;
        private Vector3 _destination;

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
                var distance = Vector3.Distance(_destination, _startPosition) - 
                    Vector3.Distance(_startPosition, Bullet.transform.position);
                if (distance < 0)
                {
                    _isActive = false;
                    BulletFlightIsOver?.Invoke(this);
                }
            }
        }

        public void StartFlight(Vector3 destination, Vector3 startPosition)
        {
            _startPosition = startPosition;
            _destination = destination;
            _target = _destination + Vector3.up *
                (Vector3.Distance(startPosition, _destination) * COEFFICIENT);
            _isActive = true;
            Bullet.transform.position = startPosition;
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(Bullet);
        }
    }
}