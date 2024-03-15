using UnityEngine;

namespace BrewSystem
{
    internal interface IIngridienData
    {
        public string Name { get; }
        public string Description { get; }
        Sprite Icon { get; }
        public float ABV { get; }
        public float Taste { get; }
        public float Flavor { get; }
    }
}
