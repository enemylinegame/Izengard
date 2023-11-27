using Abstraction;
using BattleSystem.Buildings.Interfaces;
using System;
using UnityEngine;

namespace BattleSystem.Buildings.View
{
    public class WarBuildingView : MonoBehaviour, IWarBuildingView
    {
        [SerializeField] private int _id;

        public int Id => _id;

        public Vector3 Position => transform.position;
        
        public event Action<IDamage> OnTakeDamage;

        private void Awake()
        {
            _id = Math.Abs(gameObject.GetInstanceID());
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void ChangeHealth(int hpValue)
        {
            Debug.Log("WarBuildingView->ChangeHealth: hpValue = " + hpValue.ToString());
        }

        public void TakeDamage(IDamage damage)
        {
            OnTakeDamage?.Invoke(damage);
        }
    }
}