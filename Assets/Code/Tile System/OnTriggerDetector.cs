using System;
using UnityEngine;

namespace Code.TileSystem
{
    public class OnTriggerDetector : MonoBehaviour
    {
        
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"OnTriggerDetector->OnTriggerEnter: tr = {transform.name}; other = " +
                      $"{other.gameObject.name}; other layer = {other.gameObject.layer}");
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log($"OnTriggerDetector->OnTriggerExit: tr = {transform.name}; other = " +
                      $"{other.gameObject.name}; other layer = {other.gameObject.layer}");
        }
    }
}