﻿using UnityEngine;

namespace BrewSystem.Configs
{
    [CreateAssetMenu(fileName = nameof(BrewConfig), menuName = "BrewSystem/" + nameof(BrewConfig))]
    public class BrewConfig : ScriptableObject
    {
        [field: SerializeField] public int MaxBrewIngridients { get; private set; } = 5;
        
        [field: Header("ABV Range")]
        [field: SerializeField] public float MinABVValue { get; private set; } = -100.0f;
        [field: SerializeField] public float MaxABVValue { get; private set; } = 100.0f;
        [field: Header("ABV Range")]
        [field: SerializeField] public float MinTasteValue { get; private set; } = -100.0f;
        [field: SerializeField] public float MaxTasteValue { get; private set; } = 100.0f;
        [field: Header("ABV Range")]
        [field: SerializeField] public float MinFlavorValue { get; private set; } = -100.0f;
        [field: SerializeField] public float MaxFlavorValue { get; private set; } = 100.0f;

        [field:SerializeField] public IngridientsDataConfig IngridientsData { get; set; }
    }
}
