using BrewSystem.Configs;
using BrewSystem.Model;
using System;
using System.Collections.Generic;
using Tools.DragAndDrop;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BrewSystem.UI
{
    public class BrewSystemUIController : IDisposable
    {
        private readonly BrewSystemUI _view;
        private readonly DragAndDropController _tankDragDropController;

        private List<IngridientUI> _ingridientsViewCollection = new();

        public Action OnCheckBrewResult;
        public Action<int> OnIngridientClicked;

        public BrewSystemUIController(
            Canvas canvas,
            BrewSystemUIFactory factory, 
            BrewConfig config,
            List<IngridientModel> ingridients)
        {
            _view = factory.GetView(factory.UIElementsConfig.BrewSystemUI);

            _view.InitUI(config);

            _view.CheckBrewResultButton.onClick.AddListener(CheckBrewResultPressed);

            _tankDragDropController =new DragAndDropController(canvas, _view.BrewTank);

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

                _ingridientsViewCollection.Add(ingridientView);

                _tankDragDropController.AddDraggable(ingridientView);
            }
        }

        private void OnClickedIngridient(int ingridientId)
        {
            OnIngridientClicked?.Invoke(ingridientId);
        }

        public void ChangeIngridientSelection(int ingridientId, bool selectionState)
        {
            var ingridientUI = _ingridientsViewCollection.Find(ing => ing.Id ==  ingridientId);
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

        public void ResetBrewResult()
        {
            _view.BrewResult.ResetUI();
            _view.BrewTank.ResetUI();
        }

        #region IDisposable

        private bool _disposed;

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            _view.CheckBrewResultButton.onClick.RemoveListener(CheckBrewResultPressed);

            foreach (var ingridientView in _ingridientsViewCollection)
            {
                ingridientView.OnClicked -= OnIngridientClicked; 

                ingridientView.Dispose();

                _tankDragDropController.RemoveDraggable(ingridientView);

                Object.Destroy(ingridientView);
            }

            _ingridientsViewCollection.Clear();
        }

        #endregion
    }
}
