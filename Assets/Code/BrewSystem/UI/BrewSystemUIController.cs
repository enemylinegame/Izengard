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
        public Action<int> OnIngridientClicked;

        public BrewSystemUIController(
            BrewSystemUIFactory factory, 
            BrewConfig config,
            List<IngridientModel> ingridients)
        {
            _view = factory.GetView(factory.UIElementsConfig.BrewSystemUI);

            _view.InitUI(config);

            _view.CheckBrewResultButton.onClick.AddListener(CheckBrewResultPressed);

            FillIngridients(ingridients);

            UpdateIngridientsInBrewCount(0);
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

                ingridientView.OnClicked += OnClickedIngridient;

                _ingridientsCollection.Add(ingridientView);
            }
        }

        private void OnClickedIngridient(int ingridientId)
        {
            OnIngridientClicked?.Invoke(ingridientId);
        }

        public void ChangeIngridientSelection(int ingridientId, bool selectionState)
        {
            var ingridientUI = _ingridientsCollection.Find(ing => ing.Id ==  ingridientId);
            ingridientUI.ChangeSelection(selectionState);
        }

        public void UpdateIngridientsInBrewCount(int value)
        {
            _view.IngridientsCount.text = $"Ingridients in Brew : {value}"; 
        }

        public void UpdateBrewStatus(BrewModel brewModel)
        {
            _view.BrewStatus.ChangeStatus(brewModel.ABV, brewModel.Taste, brewModel.Flavor);
        }

        public void DisplayBrewResult(BrewResultType brewResult)
        {
            _view.BrewResult.ShowResult(brewResult);
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
                ingridient.OnClicked -= OnIngridientClicked; 

                ingridient.Dispose();

                Object.Destroy(ingridient);
            }

            _ingridientsCollection.Clear();
        }

        #endregion
    }
}
