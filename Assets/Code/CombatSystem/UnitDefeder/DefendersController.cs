using System;
using System.Collections.Generic;
using Code.TileSystem;
using Code.UI;
using UnityEngine;


namespace CombatSystem
{
    public class DefendersController : IOnController, IOnUpdate, IDisposable
    {
        private float _radius = 1f;
        private List<DefenderUnit> _defenderUnits; 
        private GameObject _defenderPrefab;
        private TileController _tilecontroller;
        private UIController _uiConroller;

        private Vector3 _tempOffset = new Vector3(0.5f, 0, 0);

        public DefendersController(TileController tilecontroller,UIController uiConroller, GameObject defenderPrefab)
        {
            _defenderUnits = new List<DefenderUnit>();
            _tilecontroller = tilecontroller;
            _uiConroller = uiConroller;
            _uiConroller.BuildingsUIView.BuyDefender.onClick.AddListener(BuyDefenderClicked);
            _uiConroller.BuildingsUIView.EnterToBarracks.onClick.AddListener(EnterToBarracksClicked);
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

        private void EnterToBarracksClicked()
        {
            Debug.Log("DefendersController->EnterToBarracksClicked:");

            //TestUnitMovement();

            Building barrack = FindBarrack();

            if (barrack != null)
            {
                Vector3 barrackPosition = barrack.transform.position;
                for (int i = 0; i < _defenderUnits.Count; i++)
                {
                    DefenderUnit unit = _defenderUnits[i];
                    unit.GoToPosition(barrackPosition);
                }
            }
            else
            {
                Debug.Log("DefendersController->EnterToBarracksClicked:  The Tile don't have any barracks.");
            }

        }

        private Building FindBarrack()
        {
            Building barrack = null;

            TileView tileView = _tilecontroller.View;
            if (tileView != null)
            {
                var buildings = tileView.FloodedBuildings;

                foreach (var kvp in buildings)
                {
                    if ( kvp.Key.Type == BuildingTypes.Barrack)
                    {
                        barrack = kvp.Key;
                        break;
                    }
                }
            }

            return barrack;
        }

        private void TestUnitMovement()
        {
            for (int i = 0; i < _defenderUnits.Count; i++)
            {
                var unit = _defenderUnits[i];
                Vector3 position = unit.Position;
                position += _tempOffset;
                unit.GoToPosition(position);
            }
            _tempOffset *= -1;
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
            _uiConroller.BuildingsUIView.BuyDefender.onClick.RemoveListener(BuyDefenderClicked);
            _uiConroller.BuildingsUIView.EnterToBarracks.onClick.RemoveListener(EnterToBarracksClicked);
        }
    }
}