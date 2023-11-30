using System;
using UnityEngine;

namespace Abstraction
{
    public class NoneTarget : IAttackTarget
    {
        public int Id => -1;

        public bool IsAlive => false;

        public Vector3 Position => Vector3.zero;

        public event Action<IDamage> OnTakeDamage;

        public void TakeDamage(IDamage damage) { }
    }
}
