using BattleSystem.Buildings.Configs;
using BattleSystem.Buildings.Interfaces;
using UnityEngine;

namespace BattleSystem.Buildings.View
{
    public class DefendWallObstacleView : MonoBehaviour, IObstacleView
    {
        [SerializeField] private DefenWallConfig _config;

        public DefenWallConfig Config => _config;

        #region IWarBuildingView

        private int _id;

        public int Id => _id;

        public Vector3 Position => transform.localPosition;

        public void Init(int id)
        {
            _id = id;
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

        #endregion
    }
}
