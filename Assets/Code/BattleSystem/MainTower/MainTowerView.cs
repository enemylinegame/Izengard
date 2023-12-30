using Abstraction;
using System;
using UnityEditor;
using UnityEngine;

namespace BattleSystem.MainTower
{
    public class MainTowerView : MonoBehaviour, IAttackTarget
    {
        [SerializeField] private string _id;

        public string Id => _id;

        public Vector3 Position => transform.position;
        
        public event Action<IDamage> OnTakeDamage;

        private void Awake()
        {
            _id = GUID.Generate().ToString();
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