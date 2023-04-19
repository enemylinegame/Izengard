using System;
using Controllers.NewOutPost;
using Controllers.OutPost;
using Data;
using Models.BaseUnit;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.NewOutPost
{
    /// <summary>
    /// Вьюха кнопки
    /// </summary>
    public class OutPostBtn : MonoBehaviour
    {
        [SerializeField] private Button _spawnButton;
        private OutPostPoint _point;
        public BaseUnitModel Model;
        public Action<OutPostPoint> spawnUnit;
        

        private void Start()
        {
            _spawnButton.onClick.AddListener(ButtonSpawnPressed);
        }

        private void ButtonSpawnPressed()
        {
            Model = new BaseUnitModel();
            spawnUnit?.Invoke(_point);
        }

        private void ButtonBackToSpawn()
        {
            
        }

        private void OnDestroy()
        {
            _spawnButton.onClick.RemoveAllListeners();
        }
    }
}