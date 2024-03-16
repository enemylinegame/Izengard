using BrewSystem.Configs;
using BrewSystem.UI;
using System;

namespace BrewSystem
{
    public class BrewController : IOnController, IDisposable
    {
        private readonly BrewSystemUIController _viewController;

        public BrewController(
            BrewSystemUIFactory uiFactory, 
            IngridientsDataConfig config)
        {
            _viewController = new BrewSystemUIController(uiFactory, config.Ingridients);

        }

        #region IDisposable

        private bool _disposed;

        public void Dispose()
        {
            if (_disposed) 
                return;
            
            _disposed = true;

            _viewController.Dispose();
        }

        #endregion
    }
}
