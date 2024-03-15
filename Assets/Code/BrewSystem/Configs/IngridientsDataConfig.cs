using System.Collections.Generic;
using UnityEngine;

namespace BrewSystem.Configs
{
    [CreateAssetMenu(fileName = nameof(IngridientsDataConfig), menuName = "BrewSystem/" + nameof(IngridientsDataConfig))]
    internal class IngridientsDataConfig : ScriptableObject
    {
        [field: SerializeField] public List<IngridientData> Ingridients;
    }
}
