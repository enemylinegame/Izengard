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
            Vector3 scale = transform.lossyScale;
            _sphereCollider.radius = radius / scale.x;
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