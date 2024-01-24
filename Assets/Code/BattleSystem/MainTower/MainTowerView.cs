using Abstraction;
using System;
using Tools;
using UnityEditor;
using UnityEngine;

namespace BattleSystem.MainTower
{
    public class MainTowerView : MonoBehaviour, IAttackTarget
    {
        [SerializeField] private string _id;
        [SerializeField] private string _name = "MainTower";

        public string Id => _id;

        public string Name => _name;

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
            DebugGameManager.Log($"MainTower. Health value changed. Current Health = {hpValue}", 
                new[] { DebugTags.MainTower, DebugTags.Health });
        }

        public void TakeDamage(IDamage damage)
        {
            OnTakeDamage?.Invoke(damage);
        }
    }
}