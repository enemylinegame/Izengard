using System;
using System.Collections.Generic;
using Code.TileSystem;
using Code.UI;
using UnityEngine;


namespace CombatSystem
{
    public class DefendersController : IOnController, IOnUpdate, IDefendersControll
    {
        private const string DEFENDERS_GO_ROOT_NAME = "DefenderUnitsRoot";
        private const float POSITION_RADIUS = 1f;

        private List<DefenderUnit> _defenderUnits;
        private readonly Vector3 _unitsSpawnPosition = new Vector3(200f, 0f, 200f);
        private readonly IBulletsController _bulletsController;
        private Transform _defendersRoot;

        public DefendersController(IBulletsController bulletsController)
        {
            _defenderUnits = new List<DefenderUnit>();
            _bulletsController = bulletsController;
            _defendersRoot = new GameObject(DEFENDERS_GO_ROOT_NAME).transform;
        }

        public DefenderUnit CreateDefender(TileView tile, DefenderSettings settings)
        {
            GameObject go = GameObject.Instantiate(settings.Prefab,_unitsSpawnPosition, Quaternion.identity, 
                _defendersRoot);
            Vector3 position = GeneratePositionNearTileCentre(tile);
            DefenderUnit defender = new DefenderUnit(go, position, settings, _bulletsController);
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
                    if (unit.IsInBarrack == false)
                    {
                        unit.GoToBarrack(buildingPosition);
                    }
                }
            }
        }

        public void SendDefenderToBarrack(DefenderUnit unit, TileView tile)
        {
            if (unit.IsInBarrack == false)
            {
                Vector3 buildingPosition = tile.transform.position;
                unit.GoToBarrack(buildingPosition);
            }
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
            unit.ExitFromBarrack();
            SendDefenderToTile(unit, tile);
        }

        public void DismissDefender(DefenderUnit unit)
        {
            DefenderDead(unit);
        }

        public void SendDefenderToTile(DefenderUnit unit, TileView tile)
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