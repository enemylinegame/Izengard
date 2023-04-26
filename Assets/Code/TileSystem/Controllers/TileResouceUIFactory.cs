using UnityEngine;
using UnityEngine.UI;

namespace Code.TileSystem
{
    public class TileResouceUIFactory
    {
        private RectTransform _layoutTransform;
        private ResourcesLayoutUIView _resourcesLayoutUIView;
        TileResourceUIController _tileResourceController;

        public TileResouceUIFactory(ResourcesLayoutUIView layoutView, TileResourceUIController tileResourceController)
        {
            _layoutTransform = layoutView.LayoutRectTransform;
            _resourcesLayoutUIView = layoutView;
            _tileResourceController = tileResourceController;

            _resourcesLayoutUIView.TestAdd.onClick.AddListener(() => TestAdd());
        }

        public void CreateResourceUIOnLayout(GameObject resourceUIElement)
        {
            ResourceView resourceUIObjectView;
            GameObject resourceUIObject = GameObject.Instantiate(resourceUIElement, _layoutTransform);
            resourceUIObject.TryGetComponent<ResourceView>(out resourceUIObjectView);
            _resourcesLayoutUIView.Resources.Add(resourceUIObjectView);
            _tileResourceController.AddNewLayoutElement(resourceUIObjectView);
        }
        private void TestAdd()
        {
            GameObject resourceUIElement = Resources.Load<GameObject>("UI/ResourceUI/Res");
            CreateResourceUIOnLayout(resourceUIElement);
        }
    }
}
