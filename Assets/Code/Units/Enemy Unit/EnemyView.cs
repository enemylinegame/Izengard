using System;
using CombatSystem.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyUnit
{
    public class EnemyView : MonoBehaviour, IDamageable
    {
        [SerializeField] private NavMeshAgent _navMesh;
        [SerializeField] private Animator _animator;

        public NavMeshAgent NavMesh => _navMesh;
        public Animator Animator => _animator;

        public event Action<int> OnTakeDamage;

        public void SetActive(bool state) 
        {
            gameObject.SetActive(state);
        }

        #region IDamageable

        public Vector3 Position => transform.position;

        public int ThreatLevel { get; private set; }

        public bool IsDead { get; private set; }

        public event Action OnDeath;
        public event Action<IDamageable> OnDamaged;

        public void MakeDamage(int damage, IDamageable damageDealer)
        {
            OnTakeDamage?.Invoke(damage);

            OnDamaged?.Invoke(damageDealer);
        }

        #endregion
    }
}
