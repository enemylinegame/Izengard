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

        private const float POSITION_RADIUS = 1f;

        private const int TEXT_MASSAGES_DURATION_MILISEC = 3000;
        private const int MAX_DEFENDER_UNITS = 5;

        private List<DefenderUnit> _defenderUnits; 
        private Dictionary<TileView, TileDefendersSquad> _defendersSquadron;
        private GameObject _defenderPrefab;
        private TileController _tilecontroller;
        private UIController _uiConroller;


        public DefendersController(TileController tilecontroller,UIController uiConroller, GameObject defenderPrefab)
        {
            _defenderUnits = new List<DefenderUnit>();
            _defendersSquadron = new Dictionary<TileView, TileDefendersSquad>();
            _tilecontroller = tilecontroller;
            _uiConroller = uiConroller;
            _uiConroller.BuildingsUIView.BuyDefender.onClick.AddListener(BuyDefenderClicked);
            _uiConroller.BuildingsUIView.EnterToBarracks.onClick.AddListener(EnterToBarracksClicked);
            _defenderPrefab = defenderPrefab;
        }

        private void BuyDefenderClicked()
        {
            TileDefendersSquad squad = SelectSquad();
            if (squad != null)
            {
                if ( squad.DefenderUnits.Count < MAX_DEFENDER_UNITS)
                {
                    GameObject go = GameObject.Instantiate(_defenderPrefab, new Vector3(200f, 0f, 200f), Quaternion.identity);
                    Vector3 position = GeneratePositionNearTileCentre(_tilecontroller.View);
                    DefenderUnit defender = new DefenderUnit(go, position);
                    AddDefenderToSquad(squad, defender);
                    defender.DefenderUnitDead += DefenderDead;
                    _defenderUnits.Add(defender);
                }
                else
                {
                    Debug.Log("DefendersController->EnterToBarracksClicked: " + MAX_UNITS_MESSAGE);
                    _tilecontroller.CenterText.NotificationUI(MAX_UNITS_MESSAGE, TEXT_MASSAGES_DURATION_MILISEC);
                }
            }
        }

        private Vector3 GeneratePositionNearTileCentre(TileView tile)
        {
            Vector3 position = UnityEngine.Random.insideUnitSphere * POSITION_RADIUS;
            position.y = 0f;
            return  position + tile.transform.position;
        }

        private TileDefendersSquad SelectSquad()
        {
            TileDefendersSquad squad = null;

            TileView tileView = _tilecontroller.View;
            if (tileView != null)
            {
                if (!_defendersSquadron.ContainsKey(tileView))
                {
                    squad = new TileDefendersSquad(tileView);
                    _defendersSquadron.Add(tileView, squad);
                }
                else
                {
                    squad = _defendersSquadron[tileView];
                }
            }
            return squad;
        }

        private bool AddDefenderToSquad(TileDefendersSquad squad, DefenderUnit defenderUnit)
        {
            bool isAdded = false;

            if (defenderUnit.Squad == null)
            {
                if (squad.DefenderUnits.Count < MAX_DEFENDER_UNITS)
                {
                    squad.DefenderUnits.Add(defenderUnit);
                    defenderUnit.Squad = squad;
                }
            }

            return isAdded;
        }

        private bool RemovDefenderFromSquad(TileDefendersSquad squad, DefenderUnit defenderUnit)
        {
            bool isRemoved = false;

            if (defenderUnit.Squad == squad)
            {
                if (squad.DefenderUnits.Remove(defenderUnit))
                {
                    defenderUnit.Squad = null;
                }
            }
            return isRemoved;
        }

        private void EnterToBarracksClicked()
        {
            Debug.Log("DefendersController->EnterToBarracksClicked:");

            TileDefendersSquad squad = SelectSquad();
            if (squad != null)
            {
                if (squad.IsDefendersInside)
                {
                    KickDefendersOutOfBarrack(squad);
                    squad.IsDefendersInside = false;
                }
                else
                {
                    squad.IsDefendersInside = SendDefendersToBarrack(squad);
                }
            }
        }

        private bool SendDefendersToBarrack(TileDefendersSquad squad)
        {
            bool isUnitsSended = false;

            List<DefenderUnit> defenderUnits = squad.DefenderUnits;

            if (defenderUnits.Count > 0)
            {
                Vector3 buildingPosition = squad.View.transform.position;
                for (int i = 0; i < defenderUnits.Count; i++)
                {
                    DefenderUnit unit = defenderUnits[i];
                    unit.GoToPosition(buildingPosition);
                    unit.OnDestinationReached += OnUnitReachedBarrack;
                }

                isUnitsSended = true;
            }
            return isUnitsSended;
        }

        private void OnUnitReachedBarrack(DefenderUnit unit)
        {
            unit.OnDestinationReached -= OnUnitReachedBarrack;
            unit.IsEnabled = false;
        }

        private void KickDefendersOutOfBarrack(TileDefendersSquad squad)
        {
            List<DefenderUnit> defenderUnits = squad.DefenderUnits;


            for (int i = 0; i < defenderUnits.Count; i++)
            {
                DefenderUnit unit = defenderUnits[i];
                unit.OnDestinationReached -= OnUnitReachedBarrack;
                unit.IsEnabled = true;
                SendDefenderToTilePosition(unit, squad.View);
            }
        }

        private void SendDefenderToTilePosition(DefenderUnit unit, TileView tile)
        {
            Vector3 position = GeneratePositionNearTileCentre(tile);
            unit.GoToPosition(position);
        }

        private void DefenderDead(DefenderUnit defender)
        {
            defender.DefenderUnitDead -= DefenderDead;
            _defenderUnits.Remove(defender);
            RemovDefenderFromSquad(defender.Squad, defender);
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