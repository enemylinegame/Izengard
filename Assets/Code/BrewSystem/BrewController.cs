using BrewSystem.Configs;
using BrewSystem.Model;
using BrewSystem.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BrewSystem
{
    public class BrewController : IOnController, IDisposable
    {
        private readonly BrewConfig _config;
        private readonly BrewModel _model;

        private readonly BrewSystemUIController _viewController;
        private readonly List<IngridientModel> _ingridientsCollection;

        private List<IngridientModel> _brewMixCollection;

        public BrewController(
            Canvas mainCanvas,
            BrewSystemUIFactory uiFactory,
            BrewConfig config)
        {
            _ingridientsCollection = new();
            _brewMixCollection = new();

            _config = config;
            _model = new BrewModel();

            var ingridients = config.IngridientsData.Ingridients;

            for (int i = 0; i < ingridients.Count; i++)
            {
                _ingridientsCollection.Add(new IngridientModel(i, ingridients[i]));
            }

            _viewController = new BrewSystemUIController(mainCanvas, uiFactory, config, _ingridientsCollection);

            Subscribe();

            _viewController.UpdateBrewStatus(_model);
        }

        private void Subscribe()
        {
            _viewController.OnCheckBrewResult += CheckBrewResult;
            _viewController.OnIngridientAdded += UpdateIngridientsInBrew;
        }

        private void Unsubscribe()
        {
            _viewController.OnCheckBrewResult -= CheckBrewResult;
            _viewController.OnIngridientAdded -= UpdateIngridientsInBrew;
        }

        private void CheckBrewResult()
        {
            var brewCalculation = 100.0f - GetBrewAverage(_config, _model);
            var brewResult = GetBrewResult(brewCalculation);

            _brewMixCollection.Clear();

            _model.Reset();
            _viewController.UpdateBrewStatus(_model);

            _viewController.DisplayBrewResult(brewResult);
        }

        private float GetBrewAverage(BrewConfig config, BrewModel model)
        {
            var result = 
                ( MathF.Abs(config.ABVIdealValue - model.ABV)
                + MathF.Abs(config.TasteIdealValue - model.Taste)
                + MathF.Abs(config.TasteIdealValue - model.Taste))
                / 3.0f;

            return result;
        }

        private BrewResultType GetBrewResult(float value)
        {
            if (value >= 100.0f)
                return BrewResultType.Ideal;

            var result = BrewResultType.Lost;

            if (IsInRange(value, 50.0f, 70.0f))
            {
                result = BrewResultType.Low;
            }
            else if(IsInRange(value, 70.0f, 90.0f))
            {
                result = BrewResultType.Normal;
            }
            else if(IsInRange(value, 90.0f, 100.0f))
            {
                result = BrewResultType.Ideal;
            }

            return result;
        }

        private bool IsInRange(float value, float min, float max)
        {
            return (value >= min) && (value < max);
        }

        private void UpdateIngridientsInBrew(int ingridientId)
        {
            var ingridient = _ingridientsCollection.Find(ing => ing.Id == ingridientId);

            AddIngridientToBrew(ingridient);
        }

        private void AddIngridientToBrew(IngridientModel ingridient)
        {
            if (_brewMixCollection.Count >= _config.MaxBrewIngridients)
                return;

            _brewMixCollection.Add(ingridient);

            CalculateBrew(_brewMixCollection);

            UpdateViewControllerData(ingridient.Id, true);
        }

        private void RemoveIngridientFromBrew(IngridientModel ingridient)
        {
            _brewMixCollection.Remove(ingridient);

            CalculateBrew(_brewMixCollection);

            UpdateViewControllerData(ingridient.Id, false);
        }

        private void UpdateViewControllerData(int ingridetnId, bool selectionState)
        {
            _viewController.UpdateIngridientsInBrewCount(_brewMixCollection.Count);
            _viewController.ChangeIngridientSelection(ingridetnId);
            _viewController.ResetBrewResult();
        }

        private void CalculateBrew(List<IngridientModel> brewIngridients)
        {
            _model.Reset();

            if (brewIngridients.Count != 0)
            {
                for (int i = 0; i < brewIngridients.Count; i++)
                {
                    var brewIngridient = brewIngridients[i];

                    _model.ABV += brewIngridient.Data.ABV;
                    _model.Taste += brewIngridient.Data.Taste;
                    _model.Flavor += brewIngridient.Data.Flavor;
                }
            }

            _viewController.UpdateBrewStatus(_model);
        }

        #region IDisposable

        private bool _disposed;

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            Unsubscribe();
            
            _viewController.Dispose();
        }

        #endregion
    }
}
