using BrewSystem.Configs;
using BrewSystem.UI;
using System;

namespace BrewSystem
{
    internal class BrewController : IOnController, IDisposable
    {
        private readonly BrewSystemUI _view;

        public BrewController(BrewSystemUI view, IngridientsDataConfig config)
        {
            _view = view;

            _view.InitUI(config.Ingridients);
        }

        #region IDisposable

        private bool _disposed;

        public void Dispose()
        {
            if (_disposed) 
                return;
            
            _disposed = true;

            _view.Dispose();
        }

        #endregion
    }
}
