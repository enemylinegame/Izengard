using System;
using EnemyUnit.Interfaces;
using UnityEngine;

namespace EnemyUnit
{
    public class StubEnemyController : IEnemyController
    {
        public int Index { get; } = -1;

        public event Action<int> OnDeath;

        public StubEnemyController()
        {
            Debug.LogWarning("StubEnemyController was created!");
        }

        public void Dispose() { }

        public void OnFixedUpdate(float fixedDeltaTime) { }

        public void OnUpdate(float deltaTime) { }
        public void SetIndex(int index) { }
    }
}
