using BrewSystem.Configs;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BrewSystem.UI
{
    internal class BrewSystemUI : MonoBehaviour, IDisposable
    {
        [SerializeField]
        private Transform _ingridientsHolder;
        [SerializeField]
        private GameObject _ingridientPrefab;

        private List<IngridientUI> _ingridientsCollection = new();

        public void InitUI(List<IngridientData> ingridients)
        {
            FillIngridients(ingridients);
        }

        private void FillIngridients(List<IngridientData> ingridients)
        {
            for(int i = 0; i < ingridients.Count; i++)
            {
                var ingridient = Instantiate(_ingridientPrefab, _ingridientsHolder).GetComponent<IngridientUI>();
                
                ingridient.InitUI(ingridients[i]);

                _ingridientsCollection.Add(ingridient);
            }
        }

        public void Dispose()
        {
            _ingridientsCollection.Clear();
        }
    }
}
