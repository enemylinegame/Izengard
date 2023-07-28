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

        public DefenderUnit CreateDefender(TileModel tile, DefenderSettings settings)
        {
            GameObject go = GameObject.Instantiate(settings.Prefab,_unitsSpawnPosition, Quaternion.identity, 
                _defendersRoot);
            Vector3 position = GeneratePositionNearTileCentre(tile.TilePosition);
            DefenderUnit defender = new DefenderUnit(go, position, settings, _bulletsController);
            defender.DefenderUnitDead += DefenderDead;
            _defenderUnits.Add(defender);
            return defender;
        }

        private Vector3 GeneratePositionNearTileCentre(Vector3 tilePosition)
        {
            Vector3 position = UnityEngine.Random.insideUnitSphere * POSITION_RADIUS;
            position.y = 0f;
            return position + tilePosition;
        }

        public void SendDefendersToBarrack(List<DefenderUnit> defenderUnits, TileModel tile)
        {
            if (defenderUnits.Count > 0)
            {
                for (int i = 0; i < defenderUnits.Count; i++)
                {
                    DefenderUnit unit = defenderUnits[i];
                    if (unit.IsInBarrack == false)
                    {
                        unit.GoToBarrack(tile.TilePosition);
                    }
                }
            }
        }

        public void SendDefenderToBarrack(DefenderUnit unit, TileModel tile)
        {
            if (unit.IsInBarrack == false)
            {
                unit.GoToBarrack(tile.TilePosition);
            }
        }

        public void KickDefendersOutOfBarrack(List<DefenderUnit> defenderUnits, TileModel tile)
        {
            for (int i = 0; i < defenderUnits.Count; i++)
            {
                DefenderUnit unit = defenderUnits[i];
                KickDefenderOutOfBarrack(unit, tile);
            }
        }

        public void KickDefenderOutOfBarrack(DefenderUnit unit, TileModel tile)
        {
            unit.ExitFromBarrack();
            SendDefenderToTile(unit, tile);
        }

        public void SendDefenderToTile(DefenderUnit unit, TileModel tile)
        {
            Vector3 position = GeneratePositionNearTileCentre(tile.TilePosition);
            unit.GoToPosition(position);
        }

        public void DismissDefender(DefenderUnit unit)
        {
            unit.DefenderUnitDead -= DefenderDead;
            _defenderUnits.Remove(unit);
            unit.Dismiss();
        }

        private void DefenderDead(DefenderUnit defender)
        {
            defender.DefenderUnitDead -= DefenderDead;
            _defenderUnits.Remove(defender);
            defender.DestroyItself();
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