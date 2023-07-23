using System;
using UnityEngine;

namespace Code.TileSystem
{
    public class OnTriggerDetector : MonoBehaviour
    {

        public Action<GameObject> OnTriggerContactEnter;
        public Action<GameObject> OnTriggerContactExit;

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log($"OnTriggerDetector->OnTriggerEnter: tr = {transform.name}; other = " +
            //          $"{other.gameObject.name}; other layer = {other.gameObject.layer}");
            OnTriggerContactEnter?.Invoke(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            //Debug.Log($"OnTriggerDetector->OnTriggerExit: tr = {transform.name}; other = " +
            //          $"{other.gameObject.name}; other layer = {other.gameObject.layer}");
            OnTriggerContactExit?.Invoke(other.gameObject);
        }
    }
}