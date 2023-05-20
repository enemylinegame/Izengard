using System;
using System.Collections.Generic;
using Code.TileSystem;
using Code.UI;
using UnityEngine;


namespace CombatSystem
{
    public class DefendersController : IOnController, IOnUpdate, IDefendersControll
    {
        private const float POSITION_RADIUS = 1f;

        private List<DefenderUnit> _defenderUnits;
        private GameObject _defenderPrefab;
        private TileController _tilecontroller;
        private UIController _uiConroller;
        private readonly Vector3 _unitsSpawnPosition = new Vector3(200f, 0f, 200f);


        public DefendersController(TileController tilecontroller, UIController uiConroller, GameObject defenderPrefab)
        {
            _defenderUnits = new List<DefenderUnit>();
            _tilecontroller = tilecontroller;
            _uiConroller = uiConroller;
            _defenderPrefab = defenderPrefab;
        }

        public DefenderUnit CreateDefender(TileView tile)
        {
            GameObject go = GameObject.Instantiate(_defenderPrefab, _unitsSpawnPosition, Quaternion.identity);
            Vector3 position = GeneratePositionNearTileCentre(tile);
            DefenderUnit defender = new DefenderUnit(go, position);
            defender.DefenderUnitDead += DefenderDead;
            _defenderUnits.Add(defender);
            return defender;
        }

        private Vector3 GeneratePositionNearTileCentre(TileView tile)
        {
            Vector3 position = UnityEngine.Random.insideUnitSphere * POSITION_RADIUS;
            position.y = 0f;
            return position + tile.transform.position;
        }

        public void SendDefendersToBarrack(List<DefenderUnit> defenderUnits, TileView tile)
        {
            if (defenderUnits.Count > 0)
            {
                Vector3 buildingPosition = tile.transform.position;
                for (int i = 0; i < defenderUnits.Count; i++)
                {
                    DefenderUnit unit = defenderUnits[i];
                    if (unit.IsInsideBarrack == false)
                    {
                        unit.GoToPosition(buildingPosition);
                        unit.OnDestinationReached += OnUnitReachedBarrack;
                    }
                }
            }
        }

        public void SendDefenderToBarrack(DefenderUnit unit, TileView tile)
        {
            if (unit.IsInsideBarrack == false)
            {
                Vector3 buildingPosition = tile.transform.position;
                unit.GoToPosition(buildingPosition);
                unit.OnDestinationReached += OnUnitReachedBarrack;
            }
        }

        private void OnUnitReachedBarrack(DefenderUnit unit)
        {
            unit.OnDestinationReached -= OnUnitReachedBarrack;
            unit.IsInsideBarrack = true;
        }

        public void KickDefendersOutOfBarrack(List<DefenderUnit> defenderUnits, TileView tile)
        {
            for (int i = 0; i < defenderUnits.Count; i++)
            {
                DefenderUnit unit = defenderUnits[i];
                KickDefenderOutOfBarrack(unit, tile);
            }
        }

        public void KickDefenderOutOfBarrack(DefenderUnit unit, TileView tile)
        {
            unit.OnDestinationReached -= OnUnitReachedBarrack;
            unit.IsInsideBarrack = false;
            SendDefenderToTilePosition(unit, tile);
        }

        public void DismissDefender(DefenderUnit unit)
        {
            throw new NotImplementedException();
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
            GameObject.Destroy(defender.DefenderGameObject);
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (DefenderUnit unit in _defenderUnits)
            {
                unit.OnUpdate(deltaTime);
            }
        }

    }
}