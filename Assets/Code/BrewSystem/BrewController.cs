using BrewSystem.Configs;
using BrewSystem.Model;
using BrewSystem.UI;
using System;
using System.Collections.Generic;

namespace BrewSystem
{
    public class BrewController : IOnController, IDisposable
    {
        private readonly BrewModel _model;

        private readonly BrewSystemUIController _viewController;
        private readonly List<IngridientModel> _ingridientsCollection;

        private List<IngridientModel> _brewMixCollection;

        public BrewController(
            BrewSystemUIFactory uiFactory,
            BrewConfig config)
        {
            _ingridientsCollection = new();
            _brewMixCollection = new();

            _model = new BrewModel();

            var ingridients = config.IngridientsData.Ingridients;

            for (int i = 0; i < ingridients.Count; i++)
            {
                _ingridientsCollection.Add(new IngridientModel(i, ingridients[i]));
            }

            _viewController = new BrewSystemUIController(uiFactory, config, _ingridientsCollection);

            Subscribe();

            _viewController.UpdateBrewStatus(_model);
        }

        private void Subscribe()
        {
            _viewController.OnCheckBrewResult += CheckBrewResult;
            _viewController.OnIngridientSelected += IngridientSelected;
            _viewController.OnIngridientUnselected += IngridientUnselected;
        }

        private void Unsubscribe()
        {
            _viewController.OnCheckBrewResult -= CheckBrewResult;
            _viewController.OnIngridientSelected -= IngridientSelected;
            _viewController.OnIngridientUnselected -= IngridientUnselected;
        }

        private void CheckBrewResult()
        {
            
        }

        private void IngridientSelected(int ingridientId)
        {
            var ingridient = _ingridientsCollection.Find(ing => ing.Id == ingridientId);
            _brewMixCollection.Add(ingridient);

            CalculateBrew(_brewMixCollection);
        }

        private void IngridientUnselected(int ingridientId)
        {
            var ingridient = _ingridientsCollection.Find(ing => ing.Id == ingridientId);
            _brewMixCollection.Remove(ingridient);

            CalculateBrew(_brewMixCollection);
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
