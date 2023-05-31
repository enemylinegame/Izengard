using System;
using UnityEngine;


namespace CombatSystem
{
    public class SearchScope : MonoBehaviour
    {
        public event Action<GameObject> OnEnter;
        public event Action<GameObject> OnExit;

        private SphereCollider _sphereCollider;

        private void Awake()
        {
            _sphereCollider = GetComponent<SphereCollider>();
        }


        public void SetRadius(float radius)
        {
            _sphereCollider.radius = radius;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            OnEnter?.Invoke(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            OnExit?.Invoke(other.gameObject);
        }
    }
}