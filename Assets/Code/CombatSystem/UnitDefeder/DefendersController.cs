using System;
using System.Collections.Generic;
using Code.TileSystem;
using Code.UI;
using UnityEngine;


namespace CombatSystem
{
    public class DefendersController : IOnController, IOnUpdate, IDisposable
    {

        private const string NO_BARRACKS_MESSAGE = "The Tile don't have any barracks.";
        private const string MAX_UNITS_MESSAGE = "Maximum number of units reached.";

        private const int TEXT_MASSAGES_DURATION_MILISEC = 3000;
        private const int MAX_DEFENDER_UNITS = 5;

        private float _radius = 1f;
        private List<DefenderUnit> _defenderUnits; 
        private GameObject _defenderPrefab;
        private TileController _tilecontroller;
        private UIController _uiConroller;

        private Vector3 _tempOffset = new Vector3(0.5f, 0, 0);

        private bool _isDefendersInsideBarrack;


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
            if (_defenderUnits.Count < MAX_DEFENDER_UNITS)
            {
                GameObject go = GameObject.Instantiate(_defenderPrefab, new Vector3(200f, 0f, 200f), Quaternion.identity);
                Vector3 position = UnityEngine.Random.insideUnitSphere * _radius;
                position.y = 0f;
                position += _tilecontroller.View.transform.position;
                DefenderUnit defender = new DefenderUnit(go, position);
                _defenderUnits.Add(defender);
                defender.DefenderUnitDead += DefenderDead;
            }
            else
            {
                Debug.Log("DefendersController->EnterToBarracksClicked: " + MAX_UNITS_MESSAGE);
                _tilecontroller.CenterText.NotificationUI(MAX_UNITS_MESSAGE, TEXT_MASSAGES_DURATION_MILISEC);
            }
        }

        private void EnterToBarracksClicked()
        {
            Debug.Log("DefendersController->EnterToBarracksClicked:");

            //TestUnitMovement();

            if (_isDefendersInsideBarrack)
            {
                KickDefendersOutOfBarrack();
                _isDefendersInsideBarrack = false;
            }
            else
            {
                //_isDefendersInsideBarrack = SentDefendersToBarrack();
                _isDefendersInsideBarrack = SendDefendersToCentralBuilding();
            }

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

        private bool SentDefendersToBarrack()
        {
            bool isUnitsSended = false;
            Building barrack = FindBarrack();

            if (barrack != null)
            {
                SendDefendersIntoBuilding(barrack.transform.position);
                isUnitsSended = true;
            }
            else
            {
                Debug.Log("DefendersController->EnterToBarracksClicked: " + NO_BARRACKS_MESSAGE);
                _tilecontroller.CenterText.NotificationUI(NO_BARRACKS_MESSAGE, TEXT_MASSAGES_DURATION_MILISEC);
            }

            return isUnitsSended;
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
                    if (kvp.Key.Type == BuildingTypes.Barrack)
                    {
                        barrack = kvp.Key;
                        break;
                    }
                }
            }

            return barrack;
        }

        private bool SendDefendersToCentralBuilding()
        {
            bool isUnitsSended = false;

            TileView tileView = _tilecontroller.View;
            if (tileView != null)
            {
                SendDefendersIntoBuilding(tileView.transform.position);
                isUnitsSended = true;
            }

            return isUnitsSended;
        }

        private void SendDefendersIntoBuilding(Vector3 buildingPosition)
        {
            for (int i = 0; i < _defenderUnits.Count; i++)
            {
                DefenderUnit unit = _defenderUnits[i];
                unit.GoToPosition(buildingPosition);
                unit.OnDestinationReached += OnUnitReachedBarrack;
            }
        }

        private void OnUnitReachedBarrack(DefenderUnit unit)
        {
            unit.OnDestinationReached -= OnUnitReachedBarrack;
            if (_isDefendersInsideBarrack)
            {
                unit.IsEnabled = false;
            }
        }

        private void KickDefendersOutOfBarrack()
        {
            for (int i = 0; i < _defenderUnits.Count; i++)
            {
                DefenderUnit unit = _defenderUnits[i];
                unit.OnDestinationReached -= OnUnitReachedBarrack;
                unit.IsEnabled = true;
                SendDefenderToTilePosition(unit);
            }
        }

        private void SendDefenderToTilePosition(DefenderUnit unit)
        {

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