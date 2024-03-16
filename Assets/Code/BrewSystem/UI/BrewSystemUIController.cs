using BrewSystem.Configs;
using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace BrewSystem.UI
{
    public class BrewSystemUIController : IDisposable
    {
        private readonly BrewSystemUI _view;

        private List<IngridientUI> _ingridientsCollection = new();

        public BrewSystemUIController(
            BrewSystemUIFactory factory, 
            List<IngridientData> ingridients)
        {
            _view = factory.GetView(factory.UIElementsConfig.BrewSystemUI);

            FillIngridients(ingridients);
        }

        private void FillIngridients(List<IngridientData> ingridients)
        {
            for (int i = 0; i < ingridients.Count; i++)
            {
                var ingridient = Object.Instantiate(_view.IngridientPrefab, _view.IngridientsHolder).GetComponent<IngridientUI>();

                ingridient.InitUI(ingridients[i]);

                _ingridientsCollection.Add(ingridient);
            }
        }

        #region IDisposable

        private bool _disposed;

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            _ingridientsCollection.Clear();
        }

        #endregion
    }
}
