using Abstraction;
using BattleSystem.Buildings.Interfaces;
using UnityEngine;

namespace BattleSystem.Buildings.View
{
    public class WarBuildingView : MonoBehaviour, IWarBuildingView, ITarget
    {

        private int _id;

        
        public int Id => _id;

        
        #region IWarBuildingView

        public Vector3 Position => transform.position;
        
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

        public void ChangeHealth(int hpValue) { }
        
        #endregion

    }
}