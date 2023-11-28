using Abstraction;
using System;
using UnityEngine;

namespace BattleSystem.Obstacle
{
    public class DefendWallObstacleView : MonoBehaviour, IObstacleView
    {
        [SerializeField] private DefenWallConfig _config;

        private int _id;

        public DefenWallConfig Config => _config;

        public int Id => _id;

        public Vector3 Position => transform.localPosition;
        
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
            Debug.Log("DefendWallObstacleView->ChangeHealth: hpValue = " + hpValue.ToString());
        }

        public void TakeDamage(IDamage damage)
        {
            OnTakeDamage?.Invoke(damage);
        }
    }
}
