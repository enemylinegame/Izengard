
using System;
using System.Collections.Generic;
using Code.TileSystem;
using Code.UI;
using UnityEngine;
using UnityEngine.AI;

namespace CombatSystem
{
    public class DefendersController : IOnController, IOnUpdate, IDisposable
    {
        private float _radius = 1f;
        private List<DefenderUnit> _defenderUnits; 
        private GameObject _defenderPrefab;
        private TileController _tilecontroller;
        private NavMeshAgent _navMeshAgent;
        private UIController _uiConroller;

        public DefendersController(TileController tilecontroller,UIController _uiConroller, GameObject defenderPrefab)
        {
            _defenderUnits = new List<DefenderUnit>();
            _tilecontroller = tilecontroller;
            _uiConroller.BuildingsUIView.BuyDefender.onClick.AddListener(BuyDefenderClicked);
            _defenderPrefab = defenderPrefab;
        }

        private void BuyDefenderClicked()
        {
            GameObject go = GameObject.Instantiate(_defenderPrefab,new Vector3(200f,0f,200f) ,Quaternion.identity);
            Vector3 position = UnityEngine.Random.insideUnitSphere * _radius;
            position.y = 0f;
            position += _tilecontroller.View.transform.position;
            DefenderUnit defender = new DefenderUnit(go, position);
            _defenderUnits.Add(defender);
            defender.DefenderUnitDead += DefenderDead;

        }

        private void DefenderDead(DefenderUnit defender)
        {
            defender.DefenderUnitDead -= DefenderDead;
            _defenderUnits.Remove(defender);
            GameObject.Destroy(defender.DefenderGameObject);
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (DefenderUnit unit in _defenderUnits)
            {
                unit.OnUpdate(deltaTime);
            }
        }

        public void Dispose()
        {
            _tilecontroller.BuildingsUIView.BuyDefender.onClick.RemoveListener(BuyDefenderClicked);
        }
    }
}

