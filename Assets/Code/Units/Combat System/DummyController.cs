using System;
using UnityEngine;


namespace CombatSystem
{
    public class DummyController : IDisposable
    {
        private readonly Damageable _dummy;
        private readonly Renderer _dummyRenderer;

        public Damageable Dummy => _dummy;
        public DummyController(GameObject dummy)
        {
            _dummy = dummy.GetComponent<Damageable>();
            _dummyRenderer = dummy.GetComponent<Renderer>();
            _dummy.Init(1000, 5);
            _dummy.OnDeath += OnDead;
        }

        public void Spawn(int MaxHealth) 
        {
            _dummy.gameObject.SetActive(true);
            _dummy.Init(MaxHealth);
            _dummyRenderer.material.color = Color.white;
        }

        private void OnDead()
        {
            //_dummy.gameObject.SetActive(false);
            _dummyRenderer.material.color = Color.grey;
        }

        public void Dispose()
        {
            _dummy.OnDeath -= OnDead;
            UnityEngine.Object.Destroy(_dummy.gameObject);
        }
    }
}