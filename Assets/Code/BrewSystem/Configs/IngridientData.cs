using System;
using UnityEngine;

namespace BrewSystem.Configs
{
    [Serializable]
    public class IngridientData : IIngridienData
    {
        [SerializeField]
        private string _name;
        [SerializeField]
        private string _description;
        [SerializeField]
        private Sprite _icon;
        [SerializeField]
        private float _abv = 5.0f;
        [SerializeField]
        private float _taste = 1.0f;
        [SerializeField]
        private float _flavor = 1.0f;

        public string Name => _name;
        public string Description => _description;
        public Sprite Icon => _icon;
        public float ABV => _abv;
        public float Taste => _taste;
        public float Flavor => _flavor;
    }
}
