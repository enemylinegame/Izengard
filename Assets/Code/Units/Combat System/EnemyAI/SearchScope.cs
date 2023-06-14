using System;
using UnityEngine;


namespace CombatSystem
{
    public class SearchScope : MonoBehaviour
    {
        public event Action<GameObject> OnTriger;


        private void OnTriggerStay(Collider other)
        {
            OnTriger?.Invoke(other.gameObject);
        }
    }
}