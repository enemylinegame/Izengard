using Abstraction;
using System;
using UnityEditor;
using UnityEngine;

namespace BattleSystem.Obstacle
{
    public class DefendWallObstacleView : MonoBehaviour, IObstacleView
    {
        [SerializeField] private DefenWallConfig _config;

        private string _id;
        private string _name;

        public DefenWallConfig Config => _config;

        public string Id => _id;
        public string Name => _name;
        public Vector3 Position => transform.localPosition;


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
            Debug.Log("DefendWallObstacleView->ChangeHealth: hpValue = " + hpValue.ToString());
        }

        public void TakeDamage(IDamage damage)
        {
            OnTakeDamage?.Invoke(damage);
        }
    }
}
