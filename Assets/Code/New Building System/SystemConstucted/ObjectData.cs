using System;
using UnityEngine;
using UnityEngine.UI;

namespace NewBuildingSystem
{
    [Serializable]
    public class ObjectData
    {
        [field: SerializeField] public int ID { get; private set; }
        [field: Space]
        [field: SerializeField] public Sprite Image { get; private set; }
        [field: SerializeField] public EnumBuildings BuildingType { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int MaxWorkers { get; private set; }
        [field: SerializeField] public Vector2Int Size { get; private set; } = Vector2Int.one;
        [field: SerializeField] public GameObject Prefab { get; private set; }
    }
}