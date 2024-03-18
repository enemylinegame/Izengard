using BrewSystem.Configs;
using BrewSystem.Model;
using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace BrewSystem.UI
{
    public class BrewSystemUIController : IDisposable
    {
        private readonly BrewSystemUI _view;

        private List<IngridientUI> _ingridientsCollection = new();

        public Action OnCheckBrewResult;
        public Action<int> OnIngridientSelected;
        public Action<int> OnIngridientUnselected;

        public BrewSystemUIController(
            BrewSystemUIFactory factory, 
            BrewConfig config,
            List<IngridientModel> ingridients)
        {
            _view = factory.GetView(factory.UIElementsConfig.BrewSystemUI);

            _view.InitUI(config);

            _view.CheckBrewResultButton.onClick.AddListener(CheckBrewResultPressed);

            FillIngridients(ingridients);
        }

        private void CheckBrewResultPressed()
        {
            OnCheckBrewResult?.Invoke();
        }

        private void FillIngridients(List<IngridientModel> ingridients)
        {
            for (int i = 0; i < ingridients.Count; i++)
            {
                var ingridient = ingridients[i];

                var ingridientView = Object.Instantiate(_view.IngridientPrefab, _view.IngridientsHolder)
                    .GetComponent<IngridientUI>();

                ingridientView.InitUI(ingridient);

                ingridientView.OnSelected += SelectIngridient;
                ingridientView.OnUnSelected += UnselectIngridient;

                _ingridientsCollection.Add(ingridientView);
            }
        }

        private void SelectIngridient(int ingridientId)
        {
            OnIngridientSelected?.Invoke(ingridientId);
        }

        private void UnselectIngridient(int ingridientId)
        {
            OnIngridientUnselected?.Invoke(ingridientId);
        }

        public void UpdateBrewStatus(BrewModel brewModel)
        {
            _view.ChangeBrewStatus(brewModel.ABV, brewModel.Taste, brewModel.Flavor);
        }

        public void DisplayBrewResult(BrewResultType brewResult)
        {
            _view.ShowBrewStartRaiting(brewResult);
        }

        #region IDisposable

        private bool _disposed;

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            _view.CheckBrewResultButton.onClick.RemoveListener(CheckBrewResultPressed);

            foreach (var ingridient in _ingridientsCollection)
            {
                ingridient.OnSelected -= SelectIngridient; 
                ingridient.OnUnSelected -= UnselectIngridient;

                ingridient.Dispose();

                Object.Destroy(ingridient);
            }

            _ingridientsCollection.Clear();
        }

        #endregion
    }
}
