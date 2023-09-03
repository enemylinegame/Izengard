using System.Collections.Generic;
using UnityEngine;
using Code.TileSystem;
using CombatSystem.Interfaces;


namespace CombatSystem
{
    public class EnemyTileDislocation
    {
        private readonly OnTriggerDetector _triggerDetector;
        private IDamageable _damageable;

        private List<TileModel> _contactedTiles = new List<TileModel>();
        private TileModel _currentTile;


        private bool _isEnabled;

        
        public EnemyTileDislocation(Damageable damageable)
        {
            _damageable = damageable;
            _triggerDetector = damageable.gameObject.GetComponentInChildren<OnTriggerDetector>();
            _triggerDetector.OnTriggerContactEnter += EnterToTile;
            _triggerDetector.OnTriggerContactExit += ExitFromTile;
        }


        private void EnterToTile(GameObject tileGameObject)
        {
            if (!_isEnabled) return;
            TileView tileView = tileGameObject.GetComponent<TileView>();
            if (tileView)
            {
                // TileModel tile = tileView.TileModel;
                // _contactedTiles.Add(tile);
                // if (_currentTile == null)
                // {
                //     _currentTile = tile;
                //     _currentTile.EnemiesInTile.Add(_damageable);
                // }
            }
        }

        private void ExitFromTile(GameObject tileGameObject)
        {
            if (!_isEnabled) return;
            TileView tileView = tileGameObject.GetComponent<TileView>();
            if (tileView)
            {
                /*TileModel tile = tileGameObject.GetComponent<TileView>().TileModel;
                _contactedTiles.Remove(tile);
                if (_currentTile == tile)
                {
                    _currentTile.EnemiesInTile.Remove(_damageable);
                    _currentTile = null;
                    if (_contactedTiles.Count > 0)
                    {
                        _currentTile = _contactedTiles[0];
                        _currentTile.EnemiesInTile.Add(_damageable);
                    }
                }*/
            }
        }

        public void On()
        {
            _isEnabled = true;
        }

        public void Off()
        {
            _isEnabled = false;
            if (_currentTile != null)
            {
                _currentTile.EnemiesInTile.Remove(_damageable);
                _currentTile = null;
            }
            _contactedTiles.Clear();
        }

    }
}