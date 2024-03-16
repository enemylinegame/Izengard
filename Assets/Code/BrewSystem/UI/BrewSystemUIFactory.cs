using UI;
using UnityEngine;

namespace BrewSystem.UI
{
    public class BrewSystemUIFactory : UIViewFactory<BrewSystemUI>
    {
        public BrewSystemUIFactory(UIElementsConfig uIElementsConfig, Canvas mainCanvas) : base(uIElementsConfig, mainCanvas) { }
    }
}
