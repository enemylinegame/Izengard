using System;
using UnityEngine;

namespace Abstraction
{
    public class NoneTarget : IAttackTarget
    {
        public string Id { get; }
        public string Name { get; }
        public bool IsAlive => false;

        public Vector3 Position => Vector3.zero;

        public event Action<IDamage> OnTakeDamage;

        public void TakeDamage(IDamage damage) { }
    }
}
