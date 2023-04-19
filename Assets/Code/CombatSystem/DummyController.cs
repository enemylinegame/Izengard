using System;
using UnityEngine;


namespace CombatSystem
{
    public class DummyController : IDisposable
    {
        private readonly Damageable _dummy;

        private Transform _transform;

        public Transform TransformDummy => _transform;

        public DummyController(GameObject dummy)
        {
            _transform = dummy.transform;
            _dummy = dummy.GetComponent<Damageable>();
            _dummy.Init(100);
            _dummy.DeathAction += OnDead;
        }

        public void Spawn()
        {
            if (!_dummy.gameObject.activeInHierarchy)
            {
                _dummy.gameObject.SetActive(true);
                _dummy.Init((int)_dummy.MaxHealth);
            }
        }

        private void OnDead()
        {
            _dummy.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            _dummy.DeathAction -= OnDead;
            UnityEngine.Object.Destroy(_dummy.gameObject);
        }
    }
}