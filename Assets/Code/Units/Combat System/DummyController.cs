using System;
using UnityEngine;


namespace CombatSystem
{
    public class DummyController : IDisposable
    {
        private readonly Damageable _dummy;


        public DummyController(GameObject dummy)
        {
            _dummy = dummy.GetComponent<Damageable>();
            _dummy.Init(100, 5);
            _dummy.OnDeath += OnDead;
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
            _dummy.OnDeath -= OnDead;
            UnityEngine.Object.Destroy(_dummy.gameObject);
        }
    }
}