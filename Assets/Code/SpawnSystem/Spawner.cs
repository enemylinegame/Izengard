using Abstraction;
using NewBuildingSystem;
using System;
using Tools;
using UnityEngine;

namespace SpawnSystem
{
    public class Spawner : BaseGameObject, ITarget, ISelectedObject
    {
        [Header("Info")]
        [SerializeField]
        private string _id;
        [SerializeField]
        private string _name;
        private Vector2Int _sise;
        [SerializeField]
        private Transform _spawnlocation;

        [Header("Colliders")]
        public BoxCollider Collider;

        [Space, Header("Components")]
        public Transform PositionBuild;
        public GameObject ObjectBuild;

        public string Id => _id;
        public string Name => _name;
        public Vector2Int Size => _sise;

        public Transform SpawnLocation => _spawnlocation;
        public Vector3 Position => transform.position;

        public FactionType FactionType { get; private set; }

        public void Init(string buildingId, string name)
        {
            _id = buildingId;
            _name = name;
            _sise = new Vector2Int(1, 1);

            _selectionEffect = GetComponent<OnSelectionEffect>();
        }

        public void SetFaction(FactionType factionType)
        {
            FactionType = factionType;
        }

        #region ISelectedObject

        private OnSelectionEffect _selectionEffect;

        public void Select()
        {
            if (_selectionEffect == null)
                return;

            _selectionEffect.Display();
        }

        public void Unselect()
        {
            if (_selectionEffect == null)
                return;

            _selectionEffect.Hide();
        }

        #endregion
    }
}
